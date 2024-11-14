using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaOdontologica
{
    public partial class FrmAgendarCitaOdonto : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataSet dtusu = new DataSet();
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        Object[] objcitamedica = new Object[23];
        Object[] objdatostitu = new Object[4];
        Object[] objdatosmotivo = new Object[3];
        Object[] objdatoscancel = new Object[11];
        Object[] objsendmails = new Object[3];
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
        string horaSistema = "", fechaActual = "", Calendario = "", nameFile = "", filePath = "", returnFile = "",
                fileTemplate = "", subject = "", agenda = "", status = "", mensaje = "", fileLogo = "", mailsP = "",
                mailsA = "", mailsD = "", sendmails = "", msjEmail = "", nuevoEstado = "", medicamentos = "", mailsU = "",
                estadoActual = "", tipofecha = "", sql = "", _enviarsms = "";
        CheckBox chkselecc = new CheckBox();
        int codigocita = 0, contar = 0, x = 0, asisanual = 0, asismensual = 0, aniocita = 0, mescita = 0, citasmes = 0, citasanual = 0;
        DateTime fechaCita, fechaFinCobertura, fechaSistema;
        Thread thrEnviarSMS;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
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
                tbCitaMedica.Columns.Add("EspeCodigo");
                tbCitaMedica.Columns.Add("HodeCodigo");
                tbCitaMedica.Columns.Add("TipoCliente");
                tbCitaMedica.Columns.Add("TituCodigo");
                tbCitaMedica.Columns.Add("BeneCodigo");
                tbCitaMedica.Columns.Add("CodParentesco");
                tbCitaMedica.Columns.Add("EstatusCita");
                tbCitaMedica.Columns.Add("Longitud");
                tbCitaMedica.Columns.Add("Latitud");
                tbCitaMedica.Columns.Add("Observacion");
                tbCitaMedica.Columns.Add("FechaCitax");
                ViewState["tbCitaMedica"] = tbCitaMedica;

                lbltitulo.Text = "Agendar Cita";
                Session["CodigoTitular"] = Request["CodigoTitular"];
                Session["CodigoProducto"] = Request["CodigoProducto"];
                Session["TipoCita"] = "CitaOdontologica";
                ViewState["Intervalo"] = 0;
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
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
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
                    //lblTipodoc.InnerText = dt.Tables[0].Rows[0]["TipoDocumento"].ToString();
                    //lblIdentificacion.InnerText = dt.Tables[0].Rows[0]["Identificacion"].ToString();
                    //lblNombres.InnerText = dt.Tables[0].Rows[0]["Nombres"].ToString() + " " + dt.Tables[0].Rows[0]["Apellidos"].ToString();
                    //lblTelefonoCasa.InnerText = dt.Tables[0].Rows[0]["FonoCasa"].ToString();
                    //lblTelefonoOficina.InnerText = dt.Tables[0].Rows[0]["FonoOficina"].ToString();
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
                lbltitulo.Text = "Agenda Cita Odontológica - Producto: " + dt.Tables[0].Rows[0][1].ToString();
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
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[1] = "";
                objparam[2] = 103;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) ViewState["Secuencial"] = dt.Tables[0].Rows[0][0].ToString();
                //PONER INFORMACION ADICIONAL
                objparam[0] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[2] = 128;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatosAdicionales.DataSource = dt.Tables[0];
                grdvDatosAdicionales.DataBind();
                //Array.Resize(ref objparam, 11);
                //objparam[0] = 7;
                //objparam[1] = "";
                //objparam[2] = "";
                //objparam[3] = "";
                //objparam[4] = "";
                //objparam[5] = "";
                //objparam[6] = int.Parse(ViewState["CodigoGrupo"].ToString());
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

        private void FunEliminarReservas()
        {
            try
            {
                //BORRAR LAS CITAS QUE QUEDARON RESERVADAS Y SE PASARON DE LA FECHA-HORA
                Array.Resize(ref objparam, 18);
                objparam[0] = 9;
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
            }
        }

        private void FunHistorialCitas()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["CodigoTitular"].ToString());
                objparam[1] = "";
                objparam[2] = 48;
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
        private void FunGetReservas()
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 10;
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
                if (dt != null)
                {
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
        private void FunCascadaCombos(int opcion)
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
                    objparam[2] = 49;
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
                    objparam[2] = 50;
                    ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlEspecialidad.DataTextField = "Descripcion";
                    ddlEspecialidad.DataValueField = "Codigo";
                    ddlEspecialidad.DataBind();
                    break;
                case 4:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlEspecialidad.SelectedValue;
                    objparam[1] = ddlPrestadora.SelectedValue;
                    objparam[2] = 51;
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
                    objparam[0] = 25;
                    ddlMotivoCita.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlMotivoCita.DataTextField = "Descripcion";
                    ddlMotivoCita.DataValueField = "Codigo";
                    ddlMotivoCita.DataBind();
                    break;
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
        private void FunAgendaHoras(int tipo, int preecodigo, int codmedico, int especod, string fechachita, string diacita)
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 11;
                objparam[1] = preecodigo;
                objparam[2] = codmedico;
                objparam[3] = especod;
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
        private void FunLimpiarAgendamiento()
        {
            foreach (GridViewRow row in grdvTitulares.Rows)
            {
                (row.FindControl("chkSeleccionar") as CheckBox).Checked = false;
            }
            txtObservacion.Text = "";
            ddlMotivoCita.SelectedValue = "0";
        }
        private void FunEnviarMailCita(DataTable tblCitaMedica)
        {

            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 107;
                dtusu = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                mailsU = dtusu.Tables[0].Rows[0][0].ToString();
                
                //System.Threading.Thread.Sleep(300);

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
                //filePath = Server.MapPath("~" + "\\CitasAgendadasO\\");
                fileTemplate = Server.MapPath("~/Template/HtmlTemplate.html");
                fileLogo = @ViewState["Ruta"].ToString() + ViewState["Logo"].ToString();
                subject = "Agendamiento - Odontológico " + ViewState["Campaing"].ToString() + "-" + ViewState["Producto"].ToString();
                subject = subject.Replace('\r', ' ').Replace('\n', ' ');
                Array.Resize(ref objparam, 18);
                objparam[0] = 17;
                objparam[13] = "";
                objparam[14] = "";
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                objparam[17] = "";
                objcitamedica[0] = ViewState["Campaing"].ToString();
                objcitamedica[1] = ViewState["Producto"].ToString();
                foreach (DataRow dr in tblCitaMedica.Rows)
                {
                    mailsP = FunMailsEnviar(int.Parse(dr[9].ToString()));
                    if (string.IsNullOrEmpty(mailsP))
                    {
                        mensaje = "Hubo errores en el envío, Defina mails de la Prestadora..!";
                        lblerror.Text = "Defina mails de envío de la Prestadora..!";
                        break;
                    }
                    mailsA = FunMailsAlterna();
                    mailsD = FunMailsMedicos(int.Parse(dr[10].ToString()));
                    objparam[1] = int.Parse(dr[9].ToString());
                    objparam[2] = int.Parse(dr[10].ToString());
                    objparam[3] = int.Parse(dr[11].ToString());
                    objparam[4] = dr[5].ToString();
                    objparam[5] = dr[6].ToString();
                    objparam[6] = dr[7].ToString();
                    objparam[7] = "";
                    objparam[8] = int.Parse(dr[12].ToString());
                    objparam[9] = dr[13].ToString(); ;
                    objparam[10] = int.Parse(dr[14].ToString()); ;
                    objparam[11] = int.Parse(dr[15].ToString()); ;
                    objparam[12] = dr[16].ToString(); ;
                    codigocita = new Conexion(2, "").FunGetCodigoCita(objparam);
                    medicamentos = FunGetMedicamento(int.Parse(ViewState["CodigoGrupo"].ToString()));
                    objcitamedica[2] = codigocita;
                    objcitamedica[3] = dr[1].ToString();
                    objcitamedica[4] = dr[5].ToString();
                    objcitamedica[5] = dr[7].ToString().Substring(0, 5);
                    objcitamedica[6] = dr[2].ToString();
                    objcitamedica[7] = dr[3].ToString().Replace("&#209;", "Ñ");
                    objcitamedica[8] = dr[4].ToString();
                    objcitamedica[9] = FunGetMotivo(dr[8].ToString());
                    objcitamedica[10] = ViewState["Indentificacion"].ToString();
                    objcitamedica[11] = dr[13].ToString() == "T" ? "TITULAR" : "BENEFICIARIO";
                    objcitamedica[12] = dr[0].ToString().Replace("&#209;", "Ñ");
                    objdatostitu[0] = dr[13].ToString() == "T" ? 2 : 3;
                    objdatostitu[1] = dr[14].ToString();
                    objdatostitu[2] = dr[15].ToString();
                    objdatostitu[3] = 0;
                    dt = new Conexion(2, "").FunGetDatosTituBene(objdatostitu);
                    objcitamedica[13] = dt.Tables[0].Rows[0][0].ToString();
                    objcitamedica[14] = dt.Tables[0].Rows[0][2].ToString();
                    objcitamedica[15] = dt.Tables[0].Rows[0][3].ToString();
                    objcitamedica[20] = medicamentos;
                    objcitamedica[21] = Session["usuLogin"].ToString();
                    objcitamedica[22] = dr[20].ToString();
                    objcitamedica[23] = "";
                    nameFile = filePath + "CitaOdontologica_" + dr[5].ToString().Replace("/", "") + "_" + codigocita.ToString() + ".txt";
                    msjEmail = nameFile;

                    if (!string.IsNullOrEmpty(lblCelular.InnerText.Trim()))
                    {
                        //thrEnviarSMS = new Thread(new ThreadStart(FunEnviarSMS));
                        //thrEnviarSMS.Start();
                    }

                    //returnFile = new Funciones().funCrearArchivoCita(nameFile, objcitamedica);
                    returnFile = "";
                    msjEmail = new Funciones().funEnviarMail(mailsP, subject, objcitamedica, fileTemplate,
                        ViewState["Host"].ToString(), int.Parse(ViewState["Port"].ToString()), bool.Parse(ViewState["EnableSSl"].ToString()),
                        ViewState["Usuario"].ToString(), ViewState["Password"].ToString(), returnFile, fileLogo, mailsA, mailsD, mailsU,"","");
                }
                if (msjEmail == "") Response.Redirect("FrmCitaOdontoAdmin.aspx?MensajeRetornado='Cita(s) Agendada(s) con Éxito'", true);
                else
                {
                    mensaje = "Errores en el Envío, Revise el LOG de Errores";
                    new Funciones().funCrearLogAuditoria(1, "FrmAgendaCitaOdonto", msjEmail, 1);
                    Response.Redirect("FrmCitaOdontoAdmin.aspx?MensajeRetornado='" + mensaje + "'", true);
                }

            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                new Funciones().funCrearLogAuditoria(1, "Envío Mail", ex.Message, 1);
            }
        }

        private void FunEnviarSMS()
        {
            try
            {
                _enviarsms += "{\"phoneNumber\":\"" + lblCelular.InnerText + "\",";
                _enviarsms += "\"messageId\":" + 134056 + ",";
                _enviarsms += "\"transactionId\":\"" + ViewState["CodigoCita"].ToString() + "\",";
                _enviarsms += "\"dataVariable\":[\"" + ViewState["Paciente"].ToString() + "\",\"" + ViewState["FechaCita"].ToString() +
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

        private string FunGetMedicamento(int codigoProducto)
        {
            Array.Resize(ref objdatosmotivo, 3);
            objdatosmotivo[0] = codigoProducto;
            objdatosmotivo[1] = "";
            objdatosmotivo[2] = 94;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objdatosmotivo);
            if (dt.Tables[0].Rows.Count > 0) return dt.Tables[0].Rows[0][0].ToString();
            else return "NO";
        }
        private string FunMailsEnviar(int codigoprestadora)
        {
            sendmails = "";
            x = 0;
            Array.Resize(ref objsendmails, 3);
            objsendmails[0] = codigoprestadora;
            objsendmails[1] = "";
            objsendmails[2] = 52;
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
                sendmails = sendmails + dr[0].ToString().ToLower() + ",";
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
            objsendmails[2] = 53;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objsendmails);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][0].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][0].ToString().ToLower() + ",";
                if (!string.IsNullOrEmpty(dt.Tables[0].Rows[0][1].ToString())) sendmails = sendmails + dt.Tables[0].Rows[0][1].ToString().ToLower() + ",";
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
            objdatosmotivo[2] = 54;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objdatosmotivo);
            if (dt.Tables[0].Rows.Count > 0) return dt.Tables[0].Rows[0][0].ToString();
            else return "Consultar a PRESTASALUD S.A";

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
                System.Threading.Thread.Sleep(3000);
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
                //filePath = Server.MapPath("~" + "\\CitasCanceladasO\\");
                fileTemplate = Server.MapPath("~/Template/HtmlTemplateC.html");
                fileLogo = @ViewState["Ruta"].ToString() + ViewState["Logo"].ToString();
                subject = "Cancelar - Odontológico " + ViewState["Campaing"].ToString() + "-" + ViewState["Producto"].ToString();
                subject = subject.Replace('\r', ' ').Replace('\n', ' ');
                //fileLogo = "F:\\Logo\\LOGOPRESTASALUD.jpg";
                fileLogo = @ViewState["Ruta"].ToString() + ViewState["Logo"].ToString();
                objcitamedica[0] = codigoGenerado;
                objcitamedica[1] = ciudadCita;
                objcitamedica[2] = fechaCita;
                objcitamedica[3] = horaCita.Substring(0,5);
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
                returnFile = "";
                mensaje = new Funciones().funEnviarMailCancel(mailsP, "CANCELAR CITA", objcitamedica, fileTemplate,
                    ViewState["Host"].ToString(), int.Parse(ViewState["Port"].ToString()), bool.Parse(ViewState["EnableSSl"].ToString()),
                    ViewState["Usuario"].ToString(), ViewState["Password"].ToString(), returnFile, fileLogo, mailsA, mailsD, mailsU);
            }
            catch (Exception ex)
            {
                mensaje = ex.ToString();
            }
            return mensaje;
        }
        private void FunEliminarReservasOtraOpcion()
        {
            try
            {
                Array.Resize(ref objparam, 18);
                objparam[0] = 15;
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
            FunCascadaCombos(4);
            FunLimpiarCampos();
        }

        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            CalendarioCita.SelectedDate = DateTime.Today;
            FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd"));

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
            FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd"));
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
                //Buscar si ya existe alguna reserva realizada
                Array.Resize(ref objparam, 18);
                objparam[0] = 12;
                objparam[1] = int.Parse(ddlPrestadora.SelectedValue);
                objparam[2] = int.Parse(ddlMedico.SelectedValue);
                objparam[3] = int.Parse(ddlEspecialidad.SelectedValue);
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
                    FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                    return;
                }
                //Buscar si ya existe alguna cita medica realizada
                objparam[0] = 13;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    new Funciones().funShowJSMessage("Ya Existe una cita médica realizada, seleccione otra fecha/hora..!", this);
                    FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
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
                        DataRow result = tblbuscar.Select("Cliente='" + ViewState["Cliente"].ToString() + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Titular/Beneficiario ya existe registrado para la cita..!", this);
                    return;
                }

                //Preguntar si tiene PRESPUESTO INICIADO O EN PROCESO
                Array.Resize(ref objparam, 7);
                objparam[0] = 3;
                objparam[1] = 0;
                objparam[2] = int.Parse(ViewState["TituCodigo"].ToString());
                objparam[3] = int.Parse(ViewState["BeneCodigo"].ToString());
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (ddlMotivoCita.SelectedValue == "PR")
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        new Funciones().funShowJSMessage("Cliente ya tiene iniciado un PRESUPUESTO..!", this);
                        return;
                    }
                }
                if (ddlMotivoCita.SelectedValue != "PR")
                {
                    if (dt.Tables[0].Rows.Count == 0)
                    {
                        new Funciones().funShowJSMessage("Cliente no tiene iniciado un PRESUPUESTO..!", this);
                        return;
                    }
                }

                objparam[0] = 5;
                //PREGUNTAR SI EL CLIENTE YA TIENE AGENDADA UNA CITA EN ESTADO ACTIVO
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    estadoActual = dt.Tables[0].Rows[0][0].ToString();
                    codigocita = int.Parse(dt.Tables[0].Rows[0][1].ToString());
                }
                if (estadoActual == "A")
                {
                    new Funciones().funShowJSMessage("Cliente tiene una Cita Activa..!", this);
                    return;
                }
                else
                {
                    objparam[0] = 14;
                    objparam[1] = codigocita;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                    if (dt.Tables[0].Rows.Count > 0) ViewState["UltFechaAgendamiento"] = dt.Tables[0].Rows[0][0].ToString();
                    else ViewState["UltFechaAgendamiento"] = "";
                }
                //REALIZAR EL CONTEO DE LAS CITAS SEGUN LA PROGRAMACION DE SU PRODUCTO
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoGrupo"].ToString());
                objparam[1] = "Ox";
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
                        fechaFinCobertura = DateTime.ParseExact(ViewState["FechaFinCobertura"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (fechaFinCobertura < fechaSistema)
                        {
                            new Funciones().funShowJSMessage("Fecha de Cobertura Finalizada..!", this);
                            return;
                        }
                    }
                    //CONSULTAR LA CANTIDAD DE CITAS REALIZAD POR AÑO
                    Array.Resize(ref objparam, 11);
                    aniocita = fechaSistema.Year;
                    objparam[0] = 1;
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
                    objparam[0] = 0;
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
                //DETERMINAR SI LA FECHA SELECCIONADA ESTA PERMITIDA PARA LA SIGUIENTE CITA
                //if (!string.IsNullOrEmpty(ViewState["UltFechaAgendamiento"].ToString()))
                //{
                //    if (ddlMotivoCita.SelectedValue != "OD" && ddlMotivoCita.SelectedValue != "PN")
                //    {
                //        fechaultimoevento = ViewState["UltFechaAgendamiento"].ToString();
                //        //TRAER LOS DIAS DE ATENCION
                //        Array.Resize(ref objparam, 1);
                //        objparam[0] = 29;
                //        dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                //        if (dt.Tables[0].Rows.Count == 0) dias = 30;
                //        else dias = int.Parse(dt.Tables[0].Rows[0][0].ToString());

                //        fechacalendario = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                //        fechanext = DateTime.ParseExact(fechaultimoevento, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //        fechanext = fechanext.AddDays(dias);
                //        fechacalen = DateTime.ParseExact(fechacalendario, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //        if (fechacalen < fechanext & estadoActual != "C")
                //        {
                //            string alerta = "Cliente puede agendar a partir del " + fechanext.ToString("yyyy/MM/dd");
                //            ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('" + alerta + "')", true);
                //            return;
                //        }
                //    }
                //}
                Array.Resize(ref objparam, 18);
                objparam[0] = 16;
                objparam[1] = int.Parse(ddlPrestadora.SelectedValue);
                objparam[2] = int.Parse(ddlMedico.SelectedValue);
                objparam[3] = int.Parse(ddlEspecialidad.SelectedValue);
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
                    filagre["PreeCodigo"] = int.Parse(ddlPrestadora.SelectedValue);
                    filagre["MediCodigo"] = int.Parse(ddlMedico.SelectedValue);
                    filagre["EspeCodigo"] = int.Parse(ddlEspecialidad.SelectedValue);
                    filagre["HodeCodigo"] = int.Parse(grdvDatosCitas.DataKeys[intIndex].Values["HodeCodigo"].ToString());
                    filagre["TipoCliente"] = ViewState["TipoCliente"].ToString();
                    filagre["TituCodigo"] = int.Parse(ViewState["TituCodigo"].ToString());
                    filagre["BeneCodigo"] = int.Parse(ViewState["BeneCodigo"].ToString());
                    filagre["CodParentesco"] = ViewState["CodParentesco"].ToString();
                    filagre["EstatusCita"] = ddlOpcion.SelectedValue;
                    filagre["Longitud"] = "";
                    filagre["Latitud"] = "";
                    filagre["Observacion"] = txtObservacion.Text.Trim().ToUpper();
                    filagre["FechaCitax"] = CalendarioCita.SelectedDate.ToString("dd/MM/yyyy");

                    tblagre.Rows.Add(filagre);
                    ViewState["tbCitaMedica"] = tblagre;
                    grdvResumenCita.DataSource = tblagre;
                    grdvResumenCita.DataBind();
                    imgCancelar.Enabled = true;
                    FunLimpiarAgendamiento();
                    FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
                }
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
                            btnAgenda.ImageUrl = "~/Botones/muelaenable.png";
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
                int especodigo = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["EspeCodigo"].ToString());
                string fechacita = grdvResumenCita.DataKeys[intIndex].Values["FechaCita"].ToString();
                //string fechacita = grdvResumenCita.Rows[intIndex].Cells[6].Text;
                string diacita = DateTime.ParseExact(fechacita, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("dddd");
                Array.Resize(ref objparam, 18);
                objparam[0] = 14;
                objparam[1] = preecodigo;
                objparam[2] = medicodigo;
                objparam[3] = especodigo;
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
                    FunAgendaHoras(0, preecodigo, medicodigo, especodigo, fechacita, diacita);
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('Reserva Cancelada!..');", true);
                    ViewState["Cliente"] = null;
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
                int preecodigo = int.Parse(grdvResumenCita.DataKeys[intIndex].Values["PreeCodigo"].ToString());
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestaOdonto.aspx?CodigoPrestadora=" + preecodigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgAgendar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tbCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                if (tbCitaMedica.Rows.Count > 0)
                {
                    string[] columnas = new[] { "PreeCodigo", "MediCodigo", "EspeCodigo", "TipoCliente", "TituCodigo", "BeneCodigo", "CodParentesco",
                "EstatusCita","FechaCita","DiaCita","Hora","HodeCodigo","Detalle","Longitud","Latitud","Observacion"};
                    tbNuevaCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                    tbMailCitaMedica = (DataTable)ViewState["tbCitaMedica"];
                    DataView view = new DataView(tbNuevaCitaMedica);
                    tbNuevaCitaMedica = view.ToTable(true, columnas);
                    Array.Resize(ref objparam, 5);
                    objparam[0] = 0;
                    objparam[1] = int.Parse(Session["CodigoProducto"].ToString());
                    objparam[2] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[3] = Session["MachineName"].ToString();
                    objparam[4] = txtObservacionG.Text.Trim().ToUpper();
                    mensaje = new Conexion(2, "").FunAgendaCitaOdonto(objparam, tbNuevaCitaMedica);
                    if (mensaje == "")
                    {
                        Session["SalirAgenda"] = "SI";
                        FunEnviarMailCita(tbMailCitaMedica);
                    }
                }
                else FunShowJSMessage("Seleccione Datos para Agendar la Cita");
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
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
                    tbCitaMedica.Columns.Add("CodigoPrestadora");
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

        protected void grdvHistorialCitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    btnEstado = (ImageButton)(e.Row.Cells[0].FindControl("imgEstado"));
                    chkCancelar = (CheckBox)(e.Row.Cells[6].FindControl("chkSelecc"));
                    status = grdvHistorialCitas.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    codigocita = int.Parse(grdvHistorialCitas.DataKeys[e.Row.RowIndex].Values["CitaCodigo"].ToString());
                    //PREGUNTAR SI YA EXISTE ALGUN PROCEDIMIENTO REGISTRADO
                    Array.Resize(ref objparam, 3);
                    objparam[0] = codigocita;
                    objparam[1] = "";
                    objparam[2] = 92;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    nuevoEstado = dt.Tables[0].Rows[0][0].ToString();
                    //if (!string.IsNullOrEmpty(nuevoEstado)) status = nuevoEstado;
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
                        case "F":
                            btnEstado.ImageUrl = "~/Botones/procesofinalizado.jpg";
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
                objdatoscancel[0] = 1;
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
                if (mensaje == "") Response.Redirect("FrmCitaOdontoAdmin.aspx?MensajeRetornado='Cita(s) Cancelada(s) con Éxito'", false);
                else Response.Redirect("FrmCitaOdontoAdmin.aspx?MensajeRetornado='Revise Mails, hubo errores en el envío..!'", false);
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
                if (int.Parse(ddlMedico.SelectedValue) > 0) FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void lnkActualizar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updDatosPersonales, GetType(), "Actualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmActualizarDatosO.aspx?CodTitular=" + Session["CodigoTitular"].ToString() + "&CodProducto=" + Session["CodigoProducto"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=600px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void imgHorarios_Click(object sender, ImageClickEventArgs e)
        {
            if (int.Parse(ddlMedico.SelectedValue) == 0)
            {
                lblerror.Text = "Seleccione Médico para mostrar datos..!";
                return;
            }
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosOdontoHorarios.aspx?CodigoMedico=" + ddlMedico.SelectedValue + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ddlOpcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAgendamientos.Visible = false;
            pnlResumenCita.Visible = false;
            CalendarioCita.Visible = false;
            if (ddlOpcion.SelectedValue == "A" || ddlOpcion.SelectedValue == "I")
            {
                if (int.Parse(ddlMedico.SelectedValue) > 0) FunAgendaHoras(0, int.Parse(ddlPrestadora.SelectedValue), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlEspecialidad.SelectedValue), CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), CalendarioCita.SelectedDate.ToString("dddd").Trim().ToUpper());
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
                objparam[2] = 55;
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
            Response.Redirect("FrmCitaOdontoAdmin.aspx", true);
        }
        protected void imgPrestadora_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlPrestadora.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Prestadora para mostrar datos..!";
                return;
            }
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestaOdonto.aspx?CodigoPrestadora=" + ddlPrestadora.SelectedValue + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }
        protected void imgBuscarPresta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int preecodigo = int.Parse(grdvHistorialCitas.DataKeys[intIndex].Values["PrestaCodigo"].ToString());
                ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDatosPrestaOdonto.aspx?CodigoPrestadora=" + preecodigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=800px, height=300px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}