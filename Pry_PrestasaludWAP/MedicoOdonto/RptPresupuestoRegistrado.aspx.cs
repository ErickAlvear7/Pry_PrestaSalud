namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using Microsoft.Reporting.WebForms;
    using System;
    using System.Data;

    public partial class RptPresupuestoRegistrado : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtl = new DataTable();
        Object[] objparam = new Object[1];
        int edad = 0;
        string ondotoGram = "", pathLogoCabecera = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                lbltitulo.Text = "REPORTE PRESUPUESTOS REGISTRADOS";
                ViewState["CodigoCabecera"] = Request["CodigoCabecera"];
                ViewState["CodigoPrestadora"] = Request["CodigoPrestadora"];
                ViewState["Regresar"] = Request["Regresar"];
                rptPresupuesto.Visible = true;
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
                objparam[0] = int.Parse(ViewState["CodigoCabecera"].ToString());
                objparam[1] = ViewState["CodigoPrestadora"].ToString();
                objparam[2] = 81;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["Titular"] = dt.Tables[0].Rows[0]["Titular"].ToString();
                ViewState["Paciente"] = dt.Tables[0].Rows[0]["Paciente"].ToString();
                ViewState["Parentesco"] = dt.Tables[0].Rows[0]["Parentesco"].ToString();
                ViewState["Prestadora"] = dt.Tables[0].Rows[0]["Prestadora"].ToString();
                ViewState["Direccion"] = dt.Tables[0].Rows[0]["Direccion"].ToString();
                ViewState["Telefono"] = dt.Tables[0].Rows[0]["Telefono"].ToString();
                ViewState["Medico"] = dt.Tables[0].Rows[0]["Medico"].ToString();
                ViewState["CodigoMedico"] = dt.Tables[0].Rows[0]["CodigoMedico"].ToString();
                ViewState["TipoCliente"] = dt.Tables[0].Rows[0]["TipoCliente"].ToString();
                ViewState["CodigoTitu"] = dt.Tables[0].Rows[0]["CodigoTitu"].ToString();
                ViewState["CodigoBene"] = dt.Tables[0].Rows[0]["CodigoBene"].ToString();
                ViewState["Odontogram"] = "~/Images/Odonto_Adultos.jpg";
                if (ViewState["TipoCliente"].ToString() == "B")
                {
                    objparam[0] = int.Parse(ViewState["CodigoBene"].ToString());
                    objparam[1] = "";
                    objparam[2] = 119;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    edad = int.Parse(dt.Tables[0].Rows[0]["Edad"].ToString());
                    if (edad < 18) ViewState["Odontogram"] = "~/Images/Odonto_Pediatrico.jpg";
                }
                //TOTALIZAR VALORES
                objparam[0] = int.Parse(ViewState["CodigoCabecera"].ToString());
                objparam[1] = ViewState["CodigoPrestadora"].ToString();
                objparam[2] = 82;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["TotalPVP"] = dt.Tables[0].Rows[0][0].ToString();
                ViewState["TotalCosto"] = dt.Tables[0].Rows[0][1].ToString();
                ViewState["TotalPagar"] = dt.Tables[0].Rows[0][2].ToString();

                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoCabecera"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_GetPresupuestos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    //dtl = dt.Tables[0].AsEnumerable().Where(d => d["CodigoCabecera"].ToString() == ViewState["CodigoCabecera"].ToString()).CopyToDataTable();
                    funGenerarReporte(dt.Tables[0]);
                }
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
                rptPresupuesto.Reset();
                rptPresupuesto.SizeToReportContent = true;
                rptPresupuesto.LocalReport.ReportPath = Server.MapPath("~/Reports/RptPresupuestoRegistrado.rdlc");
                rptPresupuesto.LocalReport.EnableExternalImages = true;
                pathLogoCabecera = new Uri(Server.MapPath(Session["LogoC"].ToString())).AbsoluteUri;
                ondotoGram = new Uri(Server.MapPath(ViewState["Odontogram"].ToString())).AbsoluteUri;
                rptPresupuesto.LocalReport.DataSources.Clear();
                ReportDataSource _rsource = new ReportDataSource("dtReporte", dtp);
                rptPresupuesto.LocalReport.DataSources.Add(_rsource);
                ReportParameter[] parametros = new ReportParameter[]
                {
                    new ReportParameter("Titular",ViewState["Titular"].ToString()),
                    new ReportParameter("Paciente",ViewState["Paciente"].ToString()),
                    new ReportParameter("Parentesco",ViewState["Parentesco"].ToString()),
                    new ReportParameter("Prestadora",ViewState["Prestadora"].ToString()),
                    new ReportParameter("Direccion",ViewState["Direccion"].ToString()),
                    new ReportParameter("Telefono",ViewState["Telefono"].ToString()),
                    new ReportParameter("CodigoCabecera",ViewState["CodigoCabecera"].ToString()),
                    new ReportParameter("TotalPVP",ViewState["TotalPVP"].ToString()),
                    new ReportParameter("TotalCosto",ViewState["TotalCosto"].ToString()),
                    new ReportParameter("TotalPagar",ViewState["TotalPagar"].ToString()),
                    new ReportParameter("OdontoGrama",ondotoGram),
                    new ReportParameter("Medico",ViewState["Medico"].ToString()),
                    new ReportParameter("FechaReporte",DateTime.Now.ToString("MM-dd-yyyy HH:mm")),
                    new ReportParameter("LogoC",pathLogoCabecera)
                };
                rptPresupuesto.LocalReport.SetParameters(parametros);
                rptPresupuesto.LocalReport.Refresh();
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
                    Response.Redirect("FrmPrespuestoRealizadoAdmin.aspx?CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                    break;
                case "1":                    
                    Response.Redirect("../Administrador/FrmPresuRealizadoAdministrador.aspx?CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                    break;
            }
            
        }
        #endregion
    }
}