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
        string idpaciente = "";
        string idbene = "";
        string idpro = "";
        string user = "";
        string pass = "";
        string documento = "";
        string response = "";
        string tipodocumento = "";
        string tipoidentificacion = "";
        string nombresCompletos = "";
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

        DataTable datosMedico = new DataTable();
        DataTable datosDisponibie = new DataTable();

        DataTable _datosMedico = new DataTable();
        DataTable _datosDisponible = new DataTable();

        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                idtitular = Request["CodigoTitular"];
                idbene = Request["CodigoBene"];
                idpro = Request["CodigoPro"];

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

                datosMedico.Columns.Add("codigoMedico");
                datosMedico.Columns.Add("nombreMedico");
                ViewState["DatosMedico"] = datosMedico;

                datosDisponibie.Columns.Add("codigoMedico");
                datosDisponibie.Columns.Add("idHorarioDisponible");
                datosDisponibie.Columns.Add("horaDisponible");
                ViewState["DatosDisponibles"] = datosDisponibie;


                if (idtitular != "" && idbene == "0")
                {
                    //GET DATOS TITULAR
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(idtitular);
                    objparam[1] = "";
                    objparam[2] = 185;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        tipoidentificacion = dr[0].ToString();
                        documento = dr[1].ToString();
                        nombresCompletos = dr[2].ToString();
                    }

                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    ViewState["Cedula"] = documento;
                    ViewState["Titular"] = "TITULAR";
                    ViewState["Nombres"] = nombresCompletos;

                    //verificar su paciente esta registrado en MEDILINK
                    response = new MediLinkApi().GetVerificarPaciente(_url, accessToken, documento, tipoidentificacion);

                    if (response == "SI")
                    {
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        //TRAER DE BASE DE  DATOS
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(idtitular);
                        objparam[1] = "";
                        objparam[2] = 188;
                        dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        idpaciente = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["idPaciente"] = idpaciente;
                    }
                    else 
                    {
                        Array.Resize(ref objparam, 3);
                        objparam[0] = 1;
                        objparam[1] = int.Parse(idtitular);
                        objparam[2] = 0;
                        dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularBene", objparam);

                        foreach (DataRow dr in dt.Tables[0].Rows)
                        {
                            tipodocumento = dr[0].ToString();
                            nombre1 = dr[1].ToString();
                            nombre2 = dr[2].ToString();
                            apellido1 = dr[3].ToString();
                            apellido2 = dr[4].ToString();
                            genero = dr[5].ToString();
                            fechanac = dr[6].ToString();
                            celular = dr[7].ToString();
                            email = dr[8].ToString();
                            direccion = dr[9].ToString();
                        }

                        ViewState["FechaNaci"] = fechanac;

                        //REGISTRAR PACIENTE
                        string respuesta = FunRegistrarPaciente();

                    }

                }else if(idtitular != "" && idbene != "")
                {

                    //DATOS BENEFICIARIO
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 2;
                    objparam[1] = int.Parse(idtitular);
                    objparam[2] = int.Parse(idbene); ;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularBene", objparam);

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        tipodocumento = dr[0].ToString();
                        documento = dr[1].ToString();
                        nombresCompletos = dr[2].ToString();
                        nombre1 = dr[3].ToString();
                        nombre2 = dr[4].ToString();
                        apellido1 = dr[5].ToString();
                        apellido2 = dr[6].ToString();
                        genero = dr[7].ToString();
                        fechanac = dr[8].ToString();
                        direccion = dr[9].ToString();
                        celular = dr[10].ToString();
                        email = dr[11].ToString();   
                    }

                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    response = new MediLinkApi().GetVerificarPaciente(_url, accessToken, documento, tipodocumento);
                    if (response == "SI")
                    {
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        //OBTENER ID 
                    }
                    else if(response == "NO")
                    {
                        string respuesta = "OK";
                        //REGUSTRAR BENEFICIARIO
                       //string respuesta = FunRegistrarPaciente();
                        if(respuesta == "OK")
                        {
                            FunGetCiudad();
                            pnlOpciones.Visible = true;
                            lblRegistro.Text = "Nuevo";
                        }

                    }

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
            if (response != "") lstBoxMedicos.Visible = true;

            var Resultjson = JsonConvert.DeserializeObject<DisponibilidadObj>(response);

            _datosMedico = (DataTable)ViewState["DatosMedico"];
            _datosMedico.Clear();

            _datosDisponible = (DataTable)ViewState["DatosDisponibles"];
            _datosDisponible.Clear();

            if(Resultjson != null)
            {
                foreach (var _datos in Resultjson.datos)
                {
                    DataRow rowMedico = _datosMedico.NewRow();
                    rowMedico["codigoMedico"] = _datos.codigoMedico;
                    rowMedico["nombreMedico"] = _datos.nombreMedico;
                    _datosMedico.Rows.Add(rowMedico);

                    foreach (var _datosdisponibles in _datos.disponibilidad)
                    {
                        foreach (var _horarios in _datosdisponibles.horario)
                        {
                            DataRow rowDisponible = _datosDisponible.NewRow();
                            rowDisponible["codigoMedico"] = _datos.codigoMedico;
                            rowDisponible["idHorarioDisponible"] = _horarios.idHorarioDisponible;
                            rowDisponible["horaDisponible"] = _horarios.horaInicio + "-" + _horarios.horaFin;
                            _datosDisponible.Rows.Add(rowDisponible);
                            ViewState["DatosDisponibles"] = _datosDisponible;
                        }

                    }
                }

                FunLlenarListMedico();
            }
            else
            {
                new Funciones().funShowJSMessage("Sin Medicos", this);
            }

        }

        private void FunLlenarListMedico() 
        {
            
            foreach (DataRow _row in _datosMedico.Rows)
            {
                var newItem = new ListItem();

                newItem.Value = _row["codigoMedico"].ToString();
                newItem.Text = _row["nombreMedico"].ToString();
                lstBoxMedicos.Items.Add(newItem);
            }
        }

        //Registrar Paciente
        private string FunRegistrarPaciente()
        {

            //validar admision
            var admision = new Admision
            {
                identificacion = documento,
                empresaAdmision = "1792206979001"

            };

            var dataAdmision = new JavaScriptSerializer().Serialize(admision);

            response = new MediLinkApi().PostAdmision(_url, dataAdmision, Session["AccessToken"].ToString());


            DateTime FechaNaci = DateTime.ParseExact(fechanac, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string newFechaNaci = FechaNaci.ToString("yyyy-MM-dd");

            if(response == "OK")
            {

                var paciente = new Paciente
                {
                    tipoIdentificacion = tipodocumento,
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
            else
            {
                new Funciones().funShowJSMessage("Fallo Admision", this);
            }

            return "OK";
        }

        private void FunAgendarCita(int idPaciente,int idCiudad,int idMedico,int idSucur,int idEspeci,int idHorario)
        {
            
            var crearCita = new CrearCita
            {
                idPaciente = idPaciente,
                idCiudad = idCiudad,
                idMedico = idMedico,
                idSucursal = idSucur,
                idEspecialidad = idEspeci,
                horarioDisponible = new HorarioDisponible
                {
                    idHorarioDisponible = idHorario
                }
            };
   
            var cita = new JavaScriptSerializer().Serialize(crearCita);
            response = new MediLinkApi().PostCrearCita(_url, Session["AccessToken"].ToString(), cita);

            if(response != "")
            {
                dynamic datoscita = JObject.Parse(response);
                string codigo = datoscita.datos.codigoCita;
                string direccion = datoscita.datos.direccion;
                
                txtTitCita.Visible = true;
                txtCodCita.Visible = true;
                lblCodigo.Text = codigo;
                txtCiudad.Visible = true;
                lblCiudad.Text = ViewState["Ciudad"].ToString();
                txtFecha.Visible = true;
                lblFecha.Text = "";
                txtHora.Visible = true;
                lblHora.Text = ViewState["Hora"].ToString();
                txtPrestador.Visible = true;
                lblPrestador.Text = ViewState["Sucursal"].ToString();
                txtMedico.Visible = true;
                lblMedico.Text = ViewState["Medico"].ToString();
                txtEspe.Visible = true;
                lblEspe.Text = ViewState["Especialidad"].ToString();
                txtObservacion.Visible = true;
                lblObser.Text = "";
                txtCedula.Visible = true;
                lblCedula.Text = ViewState["Cedula"].ToString();
                txtTipo.Visible = true;
                lblTipo.Text = ViewState["Titular"].ToString();
                txtPaciente.Visible = true;
                lblPaciente.Text = ViewState["Nombres"].ToString();
                txtFechaNaci.Visible = true;
                lblFechaNaci.Text = "";
                txtDireccion.Visible = true;
                lblDireccion.Text = direccion;
                txtTelefono.Visible = true;
                lblTelefono.Text = "";
            }
            else
            {
                new Funciones().funShowJSMessage("No se genero Cita", this);
            }
        }
        #endregion

        #region Eventos

        protected void ddlciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            string ciudad = ddlciudad.SelectedItem.ToString();
            ViewState["codCiudad"] = codCiudad;
            ViewState["Ciudad"] = ciudad;
            FunGetSucursal(codCiudad);
        }


        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codSucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            string sucursal = ddlSucursal.SelectedItem.ToString();
            ViewState["codSucursal"] = codSucursal;
            ViewState["Sucursal"] = sucursal;
            FunGetEspe(codSucursal);
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            FunRegistrarPaciente();
        }

        protected void ddlespeci_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codEspe = int.Parse(ddlEspecialidad.SelectedValue.ToString());
            string espe = ddlEspecialidad.SelectedItem.ToString();
            ViewState["codEspecialidad"] = codEspe;
            ViewState["Especialidad"] = espe;
            //FunDisponibilidades(int.Parse(ViewState["codCiudad"].ToString()), codEspe, int.Parse(ViewState["codSucursal"].ToString()), txtFechaIni.Text);
            FunDisponibilidades(int.Parse(ddlciudad.SelectedValue), codEspe, 1, txtFechaIni.Text);
        }

        protected void lstBoxMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _codMedico = lstBoxMedicos.SelectedItem.Value;
            string medico = lstBoxMedicos.SelectedItem.ToString();
            ViewState["codMedico"] = _codMedico;
            ViewState["Medico"] = medico;
            DataTable dttDisponibles = (DataTable)ViewState["DatosDisponibles"];
            if (dttDisponibles.ToString() != "") LstBoxHorario.Visible = true;

            DataRow[] _datorow = dttDisponibles.Select("codigoMedico=" + _codMedico);

            foreach (var _datosdisponibles in _datorow)
            {
            
                var newItem = new ListItem();
                newItem.Value = _datosdisponibles["idHorarioDisponible"].ToString();
                newItem.Text = _datosdisponibles["horaDisponible"].ToString(); 
                LstBoxHorario.Items.Add(newItem);
            }

        }

        protected void lstBoxHorasMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _codHoraMed = LstBoxHorario.SelectedItem.Value;
            string hora = LstBoxHorario.SelectedItem.ToString();
            ViewState["codHoraMed"] = _codHoraMed;
            ViewState["Hora"] = hora;
            btnCrearCita.Visible = true;
        }
        #endregion

        protected void btnAgendar_Click(object sender, EventArgs e)
        {

            FunAgendarCita(int.Parse(ViewState["idPaciente"].ToString()), int.Parse(ViewState["codCiudad"].ToString()), int.Parse(ViewState["codMedico"].ToString()),int.Parse(ViewState["codSucursal"].ToString()), int.Parse(ViewState["codEspecialidad"].ToString()),int.Parse(ViewState["codHoraMed"].ToString()));

        }
    }
}