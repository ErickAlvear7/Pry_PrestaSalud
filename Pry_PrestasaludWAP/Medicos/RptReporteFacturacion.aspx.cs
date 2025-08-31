namespace Pry_PrestasaludWAP.Medicos
{
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;
    using System.Globalization;

    public partial class RptReporteFacturacion : System.Web.UI.Page
    {
        #region Variables
        DataSet dtsFactura = new DataSet();
        DataSet dtsCabecera = new DataSet();
        DataSet dts = new DataSet();
        DataTable dtl = new DataTable();
        Object[] objparam = new Object[1];
        string pathLogoCabecera = "", pathLogoPie = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                txtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                rptFacturacion.Visible = false;
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 124;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["Medico"] = dts.Tables[0].Rows[0][0].ToString();
                ViewState["CodigoMedico"] = dts.Tables[0].Rows[0][1].ToString();
                lbltitulo.Text = "Reporte Facturación : " + ViewState["Medico"].ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunGenerarReporte(DataTable dtpCabecera, DataTable dtpFactura)
        {
            try
            {
                rptFacturacion.Reset();
                rptFacturacion.SizeToReportContent = true;
                rptFacturacion.LocalReport.ReportPath = Server.MapPath("~/Reports/RptFacturaPrestaMedicoNew.rdlc");
                rptFacturacion.LocalReport.EnableExternalImages = true;
                rptFacturacion.LocalReport.DataSources.Clear();
                ReportDataSource dtrCabecera = new ReportDataSource("dsCabecera", dtpCabecera);
                ReportDataSource dtrFactura = new ReportDataSource("dsFactura", dtpFactura);
                rptFacturacion.LocalReport.DataSources.Add(dtrCabecera);
                rptFacturacion.LocalReport.DataSources.Add(dtrFactura);

                //ReportParameter[] parameters = new ReportParameter[6];
                //parameters[0] = new ReportParameter("Cedente", DdlCedente.SelectedItem.ToString());
                //parameters[1] = new ReportParameter("Catalogo", DdlCatalogo.SelectedItem.ToString());
                //parameters[2] = new ReportParameter("FechaD", TxtFechaIni.Text.Trim());
                //parameters[3] = new ReportParameter("FechaH", TxtFechaFin.Text.Trim());
                //parameters[4] = new ReportParameter("Operador", DdlGestor.SelectedValue == "0" ? "TODOS" : DdlGestor.SelectedItem.ToString());
                //parameters[5] = new ReportParameter("Logo", _imagepath);
                //RptViewDatos.LocalReport.SetParameters(parameters);

                //ReportParameter[] parametros = new ReportParameter[]
                //{
                //    new ReportParameter("FechaDesde",txtFechaDesde.Text),
                //    new ReportParameter("FechaHasta",txtFechaHasta.Text),
                //    new ReportParameter("Medico",ViewState["Medico"].ToString()),
                //    new ReportParameter("FechaReporte",DateTime.Now.ToString("MM-dd-yyyy HH:mm")),
                //    new ReportParameter("LogoC",pathLogoCabecera)
                //};
                //rptFacturacion.LocalReport.SetParameters(parametros);
                rptFacturacion.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        #endregion

        #region Botones y Eventos
        protected void ddlTipoReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            rptFacturacion.Visible = false;
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            try
            {
                if (ddlTipoReporte.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Tipo de Reporte..!", this);
                    return;
                }
                if (!new Funciones().IsDate(txtFechaDesde.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    return;
                }
                if (!new Funciones().IsDate(txtFechaHasta.Text))
                {
                    new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                    return;
                }
                if (DateTime.ParseExact(txtFechaDesde.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaHasta.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                {
                    new Funciones().funShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                    return;
                }

                pathLogoCabecera = new Uri(Server.MapPath(Session["LogoC"].ToString())).AbsoluteUri;
                pathLogoPie = "";

                Array.Resize(ref objparam, 12);
                objparam[0] = 1;
                objparam[1] = txtFechaDesde.Text.Trim();
                objparam[2] = txtFechaHasta.Text.Trim();
                objparam[3] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[4] = pathLogoCabecera;
                objparam[5] = pathLogoPie;
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = 0;
                objparam[10] = 0;
                objparam[11] = 0;
                dtsCabecera = new Conexion(2, "").funConsultarSqls("sp_RepFacturaCabecera", objparam);

                Array.Resize(ref objparam, 8);
                objparam[0] = ddlTipoReporte.SelectedValue == "1" ? 0 : 1;
                objparam[1] = txtFechaDesde.Text;
                objparam[2] = txtFechaHasta.Text;
                objparam[3] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = 0;
                dtsFactura = new Conexion(2, "").funConsultarSqls("sp_ReporteFacturacionMedico", objparam);
                if (dtsFactura.Tables[0].Rows.Count > 0)
                {
                    rptFacturacion.Visible = true;
                    FunGenerarReporte(dtsCabecera.Tables[0], dtsFactura.Tables[0]);
                }
                else
                {
                    rptFacturacion.Visible = false;
                    new Funciones().funShowJSMessage("No existe registros en ese período..!", this);
                    objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                    Array.Resize(ref objparam, 3);
                    objparam[1] = "";
                    objparam[2] = 100;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                }

                //dtsFactura = new Conexion(2, "").FuConsultarSOLOFacturasNew(objparam);
                //if (dtsFactura.Tables[0].Rows.Count > 0)
                //{
                //    rptFacturacion.Visible = true;
                //    FunGenerarReporte(dtsCabecera.Tables[0], dtsFactura.Tables[0]);
                //}
                //else
                //{
                //    rptFacturacion.Visible = false;
                //    new Funciones().funShowJSMessage("No existe registros en ese período..!", this);
                //    objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                //    Array.Resize(ref objparam, 3);
                //    objparam[1] = "";
                //    objparam[2] = 100;
                //    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                //}


            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}