using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Globalization;

namespace Pry_PrestasaludWAP.Reportes
{
    public partial class Frm_RepGerencialV2 : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtl = new DataTable();
        Object[] objparam = new Object[1];
        string reporte = "", pathLogoCabecera = "", pathLogoPie = "", sqlFormat = "", citastatus = "", sqlContador = "";
        int codigogenerado = 0, tipoReport = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                if (Session["SalirAgenda"].ToString() == "NO")
                {
                    if (Session["TipoCita"].ToString() == "CitaMedica")
                        Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                    if (Session["TipoCita"].ToString() == "CitaOdontologica")
                        Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                    return;
                }
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 111;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                codigogenerado = int.Parse(dt.Tables[0].Rows[0][0].ToString());
                ViewState["CodigoGenerado"] = codigogenerado;
                objparam[0] = codigogenerado;
                objparam[2] = 112;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["LogoC"] = dt.Tables[0].Rows[0][0].ToString();
                ViewState["LogoP"] = dt.Tables[0].Rows[0][1].ToString();
                if (ViewState["CodigoGenerado"] == null || codigogenerado == 0)
                {
                    lblerror.Text = "Usuario no tiene asociado Código del Cliente..!";
                    btnProcesar.Enabled = false;
                }
                lbltitulo.Text = "REPORTE GERENCIAL CITAS ODONTOLOGICAS";
                txtFechaDesde.Text = DateTime.Now.ToString("MM/dd/yyyy");
                txtFechaHasta.Text = DateTime.Now.ToString("MM/dd/yyyy");
                ViewState["Procesar"] = "1";
                ViewState["Iniciar"] = "S";
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funGenerarReporte(DataTable dtp)
        {
            try
            {
                if (rdbHoras.Checked) reporte = "RptGerencialOdontoHoras.rdlc";
                if (rdbCiudad.Checked) reporte = "RptGerencialOdontoCiudad.rdlc";
                if (rdbPrestadora.Checked) reporte = "RptGerencialOdontoPrestadora.rdlc";
                if (rdbEspecialidad.Checked) reporte = "RptGerencialOdontoEspecialidad.rdlc";
                if (rdbGenero.Checked) reporte = "RptGerencialOdontoGenero.rdlc";
                if (rdbProducto.Checked) reporte = "RptGerencialOdontoProducto.rdlc";
                rptGerencial.Reset();
                rptGerencial.SizeToReportContent = true;
                //rptGerencial.LocalReport.ReportPath = Server.MapPath("~/Reports/RptGerencialDoctor.rdlc");
                rptGerencial.LocalReport.ReportPath = Server.MapPath("~/Reports/" + reporte);
                rptGerencial.LocalReport.EnableExternalImages = true;
                pathLogoCabecera = new Uri(Server.MapPath(ViewState["LogoC"].ToString())).AbsoluteUri;
                pathLogoPie = new Uri(Server.MapPath(ViewState["LogoP"].ToString())).AbsoluteUri;
                rptGerencial.LocalReport.DataSources.Clear();
                ReportDataSource _rsource = new ReportDataSource("dsGerencialDoctor", dtp);
                rptGerencial.LocalReport.DataSources.Add(_rsource);
                //if (rdbGenero.Checked)
                //{
                //    rptGerencial.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(DoctorDetailSubReportProcessing);
                //}
                //if (rdbCiudad.Checked)
                //{
                //    rptGerencial.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(CiudadDetailSubReportProcessing);
                //}
                ReportParameter[] parametros = new ReportParameter[]
                {
                    new ReportParameter("tipoagenda",ddlTipoAgenda.SelectedItem.ToString()),
                    new ReportParameter("tipocliente",ddlTipoCliente.SelectedItem.ToString()),
                    new ReportParameter("rangofecha","Desde: "+txtFechaDesde.Text+" Hasta: "+ txtFechaHasta.Text),
                    new ReportParameter("FechaReporte",DateTime.Now.ToString()),
                    new ReportParameter("LogoC",pathLogoCabecera),
                    new ReportParameter("LogoP",pathLogoPie),
                    new ReportParameter("TotalReg",ViewState["TotalReg"].ToString())
                };
                rptGerencial.LocalReport.SetParameters(parametros);

                rptGerencial.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        void DoctorDetailSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            int codcita = int.Parse(e.Parameters["CodigoCita"].Values[0].ToString());
            dtl = funGetDoctorDetail(codcita);
            ReportDataSource dts = new ReportDataSource("dsDoctorDetail", dtl);
            e.DataSources.Add(dts);
        }

        private DataTable funGetDoctorDetail(int CodigoCita)
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = CodigoCita;
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporteOdontoDetalle", objparam);
            return dt.Tables[0];
        }

        void CiudadDetailSubReportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            string fechad = txtFechaDesde.Text.Trim();
            string fechah = txtFechaHasta.Text.Trim();
            string tipoagenda = ddlTipoAgenda.SelectedValue;
            string tipocliente = ddlTipoCliente.SelectedValue;
            int ciudcod = int.Parse(e.Parameters["CiuCodigo"].Values[0].ToString());
            int codcamp = int.Parse(ViewState["CodigoGenerado"].ToString());
            dtl = funGetCiudadDetail(fechad, fechah, tipoagenda, tipocliente, ciudcod, codcamp);
            ReportDataSource dts = new ReportDataSource("dsCiudadDetail", dtl);
            e.DataSources.Add(dts);
        }

        private DataTable funGetCiudadDetail(string fechadesde, string fechahasta, string tipoagenda, string tipocliente, int ciucod, int codamp)
        {
            Array.Resize(ref objparam, 6);
            objparam[0] = fechadesde;
            objparam[1] = fechahasta;
            objparam[2] = tipoagenda;
            objparam[3] = tipocliente;
            objparam[4] = ciucod;
            objparam[5] = codamp;
            dt = new Conexion(2, "").funConsultarSqls("sp_ReporteCiudadDetalleOdon", objparam);
            return dt.Tables[0];

        }

        #endregion

        #region Botones y Eventos
        protected void rdbHoras_CheckedChanged(object sender, EventArgs e)
        {
            rdbCiudad.Checked = false;
            rdbGenero.Checked = false;
            rdbEspecialidad.Checked = false;
            rdbPrestadora.Checked = false;
            rdbProducto.Checked = false;
        }

        protected void rdbCiudad_CheckedChanged(object sender, EventArgs e)
        {
            rdbHoras.Checked = false;
            rdbGenero.Checked = false;
            rdbEspecialidad.Checked = false;
            rdbPrestadora.Checked = false;
            rdbProducto.Checked = false;
        }

        protected void rdbPrestadora_CheckedChanged(object sender, EventArgs e)
        {
            rdbHoras.Checked = false;
            rdbGenero.Checked = false;
            rdbEspecialidad.Checked = false;
            rdbCiudad.Checked = false;
            rdbProducto.Checked = false;
        }

        protected void rdbEspecialidad_CheckedChanged(object sender, EventArgs e)
        {
            rdbHoras.Checked = false;
            rdbGenero.Checked = false;
            rdbPrestadora.Checked = false;
            rdbCiudad.Checked = false;
            rdbProducto.Checked = false;
        }

        protected void rdbGenero_CheckedChanged(object sender, EventArgs e)
        {
            rdbHoras.Checked = false;
            rdbEspecialidad.Checked = false;
            rdbPrestadora.Checked = false;
            rdbCiudad.Checked = false;
            rdbProducto.Checked = false;
        }

        protected void rdbProducto_CheckedChanged(object sender, EventArgs e)
        {
            rdbHoras.Checked = false;
            rdbGenero.Checked = false;
            rdbEspecialidad.Checked = false;
            rdbCiudad.Checked = false;
            rdbPrestadora.Checked = false;
        }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";
                if (int.Parse(ViewState["Procesar"].ToString()) == 1)
                {
                    pnlOpciones.Visible = false;
                    pnlDivision0.Visible = false;
                    btnProcesar.Text = "Consultar";
                    ViewState["Procesar"] = "0";
                    if (!rdbHoras.Checked && !rdbCiudad.Checked && !rdbEspecialidad.Checked && !rdbGenero.Checked && !rdbPrestadora.Checked && !rdbProducto.Checked)
                    {
                        new Funciones().funShowJSMessage("Seleccione Tipo de Reporte Gerencial..!", this);
                        return;
                    }
                    if (ddlTipoAgenda.SelectedItem.ToString() == "--Tipo Agenda--")
                    {
                        new Funciones().funShowJSMessage("Seleccione Tipo de Agenda..!", this);
                        return;
                    }
                    if (ddlTipoCliente.SelectedValue == "C")
                    {
                        new Funciones().funShowJSMessage("Seleccione Tipo de Cliente..!", this);
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

                    if (ddlTipoAgenda.SelectedValue == "A")
                    {
                        citastatus = "where CI.ciod_estatuscita in(''A'',''S'',''T'',''F'') and ";

                        if (ddlTipoCliente.SelectedValue != "S") citastatus += " CI.ciod_tipocliente=''" + ddlTipoCliente.SelectedValue + "'' and ";
                    }
                    if (ddlTipoAgenda.SelectedValue == "C")
                    {
                        citastatus = "where CI.ciod_estatuscita in(''C'') and ";
                        if (ddlTipoCliente.SelectedValue != "S") citastatus += " CI.ciod_tipocliente=''" + ddlTipoCliente.SelectedValue + "'' and ";
                    }
                    sqlContador = "select COUNT(*) from Expert_CITAODONTO CI (nolock) ";
                    sqlContador += "INNER join Expert_PRESTADORA_ODONTO PR ON CI.PPRO_CODIGO=PR.PPRO_CODIGO ";
                    sqlContador += "INNER JOIN Expert_ESPECIALIDAD ES ON CI.ESPE_CODIGO=ES.ESPE_CODIGO ";
                    sqlContador += "INNER JOIN Expert_MEDICOPRESODON ME ON CI.MEOD_CODIGO=ME.MEOD_CODIGO ";
                    sqlContador += "INNER JOIN Expert_TITULAR TI ON TI.TITU_CODIGO = CI.ciod_titucodigo ";
                    sqlContador += "INNER JOIN Expert_PRODUCTOS PD ON PD.PROD_CODIGO = TI.PROD_CODIGO ";
                    citastatus += "CONVERT(date,CI.ciod_fechacreacion,101) between convert(date," + "''" + txtFechaDesde.Text + "'',101)" + " and convert(date," + "''" + txtFechaHasta.Text + "'',101) and ";
                    citastatus += "TI.PROD_CODIGO in (select PROD_CODIGO from Expert_PRODUCTOS where CAMP_CODIGO=" + ViewState["CodigoGenerado"].ToString();
                    citastatus += " and prod_auxi2=''SI'')";
                    sqlContador += citastatus;
                    if (rdbHoras.Checked)
                    {
                        sqlFormat = "select Definicion = DATEPART(hour,CI.ciod_fechacreacion),IdCodigo = DATEPART(hour,CI.ciod_fechacreacion),";
                        sqlFormat += "TotalCitas = COUNT(*) from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO ";
                        sqlFormat += citastatus;
                        sqlFormat += " GROUP BY DATEPART(hour,CI.ciod_fechacreacion) ORDER BY COUNT(*) desc";
                        tipoReport = 0;
                    }
                    if (rdbCiudad.Checked)
                    {
                        sqlFormat = "select Definicion = CU.ciud_nombre,IdCodigo = CU.CIUD_COD,TotalCitas = COUNT(*) from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_PRESTADORA_ODONTO PR ON CI.PPRO_CODIGO=PR.PPRO_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_CIUDAD CU ON CU.CIUD_COD=PR.CIUD_COD ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo = TI.TITU_CODIGO ";
                        sqlFormat += citastatus;
                        sqlFormat += " GROUP BY CU.ciud_nombre,CU.CIUD_COD ORDER BY COUNT(*) desc";
                        tipoReport = 1;
                    }
                    if (rdbPrestadora.Checked)
                    {
                        sqlFormat = "select Definicion = PR.prod_nombre,IdCodigo = PR.PPRO_CODIGO,TotalCitas = COUNT(*) from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_PRESTADORA_ODONTO PR ON CI.PPRO_CODIGO=PR.PPRO_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO ";
                        sqlFormat += citastatus;
                        sqlFormat += " GROUP BY PR.prod_nombre,PR.PPRO_CODIGO ORDER BY COUNT(*) desc";
                        tipoReport = 2;
                    }
                    if (rdbEspecialidad.Checked)
                    {
                        sqlFormat = "select Definicion = ES.espe_nombre,IdCodigo = ES.ESPE_CODIGO,TotalCitas = COUNT(*) ";
                        sqlFormat += "from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_ESPECIALIDAD ES ON CI.ESPE_CODIGO=ES.ESPE_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO ";
                        sqlFormat += citastatus;
                        sqlFormat += " GROUP BY ES.espe_nombre,ES.ESPE_CODIGO ORDER BY COUNT(*) desc";
                        tipoReport = 3;
                    }
                    if (rdbGenero.Checked)
                    {
                        sqlFormat = "select Definicion = case PE.pers_genero when ''M'' then ''MASCULINO'' else ''FEMENINO'' end,";
                        sqlFormat += "IdCodigo = PE.pers_genero,TotalCitas = COUNT(*) from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_PERSONA PE ON TI.PERS_CODIGO=PE.PERS_CODIGO ";
                        sqlFormat += citastatus;
                        sqlFormat += " GROUP BY PE.pers_genero ORDER BY COUNT(*) desc";
                        tipoReport = 4;
                    }
                    if (rdbProducto.Checked)
                    {
                        sqlFormat = "select Definicion = PL.pade_nombre,IdCodigo = PL.pade_nombre,TotalCitas = COUNT(*) ";
                        sqlFormat += "from Expert_CITAODONTO CI (nolock) ";
                        sqlFormat += "INNER JOIN Expert_TITULAR TI ON CI.ciod_titucodigo=TI.TITU_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_PRODUCTOS PD ON PD.PROD_CODIGO = TI.PROD_CODIGO ";
                        sqlFormat += "INNER JOIN Expert_PARAMETRO_DETALLE PL ON PD.prod_auxi1=PL.pade_valor ";
                        sqlFormat += "INNER JOIN Expert_PARAMETRO_CABECERA PC ON PL.PARA_CODIGO=PC.PARA_CODIGO ";
                        sqlFormat += citastatus + "and PC.para_nombre=''GRUPOS PRODUCTOS'' ";
                        sqlFormat += " GROUP BY PL.pade_nombre ORDER BY COUNT(*) desc";
                        tipoReport = 5;
                    }

                    Array.Resize(ref objparam, 12);
                    objparam[0] = 0;
                    objparam[1] = txtFechaDesde.Text;
                    objparam[2] = txtFechaHasta.Text;
                    objparam[3] = ddlTipoAgenda.SelectedValue;
                    objparam[4] = ddlTipoCliente.SelectedValue;
                    objparam[5] = 0;
                    objparam[6] = sqlContador;
                    objparam[7] = "";
                    objparam[8] = "";
                    objparam[9] = int.Parse(ViewState["CodigoGenerado"].ToString());
                    objparam[10] = tipoReport;
                    objparam[11] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ReporteGerencial", objparam);
                    ViewState["TotalReg"] = dt.Tables[0].Rows[0][0].ToString();
                    objparam[6] = sqlFormat;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ReporteGerencial", objparam);

                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        pnlFacturacion.Visible = true;
                        funGenerarReporte(dt.Tables[0]);
                    }
                    else
                    {
                        pnlFacturacion.Visible = false;
                        lblerror.Text = "No existe registros en ese período..!";
                    }
                }
                else
                {
                    btnProcesar.Text = "Procesar";
                    ViewState["Procesar"] = "1";
                    pnlFacturacion.Visible = false;
                    pnlOpciones.Visible = true;
                    pnlDivision0.Visible = true;
                    //Response.Redirect(Request.Url.AbsolutePath, true);
                }
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