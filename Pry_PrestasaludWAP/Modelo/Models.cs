using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pry_PrestasaludWAP.Modelo
{
    public class Models
    {

        public class ApiKey
        {
            public string api_key { get; set; }
        }

        public class Patient
        {

            public string name { get; set; }
            public string surnames { get; set; }
            public string email { get; set; }
            public string birthdate { get; set; }
            public string gender { get; set; }
            public string phone { get; set; }
            //public int role { get; set; }
            //public bool oneclick { get; set; }
            public string contractId { get; set; }
            //public string customId { get; set; }
            //public int cp { get; set; }
            //public string city { get; set; }
            //public bool TyC_aceptados { get; set; }

        }

        public class Consulta
        {
            public string idPatient { get; set; }
            public string idContrato { get; set; }
            public string idEspecialidad { get; set; }
            public string idServicio { get; set; }
            //public string date { get; set; }
            //public string hour { get; set; }
            //public string timeZone { get; set; }
            public string reason { get; set; }
            //public string idMedico { get; set; }
            public string customId { get; set; }
            //public bool oneclick { get; set; }

        }


    }
}