using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmTiemposExamenes : Page
    {
        private bool exportandoExcel;
        protected override void Render(HtmlTextWriter writer)
        {
            if (!exportandoExcel) base.Render(writer);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager sm = ScriptManager.GetCurrent(Page);
            if (sm != null) sm.RegisterPostBackControl(BtnExportarExcel);
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
            string titulo = etapa == "Agenda" ? "Agendamiento" : etapa == "Auditoria" ? "Auditoría" : etapa == "Cliente" ? "Cliente" : "Resultados";
            string intervalo = etapa == "Agenda" ? "Solicitud enviada → Cita agendada" :
                etapa == "Resultados" ? "Cita agendada → Resultados enviados" : etapa == "Cliente" ? "Auditoría completa → Informes de rechazados" : "Resultados enviados → Todos auditados";
            string inicio = etapa == "Agenda" ? "FechaSolicitud" : etapa == "Resultados" ? "FechaRegistroAgenda" : etapa == "Cliente" ? "FechaInicioCliente" : "FechaEnvio";
            string fin = etapa == "Agenda" ? "FechaRegistroAgenda" : etapa == "Resultados" ? "FechaEnvio" : etapa == "Cliente" ? "FechaCierreCliente" : "FechaCierreAuditoria";
            string textoEstado = situacion == "FINALIZADO" ? "Finalizado" : situacion == "PENDIENTE" ? "En curso" :
                situacion == "NO INICIADO" ? "Por iniciar" : situacion == "NO APLICA" ? "No aplica" : "Sin registro";
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
            if (situacion == "NO APLICA") fechaInicio = "Sin rechazados";
            string fechaFin = situacion == "PENDIENTE" ? "Ahora (" + Convert.ToDateTime(r["FechaConsulta"]).ToString("HH:mm:ss") + ")" :
                situacion == "FINALIZADO" && r[fin] != DBNull.Value ? Convert.ToDateTime(r[fin]).ToString("dd/MM/yyyy HH:mm:ss") : "—";
            string progreso = etapa == "Auditoria" ? "<br />Pacientes: " +
                System.Web.HttpUtility.HtmlEncode(r["TotalAuditados"].ToString() + " / " + r["TotalEnviados"].ToString()) : etapa == "Cliente" ? "<br />Rechazados con informe: " +
                System.Web.HttpUtility.HtmlEncode(r["TotalInformados"].ToString() + " / " + r["TotalRechazados"].ToString()) : "";
            return "<div class=\"etapa " + clase + "\"><h4>" + titulo + "</h4><div class=\"intervalo\">" +
                System.Web.HttpUtility.HtmlEncode(intervalo) + "</div><div class=\"duracion\">" +
                System.Web.HttpUtility.HtmlEncode(duracion) + "</div><div class=\"situacion\">" + textoEstado +
                "</div><div class=\"fechas\">Inicio: " + System.Web.HttpUtility.HtmlEncode(fechaInicio) +
                "<br />Fin: " + System.Web.HttpUtility.HtmlEncode(fechaFin) + progreso + "</div></div>";
        }
        protected void BtnExportarExcel_Click(object sender, EventArgs e)
        {
            if (!SesionValida() || !GuardarFiltros()) return;
            byte[] archivo;
            try
            {
                object[] parametros = new object[5];
                parametros[0] = ((DateTime)ViewState["Desde"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                parametros[1] = ((DateTime)ViewState["Hasta"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture);
                parametros[2] = Convert.ToString(ViewState["Documento"]).Replace("'", "''");
                parametros[3] = Convert.ToString(ViewState["Estado"]);
                parametros[4] = 1;
                DataSet ds = new Conexion(2, "").funConsultarSqls("sp_Examen_Tiempos_Listar", parametros);
                if (ds == null || ds.Tables.Count == 0) throw new InvalidOperationException();
                DataTable datos = ds.Tables[0];
                if (datos.Rows.Count == 0)
                {
                    LblError.Text = "No hay solicitudes para exportar con los filtros seleccionados.";
                    return;
                }
                if (datos.Rows.Count > 1048572)
                {
                    LblError.Text = "El reporte supera el límite de filas de Excel. Reduzca el rango de fechas.";
                    return;
                }
                archivo = CrearExcelTiempos(datos);
            }
            catch (Exception)
            {
                LblError.Text = "No se pudo generar Excel. Verifique la actualización del SP y las referencias de la aplicación.";
                return;
            }
            exportandoExcel = true;
            Response.Clear();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"Tiempos_Examenes_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx\"");
            Response.AddHeader("Content-Length", archivo.Length.ToString(CultureInfo.InvariantCulture));
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Response.BinaryWrite(archivo);
            Context.ApplicationInstance.CompleteRequest();
        }
        private const string NsExcel = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        private byte[] CrearExcelTiempos(DataTable datos)
        {
            using (System.IO.MemoryStream memoria = new System.IO.MemoryStream())
            {
                using (System.IO.Packaging.Package paquete = System.IO.Packaging.Package.Open(memoria, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                {
                    System.IO.Packaging.PackagePart libro = ParteExcel(paquete, "/xl/workbook.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");
                    System.IO.Packaging.PackagePart hoja = ParteExcel(paquete, "/xl/worksheets/sheet1.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml");
                    System.IO.Packaging.PackagePart estilos = ParteExcel(paquete, "/xl/styles.xml", "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
                    paquete.CreateRelationship(new Uri("/xl/workbook.xml", UriKind.Relative), System.IO.Packaging.TargetMode.Internal,
                        "http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument", "rId1");
                    libro.CreateRelationship(new Uri("worksheets/sheet1.xml", UriKind.Relative), System.IO.Packaging.TargetMode.Internal,
                        "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet", "rId1");
                    libro.CreateRelationship(new Uri("styles.xml", UriKind.Relative), System.IO.Packaging.TargetMode.Internal,
                        "http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles", "rId2");
                    using (System.Xml.XmlWriter w = EscritorExcel(libro))
                    {
                        w.WriteStartElement("workbook", NsExcel);
                        w.WriteStartElement("sheets", NsExcel); w.WriteStartElement("sheet", NsExcel);
                        w.WriteAttributeString("name", "Tiempos"); w.WriteAttributeString("sheetId", "1");
                        w.WriteAttributeString("r", "id", "http://schemas.openxmlformats.org/officeDocument/2006/relationships", "rId1");
                        w.WriteEndElement(); w.WriteEndElement(); w.WriteEndElement();
                    }
                    using (System.Xml.XmlWriter w = EscritorExcel(estilos))
                    {
                        w.WriteRaw("<styleSheet xmlns=\"" + NsExcel + "\"><numFmts count=\"2\"><numFmt numFmtId=\"164\" formatCode=\"dd/mm/yyyy hh:mm:ss\"/><numFmt numFmtId=\"165\" formatCode=\"[h]:mm:ss\"/></numFmts><fonts count=\"2\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font><font><b/><color rgb=\"FFFFFFFF\"/><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts><fills count=\"3\"><fill><patternFill patternType=\"none\"/></fill><fill><patternFill patternType=\"gray125\"/></fill><fill><patternFill patternType=\"solid\"><fgColor rgb=\"FF2879BD\"/><bgColor indexed=\"64\"/></patternFill></fill></fills><borders count=\"1\"><border><left/><right/><top/><bottom/><diagonal/></border></borders><cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs><cellXfs count=\"4\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\"/><xf numFmtId=\"0\" fontId=\"1\" fillId=\"2\" borderId=\"0\" xfId=\"0\" applyAlignment=\"1\"><alignment wrapText=\"1\" vertical=\"center\"/></xf><xf numFmtId=\"164\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\"/><xf numFmtId=\"165\" fontId=\"0\" fillId=\"0\" borderId=\"0\" xfId=\"0\" applyNumberFormat=\"1\"/></cellXfs><cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles></styleSheet>");
                    }
                    EscribirReporteExcel(hoja, datos);
                }
                return memoria.ToArray();
            }
        }
        private static System.IO.Packaging.PackagePart ParteExcel(System.IO.Packaging.Package p, string ruta, string tipo)
        {
            return p.CreatePart(new Uri(ruta, UriKind.Relative), tipo, System.IO.Packaging.CompressionOption.Normal);
        }
        private static System.Xml.XmlWriter EscritorExcel(System.IO.Packaging.PackagePart p)
        {
            return System.Xml.XmlWriter.Create(p.GetStream(), new System.Xml.XmlWriterSettings { Encoding = new System.Text.UTF8Encoding(false), CloseOutput = true, CheckCharacters = true });
        }
        private void EscribirReporteExcel(System.IO.Packaging.PackagePart hoja, DataTable datos)
        {
            string[] campos = { "Solicitud", "Documento", "Paciente", "Estado", "FechaSolicitud",
                "FechaRegistroAgenda", "SegundosAgenda", "SituacionAgenda",
                "FechaEnvio", "SegundosResultados", "SituacionResultados",
                "CierreAuditoriaExportar", "SegundosAuditoria", "SituacionAuditoria", "TotalAuditados", "TotalEnviados",
                "FechaInicioCliente", "CierreClienteExportar", "SegundosCliente", "SituacionCliente", "TotalInformados", "TotalRechazados", "FechaConsulta" };
            string[] titulos = { "Solicitud", "Documento titular", "Titular", "Estado solicitud", "Solicitud enviada / Inicio agendamiento",
                "Agendado el / Inicio resultados", "Tiempo agendamiento (lun-vie)", "Situación agendamiento",
                "Resultados enviados / Inicio auditoría", "Tiempo resultados", "Situación resultados",
                "Auditoría completada", "Tiempo auditoría", "Situación auditoría", "Pacientes auditados", "Resultados enviados",
                "Inicio cliente", "Informes cliente completados", "Tiempo cliente", "Situación cliente", "Rechazados con informe", "Total rechazados", "Fecha de corte" };
            using (System.Xml.XmlWriter w = EscritorExcel(hoja))
            {
                w.WriteStartElement("worksheet", NsExcel);
                w.WriteStartElement("sheetViews", NsExcel); w.WriteStartElement("sheetView", NsExcel); w.WriteAttributeString("workbookViewId", "0");
                w.WriteStartElement("pane", NsExcel); w.WriteAttributeString("ySplit", "4"); w.WriteAttributeString("topLeftCell", "A5"); w.WriteAttributeString("activePane", "bottomLeft"); w.WriteAttributeString("state", "frozen");
                w.WriteEndElement(); w.WriteEndElement(); w.WriteEndElement();
                w.WriteStartElement("cols", NsExcel);
                for (int i = 0; i < campos.Length; i++)
                {
                    w.WriteStartElement("col", NsExcel); w.WriteAttributeString("min", (i + 1).ToString()); w.WriteAttributeString("max", (i + 1).ToString());
                    w.WriteAttributeString("width", i == 2 ? "35" : i == 0 ? "12" : "24"); w.WriteAttributeString("customWidth", "1"); w.WriteEndElement();
                }
                w.WriteEndElement();
                w.WriteStartElement("sheetData", NsExcel);
                w.WriteStartElement("row", NsExcel); w.WriteAttributeString("r", "1"); CeldaExcel(w, "A1", "REPORTE DE TIEMPOS DE EXÁMENES", false, 1); w.WriteEndElement();
                w.WriteStartElement("row", NsExcel); w.WriteAttributeString("r", "2");
                string filtros = "Desde " + ((DateTime)ViewState["Desde"]).ToString("dd/MM/yyyy") + " hasta " + ((DateTime)ViewState["Hasta"]).ToString("dd/MM/yyyy") +
                    " | Documento: " + (Convert.ToString(ViewState["Documento"]) == "" ? "Todos" : Convert.ToString(ViewState["Documento"])) +
                    " | Estado: " + (Convert.ToString(ViewState["Estado"]) == "" ? "Todos" : Convert.ToString(ViewState["Estado"])) + " | Solicitudes: " + datos.Rows.Count.ToString();
                CeldaExcel(w, "A2", filtros, false, 0); w.WriteEndElement();
                w.WriteStartElement("row", NsExcel); w.WriteAttributeString("r", "3");
                CeldaExcel(w, "A3", "Duraciones [h]:mm:ss. Solo agendamiento excluye sábados y domingos; las otras etapas cuentan tiempo corrido. Vacío no equivale a cero: consultar la situación de cada etapa.", false, 0); w.WriteEndElement();
                w.WriteStartElement("row", NsExcel); w.WriteAttributeString("r", "4"); w.WriteAttributeString("ht", "45"); w.WriteAttributeString("customHeight", "1");
                for (int i = 0; i < titulos.Length; i++) CeldaExcel(w, ColumnaExcel(i + 1) + "4", titulos[i], false, 1);
                w.WriteEndElement();
                int numero = 5;
                foreach (DataRow r in datos.Rows)
                {
                    w.WriteStartElement("row", NsExcel); w.WriteAttributeString("r", numero.ToString());
                    for (int i = 0; i < campos.Length; i++)
                    {
                        string campo = campos[i]; object valor;
                        if (campo == "CierreAuditoriaExportar") valor = r["SituacionAuditoria"].ToString() == "FINALIZADO" ? r["FechaCierreAuditoria"] : DBNull.Value;
                        else if (campo == "CierreClienteExportar") valor = r["SituacionCliente"].ToString() == "FINALIZADO" ? r["FechaCierreCliente"] : DBNull.Value;
                        else valor = r[campo];
                        int estilo = 0; bool numerico = false;
                        if (valor != DBNull.Value && campo.StartsWith("Segundos"))
                        {
                            string situacion = r["Situacion" + campo.Substring(8)].ToString();
                            if (situacion != "PENDIENTE" && situacion != "FINALIZADO") valor = DBNull.Value;
                            else { valor = Convert.ToDouble(valor) / 86400d; estilo = 3; numerico = true; }
                        }
                        else if (valor is DateTime) { valor = ((DateTime)valor).ToOADate(); estilo = 2; numerico = true; }
                        else if (campo == "Solicitud" || campo.StartsWith("Total")) numerico = true;
                        if (valor != DBNull.Value) CeldaExcel(w, ColumnaExcel(i + 1) + numero.ToString(), Convert.ToString(valor, CultureInfo.InvariantCulture), numerico, estilo);
                    }
                    w.WriteEndElement(); numero++;
                }
                w.WriteEndElement();
                w.WriteStartElement("autoFilter", NsExcel); w.WriteAttributeString("ref", "A4:" + ColumnaExcel(campos.Length) + (numero - 1).ToString()); w.WriteEndElement();
                w.WriteStartElement("mergeCells", NsExcel); w.WriteAttributeString("count", "3");
                for (int i = 1; i <= 3; i++) { w.WriteStartElement("mergeCell", NsExcel); w.WriteAttributeString("ref", "A" + i + ":" + ColumnaExcel(campos.Length) + i); w.WriteEndElement(); }
                w.WriteEndElement(); w.WriteEndElement();
            }
        }
        private static string ColumnaExcel(int n)
        {
            string s = "";
            while (n > 0) { n--; s = (char)('A' + n % 26) + s; n /= 26; }
            return s;
        }
        private static void CeldaExcel(System.Xml.XmlWriter w, string referencia, string texto, bool numero, int estilo)
        {
            w.WriteStartElement("c", NsExcel); w.WriteAttributeString("r", referencia); w.WriteAttributeString("s", estilo.ToString());
            if (numero) w.WriteElementString("v", NsExcel, texto);
            else
            {
                w.WriteAttributeString("t", "inlineStr"); w.WriteStartElement("is", NsExcel); w.WriteStartElement("t", NsExcel);
                w.WriteAttributeString("xml", "space", "http://www.w3.org/XML/1998/namespace", "preserve");
                w.WriteString(texto); w.WriteEndElement(); w.WriteEndElement();
            }
            w.WriteEndElement();
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
