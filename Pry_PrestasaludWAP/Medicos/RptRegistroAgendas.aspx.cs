namespace Pry_PrestasaludWAP.Medicos
{
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;

    public partial class RptRegistroAgendas : System.Web.UI.Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataTable dtb = new DataTable();
        Object[] objparam = new Object[1];
        string pathLogoCabecera = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                lbltitulo.Text = "REPORTE CITAS MEDICAS REGISTRADAS";
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["Regresar"] = Request["Regresar"];
                rptCitasMedicas.Visible = true;
                funCargaMantenimiento();
            }
        }
        #endregion

        #region Funciones y Procedimientos
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[1] = "";
                objparam[2] = 122;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["Titular"] = dts.Tables[0].Rows[0]["Titular"].ToString();
                ViewState["Paciente"] = dts.Tables[0].Rows[0]["Paciente"].ToString();
                ViewState["Parentesco"] = dts.Tables[0].Rows[0]["Parentesco"].ToString();
                ViewState["Prestadora"] = dts.Tables[0].Rows[0]["Prestadora"].ToString();
                ViewState["Direccion"] = dts.Tables[0].Rows[0]["Direccion"].ToString();
                ViewState["Telefono"] = dts.Tables[0].Rows[0]["Telefono"].ToString();
                ViewState["Medico"] = dts.Tables[0].Rows[0]["Medico"].ToString();
                ViewState["Especialidad"] = dts.Tables[0].Rows[0]["Especialidad"].ToString();
                ViewState["CodigoTitu"] = dts.Tables[0].Rows[0]["CodigoTitu"].ToString();
                ViewState["CodigoBene"] = dts.Tables[0].Rows[0]["CodigoBene"].ToString();
                Array.Resize(ref objparam, 7);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoBene"].ToString());
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = 0;
                objparam[6] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_GetCitasAgendadas", objparam);
                if (dts.Tables[0].Rows.Count > 0) funGenerarReporte(dts.Tables[0]);
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }

        private void funGenerarReporte(DataTable dtp)
        {
            try
            {
                rptCitasMedicas.Reset();
                rptCitasMedicas.SizeToReportContent = true;
                rptCitasMedicas.LocalReport.ReportPath = Server.MapPath("~/Reports/RptAgendasMedicas.rdlc");
                rptCitasMedicas.LocalReport.EnableExternalImages = true;
                pathLogoCabecera = new Uri(Server.MapPath(Session["LogoC"].ToString())).AbsoluteUri;
                rptCitasMedicas.LocalReport.DataSources.Clear();
                ReportDataSource _rsource = new ReportDataSource("dtsCitasAgendadas", dtp);
                rptCitasMedicas.LocalReport.DataSources.Add(_rsource);
                ReportParameter[] parametros = new ReportParameter[]
                {
                    new ReportParameter("Titular",ViewState["Titular"].ToString()),
                    new ReportParameter("Paciente",ViewState["Paciente"].ToString()),
                    new ReportParameter("Parentesco",ViewState["Parentesco"].ToString()),
                    new ReportParameter("Prestadora",ViewState["Prestadora"].ToString()),
                    new ReportParameter("Direccion",ViewState["Direccion"].ToString()),
                    new ReportParameter("Telefono",ViewState["Telefono"].ToString()),
                    new ReportParameter("Medico",ViewState["Medico"].ToString()),
                    new ReportParameter("Especialidad",ViewState["Especialidad"].ToString()),
                    new ReportParameter("FechaReporte",DateTime.Now.ToString("MM-dd-yyyy HH:mm")),
                    new ReportParameter("LogoC",pathLogoCabecera)
                };
                rptCitasMedicas.LocalReport.SetParameters(parametros);
                rptCitasMedicas.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        #endregion

        #region Botones y Eventos
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            switch (ViewState["Regresar"].ToString())
            {
                case "0":
                    Response.Redirect("FrmGestionesRealizadasAdmin.aspx", true);
                    break;
                case "1":
                    Response.Redirect("../Administrador/FrmGesReaMedAdmin.aspx", true);
                    break;
            }            
        }
        #endregion
    }
}