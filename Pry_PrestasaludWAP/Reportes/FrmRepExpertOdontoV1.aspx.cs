//namespace Pry_PrestasaludWAP.Reportes
//{
//    using System;
//    using System.Data;
//    using System.Globalization;
//    using System.IO;
//    using System.Web.UI;
//    using System.Web.UI.WebControls;

//    public partial class FrmRepExpertOdontoV1 : Page
//    {
//        #region Variables
//        Object[] objparam = new Object[1];
//        DataSet ds = new DataSet();
//        int opcion = 0;
//        #endregion

//        #region Load
//        protected void Page_Load(object sender, EventArgs e)
//        {
//            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
//            scriptManager.RegisterPostBackControl(this.imgExportar);
//            if (!IsPostBack)
//            {
//                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");
//                txtFechaFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
//                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
//                funCascadaCombos(0);
//                funCascadaCombos(1);
//            }
//            else grdvDatos.DataSource = ViewState["grdvDatos"];
//        }
//        #endregion

//        #region Procedimientos y Funciones
//        private void funCascadaCombos(int opcion)
//        {
//            switch (opcion)
//            {
//                case 0:
//                    Array.Resize(ref objparam, 1);
//                    objparam[0] = 38;
//                    //ddlTipoAgenda.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
//                    //ddlTipoAgenda.DataTextField = "Descripcion";
//                    //ddlTipoAgenda.DataValueField = "Codigo";
//                    //ddlTipoAgenda.DataBind();
//                    break;
//                case 1:
//                    Array.Resize(ref objparam, 1);
//                    objparam[0] = 51;
//                    ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
//                    ddlCliente.DataTextField = "Descripcion";
//                    ddlCliente.DataValueField = "Codigo";
//                    ddlCliente.DataBind();
//                    break;
//            }
//        }
//        public override void VerifyRenderingInServerForm(Control control)
//        {
//            /* Verifies that the control is rendered */
//        }
//        #endregion

//        #region Botones y Eventos
//        protected void btnProcesar_Click(object sender, EventArgs e)
//        {
//            lblerror.Text = "";

//            if (!new Funciones().IsDateNewx(txtFechaIni.Text))
//            {
//                lblerror.Text = "No es una fecha válida..!";
//                return;
//            }
//            if (!new Funciones().IsDateNewx(txtFechaFin.Text))
//            {
//                lblerror.Text = "No es una fecha válida..!";
//                return;
//            }
//            if (DateTime.ParseExact(txtFechaIni.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture))
//            {
//                lblerror.Text = "La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!";
//                return;
//            }
//            System.Threading.Thread.Sleep(300);
//            Array.Resize(ref objparam, 6);
//            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 0;
//            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 1;
//            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 2;
//            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 3;
//            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 4;
//            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 5;
//            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 6;
//            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 7;

//            imgExportar.Visible = true;
//            lblExportar.Visible = true;

//            if (ddlCliente.SelectedValue == "0") objparam[0] = 0;
//            if (ddlCliente.SelectedValue != "0") objparam[0] = 1;


//            objparam[1] = int.Parse(ddlCliente.SelectedValue.ToString());
//            objparam[2] = "";
//            objparam[3] = "";
//            objparam[4] = txtFechaIni.Text;
//            objparam[5] = txtFechaFin.Text;
//            ds = new Conexion(2, "").funConsultarSqls("sp_ReportesExpertOdonto", objparam);

//            if (ds.Tables[0].Rows.Count > 0 || ds.Tables[0] != null)
//            {
//                grdvDatos.DataSource = ds.Tables[0];
//                ViewState["grdvDatos"] = ds.Tables[0];
//                grdvDatos.DataBind();

//            }
//            else
//            {
//                new Funciones().funShowJSMessage("No se encontraron datos..!", this);
//            }
//        }
//        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
//        {
//            Response.Clear();
//            Response.Buffer = true;
//            string FileName = "ReporteExpertDoctor_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
//            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
//            Response.Charset = "";
//            Response.ContentType = "application/vnd.ms-excel";
//            using (StringWriter sw = new StringWriter())
//            {
//                HtmlTextWriter hw = new HtmlTextWriter(sw);
//                grdvDatos.AllowPaging = false;
//                grdvDatos.DataSource = (DataTable)ViewState["grdvDatos"];
//                grdvDatos.DataBind();
//                //grdvDatos.HeaderRow.BackColor = Color.White;
//                foreach (GridViewRow row in grdvDatos.Rows)
//                {
//                    //row.BackColor = Color.White;
//                    row.Cells[5].Style.Add("mso-number-format", "\\@");
//                }
//                grdvDatos.RenderControl(hw);
//                string style = @"<style> .textmode { } </style>";
//                Response.Write(style);
//                Response.Output.Write(sw.ToString());
//                Response.Flush();
//                Response.End();
//            }
//        }
//        protected void btnSalir_Click(object sender, EventArgs e)
//        {
//            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
//        }
//        #endregion
//    }
//}
namespace Pry_PrestasaludWAP.Reportes
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmRepExpertOdontoV1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager == null || imgExportar == null)
            {
                lblerror.Text = "Verifique que el ASPX publicado tenga el ScriptManager y el botón de exportación correspondientes.";
                return;
            }

            scriptManager.AsyncPostBackTimeout = 600;
            scriptManager.RegisterPostBackControl(imgExportar);
            if (!IsPostBack)
            {
                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
                imgExportar.Visible = false;
                lblExportar.Visible = false;
                funCascadaCombos(1);
            }
            else
            {
                grdvDatos.DataSource = ViewState["grdvDatos"];
            }
        }

        private void funCascadaCombos(int opcion)
        {
            if (opcion != 1) return;
            object[] parametros = { 51 };
            DataSet clientes = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", parametros);
            if (clientes == null || clientes.Tables.Count == 0)
            {
                lblerror.Text = "No se pudo cargar el listado de clientes. Revise la conexión y la consulta de combos.";
                return;
            }
            ddlCliente.DataSource = clientes.Tables[0];
            ddlCliente.DataTextField = "Descripcion";
            ddlCliente.DataValueField = "Codigo";
            ddlCliente.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control) { }

        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            ViewState["grdvDatos"] = null;
            imgExportar.Visible = false;
            lblExportar.Visible = false;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
            try
            {
                DateTime desde, hasta;
                if (!DateTime.TryParseExact(txtFechaIni.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out desde) ||
                    !DateTime.TryParseExact(txtFechaFin.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out hasta))
                {
                    lblerror.Text = "Ingrese fechas válidas de inicio y fin.";
                    return;
                }
                if (desde > hasta)
                {
                    lblerror.Text = "La fecha de inicio no puede ser mayor a la fecha de fin.";
                    return;
                }
                int cliente;
                if (!int.TryParse(ddlCliente.SelectedValue, out cliente) || cliente < 0)
                {
                    lblerror.Text = "Seleccione un cliente válido o la opción Todos.";
                    return;
                }

                object[] parametros = {
                    cliente == 0 ? 0 : 1, cliente, "", "",
                    desde.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    hasta.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                };
                DataSet resultado = new Conexion(2, "").FunReporteExpertOdonto(parametros);
                if (resultado == null)
                {
                    lblerror.Text = "El método del reporte devolvió null. Verifique que se haya incorporado la versión actual de FunReporteExpertOdonto.";
                    return;
                }
                if (resultado.Tables.Count == 0)
                {
                    lblerror.Text = "El procedimiento no devolvió una tabla de resultados.";
                    return;
                }

                DataTable tabla = resultado.Tables[0];
                grdvDatos.PageIndex = 0;
                grdvDatos.DataSource = tabla;
                grdvDatos.DataBind();
                ViewState["grdvDatos"] = tabla;
                imgExportar.Visible = tabla.Rows.Count > 0;
                lblExportar.Visible = tabla.Rows.Count > 0;
                if (tabla.Rows.Count == 0)
                    lblerror.Text = "No se encontraron datos para los filtros seleccionados.";
            }
            catch (Exception ex)
            {
                ViewState["grdvDatos"] = null;
                imgExportar.Visible = false;
                lblExportar.Visible = false;
                lblerror.Text = System.Web.HttpUtility.HtmlEncode(ex.Message);
            }
        }

        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
        {
            DataTable tabla = ViewState["grdvDatos"] as DataTable;
            if (tabla == null || tabla.Rows.Count == 0)
            {
                lblerror.Text = "Consulte el reporte y obtenga registros antes de exportar.";
                return;
            }

            bool paginacionAnterior = grdvDatos.AllowPaging;
            int paginaAnterior = grdvDatos.PageIndex;
            string contenido;
            try
            {
                using (StringWriter salida = new StringWriter())
                {
                    HtmlTextWriter escritor = new HtmlTextWriter(salida);
                    grdvDatos.AllowPaging = false;
                    grdvDatos.DataSource = tabla;
                    grdvDatos.DataBind();
                    foreach (GridViewRow fila in grdvDatos.Rows)
                    {
                        if (fila.Cells.Count > 5)
                            fila.Cells[5].Style.Add("mso-number-format", "\\@");
                    }
                    grdvDatos.RenderControl(escritor);
                    contenido = salida.ToString();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = System.Web.HttpUtility.HtmlEncode(ex.Message);
                return;
            }
            finally
            {
                grdvDatos.AllowPaging = paginacionAnterior;
                grdvDatos.PageIndex = paginaAnterior;
                grdvDatos.DataSource = tabla;
                grdvDatos.DataBind();
            }

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment;filename=ReporteExpertDoctor_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write("<style> .textmode { } </style>");
            Response.Write(contenido);
            Response.End();
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }
    }
}
