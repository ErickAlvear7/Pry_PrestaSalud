using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pry_PrestasaludWAP.Modelo
{
    public class ModelSms
    {

        public class SMS
        {
            public string user { get; set; }
            public string pass { get; set; }
            public int mensajeid { get; set; }
            public string campana { get; set; }
            public string telefono { get; set; }
            public int tipo { get; set; }
            public int ruta { get; set; }
            public string datos { get; set; }
        }
    }
}