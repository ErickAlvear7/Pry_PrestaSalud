using System;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
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
        string idpro = "";
        string user = "";
        string pass = "";
        string documento = "";
        string response = "";
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            idtitular = Request["CodigoTitular"];
            idbene = Request["CodigoBene"];
            idpro = Request["CodigoPro"];

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
            accessToken = new MediLinkApi().PostAccesLogin("https://testagendamiento.medilink.com.ec:443/", data);

            //GET CEDULA TITULAR
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(idtitular);
            objparam[1] = "";
            objparam[2] = 185;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            documento = dt.Tables[0].Rows[0][0].ToString();
            lblDocumento.Text = documento;




            if (idbene != "")
            {

            }

            bool existe = true;

            if (!IsPostBack)
            {


                if (existe)
                {
                    FunGetCiudad();
                    
                }

            }
            



        }

       

        private void FunGetCiudad()
        {

            response = new MediLinkApi().GetCiudad("https://testagendamiento.medilink.com.ec:443/", accessToken);
            //dynamic dataCiudad = JObject.Parse(response);

            var Resultjson = JsonConvert.DeserializeObject<DatoObj>(response);

            ddlciudad.Items.Clear();
            ListItem i;
            i = new ListItem("--Seleccione Ciudad--", "0");
            ddlciudad.Items.Add(i);
            foreach (var item in Resultjson.datos)
            {
                string codigoCiudad = item.codCiudad.ToString();
                string provincia = item.distritoProvincia;
                string ciudad = item.nombreCiudad;

                i = new ListItem(ciudad, codigoCiudad);

                ddlciudad.Items.Add(i);

            }

        }

        private void FunGetEspe()
        {
            response = new MediLinkApi().GetEspecialidad("https://testagendamiento.medilink.com.ec:443/", accessToken, 1);
            var Resultjson = JsonConvert.DeserializeObject<EspeObj>(response);
            ListItem e;
            e = new ListItem("--Seleccione Especialidad--", "0");
            ddlEspecialidad.Items.Add(e);

            foreach (var espe in Resultjson.datos)
            {
                string codigoEspecialidad = espe.codEspecialidad.ToString();
                string codigoSucursal = espe.codSucursal.ToString();
                string espeNombre = espe.espNombre;

                e = new ListItem(espeNombre, codigoEspecialidad);
                ddlEspecialidad.Items.Add(e);
            }


        }

        protected void ddlciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
        }

        protected void btnConsul_Click(object sender, EventArgs e)
        {
            response = new MediLinkApi().GetVerificarPaciente("https://testagendamiento.medilink.com.ec:443/", accessToken, documento, "C");
            if(response == "SI")
            {

            }

        }
    }
}