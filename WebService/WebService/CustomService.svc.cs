﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;



namespace WebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : ICustomService{

        private static XmlDocument velibResponse;
        private static double departureLat;
        private static double departureLng;
        private static double arrivalToStationDistance = 99999999;
        private static double departureToStationDistance = 99999999;
        private static double arrivalLat;
        private static double arrivalLng;
        private static double stationLat;
        private static double stationLng;
        private static string closestStationFromDeparture;
        private static string closestStationFromArrival;



        public string submitPathData(PathData data){

            WebRequest requestDeparture = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/xml?address=" + data.Departure + "&key=AIzaSyBWOayAMh6TKPXkcEvcwDQI1iKyYl0_8Ow");
            WebResponse responseDeparture = requestDeparture.GetResponse();
            Stream dataStreamDeparture = responseDeparture.GetResponseStream();
            StreamReader readerDeparture = new StreamReader(dataStreamDeparture); // Read the content.
            string responseFromServerDeparture = readerDeparture.ReadToEnd(); // Put it in a String 
            parseDepartureResult(responseFromServerDeparture);

            WebRequest requestArrival = WebRequest.Create("https://maps.googleapis.com/maps/api/geocode/xml?address=" + data.Arrival + "&key=AIzaSyBWOayAMh6TKPXkcEvcwDQI1iKyYl0_8Ow");
            WebResponse responseArrival = requestArrival.GetResponse();
            Stream dataStreamArrival = responseArrival.GetResponseStream();
            StreamReader readerArrival = new StreamReader(dataStreamArrival); // Read the content.
            string responseFromServerArrival = readerArrival.ReadToEnd(); // Put it in a String 
            parseArrivalResult(responseFromServerArrival);

            getAllStations();
            getClosestStationFromDeparture();
            getClosestStationFromArrival();

            return closestStationFromDeparture + " " + closestStationFromArrival;

        }

        static void getClosestStationFromDeparture()
        {
            XmlNodeList elemList = velibResponse.GetElementsByTagName("marker");
            for (int i = 0; i < elemList.Count; i++)
            {
                /*
                //Get the number of the Station 
                String numPoint = elemList[i].Attributes["number"].Value;

                // Create a request for the URL.
                WebRequest request_for_data = WebRequest.Create("http://www.velib.paris/service/stationdetails/" + numPoint);
                // Get Response 
                WebResponse response_for_data = request_for_data.GetResponse();
                // Open the stream using a StreamReader for easy access and put it into a string
                Stream dataStream_for_data = response_for_data.GetResponseStream();
                StreamReader reader_for_data = new StreamReader(dataStream_for_data); // Read the content.
                string responseFromServer_for_data = reader_for_data.ReadToEnd(); // Put it in a String
                // Parse the response and put the entries in XmlNodeList 
                XmlDocument doc_for_data = new XmlDocument();
                doc_for_data.LoadXml(responseFromServer_for_data);
                XmlNodeList elemList_for_data = doc_for_data.GetElementsByTagName("available");

                if(int.Parse(elemList_for_data[0].FirstChild.Value) != 0)
                {
                */
                    shortestDistance(elemList[i].Attributes["address"].Value, double.Parse(elemList[i].Attributes["lat"].Value, CultureInfo.InvariantCulture), double.Parse(elemList[i].Attributes["lng"].Value, CultureInfo.InvariantCulture));
            }
        }

        static void getClosestStationFromArrival()
        {
            XmlNodeList elemList = velibResponse.GetElementsByTagName("marker");
            for (int i = 0; i < elemList.Count; i++)
            {
                shortestDistance(elemList[i].Attributes["address"].Value, double.Parse(elemList[i].Attributes["lat"].Value, CultureInfo.InvariantCulture), double.Parse(elemList[i].Attributes["lng"].Value, CultureInfo.InvariantCulture));
            }
        }

        static bool shortestDistance(string address, double lat, double lng)
        {
            double distance = Distance(departureLat, departureLng, lat, lng);
            if(distance < departureToStationDistance)
            {
                departureToStationDistance = distance;
                closestStationFromDeparture = address;
                return true;
            }
            return false;
        }

        static void parseResult(String s)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("route")[0];
            Console.WriteLine(elem.Attributes["route"]);
        }

        static void parseDepartureResult(String s)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("result")[0];
            XmlNodeList list = elem.ChildNodes;
            foreach (XmlNode n in list)
            {
                if (n.Name == "geometry")
                {
                    foreach (XmlNode node in n)
                    {
                        if (node.Name == "location")
                        {
                            XmlNode lat = node.ChildNodes[0];
                            XmlNode lng = node.ChildNodes[1];
                            departureLat = double.Parse(lat.InnerText, CultureInfo.InvariantCulture);
                            departureLng = double.Parse(lng.InnerText, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
        }

        static void parseStationResult(String s)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("result")[0];
            XmlNodeList list = elem.ChildNodes;
            foreach (XmlNode n in list)
            {
                if (n.Name == "geometry")
                {
                    foreach (XmlNode node in n)
                    {
                        if (node.Name == "location")
                        {
                            XmlNode lat = node.ChildNodes[0];
                            XmlNode lng = node.ChildNodes[1];
                            stationLat = double.Parse(lat.InnerText, CultureInfo.InvariantCulture);
                            stationLng = double.Parse(lng.InnerText, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
        }

        static void parseArrivalResult(String s)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("result")[0];
            XmlNodeList list = elem.ChildNodes;
            foreach (XmlNode n in list)
            {
                if (n.Name == "geometry")
                {
                    foreach (XmlNode node in n)
                    {
                        if (node.Name == "location")
                        {
                            XmlNode lat = node.ChildNodes[0];
                            XmlNode lng = node.ChildNodes[1];
                            arrivalLat = double.Parse(lat.InnerText, CultureInfo.InvariantCulture);
                            arrivalLng = double.Parse(lng.InnerText, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
        }

        static double convertRad(double input)
        {
            return (Math.PI * input) / 180;
        }

        static double Distance(double lat_a_degre, double lon_a_degre, double lat_b_degre, double lon_b_degre)
        {
            int R = 6378000; //Rayon de la terre en mètre
            double lat_a = convertRad(lat_a_degre);
            double lon_a = convertRad(lon_a_degre);
            double lat_b = convertRad(lat_b_degre);
            double lon_b = convertRad(lon_b_degre);

            double d = R * (Math.PI / 2 - Math.Asin(Math.Sin(lat_b) * Math.Sin(lat_a) + Math.Cos(lon_b - lon_a) * Math.Cos(lat_b) * Math.Cos(lat_a)));
            return d;
        }

        static void getAllStations()
        {

            WebRequest request = WebRequest.Create("http://www.velib.paris/service/carto");

            // Get Response from the Service 
            WebResponse response = request.GetResponse();

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access and put it into a string
            StreamReader readerVelib = new StreamReader(dataStream); // Read the content.
            string responseFromServer = readerVelib.ReadToEnd(); // Put it in a String 

            // Parse the response and put the entries in XmlNodeList 
            velibResponse = new XmlDocument();
            velibResponse.LoadXml(responseFromServer);

        }
    }
}
