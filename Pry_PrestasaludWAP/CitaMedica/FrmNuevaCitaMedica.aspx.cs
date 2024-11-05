using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmNuevaCitaMedica : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        DataTable tbTurnoMedico = new DataTable();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["usuCodigo"] = "1";
                Session["MachineName"] = "Terminal1";
                CalendarioCita.SelectedDate = DateTime.Today;
                lbltitulo.Text = "Agendar Cita";
                ViewState["CodigoTitular"] = Request["Codigo"];
                ViewState["CodigoProducto"] = Request["CodProducto"];
                funCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 1;
                objparam[1] = int.Parse(ViewState["CodigoTitular"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularEdit", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    txtNombres.Text = dt.Tables[0].Rows[0]["Nombres"].ToString();
                    txtApellidos.Text = dt.Tables[0].Rows[0]["Apellidos"].ToString();
                    txtGenero.Text = dt.Tables[0].Rows[0]["Genero"].ToString();
                    txtEstadoCivil.Text = dt.Tables[0].Rows[0]["EstadoCivil"].ToString();
                }

                Array.Resize(ref objparam, 1);
                objparam[0] = 6;
                ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlProvincia.DataTextField = "Descripcion";
                ddlProvincia.DataValueField = "Codigo";
                ddlProvincia.DataBind();

                Array.Resize(ref objparam, 3);
                objparam[0] = ddlProvincia.SelectedValue;
                objparam[1] = "";
                objparam[2] = 4;
                ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlCiudad.DataTextField = "Descripcion";
                ddlCiudad.DataValueField = "Codigo";
                ddlCiudad.DataBind();

                objparam[0] = ddlCiudad.SelectedValue;
                objparam[1] = "";
                objparam[2] = 5;
                ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlPrestadora.DataTextField = "Descripcion";
                ddlPrestadora.DataValueField = "Codigo";
                ddlPrestadora.DataBind();

                Array.Resize(ref objparam, 3);
                objparam[0] = ddlPrestadora.SelectedIndex > 0 ? int.Parse(ddlPrestadora.SelectedValue) : 0;
                objparam[1] = "";
                objparam[2] = 7;
                ddlMedico.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlMedico.DataTextField = "Descripcion";
                ddlMedico.DataValueField = "Codigo";
                ddlMedico.DataBind();

                objparam[0] = ddlPrestadora.SelectedIndex > 0 ? int.Parse(ddlPrestadora.SelectedValue) : 0;
                objparam[1] = "";
                objparam[2] = 8;
                ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlEspecialidad.DataTextField = "Descripcion";
                ddlEspecialidad.DataValueField = "Codigo";
                ddlEspecialidad.DataBind();

                objparam[0] = int.Parse(ViewState["CodigoTitular"].ToString());
                objparam[1] = "";
                objparam[2] = 11;
                ddlBeneficiario.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlBeneficiario.DataTextField = "Descripcion";
                ddlBeneficiario.DataValueField = "Codigo";
                ddlBeneficiario.DataBind();
            }
            catch(Exception ex)
            {
                lblerror.Text = ex.ToString();
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCitaMedica", ex.ToString(), frame.GetFileLineNumber());
            }
        }
        private void funAgendaHoras(string strDia, int intCodMedico, int intTurnoCod)
        {
            try
            {
                grdvDatosCitas.DataSource = null;
                grdvDatosCitas.DataBind();
                Array.Resize(ref objparam, 4);
                objparam[0] = 0;
                objparam[1] = intCodMedico;
                objparam[2] = intTurnoCod;
                objparam[3] = strDia;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarAgendarHoras", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatosCitas.DataSource = dt;
                    grdvDatosCitas.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCitaMedica", ex.ToString(), frame.GetFileLineNumber());
            }
        }

        private void funCargarHorarios(int intTurno)
        {
            try
            {
                grdvDatosHoras.DataSource = null;
                grdvDatosHoras.DataBind();
                Array.Resize(ref objparam, 3);
                objparam[0] = intTurno;
                objparam[1] = "";
                objparam[2] = 10;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatosHoras.DataSource = dt;
                    grdvDatosHoras.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCitaMedica", ex.ToString(), frame.GetFileLineNumber());
            }
        }
        private void funLimpiarCampos()
        {
            lblerror.Text = "";
            txtMedico.Text = "";
            txtDiaHora.Text = "";
        }
        private void funCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    ddlCiudad.Items.Clear();
                    ddlPrestadora.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlProvincia.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 4;
                    ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlCiudad.DataTextField = "Descripcion";
                    ddlCiudad.DataValueField = "Codigo";
                    ddlCiudad.DataBind();
                    break;
                case 1:
                    ddlPrestadora.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlCiudad.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 5;
                    ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlPrestadora.DataTextField = "Descripcion";
                    ddlPrestadora.DataValueField = "Codigo";
                    ddlPrestadora.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlTurno.Items.Clear();
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ddlMedico.SelectedValue);
            objparam[1] = "";
            objparam[2] = 9;
            ddlTurno.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            ddlTurno.DataTextField = "Descripcion";
            ddlTurno.DataValueField = "Codigo";
            ddlTurno.DataBind();
            funLimpiarCampos();
        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTurno.SelectedItem.ToString() != "--Seleccione Turno---")
            {
                funCargarHorarios(int.Parse(ddlTurno.SelectedValue));
                funAgendaHoras(CalendarioCita.SelectedDate.ToString("dddd").ToUpper(), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlTurno.SelectedValue));
            }
        }

        protected void CalendarioCita_SelectionChanged(object sender, EventArgs e)
        {
            funLimpiarCampos();
            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaCalendar = DateTime.ParseExact(CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"),"MM/dd/yyyy",CultureInfo.InvariantCulture);
            if (dtmFechaCalendar < dtmFechaActual)
            {
                lblerror.Text = "La fecha de la cita no puede ser menor a la fecha actual";
                return;
            }
            funAgendaHoras(CalendarioCita.SelectedDate.ToString("dddd").ToUpper(), int.Parse(ddlMedico.SelectedValue), int.Parse(ddlTurno.SelectedValue));
        }

        protected void imgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            lblerror.Text = "";
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            if (ddlEspecialidad.SelectedIndex == 0)
            {
                lblerror.Text = "Seleccione Especialidad..!";
                return;
            }
            if (ddlAgendar.SelectedValue == "N")
            {
                lblerror.Text = "Seleccione Agendar a..!";
                return; 
            }
            if (ddlAgendar.SelectedValue == "B")
            {
                if (ddlBeneficiario.SelectedIndex == 0)
                {
                    lblerror.Text = "Seleccione Beneficiario..!";
                    return;
                }
            }

            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaCalendar = DateTime.ParseExact(CalendarioCita.SelectedDate.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture);
            if (dtmFechaCalendar < dtmFechaActual)
            {
                lblerror.Text = "La fecha de la cita no puede ser menor a la fecha actual";
                return;
            }

            ViewState["HoraCita"] = grdvDatosCitas.DataKeys[intIndex].Values["HoraInicio"].ToString();
            txtDiaHora.Text = CalendarioCita.SelectedDate.ToString("dddd").ToUpper() + "," +
                        CalendarioCita.SelectedDate.ToString("dd") + " de " + CalendarioCita.SelectedDate.ToString("MMMM").ToString() +
                        " De: " + grdvDatosCitas.DataKeys[intIndex].Values["HoraInicio"].ToString() + " a " +
                        grdvDatosCitas.DataKeys[intIndex].Values["HoraFinal"].ToString();
            txtMedico.Text = ddlMedico.SelectedItem.ToString();
        }

        protected void ddlAgendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlAgendar.SelectedValue=="B")
            {
                lblBeneficiario.Visible = true;
                ddlBeneficiario.Visible = true;
            }
            else
            {
                lblBeneficiario.Visible = false;
                ddlBeneficiario.Visible = false;
            }
        }
        protected void imgAgendar_Click(object sender, ImageClickEventArgs e)
        {
            if (txtDiaHora.Text == "")
            {
                lblerror.Text = "Seleccione todas las opciones para realizar el Agendamiento..!";
                return;
            }
            if (txtDetalleCita.Text == "")
            {
                lblerror.Text = "Ingrese Descripción de la cita..!";
                return;
            }
            try
            {
                Array.Resize(ref objparam, 10);
                objparam[0] = int.Parse(ddlMedico.SelectedValue);
                objparam[1] = int.Parse(ddlEspecialidad.SelectedValue);
                objparam[2] = ddlAgendar.SelectedValue;
                objparam[3] = ddlAgendar.SelectedValue == "T" ? int.Parse(ViewState["CodigoTitular"].ToString()) : int.Parse(ddlBeneficiario.SelectedValue);
                objparam[4] = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                objparam[5] = ViewState["HoraCita"].ToString();
                objparam[6] = txtDetalleCita.Text.ToUpper();
                objparam[7] = "A";
                objparam[8] = int.Parse(Session["usuCodigo"].ToString());
                objparam[9] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_NuevaCitaMedica", objparam);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    string response = string.Format("{0}?Codigo={1}&CodPrestadora={2}&MensajeRetornado={3}", Request.Url.AbsolutePath, ViewState["CodigoTitular"].ToString(),
                        ViewState["CodigoPrestadora"].ToString(), "Cita Agendada Correctamente");
                    Response.Redirect(response);
                }
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCitaMedica", ex.ToString(), frame.GetFileLineNumber());
            }
        }

        protected void grdvDatosCitas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblAgen = new Label();
            Label lblCli = new Label();
            Label lblTip = new Label();
            Label lblPar = new Label();
            ImageButton imgCita = new ImageButton();
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    lblAgen = (Label)(e.Row.Cells[2].FindControl("lblAgendado"));
                    lblCli = (Label)(e.Row.Cells[3].FindControl("lblCliente"));
                    lblTip = (Label)(e.Row.Cells[4].FindControl("lblTipo"));
                    lblPar = (Label)(e.Row.Cells[5].FindControl("lblParentesco"));
                    imgCita = (ImageButton)(e.Row.Cells[5].FindControl("imgSelecc"));

                    int medicod = int.Parse(ddlMedico.SelectedValue);
                    string fechaCita = CalendarioCita.SelectedDate.ToString("MM/dd/yyyy");
                    string horaCita = grdvDatosCitas.DataKeys[e.Row.RowIndex].Values["HoraInicio"].ToString();

                    Array.Resize(ref objparam, 3);
                    objparam[0] = medicod;
                    objparam[1] = fechaCita;
                    objparam[2] = horaCita;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargarDatosCitas", objparam);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        imgCita.ImageUrl = "~/Botones/citamedicadesac.png";
                        imgCita.Height = 25;
                        imgCita.Width = 25;
                        imgCita.Enabled = false;
                        lblAgen.Text = dt.Tables[0].Rows[0][0].ToString();
                        lblCli.Text = dt.Tables[0].Rows[0][1].ToString();
                        lblTip.Text = dt.Tables[0].Rows[0][2].ToString();
                        lblPar.Text = dt.Tables[0].Rows[0][3].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(0);
                new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevaCitaMedica", ex.ToString(), frame.GetFileLineNumber());
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCitaMedicaAdmin.aspx?codPrestadora=" + ViewState["CodigoPrestadora"].ToString(), false);
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            ddlMedico.SelectedIndex = 0;
            ddlTurno.Items.Clear();
            ddlEspecialidad.SelectedIndex = 0;
            ddlAgendar.SelectedIndex = 0;
            lblBeneficiario.Visible = false;
            ddlBeneficiario.Visible = false;
            ddlBeneficiario.SelectedIndex = 0;
            CalendarioCita.SelectedDate = DateTime.Today;
            grdvDatosCitas.DataSource = null;
            grdvDatosCitas.DataBind();
            grdvDatosHoras.DataSource = null;
            grdvDatosHoras.DataBind();
            txtDiaHora.Text = "";
            txtMedico.Text = "";
            txtDetalleCita.Text = "";
        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(0);
            funCascadaCombos(1);
        }
        protected void ddlPrestadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMedico.Items.Clear();
            ddlTurno.Items.Clear();
            funLimpiarCampos();
            Array.Resize(ref objparam, 3);
            objparam[0] = ddlPrestadora.SelectedValue;
            objparam[1] = "";
            objparam[2] = 7;
            ddlMedico.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            ddlMedico.DataTextField = "Descripcion";
            ddlMedico.DataValueField = "Codigo";
            ddlMedico.DataBind();
        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(1);
        }
        #endregion

    }
}