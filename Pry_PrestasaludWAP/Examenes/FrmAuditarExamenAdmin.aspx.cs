using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmAuditarExamenAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        Image ImgEstado = new Image();
        string codigoexso = "";
        int codigocamp = 0;
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
                    Lbltitulo.Text = "Auditoria de Exámenes";
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
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 145;//cambio aki camp codigo 24
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts == null || dts.Tables.Count == 0 || dts.Tables[0].Rows.Count == 0
                    || !int.TryParse(Convert.ToString(dts.Tables[0].Rows[0]["CodigoCAMP"]), out codigocamp) || codigocamp <= 0)
                    throw new InvalidOperationException("El usuario no tiene una campaña válida para consultar auditorías.");

                objparam[0] = codigocamp;
                objparam[1] = "";
                objparam[2] = 159;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts == null || dts.Tables.Count == 0) throw new InvalidOperationException("No se obtuvo el listado de auditorías.");
                DataTable datos = dts.Tables[0];
                DateTime? desde = ViewState["AuditorFiltroDesde"] as DateTime?;
                DateTime? hasta = ViewState["AuditorFiltroHasta"] as DateTime?;
                string documento = Convert.ToString(ViewState["AuditorFiltroDocumento"]);
                string proceso = Convert.ToString(ViewState["AuditorFiltroProceso"]);
                datos = FiltrarSolicitudesAuditor(datos, desde, hasta, documento, proceso);
                LblResumenAuditor.Text = HttpUtility.HtmlEncode(datos.Rows.Count + " registro(s) · Desde: "
                    + (desde.HasValue ? desde.Value.ToString("dd/MM/yyyy") : "Sin límite") + " · Hasta: "
                    + (hasta.HasValue ? hasta.Value.ToString("dd/MM/yyyy") : "Sin límite")
                    + " · Documento: " + (documento.Length == 0 ? "Todos" : documento)
                    + " · Proceso: " + (proceso.Length == 0 ? "Todos" : proceso));
                int ultimaPagina = datos.Rows.Count == 0 ? 0 : (datos.Rows.Count - 1) / GrdvDatos.PageSize;
                if (GrdvDatos.PageIndex > ultimaPagina) GrdvDatos.PageIndex = ultimaPagina;
                GrdvDatos.DataSource = datos;
                GrdvDatos.DataBind();
                ActualizarPaginadorAuditor();
                if (GrdvDatos.HeaderRow != null)
                { GrdvDatos.UseAccessibleHeader = true; GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader; }

            }
            catch (Exception ex)
            {
                GrdvDatos.DataSource = null; GrdvDatos.DataBind(); ActualizarPaginadorAuditor(); LblResumenAuditor.Text = "";
                Lblerror.Text = HttpUtility.HtmlEncode(ex.Message);
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnAuditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
                Response.Redirect("FrmAuditarExamen.aspx?CodigoEXSO=" + codigoexso, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                string estado = "";

                if (GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"] != null)
                {
                    estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString().Trim().ToUpper();
                }

                // Columna Estado
                TableCell celdaEstado = e.Row.Cells[3];


                switch (estado)
                {
                    // =====================================
                    // PENDIENTE DE AUDITAR
                    // =====================================
                    case "EXR":

                        celdaEstado.BackColor = System.Drawing.Color.Aquamarine;
                        celdaEstado.ForeColor = System.Drawing.Color.DarkSlateGray;
                        break;

                    // =====================================
                    // AUDITADO ACEPTADO
                    // =====================================
                    case "AUA":

                        celdaEstado.BackColor = System.Drawing.Color.LightGreen;
                        celdaEstado.ForeColor = System.Drawing.Color.DarkGreen;
                        break;


                    // =====================================
                    // AUDITADO RECHAZADO
                    // =====================================
                    case "AUR":

                        DataRowView filaEstado = e.Row.DataItem as DataRowView;
                        bool tieneApelable = filaEstado != null
                            && filaEstado.DataView.Table.Columns.Contains("TieneRechazoApelable")
                            && filaEstado["TieneRechazoApelable"] != DBNull.Value
                            && Convert.ToBoolean(filaEstado["TieneRechazoApelable"]);
                        celdaEstado.BackColor = tieneApelable
                            ? System.Drawing.Color.PeachPuff : System.Drawing.Color.LightCoral;
                        celdaEstado.ForeColor = tieneApelable
                            ? System.Drawing.Color.DarkOrange : System.Drawing.Color.DarkRed;
                        break;
                }

                celdaEstado.Font.Bold = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
        protected void BtnConsultarAuditor_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime desde, hasta;
                DateTime? filtroDesde = null, filtroHasta = null;
                if (!string.IsNullOrWhiteSpace(TxtDesdeAuditor.Text))
                {
                    if (!DateTime.TryParseExact(TxtDesdeAuditor.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out desde))
                        throw new InvalidOperationException("Seleccione una fecha Desde válida.");
                    filtroDesde = desde;
                }
                if (!string.IsNullOrWhiteSpace(TxtHastaAuditor.Text))
                {
                    if (!DateTime.TryParseExact(TxtHastaAuditor.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hasta))
                        throw new InvalidOperationException("Seleccione una fecha Hasta válida.");
                    filtroHasta = hasta;
                }
                if (filtroDesde.HasValue && filtroHasta.HasValue && filtroDesde.Value > filtroHasta.Value)
                    throw new InvalidOperationException("La fecha Desde no puede ser posterior a Hasta.");
                ViewState["AuditorFiltroDesde"] = filtroDesde;
                ViewState["AuditorFiltroHasta"] = filtroHasta;
                ViewState["AuditorFiltroDocumento"] = TxtDocumentoAuditor.Text.Trim();
                ViewState["AuditorFiltroProceso"] = DdlProcesoAuditor.SelectedValue;
                GrdvDatos.PageIndex = 0;
                Lblerror.Text = "";
                FunCargaMantenimiento();
            }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }
        protected void BtnLimpiarAuditor_Click(object sender, EventArgs e)
        {
            TxtDesdeAuditor.Text = ""; TxtHastaAuditor.Text = ""; TxtDocumentoAuditor.Text = "";
            DdlProcesoAuditor.SelectedIndex = 0;
            BtnConsultarAuditor_Click(sender, e);
        }
        internal static DataTable FiltrarSolicitudesAuditor(DataTable datos, DateTime? desde, DateTime? hasta, string documento, string proceso)
        {
            DataTable filtrados = datos.Clone();
            foreach (DataRow fila in datos.Rows)
            {
                if (!string.IsNullOrEmpty(documento) && !string.Equals(Convert.ToString(fila["Cedula"]).Trim(), documento.Trim(), StringComparison.OrdinalIgnoreCase)) continue;
                if (proceso == "APELABLE")
                {
                    bool apelable = datos.Columns.Contains("TieneRechazoApelable") && fila["TieneRechazoApelable"] != DBNull.Value
                        ? Convert.ToBoolean(fila["TieneRechazoApelable"])
                        : Convert.ToString(fila["Estado"]).IndexOf("APELABLE", StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!apelable) continue;
                }
                else if (!string.IsNullOrEmpty(proceso) && !string.Equals(Convert.ToString(fila["EstadoCodigo"]).Trim(), proceso, StringComparison.OrdinalIgnoreCase)) continue;
                if (desde.HasValue || hasta.HasValue)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(fila["FechaEnvioResultados"]))) continue;
                    DateTime fecha;
                    if (!LeerFechaSolicitudAuditor(fila["FechaEnvioResultados"], out fecha))
                        throw new InvalidOperationException("No se pudo interpretar la fecha de envío de resultados de la solicitud " + fila["CodigoEXSO"] + ". Revise FecSolicitud.");
                    if (desde.HasValue && fecha.Date < desde.Value.Date) continue;
                    if (hasta.HasValue && fecha.Date > hasta.Value.Date) continue;
                }
                filtrados.ImportRow(fila);
            }
            return filtrados;
        }
        internal static bool LeerFechaSolicitudAuditor(object valor, out DateTime fecha)
        {
            if (valor is DateTime) { fecha = (DateTime)valor; return true; }
            // Operación 144: MM/dd/yyyy con hora AM/PM. Orden adicional: yyyy-MM-dd HH:mm:ss.
            // No usar la cultura del servidor para evitar intercambiar día y mes.
            return DateTime.TryParse(Convert.ToString(valor).Trim(), CultureInfo.GetCultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out fecha);
        }

        protected void GrdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.Cancel = true; if (e.NewPageIndex < 0) return;
            try { Lblerror.Text = ""; GrdvDatos.PageIndex = e.NewPageIndex; FunCargaMantenimiento(); }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }

        private void ActualizarPaginadorAuditor()
        {
            int paginas = Math.Max(1, GrdvDatos.PageCount);
            LblPaginaAuditor.Text = "Página " + (GrdvDatos.PageIndex + 1) + " de " + paginas;
            BtnAnteriorAuditor.Enabled = GrdvDatos.PageIndex > 0;
            BtnSiguienteAuditor.Enabled = GrdvDatos.PageIndex + 1 < paginas;
        }
        protected void BtnAnteriorAuditor_Click(object sender, EventArgs e)
        {
            if (GrdvDatos.PageIndex > 0)
                GrdvDatos_PageIndexChanging(sender, new GridViewPageEventArgs(GrdvDatos.PageIndex - 1));
        }
        protected void BtnSiguienteAuditor_Click(object sender, EventArgs e)
        {
            if (GrdvDatos.PageIndex + 1 < GrdvDatos.PageCount)
                GrdvDatos_PageIndexChanging(sender, new GridViewPageEventArgs(GrdvDatos.PageIndex + 1));
        }

    }
}