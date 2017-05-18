using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


// A Hello World! program in C#.
using System;
namespace ConsoleApp2
{
    class Hello
    {
        static void parseResult(String s) {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(s);
            XmlNode elem = doc.GetElementsByTagName("route")[0];
            Console.WriteLine(elem.Attributes["summary"]);

        }

        static void Main()
        {
            string addressOrigin = "";
            string addressDestination = "";


            Console.WriteLine("Entrez le nom de l'adresse d'origine à trouver sur GM:");
            addressOrigin = Console.ReadLine();
            Console.WriteLine("Entrez le nom de l'adresse de destination à trouver sur GM:");
            addressDestination = Console.ReadLine();


            WebRequest request = WebRequest.Create("https://maps.googleapis.com/maps/api/directions/xml?origin="+addressOrigin+"&destination="+addressDestination+"&key=AIzaSyBWOayAMh6TKPXkcEvcwDQI1iKyYl0_8Ow");

            WebResponse response = request.GetResponse();

            Stream dataStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(dataStream); // Read the content.
            string responseFromServer = reader.ReadToEnd(); // Put it in a String 

            parseResult(responseFromServer);

            //Console.WriteLine(responseFromServer);


            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}