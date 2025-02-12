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

        public class DatoObj
        {
            public List<Dato> datos { get; set; }
        
        }

        public class Dato
        {
            public string codCiudad { get; set; }
            public string distritoProvincia { get; set; }
            public string nombreCiudad { get; set; }
        }

        public class EspeObj
        {
            public List<Especialidad> datos { get; set; }

        }


        public class Especialidad
        {
            public string codEspecialidad { get; set; }
            public string codSucursal { get; set; }
            public string espNombre { get; set; }
        }

    }
}