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
        #region Variables
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
        string _url = "https://testagendamiento.medilink.com.ec:443/";
        int codCiudad = 0;
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        #endregion

        #region Load
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
                accessToken = new MediLinkApi().PostAccesLogin(_url, data);

                Session["AccessToken"] = accessToken;
                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");

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

                response = new MediLinkApi().GetVerificarPaciente(_url, accessToken, documento, "C");

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
            }
        }
        #endregion

        #region Funciones
        //Llenar combo Ciudad
        private void FunGetCiudad()
        {

            response = new MediLinkApi().GetCiudad(_url, Session["AccessToken"].ToString());
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

        //Llenar combo Sucursal
        private void FunGetSucursal(int codciudad)
        {
            response = new MediLinkApi().GetSucursal(_url, Session["AccessToken"].ToString(), codCiudad);

            var Resultjson = JsonConvert.DeserializeObject<SucursalObj>(response);

            ddlSucursal.Items.Clear();
            ListItem s;
            s = new ListItem("--Seleccione Sucursal--", "0");
            ddlSucursal.Items.Add(s);
            foreach (var item in Resultjson.datos)
            {
                string codigosucursal = item.idSucursal;
                string nombrecentromedico = item.nombreCentromedico;

                s = new ListItem(nombrecentromedico, codigosucursal);

                ddlSucursal.Items.Add(s);

            }

        }

        //Llenar combo Especialidad
        private void FunGetEspe(int codSucursal)
        {
            response = new MediLinkApi().GetEspecialidad(_url, Session["AccessToken"].ToString(), codSucursal);
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

        //Llenar combo Medico
        private void FunGetMedico(int codEspe, int CodSucur)
        {
            response = new MediLinkApi().GetMedicos(_url, Session["AccessToken"].ToString(), codEspe, CodSucur);
            var Resultjson = JsonConvert.DeserializeObject<MedicoObj>(response);

            ListItem m;
            m = new ListItem("--Seleccione Medico--", "0");
            //ddlMedicos.Items.Add(m);
            foreach (var med in Resultjson.datos)
            {
                string codigoMedico = med.idMedico.ToString();
                string medicoNombre = med.nombreCompleto.ToString();

                m = new ListItem(medicoNombre, codigoMedico);
                //ddlMedicos.Items.Add(m);
            }

        }

        //Obtener disponibilidad
         private void FunDisponibilidades(int codCiudad,int codEspeci,int codSucur, string fechacita)
         {
            string fechanew = DateTime.ParseExact(fechacita, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            response = new MediLinkApi().GetDisponibilidad(_url, Session["AccessToken"].ToString(), codCiudad, codEspeci, codSucur, fechanew);

            var Resultjson = JsonConvert.DeserializeObject<DisponibilidadObj>(response);

            foreach (var _dat in Resultjson.datos)
            {
                string codigosucursal = _dat.codigoSucursal;


                foreach (var _fec in _dat.disponibilidad)
                {
                    string _fechadisponible = _fec.fechaDisponibilidad;

                    foreach (var _hor in _fec.horario) 
                    {
                        string idhorario = _hor.idHorarioDisponible;
                    }

                }

            }

        }



        //Registrar Paciente
        private void FunRegistrarPaciente()
        {

            //validar admision
            var admision = new Admision
            {
                identificacion = documento,
                empresaAdmision = "1792206979001"

            };

            var dataAdmision = new JavaScriptSerializer().Serialize(admision);

            response = new MediLinkApi().PostAdmision(_url, dataAdmision, accessToken);


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
            response = new MediLinkApi().PostCrearPaciente(_url, data, accessToken);
        }
        #endregion

        #region Eventos

        protected void ddlciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            ViewState["codCiudad"] = codCiudad;
            FunGetSucursal(codCiudad);
        }


        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codSucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            ViewState["codSucursal"] = codSucursal;
            FunGetEspe(codSucursal);
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            FunRegistrarPaciente();
        }

        protected void ddlespeci_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codEspe = int.Parse(ddlEspecialidad.SelectedValue.ToString());
            ViewState["codEspecialidad"] = codEspe;
            //FunDisponibilidades(int.Parse(ViewState["codCiudad"].ToString()), codEspe, int.Parse(ViewState["codSucursal"].ToString()), txtFechaIni.Text);
            FunDisponibilidades(int.Parse(ddlciudad.SelectedValue), codEspe, 1, txtFechaIni.Text);
        }

        //protected void ddlmedico_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //int codMedico = int.Parse(ddlMedicos.SelectedValue.ToString());
        //    //ViewState["codMedico"] = codMedico;
        //}
        #endregion
    }
}