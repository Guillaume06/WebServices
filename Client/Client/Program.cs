using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Xml;
using System.Runtime.Serialization;
using Client.ServiceReference1;


namespace Client
{
        class Program
    {
            

        static void Main()
        {
            while (true)
            {
                string departure = "";
                string arrival = "";


                Console.WriteLine("Entrez le nom de l'adresse d'origine à trouver sur GM:");
                departure = Console.ReadLine();
                Console.WriteLine("Entrez le nom de l'adresse de destination à trouver sur GM:");
                arrival = Console.ReadLine();
                PathData data = new PathData();
                data.Arrival = arrival;
                data.Departure = departure;
                CustomServiceClient service = new CustomServiceClient();
                string response = service.submitPathData(data);
                Console.WriteLine(response);

            }
        }
    
    }
}

