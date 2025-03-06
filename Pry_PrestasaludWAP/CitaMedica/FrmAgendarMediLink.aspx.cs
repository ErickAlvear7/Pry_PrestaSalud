using System;
using System.Collections.Generic;
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
        string usuario = "";
        string user = "";
        string pass = "";
        string documento = "";
        string fechanacimiento = "";
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
        string telcasa = "";
        string telcelular = "";
        string email = "";
        string direccion = "";
        string telefonos = "";
        string fechaCita = "";
        string _url = "https://testagendamiento.medilink.com.ec:443/";
        string _urlpro = "https://agendamiento.medilink.com.ec:8443/";
        int codCiudad = 0;
        int codEspe = 0;
        int codSucursal = 0;
        string sucursal = "";
        string espe = "";
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();

        DataTable datosMedico = new DataTable();
        DataTable datosDisponibie = new DataTable();

        DataTable _datosMedico = new DataTable();
        DataTable _datosDisponible = new DataTable();

        DataTable dtsucursal = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                idtitular = Request["CodigoTitular"];
                idbene = Request["CodigoBene"];
                idpro = Request["CodigoPro"];
                usuario = Session["usuLogin"].ToString();

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
                accessToken = new MediLinkApi().PostAccesLogin(_urlpro, data);

                Session["AccessToken"] = accessToken;
                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");

                datosMedico.Columns.Add("codigoMedico");
                datosMedico.Columns.Add("nombreMedico");
                ViewState["DatosMedico"] = datosMedico;

                datosDisponibie.Columns.Add("codigoMedico");
                datosDisponibie.Columns.Add("idHorarioDisponible");
                datosDisponibie.Columns.Add("horaDisponible");
                ViewState["DatosDisponibles"] = datosDisponibie;

                
                dtsucursal.Columns.Add("codCiudad");
                dtsucursal.Columns.Add("codSucursal");
                dtsucursal.Columns.Add("sucursalNombreComercial");


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
                        fechanacimiento = dr[3].ToString(); //yyyy-MM-dd
                        telcasa = dr[4].ToString();
                        telcelular = dr[5].ToString();
                    }

                    telefonos = telcasa + "/" + telcelular;
                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    ViewState["Cedula"] = documento;
                    ViewState["Titular"] = "TITULAR";
                    ViewState["Nombres"] = nombresCompletos;
                    ViewState["FechaNaci"] = fechanacimiento;
                    ViewState["Telefonos"] = telefonos;

                    //verificar su paciente esta registrado en MEDILINK
                    response = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipoidentificacion);

                    if (response != "")
                    {
                        int idresponse = int.Parse(response.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        //INSERT ID O GET ID MEDILINK
                        Array.Resize(ref objparam, 6);
                        objparam[0] = 0;
                        objparam[1] = idresponse;
                        objparam[2] = int.Parse(idtitular); 
                        objparam[3] = 0;
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        idpaciente = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["idPaciente"] = idpaciente;
                    }
                    else 
                    {
                        //CONSULTAR DATOS DE TITULAR PARA REGISTRAR EN MEDILINK
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
                            fechanac = dr[6].ToString();//yyyy-MM-dd
                            celular = dr[7].ToString();
                            email = dr[8].ToString();
                            direccion = dr[9].ToString();
                        }

                        ViewState["FechaNaci"] = fechanac;

                        //REGISTRAR PACIENTE
                        string respuesta = FunRegistrarPaciente();
                        
                        if (respuesta != "")
                        {
                            //GUARDAR ID BDD Expert_MEDILINK
                            idpaciente = respuesta;
                            
                            Array.Resize(ref objparam, 6);
                            objparam[0] = 0;
                            objparam[1] = int.Parse(idpaciente); 
                            objparam[2] = int.Parse(idtitular);
                            objparam[3] = 0;
                            objparam[4] = int.Parse(idpro);
                            objparam[5] = usuario;
                            dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                            if (dt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                FunGetCiudad();
                                pnlOpciones.Visible = true;
                                lblRegistro.Text = "Nuevo";
                            }

                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Fallo Registro Paciente", this);
                        }
                    }

                }else if(idtitular != "" && idbene != "")//SI LA CEDUDA ES DEL BENEFICIARIO
                {

                    //DATOS BENEFICIARIO
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 2;
                    objparam[1] = int.Parse(idtitular);
                    objparam[2] = int.Parse(idbene); 
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
                        fechanac = dr[8].ToString();//yyyy-MM-dd
                        direccion = dr[9].ToString();
                        celular = dr[10].ToString();
                        email = dr[11].ToString();   
                    }

                    ViewState["FechaNaci"] = fechanac;

                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    response = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);

                    if (response != "")
                    {
                        int idresponse = int.Parse(response.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        //INSERT ID O GET ID MEDILINKBE
                        Array.Resize(ref objparam, 6);
                        objparam[0] = 1;
                        objparam[1] = idresponse;
                        objparam[2] = int.Parse(idtitular);
                        objparam[3] = int.Parse(idbene);
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        idpaciente = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["idPaciente"] = idpaciente;
                    }
                    else
                    {
                        string respuesta = FunRegistrarPaciente();

                        if (respuesta != "")
                        {
                            //GUARDAR ID BDD Expert_MEDILINK
                            idpaciente = respuesta;

                            Array.Resize(ref objparam, 6);
                            objparam[0] = 1;
                            objparam[1] = int.Parse(idpaciente);
                            objparam[2] = int.Parse(idtitular);
                            objparam[3] = int.Parse(idbene);
                            objparam[4] = int.Parse(idpro);
                            objparam[5] = usuario;
                            dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                            if (dt.Tables[0].Rows[0][0].ToString() != "")
                            {
                                FunGetCiudad();
                                pnlOpciones.Visible = true;
                                lblRegistro.Text = "Nuevo";
                            }
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Fallo Registro Beneficiario", this);
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

            response = new MediLinkApi().GetCiudad(_urlpro, Session["AccessToken"].ToString());
            //dynamic dataCiudad = JObject.Parse(response);
            if(response != "")
            {
                //var Resultjson = JsonConvert.DeserializeObject<List<Root>>(response);

                var Resultjson = JsonConvert.DeserializeObject<CiudadesSucursal>(response);

                ddlciudad.Items.Clear();

                ListItem i;
                i = new ListItem("--Seleccione Ciudad--", "0");
                ddlciudad.Items.Add(i);
                foreach (var ciudades in Resultjson.datos)
                {
                    string codigoCiudad = ciudades.codCiudad.ToString();
                    string ciudad = ciudades.nombreCiudad;
                    
                    i = new ListItem(ciudad, codigoCiudad);

                    ddlciudad.Items.Add(i);
               
                    foreach(var sucursales in ciudades.sucursales )
                    {
                        string _codsucursal = sucursales.codSucursal;
                        string _nomsucursal = sucursales.sucursalNombreComercial;

                        DataRow rowSucursal = dtsucursal.NewRow();
                        rowSucursal["codCiudad"] = codigoCiudad;
                        rowSucursal["codSucursal"] = _codsucursal;
                        rowSucursal["sucursalNombreComercial"] = _nomsucursal;
                        dtsucursal.Rows.Add(rowSucursal);

                    }
                }
                ViewState["DatosSucursal"] = dtsucursal;
            }
            else
            {
                new Funciones().funShowJSMessage("No hay Ciudades para listar!!", this);
            }
          
        }

        //Llenar combo Sucursal
        private void FunGetSucursal(int codciudad)
        {
            response = new MediLinkApi().GetSucursal(_urlpro, Session["AccessToken"].ToString(), codCiudad);

            if(response != "")
            {
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
            else
            {
                new Funciones().funShowJSMessage("No hay Sucursales para listar!!", this);
            }       
        }

        //Llenar combo Especialidad
        private void FunGetEspe(int codSucursal)
        {
            response = new MediLinkApi().GetEspecialidad(_urlpro, Session["AccessToken"].ToString(), codSucursal);

            if(response != "")
            {
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
            else
            {
                new Funciones().funShowJSMessage("No hay Especialidades para listar!!", this);
            }
     
        }

        //Llenar combo Medico
        private void FunGetMedico(int codEspe, int CodSucur)
        {
            response = new MediLinkApi().GetMedicos(_urlpro, Session["AccessToken"].ToString(), codEspe, CodSucur);

            if(response != "")
            {
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
            else
            {
                new Funciones().funShowJSMessage("No hay Medicos para listar!!", this);
            }          
        }

        //Obtener disponibilidad
         private void FunDisponibilidades(int codCiudad,int codEspeci,int codSucur, string fechacita)
         {
            string fechanew = DateTime.ParseExact(fechacita, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            response = new MediLinkApi().GetDisponibilidad(_urlpro, Session["AccessToken"].ToString(), codCiudad, codEspeci, codSucur, fechanew);
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
            string idregistro = "";

                var paciente = new Paciente
                {
                    tipoIdentificacion=tipodocumento,
                    numeroIdentificacion=documento,
                    primerNombre=nombre1,
                    segundoNombre=nombre2,
                    primerApellido=apellido1,
                    segundoApellido=apellido2,
                    fechaNacimiento=fechanac,
                    email=email,
                    sexo=genero,
                    telefonioMovil=celular
                };

                var data = new JavaScriptSerializer().Serialize(paciente);
                response = new MediLinkApi().PostCrearPaciente(_urlpro, data, accessToken);
                
                if(response == "VALIDAR")
                {
                    var admision = new Admision
                    {
                        identificacion = documento,
                        empresaAdmision = "1792206979001" //RUC PRESTASALUD
                    };

                    var dataAdmision = new JavaScriptSerializer().Serialize(admision);

                    response = new MediLinkApi().PostAdmision(_urlpro, dataAdmision, Session["AccessToken"].ToString());

                    if(response == "OK")
                    {
                       idregistro = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);
                       
                    }
                    else
                    {
                       idregistro = "";
                    }
                }
                
            return idregistro;
        }

        private void FunAgendarCita(int idPaciente,int idCiudad,int idMedico,int idSucur,int idEspeci,int idHorario,string fechaCita)
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
            response = new MediLinkApi().PostCrearCita(_urlpro, Session["AccessToken"].ToString(), cita);

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
                lblFecha.Text = fechaCita;
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
                lblFechaNaci.Text = ViewState["FechaNaci"].ToString();
                txtDireccion.Visible = true;
                lblDireccion.Text = direccion;
                txtTelefono.Visible = true;
                lblTelefono.Text = ViewState["Telefonos"].ToString();
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
            ddlSucursal.Items.Clear();
            ddlEspecialidad.Items.Clear();
            lstBoxMedicos.Visible = false;
            LstBoxHorario.Visible = false;
            codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            string ciudad = ddlciudad.SelectedItem.ToString();
            ViewState["codCiudad"] = codCiudad;
            ViewState["Ciudad"] = ciudad;

            DataTable _datossucursal = (DataTable)ViewState["DatosSucursal"];

            DataRow[] _row = _datossucursal.Select("codCiudad=" + codCiudad);

            ddlSucursal.Items.Clear();
            ListItem s;
            s = new ListItem("--Seleccione Sucursal--", "0");
            ddlSucursal.Items.Add(s);

            foreach (var _datos in _row)
            {
                s = new ListItem(_datos["sucursalNombreComercial"].ToString(), _datos["codSucursal"].ToString());

                ddlSucursal.Items.Add(s);
            }


            //FunGetSucursal(codCiudad);
        }

        protected void ddlSucursal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEspecialidad.Items.Clear();
            lstBoxMedicos.Visible = false;
            LstBoxHorario.Visible = false;
            codSucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            sucursal = ddlSucursal.SelectedItem.ToString();
            ViewState["codSucursal"] = codSucursal;
            ViewState["Sucursal"] = sucursal;
            FunGetEspe(codSucursal);
        }

        protected void ddlespeci_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstBoxMedicos.Items.Clear();
            LstBoxHorario.Visible = false;
            LstBoxHorario.Items.Clear();
            codEspe = int.Parse(ddlEspecialidad.SelectedValue.ToString());
            espe = ddlEspecialidad.SelectedItem.ToString();
            codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            int codsucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            ViewState["codEspecialidad"] = codEspe;
            ViewState["Especialidad"] = espe;
            FunDisponibilidades(codCiudad, codEspe, codsucursal, txtFechaIni.Text);
        }

        protected void lstBoxMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstBoxHorario.Items.Clear();
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

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            fechaCita = txtFechaIni.Text;

            FunAgendarCita(int.Parse(ViewState["idPaciente"].ToString()), int.Parse(ViewState["codCiudad"].ToString()), int.Parse(ViewState["codMedico"].ToString()), int.Parse(ViewState["codSucursal"].ToString()), int.Parse(ViewState["codEspecialidad"].ToString()), int.Parse(ViewState["codHoraMed"].ToString()), fechaCita);

        }
        #endregion
    }
}