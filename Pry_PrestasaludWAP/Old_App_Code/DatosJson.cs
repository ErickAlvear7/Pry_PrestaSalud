namespace Pry_PrestasaludWAP.Old_App_Code
{
    public class DatosJson
    {
        public class Titular
        {
            public string ti_tipoident { get; set; }
            public string ti_cedula { get; set; }
            public string ti_nombre { get; set; }
            public string ti_primernom { get; set; }
            public string ti_segundonom { get; set; }
            public string ti_primerape { get; set; }
            public string ti_segundoape { get; set; }
            public string ti_mail { get; set; }
            public string ti_telefonos { get; set; }
            public string ti_fechanaci { get; set; }
            public string ti_estado { get; set; }
            public string ti_genero { get; set; }
            public string ti_contrato { get; set; }
            public string ti_fechavigencia { get; set; }
            public string ti_codplan { get; set; }
        }
        public class Beneficiario
        {
            public string de_tipoident { get; set; }
            public string de_cedula { get; set; }
            public string de_nombre { get; set; }
            public string de_primernom { get; set; }
            public string de_segundonom { get; set; }
            public string de_primerape { get; set; }
            public string de_segundoape { get; set; }
            public string de_mail { get; set; }
            public string de_telefonos { get; set; }
            public string de_tipo { get; set; }
            public string de_autcodigo { get; set; }
            public string de_fechanaci { get; set; }
            public string de_genero { get; set; }
        }
    }
}