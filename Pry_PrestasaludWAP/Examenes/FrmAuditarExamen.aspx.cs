using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmAuditarExamen : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbexamenes = new DataTable();
        Object[] objparam = new Object[1];
        ImageButton imgexa1, imgexa2, imgexa3, imgexa4, imgexa5;
        string type = "", name = "", exa1 = "", exa2 = "", exa3 = "", exa4 = "", exa5 = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                try
                {
                    ViewState["CodigoEXSO"] = Request["CodigoEXSO"];
                    FunCargaMantenimiento();
                    Lbltitulo.Text = "Auditar Registro Exámenes";
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
       
        private void FunCargaMantenimiento()
        {
            try
            {
                int codigoEXSO = 0;

                if (ViewState["CodigoEXSO"] == null || !int.TryParse(ViewState["CodigoEXSO"].ToString(),out codigoEXSO))
                {
                    Lblerror.Text = "No se recibió el código de la solicitud.";
                    return;
                }

                if (codigoEXSO <= 0)
                {
                    Lblerror.Text = "El código de la solicitud no es válido.";
                    return;
                }

                object[] parametros = CrearParametrosOperacionSolicitud(19,codigoEXSO);

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
        private void FunDownloadDocument(int opcion)
        {
            try
            {
                Array.Resize(ref objparam, 11);
                objparam[0] = 28;
                objparam[1] = "";
                objparam[2] = "";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                switch (opcion)
                {
                    case 0:
                        objparam[6] = 0;
                        break;
                    case 1:
                        objparam[6] = 1;
                        break;
                    case 2:
                        objparam[6] = 2;
                        break;
                    case 3:
                        objparam[6] = 3;
                        break;
                    case 4:
                        objparam[6] = 4;
                        break;
                }
                dtx = new Conexion(2, "").FunConsultaDatos1(objparam);
                type = dtx.Tables[0].Rows[0]["Tipo"].ToString();
                name = dtx.Tables[0].Rows[0]["Nombre"].ToString();
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = type;
                Response.AddHeader("content-disposition", "attachment;filename=" + name);
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dtx.Tables[0].Rows[0]["DataBin"]);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        //protected void BtnGrabar_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(TxtObservacion.Text.Trim()))
        //        {
        //            new Funciones().funShowJSMessage("Ingrese Observación..!", this);
        //            return;
        //        }

        //        Array.Resize(ref objparam, 11);
        //        objparam[0] = 26;
        //        objparam[1] = TxtObservacion.Text.Trim().ToUpper();
        //        objparam[2] = "EAU";
        //        objparam[3] = "";
        //        objparam[4] = "";
        //        objparam[5] = "";
        //        objparam[6] = int.Parse(Session["usuCodigo"].ToString()); 
        //        objparam[7] = int.Parse(ViewState["CodigoEXSO"].ToString());
        //        objparam[8] = 0;
        //        objparam[9] = 0;
        //        objparam[10] = 0;
        //        dts = new Conexion(2, "").FunConsultaDatos1(objparam);
        //        Response.Redirect("FrmAuditarExamenAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        Lblerror.Text = ex.ToString();
        //    }
        //}

        private object[] CrearParametrosOperacionSolicitud(int tipo,int codigoEXSO)
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

        protected string FormatearFecha(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "";
            }

            DateTime fecha;

            if (DateTime.TryParse(valor.ToString(),out fecha))
            {
                return fecha.ToString("dd/MM/yyyy");
            }

            return valor.ToString();
        }

        protected string FormatearMonto(object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "$ 0,00";
            }

            string texto = valor.ToString().Trim();
            decimal monto = 0m;

            if (!decimal.TryParse(texto,NumberStyles.Any,CultureInfo.InvariantCulture,out monto))
            {
                decimal.TryParse(texto,NumberStyles.Any,new CultureInfo("es-EC"),out monto);
            }

            return "$ " + monto.ToString("N2",new CultureInfo("es-EC"));
        }

        protected bool EsPdf(object extension)
        {
            if (extension == null || extension == DBNull.Value)
                return false;

            string ext = extension.ToString().Trim().ToLower();
            return ext == ".pdf" || ext == "pdf";
        }

        protected string MostrarTipoArchivo(object extension)
        {
            if (extension == null || extension == DBNull.Value)
                return "";

            string ext = extension.ToString().Trim().ToLower().Replace(".", "");

            if (ext == "pdf")return "PDF";
            if (ext == "xls" || ext == "xlsx")return "EXCEL";
            return ext.ToUpper();
        }

        protected void RptPacientes_ItemDataBound(object sender,RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item &&
                    e.Item.ItemType != ListItemType.AlternatingItem)
                {
                    return;
                }

                DataRowView fila = e.Item.DataItem as DataRowView;

                if (fila == null)
                {
                    return;
                }

                int codigoPERS = 0;

                int.TryParse(fila["PERS_CODIGO"].ToString(),out codigoPERS);

                if (codigoPERS <= 0)
                {
                    return;
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);

                // =====================================================
                // 1. CARGAR RESULTADOS ADJUNTOS
                // TIPO 20
                // =====================================================

                Repeater rptResultados = e.Item.FindControl("RptResultados") as Repeater;
                Label lblSinResultados = e.Item.FindControl("LblSinResultados") as Label;

                object[] parametrosResultados = CrearParametrosOperacionSolicitud(20,codigoEXSO);

                // @in_auxi1 = PERS_CODIGO
                parametrosResultados[36] = codigoPERS;
                DataSet dsResultados = new Conexion(2, "").FunInsertSolictudExamen(parametrosResultados);

                // TIPO 20:
                // TABLA 0 = CABECERA RESULTADO
                // TABLA 1 = DOCUMENTOS
                if (dsResultados != null && dsResultados.Tables.Count >= 2 && dsResultados.Tables[1].Rows.Count > 0)
                {
                    if (rptResultados != null)
                    {
                        rptResultados.DataSource = dsResultados.Tables[1];
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
                        rptResultados.DataSource =
                            null;

                        rptResultados.DataBind();
                    }


                    if (lblSinResultados != null)
                    {
                        lblSinResultados.Visible =
                            true;
                    }
                }

                // =====================================================
                // 2. CONSULTAR AUDITORIA DEL PACIENTE
                // TIPO 22
                // =====================================================

                object[] parametrosAuditoria = CrearParametrosOperacionSolicitud(22,codigoEXSO);

                // @in_auxi1 = PERS_CODIGO
                parametrosAuditoria[36] = codigoPERS;

                DataSet dsAuditoria = new Conexion(2, "").FunInsertSolictudExamen(parametrosAuditoria);

                // =====================================================
                // 3. CONTROLES DE AUDITORIA
                // =====================================================

                RadioButton rdbAceptado =
                    e.Item.FindControl(
                        "RdbAceptado"
                    ) as RadioButton;

                RadioButton rdbRechazado =
                    e.Item.FindControl(
                        "RdbRechazado"
                    ) as RadioButton;


                AjaxControlToolkit.HTMLEditor.Editor txtObservacion =
                    e.Item.FindControl(
                        "TxtObservacionAuditor"
                    ) as AjaxControlToolkit.HTMLEditor.Editor;


                Label lblEstado =
                    e.Item.FindControl(
                        "LblEstadoAuditoria"
                    ) as Label;

                Label lblAdjunto =
                    e.Item.FindControl(
                        "LblAdjuntoAuditor"
                    ) as Label;

                Button btnGuardar =
                    e.Item.FindControl(
                        "BtnGuardarAuditoria"
                    ) as Button;


                // =====================================================
                // FILEUPLOAD NECESITA POSTBACK COMPLETO
                // =====================================================

                ScriptManager sm = ScriptManager.GetCurrent(Page);


                if (sm != null && btnGuardar != null)
                {
                    sm.RegisterPostBackControl(btnGuardar);
                }

                // =====================================================
                // 4. CARGAR AUDITORIA EXISTENTE
                // =====================================================

                if (dsAuditoria != null && dsAuditoria.Tables.Count > 0 && dsAuditoria.Tables[0].Rows.Count > 0)
                {

                    DataRow auditoria = dsAuditoria.Tables[0].Rows[0];
                    string estadoAuditoria = auditoria["EXRA_ESTADO"].ToString().Trim().ToUpper();

                    if (rdbAceptado != null)
                    {
                        rdbAceptado.Checked = estadoAuditoria == "ACEPTADO";
                    }

                    if (rdbRechazado != null)
                    {
                        rdbRechazado.Checked = estadoAuditoria == "RECHAZADO";
                    }

                    string informeHtml = auditoria["EXRA_OBSERVACION"] != DBNull.Value ? auditoria["EXRA_OBSERVACION"].ToString() : "";
                    Label lblInforme = e.Item.FindControl("LblResumenObservacion") as Label;

                    if (txtObservacion != null)
                    {
                        txtObservacion.Content = auditoria["EXRA_OBSERVACION"].ToString();
                    }

                    if (lblInforme != null)
                    {
                        if (string.IsNullOrWhiteSpace(informeHtml))
                        {
                            lblInforme.Text = "Sin informe";
                            lblInforme.ForeColor = System.Drawing.Color.Gray;
                        }
                        else
                        {
                            lblInforme.Text = "Informe registrado";
                            lblInforme.ForeColor = System.Drawing.Color.Green;
                        }
                    }

                    if (lblEstado != null)
                    {
                        lblEstado.Text = estadoAuditoria;

                        if (estadoAuditoria == "ACEPTADO")
                        {
                            lblEstado.ForeColor = System.Drawing.Color.Green;
                        }
                        else if (
                            estadoAuditoria == "RECHAZADO"
                        )
                        {
                            lblEstado.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblEstado.ForeColor =System.Drawing.Color.Gray;
                        }
                    }

                    if (lblAdjunto != null)
                    {
                        string nombreAdjunto = auditoria["EXRA_NOMBRE_DOC"].ToString().Trim();

                        lblAdjunto.Text = !string.IsNullOrEmpty(nombreAdjunto) ? "Archivo actual: " + nombreAdjunto : "Sin archivo adjunto.";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected string ObtenerUrlResultado(object codigoEXRD)
        {
            string codigoEXSO = ViewState["CodigoEXSO"] != null ? ViewState["CodigoEXSO"].ToString() : "0";
            return ResolveUrl("~/Examenes/VerResultadoExamen.ashx?exso=" + HttpUtility.UrlEncode(codigoEXSO) + "&exrd=" + HttpUtility.UrlEncode(codigoEXRD.ToString()));
        }

        protected string ClaseAcordeonPaciente(object codigoPERS)
        {
            if (ViewState["PacienteAuditoriaAbrir"] == null)
            {
                return "panel-collapse collapse";
            }

            string pacienteAbrir = ViewState["PacienteAuditoriaAbrir"].ToString();
            string codigoPaciente = codigoPERS != null ? codigoPERS.ToString() : "";

            if (pacienteAbrir == codigoPaciente)
            {
                return "panel-collapse collapse in";
            }

            return "panel-collapse collapse";
        }

        protected void BtnGuardarAuditoria_Command(object sender,CommandEventArgs e)
        {
            try
            {
                int codigoPERS = 0;

                if (!int.TryParse(e.CommandArgument.ToString(),out codigoPERS) || codigoPERS <= 0)
                {
                    Lblerror.Text = "No se pudo identificar al paciente.";
                    return;
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);
                Button boton = sender as Button;


                if (boton == null)
                {
                    Lblerror.Text = "No se pudo identificar el botón de auditoría.";
                    return;
                }

                RepeaterItem item = boton.NamingContainer as RepeaterItem;

                if (item == null)
                {
                    Lblerror.Text = "No se pudo identificar el registro del paciente.";
                    return;
                }

                RadioButton rdbAceptado = item.FindControl("RdbAceptado") as RadioButton;
                RadioButton rdbRechazado = item.FindControl("RdbRechazado") as RadioButton;
                //TextBox txtObservacion = item.FindControl("TxtObservacionAuditor") as TextBox;
                string claveObservacion = "ObservacionAuditoria_" + codigoPERS.ToString();

                string observacionHtml = "";

                if (ViewState[claveObservacion] != null)
                {
                    observacionHtml = ViewState[claveObservacion].ToString();
                }

                List<ImagenObservacionAuditoria>imagenesObservacion = ExtraerImagenesObservacion(ref observacionHtml);

                FileUpload fupAdjunto =item.FindControl("FupAdjuntoAuditor") as FileUpload;

                string estado = "";

                if (rdbAceptado != null && rdbAceptado.Checked)
                {
                    estado = "ACEPTADO";
                }

                if (rdbRechazado != null && rdbRechazado.Checked)
                {
                    estado = "RECHAZADO";
                }

                if (string.IsNullOrEmpty(estado))
                {
                    new Funciones().funShowJSMessage("Seleccione ACEPTADO o RECHAZADO.",this);
                    return;
                }

                //string observacion = txtObservacion != null ? txtObservacion.Text.Trim().ToUpper() : "";

                string observacion = ViewState[claveObservacion] != null ? ViewState[claveObservacion].ToString() : "";
                byte[] archivo = new byte[0];
                string nombreArchivo = "";
                string extension = "";
                string tipoArchivo = "";

                if (fupAdjunto != null && fupAdjunto.HasFile)
                {
                    nombreArchivo = System.IO.Path.GetFileName(fupAdjunto.PostedFile.FileName);

                    if (nombreArchivo.Length > 150)
                    {
                        new Funciones().funShowJSMessage("El nombre del archivo no puede superar 150 caracteres.",this);
                        return;
                    }

                    extension = System.IO.Path.GetExtension(nombreArchivo).ToLower();

                    switch (extension)
                    {
                        case ".pdf":

                            tipoArchivo = "application/pdf";
                            break;

                        case ".xls":

                            tipoArchivo = "application/vnd.ms-excel";
                            break;

                        case ".xlsx":

                            tipoArchivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;

                        default:

                            new Funciones().funShowJSMessage("El archivo adjunto debe ser PDF, XLS o XLSX.",this);
                            return;
                    }

                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(fupAdjunto.PostedFile.InputStream))
                    {
                        archivo = br.ReadBytes(fupAdjunto.PostedFile.ContentLength);
                    }
                }

                object[] parametros = CrearParametrosOperacionSolicitud(23,codigoEXSO);

                parametros[20] = observacion;
                parametros[21] = archivo;
                parametros[22] = nombreArchivo;
                parametros[23] = tipoArchivo;
                parametros[24] = extension;
                parametros[29] = codigoEXSO;
                parametros[31] = estado;
                parametros[36] = codigoPERS;

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    Lblerror.Text = "No fue posible guardar la auditoría.";
                    return;
                }

                string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();
                int codigoEXRA = Convert.ToInt32(ds.Tables[0].Rows[0]["CodigoEXRA"]);

                foreach (ImagenObservacionAuditoria imagen in imagenesObservacion)
                {

                    object[] parametrosImagen = CrearParametrosOperacionSolicitud(24,codigoEXSO);

                    parametrosImagen[21] = imagen.Bytes;
                    parametrosImagen[22] = imagen.Nombre;
                    parametrosImagen[23] = imagen.Mime;
                    parametrosImagen[24] = imagen.Extension;
                    parametrosImagen[29] = codigoEXSO;
                    parametrosImagen[36] = codigoEXRA;
                    parametrosImagen[37] = imagen.Orden;

                    DataSet dsImagen = new Conexion(2, "").FunInsertSolictudExamen(parametrosImagen);

                    if (dsImagen == null || dsImagen.Tables.Count == 0 || dsImagen.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception("No fue posible guardar la imagen " + imagen.Orden.ToString() + " de la observación.");
                    }

                    string resultadoImagen = dsImagen.Tables[0].Rows[0]["Resultado"].ToString().Trim();

                    if (resultadoImagen != "OK-IMAGEN")
                    {
                        throw new Exception("No fue posible guardar la imagen " + imagen.Orden.ToString() + ".");
                    }


                    int codigoEXAI =
                        Convert.ToInt32(
                            dsImagen.Tables[0]
                                    .Rows[0]["CodigoEXAI"]
                        );


                    string urlImagen =
                        ResolveUrl(
                            "~/Examenes/VerImagenAuditoria.ashx" +
                            "?exso=" +
                            codigoEXSO.ToString() +
                            "&id=" +
                            codigoEXAI.ToString()
                        );


                    observacionHtml =
                        observacionHtml.Replace(
                            imagen.Token,
                            urlImagen
                        );
                }

                object[] parametrosObservacion =CrearParametrosOperacionSolicitud(25,codigoEXSO);

                parametrosObservacion[20] = observacionHtml;
                parametrosObservacion[29] = codigoEXSO;
                parametrosObservacion[36] = codigoEXRA;

                DataSet dsObservacion = new Conexion(2, "").FunInsertSolictudExamen(parametrosObservacion);


                if (dsObservacion == null || dsObservacion.Tables.Count == 0 || dsObservacion.Tables[0].Rows.Count == 0 || dsObservacion.Tables[0].Rows[0]["Resultado"].ToString().Trim() != "OK-OBSERVACION")
                {
                    throw new Exception("La auditoría fue guardada, pero no se pudo actualizar la observación.");
                }

                if (resultado != "OK-AUDITORIA")
                {
                    Lblerror.Text = "No fue posible guardar la auditoría. " + resultado;
                    return;
                }

                string estadoSolicitud = ds.Tables[0].Rows[0]["EstadoSolicitud"].ToString().Trim();
                ViewState[claveObservacion] = observacionHtml;
                ViewState["PacienteAuditoriaAbrir"] = codigoPERS.ToString();
                FunCargaMantenimiento();

                new Funciones().funShowJSMessage("Auditoría guardada correctamente. Estado de la solicitud: " + estadoSolicitud,this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnEditarObservacion_Command(object sender,CommandEventArgs e)
        {
            try
            {
                int codigoPERS = 0;

                if (!int.TryParse(
                        e.CommandArgument.ToString(),
                        out codigoPERS) ||
                    codigoPERS <= 0)
                {
                    Lblerror.Text =
                        "No se pudo identificar al paciente.";

                    return;
                }


                int codigoEXSO =
                    Convert.ToInt32(
                        ViewState["CodigoEXSO"]
                    );


                // Guardamos qué persona estamos editando
                ViewState["PacienteEditorPERS"] =
                    codigoPERS;


                // ==========================================
                // BUSCAR NOMBRE DEL PACIENTE
                // ==========================================

                string nombrePaciente = "";


                Button boton =
                    sender as Button;


                if (boton != null)
                {
                    RepeaterItem item =
                        boton.NamingContainer
                        as RepeaterItem;

                    if (item != null &&
                        item.DataItem != null)
                    {
                        // No siempre estará disponible
                        // porque depende del ciclo del Repeater.
                    }
                }


                // ==========================================
                // OBTENER OBSERVACION
                // ==========================================

                string clave =
                    "ObservacionAuditoria_" +
                    codigoPERS.ToString();


                string observacion = "";


                /*
                 * Si ya la editamos durante esta sesión
                 * usamos lo que está en ViewState.
                 */
                if (ViewState[clave] != null)
                {
                    observacion =
                        ViewState[clave]
                        .ToString();
                }
                else
                {
                    // Si todavía no está en memoria,
                    // consultamos lo guardado en BD - TIPO 22

                    object[] parametros =
                        CrearParametrosOperacionSolicitud(
                            22,
                            codigoEXSO
                        );


                    // @in_auxi1 = PERS_CODIGO
                    parametros[36] =
                        codigoPERS;


                    DataSet ds =
                        new Conexion(2, "")
                            .FunInsertSolictudExamen(
                                parametros
                            );


                    if (ds != null &&
                        ds.Tables.Count > 0 &&
                        ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow fila =
                            ds.Tables[0].Rows[0];


                        observacion =
                            fila["EXRA_OBSERVACION"]
                            .ToString();


                        ViewState[clave] =
                            observacion;
                    }
                }


                EditorObservacionAuditor.Content =
                    observacion;


                // ==========================================
                // OBTENER NOMBRE DESDE EL REPEATER
                // ==========================================

                foreach (RepeaterItem item
                         in RptPacientes.Items)
                {
                    HiddenField hfPers =
                        item.FindControl(
                            "HfCodigoPERS"
                        ) as HiddenField;


                    if (hfPers == null)
                        continue;


                    int persFila = 0;

                    int.TryParse(
                        hfPers.Value,
                        out persFila
                    );


                    if (persFila ==
                        codigoPERS)
                    {
                        HiddenField hfNombre =
                            item.FindControl(
                                "HfNombrePaciente"
                            ) as HiddenField;


                        if (hfNombre != null)
                        {
                            nombrePaciente =
                                hfNombre.Value;
                        }

                        break;
                    }
                }


                LblPacienteEditor.Text =
                    nombrePaciente;


                PnlEditorAuditoria.Visible =
                    true;


                // Mantener abierto el acordeón
                ViewState["PacienteAuditoriaAbrir"] =
                    codigoPERS.ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }

        protected void BtnAceptarObservacion_Click(object sender,EventArgs e)
        {
            try
            {
                if (ViewState["PacienteEditorPERS"] == null)
                {
                    Lblerror.Text =
                        "No se pudo identificar al paciente.";

                    return;
                }


                int codigoPERS =
                    Convert.ToInt32(
                        ViewState["PacienteEditorPERS"]
                    );


                string contenido =
                    EditorObservacionAuditor.Content != null
                        ? EditorObservacionAuditor.Content.Trim()
                        : "";


                string clave =
                    "ObservacionAuditoria_" +
                    codigoPERS.ToString();


                // Aquí guardamos TEMPORALMENTE el HTML
                // para esta persona únicamente.
                ViewState[clave] =
                    contenido;


                // Mantener abierto su acordeón
                ViewState["PacienteAuditoriaAbrir"] =
                    codigoPERS.ToString();


                PnlEditorAuditoria.Visible =
                    false;


                EditorObservacionAuditor.Content =
                    "";


                ViewState["PacienteEditorPERS"] =
                    null;


                LblPacienteEditor.Text =
                    "";
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }

        protected void BtnCerrarEditor_Click(object sender,EventArgs e)
        {
            try
            {
                int codigoPERS = 0;


                if (ViewState["PacienteEditorPERS"] != null)
                {
                    int.TryParse(
                        ViewState["PacienteEditorPERS"]
                            .ToString(),
                        out codigoPERS
                    );
                }


                if (codigoPERS > 0)
                {
                    ViewState["PacienteAuditoriaAbrir"] =
                        codigoPERS.ToString();
                }


                /*
                 * Cerrar NO modifica la observación.
                 * Se descarta lo que estaba escribiendo
                 * si no presionó Aceptar observación.
                 */
                EditorObservacionAuditor.Content =
                    "";


                PnlEditorAuditoria.Visible =
                    false;


                ViewState["PacienteEditorPERS"] =
                    null;


                LblPacienteEditor.Text =
                    "";
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }


        protected void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(0);
        }

        protected void ImgDownload1_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(1);
        }

        protected void ImgDownload2_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(2);
        }

        protected void ImgDownload3_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(3);
        }

        protected void ImgDownload4_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(4);
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmAuditarExamenAdmin.aspx", true);
        }

        private class ImagenObservacionAuditoria
        {
            public int Orden { get; set; }

            public string Token { get; set; }

            public string Mime { get; set; }

            public string Extension { get; set; }

            public string Nombre { get; set; }

            public byte[] Bytes { get; set; }
        }

        private List<ImagenObservacionAuditoria>ExtraerImagenesObservacion(ref string html)
        {
            List<ImagenObservacionAuditoria> imagenes = new List<ImagenObservacionAuditoria>();

            if (string.IsNullOrEmpty(html))
            {
                return imagenes;
            }

            int orden = 0;


            string patron =
                @"src\s*=\s*[""']data:(?<mime>image\/(?:png|jpeg|jpg|gif));base64,(?<data>[^""']+)[""']";


            html = Regex.Replace(
                html,
                patron,
                delegate (Match match)
                {
                    orden++;


                    string mime =
                        match.Groups["mime"]
                             .Value
                             .ToLower();


                    string base64 =
                        match.Groups["data"]
                             .Value;


                    byte[] bytes;

                    try
                    {
                        bytes =
                            Convert.FromBase64String(base64);
                    }
                    catch
                    {
                        throw new Exception(
                            "No fue posible procesar la imagen " +
                            orden.ToString() +
                            " de la observación."
                        );
                    }


                    string extension = ".png";


                    if (mime == "image/jpeg" ||
                        mime == "image/jpg")
                    {
                        extension = ".jpg";
                    }
                    else if (mime == "image/gif")
                    {
                        extension = ".gif";
                    }


                    string token =
                        "__AUDITORIA_IMAGEN_" +
                        orden.ToString() +
                        "__";


                    ImagenObservacionAuditoria imagen =
                        new ImagenObservacionAuditoria();

                    imagen.Orden = orden;
                    imagen.Token = token;
                    imagen.Mime = mime;
                    imagen.Extension = extension;

                    imagen.Nombre =
                        "imagen_auditoria_" +
                        orden.ToString() +
                        extension;

                    imagen.Bytes = bytes;


                    imagenes.Add(imagen);


                    return "src=\"" + token + "\"";
                },
                RegexOptions.IgnoreCase
            );


            return imagenes;
        }


        #endregion
    }
}