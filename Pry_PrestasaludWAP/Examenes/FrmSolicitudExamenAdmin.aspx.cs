using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmSolicitudExamenAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        string codigoexso = "", codigopers = "", numdocumento = "", estado = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Administrar Solicitud Exámenes";
                    FunCargaMantenimiento();
                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = "";
            objparam[2] = 142;
            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);//comentado sp traer solo solicitados
            if (dts == null || dts.Tables.Count == 0) throw new InvalidOperationException("No se obtuvo el listado de solicitudes.");
            DataTable datos = dts.Tables[0];
            DateTime? desde = ViewState["SolicitaFiltroDesde"] as DateTime?;
            DateTime? hasta = ViewState["SolicitaFiltroHasta"] as DateTime?;
            string documento = Convert.ToString(ViewState["SolicitaFiltroDocumento"]);
            string proceso = Convert.ToString(ViewState["SolicitaFiltroProceso"]);
            datos = FiltrarSolicitudesSolicita(datos, desde, hasta, documento, proceso);
            LblResumenSolicita.Text = HttpUtility.HtmlEncode(datos.Rows.Count + " registro(s) · Desde: "
                + (desde.HasValue ? desde.Value.ToString("dd/MM/yyyy") : "Sin límite") + " · Hasta: "
                + (hasta.HasValue ? hasta.Value.ToString("dd/MM/yyyy") : "Sin límite")
                + " · Documento: " + (documento.Length == 0 ? "Todos" : documento)
                + " · Proceso: " + (proceso.Length == 0 ? "Todos" : proceso));
            int ultimaPagina = datos.Rows.Count == 0 ? 0 : (datos.Rows.Count - 1) / GrdvDatos.PageSize;
            if (GrdvDatos.PageIndex > ultimaPagina) GrdvDatos.PageIndex = ultimaPagina;
            GrdvDatos.DataSource = datos;
            GrdvDatos.DataBind();

            if (GrdvDatos.HeaderRow != null)
            {
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            //if (GrdvDatos.Rows.Count > 0)
            //{
            //    GrdvDatos.UseAccessibleHeader = true;
            //    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }
        #endregion

        #region Botones y Eventos
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString();
                    System.Web.UI.WebControls.TableCell celda = e.Row.Cells[4];
                    celda.Font.Bold = true;
                    switch (estado.Trim().ToUpperInvariant())
                    {
                        case "SRR": celda.BackColor = System.Drawing.Color.LightSeaGreen; break;
                        case "SRV": celda.BackColor = System.Drawing.Color.Coral; break;
                        case "SGA": celda.BackColor = System.Drawing.Color.LightGreen; break;
                        case "EXR": celda.BackColor = System.Drawing.Color.Aquamarine; celda.ForeColor = System.Drawing.Color.DarkSlateGray; break;
                        case "AUA": celda.BackColor = System.Drawing.Color.LightGreen; celda.ForeColor = System.Drawing.Color.DarkGreen; break;
                        case "AUR": celda.BackColor = System.Drawing.Color.LightCoral; celda.ForeColor = System.Drawing.Color.DarkRed; break;
                    }
                    if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EstadoSolicitud")).Trim().Equals("Cancelado", StringComparison.OrdinalIgnoreCase))
                    { e.Row.Cells[5].BackColor = System.Drawing.Color.LightGray; e.Row.Cells[5].ForeColor = System.Drawing.Color.DimGray; }

                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSolicitudExamen.aspx?CodigoPERS=0" +
                "&CodigoEXSO=0&NumDocumento=''", true);
        }

        protected void ImgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
            codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
            numdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
            Response.Redirect("FrmSolicitudExamen.aspx?CodigoPERS=" + codigopers +
                "&CodigoEXSO=" + codigoexso + "&NumDocumento=" + numdocumento, true);
        }
        #endregion
        protected void BtnConsultarSolicita_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime desde, hasta;
                DateTime? filtroDesde = null, filtroHasta = null;
                if (!string.IsNullOrWhiteSpace(TxtDesdeSolicita.Text))
                {
                    if (!DateTime.TryParseExact(TxtDesdeSolicita.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out desde))
                        throw new InvalidOperationException("Seleccione una fecha Desde válida.");
                    filtroDesde = desde;
                }
                if (!string.IsNullOrWhiteSpace(TxtHastaSolicita.Text))
                {
                    if (!DateTime.TryParseExact(TxtHastaSolicita.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hasta))
                        throw new InvalidOperationException("Seleccione una fecha Hasta válida.");
                    filtroHasta = hasta;
                }
                if (filtroDesde.HasValue && filtroHasta.HasValue && filtroDesde.Value > filtroHasta.Value)
                    throw new InvalidOperationException("La fecha Desde no puede ser posterior a Hasta.");
                ViewState["SolicitaFiltroDesde"] = filtroDesde;
                ViewState["SolicitaFiltroHasta"] = filtroHasta;
                ViewState["SolicitaFiltroDocumento"] = TxtDocumentoSolicita.Text.Trim();
                ViewState["SolicitaFiltroProceso"] = DdlProcesoSolicita.SelectedValue;
                GrdvDatos.PageIndex = 0;
                Lblerror.Text = "";
                FunCargaMantenimiento();
            }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }
        protected void BtnLimpiarSolicita_Click(object sender, EventArgs e)
        {
            TxtDesdeSolicita.Text = ""; TxtHastaSolicita.Text = ""; TxtDocumentoSolicita.Text = "";
            DdlProcesoSolicita.SelectedIndex = 0;
            BtnConsultarSolicita_Click(sender, e);
        }
        internal static DataTable FiltrarSolicitudesSolicita(DataTable datos, DateTime? desde, DateTime? hasta, string documento, string proceso)
        {
            DataTable filtrados = datos.Clone();
            foreach (DataRow fila in datos.Rows)
            {
                if (!string.IsNullOrEmpty(documento) && !string.Equals(Convert.ToString(fila["NumDocumento"]).Trim(), documento.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                if (!string.IsNullOrEmpty(proceso) && !string.Equals(Convert.ToString(fila["EstadoCodigo"]).Trim(), proceso, StringComparison.OrdinalIgnoreCase)) continue;
                if (desde.HasValue || hasta.HasValue)
                {
                    DateTime fecha;
                    if (!LeerFechaSolicitudSolicita(fila["FecSolicitud"], out fecha))
                        throw new InvalidOperationException("No se pudo interpretar la fecha de la solicitud " + fila["CodigoEXSO"] + ". Revise FecSolicitud.");
                    if (desde.HasValue && fecha.Date < desde.Value.Date) continue;
                    if (hasta.HasValue && fecha.Date > hasta.Value.Date) continue;
                }
                filtrados.ImportRow(fila);
            }
            return filtrados;
        }
        internal static bool LeerFechaSolicitudSolicita(object valor, out DateTime fecha)
        {
            if (valor is DateTime) { fecha = (DateTime)valor; return true; }
            // Operación 144: MM/dd/yyyy con hora AM/PM. Orden adicional: yyyy-MM-dd HH:mm:ss.
            // No usar la cultura del servidor para evitar intercambiar día y mes.
            return DateTime.TryParse(Convert.ToString(valor).Trim(), CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out fecha);
        }

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.Cancel = true;
            if (e.NewPageIndex < 0) return;
            try { Lblerror.Text = ""; GrdvDatos.PageIndex = e.NewPageIndex; FunCargaMantenimiento(); }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }

    }
}