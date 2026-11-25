using System;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmGestionExamenCliente : Page
    {

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                {
                    Response.Redirect("~/Reload.html");
                    return;
                }

                Page.Form.Attributes.Add("enctype", "multipart/form-data");
                if (IsPostBack) ValidarAccesoCliente();
                ScriptManager sm = ScriptManager.GetCurrent(Page);

                if (sm != null)
                {
                    sm.RegisterPostBackControl(BtnInsertarImagenCliente);
                }

                if (!IsPostBack)
                {
                    int codigoEXSO = 0;

                    if (!int.TryParse(Request["CodigoEXSO"], out codigoEXSO))
                    {
                        Lblerror.Text = "No se recibió el código de la solicitud.";
                        return;
                    }

                    if (codigoEXSO <= 0)
                    {
                        Lblerror.Text = "El código de solicitud no es válido.";
                        return;
                    }

                    ViewState["CodigoEXSO"] = codigoEXSO;
                    LblCodigoSolicitud.Text = codigoEXSO.ToString();
                    Lbltitulo.Text = "Gestión Cliente - Exámenes";

                    ValidarAccesoCliente();
                    CargarSolicitud();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion


        #region Carga

        private void CargarSolicitud()
        {
            try
            {
                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);

                object[] parametros = CrearParametrosOperacionSolicitud(19, codigoEXSO);

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count < 2)
                {
                    Lblerror.Text = "No fue posible obtener la información de la solicitud.";
                    return;
                }

                DataTable pacientes = ds.Tables[1];

                if (pacientes == null || pacientes.Rows.Count == 0)
                {
                    RptPacientes.DataSource = null;
                    RptPacientes.DataBind();
                    PnlSinPacientes.Visible = true;
                    return;
                }

                PnlSinPacientes.Visible = false;
                RptPacientes.DataSource = pacientes;
                RptPacientes.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void RptPacientes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                {
                    return;
                }

                DataRowView fila = e.Item.DataItem as DataRowView;

                if (fila == null)
                {
                    return;
                }

                int codigoPERS = 0;

                int.TryParse(fila["PERS_CODIGO"].ToString(), out codigoPERS);

                if (codigoPERS <= 0)
                {
                    return;
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);
                Repeater rptResultados = e.Item.FindControl("RptResultados") as Repeater;
                bool puedeGestionarRechazado = false;
                Label lblSinResultados = e.Item.FindControl("LblSinResultados") as Label;

                object[] parametrosResultado = CrearParametrosOperacionSolicitud(20, codigoEXSO);
                parametrosResultado[36] = codigoPERS;

                DataSet dsResultado = new Conexion(2, "").FunInsertSolictudExamen(parametrosResultado);

                if (dsResultado != null && dsResultado.Tables.Count >= 2 && dsResultado.Tables[1].Rows.Count > 0)
                {
                    if (rptResultados != null)
                    {
                        rptResultados.DataSource = dsResultado.Tables[1];
                        rptResultados.DataBind();
                    }

                    if (lblSinResultados != null)
                    {
                        lblSinResultados.Visible = false;
                    }
                }
                else
                {
                    if (rptResultados != null)
                    {
                        rptResultados.DataSource = null;
                        rptResultados.DataBind();
                    }

                    if (lblSinResultados != null)
                    {
                        lblSinResultados.Visible = true;
                    }
                }


                // ==========================================
                // AUDITORIA - TIPO 22
                // ==========================================

                object[] parametrosAuditoria = CrearParametrosOperacionSolicitud(22, codigoEXSO);
                parametrosAuditoria[36] = codigoPERS;
                DataSet dsAuditoria = new Conexion(2, "").FunInsertSolictudExamen(parametrosAuditoria);

                ViewState["EsApelableCliente_" + codigoPERS] = false;
                Label lblEstado = e.Item.FindControl("LblEstadoAuditoria") as Label;
                Label lblEstadoDetalle = e.Item.FindControl("LblEstadoAuditoriaDetalle") as Label;
                Literal litInforme = e.Item.FindControl("LitInformeAuditor") as Literal;
                Label lblAdjunto = e.Item.FindControl("LblAdjuntoAuditor") as Label;

                if (dsAuditoria != null && dsAuditoria.Tables.Count > 0 && dsAuditoria.Tables[0].Rows.Count > 0)
                {

                    DataRow auditoria = dsAuditoria.Tables[0].Rows[0];
                    string estado = auditoria["EXRA_ESTADO"].ToString().Trim().ToUpper();
                    puedeGestionarRechazado = false; // La gestión anterior de rechazos queda deshabilitada.
                    bool esApelable = estado == "RECHAZADO APELABLE";
                    string informe = auditoria["EXRA_OBSERVACION"] != DBNull.Value ? auditoria["EXRA_OBSERVACION"].ToString() : "";
                    string adjunto = auditoria["EXRA_NOMBRE_DOC"].ToString().Trim();

                    if (lblEstado != null)
                    {
                        lblEstado.Text = estado;

                        if (estado == "ACEPTADO")
                        {
                            lblEstado.CssClass = "pull-right estado-aceptado";
                        }
                        else if (
                            estado == "RECHAZADO")
                        {
                            lblEstado.CssClass = "pull-right estado-rechazado";
                        }
                        else if (estado == "RECHAZADO APELABLE")
                        {
                            lblEstado.CssClass = "pull-right estado-apelable";
                        }
                        else
                        {
                            lblEstado.CssClass = "pull-right estado-pendiente";
                        }
                    }

                    if (lblEstadoDetalle != null)
                    {
                        lblEstadoDetalle.Text = estado;

                        if (estado == "ACEPTADO")
                        {
                            lblEstadoDetalle.ForeColor = System.Drawing.Color.Green;
                        }
                        else if (estado == "RECHAZADO")
                        {
                            lblEstadoDetalle.ForeColor = System.Drawing.Color.Red;
                        }
                        else if (estado == "RECHAZADO APELABLE")
                        {
                            lblEstadoDetalle.ForeColor = System.Drawing.Color.DarkOrange;
                        }
                    }

                    PrepararInformeCompacto(e.Item, codigoPERS, informe, adjunto, Convert.ToString(auditoria["EXRA_EXTENSION"]));
                    ViewState["EsApelableCliente_" + codigoPERS] = esApelable;
                    if (lblAdjunto != null)
                    {
                        lblAdjunto.Text = string.IsNullOrEmpty(adjunto) ? "Sin archivo adjunto." : HttpUtility.HtmlEncode(adjunto);
                    }
                }

                // ==========================================
                // GESTIÓN CLIENTE
                // TIPO 30
                // ==========================================

                object[] parametrosCliente = CrearParametrosOperacionSolicitud(30, codigoEXSO);

                parametrosCliente[36] = codigoPERS;

                DataSet dsCliente = new Conexion(2, "").FunInsertSolictudExamen(parametrosCliente);
                RadioButton rdbClienteAceptado = e.Item.FindControl("RdbClienteAceptado") as RadioButton;
                RadioButton rdbClienteRechazado = e.Item.FindControl("RdbClienteRechazado") as RadioButton;
                Label lblEstadoCliente = e.Item.FindControl("LblEstadoCliente") as Label;
                Label lblResumenCliente = e.Item.FindControl("LblResumenInformeCliente") as Label;
                Label lblAdjuntoCliente = e.Item.FindControl("LblAdjuntoCliente") as Label;

                GridView historial = e.Item.FindControl("GrdvHistorialCliente") as GridView;


                // ==========================================
                // GESTIÓN VIGENTE
                // ==========================================

                if (dsCliente != null && dsCliente.Tables.Count > 0 && dsCliente.Tables[0].Rows.Count > 0)
                {
                    DataRow gestion = dsCliente.Tables[0].Rows[0];
                    string estadoCliente = gestion["EXRC_ESTADO"].ToString().Trim().ToUpper();


                    if (rdbClienteAceptado != null)
                    {
                        rdbClienteAceptado.Checked = estadoCliente == "ACEPTADO";
                    }

                    if (rdbClienteRechazado != null)
                    {
                        rdbClienteRechazado.Checked = estadoCliente == "RECHAZADO";
                    }

                    if (lblEstadoCliente != null)
                    {
                        lblEstadoCliente.Text = estadoCliente;


                        if (estadoCliente == "ACEPTADO")
                        {
                            lblEstadoCliente.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            lblEstadoCliente.ForeColor = System.Drawing.Color.Red;
                        }
                    }

                    string informeCliente = gestion["EXRC_OBSERVACION"] != DBNull.Value ? gestion["EXRC_OBSERVACION"].ToString() : "";

                    string claveBorrador = "InformeCliente_" + codigoPERS.ToString();
                    if (ViewState[claveBorrador] == null)
                        ViewState[claveBorrador] = informeCliente;

                    if (lblResumenCliente != null)
                    {
                        if (string.IsNullOrWhiteSpace(informeCliente))
                        {
                            lblResumenCliente.Text = "Sin informe";
                            lblResumenCliente.ForeColor = System.Drawing.Color.Gray;
                        }
                        else
                        {
                            lblResumenCliente.Text = "Informe registrado";
                            lblResumenCliente.ForeColor = System.Drawing.Color.Green;
                        }
                    }

                    if (lblAdjuntoCliente != null)
                    {
                        string adjuntoCliente = gestion["EXRC_NOMBRE_DOC"].ToString().Trim();
                        lblAdjuntoCliente.Text = string.IsNullOrEmpty(adjuntoCliente) ? "Sin archivo adjunto." : "Archivo actual: " + adjuntoCliente;
                    }
                }
                else
                {
                    if (lblEstadoCliente != null)
                    {
                        lblEstadoCliente.Text = "PENDIENTE";
                        lblEstadoCliente.ForeColor = System.Drawing.Color.Gray;
                    }
                }


                // ==========================================
                // HISTORIAL
                // TABLES[1]
                // ==========================================

                if (historial != null)
                {
                    if (dsCliente != null && dsCliente.Tables.Count >= 2)
                    {
                        historial.DataSource = dsCliente.Tables[1];
                    }
                    else
                    {
                        historial.DataSource = null;
                    }

                    historial.DataBind();
                }


                // ==========================================
                // FILEUPLOAD CLIENTE
                // NECESITA POSTBACK COMPLETO
                // ==========================================

                Button btnGuardarCliente = e.Item.FindControl("BtnGuardarCliente") as Button;
                if (btnGuardarCliente != null)
                {
                    btnGuardarCliente.Enabled = puedeGestionarRechazado;
                    btnGuardarCliente.Visible = puedeGestionarRechazado;
                }
                if (rdbClienteAceptado != null) rdbClienteAceptado.Enabled = puedeGestionarRechazado;
                if (rdbClienteRechazado != null) rdbClienteRechazado.Enabled = puedeGestionarRechazado;
                WebControl editarInforme = e.Item.FindControl("BtnEditarInformeCliente") as WebControl;
                if (editarInforme != null) editarInforme.Visible = puedeGestionarRechazado;
                FileUpload subirAdjunto = e.Item.FindControl("FupAdjuntoCliente") as FileUpload;
                if (subirAdjunto != null) subirAdjunto.Enabled = puedeGestionarRechazado;

                ScriptManager sm = ScriptManager.GetCurrent(Page);

                if (sm != null && btnGuardarCliente != null)
                {
                    sm.RegisterPostBackControl(btnGuardarCliente);
                }


                // ==========================================
                // HISTORIAL
                // TODAVIA VACIO HASTA CONECTAR TABLA
                // ==========================================

                //GridView historial =
                //    e.Item.FindControl(
                //        "GrdvHistorialCliente"
                //    ) as GridView;


                //if (historial != null)
                //{
                //    historial.DataSource =
                //        CrearTablaHistorialVacia();

                //    historial.DataBind();
                //}
                bool apelableActual = ViewState["EsApelableCliente_" + codigoPERS] != null
                    && Convert.ToBoolean(ViewState["EsApelableCliente_" + codigoPERS]);
                Panel gestionNormal = e.Item.FindControl("PnlGestionNormal") as Panel;
                Panel gestionApelable = e.Item.FindControl("PnlGestionApelable") as Panel;
                Panel historialNormal = e.Item.FindControl("PnlHistorialGestionNormal") as Panel;
                Panel gestionPaciente = e.Item.FindControl("PnlGestionClientePaciente") as Panel;
                if (gestionPaciente != null) gestionPaciente.Visible = apelableActual;
                if (gestionNormal != null) gestionNormal.Visible = false;
                if (gestionApelable != null) gestionApelable.Visible = apelableActual;
                if (historialNormal != null) historialNormal.Visible = false;
                if (apelableActual) CargarBorradorPaciente(e.Item, codigoPERS);

            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion


        #region Informe Cliente

        protected void BtnEditarInformeCliente_Command(object sender, CommandEventArgs e)
        {
            PnlEditorCliente.Visible = false;
            Lblerror.Text = "Esta gestión anterior ya no está disponible. Solo los rechazos apelables permiten solicitar exámenes adicionales.";

        }


        protected void BtnAceptarInformeCliente_Click(object sender, EventArgs e)
        {
            PnlEditorCliente.Visible = false;
            Lblerror.Text = "Esta gestión anterior ya no está disponible. Solo los rechazos apelables permiten solicitar exámenes adicionales.";

        }

        protected void BtnInsertarImagenCliente_Click(object sender, EventArgs e)
        {
            PnlEditorCliente.Visible = false;
            Lblerror.Text = "Esta gestión anterior ya no está disponible. Solo los rechazos apelables permiten solicitar exámenes adicionales.";

        }


        protected void BtnCerrarEditorCliente_Click(object sender, EventArgs e)
        {
            PnlEditorCliente.Visible = false;
        }

        #endregion


        #region Guardar Cliente

        protected void BtnGuardarCliente_Command(object sender, CommandEventArgs e)
        {
            PnlEditorCliente.Visible = false;
            Lblerror.Text = "Esta gestión anterior ya no está disponible. Solo los rechazos apelables permiten solicitar exámenes adicionales.";

        }

        #endregion


        #region Helpers

        private void ValidarRechazoAuditor(int codigoPERS)
        {
            throw new InvalidOperationException("La gestión anterior del cliente está deshabilitada.");

        }

        private DataSet OperacionNotificacionCliente(int codigoEXSO, string accion, Guid token)
        {
            object[] parametros = new object[4];
            parametros[0] = codigoEXSO;
            parametros[1] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[2] = accion;
            parametros[3] = token.ToString();
            return new Conexion(2, "").funConsultarSqls("sp_Examen_Cliente_Notificacion", parametros);
        }
        private bool NotificarGestionClienteCompleta(int codigoEXSO)
        {
            Guid token = Guid.NewGuid();
            bool reservado = false;
            bool smtpConfirmado = false;
            try
            {
                DataSet aviso = OperacionNotificacionCliente(codigoEXSO, "RESERVAR", token);
                if (aviso == null || aviso.Tables.Count == 0 || aviso.Tables[0].Rows.Count == 0)
                    throw new InvalidOperationException("No se confirmó la reserva del correo.");
                string resultadoAviso = aviso.Tables[0].Rows[0]["Resultado"].ToString();
                if (resultadoAviso == "PENDIENTE" || resultadoAviso == "YA_REGISTRADO" || resultadoAviso == "NO_APLICA") return true;
                if (resultadoAviso != "RESERVADO") throw new InvalidOperationException("Respuesta de notificación no válida.");
                reservado = true;
                if (aviso.Tables.Count < 2 || aviso.Tables[1].Rows.Count == 0)
                    throw new InvalidOperationException("No se obtuvieron los pacientes del correo.");
                System.Text.StringBuilder cuerpo = new System.Text.StringBuilder();
                cuerpo.Append("<h3>GESTIÓN DEL CLIENTE FINALIZADA</h3>");
                cuerpo.Append("<p><strong>Titular:</strong> ")
                    .Append(HttpUtility.HtmlEncode(aviso.Tables[0].Rows[0]["Titular"].ToString())).Append("</p>");
                cuerpo.Append("<p>Se completaron los informes del cliente para los resultados rechazados por auditoría.</p>");
                foreach (DataRow paciente in aviso.Tables[1].Rows)
                {
                    string rol = paciente["TipoPersona"].ToString() == "TITULAR" ? "Titular" : "Codeudor";
                    cuerpo.Append("<p><strong>").Append(rol).Append(":</strong> ")
                        .Append(HttpUtility.HtmlEncode(paciente["Paciente"].ToString()))
                        .Append("<br /><strong>Resultado:</strong> ")
                        .Append(HttpUtility.HtmlEncode(paciente["EstadoCliente"].ToString())).Append("</p>");
                }
                string body = cuerpo.ToString();
                string correo = new Funciones().SendHtmlEmailExamen("vroldan@prestasalud.com", "GESTIÓN DEL CLIENTE FINALIZADA", body, "mail.prestasalud.com", 587,
    true, "info@prestasalud.com", "Info.Presta.2025$", "", "", "ealvear@prestasalud.com,vgavilanez@prestasalud.com");
                // Confirmar el contrato de SendHtmlEmailExamen: se presupone vacío al tener éxito.
                if (!string.IsNullOrWhiteSpace(correo))
                    throw new InvalidOperationException("El servicio de correo no confirmó el envío.");
                smtpConfirmado = true;
                DataSet confirmado = OperacionNotificacionCliente(codigoEXSO, "ENVIADO", token);
                if (confirmado == null || confirmado.Tables.Count == 0 || confirmado.Tables[0].Rows.Count == 0 ||
                    confirmado.Tables[0].Rows[0]["Resultado"].ToString() != "OK")
                    throw new InvalidOperationException("El correo se envió, pero no se confirmó su registro.");
                return true;
            }
            catch (Exception)
            {
                if (reservado && !smtpConfirmado)
                {
                    // No reintentar automáticamente: SMTP puede haber aceptado el mensaje antes del error.
                    try { OperacionNotificacionCliente(codigoEXSO, "REVISAR", token); } catch { }
                }
                Lblerror.Text = smtpConfirmado
                    ? "La gestión y el informe del cliente se guardaron y el correo se envió, pero falta confirmar el registro de notificación. Revise antes de reenviar."
                    : "La gestión y el informe del cliente se guardaron. No se confirmó el correo; revise el registro de notificación antes de reenviar.";
                return false;
            }
        }


        private object[] CrearParametrosOperacionSolicitud(int tipo, int codigoEXSO)
        {

            object[] parametros = new object[43];
            parametros[0] = tipo;
            parametros[1] = 0;
            parametros[2] = "";
            parametros[3] = "";
            parametros[4] = "";
            parametros[5] = "";
            parametros[6] = "";
            parametros[7] = "";
            parametros[8] = "";
            parametros[9] = "";
            parametros[10] = "";
            parametros[11] = 0;
            parametros[12] = "";
            parametros[13] = "";
            parametros[14] = "";
            parametros[15] = "";
            parametros[16] = "";
            parametros[17] = 0;
            parametros[18] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[19] = DateTime.Now;
            parametros[20] = "";
            parametros[21] = new byte[0];
            parametros[22] = "";
            parametros[23] = "";
            parametros[24] = "";
            parametros[25] = 0;
            parametros[26] = "0.00";
            parametros[27] = "0.00";
            parametros[28] = "0";
            parametros[29] = codigoEXSO;
            parametros[30] = "Activo";
            parametros[31] = "";
            parametros[32] = "";
            parametros[33] = "";
            parametros[34] = "";
            parametros[35] = "";
            parametros[36] = 0;
            parametros[37] = 0;
            parametros[38] = 0;
            parametros[39] = 0;
            parametros[40] = 0;
            parametros[41] = Convert.ToInt32(Session["usuCodigo"]);
            parametros[42] = Session["MachineName"] != null ? Session["MachineName"].ToString() : "";

            return parametros;
        }

        public string FormatearFecha(object valor)
        {

            if (valor == null || valor == DBNull.Value)
            {
                return "";
            }

            DateTime fecha;

            if (DateTime.TryParse(valor.ToString(), out fecha))
            {
                return fecha.ToString("dd/MM/yyyy");
            }

            return valor.ToString();
        }


        public string FormatearMonto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "$ 0,00";
            }

            string texto = valor.ToString().Trim();
            decimal monto = 0m;

            if (!decimal.TryParse(texto, NumberStyles.Any, CultureInfo.InvariantCulture, out monto))
            {
                decimal.TryParse(texto, NumberStyles.Any, new CultureInfo("es-EC"), out monto);
            }

            return "$ " + monto.ToString("N2", new CultureInfo("es-EC"));
        }


        public bool EsPdf(object extension)
        {
            if (extension == null || extension == DBNull.Value)
            {
                return false;
            }

            string ext = extension.ToString().Trim().ToLower();

            return ext == ".pdf" || ext == "pdf";
        }


        public string MostrarTipoArchivo(object extension)
        {
            if (extension == null || extension == DBNull.Value)
            {
                return "";
            }

            string ext = extension.ToString().Trim().ToLower().Replace(".", "");

            if (ext == "pdf")
                return "PDF";

            if (ext == "xls" || ext == "xlsx")
                return "EXCEL";

            return ext.ToUpper();
        }


        public string ObtenerUrlResultado(object codigoEXRD)
        {
            string codigoEXSO = ViewState["CodigoEXSO"] != null ? ViewState["CodigoEXSO"].ToString() : "0";

            return ResolveUrl("~/Examenes/VerResultadoExamen.ashx" + "?exso=" + HttpUtility.UrlEncode(codigoEXSO) + "&exrd=" + HttpUtility.UrlEncode(codigoEXRD.ToString()));
        }


        private DataTable CrearTablaHistorialVacia()
        {

            DataTable tabla = new DataTable();
            tabla.Columns.Add("Fecha");
            tabla.Columns.Add("Usuario");
            tabla.Columns.Add("Estado");
            tabla.Columns.Add("Informe");
            tabla.Columns.Add("Adjunto");

            return tabla;
        }

        private class ImagenInformeCliente
        {
            public int Orden { get; set; }
            public string Token { get; set; }
            public string Mime { get; set; }
            public string Extension { get; set; }
            public string Nombre { get; set; }
            public byte[] Bytes { get; set; }
        }
        private List<ImagenInformeCliente> ExtraerImagenesCliente(ref string html)
        {
            List<ImagenInformeCliente> imagenes = new List<ImagenInformeCliente>();

            if (string.IsNullOrEmpty(html))
            {
                return imagenes;
            }

            int orden = 0;

            string patron = @"src\s*=\s*[""']data:(?<mime>image\/(?:png|jpeg|jpg|gif));base64,(?<data>[^""']+)[""']";


            html =
                Regex.Replace(html, patron,

                    delegate (Match match)
                    {
                        orden++;

                        string mime = match.Groups["mime"].Value.ToLower();
                        string base64 = match.Groups["data"].Value;
                        byte[] bytes;

                        try
                        {
                            bytes = Convert.FromBase64String(base64);
                        }
                        catch
                        {
                            throw new Exception("No fue posible procesar la imagen " + orden.ToString() + " del informe.");
                        }

                        string extension = ".png";

                        if (mime == "image/jpeg" || mime == "image/jpg")
                        {
                            extension = ".jpg";
                        }
                        else if (mime == "image/gif")
                        {
                            extension = ".gif";
                        }

                        string token = "__CLIENTE_IMAGEN_" + orden.ToString() + "__";
                        ImagenInformeCliente imagen = new ImagenInformeCliente();

                        imagen.Orden = orden;
                        imagen.Token = token;
                        imagen.Mime = mime;
                        imagen.Extension = extension;
                        imagen.Nombre = "imagen_cliente_" + orden.ToString() + extension;
                        imagen.Bytes = bytes;
                        imagenes.Add(imagen);

                        return
                            "src=\"" + token + "\"";
                    },

                    RegexOptions.IgnoreCase
                );

            return imagenes;
        }

        #endregion


        #region Salir

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Examenes/FrmNuevoExamenCliente.aspx", true);
        }

        #endregion

        private void ValidarAccesoCliente()
        {
            int usuario, solicitud;
            if (!int.TryParse(Convert.ToString(Session["usuCodigo"]), out usuario) || usuario <= 0
                || !int.TryParse(Convert.ToString(ViewState["CodigoEXSO"]), out solicitud) || solicitud <= 0)
                throw new InvalidOperationException("La sesión o la solicitud no es válida.");
            object[] argumentos = { usuario, null, null, "", "", 1, 1, solicitud };
            DataSet ds = new Conexion(2, "").FunSeguimientoExamenesCliente(argumentos);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1
                || !Convert.ToBoolean(ds.Tables[0].Rows[0]["PuedeGestionar"]))
                throw new InvalidOperationException("La solicitud no está disponible en su campaña o falta completar la auditoría.");
        }
        private DataRow LeerInformeOrden(int persona, string accion = "LEER", string texto = null, DateTime? version = null)
        {
            DataSet ds = new Conexion(2, "").FunInformeOrdenCliente(
                Convert.ToInt32(Session["usuCodigo"]), Convert.ToInt32(ViewState["CodigoEXSO"]), persona, accion, texto, version);
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1)
                throw new InvalidOperationException("No se pudo consultar el informe del auditor.");
            return ds.Tables[0].Rows[0];
        }
        public string ResumenInformeAuditor(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "Sin informe registrado.";
            string texto = Regex.Replace(html, @"<[^>]*>", " ");
            texto = Regex.Replace(HttpUtility.HtmlDecode(texto), @"\s+", " ").Trim();
            if (texto.Length == 0) return "Informe con contenido gráfico. Pulse Ver informe completo.";
            return texto.Length > 180 ? texto.Substring(0, 180) + "…" : texto;
        }
        private void PrepararInformeCompacto(RepeaterItem item, int persona, string html, string adjunto, string extension)
        {
            Literal resumen = item.FindControl("LitInformeAuditor") as Literal;
            if (resumen != null) resumen.Text = HttpUtility.HtmlEncode(ResumenInformeAuditor(html));
            Button ver = item.FindControl("BtnVerInformeAuditor") as Button;
            if (ver != null) ver.Visible = !string.IsNullOrWhiteSpace(html);
            HyperLink verPdf = item.FindControl("LnkVerAdjuntoAuditor") as HyperLink;
            HyperLink descargar = item.FindControl("LnkDescargarAdjuntoAuditor") as HyperLink;
            string url = ResolveUrl("~/Examenes/VerAdjuntoAuditorCliente.ashx?exso="
                + Convert.ToInt32(ViewState["CodigoEXSO"]) + "&pers=" + persona);
            if (verPdf != null) { verPdf.Visible = !string.IsNullOrWhiteSpace(adjunto) && EsPdf(extension); verPdf.NavigateUrl = url; }
            if (descargar != null) { descargar.Visible = !string.IsNullOrWhiteSpace(adjunto); descargar.NavigateUrl = url + "&descargar=1"; }
        }
        protected void BtnVerInformeAuditor_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int persona;
                if (!int.TryParse(Convert.ToString(e.CommandArgument), out persona) || persona <= 0)
                    throw new InvalidOperationException("No se pudo identificar al paciente.");
                ValidarAccesoCliente();
                DataRow r = LeerInformeOrden(persona);
                string html = Convert.ToString(r["EXRA_OBSERVACION"]);
                if (string.IsNullOrWhiteSpace(html)) throw new InvalidOperationException("El auditor no registró un informe.");
                string documento = "<!doctype html><html><head><meta charset='utf-8'><style>body{font:14px Arial,sans-serif;padding:16px;overflow-wrap:anywhere;}img{max-width:100%;height:auto;}table{max-width:100%;}</style></head><body>" + html + "</body></html>";
                LitVisorInformeAuditor.Text = "<iframe title=\"Informe completo del auditor\" sandbox=\"allow-same-origin\" style=\"width:100%;height:100%;border:0;\" srcdoc=\""
                    + HttpUtility.HtmlAttributeEncode(documento) + "\"></iframe>";
                LblTituloVisorAuditor.Text = HttpUtility.HtmlEncode("Informe del auditor · " + Convert.ToString(r["Paciente"]));
                PnlVisorInformeAuditor.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "VerInformeAuditorCliente",
                    "abrirInformeAuditorCliente('" + HttpUtility.JavaScriptStringEncode(PnlVisorInformeAuditor.ClientID) + "');", true);
            }
            catch (Exception ex) { Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); }
        }
        private void CargarBorradorPaciente(RepeaterItem item, int persona, DataRow datos = null)
        {
            DataRow r = datos ?? LeerInformeOrden(persona);
            ViewState["VersionAuditorCliente_" + persona] = Convert.ToDateTime(r["VersionAuditoria"]);
            TextBox texto = item.FindControl("TxtExamenesAdicionales") as TextBox;
            Label resumen = item.FindControl("LblBorradorAdicional") as Label;
            Button guardar = item.FindControl("BtnGuardarBorradorAdicional") as Button;
            string estado = Convert.ToString(r["EstadoOrden"]);
            bool editable = Convert.ToString(r["EXRA_ESTADO"]) == "RECHAZADO APELABLE" && (estado == "" || estado == "BORRADOR");
            if (texto != null) { texto.Text = Convert.ToString(r["ExamenesSolicitados"]); texto.Enabled = editable; }
            if (guardar != null) guardar.Enabled = editable;
            if (resumen != null)
            {
                if (Convert.ToInt32(r["OrdenCodigo"]) == 0) resumen.Text = "Sin borrador guardado.";
                else if (estado == "BORRADOR")
                {
                    DateTime fecha = Convert.ToDateTime(r["FechaModificacion"] == DBNull.Value ? r["FechaCreacion"] : r["FechaModificacion"]);
                    resumen.Text = "Borrador guardado el " + fecha.ToString("dd/MM/yyyy HH:mm") + ". Aún no se ha enviado.";
                }
                else resumen.Text = HttpUtility.HtmlEncode(estado == "ENVIADA" ? "Orden enviada a agendamiento." : "Estado de la orden: " + estado);
            }
            Button enviar = item.FindControl("BtnEnviarOrdenAdicional") as Button;
            if (enviar != null) 
            { 
                enviar.Visible = editable; enviar.Enabled = editable; 
            }
            HyperLink pdf = item.FindControl("LnkPdfOrdenAdicional") as HyperLink;
            if (pdf != null)
            {
                int orden = Convert.ToInt32(r["OrdenCodigo"]); pdf.Visible = orden > 0;
                pdf.Text = estado == "BORRADOR" ? "Ver PDF del borrador guardado" : "Ver PDF de la orden enviada";
                pdf.NavigateUrl = ResolveUrl("~/Examenes/VerOrdenAdicionalPdf.ashx?ambito=cliente&orden=" + orden);
            }
            Label cabecera = item.FindControl("LblEstadoCliente") as Label;
            if (cabecera != null) 
            { 
                cabecera.Text = estado == "" ? "PENDIENTE" : HttpUtility.HtmlEncode(estado); cabecera.ForeColor = System.Drawing.Color.DarkOrange; 
            }
        }
        protected void BtnGuardarBorradorAdicional_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int persona;
                Button boton = sender as Button;
                RepeaterItem item = boton == null ? null : boton.NamingContainer as RepeaterItem;
                if (item == null || !int.TryParse(Convert.ToString(e.CommandArgument), out persona) || persona <= 0)
                    throw new InvalidOperationException("No se pudo identificar al paciente.");
                ValidarAccesoCliente();
                TextBox campo = item.FindControl("TxtExamenesAdicionales") as TextBox;
                string texto = campo == null ? "" : campo.Text.Trim();
                if (string.IsNullOrWhiteSpace(texto) || texto.Length > 12000)
                    throw new InvalidOperationException("Ingrese los exámenes solicitados, hasta 12000 caracteres.");
                object version = ViewState["VersionAuditorCliente_" + persona];
                if (version == null) throw new InvalidOperationException("Recargue la página y revise el informe del auditor.");
                DataRow r = LeerInformeOrden(persona, "BORRADOR", texto, Convert.ToDateTime(version));
                CargarBorradorPaciente(item, persona, r);
                Lblerror.Text = "Borrador guardado. Aún no se ha enviado a agendamiento.";
            }
            catch (Exception ex) 
            { 
                Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); 
            }
        }

        protected void BtnEnviarOrdenAdicional_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int persona; Button boton = sender as Button;
                RepeaterItem item = boton == null ? null : boton.NamingContainer as RepeaterItem;
                if (item == null || !int.TryParse(Convert.ToString(e.CommandArgument), out persona) || persona <= 0)
                    throw new InvalidOperationException("No se pudo identificar al paciente.");
                ValidarAccesoCliente();
                DataRow actual = LeerInformeOrden(persona);
                if (Convert.ToString(actual["EstadoOrden"]) == "ENVIADA" || Convert.ToString(actual["EstadoOrden"]) == "AGENDADA")
                { 
                    CargarBorradorPaciente(item, persona, actual); Lblerror.Text = "La orden ya fue enviada a agendamiento."; return; 
                }
                TextBox campo = item.FindControl("TxtExamenesAdicionales") as TextBox;
                string texto = campo == null ? "" : campo.Text.Trim();
                if (string.IsNullOrWhiteSpace(texto) || texto.Length > 12000) throw new InvalidOperationException("Ingrese los exámenes solicitados, hasta 12000 caracteres.");
                object version = ViewState["VersionAuditorCliente_" + persona];
                if (version == null) throw new InvalidOperationException("Recargue la página y revise el informe del auditor.");
                DataRow guardado = LeerInformeOrden(persona, "BORRADOR", texto, Convert.ToDateTime(version));
                int orden = Convert.ToInt32(guardado["OrdenCodigo"]);
                DataSet ds = new Conexion(2, "").FunOrdenAdicionalCliente(Convert.ToInt32(Session["usuCodigo"]), orden, "PREPARAR");
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1) throw new InvalidOperationException("No se pudo preparar la orden.");
                DataRow datos = ds.Tables[0].Rows[0];
                byte[] pdf = OrdenAdicionalPdf.Crear(datos);
                string nombre = "Solicitud_" + ViewState["CodigoEXSO"] + "_OrdenAdicional_" + orden + ".pdf";
                DataSet envio = new Conexion(2, "").FunOrdenAdicionalCliente(Convert.ToInt32(Session["usuCodigo"]), orden, "ENVIAR",
                    pdf, nombre, Convert.ToString(datos["ExamenesSolicitados"]), Convert.ToDateTime(datos["VersionAuditoria"]));
                if (envio == null || envio.Tables.Count == 0 || envio.Tables[0].Rows.Count != 1) throw new InvalidOperationException("No se pudo confirmar el envío de la orden.");
                string resultado = Convert.ToString(envio.Tables[0].Rows[0]["Resultado"]);
                if (resultado != "ENVIADA" && resultado != "YA_ENVIADA") throw new InvalidOperationException("No se confirmó el envío de la orden.");
                CargarBorradorPaciente(item, persona);
                Lblerror.Text = "Orden adicional enviada a agendamiento. Su PDF ya está disponible.";
            }
            catch (Exception ex) 
            { 
                Lblerror.Text = HttpUtility.HtmlEncode(ex.Message); 
            }
        }

    }
}
