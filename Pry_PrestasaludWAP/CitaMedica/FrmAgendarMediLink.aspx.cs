using System;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Pry_PrestasaludWAP.Api;
using static Pry_PrestasaludWAP.Modelo.MediLinkModel;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmAgendarMediLink : System.Web.UI.Page
    {

        string accessToken = "";
        string idtitular = "";
        string idbene = "";
        string user = "";
        string pass = "";
        string documento = "";
        string response = "";
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            idtitular = Request["CodigoTitular"];
            //GET PARAMETROS USUARIO Y PASSWORD MEDILINK
            objparam[0] = 61;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            if(dt != null && dt.Tables[0].Rows.Count > 0)
            {
                user = dt.Tables[0].Rows[0][1].ToString().Trim();
                pass = dt.Tables[0].Rows[1][1].ToString().Trim();
            }

            var login = new LoginApi
           {
               username = user,
               password = pass
            };

            var data = new JavaScriptSerializer().Serialize(login);
            accessToken = new MediLinkApi().PostAccesLogin("https://testagendamiento.medilink.com.ec/", data);

            //GET CEDULA TITULAR
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(idtitular);
            objparam[1] = "";
            objparam[2] = 185;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            documento = dt.Tables[0].Rows[0][0].ToString();
            lblDocumento.Text = documento;
            */


        }
        
        protected void Button1_Click(object sender, EventArgs e)
        {

            //GET PARAMETROS USUARIO Y PASSWORD MEDILINK
            objparam[0] = 61;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                user = dt.Tables[0].Rows[0][1].ToString().Trim();
                pass = dt.Tables[0].Rows[1][1].ToString().Trim();
            }

            var login = new LoginApi
            {
                username = user,
                password = pass
            };

            var data = new JavaScriptSerializer().Serialize(login);
            accessToken = new MediLinkApi().PostAccesLogin("https://testagendamiento.medilink.com.ec/", data);

            //GET CEDULA TITULAR
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ViewState["CodigoTitular"].ToString());
            objparam[1] = "";
            objparam[2] = 185;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            documento = dt.Tables[0].Rows[0][0].ToString();
            lblDocumento.Text = documento;

            //response = new MediLinkApi().GetVerificarPaciente("https://testagendamiento.medilink.com.ec/", accessToken, documento, "C");

            response = new MediLinkApi().GetCiudad("https://testagendamiento.medilink.com.ec/", accessToken);

            dynamic dataCiudad = JObject.Parse(response);

        }
    }
}