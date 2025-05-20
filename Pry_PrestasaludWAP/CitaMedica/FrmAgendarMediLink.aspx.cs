using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pry_PrestasaludWAP.Api;
using System;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.WebPages;
using static Pry_PrestasaludWAP.Modelo.MediLinkModel;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmAgendarMediLink : System.Web.UI.Page
    {
        #region Variables
        string accessToken = "", fechaCalendar="", fechaCita="";
        string idtitular = "", idbene="", idpro=" ", usuario="";
        string _idtitumed = "", _idbenemed="", direcsucursal="";
        string userApikey = "", passApikey = "";
        string documento = "", fechanacimiento = "", parentesco = "", tipodocumento = "", nombresCompletos = "", nombre1 = "",nombre2 = "",
               apellido1 = "", apellido2 = "", genero = "", celular = "", telcasa = "", email = "", direccion = "", telefonos = "";
        string respVerifPacient = "", respRegisPacient="",respGetCiudad="",respGetSucur="",respGetEspe="",respGetMedico="",respGetDispo="",
               respCrearPacient = "",respAdmision="",respCrearCita="", nombreProducto="", medicamento="", fileTemplate="",nombreCliente="", mailsA="", sendmails="", subject="", mensaje="";

      

        string _urlpro = "https://agendamiento.medilink.com.ec:8443/", _url = "https://testagendamiento.medilink.com.ec:443/";
        int _idresponVP = 0, codsucursal=0;
        int codCiudad = 0, codEspe = 0, codSucursal = 0, codMedico = 0, codPrestador = 0, codEspepres = 0, codMedicopres=0;
        string ddlTxtSucursal = "", ddlTxtCiudad= "", ddlTxtEspeci="", lstCodHoraMed = "", lstTxthoraMed = "", diaCalendar="", codCitaPresta="", codCitaMedilink="";
     
        Object[] objparam = new Object[1];
        Object[] objcitamedica = new Object[23];
        Object[] objsendmails = new Object[3];
        DataSet dt = new DataSet();
        DataSet dts = new DataSet();
        DataSet dtusu = new DataSet();

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
                ViewState["CodBeneficiario"] = idbene;
                ViewState["CodTitular"] = idtitular;
                ViewState["CodProducto"] = idpro;

                objparam[0] = 61;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    userApikey = dt.Tables[0].Rows[0][1].ToString().Trim();
                    passApikey = dt.Tables[0].Rows[1][1].ToString().Trim();
                }

                var login = new LoginApi
                {
                    username = userApikey,
                    password = passApikey
                };


                //consultar nombre del cliente y producto
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(idpro);
                objparam[1] = "";
                objparam[2] = 188;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    nombreProducto = dr[0].ToString();
                    nombreCliente = dr[1].ToString();                
                }

                ViewState["nombreProducto"] = nombreProducto;
                ViewState["nombreCliente"] = nombreCliente;

                var data = new JavaScriptSerializer().Serialize(login);
                accessToken = new MediLinkApi().PostAccesLogin(_urlpro, data);
                fechaCalendar = DateTime.Now.ToString("yyyyMMdd");
                fechaCita = DateTime.Now.ToString("yyyy-MM-dd");
                diaCalendar = Calendar.SelectedDate.ToString("dddd");

                ViewState["FechaCita"] = fechaCita;
                ViewState["FechaCalendar"] = fechaCalendar;
                ViewState["DiaCalendar"] = diaCalendar;

                Session["AccessToken"] = accessToken;
                
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

                Array.Resize(ref objparam, 1);
                objparam[0] = 13;
                dts = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dts != null && dts.Tables[0].Rows.Count > 0)
                {
                    ViewState["Host"] = dts.Tables[0].Rows[0][0].ToString().Trim().ToLower();
                    ViewState["Port"] = dts.Tables[0].Rows[1][0].ToString().Trim().ToLower();
                    ViewState["EnableSSl"] = dts.Tables[0].Rows[2][0].ToString().Trim().ToLower();
                    ViewState["Usuario"] = dts.Tables[0].Rows[3][0].ToString().Trim().ToLower();
                    ViewState["Password"] = dts.Tables[0].Rows[4][0].ToString().Trim();
                }


                if (int.Parse(idtitular) > 0 && int.Parse(idbene) == 0)
                {
                
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 1;
                    objparam[1] = int.Parse(idtitular);
                    objparam[2] = 0;
                    dt = new Conexion(2, " ").funConsultarSqls("sp_CargarTitularBene", objparam);

                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        tipodocumento = dr[0].ToString();
                        documento = dr[1].ToString();
                        nombre1 = dr[2].ToString();
                        nombre2 = dr[3].ToString();
                        apellido1 = dr[4].ToString();
                        apellido2 = dr[5].ToString();
                        nombresCompletos = dr[6].ToString();
                        genero = dr[7].ToString();
                        fechanacimiento = dr[8].ToString();//yyyy-MM-dd
                        telcasa = dr[9].ToString();
                        celular = dr[10].ToString();
                        email = dr[11].ToString();
                        direccion = dr[12].ToString();
                    }

                    telefonos = telcasa + "/" + celular;
                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    ViewState["CodTitular"] = idtitular;
                    ViewState["Cedula"] = documento;
                    ViewState["Titular"] = "TITULAR";
                    ViewState["Tipo"] = "T";
                    ViewState["Parentesco"] = "";
                    ViewState["Nombres"] = nombresCompletos;
                    ViewState["FechaNaci"] = fechanacimiento;
                    ViewState["Telefonos"] = telefonos;
                    ViewState["Direccion"] = direccion;

                    respVerifPacient = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);

                    if (!respVerifPacient.IsEmpty())
                    {
                        _idresponVP = int.Parse(respVerifPacient.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        Array.Resize(ref objparam, 14);
                        objparam[0] = 0;
                        objparam[1] = _idresponVP;
                        objparam[2] = int.Parse(idtitular); 
                        objparam[3] = 0;
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        objparam[6] = "";
                        objparam[7] = "";
                        objparam[8] = "";
                        objparam[9] = "";
                        objparam[10] = 0;
                        objparam[11] = 0;
                        objparam[12] = 0;
                        objparam[13] = 0;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        _idtitumed = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["IdMedilink"] = _idtitumed;
                    }
                    else 
                    {                                
                        respRegisPacient = FunRegistrarPaciente();
                        
                        if (!respRegisPacient.IsEmpty())
                        {
                            _idtitumed = respRegisPacient;
                            ViewState["IdMedilink"] = _idtitumed;

                            Array.Resize(ref objparam, 14);
                            objparam[0] = 0;
                            objparam[1] = int.Parse(_idtitumed); 
                            objparam[2] = int.Parse(idtitular);
                            objparam[3] = 0;
                            objparam[4] = int.Parse(idpro);
                            objparam[5] = usuario;
                            objparam[6] = "";
                            objparam[7] = "";
                            objparam[8] = "";
                            objparam[9] = "";
                            objparam[10] = 0;
                            objparam[11] = 0;
                            objparam[12] = 0;
                            objparam[13] = 0;
                            dt = new Conexion(2, " ").funConsultarSqls("sp_InsertMedilink", objparam);

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

                }else if(int.Parse(idtitular) > 0 && int.Parse(idbene) > 0)//SI LA CEDUDA ES DEL BENEFICIARIO
                {
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
                        fechanacimiento = dr[8].ToString();//yyyy-MM-dd
                        parentesco = dr[9].ToString();
                        direccion = dr[10].ToString();
                        telcasa = dr[11].ToString();
                        celular = dr[12].ToString();
                        email = dr[13].ToString();   
                    }
   
                    telefonos = telcasa + "/" + celular;
                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    ViewState["Cedula"] = documento;
                    ViewState["Titular"] = "BENEFICIARIO";
                    ViewState["Parentesco"] = parentesco;
                    ViewState["Tipo"] = "B";
                    ViewState["Nombres"] = nombresCompletos;
                    ViewState["FechaNaci"] = fechanacimiento;
                    ViewState["Telefonos"] = telefonos;
                    ViewState["CodBeneficiario"] = idbene;
                    ViewState["Direccion"] = direccion;

                    respVerifPacient = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);

                    if (!respVerifPacient.IsEmpty())
                    {
                        _idresponVP = int.Parse(respVerifPacient.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        Array.Resize(ref objparam, 14);
                        objparam[0] = 1;
                        objparam[1] = _idresponVP;
                        objparam[2] = int.Parse(idtitular);
                        objparam[3] = int.Parse(idbene);
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        objparam[6] = "";
                        objparam[7] = "";
                        objparam[8] = "";
                        objparam[9] = "";
                        objparam[10] = 0;
                        objparam[11] = 0;
                        objparam[12] = 0;
                        objparam[13] = 0;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        _idbenemed = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["IdMedilink"] = _idbenemed;
                    }
                    else
                    {
                        respRegisPacient = FunRegistrarPaciente();

                        if (!respRegisPacient.IsEmpty())
                        {
                            _idbenemed = respRegisPacient;
                            ViewState["IdMedilink"] = _idbenemed;

                            Array.Resize(ref objparam, 14);
                            objparam[0] = 1;
                            objparam[1] = int.Parse(_idbenemed);
                            objparam[2] = int.Parse(idtitular);
                            objparam[3] = int.Parse(idbene);
                            objparam[4] = int.Parse(idpro);
                            objparam[5] = usuario;
                            objparam[6] = "";
                            objparam[7] = "";
                            objparam[8] = "";
                            objparam[9] = "";
                            objparam[10] = 0;
                            objparam[11] = 0;
                            objparam[12] = 0;
                            objparam[13] = 0;
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
                            new Funciones().funShowJSMessage("Fallo Registro Beneficiario revise Cedula", this);
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
            respGetCiudad = new MediLinkApi().GetCiudad(_urlpro, Session["AccessToken"].ToString());
            //dynamic dataCiudad = JObject.Parse(response);
            if(!respGetCiudad.IsEmpty())
            {
                //var Resultjson = JsonConvert.DeserializeObject<List<Root>>(response);

                var Resultjson = JsonConvert.DeserializeObject<CiudadesSucursal>(respGetCiudad);

                ddlciudad.Items.Clear();

                ListItem i;
                i = new ListItem("--Seleccione Ciudad--", "0");
                ddlciudad.Items.Add(i);
                foreach (var ciudades in Resultjson.datos)
                {
                    string codigoCiudad = ciudades.codCiudad.ToString();
                    string ciudad = ciudades.nombreCiudad;

                    if(codigoCiudad == "1871" || codigoCiudad== "1180" || codigoCiudad == "1181")
                    {
                        i = new ListItem(ciudad, codigoCiudad);
                        ddlciudad.Items.Add(i);
                    }
                              
                    foreach(var sucursales in ciudades.sucursales )
                    {
                        string _codsucursal = sucursales.codSucursal;
                        string _nomsucursal = sucursales.sucursalNombreComercial;

                        if(_codsucursal =="28" || _codsucursal == "1" || _codsucursal == "3" || _codsucursal == "5" /*|| _codsucursal == "7"*/)
                        {
                            DataRow rowSucursal = dtsucursal.NewRow();
                            rowSucursal["codCiudad"] = codigoCiudad;
                            rowSucursal["codSucursal"] = _codsucursal;
                            rowSucursal["sucursalNombreComercial"] = _nomsucursal;
                            dtsucursal.Rows.Add(rowSucursal);
                        }                   
                    }
                }
                //s = new ListItem("--Seleccione Sucursal--", "0");
                //ddlSucursal.Items.Add(s);
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
            respGetSucur = new MediLinkApi().GetSucursal(_urlpro, Session["AccessToken"].ToString(), codCiudad);

            if(!respGetSucur.IsEmpty())
            {
                var Resultjson = JsonConvert.DeserializeObject<SucursalObj>(respGetSucur);

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
            respGetEspe = new MediLinkApi().GetEspecialidad(_urlpro, Session["AccessToken"].ToString(), codSucursal);

            if(!respGetEspe.IsEmpty())
            {
                var Resultjson = JsonConvert.DeserializeObject<EspeObj>(respGetEspe);
                ListItem e;
                e = new ListItem("--Seleccione Especialidad--", "0");
                ddlEspecialidad.Items.Add(e);

                foreach (var espe in Resultjson.datos)
                {
                    string codigoEspecialidad = espe.codEspecialidad.ToString();
                    string codigoSucursal = espe.codSucursal.ToString();
                    string espeNombre = espe.espNombre;

                    if(codigoEspecialidad == "11" || codigoEspecialidad == "22" || codigoEspecialidad == "22" || codigoEspecialidad == "42"
                        || codigoEspecialidad == "44" || codigoEspecialidad == "16" || codigoEspecialidad == "44" || codigoEspecialidad == "16"
                           || codigoEspecialidad == "23" || codigoEspecialidad == "32" || codigoEspecialidad == "28" || codigoEspecialidad == "20")
                    {
                        e = new ListItem(espeNombre, codigoEspecialidad);
                        ddlEspecialidad.Items.Add(e);
                    }                
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
            respGetMedico = new MediLinkApi().GetMedicos(_urlpro, Session["AccessToken"].ToString(), codEspe, CodSucur);

            if(!respGetMedico.IsEmpty())
            {
                var Resultjson = JsonConvert.DeserializeObject<MedicoObj>(respGetMedico);

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
         private void FunDisponibilidades(int codCiudad,int codEspeci,int codSucur, string fechaActual)
         {
            respGetDispo = new MediLinkApi().GetDisponibilidad(_urlpro, Session["AccessToken"].ToString(), codCiudad, codEspeci, codSucur, fechaActual);

            if (!respGetDispo.IsEmpty()) lstBoxMedicos.Visible = true;

            var Resultjson = JsonConvert.DeserializeObject<DisponibilidadObj>(respGetDispo);

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
                new Funciones().funShowJSMessage("Sin Turnos Disponibles", this);
                return;
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
                    fechaNacimiento= fechanacimiento,
                    email=email,
                    sexo=genero,
                    telefonioMovil=celular
                };

                var data = new JavaScriptSerializer().Serialize(paciente);
                respCrearPacient = new MediLinkApi().PostCrearPaciente(_urlpro, data, accessToken);
                
                if(respCrearPacient == "VALIDAR")
                {
                    var admision = new Admision
                    {
                        identificacion = documento,
                        empresaAdmision = "1792206979001" //RUC PRESTASALUD
                    };

                    var dataAdmision = new JavaScriptSerializer().Serialize(admision);

                    respAdmision = new MediLinkApi().PostAdmision(_urlpro, dataAdmision, Session["AccessToken"].ToString());

                    if(respAdmision == "OK")
                    {
                       idregistro = new MediLinkApi().GetVerificarPaciente(_urlpro, Session["AccessToken"].ToString(), documento, tipodocumento);
                       
                    }
                    else
                    {
                       idregistro = "";
                    }
                }
                
            return idregistro;
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
            respCrearCita = new MediLinkApi().PostCrearCita(_urlpro, Session["AccessToken"].ToString(), cita);

            if (!respCrearCita.IsEmpty())
            {
                dynamic datoscita = JObject.Parse(respCrearCita);
                codCitaMedilink = datoscita.datos.codigoCita;
                direcsucursal = datoscita.datos.direccion;
                //CONSULTAR HOMOLOGACION
                Array.Resize(ref objparam, 14);
                objparam[0] = 2;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = int.Parse(ViewState["CodSucursal"].ToString());
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                if(dt.Tables[0].Rows.Count > 0)
                {
                    codPrestador = int.Parse(dt.Tables[0].Rows[0][0].ToString());
                }

                Array.Resize(ref objparam, 14);
                objparam[0] = 4;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = int.Parse(ViewState["CodEspecialidad"].ToString());
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    codEspepres = int.Parse(dt.Tables[0].Rows[0][0].ToString());
                }

                Array.Resize(ref objparam, 14);
                objparam[0] = 5;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = int.Parse(ViewState["CodMedico"].ToString());
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                if (dt.Tables[0].Rows.Count > 0)
                {
                    codMedicopres = int.Parse(dt.Tables[0].Rows[0][0].ToString());
                }

                if (codMedicopres == 0) codMedicopres= int.Parse(ViewState["CodMedico"].ToString());

                Array.Resize(ref objparam, 14);
                objparam[0] = 0;
                objparam[1] = codPrestador; //CODIGO PRESTADOR
                objparam[2] = codEspepres; //CODIGO ESPECIALIDAD
                objparam[3] = codMedicopres;//CODIGO MEDICO
                objparam[4] = ViewState["Tipo"].ToString();
                objparam[5] = int.Parse(ViewState["CodTitular"].ToString());
                objparam[6] = int.Parse(ViewState["CodBeneficiario"].ToString());//problema
                objparam[7] = ViewState["Parentesco"].ToString(); 
                objparam[8] = ViewState["FechaCita"].ToString();
                objparam[9] = ViewState["DiaCalendar"].ToString();
                objparam[10] = ViewState["HoraMed"].ToString();
                objparam[11] = int.Parse(Session["usuCodigo"].ToString());
                objparam[12] = Session["MachineName"].ToString();
                objparam[13] = "AGENDAMIENTO MEDILINK";//OBSERVACION
                DataSet ds = new Conexion(2, "").FunCodigoCitaMedilink(objparam);
                codCitaPresta = ds.Tables[0].Rows[0][0].ToString();

                //GUARDAR CODIGOS DE CITA
                Array.Resize(ref objparam, 14);
                objparam[0] = 3;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] =" ";
                objparam[6] = ViewState["FechaCita"].ToString();
                objparam[7] = Session["MachineName"].ToString();
                objparam[8] = " ";
                objparam[9] = " ";
                objparam[10] = codCitaPresta;
                objparam[11] = codCitaMedilink;
                objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                objparam[13] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);

                if(dt.Tables[0].Rows[0][0].ToString() == "OK")
                {
                    txtTitCita.Visible = true;
                    txtCodCita.Visible = true;
                    lblCodigo.Text = codCitaPresta;
                    txtCiudad.Visible = true;
                    lblCiudad.Text = ViewState["Ciudad"].ToString();
                    txtFecha.Visible = true;
                    lblFecha.Text = ViewState["FechaCita"].ToString();///fecha cita
                    txtHora.Visible = true;
                    lblHora.Text = ViewState["HoraMed"].ToString();
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
                    lblDireccion.Text = direcsucursal;
                    txtTelefono.Visible = true;
                    lblTelefono.Text = ViewState["Telefonos"].ToString();
                    btnSalirMed.Visible = true;

                    FunEnviarMailCitaMedilink(ViewState["nombreCliente"].ToString(), ViewState["nombreProducto"].ToString(), "SI");
                }
            }
            else
            {
                new Funciones().funShowJSMessage("No se Genero Cita", this);
            }
        }

        private void FunEnviarMailCitaMedilink(string cliente,string producto,string medicamento)
        {

            mailsA = FunMailsAlternaMediLink();

            Array.Resize(ref objparam, 1);
            objparam[0] = 59;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);

            Array.Resize(ref objcitamedica, 18);
            objcitamedica[0] = cliente;
            objcitamedica[1] = producto;
            objcitamedica[2] = medicamento;
            objcitamedica[3] = codCitaPresta;
            objcitamedica[4] = ViewState["Ciudad"].ToString();
            objcitamedica[5] = ViewState["FechaCita"].ToString();
            objcitamedica[6] = ViewState["HoraMed"].ToString();
            objcitamedica[7] = ViewState["Sucursal"].ToString();
            objcitamedica[8] = direcsucursal;
            objcitamedica[9] = ViewState["Medico"].ToString();
            objcitamedica[10] = ViewState["Especialidad"].ToString();
            objcitamedica[11] = ViewState["Cedula"].ToString();
            objcitamedica[12] = ViewState["Titular"].ToString();
            objcitamedica[13] = ViewState["Nombres"].ToString();
            objcitamedica[14] = ViewState["Telefonos"].ToString();
            objcitamedica[15] = "3 USD";

            if (dt.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Tables[0].Rows)
                {
                    if (dr[0].ToString() == "PIE1") objcitamedica[16] = dr[1].ToString();
                    if (dr[0].ToString() == "PIE2") objcitamedica[17] = dr[1].ToString();
                }
            }

            fileTemplate = Server.MapPath("~/Template/HtmlTemplateMedilink.html");
            subject = "Medilink - " + "-" + producto;
            subject = subject.Replace('\r', ' ').Replace('\n', ' ');

            mensaje = new Funciones().funEnviarMailMediLink(subject, objcitamedica, fileTemplate,
                    ViewState["Host"].ToString(), int.Parse(ViewState["Port"].ToString()), bool.Parse(ViewState["EnableSSl"].ToString()),
                    ViewState["Usuario"].ToString(), ViewState["Password"].ToString(), mailsA);

        }

        private string FunMailsAlternaMediLink()
        {
            sendmails = "";
            int x = 0;
            Array.Resize(ref objsendmails, 3);
            objsendmails[0] = 0;
            objsendmails[1] = "MAILS MEDILINK";
            objsendmails[2] = 189;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objsendmails);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                sendmails = sendmails + dr[0].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(sendmails)) x = sendmails.LastIndexOf(",");
            if (x > 0) sendmails = sendmails.Remove(x, 1);
            return sendmails;
        }


        #endregion

        #region Eventos

        protected void ddlciudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calendar.SelectedDate = Convert.ToDateTime(DateTime.Now.ToString());
            ddlSucursal.Items.Clear();
            ddlEspecialidad.Items.Clear();
            ListItem espe;
            espe = new ListItem("--Seleccione Especialidad--", "0");
            ddlEspecialidad.Items.Add(espe);
            Calendar.Visible = false;
            lblHorarios.Visible = false;
            lstBoxMedicos.Visible = false;
            LstBoxHorario.Visible = false;
            btnCrearCita.Visible = false;
            codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            ddlTxtCiudad = ddlciudad.SelectedItem.ToString();
            ViewState["CodCiudad"] = codCiudad;
            ViewState["Ciudad"] = ddlTxtCiudad;

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
            Calendar.Visible = false;
            lblHorarios.Visible = false;
            lstBoxMedicos.Visible = false;
            LstBoxHorario.Visible = false;
            btnCrearCita.Visible = false;
            codSucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            ddlTxtSucursal = ddlSucursal.SelectedItem.ToString();
            ViewState["CodSucursal"] = codSucursal;
            ViewState["Sucursal"] = ddlTxtSucursal;
            FunGetEspe(codSucursal);
        }

        protected void ddlespeci_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["FechaCalendar"] = DateTime.Now.ToString("yyyyMMdd");
            ViewState["DiaCalendar"] = Calendar.SelectedDate.ToString("dddd");
            lstBoxMedicos.Items.Clear();
            LstBoxHorario.Visible = false;
            LstBoxHorario.Items.Clear();
            btnCrearCita.Visible = false;
            Calendar.SelectedDate = DateTime.Now.AddDays(-8);
            codEspe = int.Parse(ddlEspecialidad.SelectedValue.ToString());
            ddlTxtEspeci = ddlEspecialidad.SelectedItem.ToString();
            codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            codsucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            ViewState["CodEspecialidad"] = codEspe;
            ViewState["Especialidad"] = ddlTxtEspeci;
            Calendar.Visible = true;
            lblHorarios.Visible = true;
            lstBoxMedicos.Visible = true;
            DateTime dtmFechaCalendar = DateTime.ParseExact(Calendar.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string fechabloqueo = DateTime.Now.ToString("MM/dd/yyyy");
            if (fechabloqueo == "05/23/2025")
            {

                new Funciones().funShowJSMessage("Fecha no disponible para agendar" +" " + fechabloqueo, this);
                return;
            }
            FunDisponibilidades(codCiudad, codEspe, codsucursal, ViewState["FechaCalendar"].ToString());
        }

        protected void lstBoxMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstBoxHorario.Items.Clear();
            codMedico = int.Parse(lstBoxMedicos.SelectedItem.Value.ToString());
            string medico = lstBoxMedicos.SelectedItem.ToString();
            ViewState["CodMedico"] = codMedico;
            ViewState["Medico"] = medico;
            DataTable dttDisponibles = (DataTable)ViewState["DatosDisponibles"];
            if (dttDisponibles.ToString() != "") LstBoxHorario.Visible = true;

            DataRow[] _datorow = dttDisponibles.Select("codigoMedico='" + codMedico + "'");

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
            lstCodHoraMed = LstBoxHorario.SelectedItem.Value;
            lstTxthoraMed = LstBoxHorario.SelectedItem.ToString();
            ViewState["CodHoraMed"] = lstCodHoraMed;
            ViewState["HoraMed"] = lstTxthoraMed;
            btnCrearCita.Visible = true;
        }

        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            btnCrearCita.Visible = false;
            lstBoxMedicos.Items.Clear();
            LstBoxHorario.Items.Clear();
            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaCalendar = DateTime.ParseExact(Calendar.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaCita = DateTime.ParseExact(Calendar.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtmFechaCalendar < dtmFechaActual)
            {
                new Funciones().funShowJSMessage("Fecha seleccionada menor a la Actual!!", this);
                Calendar.SelectedDate = DateTime.Now.AddDays(-8);
                return;
            }

            TimeSpan _difdias = dtmFechaCalendar.Subtract(dtmFechaActual);

            int _dias = _difdias.Days;

            if (_dias > 10)
            {
                new Funciones().funShowJSMessage("No se puede agendar con tantos dias de Anticipacion..!!", this);
                return;
            }

            string fechabloqueo = dtmFechaCalendar.ToString("MM/dd/yyyy");
            if (fechabloqueo == "05/23/2025")
            {
                
                new Funciones().funShowJSMessage("Fecha no disponible", this);
                return;
            }

            fechaCalendar = dtmFechaCalendar.ToString("yyyyMMdd");
            fechaCita = dtmFechaCita.ToString("yyyy-MM-dd");
            diaCalendar = Calendar.SelectedDate.ToString("dddd");
            codCiudad = int.Parse(ddlciudad.SelectedValue.ToString());
            codEspe = int.Parse(ddlEspecialidad.SelectedValue.ToString());
            codSucursal = int.Parse(ddlSucursal.SelectedValue.ToString());
            ViewState["FechaCita"] = fechaCita;
            ViewState["FechaCalendar"] = fechaCalendar;
            ViewState["DiaCalendar"] = diaCalendar;

            FunDisponibilidades(codCiudad, codEspe, codSucursal, ViewState["FechaCalendar"].ToString());
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            FunAgendarCita(int.Parse(ViewState["IdMedilink"].ToString()), int.Parse(ViewState["CodCiudad"].ToString()), int.Parse(ViewState["CodMedico"].ToString()), int.Parse(ViewState["CodSucursal"].ToString()), int.Parse(ViewState["CodEspecialidad"].ToString()), int.Parse(ViewState["CodHoraMed"].ToString()));
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.opener.location='FrmAgendarCitaMedica?CodigoTitular=" + ViewState["CodTitular"].ToString() + "&CodigoProducto=" + ViewState["CodProducto"].ToString() + "&Regresar=0" + "';window.close();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);

        }
        #endregion
    }
}