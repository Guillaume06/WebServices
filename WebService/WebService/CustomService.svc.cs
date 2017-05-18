using System;
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



namespace WebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" dans le code, le fichier svc et le fichier de configuration.
    // REMARQUE : pour lancer le client test WCF afin de tester ce service, sélectionnez Service1.svc ou Service1.svc.cs dans l'Explorateur de solutions et démarrez le débogage.
    public class Service1 : ICustomService{ 

        public string submitPathData(PathData data){

            WebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/directions/xml?origin=" + data.Departure + "&destination=" + data.Arrival + "&key=AIzaSyBWOayAMh6TKPXkcEvcwDQI1iKyYl0_8Ow");

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream); // Read the content.
            string responseFromServer = reader.ReadToEnd(); // Put it in a String 

            parseResult(responseFromServer);
            return responseFromServer;

        }
        static void parseResult(String s){
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("route")[0];
            Console.WriteLine(elem.Attributes["summary"]);

        }

    }
}
