using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WebService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface ICustomService
    {

        [OperationContract]
        string submitPathData(PathData data);

        // TODO: ajoutez vos opérations de service ici
    }


    // Utilisez un contrat de données comme indiqué dans l'exemple ci-après pour ajouter les types composites aux opérations de service.
    [DataContract]
    public class PathData
    {
        string departure;
        string arrival;

        [DataMember]
        public string Departure
        {
            get { return departure; }
            set { departure = value; }
        }

        [DataMember]
        public string Arrival
        {
            get { return arrival; }
            set { arrival = value; }
        }
    }
}
