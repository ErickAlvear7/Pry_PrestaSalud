﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pry_PrestasaludWAP.Api;
using System;
using System.Data;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using static Pry_PrestasaludWAP.Modelo.MediLinkModel;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmAgendarMediLink : System.Web.UI.Page
    {
        #region Variables
        string accessToken = "", fechaActual="";
        string idtitular = "", idbene="", idpro="", usuario="";
        string _idtitumed = "", _idbenemed;
        string userApikey = "", passApikey = "";
        string documento = "", fechanacimiento = "", parentesco = "", tipodocumento = "", nombresCompletos = "", nombre1 = "",nombre2 = "",
               apellido1 = "", apellido2 = "", genero = "", celular = "", telcasa = "", email = "", direccion = "", telefonos = "";
        string respVerifPacient = "", respRegisPacient="",respGetCiudad="",respGetSucur="",respGetEspe="",respGetMedico="",respGetDispo="",
               respCrearPacient = "",respAdmision="",respCrearCita="";
        string fechaCita = "";
        string _urlpro = "https://agendamiento.medilink.com.ec:8443/", _url = "https://testagendamiento.medilink.com.ec:443/";
        int _idresponVP = 0;
        int codCiudad = 0, codEspe = 0, codSucursal = 0, codMedico = 0;
        string sucursal = "", espe = "";
        
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
                    userApikey = dt.Tables[0].Rows[0][1].ToString().Trim();
                    passApikey = dt.Tables[0].Rows[1][1].ToString().Trim();
                }

                var login = new LoginApi
                {
                    username = userApikey,
                    password = passApikey
                };

                var data = new JavaScriptSerializer().Serialize(login);
                accessToken = new MediLinkApi().PostAccesLogin(_urlpro, data);

                Session["AccessToken"] = accessToken;
                fechaActual = DateTime.Now.ToString("yyyy-MM-dd");

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
                    //Array.Resize(ref objparam, 3);
                    //objparam[0] = int.Parse(idtitular);
                    //objparam[1] = "";
                    //objparam[2] = 185;
                    //dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                    //foreach (DataRow dr in dt.Tables[0].Rows)
                    //{
                    //    tipoidentificacion = dr[0].ToString();
                    //    documento = dr[1].ToString();
                    //    nombresCompletos = dr[2].ToString();
                    //    fechanacimiento = dr[3].ToString(); //yyyy-MM-dd
                    //    telcasa = dr[4].ToString();
                    //    telcelular = dr[5].ToString();
                    //}
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 1;
                    objparam[1] = int.Parse(idtitular);
                    objparam[2] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularBene", objparam);

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

                    ViewState["Cedula"] = documento;
                    ViewState["Titular"] = "TITULAR";
                    ViewState["Tipo"] = "T";
                    ViewState["Nombres"] = nombresCompletos;
                    ViewState["FechaNaci"] = fechanacimiento;
                    ViewState["Telefonos"] = telefonos;

                    //verificar si paciente esta registrado en MEDILINK
                    respVerifPacient = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);

                    if (respVerifPacient != "")
                    {
                        _idresponVP = int.Parse(respVerifPacient.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                       //INSERT ID O GET ID MEDILINK
                        Array.Resize(ref objparam, 6);
                        objparam[0] = 0;
                        objparam[1] = _idresponVP;
                        objparam[2] = int.Parse(idtitular); 
                        objparam[3] = 0;
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        _idtitumed = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["IdTituMed"] = _idtitumed;
                    }
                    else 
                    {
                                   
                        //REGISTRAR PACIENTE
                        respRegisPacient = FunRegistrarPaciente();
                        
                        if (respRegisPacient != "")
                        {
                            //GUARDAR ID BDD Expert_MEDILINK
                            _idtitumed = respRegisPacient;
                            
                            Array.Resize(ref objparam, 6);
                            objparam[0] = 0;
                            objparam[1] = int.Parse(_idtitumed); 
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
                        fechanacimiento = dr[8].ToString();//yyyy-MM-dd
                        parentesco = dr[9].ToString();
                        direccion = dr[10].ToString();
                        celular = dr[11].ToString();
                        email = dr[12].ToString();   
                    }

                    ViewState["FechaNaci"] = fechanacimiento;
                    ViewState["Parentesco"] = parentesco;
                    ViewState["Titular"] = "BENEFICIARIO";
                    ViewState["Tipo"] = "B";

                    lblDocumento.Text = documento;
                    lblNombresCompletos.Text = nombresCompletos;

                    respVerifPacient = new MediLinkApi().GetVerificarPaciente(_urlpro, accessToken, documento, tipodocumento);

                    if (respVerifPacient != "")
                    {
                        _idresponVP = int.Parse(respVerifPacient.ToString());
                        FunGetCiudad();
                        pnlOpciones.Visible = true;
                        lblRegistro.Text = "Activo";

                        //INSERT ID O GET ID MEDILINKBE
                        Array.Resize(ref objparam, 6);
                        objparam[0] = 1;
                        objparam[1] = _idresponVP;
                        objparam[2] = int.Parse(idtitular);
                        objparam[3] = int.Parse(idbene);
                        objparam[4] = int.Parse(idpro);
                        objparam[5] = usuario;
                        dt = new Conexion(2, "").funConsultarSqls("sp_InsertMedilink", objparam);
                        _idbenemed = dt.Tables[0].Rows[0][0].ToString();
                        ViewState["IdBeneMed"] = _idbenemed;
                    }
                    else
                    {
                        respRegisPacient = FunRegistrarPaciente();

                        if (respRegisPacient != "")
                        {
                            //GUARDAR ID BDD Expert_MEDILINK
                            _idbenemed = respRegisPacient;

                            Array.Resize(ref objparam, 6);
                            objparam[0] = 1;
                            objparam[1] = int.Parse(_idbenemed);
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

            respGetCiudad = new MediLinkApi().GetCiudad(_urlpro, Session["AccessToken"].ToString());
            //dynamic dataCiudad = JObject.Parse(response);
            if(respGetCiudad != "")
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
            respGetSucur = new MediLinkApi().GetSucursal(_urlpro, Session["AccessToken"].ToString(), codCiudad);

            if(respGetSucur != "")
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

            if(respGetEspe != "")
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
            respGetMedico = new MediLinkApi().GetMedicos(_urlpro, Session["AccessToken"].ToString(), codEspe, CodSucur);

            if(respGetMedico != "")
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
         private void FunDisponibilidades(int codCiudad,int codEspeci,int codSucur, string fechacita)
         {
            string fechanew = DateTime.ParseExact(fechacita, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            respGetDispo = new MediLinkApi().GetDisponibilidad(_urlpro, Session["AccessToken"].ToString(), codCiudad, codEspeci, codSucur, fechanew);
            if (respGetDispo != "") lstBoxMedicos.Visible = true;

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
            respCrearCita = new MediLinkApi().PostCrearCita(_urlpro, Session["AccessToken"].ToString(), cita);

            if(respCrearCita != "")
            {
                dynamic datoscita = JObject.Parse(respCrearCita);
                string codigo = datoscita.datos.codigoCita;

                Array.Resize(ref objparam, 13);
                objparam[0] = 0;
                objparam[1] = 0; //CODIGO PRESTADOR
                objparam[2] = 0;//CODIGO MEDICO
                objparam[3] = ViewState["Tipo"].ToString();
                objparam[4] = 0; //CODIGO TITULAR
                objparam[5] = 0;//CODIGO BENE
                objparam[6] = ViewState["Parentesco"].ToString(); // PARENTESCO
                objparam[7] = fechaCita;
                objparam[8] = "";//DIA CITA
                objparam[9] = ViewState["Hora"].ToString();
                objparam[10] = int.Parse(Session["usuCodigo"].ToString());
                objparam[11] = Session["MachineName"].ToString();
                objparam[12] = "";//OBSERVACION
                DataSet ds = new Conexion(2, "").FunCodigoCitaMedilink(objparam);
                int codCita = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                txtTitCita.Visible = true;
                txtCodCita.Visible = true;
                lblCodigo.Text = codCita.ToString();
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
                new Funciones().funShowJSMessage("No se Agendo Cita", this);
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
            FunDisponibilidades(codCiudad, codEspe, codsucursal, fechaActual);
        }

        protected void lstBoxMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstBoxHorario.Items.Clear();
            codMedico = int.Parse(lstBoxMedicos.SelectedItem.Value.ToString());
            string medico = lstBoxMedicos.SelectedItem.ToString();
            ViewState["codMedico"] = codMedico;
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
            string _codHoraMed = LstBoxHorario.SelectedItem.Value;
            string hora = LstBoxHorario.SelectedItem.ToString();
            ViewState["codHoraMed"] = _codHoraMed;
            ViewState["Hora"] = hora;
            btnCrearCita.Visible = true;
        }

        protected void btnAgendar_Click(object sender, EventArgs e)
        {
            fechaCita = fechaActual;

            FunAgendarCita(int.Parse(ViewState["idPaciente"].ToString()), int.Parse(ViewState["codCiudad"].ToString()), int.Parse(ViewState["codMedico"].ToString()), int.Parse(ViewState["codSucursal"].ToString()), int.Parse(ViewState["codEspecialidad"].ToString()), int.Parse(ViewState["codHoraMed"].ToString()), fechaCita);

        }
        #endregion
    }
}