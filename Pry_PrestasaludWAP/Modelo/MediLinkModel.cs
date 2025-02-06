using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pry_PrestasaludWAP.Modelo
{
    public class MediLinkModel
    {

        public class LoginApi
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class Ciudad
        {
            //public string[] codCiudad { get; set; }
            //public string[] distritoProvincia { get; set; }
            public string[] nombreCiudad { get; set; }

            public Ciudad(string codCiudad)
            {
                codCiudad = codCiudad;
            //    distritoProvincia = distritoProvincia;
            //    nombreCiudad = nombreCiudad;
            }

            public Ciudad()
            {

            }
        }
    }
}