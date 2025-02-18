using System;
using System.Data;
using System.Globalization;
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
        string nombre1 = "";
        string nombre2 = "";
        string apellido1 = "";
        string apellido2 = "";
        string genero = "";
        string fechanac = "";
        string celular = "";
        string email = "";
        string direccion = "";
        int codCiudad = 0;
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                idtitular = Request["CodigoTitular"];
                idbene = Request["CodigoBene"];
                idpro = Request["CodigoPro"];
                //updPaciente.Visible = false;
                //updCombos.Visible = false;

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
                accessToken = new MediLinkApi().PostAccesLogin("https://testagendamiento.medilink.com.ec:443/", data);

                Session["AccessToken"] = accessToken;

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

                response = new MediLinkApi().GetVerificarPaciente("https://testagendamiento.medilink.com.ec:443/", accessToken, documento, "C");

                if (response == "SI")
                {
                    FunGetCiudad();
                    pnlOpciones.Visible = true;
                }
                else if (response == "NO")
                {
                    pnlPaciente.Visible = true;
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 1;
                    objparam[1] = int.Parse(idtitular);
                    objparam[2] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularBene", objparam);

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        nombre1 = dr[0].ToString();
                        nombre2 = dr[1].ToString();
                        apellido1 = dr[2].ToString();
                        apellido2 = dr[3].ToString();
                        genero = dr[4].ToString();
                        fechanac = dr[5].ToString();
                        celular = dr[6].ToString();
                        email = dr[7].ToString();
                        direccion = dr[8].ToString();
                    }

                    txtNombre1.Text = nombre1;
                    txtNombre2.Text = nombre2;
                    txtApellido1.Text = apellido1;
                    txtApellido2.Text = apellido2;
                    txtCelular.Text = celular;
                    txtEmail.Text = email;
                    txtFecha.Text = fechanac;
                    txtDireccion.Text = direccion;

                    txtApellido1.Enabled = false;
                }

                //FunGetCiudad();

            }

        }

       

        private void FunGetCiudad()
        {

            response = new MediLinkApi().GetCiudad("https://testagendamiento.medilink.com.ec:443/", Session["AccessToken"].ToString());
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

        private void FunGetSucursal(int codciudad)
        {
            response = new MediLinkApi().GetSucursal("https://testagendamiento.medilink.com.ec:443/", Session["AccessToken"].ToString(), codCiudad);

            var Resultjson = JsonConvert.DeserializeObject<SucursalObj>(response);

            ddlSucursal.Items.Clear();
            ListItem i;
            i = new ListItem("--Seleccione Sucursal--", "0");
            ddlSucursal.Items.Add(i);
            foreach (var item in Resultjson.datos)
            {
                string codigosucursal = item.idSucursal;
                string nombrecentromedico = item.nombreCentromedico;

                i = new ListItem(nombrecentromedico, codigosucursal);

                ddlSucursal.Items.Add(i);

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

        private void FunRegistrarPaciente()
        {

            //validar admision
            var admision = new Admision
            {
                identificacion = documento,
                empresaAdmision = "1792206979001"

            };

            var dataAdmision = new JavaScriptSerializer().Serialize(admision);

            response = new MediLinkApi().PostAdmision("https://testagendamiento.medilink.com.ec:443/", dataAdmision, accessToken);


            DateTime FechaNaci = DateTime.ParseExact(fechanac, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string newFechaNaci = FechaNaci.ToString("yyyy-MM-dd");

            var paciente = new Paciente
            {
                tipoIdentificacion = "C",
                numeroIdentificacion = documento,
                primerNombre = nombre1,
                segundoNombre = nombre2,
                primerApellido = apellido1,
                segundoApellido = apellido2,
                fechaNacimiento = newFechaNaci,
                email = email,
                sexo = genero,
                telefonioMovil = celular
            };

            var data = new JavaScriptSerializer().Serialize(paciente);
            response = new MediLinkApi().PostCrearPaciente("https://testagendamiento.medilink.com.ec:443/", data, accessToken);
        }

        protected void ddlciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            FunGetSucursal(codCiudad);
        }

        protected void btnConsul_Click(object sender, EventArgs e)
        {
            response = new MediLinkApi().GetVerificarPaciente("https://testagendamiento.medilink.com.ec:443/", accessToken, documento, "C");
            if(response == "SI")
            {
                FunGetCiudad();
            }
            else if(response == "NO")
            {

                updPaciente.Visible = true;

            }

        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            FunRegistrarPaciente();
        }
    }
}