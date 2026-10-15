using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmTiemposExamenes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || string.IsNullOrWhiteSpace(Session["usuCodigo"].ToString()))
            {
                Response.Redirect("~/Reload.html", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            if (!IsPostBack)
            {
                TxtDesde.Text = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                TxtHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
                GuardarFiltros();
                CargarTiempos();
            }
        }
        private bool SesionValida()
        {
            return Session["usuCodigo"] != null && !string.IsNullOrWhiteSpace(Session["usuCodigo"].ToString());
        }
        private bool GuardarFiltros()
        {
            DateTime desde, hasta;
            if (!DateTime.TryParseExact(TxtDesde.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out desde) ||
                !DateTime.TryParseExact(TxtHasta.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hasta) || hasta < desde)
            {
                LblError.Text = "Ingrese un rango de fechas válido.";
                return false;
            }
            ViewState["Desde"] = desde; ViewState["Hasta"] = hasta;
            ViewState["Documento"] = TxtDocumento.Text.Trim(); ViewState["Estado"] = DdlEstado.SelectedValue;
            return true;
        }
        private void CargarTiempos()
        {
            if (!SesionValida()) return;
            try
            {
                LblError.Text = "";
                object[] parametros = new object[4];
                parametros[0] = ((DateTime)ViewState["Desde"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                parametros[1] = ((DateTime)ViewState["Hasta"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                // funConsultarSqls concatena estos valores entre comillas simples.
                parametros[2] = Convert.ToString(ViewState["Documento"]).Replace("'", "''");
                parametros[3] = Convert.ToString(ViewState["Estado"]);
                DataSet ds = new Conexion(2, "").funConsultarSqls("sp_Examen_Tiempos_Listar", parametros);
                if (ds == null || ds.Tables.Count == 0)
                    throw new InvalidOperationException("No se obtuvo respuesta de la consulta de tiempos.");
                DataTable dt = ds.Tables[0].Copy();
                dt.Columns.Add("TiempoAgenda", typeof(string)); dt.Columns.Add("TiempoResultados", typeof(string));
                dt.Columns.Add("TiempoAuditoria", typeof(string)); dt.Columns.Add("ProgresoAuditoria", typeof(string));
                dt.Columns.Add("CierreAuditoriaMostrar", typeof(string));
                foreach (DataRow r in dt.Rows)
                {
                    r["TiempoAgenda"] = Tiempo(r, "SegundosAgenda", "SituacionAgenda");
                    r["TiempoResultados"] = Tiempo(r, "SegundosResultados", "SituacionResultados");
                    r["TiempoAuditoria"] = Tiempo(r, "SegundosAuditoria", "SituacionAuditoria");
                    r["ProgresoAuditoria"] = r["TotalAuditados"].ToString() + " / " + r["TotalEnviados"].ToString();
                    r["CierreAuditoriaMostrar"] = r["SituacionAuditoria"].ToString() == "FINALIZADO" && r["FechaCierreAuditoria"] != DBNull.Value
                        ? Convert.ToDateTime(r["FechaCierreAuditoria"]).ToString("dd/MM/yyyy HH:mm:ss") : "—";
                }
                int ultimaPagina = Math.Max(0, (dt.Rows.Count - 1) / GrdvTiempos.PageSize);
                if (GrdvTiempos.PageIndex > ultimaPagina) GrdvTiempos.PageIndex = ultimaPagina;
                GrdvTiempos.DataSource = dt; GrdvTiempos.DataBind();
                DateTime consulta = dt.Rows.Count > 0 ? Convert.ToDateTime(dt.Rows[0]["FechaConsulta"]) : DateTime.Now;
                LblActualizacion.Text = "Última consulta: " + consulta.ToString("dd/MM/yyyy HH:mm:ss") +
                    " · " + dt.Rows.Count.ToString() + " solicitud(es)" +
                    (dt.Rows.Count == 500 ? " · Límite de 500: reduzca el rango de fechas." : "");
            }
            catch (Exception)
            {
                LblError.Text = "No se pudo actualizar la consulta de tiempos. Verifique el SP y la conexión de la aplicación en local.";
                LblActualizacion.Text = "Consulta sin actualizar. Los datos visibles pueden ser anteriores.";
            }
        }
        private static string Tiempo(DataRow r, string segundos, string situacion)
        {
            string estado = r[situacion].ToString();
            if (estado == "NO INICIADO" || estado == "SIN REGISTRO") return estado;
            if (r[segundos] == DBNull.Value) return "REVISAR FECHAS";
            long s = Convert.ToInt64(r[segundos]);
            return (s / 3600).ToString() + " h " + ((s % 3600) / 60).ToString("00") + " m " +
                (s % 60).ToString("00") + " s · " + estado;
        }
        protected string BloqueEtapa(object datos, string etapa)
        {
            DataRow r = ((DataRowView)datos).Row;
            string situacion = r["Situacion" + etapa].ToString();
            string titulo = etapa == "Agenda" ? "Agendamiento" : etapa == "Auditoria" ? "Auditoría" : "Resultados";
            string intervalo = etapa == "Agenda" ? "Solicitud enviada → Cita agendada" :
                etapa == "Resultados" ? "Cita agendada → Resultados enviados" : "Resultados enviados → Todos auditados";
            string inicio = etapa == "Agenda" ? "FechaSolicitud" : etapa == "Resultados" ? "FechaRegistroAgenda" : "FechaEnvio";
            string fin = etapa == "Agenda" ? "FechaRegistroAgenda" : etapa == "Resultados" ? "FechaEnvio" : "FechaCierreAuditoria";
            string textoEstado = situacion == "FINALIZADO" ? "Finalizado" : situacion == "PENDIENTE" ? "En curso" :
                situacion == "NO INICIADO" ? "Por iniciar" : "Sin registro";
            string clase = situacion == "FINALIZADO" ? "finalizada" : situacion == "PENDIENTE" ? "curso" : "";
            string duracion = "—";
            if (situacion == "FINALIZADO" || situacion == "PENDIENTE")
            {
                if (r["Segundos" + etapa] == DBNull.Value) textoEstado = "Revisar fechas";
                else
                {
                    long segundos = Convert.ToInt64(r["Segundos" + etapa]);
                    duracion = (segundos / 3600).ToString() + " h " + ((segundos % 3600) / 60).ToString("00") +
                        " m " + (segundos % 60).ToString("00") + " s";
                }
            }
            string fechaInicio = r[inicio] == DBNull.Value ? "Sin registro" : Convert.ToDateTime(r[inicio]).ToString("dd/MM/yyyy HH:mm:ss");
            if (situacion == "NO INICIADO") fechaInicio = "Por iniciar";
            string fechaFin = situacion == "PENDIENTE" ? "Ahora (" + Convert.ToDateTime(r["FechaConsulta"]).ToString("HH:mm:ss") + ")" :
                situacion == "FINALIZADO" && r[fin] != DBNull.Value ? Convert.ToDateTime(r[fin]).ToString("dd/MM/yyyy HH:mm:ss") : "—";
            string progreso = etapa == "Auditoria" ? "<br />Pacientes: " +
                System.Web.HttpUtility.HtmlEncode(r["TotalAuditados"].ToString() + " / " + r["TotalEnviados"].ToString()) : "";
            return "<div class=\"etapa " + clase + "\"><h4>" + titulo + "</h4><div class=\"intervalo\">" +
                System.Web.HttpUtility.HtmlEncode(intervalo) + "</div><div class=\"duracion\">" +
                System.Web.HttpUtility.HtmlEncode(duracion) + "</div><div class=\"situacion\">" + textoEstado +
                "</div><div class=\"fechas\">Inicio: " + System.Web.HttpUtility.HtmlEncode(fechaInicio) +
                "<br />Fin: " + System.Web.HttpUtility.HtmlEncode(fechaFin) + progreso + "</div></div>";
        }
        protected void BtnConsultar_Click(object sender, EventArgs e)
        {
            if (!SesionValida() || !GuardarFiltros()) return;
            GrdvTiempos.PageIndex = 0; CargarTiempos();
        }
        protected void TimerTiempos_Tick(object sender, EventArgs e)
        {
            if (!SesionValida()) { TimerTiempos.Enabled = false; return; }
            // Conserva los filtros aplicados mediante Consultar.
            CargarTiempos();
        }
        protected void ChkEnLinea_CheckedChanged(object sender, EventArgs e)
        {
            TimerTiempos.Enabled = ChkEnLinea.Checked;
        }
        protected void GrdvTiempos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (!SesionValida()) return;
            GrdvTiempos.PageIndex = e.NewPageIndex; CargarTiempos();
        }
    }
}
