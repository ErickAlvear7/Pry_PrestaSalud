﻿using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmAgendarCitaMedica : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataSet dts = new DataSet();
        DataSet dtusu = new DataSet();
        DataSet dtsexa = new DataSet();
        Object[] objparam = new Object[1];
        Object[] objcitamedica = new Object[23];
        Object[] objdatostitu = new Object[4];
        Object[] objdatosmotivo = new Object[3];
        Object[] objdatoscancel = new Object[11];
        Object[] objsendmails = new Object[3];
        Object[] objsendsms = new Object[3];
        Object[] objconsulta = new Object[3];
        Object[] objparamdirecpre = new Object[3];
        DataTable tbDatosCita = new DataTable();
        DataTable tbCitaMedica = new DataTable();
        DataTable tbNuevaCitaMedica = new DataTable();
        DataTable tbMailCitaMedica = new DataTable();
        ListItem presta = new ListItem();
        ListItem espe = new ListItem();
        ListItem medi = new ListItem();
        ListItem ciud = new ListItem();
        ImageButton btnEstado = new ImageButton();
        CheckBox chkCancelar = new CheckBox();
        TimeSpan horaActual, horaAgenda, intervalo, intervaloHoras;
        string horaSistema = "", fechaActual = "", Calendario = "", returnFile = "",
                fileTemplate = "", subject = "", agenda = "", status = "", mensaje = "", fileLogo = "", mailsP = "",
                mailsA = "", mailsD = "", sendmails = "", medicamentos = "", mailsU = "", codigo = "", descripcion = "",
                textoSMS = "", trama = "", celular = "", usuarioSMS = "", passSMS = "", alerta = "", tipofecha = "", sql = "",
                filePath1 = "", filename1 = "", ext1 = "", type1 = "", name1 = "", _enviarsms = "";
        CheckBox chkselecc = new CheckBox();
        int codigocita = 0, contar = 0, x = 0, asisanual = 0, asismensual = 0, aniocita = 0, mescita = 0, citasmes = 0, citasanual = 0;
        DateTime fechaCita, fechaFinCobertura, fechaSistema;
        Thread thrEnviarSMS;
        Byte[] bytes;
        decimal totalLAB = 0;

        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-EC");
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                CalendarioCita.SelectedDate = DateTime.Today;
                tbDatosCita.Columns.Add("Hora");
                tbDatosCita.Columns.Add("Accion");
                tbDatosCita.Columns.Add("Cliente");
                tbDatosCita.Columns.Add("Tipo");
                tbDatosCita.Columns.Add("Usuario");
                tbDatosCita.Columns.Add("Parentesco");
                tbDatosCita.Columns.Add("HodeCodigo");
                tbDatosCita.Columns.Add("TumeCodigo");
                ViewState["tbDatosCita"] = tbDatosCita;
                grdvDatosCitas.DataSource = tbDatosCita;
                grdvDatosCitas.DataBind();

                tbCitaMedica.Columns.Add("Cliente");
                tbCitaMedica.Columns.Add("Ciudad");
                tbCitaMedica.Columns.Add("Prestadora");
                tbCitaMedica.Columns.Add("Medico");
                tbCitaMedica.Columns.Add("Especialidad");
                tbCitaMedica.Columns.Add("FechaCita");
                tbCitaMedica.Columns.Add("DiaCita");
                tbCitaMedica.Columns.Add("Hora");
                tbCitaMedica.Columns.Add("Detalle");
                tbCitaMedica.Columns.Add("PreeCodigo");
                tbCitaMedica.Columns.Add("MediCodigo");
                tbCitaMedica.Columns.Add("HodeCodigo");
                tbCitaMedica.Columns.Add("TipoCliente");
                tbCitaMedica.Columns.Add("TituCodigo");
                tbCitaMedica.Columns.Add("BeneCodigo");
                tbCitaMedica.Columns.Add("CodParentesco");
                tbCitaMedica.Columns.Add("EstatusCita");
                tbCitaMedica.Columns.Add("Longitud");
                tbCitaMedica.Columns.Add("Latitud");
                tbCitaMedica.Columns.Add("CodigoPrestadora");
                tbCitaMedica.Columns.Add("Observacion");
                ViewState["tbCitaMedica"] = tbCitaMedica;
                ViewState["Mgeneral"] = 0;
                ViewState["Especialidad"] = 0;
                ViewState["TotalLab"] = 0;
                ViewState["TotalRestante"] = 0;

                lbltitulo.Text = "Agendar Cita";
                //Session["CodigoTitular"] = Request["CodigoTitular"];
                //Session["CodigoProducto"] = Request["CodigoProducto"];
                Session["CodigoTitular"] = Request["CodigoTitular"];
                Session["CodigoProducto"] = Request["CodigoProducto"];
                Session["Regresar"] = Request["Regresar"];
                Session["TipoCita"] = "CitaMedica";
                ViewState["Intervalo"] = 0;
                Session["codigocita"] = 0;
                FunContadorCitas();
                FunValidarEspe();
                FunCargaMantenimiento();
                FunHistorialCitas();
                FunEliminarReservas();
                FunGetReservas();
                FunCascadaCombos(2);
                FunCascadaCombos(0);
                FunCascadaCombos(1);
                FunCascadaCombos(3);
                FunCascadaCombos(4);
                FunCascadaCombos(5);
                FunCascadaCombos(6);
                FunCascadaCombos(7);
                FunCascadaCombos(8);
                pnlDatosPersonales.Height = 120;
                pnlInfAdicional.Height = 120;
                if (ViewState["Host"] == null || ViewState["Port"] == null || ViewState["EnableSSl"] == null || ViewState["Usuario"] == null || ViewState["Password"] == null)
                {
                    lblerror.Text = "No ha Definido Parámetros para enviar Mails...";
                    ddlOpcion.Enabled = false;
                    imgAgendar.Enabled = false;
                }
                if (ViewState["Ruta"] == null || ViewState["Logo"] == null)
                {
                    lblerror.Text = "No ha Definido Ruta/Logo para enviar Mails...";
                    ddlOpcion.Enabled = false;
                    imgAgendar.Enabled = false;
                }
                if (ViewState["Secuencial"] == null)
                {
                    lblerror.Text = "No ha Definido Secuencial para el Producto...";
                    ddlOpcion.Enabled = false;
                    imgAgendar.Enabled = false;
                }
                
                if(Session["Regresar"] != null)
                {
                    if (Session["Regresar"].ToString() == "1") TrFileUpload.Visible = true;
                }
                
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Funciones y Procedimientos
        private void FunCargaMantenimiento()
        {
            try
            {
                //LLENAR COMBOS
                Array.Resize(ref objparam, 1);
                objparam[0] = 12;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    if (new Funciones().IsNumber(dt.Tables[0].Rows[0][0].ToString()))
                    {
                        ViewState["Intervalo"] = dt.Tables[0].Rows[0][0].ToString();
                    }
                    else ViewState["Intervalo"] = 0;
                }
                Array.Resize(ref objparam, 4);
                objparam[0] = 4;
                objparam[1] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[2] = 0;
                objparam[3] = int.Parse(Session["CodigoProducto"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularEdit", objparam);
                grdvDatosPersonales.DataSource = dt.Tables[0];
                grdvDatosPersonales.DataBind();
                ViewState["Indentificacion"] = dt.Tables[0].Rows[0]["Identificacion"].ToString();
                lblCelular.InnerText = dt.Tables[0].Rows[0]["Celular"].ToString();
                if (dt.Tables[0].Rows.Count > 0)
                {
                    lblCelular.InnerText = dt.Tables[0].Rows[0]["Celular"].ToString();
                    ViewState["FechaIniCobertura"] = dt.Tables[0].Rows[0]["FechaIniCobertura"].ToString();
                    ViewState["FechaFinCobertura"] = dt.Tables[0].Rows[0]["FechaFinCobertura"].ToString();
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    grdvTitulares.DataSource = dt.Tables[1];
                    grdvTitulares.DataBind();
                }
                objparam[0] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularEdit", objparam);
                ViewState["Campaing"] = dt.Tables[0].Rows[0][0].ToString();
                ViewState["Producto"] = dt.Tables[0].Rows[0][1].ToString();
                ViewState["Grupo"] = dt.Tables[0].Rows[0][2].ToString();
                ViewState["CodigoGrupo"] = dt.Tables[0].Rows[0][3].ToString();
                Array.Resize(ref objparam, 1);
                objparam[0] = 13;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    ViewState["Host"] = dt.Tables[0].Rows[0][0].ToString().Trim().ToLower();
                    ViewState["Port"] = dt.Tables[0].Rows[1][0].ToString().Trim().ToLower();
                    ViewState["EnableSSl"] = dt.Tables[0].Rows[2][0].ToString().Trim().ToLower();
                    ViewState["Usuario"] = dt.Tables[0].Rows[3][0].ToString().Trim().ToLower();
                    ViewState["Password"] = dt.Tables[0].Rows[4][0].ToString().Trim();
                }
                objparam[0] = 17;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    ViewState["Ruta"] = dt.Tables[0].Rows[0][1].ToString().Trim();
                    ViewState["Logo"] = dt.Tables[0].Rows[1][1].ToString().Trim();
                }
                lbltitulo.Text = "Agendar Cita Producto: " + ViewState["Producto"].ToString();

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[1] = "";
                objparam[2] = 102;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) ViewState["Secuencial"] = dt.Tables[0].Rows[0][0].ToString();
                //CODIGO DE CAMPAING
                objparam[0] = int.Parse(Session["CodigoProducto"].ToString());
                objparam[2] = 109;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["CodigoCamp"] = dt.Tables[0].Rows[0][0].ToString();
                objparam[0] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[2] = 128;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatosAdicionales.DataSource = dt.Tables[0];
                grdvDatosAdicionales.DataBind();
                //PONER INFORMACION ADICIONAL
                //Array.Resize(ref objparam, 11);
                //objparam[0] = 7;
                //objparam[1] = "";
                //objparam[2] = "";
                //objparam[3] = "";
                //objparam[4] = "";
                //objparam[5] = "";
                //objparam[6] = int.Parse(Session["CodigoProducto"].ToString());
                //objparam[7] = 0;
                //objparam[8] = 0;
                //objparam[9] = 0;
                //objparam[10] = 0;
                //dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                //if (dt.Tables[0].Rows.Count > 0)
                //{
                //    sql = "Select ";
                //    foreach (DataRow dr in dt.Tables[0].Rows)
                //    {
                //        if (dr["Ver"].ToString() == "SI") sql += dr["Cabecera"] + "=" + dr["Campo"].ToString() + ",";
                //    }
                //    sql = sql.Remove(sql.Length - 1) + " from Expert_INFORMACION_ADICIONAL INF INNER JOIN Expert_TITULAR TIT ON INF.PERS_CODIGO=TIT.PERS_CODIGO where TIT.TITU_CODIGO=" + Session["CodigoTitular"].ToString();
                //    objparam[0] = 8;
                //    objparam[1] = sql;
                //    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                //    grdvDatosAdicionales.DataSource = dt.Tables[0];
                //    grdvDatosAdicionales.DataBind();
                //}
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void FunHistorialCitas()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[1] = "";
                objparam[2] = 32;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    grdvHistorialCitas.DataSource = dt;
                    grdvHistorialCitas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void FunContadorCitas()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = int.Parse(Session["CodigoTitular"].ToString()); ;
                objparam[2] = 173;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                ViewState["Mgeneral"] = dt.Tables[0].Rows[0][1].ToString();
                ViewState["Especialidad"] = dt.Tables[0].Rows[0][2].ToString();

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    grdvContadorCitas.DataSource = dt;
                    grdvContadorCitas.DataBind();
                }

                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[2] = 176;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                labh3.Visible = false;

                string total = dt.Tables[0].Rows[0][0].ToString();

                if(total != "")
                {
                  
                    labh3.Visible = true;
                    grdvSumaLaboratorio.DataSource = dt;
                    grdvSumaLaboratorio.DataBind();         

                }

                //if (dt != null && dt.Tables[0].Rows.Count > 0)
                //{

                //    labh3.Visible = true;
                //    grdvSumaLaboratorio.DataSource = dt;
                //    grdvSumaLaboratorio.DataBind();
                //}

            }
            catch(Exception ex)
            {
                lblerror.Text = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "frmCitaMedica.cs/FunContadorCitas", ex.ToString(), 1);
            }
        
        }

        private void FunValidarEspe()
        {
            try
            {
               
                Array.Resize(ref objparam, 3);
                objparam[0] = 1; //Medicina General 
                objparam[1] = int.Parse(Session["CodigoTitular"].ToString()); ;
                objparam[2] = 174;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                int cmg = int.Parse(dt.Tables[0].Rows[0][0].ToString());

                if (cmg == 4)
                {
                    new Funciones().funShowJSMessage("Revise # Citas Agendadas!!..", this);

                }

                ViewState["ContMG"] = cmg;

                Array.Resize(ref objparam, 3);
                objparam[0] = 2; //Especialidades
                objparam[1] = int.Parse(Session["CodigoTitular"].ToString()); ;
                objparam[2] = 174;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                int cesp = int.Parse(dt.Tables[0].Rows[0][0].ToString());

                if (cesp == 3)
                {
                    new Funciones().funShowJSMessage("Revise # Citas Agendadas!!..", this);
                }

                ViewState["ContESP"] = cesp;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "frmCitaMedica.cs/FunValidarEspe", ex.ToString(), 1);
            }

        }

        private void FunEliminarReservas()
        {
            try
            {
                //BORRAR LAS CITAS QUE QUEDARON RESERVADAS Y SE PASARON DE LA FECHA-HORA
                Array.Resize(ref objparam, 18);
                objparam[0] = 5;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                objparam[5] = CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper();
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = 0;
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "frmCitaMedica.cs", ex.ToString(), 1);
            }
        }

        private void FunCascadaCombos(int opcion)
        {
            if (Session["Perfil"].ToString() == "NOVA")
            {
                switch (opcion)
                {
                    case 0:
                        ddlPrestadora.Items.Clear();
                        presta.Text = "--Seleccione Prestadora--";
                        presta.Value = "0";
                        ddlPrestadora.Items.Add(presta);

                        ddlEspecialidad.Items.Clear();
                        espe.Text = "--Seleccione Especialidad--";
                        espe.Value = "0";
                        ddlEspecialidad.Items.Add(espe);

                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);

                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(ddlProvincia.SelectedValue);
                        objparam[1] = "";
                        objparam[2] = 30;
                        ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        ddlCiudad.DataTextField = "Descripcion";
                        ddlCiudad.DataValueField = "Codigo";
                        ddlCiudad.DataBind();

                        break;
                    case 1:
                        ddlPrestadora.Items.Clear();
                        presta.Text = "--Seleccione Prestadora--";
                        presta.Value = "0";
                        ddlPrestadora.Items.Add(presta);

                        ddlEspecialidad.Items.Clear();
                        espe.Text = "--Seleccione Especialidad--";
                        espe.Value = "0";
                        ddlEspecialidad.Items.Add(espe);

                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);
                        Array.Resize(ref objparam, 11);
                        objparam[0] = 34;
                        objparam[1] = "";
                        objparam[2] = "ONLINE";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = ddlCiudad.SelectedValue;
                        objparam[7] = 0;
                        objparam[8] = 0;
                        objparam[9] = 0;
                        objparam[10] = 0;
                        ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                        ddlPrestadora.DataTextField = "Descripcion";
                        ddlPrestadora.DataValueField = "Codigo";
                        ddlPrestadora.DataBind();
                        break;
                    case 2:
                        ddlCiudad.Items.Clear();
                        ddlPrestadora.Items.Clear();
                        ddlEspecialidad.Items.Clear();
                        ddlMedico.Items.Clear();
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 10;
                        ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlProvincia.DataTextField = "Descripcion";
                        ddlProvincia.DataValueField = "Codigo";
                        ddlProvincia.DataBind();
                        break;
                    case 3:
                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);
                        Array.Resize(ref objparam, 11);
                        objparam[0] = 35;
                        objparam[1] = "";
                        objparam[2] = "ONLINE";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = ddlPrestadora.SelectedValue;
                        objparam[7] = 0;
                        objparam[8] = 0;
                        objparam[9] = 0;
                        objparam[10] = 0;
                        ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                        ddlEspecialidad.DataTextField = "Descripcion";
                        ddlEspecialidad.DataValueField = "Codigo";
                        ddlEspecialidad.DataBind();
                        break;
                    case 4:
                        Array.Resize(ref objparam, 11);
                        objparam[0] = 36;
                        objparam[1] = "";
                        objparam[2] = "ONLINE";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = ddlEspecialidad.SelectedValue;
                        objparam[7] = 0;
                        objparam[8] = 0;
                        objparam[9] = 0;
                        objparam[10] = 0;
                        ddlMedico.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                        ddlMedico.DataTextField = "Descripcion";
                        ddlMedico.DataValueField = "Codigo";
                        ddlMedico.DataBind();
                        break;
                    case 5:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 11;
                        ddlOpcion.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlOpcion.DataTextField = "Descripcion";
                        ddlOpcion.DataValueField = "Codigo";
                        ddlOpcion.DataBind();
                        break;
                    case 6:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 15;
                        ddlMotivoCancelar.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlMotivoCancelar.DataTextField = "Descripcion";
                        ddlMotivoCancelar.DataValueField = "Codigo";
                        ddlMotivoCancelar.DataBind();
                        break;
                    case 7:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 56;
                        ddlMotivoCita.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlMotivoCita.DataTextField = "Descripcion";
                        ddlMotivoCita.DataValueField = "Codigo";
                        ddlMotivoCita.DataBind();
                        break;
                    case 8:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 55;
                        ddlTipoPago.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlTipoPago.DataTextField = "Descripcion";
                        ddlTipoPago.DataValueField = "Codigo";
                        ddlTipoPago.DataBind();
                        ddlTipoPago.SelectedIndex = 2;

                        break;
                }

            }
            else
            {
                switch (opcion)
                {
                    case 0:
                        ddlPrestadora.Items.Clear();
                        presta.Text = "--Seleccione Prestadora--";
                        presta.Value = "0";
                        ddlPrestadora.Items.Add(presta);

                        ddlEspecialidad.Items.Clear();
                        espe.Text = "--Seleccione Especialidad--";
                        espe.Value = "0";
                        ddlEspecialidad.Items.Add(espe);

                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);

                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(ddlProvincia.SelectedValue);
                        objparam[1] = "";
                        objparam[2] = 30;
                        ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        ddlCiudad.DataTextField = "Descripcion";
                        ddlCiudad.DataValueField = "Codigo";
                        ddlCiudad.DataBind();

                        break;
                    case 1:
                        ddlPrestadora.Items.Clear();
                        presta.Text = "--Seleccione Prestadora--";
                        presta.Value = "0";
                        ddlPrestadora.Items.Add(presta);

                        ddlEspecialidad.Items.Clear();
                        espe.Text = "--Seleccione Especialidad--";
                        espe.Value = "0";
                        ddlEspecialidad.Items.Add(espe);

                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);
                        Array.Resize(ref objparam, 3);
                        objparam[0] = ddlCiudad.SelectedValue;
                        objparam[1] = "";
                        objparam[2] = 5;
                        ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        ddlPrestadora.DataTextField = "Descripcion";
                        ddlPrestadora.DataValueField = "Codigo";
                        ddlPrestadora.DataBind();
                        break;
                    case 2:
                        ddlCiudad.Items.Clear();
                        ddlPrestadora.Items.Clear();
                        ddlEspecialidad.Items.Clear();
                        ddlMedico.Items.Clear();
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 10;
                        ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlProvincia.DataTextField = "Descripcion";
                        ddlProvincia.DataValueField = "Codigo";
                        ddlProvincia.DataBind();
                        break;
                    case 3:
                        ddlMedico.Items.Clear();
                        medi.Text = "--Seleccione Médico--";
                        medi.Value = "0";
                        ddlMedico.Items.Add(medi);
                        Array.Resize(ref objparam, 3);
                        objparam[0] = ddlPrestadora.SelectedValue;
                        objparam[1] = "";
                        objparam[2] = 8;
                        ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        ddlEspecialidad.DataTextField = "Descripcion";
                        ddlEspecialidad.DataValueField = "Codigo";
                        ddlEspecialidad.DataBind();
                        break;
                    case 4:
                        Array.Resize(ref objparam, 3);
                        objparam[0] = int.Parse(ddlEspecialidad.SelectedValue);
                        objparam[1] = "";
                        objparam[2] = 31;
                        ddlMedico.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        ddlMedico.DataTextField = "Descripcion";
                        ddlMedico.DataValueField = "Codigo";
                        ddlMedico.DataBind();
                        break;
                    case 5:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 11;
                        ddlOpcion.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlOpcion.DataTextField = "Descripcion";
                        ddlOpcion.DataValueField = "Codigo";
                        ddlOpcion.DataBind();
                        break;
                    case 6:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 15;
                        ddlMotivoCancelar.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlMotivoCancelar.DataTextField = "Descripcion";
                        ddlMotivoCancelar.DataValueField = "Codigo";
                        ddlMotivoCancelar.DataBind();
                        break;
                    case 7:
                        Array.Resize(ref objparam, 1);
                        objparam[0] = 19;
                        ddlMotivoCita.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                        ddlMotivoCita.DataTextField = "Descripcion";
                        ddlMotivoCita.DataValueField = "Codigo";
                        ddlMotivoCita.DataBind();
                        break;
                    case 8:
                        int producto = int.Parse(Session["CodigoProducto"].ToString());
                        if (producto == 225 || producto == 226 || producto == 227)
                        {
                            Array.Resize(ref objparam, 1);
                            objparam[0] = 55;
                            ddlTipoPago.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                            ddlTipoPago.DataTextField = "Descripcion";
                            ddlTipoPago.DataValueField = "Codigo";
                            ddlTipoPago.DataBind();
                            ddlTipoPago.SelectedIndex = 2;

                        }
                        else
                        {
                            Array.Resize(ref objparam, 1);
                            objparam[0] = 55;
                            ddlTipoPago.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                            ddlTipoPago.DataTextField = "Descripcion";
                            ddlTipoPago.DataValueField = "Codigo";
                            ddlTipoPago.DataBind();
                            ddlTipoPago.SelectedIndex = 1;
                        }
                      
                        

                        break;
                }
            }
        }
        private void FunAgendaHoras(int tipo, int preecodigo, int codmedico, string fechachita, string diacita)
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 0;
                objparam[1] = int.Parse(ddlEspecialidad.SelectedValue);
                objparam[2] = codmedico;
                objparam[3] = 0;
                objparam[4] = fechachita;
                objparam[5] = diacita.Trim().ToUpper();
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = 0;
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatosCitas.DataSource = dt;
                    grdvDatosCitas.DataBind();
                    ViewState["tbDatosCita"] = dt.Tables[0];
                }
                else
                {
                    tbDatosCita.Clear();
                    ViewState["tbDatosCita"] = tbDatosCita;
                    grdvDatosCitas.DataSource = tbDatosCita;
                    grdvDatosCitas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void FunGetReservas()
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 6;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = 0;
                objparam[9] = "";
                objparam[10] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ViewState["FechaCita"] = dt.Tables[0].Rows[0]["FechaCita"].ToString();
                    ViewState["HoraCita"] = dt.Tables[0].Rows[0]["Hora"].ToString();
                    grdvResumenCita.DataSource = dt;
                    grdvResumenCita.DataBind();
                    ViewState["tbCitaMedica"] = dt.Tables[0];
                    if (dt.Tables[0].Rows.Count > 0) Session["SalirAgenda"] = "NO";
                    else Session["SalirAgenda"] = "SI";
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        private void FunLimpiarCampos()
        {
            lblerror.Text = "";
            CalendarioCita.SelectedDate = DateTime.Today;
            tbDatosCita.Clear();
            ViewState["tbDatosCita"] = tbDatosCita;
            grdvDatosCitas.DataSource = tbDatosCita;
            grdvDatosCitas.DataBind();
        }
        private void FunLimpiarAgendamiento()
        {
            foreach (GridViewRow row in grdvTitulares.Rows)
            {
                (row.FindControl("chkSeleccionar") as CheckBox).Checked = false;
            }
            ddlMotivoCita.SelectedValue = "0";
            txtObservacion.Text = "";
        }

        private void FunEliminarReservasOtraOpcion()
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 7;
                objparam[1] = 0;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = 0;
                objparam[9] = "";
                objparam[10] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                tbCitaMedica.Clear();
                ViewState["tbCitaMedica"] = tbCitaMedica;
                grdvResumenCita.DataSource = tbCitaMedica;
                grdvResumenCita.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void FunEnviarMailCita(DataTable tblCitaMedica, string parametro1)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 107;
                dtusu = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                mailsU = dtusu.Tables[0].Rows[0][0].ToString();
                //Thread.Sleep(300);

                Array.Resize(ref objcitamedica, 24);

                objcitamedica[16] = "";
                objcitamedica[17] = "";
                objcitamedica[18] = "";
                objcitamedica[19] = "";
                Array.Resize(ref objparam, 1);
                objparam[0] = 16;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        if (dr[0].ToString() == "PIE1") objcitamedica[16] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE2") objcitamedica[17] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE3") objcitamedica[18] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE4") objcitamedica[19] = dr[1].ToString();
                    }
                }
                //filePath = Server.MapPath("~" + "\\CitasAgendadas\\");
                fileTemplate = Server.MapPath("~/Template/HtmlTemplate.html");
                fileLogo = @ViewState["Ruta"].ToString() + ViewState["Logo"].ToString();
                subject = "Agendamiento - " + ViewState["Campaing"].ToString() + "-" + ViewState["Producto"].ToString();
                subject = subject.Replace('\r', ' ').Replace('\n', ' ');
                Array.Resize(ref objparam, 18);
                objparam[0] = 8;
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                objcitamedica[0] = ViewState["Campaing"].ToString();
                objcitamedica[1] = ViewState["Producto"].ToString();
                foreach (DataRow dr in tblCitaMedica.Rows)
                {
                    mailsP = FunMailsEnviar(int.Parse(dr[19].ToString()));
                    if (string.IsNullOrEmpty(mailsP))
                    {
                        mensaje = "Hubo errores en el envío, Defina mails de la Prestadora..!";
                        lblerror.Text = "Defina mails de envío de la Prestadora..!";
                        break;
                    }
                    //Traer mail del usuario, si esta vacio coger los alternativos
                    mailsA = FunMailsAlterna();
                    mailsD = FunMailsMedicos(int.Parse(dr[10].ToString()));
                    objparam[1] = int.Parse(dr[9].ToString());
                    objparam[2] = int.Parse(dr[10].ToString());
                    objparam[3] = 0;
                    objparam[4] = dr[5].ToString();
                    ViewState["FechaCita"] = objparam[4].ToString();
                    DateTime fecha = DateTime.ParseExact(objparam[4].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string newFecha = fecha.ToString("dd/MM/yyyy");
                    //string newFecha = DateTime.ParseExact(fecha.ToString("MM/dd/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString();

                    Array.Resize(ref objparamdirecpre, 3);
                    objparamdirecpre[0] = objparam[1];
                    objparamdirecpre[1] = "";
                    objparamdirecpre[2] = 180;
                    
                    DataSet dtdirec = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparamdirecpre);
                    string direcpres = dtdirec.Tables[0].Rows[0][0].ToString();


                    objparam[5] = dr[6].ToString();
                    objparam[6] = dr[7].ToString();
                    ViewState["HoraCita"] = objparam[6].ToString();
                    objparam[7] = "";
                    objparam[8] = int.Parse(dr[11].ToString());
                    objparam[9] = dr[12].ToString(); ;
                    objparam[10] = int.Parse(dr[13].ToString());
                    objparam[11] = int.Parse(dr[14].ToString());
                    objparam[12] = dr[15].ToString(); ;
                    codigocita = new Conexion(2, "").FunGetCodigoCita(objparam);
                    ViewState["CodigoCita"] = codigocita.ToString();
                    medicamentos = FunGetMedicamento(int.Parse(ViewState["CodigoGrupo"].ToString()));
                    objcitamedica[2] = codigocita;
                    objcitamedica[3] = dr[1].ToString();
                    ViewState["Ciudad"] = objcitamedica[3].ToString();
                    objcitamedica[4] = dr[5].ToString();
                    objcitamedica[5] = dr[7].ToString();
                    objcitamedica[6] = dr[2].ToString();
                    ViewState["Prestadora"] = objcitamedica[6].ToString();
                    objcitamedica[7] = dr[3].ToString().Replace("&#209;", "Ñ");
                    ViewState["Medico"] = objcitamedica[7].ToString();
                    objcitamedica[8] = dr[4].ToString();
                    ViewState["Especialidad"] = objcitamedica[8].ToString();
                    objcitamedica[9] = FunGetMotivo(dr[8].ToString());
                    objcitamedica[10] = ViewState["Indentificacion"].ToString();
                    objcitamedica[11] = dr[12].ToString() == "T" ? "TITULAR" : "BENEFICIARIO";
                    objcitamedica[12] = dr[0].ToString().Replace("&#209;", "Ñ");
                    ViewState["Paciente"] = objcitamedica[12].ToString();
                    objdatostitu[0] = dr[12].ToString() == "T" ? 2 : 3;
                    objdatostitu[1] = dr[13].ToString();
                    objdatostitu[2] = dr[14].ToString();
                    objdatostitu[3] = 0;
                    dt = new Conexion(2, "").FunGetDatosTituBene(objdatostitu);
                    objcitamedica[13] = dt.Tables[0].Rows[0][0].ToString();
                    //objcitamedica[13].ToString();
                    DateTime FechaNaci = DateTime.ParseExact(objcitamedica[13].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string newFechaNaci = FechaNaci.ToString("dd/MM/yyyy");

                    objcitamedica[14] = direcpres;
                    //objcitamedica[15] = "";
                    objcitamedica[15] = dt.Tables[0].Rows[0][3].ToString();
                    objcitamedica[20] = medicamentos;
                    objcitamedica[21] = Session["usuLogin"].ToString();
                    objcitamedica[22] = dr[20].ToString();
                    objcitamedica[23] = parametro1;
                    if (!string.IsNullOrEmpty(lblCelular.InnerText.Trim()))
                    {
                        thrEnviarSMS = new Thread(new ThreadStart(FunEnviarSMS));
                        thrEnviarSMS.Start();
                    }

                    //nameFile = filePath + "CitaMedica_" + dr[5].ToString().Replace("/", "") + "_" + codigocita.ToString() + ".txt";
                    //returnFile = new Funciones().funCrearArchivoCita(nameFile, objcitamedica);

                    mensaje = new Funciones().funEnviarMail(mailsP, subject, objcitamedica, fileTemplate,
                        ViewState["Host"].ToString(), int.Parse(ViewState["Port"].ToString()), bool.Parse(ViewState["EnableSSl"].ToString()),
                        ViewState["Usuario"].ToString(), ViewState["Password"].ToString(), returnFile, fileLogo, mailsA, mailsD, mailsU, newFecha, newFechaNaci);
                    
                }

                //mensaje = "";
                if (mensaje == "")
                {
                    Session["codigocita"] = int.Parse(ViewState["CodigoCitapop"].ToString());

                    if (Session["Regresar"].ToString() == "0")
                        Response.Redirect("FrmCitaMedicaAdmin.aspx?MensajeRetornado=Cita(s) Agendada(s) con Éxito", true);
                    else
                        Response.Redirect("~/Examenes/FrmSolicitudOperadorAdmin.aspx?MensajeRetornado='Cita(s) Agendada(s) con Éxito'", true);
                }
                else
                {
                    if (Session["Regresar"].ToString() == "0")
                        Response.Redirect("FrmCitaMedicaAdmin.aspx?MensajeRetornado=Revise Mails, hubo errores en el envío..!", true);
                    else
                        Response.Redirect("~/Examenes/FrmSolicitudOperadorAdmin.aspx?MensajeRetornado=Revise Mails, hubo errores en el envío..!", true);
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void FunEnviarSMS()
        {

            DateTime fechaSMS = DateTime.ParseExact(ViewState["FechaCita"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            string newFechasMms = fechaSMS.ToString("dd/MM/yyyy");

            try
            {
                //Array.Resize(ref objsendsms, 3);
                //objconsulta[0] = int.Parse(ViewState["CodigoCamp"].ToString());
                //objconsulta[1] = "";
                //objconsulta[2] = 110;
                //dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objconsulta);
                //if (dt.Tables[0].Rows.Count > 0)
                //{
                //    textoSMS = dt.Tables[0].Rows[0][0].ToString();
                //    Array.Resize(ref objsendsms, 20);
                //    objsendsms[0] = 2;
                //    objsendsms[1] = 0;
                //    objsendsms[2] = 0;
                //    objsendsms[3] = ViewState["Prestadora"].ToString();
                //    objsendsms[4] = ViewState["Ciudad"].ToString();
                //    objsendsms[5] = lblCelular.InnerText.Trim();
                //    objsendsms[6] = ViewState["Medico"].ToString();
                //    objsendsms[7] = ViewState["Especialidad"].ToString();
                //    objsendsms[8] = ViewState["Paciente"].ToString();
                //    objsendsms[9] = ViewState["CodigoCita"].ToString();
                //    objsendsms[10] = ViewState["FechaCita"].ToString();
                //    objsendsms[11] = ViewState["HoraCita"].ToString();
                //    objsendsms[12] = 0;
                //    objsendsms[13] = textoSMS.Trim();
                //    objsendsms[14] = "";
                //    objsendsms[15] = "";
                //    objsendsms[16] = 0;
                //    objsendsms[17] = 0;
                //    objsendsms[18] = Session["usuCodigo"].ToString();
                //    objsendsms[19] = Session["MachineName"].ToString();
                //    dt = new Conexion(2, "").funConsultarSqls("sp_GenerarTextoSMS", objsendsms);
                //    textoSMS = dt.Tables[0].Rows[0][0].ToString();
                //}
                //if (!string.IsNullOrEmpty(textoSMS))
                //{
                //    //Obtener las credenciales envio SMS
                //    Array.Resize(ref objconsulta, 3);
                //    objconsulta[0] = int.Parse(ViewState["CodigoCamp"].ToString());
                //    objconsulta[1] = "";
                //    objconsulta[2] = 113;
                //    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objconsulta);
                //    if (dt.Tables[0].Rows.Count > 0)
                //    {
                //        usuarioSMS = dt.Tables[0].Rows[0][1].ToString();
                //        passSMS = dt.Tables[0].Rows[1][1].ToString();
                //    }
                //    if (usuarioSMS != "" && passSMS != "")
                //    {
                //        celular = lblCelular.InnerText.Trim();
                //        if (celular.Length == 10) celular = celular.Substring(1, 9);
                //        WReferenceSMS.WSMPLUS sendsms = new WReferenceSMS.WSMPLUS();
                //        WReferenceSMS.RespuestaMPlus response = new WReferenceSMS.RespuestaMPlus();
                //        trama = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>";
                //        trama += "<sms-Request><celular>" + celular + "</celular><mensaje>";
                //        trama += textoSMS;
                //        trama += "</mensaje><id_mensaje>1</id_mensaje></sms-Request>";
                //        response = sendsms.RegistrarMensajeMT(usuarioSMS, passSMS, trama);
                //        System.Xml.XmlElement[] valor = response.Any;
                //        foreach (XmlNode nod in valor)
                //        {
                //            codigo = nod.ChildNodes.Item(0).InnerText;
                //            descripcion = nod.ChildNodes.Item(1).InnerText;
                //        }
                //        //Insertar EL RESULTADO DEL SMS ENVIADO
                //        objsendsms[0] = 3;
                //        objsendsms[13] = textoSMS.Trim();
                //        objsendsms[14] = descripcion;
                //        objsendsms[16] = int.Parse(codigo);
                //        new Conexion(2, "").funConsultarSqls("sp_GenerarTextoSMS", objsendsms);
                //    }
                //    else
                //    {
                //        new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "FrmAgendarCitaMedica", 
                //            "Usuario o Clave sin definir", 1);
                //    }

                //}
                _enviarsms += "{\"phoneNumber\":\"" + lblCelular.InnerText + "\",";
                _enviarsms += "\"messageId\":" + 134056 + ",";
                _enviarsms += "\"transactionId\":\"" + ViewState["CodigoCita"].ToString() + "\",";
                _enviarsms += "\"dataVariable\":[\"" + ViewState["Paciente"].ToString() + "\",\"" + newFechasMms +
                    "\"" + ",\"" + ViewState["HoraCita"].ToString() + "\"" + ",\"" + ViewState["Ciudad"].ToString() + "\"" + ",\"" +
                    ViewState["Medico"].ToString() + "\"" + ",\"" + ViewState["Prestadora"].ToString() + "\"]}";

                HttpClient _client = new HttpClient();
                {
                    //_client.BaseAddress = new Uri("http://186.3.87.6/sms/client/api.php/sendMessage");
                    _client.BaseAddress = new Uri("https://api.smsplus.net.ec/sms/client/api.php/sendMessage");
                    _client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    _client.DefaultRequestHeaders.Add("Authorization", "Basic OTU4OTMzMjA1Om9NYkVhcFA5MGwzN21nalU=");
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpContent _content = new StringContent(_enviarsms, Encoding.UTF8, "application/json");
                var _response = _client.PostAsync("sendMessage", _content);
                _response.Wait();
                HttpResponseMessage _respuesta = _response.Result;
                var _responseJason = _respuesta.Content.ReadAsStreamAsync();

            }
            catch (Exception ex)
            {
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "FrmAgendarCitaMedica", ex.ToString(), 1);
            }
        }

        private string FunMailsEnviar(int codigoprestadora)
        {
            sendmails = "";
            x = 0;
            Array.Resize(ref objsendmails, 3);
            objsendmails[0] = codigoprestadora;
            objsendmails[1] = "";
            objsendmails[2] = 42;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objsendmails);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][0].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][0].ToString() + ",";
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][1].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][1].ToString() + ",";
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][2].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][2].ToString() + ",";
                x = sendmails.LastIndexOf(",");
            }
            if (x > 0) sendmails = sendmails.Remove(x, 1);
            return sendmails;
        }

        private string FunMailsAlterna()
        {
            sendmails = "";
            x = 0;
            Array.Resize(ref objsendmails, 3);
            objsendmails[0] = 0;
            objsendmails[1] = "MAILS ALTERNATIVOS";
            objsendmails[2] = 43;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objsendmails);
            foreach (DataRow dr in dt.Tables[0].Rows)
            {
                sendmails = sendmails + dr[0].ToString() + ",";
            }
            if (!string.IsNullOrEmpty(sendmails)) x = sendmails.LastIndexOf(",");
            if (x > 0) sendmails = sendmails.Remove(x, 1);
            return sendmails;
        }

        private string FunMailsMedicos(int codigomedico)
        {
            sendmails = "";
            x = 0;
            Array.Resize(ref objsendmails, 3);
            objsendmails[0] = codigomedico;
            objsendmails[1] = "";
            objsendmails[2] = 44;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objsendmails);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][0].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][0].ToString() + ",";
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][1].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][1].ToString() + ",";
                x = sendmails.LastIndexOf(",");
            }
            if (x > 0) sendmails = sendmails.Remove(x, 1);
            return sendmails;
        }

        private string FunGetMotivo(string codigomotivo)
        {
            Array.Resize(ref objdatosmotivo, 3);
            objdatosmotivo[0] = 0;
            objdatosmotivo[1] = codigomotivo;
            objdatosmotivo[2] = 41;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objdatosmotivo);
            if (dt.Tables[0].Rows.Count > 0) return dt.Tables[0].Rows[0][0].ToString();
            else return "Consultar a PRESTASALUD S.A";

        }
        private string FunGetMedicamento(int codigoProducto)
        {
            Array.Resize(ref objdatosmotivo, 3);
            objdatosmotivo[0] = codigoProducto;
            objdatosmotivo[1] = "";
            objdatosmotivo[2] = 93;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objdatosmotivo);
            if (dt.Tables[0].Rows.Count > 0) return dt.Tables[0].Rows[0][0].ToString();
            else return "NO";
        }

        private string FunEnviarCancelCita(int codigoCita, int codigoPrestadora, string ciudadCita, string fechaCita, string horaCita, string prestadora,
            string medico, string especialidad, string tipo, string paciente, int codigoGenerado)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 107;
                dtusu = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                mailsU = dtusu.Tables[0].Rows[0][0].ToString();
                mailsP = FunMailsEnviar(codigoPrestadora);
                mailsA = FunMailsAlterna();
                Array.Resize(ref objcitamedica, 16);
                Thread.Sleep(800);
                objcitamedica[11] = "";
                objcitamedica[12] = "";
                objcitamedica[13] = "";
                objcitamedica[14] = "";
                Array.Resize(ref objparam, 1);
                objparam[0] = 16;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        if (dr[0].ToString() == "PIE1") objcitamedica[11] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE2") objcitamedica[12] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE3") objcitamedica[13] = dr[1].ToString();
                        if (dr[0].ToString() == "PIE4") objcitamedica[14] = dr[1].ToString();
                    }
                }
                //filePath = Server.MapPath("~" + "\\CitasCanceladas\\");
                fileTemplate = Server.MapPath("~/Template/HtmlTemplateC.html");
                fileLogo = @ViewState["Ruta"].ToString() + ViewState["Logo"].ToString();
                subject = "Cancelar - " + ViewState["Campaing"].ToString() + "-" + ViewState["Producto"].ToString();
                subject = subject.Replace('\r', ' ').Replace('\n', ' ');
                objcitamedica[0] = codigoGenerado;
                objcitamedica[1] = ciudadCita;
                objcitamedica[2] = fechaCita;
                objcitamedica[3] = horaCita;
                objcitamedica[4] = prestadora;
                objcitamedica[5] = medico.Replace("&#209;", "Ñ");
                objcitamedica[6] = especialidad.Replace("&#237;", "í");
                objcitamedica[7] = tipo == "T" ? "TITULAR" : "BENEFICIARIO";
                objcitamedica[8] = paciente.Replace("&#209;", "Ñ");
                objcitamedica[9] = ddlMotivoCancelar.SelectedItem.ToString();
                objcitamedica[10] = "CITA CANCELADA";
                objcitamedica[15] = Session["usuLogin"].ToString();
                //nameFile = filePath + "CitaCancelada_" + fechaCita.Replace("-", "") + "_" + codigoCita.ToString() + ".txt";
                //returnFile = new Funciones().funCrearArchivoCancel(nameFile, objcitamedica);
                mensaje = new Funciones().funEnviarMailCancel(mailsP, subject, objcitamedica, fileTemplate,
                    ViewState["Host"].ToString(), int.Parse(ViewState["Port"].ToString()), bool.Parse(ViewState["EnableSSl"].ToString()),
                    ViewState["Usuario"].ToString(), ViewState["Password"].ToString(), returnFile, fileLogo, mailsA, mailsD, mailsU);
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }
            return mensaje;
        }
        protected void FunShowJSMessage(string message)
        {

            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "alert", "alert('" + message + "');", true);
        }
        #endregion

        #region Botones y Eventos
        protected void chkSeleccionar_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkest = new CheckBox();
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            foreach (GridViewRow row in grdvTitulares.Rows)
            {
                (row.FindControl("chkSeleccionar") as CheckBox).Checked = false;
            }
            chkest = (CheckBox)(gvRow.Cells[5].FindControl("chkSeleccionar"));
            chkest.Checked = true;

            ViewState["TipoCliente"] = grdvTitulares.DataKeys[intIndex].Values["CodTipo"].ToString();
            ViewState["Cliente"] = grdvTitulares.Rows[intIndex].Cells[1].Text.Replace("&#209;", "Ñ");
            ViewState["TituCodigo"] = grdvTitulares.DataKeys[intIndex].Values["TituCodigo"].ToString();
            ViewState["BeneCodigo"] = grdvTitulares.DataKeys[intIndex].Values["BeneCodigo"].ToString();
            ViewState["CodParentesco"] = grdvTitulares.DataKeys[intIndex].Values["CodParentesco"].ToString();
        }
        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCascadaCombos(0);
            FunLimpiarCampos();
        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCascadaCombos(1);
            FunLimpiarCampos();
        }

        protected void ddlPrestadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunCascadaCombos(3);
            FunLimpiarCampos();
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {


            string codEsp = ddlEspecialidad.SelectedValue.ToString();

            Array.Resize(ref objparam, 3);
            objparam[0] = codEsp; //codigo especialidades
            objparam[1] = 0;
            objparam[2] = 175;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

            int cod = int.Parse(dt.Tables[0].Rows[0][0].ToString());

            if (cod == 1)
            {
                if(ViewState["ContMG"].ToString() == "4")
                {
                    new Funciones().funShowJSMessage("no permitido", this);
                    ddlEspecialidad.SelectedIndex = 0;
                    return;
                }

            }else if(cod == 2 || cod == 5 || cod ==7 || cod ==8 || cod == 24 || cod == 25 || cod == 26)
                      
            {
                if (ViewState["ContESP"].ToString() == "3")
                {
                    new Funciones().funShowJSMessage("no permitido", this);
                    ddlEspecialidad.SelectedIndex = 0;
                    return;
                }
            }
            FunCascadaCombos(4);
            FunLimpiarCampos();
        }
        protected void CalendarioCita_SelectionChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (ddlMedico.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Médico para listar horarios..!";
                return;
            }
            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaCalendar = DateTime.ParseExact(CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtmFechaCalendar < dtmFechaActual)
            {
                lblerror.Text = "La fecha de la cita no puede ser menor a la fecha actual";
                tbDatosCita.Clear();
                ViewState["tbDatosCita"] = tbDatosCita;
                grdvDatosCitas.DataSource = tbDatosCita;
                grdvDatosCitas.DataBind();
                return;
            }
            FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd"));
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            int codme = int.Parse(ddlMedico.SelectedValue.ToString());
            double vfinal = double.Parse(ViewState["TotalLab"].ToString());
            double vrestante = double.Parse(ViewState["TotalRestante"].ToString());
            vrestante = Math.Round(vrestante, 2);
            
            if (codme == 2332)
            {
                int lab = int.Parse(ddlEspecialidad.SelectedValue.ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = lab; //codigo especialidades
                objparam[1] = 0;
                objparam[2] = 178;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);


                double pvp = double.Parse(dt.Tables[0].Rows[0][0].ToString());

                if (vrestante != 0)
                {
                    double resta = vrestante - pvp;
                    if (resta < 0)
                    {

                        new Funciones().funShowJSMessage("No tiene cupo para tomar el examen..!!", this);
                        ddlMedico.SelectedIndex = 0;
                        ddlEspecialidad.SelectedIndex = 0;
                        return;
                    }
                }
              
                //resta = Math.Round(vrestante, 2);

                if (double.Parse(ViewState["TotalRestante"].ToString()) > 0)
                {
                    if (vrestante < 3)
                    {
                        new Funciones().funShowJSMessage("Su cupo es menor a $3" + " --> " + "Pvp:" + vrestante, this);
                        ddlMedico.SelectedIndex = 0;
                        return;

                    }

                }
              
            }

            lblerror.Text = "";
            CalendarioCita.SelectedDate = DateTime.Today;
            FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd"));
        }
        protected void imgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRowK = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRowK.RowIndex;
                bool lexiste = false;
                lblerror.Text = "";

                if (ddlOpcion.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Tipo de Registro..!", this);
                    return;
                }
                if (ddlMotivoCita.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Motivo de la Cita Médica..!", this);
                    return;
                }

                //Consulta si la hora execede el tiempo determinado en el intervalo(SOLO FECHA ACTUAL)
                fechaActual = DateTime.Now.ToString("MM/dd/yyyy");
                Calendario = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                if (fechaActual == Calendario)
                {
                    intervalo = TimeSpan.FromMinutes(int.Parse(ViewState["Intervalo"].ToString()));
                    horaSistema = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "00";
                    horaActual = TimeSpan.Parse(horaSistema);
                    string[] hora = grdvDatosCitas.Rows[intIndex].Cells[1].Text.Split(new char[] { '-' });
                    horaAgenda = TimeSpan.Parse(hora[0].ToString());
                    intervaloHoras = horaAgenda - horaActual;
                    int iHorasAgenda = (int)intervaloHoras.TotalMinutes;
                    int iHorasInterv = (int)intervalo.TotalMinutes;
                    if (iHorasAgenda <= iHorasInterv)
                    {
                        new Funciones().funShowJSMessage("Lamentamos.. Excede el intervalo de tiempo permitido..!", this);
                        return;
                    }
                }
                ViewState["FechaCita"] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                ViewState["HoraCita"] = grdvDatosCitas.Rows[intIndex].Cells[1].Text;
                //Buscar si ya existe alguna reserva realizada
                Array.Resize(ref objparam, 18);
                objparam[0] = 2;
                objparam[1] = int.Parse(ddlEspecialidad.SelectedValue);
                objparam[2] = int.Parse(ddlMedico.SelectedValue);
                objparam[3] = 0;
                objparam[4] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                objparam[5] = CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper();
                objparam[6] = grdvDatosCitas.Rows[intIndex].Cells[1].Text;
                objparam[7] = "";
                objparam[8] = int.Parse(grdvDatosCitas.DataKeys[intIndex].Values["HodeCodigo"].ToString());
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    new Funciones().funShowJSMessage("Lamentamos.. esta hora ya fue reservada..!", this);
                    FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                    return;
                }
                //Buscar si ya existe alguna cita medica realizada
                objparam[0] = 3;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    new Funciones().funShowJSMessage("Ya Existe una cita médica realizada, seleccione otra fecha/hora..!", this);
                    FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                    return;
                }

                if (ViewState["Cliente"] == null)
                {
                    new Funciones().funShowJSMessage("Seleccione Titular o Beneficiario para Agendar Cita..!", this);
                    return;
                }
                if (ViewState["tbCitaMedica"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tbCitaMedica"];
                    if (tblbuscar.Rows.Count > 0)
                    {
                        DataRow result = tblbuscar.Select("Cliente='" + ViewState["Cliente"].ToString() + "' and Especialidad='" + ddlEspecialidad.SelectedItem.ToString() + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Titular/Beneficiario ya existe registrado para la cita..!", this);
                    return;
                }

                Array.Resize(ref objparam, 18);
                objparam[0] = 18;
                objparam[1] = int.Parse(ddlEspecialidad.SelectedValue);
                objparam[2] = int.Parse(ddlMedico.SelectedValue);
                objparam[3] = 0;
                objparam[4] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                objparam[5] = CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper();
                objparam[6] = grdvDatosCitas.Rows[intIndex].Cells[1].Text;
                objparam[7] = "";
                objparam[8] = int.Parse(grdvDatosCitas.DataKeys[intIndex].Values["HodeCodigo"].ToString());
                objparam[9] = "";
                objparam[10] = int.Parse(ViewState["TituCodigo"].ToString());
                objparam[11] = int.Parse(ViewState["BeneCodigo"].ToString());
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                //PREGUNTAR SI EL CLIENTE YA TIENE AGENDADA UNA CITA EN ESTADO ACTIVO
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (Session["CodigoProducto"].ToString() == "225" || Session["CodigoProducto"].ToString() == "226" || Session["CodigoProducto"].ToString() == "227")
                {

                }
                else
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {

                        alerta = "Cliente ya tiene registrada una cita Fecha: " + dt.Tables[0].Rows[0][1].ToString();
                        new Funciones().funShowJSMessage(alerta, this);
                        return;
                    }

                }
                
                //REALIZAR EL CONTEO DE LAS CITAS SEGUN LA PROGRAMACION DE SU PRODUCTO
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[1] = "Mx";
                objparam[2] = 127;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    asisanual = int.Parse(dt.Tables[0].Rows[0]["AsisAnual"].ToString());
                    asismensual = int.Parse(dt.Tables[0].Rows[0]["AsisMes"].ToString());
                    tipofecha = dt.Tables[0].Rows[0]["TipoFecha"].ToString();
                    fechaSistema = DateTime.ParseExact(fechaActual, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    if (tipofecha == "FC")
                    {
                        fechaFinCobertura = DateTime.ParseExact(ViewState["FechaFinCobertura"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        if (fechaFinCobertura < fechaSistema)
                        {
                            new Funciones().funShowJSMessage("Fecha de Cobertura Finalizada..!", this);
                            return;
                        }
                    }
                    //CONSULTAR LA CANTIDAD DE CITAS REALIZAD POR AÑO
                    Array.Resize(ref objparam, 11);
                    aniocita = fechaSistema.Year;
                    objparam[0] = 3;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = aniocita;
                    objparam[7] = 0;
                    objparam[8] = int.Parse(Session["CodigoTitular"].ToString());
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    if (dts.Tables[0].Rows.Count > 0)
                    {
                        citasanual = int.Parse(dts.Tables[0].Rows[0]["Citas"].ToString());
                        if (citasanual >= asisanual)
                        {
                            new Funciones().funShowJSMessage("Ha Exedido del Número de Citas autorizadas por año..!", this);
                            return;
                        }
                    }
                    fechaCita = DateTime.ParseExact(ViewState["FechaCita"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    aniocita = fechaCita.Year;
                    mescita = fechaCita.Month;
                    objparam[0] = 2;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = aniocita;
                    objparam[7] = mescita;
                    objparam[8] = int.Parse(Session["CodigoTitular"].ToString());
                    objparam[9] = 0;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    if (dts.Tables[0].Rows.Count > 0)
                    {
                        citasmes = int.Parse(dts.Tables[0].Rows[0]["Citas"].ToString());
                        if (citasmes >= asismensual)
                        {
                            new Funciones().funShowJSMessage("Ha Exedido del Número de Citas autorizadas por Mes..!", this);
                            return;
                        }
                    }
                }
                Array.Resize(ref objparam, 18);
                objparam[0] = 1;
                objparam[1] = int.Parse(ddlEspecialidad.SelectedValue);
                objparam[2] = int.Parse(ddlMedico.SelectedValue);
                objparam[3] = 0;
                objparam[4] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                objparam[5] = CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper();
                objparam[6] = grdvDatosCitas.Rows[intIndex].Cells[1].Text;
                objparam[7] = ddlMotivoCita.SelectedValue;
                objparam[8] = int.Parse(grdvDatosCitas.DataKeys[intIndex].Values["HodeCodigo"].ToString()); ;
                objparam[9] = ViewState["TipoCliente"].ToString();
                objparam[10] = int.Parse(ViewState["TituCodigo"].ToString());
                objparam[11] = int.Parse(ViewState["BeneCodigo"].ToString());
                objparam[12] = ViewState["CodParentesco"].ToString();
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = txtObservacion.Text.Trim().ToUpper();
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                tbDatosCita = (DataTable)ViewState["tbDatosCita"];
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    Session["SalirAgenda"] = "NO";
                    DataTable tblagre = new DataTable();
                    tblagre = (DataTable)ViewState["tbCitaMedica"];
                    DataRow filagre = tblagre.NewRow();
                    filagre["Cliente"] = ViewState["Cliente"].ToString();
                    filagre["Ciudad"] = ddlCiudad.SelectedItem.ToString();
                    filagre["Prestadora"] = ddlPrestadora.SelectedItem.ToString();
                    filagre["Medico"] = ddlMedico.SelectedItem.ToString();
                    filagre["Especialidad"] = ddlEspecialidad.SelectedItem.ToString();
                    filagre["FechaCita"] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                    filagre["DiaCita"] = CalendarioCita.SelectedDate.ToString("dddd").ToUpper();
                    filagre["Hora"] = grdvDatosCitas.Rows[intIndex].Cells[1].Text;
                    filagre["Detalle"] = ddlMotivoCita.SelectedValue;
                    filagre["PreeCodigo"] = int.Parse(ddlEspecialidad.SelectedValue);
                    filagre["MediCodigo"] = int.Parse(ddlMedico.SelectedValue);
                    filagre["HodeCodigo"] = int.Parse(grdvDatosCitas.DataKeys[intIndex].Values["HodeCodigo"].ToString());
                    filagre["TipoCliente"] = ViewState["TipoCliente"].ToString();
                    filagre["TituCodigo"] = int.Parse(ViewState["TituCodigo"].ToString());
                    filagre["BeneCodigo"] = int.Parse(ViewState["BeneCodigo"].ToString());
                    filagre["CodParentesco"] = ViewState["CodParentesco"].ToString();
                    filagre["EstatusCita"] = ddlOpcion.SelectedValue;
                    filagre["Longitud"] = "";
                    filagre["Latitud"] = "";
                    filagre["CodigoPrestadora"] = ddlPrestadora.SelectedValue;
                    filagre["Observacion"] = txtObservacion.Text.Trim().ToUpper();
                    tblagre.Rows.Add(filagre);
                    ViewState["tbCitaMedica"] = tblagre;
                    grdvResumenCita.DataSource = tblagre;
                    grdvResumenCita.DataBind();
                    imgCancelar.Enabled = true;
                    FunLimpiarAgendamiento();
                    FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void tmrdat_Tick(object sender, EventArgs e)
        {
            try
            {
                //Lamar al procedimiento para que elimine reservas
                FunEliminarReservas();
                if (int.Parse(ddlMedico.SelectedValue) > 0) FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void grdvDatosCitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ImageButton btnAgenda = new ImageButton();
            horaSistema = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "00";
            horaActual = TimeSpan.Parse(horaSistema);

            fechaActual = DateTime.Now.ToString("MM/dd/yyyy");
            Calendario = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    intervalo = TimeSpan.FromMinutes(int.Parse(ViewState["Intervalo"].ToString()));
                    btnAgenda = (ImageButton)(e.Row.Cells[0].FindControl("imgSelecc"));
                    string[] hora = e.Row.Cells[1].Text.Split(new char[] { '-' });
                    horaAgenda = TimeSpan.Parse(hora[0].ToString());
                    agenda = e.Row.Cells[2].Text.Replace("&nbsp;", "").Trim();

                    switch (agenda)
                    {
                        case "RESERVADO":
                            btnAgenda.ImageUrl = "~/Botones/citamedicadesac.png";
                            btnAgenda.Height = 20;
                            btnAgenda.Enabled = false;
                            break;
                        case "AGENDADO":
                            btnAgenda.ImageUrl = "~/Botones/mailagenda.png";
                            btnAgenda.Height = 20;
                            btnAgenda.Enabled = false;
                            break;
                        case "ATENDIDO":
                            btnAgenda.ImageUrl = "~/Botones/atencionmedica.png";
                            btnAgenda.Height = 20;
                            btnAgenda.Enabled = false;
                            break;
                        case "AUSENTE":
                            btnAgenda.ImageUrl = "~/Botones/ausente.png";
                            btnAgenda.Height = 20;
                            btnAgenda.Enabled = false;
                            break;
                    }

                    if (fechaActual == Calendario && agenda == "")
                    {
                        intervaloHoras = horaAgenda - horaActual;
                        int iHorasAgenda = (int)intervaloHoras.TotalMinutes;
                        int iHorasInterv = (int)intervalo.TotalMinutes;
                        if (iHorasAgenda <= iHorasInterv)
                        {
                            btnAgenda.ImageUrl = "~/Botones/citamedicadesac.png";
                            btnAgenda.Height = 20;
                            btnAgenda.Enabled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int preecodigo = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["PreeCodigo"].ToString());
                int medicodigo = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["MediCodigo"].ToString());
                string fechacita = grdvResumenCita.Rows[intIndex].Cells[6].Text;
                string diacita = DateTime.ParseExact(fechacita, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("dddd");
                Array.Resize(ref objparam, 18);
                objparam[0] = 4;
                objparam[1] = preecodigo;
                objparam[2] = medicodigo;
                objparam[3] = 0;
                objparam[4] = fechacita;
                objparam[5] = diacita.ToUpper();
                objparam[6] = grdvResumenCita.Rows[intIndex].Cells[8].Text;
                objparam[7] = "";
                objparam[8] = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["HodeCodigo"].ToString()); ;
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = "";
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    FunGetReservas();
                    FunAgendaHoras(0, preecodigo, medicodigo, fechacita, diacita);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('Reserva Cancelada!..');", true);
                    ViewState["Cliente"] = null;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgHorarios_Click(object sender, ImageClickEventArgs e)
        {
            if (int.Parse(ddlMedico.SelectedValue) == 0)
            {
                lblerror.Text = "Seleccione Médico para mostrar datos..!";
                return;
            }
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosMedicoHorarios.aspx?CodigoMedico=" + ddlMedico.SelectedValue + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);

        }
        protected void ddlOpcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAgendamientos.Visible = false;
            pnlResumenCita.Visible = false;
            CalendarioCita.Visible = false;
            if (ddlOpcion.SelectedValue == "A" || ddlOpcion.SelectedValue == "I")
            {
                if (int.Parse(ddlMedico.SelectedValue) > 0) FunAgendaHoras(0, int.Parse(ddlEspecialidad.SelectedValue), int.Parse(ddlMedico.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                pnlAgendamientos.Visible = true;
                pnlResumenCita.Visible = true;
                CalendarioCita.Visible = true;
            }
            else
            {
                FunLimpiarCampos();
                FunLimpiarAgendamiento();
                FunEliminarReservasOtraOpcion();
                FunGetReservas();
            }
        }
        protected void imgAgendar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tbCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                if (tbCitaMedica.Rows.Count > 0)
                {
                    //ACTUALIZAR EN EL CASO DE SER AGENDAMIENTO POR EXAMENES
                    if (Session["Regresar"].ToString() == "1")
                    {
                        if (FileUpload1.HasFile == false)
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivo..!", this);
                            return;
                        }
                        else
                        {
                            filePath1 = FileUpload1.PostedFile.FileName;
                            filename1 = Path.GetFileName(filePath1);
                            ext1 = Path.GetExtension(filename1);
                            type1 = string.Empty;

                            if (filename1.Length > 100)
                            {
                                new Funciones().funShowJSMessage("Nombre del Archivo Máximo 100 Caractéres..!", this);
                                return;
                            }

                            switch (ext1)
                            {
                                case ".doc":
                                case ".docx":
                                    type1 = "application/word";
                                    break;
                                case ".pdf":
                                    type1 = "application/pdf";
                                    break;
                                case ".png":
                                    type1 = "application/png";
                                    break;
                                case ".jpg":
                                    type1 = "application/jpg";
                                    break;
                            }
                            if (type1 != String.Empty)
                            {
                                Stream fs = FileUpload1.PostedFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                bytes = br.ReadBytes((Int32)fs.Length);
                            }
                            else
                            {
                                new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                                return;
                            }
                        }

                        Array.Resize(ref objparam, 43);
                        objparam[0] = 3;
                        objparam[1] = 0;
                        objparam[2] = "";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = "";
                        objparam[7] = "";
                        objparam[8] = "";
                        objparam[9] = "";
                        objparam[10] = "";
                        objparam[11] = 0;
                        objparam[12] = "";
                        objparam[13] = "";
                        objparam[14] = "";
                        objparam[15] = "";
                        objparam[16] = "";
                        objparam[17] = 0;
                        objparam[18] = 0;
                        objparam[19] = "";
                        objparam[20] = txtObservacionG.Text.Trim().ToUpper();
                        objparam[21] = bytes;
                        objparam[22] = filename1;
                        objparam[23] = type1;
                        objparam[24] = ext1;
                        objparam[25] = 0;
                        objparam[26] = "0";
                        objparam[27] = "0";
                        objparam[28] = "";
                        objparam[29] = 0;
                        objparam[30] = "";
                        objparam[31] = "SGA";
                        objparam[32] = ViewState["FechaCita"].ToString() + " " + ViewState["HoraCita"].ToString().Substring(0, 5);
                        objparam[33] = "";
                        objparam[34] = "";
                        objparam[35] = "";
                        objparam[36] = int.Parse(Session["CodigoTitular"].ToString());
                        objparam[37] = int.Parse(ddlPrestadora.SelectedValue);
                        objparam[38] = int.Parse(ddlEspecialidad.SelectedValue);
                        objparam[39] = int.Parse(ddlMedico.SelectedValue);
                        objparam[40] = 0;
                        objparam[41] = int.Parse(Session["usuCodigo"].ToString());
                        objparam[42] = Session["MachineName"].ToString();
                        dtsexa = new Conexion(2, "").FunInsertSolictudExamen(objparam);
                    }

                    string[] columnas = new[] { "PreeCodigo", "MediCodigo","CodigoPrestadora", "TipoCliente", "TituCodigo", "BeneCodigo", "CodParentesco",
                        "EstatusCita","FechaCita","DiaCita","Hora","HodeCodigo","Detalle","Longitud","Latitud","Observacion"};
                    tbNuevaCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                    tbMailCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                    DataView view = new DataView(tbNuevaCitaMedica);
                    tbNuevaCitaMedica = view.ToTable(true, columnas);
                    Array.Resize(ref objparam, 7);
                    objparam[0] = 0;
                    objparam[1] = int.Parse(Session["CodigoProducto"].ToString());
                    objparam[2] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[3] = Session["MachineName"].ToString();
                    objparam[4] = txtObservacionG.Text.Trim().ToUpper();
                    objparam[5] = "";
                    objparam[6] = ddlTipoPago.SelectedValue;
                    DataSet ds = new Conexion(2, "").FunCodigoCita(objparam, tbNuevaCitaMedica);
                    int codCita = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                    if (codCita > 0)
                    {
                        Session["SalirAgenda"] = "SI";
                        ViewState["CodigoCitapop"] = codCita;

                        FunEnviarMailCita(tbMailCitaMedica, ddlTipoPago.SelectedItem.ToString());
                    }
                }
                else FunShowJSMessage("Seleccione Datos para Agendar la Cita");
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void grdvSumaLaboratorio_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totalLAB = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "ValorTotal"));
                e.Row.Cells[0].Text = "Valor Consumido";
                //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[1].Text = totalLAB.ToString("c");
                ViewState["TotalLab"] = totalLAB.ToString();

                //decimal valor = totalLAB - 100;

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //tPorcentaje = Math.Round((tPresupuesto / tExigible) * 100, 2);

                //e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
                double valorFinal = 100.00;
                e.Row.Cells[0].Text = "Valor Restante";
                double _totalfinal = valorFinal - Convert.ToDouble(ViewState["TotalLab"].ToString());
                e.Row.Cells[1].Text = _totalfinal.ToString("c");
                ViewState["TotalRestante"] = _totalfinal.ToString();
                if(_totalfinal < 3)
                {
                  e.Row.Cells[1].BackColor = System.Drawing.Color.LightPink;
                }
                else
                {
                    e.Row.Cells[1].BackColor = System.Drawing.Color.LightBlue;
                }


            }

        }

        protected void grdvContadorCitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                if (int.Parse(ViewState["Mgeneral"].ToString()) == 4)
                {
                    e.Row.Cells[1].Font.Italic = true;
                    e.Row.Cells[1].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].Text = "XX";
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }

                if (int.Parse(ViewState["Especialidad"].ToString()) == 3)
                {
                    e.Row.Cells[2].Font.Italic = true;
                    e.Row.Cells[2].ForeColor = System.Drawing.Color.Red;
                    e.Row.Cells[2].Text = "XX";
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
            }

        }
        protected void grdvHistorialCitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    btnEstado = (ImageButton)(e.Row.Cells[0].FindControl("imgEstado"));
                    chkCancelar = (CheckBox)(e.Row.Cells[6].FindControl("chkSelecc"));
                    status = grdvHistorialCitas.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    chkCancelar.Enabled = false;
                    switch (status)
                    {
                        case "A":
                            btnEstado.ImageUrl = "~/Botones/mailagenda.png";
                            btnEstado.Height = 20;
                            chkCancelar.Enabled = true;
                            break;
                        case "C":
                            btnEstado.ImageUrl = "~/Botones/emailcancel.png";
                            btnEstado.Height = 20;
                            break;
                        case "T":
                            btnEstado.ImageUrl = "~/Botones/medico.png";
                            btnEstado.Height = 20;
                            break;
                        case "S":
                            btnEstado.ImageUrl = "~/Botones/ausente.png";
                            btnEstado.Height = 20;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Array.Resize(ref objdatoscancel, 14);
                objdatoscancel[0] = 0;
                objdatoscancel[2] = "C";
                objdatoscancel[3] = int.Parse(ddlMotivoCancelar.SelectedValue);
                objdatoscancel[4] = "";
                objdatoscancel[5] = "";
                objdatoscancel[6] = DateTime.Now.ToString("MM/dd/yyyy");
                objdatoscancel[7] = DateTime.Now.ToString("HH:mm") + " - " + "00:00";
                objdatoscancel[8] = "";
                objdatoscancel[9] = "";
                objdatoscancel[10] = int.Parse(Session["CodigoProducto"].ToString());
                objdatoscancel[12] = int.Parse(Session["usuCodigo"].ToString());
                objdatoscancel[13] = Session["MachineName"].ToString();
                foreach (GridViewRow row in grdvHistorialCitas.Rows)
                {
                    chkselecc = (CheckBox)grdvHistorialCitas.Rows[row.RowIndex].Cells[6].FindControl("chkSelecc");
                    if (chkselecc.Checked) contar++;
                }
                if (contar == 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('Selecione Citas para Cancelar!..');", true);
                    return;
                }
                foreach (GridViewRow row in grdvHistorialCitas.Rows)
                {
                    chkselecc = (CheckBox)grdvHistorialCitas.Rows[row.RowIndex].Cells[6].FindControl("chkSelecc");
                    if (chkselecc.Checked)
                    {
                        int codigoCita = int.Parse(grdvHistorialCitas.DataKeys[row.RowIndex].Values["CitaCodigo"].ToString());
                        int codigoPresta = int.Parse(grdvHistorialCitas.DataKeys[row.RowIndex].Values["PrestaCodigo"].ToString());
                        int codigoGenerado = int.Parse(grdvHistorialCitas.DataKeys[row.RowIndex].Values["CodigoGenerado"].ToString());
                        string prestadora = grdvHistorialCitas.DataKeys[row.RowIndex].Values["Prestadora"].ToString();
                        string tipo = grdvHistorialCitas.DataKeys[row.RowIndex].Values["Tipo"].ToString();
                        string paciente = grdvHistorialCitas.Rows[row.RowIndex].Cells[1].Text;
                        string medico = grdvHistorialCitas.Rows[row.RowIndex].Cells[3].Text;
                        string especialidad = grdvHistorialCitas.Rows[row.RowIndex].Cells[4].Text;
                        string ciudadCita = grdvHistorialCitas.Rows[row.RowIndex].Cells[5].Text;
                        string fechaCita = grdvHistorialCitas.Rows[row.RowIndex].Cells[6].Text;
                        string horaCita = grdvHistorialCitas.Rows[row.RowIndex].Cells[7].Text;
                        objdatoscancel[1] = codigoCita;
                        objdatoscancel[11] = codigoGenerado;
                        dt = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", objdatoscancel);
                        if (dt.Tables[0].Rows[0][0].ToString() == "")
                        {
                            mensaje = FunEnviarCancelCita(codigoCita, codigoPresta, ciudadCita, fechaCita, horaCita, prestadora, medico, especialidad,
                                tipo, paciente, codigoGenerado);
                        }
                    }
                }
                if (mensaje == "") Response.Redirect("FrmCitaMedicaAdmin.aspx?MensajeRetornado='Cita(s) Cancelada(s) con Éxito'", true);
                else Response.Redirect("FrmCitaMedicaAdmin.aspx?MensajeRetornado='Revise Mails, hubo errores en el envío..!'", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void imgEstado_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int citaCodigo = int.Parse(grdvHistorialCitas.DataKeys[intIndex].Values["CitaCodigo"].ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = citaCodigo;
                objparam[1] = "";
                objparam[2] = 34;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    grdvHistorialDetalle.DataSource = dt;
                    grdvHistorialDetalle.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            if (Session["SalirAgenda"].ToString() == "NO")
            {
                string alerta = "Existe una reserva, realice el agendamiento para poder salir..! ";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('" + alerta + "')", true);
                return;
            }
            if (Session["Regresar"].ToString() == "0")
                Response.Redirect("FrmCitaMedicaAdmin.aspx", true);
            else
                Response.Redirect("~/Examenes/FrmSolicitudOperadorAdmin.aspx", true);
        }
        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 38;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    tbCitaMedica.Clear();
                    grdvResumenCita.DataSource = tbCitaMedica;
                    grdvResumenCita.DataBind();
                    tbCitaMedica.Columns.Add("Cliente");
                    tbCitaMedica.Columns.Add("Ciudad");
                    tbCitaMedica.Columns.Add("Prestadora");
                    tbCitaMedica.Columns.Add("Medico");
                    tbCitaMedica.Columns.Add("Especialidad");
                    tbCitaMedica.Columns.Add("FechaCita");
                    tbCitaMedica.Columns.Add("DiaCita");
                    tbCitaMedica.Columns.Add("Hora");
                    tbCitaMedica.Columns.Add("Detalle");
                    tbCitaMedica.Columns.Add("PreeCodigo");
                    tbCitaMedica.Columns.Add("MediCodigo");
                    tbCitaMedica.Columns.Add("HodeCodigo");
                    tbCitaMedica.Columns.Add("TipoCliente");
                    tbCitaMedica.Columns.Add("TituCodigo");
                    tbCitaMedica.Columns.Add("BeneCodigo");
                    tbCitaMedica.Columns.Add("CodParentesco");
                    tbCitaMedica.Columns.Add("EstatusCita");
                    tbCitaMedica.Columns.Add("Longitud");
                    tbCitaMedica.Columns.Add("Latitud");
                    tbCitaMedica.Columns.Add("Observacion");
                    ViewState["tbCitaMedica"] = tbCitaMedica;
                    Session["SalirAgenda"] = "SI";
                    FunLimpiarCampos();
                    ddlOpcion.SelectedValue = "0";
                    ddlMotivoCita.SelectedValue = "0";
                }

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int preecodigo = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["CodigoPrestadora"].ToString());
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestadora.aspx?CodigoPrestadora=" + preecodigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updDatosPersonales, GetType(), "Actualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmActualizarDatos.aspx?CodTitular=" +
                Session["CodigoTitular"].ToString() + "&CodProducto=" + Session["CodigoProducto"].ToString() + "&Regresar=" + Session["Regresar"].ToString() +
                "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=600px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }
        protected void imgPrestadora_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlPrestadora.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Médico para mostrar datos..!";
                return;
            }
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestadora.aspx?CodigoPrestadora=" + ddlPrestadora.SelectedValue + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }
        protected void imgBuscaPres_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int preecodigo = int.Parse(grdvHistorialCitas.DataKeys[intIndex].Values["PrestaCodigo"].ToString());
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestadora.aspx?CodigoPrestadora=" + preecodigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}