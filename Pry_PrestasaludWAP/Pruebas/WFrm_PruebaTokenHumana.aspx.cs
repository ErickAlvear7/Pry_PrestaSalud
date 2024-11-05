using Newtonsoft.Json;
using Pry_PrestasaludWAP.Old_App_Code;
using System;
using System.Web.UI;

namespace Pry_PrestasaludWAP.Pruebas
{
    public partial class WFrm_PruebaTokenHumana : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //protected void BtnToken_Click(object sender, EventArgs e)
        //{
        //    string token = new Datos_Humana().GetToken("http://almacen.humana.med.ec/WSOAUTH2/token", "usrprestasalud", "_.oMDBM263");

        //    //var mitoken = token;

        //    string datositems = new Datos_Humana().GetItems("http://almacen.humana.med.ec/WSCobis/api/ServicioExterno/Consulta_Datos_Titular",
        //        token, "1721488334", "1001722");

        //    dynamic strjson = JsonConvert.DeserializeObject(datositems);

        //    DatosJson.Titular dttitular = new DatosJson.Titular();
        //    DatosJson.Beneficiario dtbeneficiario = new DatosJson.Beneficiario();

        //    var titular = strjson.rec_titular;

        //    foreach (var titu in titular)
        //    {
        //        dttitular.ti_tipoident = titu.ti_tipoident;
        //        dttitular.ti_cedula = titu.ti_cedula;
        //        dttitular.ti_nombre = titu.ti_nombre;

        //        var beneficiario = titu.rec_dependiente;

        //        if (beneficiario != null)
        //        {
        //            foreach (var vbene in beneficiario)
        //            {
        //                dtbeneficiario.de_tipoident = vbene.de_tipoident;
        //                dtbeneficiario.de_cedula = vbene.de_cedula;
        //                dtbeneficiario.de_nombre = vbene.de_nombre;
        //            }
        //        }

        //    }
        //}
    }
}