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

        protected void Page_Load(object sender,EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                {
                    Response.Redirect("~/Reload.html");
                    return;
                }

                Page.Form.Attributes.Add("enctype","multipart/form-data");

                if (!IsPostBack)
                {
                    int codigoEXSO = 0;

                    if (!int.TryParse(Request["CodigoEXSO"],out codigoEXSO))
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


        protected void RptPacientes_ItemDataBound(object sender,RepeaterItemEventArgs e)
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

                int.TryParse(fila["PERS_CODIGO"].ToString(),out codigoPERS);


                if (codigoPERS <= 0)
                {
                    return;
                }


                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);


                Repeater rptResultados =
                    e.Item.FindControl(
                        "RptResultados"
                    ) as Repeater;


                Label lblSinResultados =
                    e.Item.FindControl(
                        "LblSinResultados"
                    ) as Label;


                object[] parametrosResultado =
                    CrearParametrosOperacionSolicitud(
                        20,
                        codigoEXSO
                    );


                parametrosResultado[36] = codigoPERS;


                DataSet dsResultado =
                    new Conexion(2, "")
                        .FunInsertSolictudExamen(
                            parametrosResultado
                        );


                if (dsResultado != null && dsResultado.Tables.Count >= 2 &&
                    dsResultado.Tables[1].Rows.Count > 0)
                {
                    if (rptResultados != null)
                    {
                        rptResultados.DataSource =
                            dsResultado.Tables[1];

                        rptResultados.DataBind();
                    }


                    if (lblSinResultados != null)
                    {
                        lblSinResultados.Visible =
                            false;
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


                // ==========================================
                // AUDITORIA - TIPO 22
                // ==========================================

                object[] parametrosAuditoria =
                    CrearParametrosOperacionSolicitud(
                        22,
                        codigoEXSO
                    );


                parametrosAuditoria[36] =
                    codigoPERS;


                DataSet dsAuditoria =
                    new Conexion(2, "")
                        .FunInsertSolictudExamen(
                            parametrosAuditoria
                        );


                Label lblEstado =
                    e.Item.FindControl(
                        "LblEstadoAuditoria"
                    ) as Label;


                Label lblEstadoDetalle =
                    e.Item.FindControl(
                        "LblEstadoAuditoriaDetalle"
                    ) as Label;


                Literal litInforme =
                    e.Item.FindControl(
                        "LitInformeAuditor"
                    ) as Literal;


                Label lblAdjunto =
                    e.Item.FindControl(
                        "LblAdjuntoAuditor"
                    ) as Label;


                if (dsAuditoria != null &&
                    dsAuditoria.Tables.Count > 0 &&
                    dsAuditoria.Tables[0].Rows.Count > 0)
                {
                    DataRow auditoria =
                        dsAuditoria.Tables[0]
                            .Rows[0];


                    string estado =
                        auditoria["EXRA_ESTADO"]
                            .ToString()
                            .Trim()
                            .ToUpper();


                    string informe =
                        auditoria[
                            "EXRA_OBSERVACION"
                        ] != DBNull.Value
                        ? auditoria[
                            "EXRA_OBSERVACION"
                          ].ToString()
                        : "";


                    string adjunto =
                        auditoria[
                            "EXRA_NOMBRE_DOC"
                        ].ToString()
                         .Trim();


                    if (lblEstado != null)
                    {
                        lblEstado.Text =
                            estado;


                        if (estado == "ACEPTADO")
                        {
                            lblEstado.CssClass =
                                "pull-right estado-aceptado";
                        }
                        else if (
                            estado == "RECHAZADO")
                        {
                            lblEstado.CssClass =
                                "pull-right estado-rechazado";
                        }
                        else
                        {
                            lblEstado.CssClass =
                                "pull-right estado-pendiente";
                        }
                    }


                    if (lblEstadoDetalle != null)
                    {
                        lblEstadoDetalle.Text =
                            estado;


                        if (estado == "ACEPTADO")
                        {
                            lblEstadoDetalle.ForeColor =
                                System.Drawing.Color.Green;
                        }
                        else if (
                            estado == "RECHAZADO")
                        {
                            lblEstadoDetalle.ForeColor =
                                System.Drawing.Color.Red;
                        }
                    }


                    if (litInforme != null)
                    {
                        if (string.IsNullOrWhiteSpace(
                                informe))
                        {
                            litInforme.Text =
                                "<span style='color:gray;'>Sin informe registrado.</span>";
                        }
                        else
                        {
                            litInforme.Text =
                                informe;
                        }
                    }


                    if (lblAdjunto != null)
                    {
                        lblAdjunto.Text =
                            string.IsNullOrEmpty(adjunto)
                            ? "Sin archivo adjunto."
                            : adjunto;
                    }
                }

                // ==========================================
                // GESTIÓN CLIENTE
                // TIPO 30
                // ==========================================

                object[] parametrosCliente =
                    CrearParametrosOperacionSolicitud(
                        30,
                        codigoEXSO
                    );


                parametrosCliente[36] =
                    codigoPERS;


                DataSet dsCliente =
                    new Conexion(2, "")
                        .FunInsertSolictudExamen(
                            parametrosCliente
                        );


                RadioButton rdbClienteAceptado =
                    e.Item.FindControl(
                        "RdbClienteAceptado"
                    ) as RadioButton;


                RadioButton rdbClienteRechazado =
                    e.Item.FindControl(
                        "RdbClienteRechazado"
                    ) as RadioButton;


                Label lblEstadoCliente =
                    e.Item.FindControl(
                        "LblEstadoCliente"
                    ) as Label;


                Label lblResumenCliente =
                    e.Item.FindControl(
                        "LblResumenInformeCliente"
                    ) as Label;


                Label lblAdjuntoCliente =
                    e.Item.FindControl(
                        "LblAdjuntoCliente"
                    ) as Label;


                GridView historial =
                    e.Item.FindControl(
                        "GrdvHistorialCliente"
                    ) as GridView;


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


                    string informeCliente =
                        gestion["EXRC_OBSERVACION"]
                            != DBNull.Value
                        ? gestion[
                            "EXRC_OBSERVACION"
                          ].ToString()
                        : "";


                    ViewState["InformeCliente_" + codigoPERS.ToString()] = informeCliente;

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
                        lblAdjuntoCliente.Text = string.IsNullOrEmpty(adjuntoCliente)
                            ? "Sin archivo adjunto."
                            : "Archivo actual: " +
                              adjuntoCliente;
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
                        historial.DataSource =null;
                    }

                    historial.DataBind();
                }


                // ==========================================
                // FILEUPLOAD CLIENTE
                // NECESITA POSTBACK COMPLETO
                // ==========================================

                Button btnGuardarCliente = e.Item.FindControl("BtnGuardarCliente") as Button;

                ScriptManager sm =ScriptManager.GetCurrent(Page);

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
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        #endregion


        #region Informe Cliente

        protected void BtnEditarInformeCliente_Command(
            object sender,
            CommandEventArgs e)
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


                ViewState["PacienteEditorCliente"] =
                    codigoPERS;


                string clave =
                    "InformeCliente_" +
                    codigoPERS.ToString();


                if (ViewState[clave] != null)
                {
                    EditorInformeCliente.Content =
                        ViewState[clave].ToString();
                }
                else
                {
                    EditorInformeCliente.Content =
                        "";
                }


                LblPacienteEditorCliente.Text =
                    "Paciente código " +
                    codigoPERS.ToString();


                PnlEditorCliente.Visible =
                    true;
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }


        protected void BtnAceptarInformeCliente_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                if (ViewState[
                        "PacienteEditorCliente"
                    ] == null)
                {
                    Lblerror.Text =
                        "No se pudo identificar al paciente.";

                    return;
                }


                int codigoPERS =
                    Convert.ToInt32(
                        ViewState[
                            "PacienteEditorCliente"
                        ]
                    );


                string clave =
                    "InformeCliente_" +
                    codigoPERS.ToString();


                ViewState[clave] =
                    EditorInformeCliente.Content;


                PnlEditorCliente.Visible =
                    false;


                CargarSolicitud();
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }


        protected void BtnCerrarEditorCliente_Click(
            object sender,
            EventArgs e)
        {
            PnlEditorCliente.Visible =
                false;
        }

        #endregion


        #region Guardar Cliente

        protected void BtnGuardarCliente_Command(object sender,CommandEventArgs e)
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


                Button boton =
                    sender as Button;


                if (boton == null)
                {
                    return;
                }


                RepeaterItem item =
                    boton.NamingContainer
                    as RepeaterItem;


                if (item == null)
                {
                    return;
                }


                RadioButton aceptado =
                    item.FindControl(
                        "RdbClienteAceptado"
                    ) as RadioButton;


                RadioButton rechazado =
                    item.FindControl(
                        "RdbClienteRechazado"
                    ) as RadioButton;


                FileUpload adjunto =
                    item.FindControl(
                        "FupAdjuntoCliente"
                    ) as FileUpload;


                string estado = "";


                if (aceptado != null &&
                    aceptado.Checked)
                {
                    estado = "ACEPTADO";
                }


                if (rechazado != null &&
                    rechazado.Checked)
                {
                    estado = "RECHAZADO";
                }


                if (string.IsNullOrEmpty(estado))
                {
                    new Funciones().funShowJSMessage(
                        "Seleccione ACEPTADO o RECHAZADO.",
                        this
                    );

                    return;
                }


                string claveInforme =
                    "InformeCliente_" +
                    codigoPERS.ToString();


                string informeHtml = "";


                if (ViewState[claveInforme] != null)
                {
                    informeHtml =
                        ViewState[claveInforme]
                            .ToString();
                }


                List<ImagenInformeCliente> imagenes =
                    ExtraerImagenesCliente(
                        ref informeHtml
                    );


                byte[] archivo =
                    new byte[0];

                string nombreArchivo = "";
                string extension = "";
                string tipoArchivo = "";


                if (adjunto != null &&
                    adjunto.HasFile)
                {
                    nombreArchivo =
                        System.IO.Path.GetFileName(
                            adjunto.PostedFile.FileName
                        );


                    if (nombreArchivo.Length > 100)
                    {
                        new Funciones().funShowJSMessage(
                            "El nombre del archivo no puede superar 100 caracteres.",
                            this
                        );

                        return;
                    }


                    extension =
                        System.IO.Path.GetExtension(
                            nombreArchivo
                        ).ToLower();


                    switch (extension)
                    {
                        case ".pdf":

                            tipoArchivo =
                                "application/pdf";

                            break;


                        case ".xls":

                            tipoArchivo =
                                "application/vnd.ms-excel";

                            break;


                        case ".xlsx":

                            tipoArchivo =
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                            break;


                        default:

                            new Funciones().funShowJSMessage(
                                "El archivo adjunto debe ser PDF, XLS o XLSX.",
                                this
                            );

                            return;
                    }


                    using (
                        System.IO.BinaryReader br =
                            new System.IO.BinaryReader(
                                adjunto.PostedFile.InputStream
                            )
                    )
                    {
                        archivo =
                            br.ReadBytes(
                                adjunto.PostedFile.ContentLength
                            );
                    }
                }


                // =====================================
                // GUARDAR GESTIÓN
                // TIPO 27
                // =====================================

                object[] parametros =
                    CrearParametrosOperacionSolicitud(
                        27,
                        codigoEXSO
                    );


                parametros[20] =
                    informeHtml;

                parametros[21] =
                    archivo;

                parametros[22] =
                    nombreArchivo;

                parametros[23] =
                    tipoArchivo;

                parametros[24] =
                    extension;

                parametros[29] =
                    codigoEXSO;

                parametros[31] =
                    estado;

                parametros[36] =
                    codigoPERS;


                DataSet ds =
                    new Conexion(2, "")
                        .FunInsertSolictudExamen(
                            parametros
                        );


                if (ds == null ||
                    ds.Tables.Count == 0 ||
                    ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception(
                        "No fue posible guardar la gestión del cliente."
                    );
                }


                string resultado =
                    ds.Tables[0]
                      .Rows[0]["Resultado"]
                      .ToString()
                      .Trim();


                if (resultado != "OK-CLIENTE")
                {
                    throw new Exception(
                        "No fue posible guardar la gestión. " +
                        resultado
                    );
                }


                int codigoEXRC =
                    Convert.ToInt32(
                        ds.Tables[0]
                          .Rows[0]["CodigoEXRC"]
                    );


                // =====================================
                // GUARDAR IMÁGENES DEL INFORME
                // TIPO 28
                // =====================================

                foreach (
                    ImagenInformeCliente imagen
                    in imagenes)
                {
                    object[] parametrosImagen =
                        CrearParametrosOperacionSolicitud(
                            28,
                            codigoEXSO
                        );


                    parametrosImagen[21] =
                        imagen.Bytes;

                    parametrosImagen[22] =
                        imagen.Nombre;

                    parametrosImagen[23] =
                        imagen.Mime;

                    parametrosImagen[24] =
                        imagen.Extension;

                    parametrosImagen[29] =
                        codigoEXSO;

                    // @in_auxi1 = EXRC
                    parametrosImagen[36] =
                        codigoEXRC;

                    // @in_auxi2 = ORDEN
                    parametrosImagen[37] =
                        imagen.Orden;


                    DataSet dsImagen =
                        new Conexion(2, "")
                            .FunInsertSolictudExamen(
                                parametrosImagen
                            );


                    if (dsImagen == null ||
                        dsImagen.Tables.Count == 0 ||
                        dsImagen.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception(
                            "No fue posible guardar una imagen del informe."
                        );
                    }


                    int codigoEXCI =
                        Convert.ToInt32(
                            dsImagen.Tables[0]
                                    .Rows[0]["CodigoEXCI"]
                        );


                    string urlImagen =
                        ResolveUrl(
                            "~/Examenes/VerImagenCliente.ashx" +
                            "?exso=" +
                            codigoEXSO.ToString() +
                            "&id=" +
                            codigoEXCI.ToString()
                        );


                    informeHtml =
                        informeHtml.Replace(
                            imagen.Token,
                            urlImagen
                        );
                }


                // =====================================
                // GUARDAR HTML FINAL
                // TIPO 29
                // =====================================

                object[] parametrosInforme =
                    CrearParametrosOperacionSolicitud(
                        29,
                        codigoEXSO
                    );


                parametrosInforme[20] =
                    informeHtml;

                parametrosInforme[29] =
                    codigoEXSO;

                parametrosInforme[36] =
                    codigoEXRC;


                DataSet dsInforme =
                    new Conexion(2, "")
                        .FunInsertSolictudExamen(
                            parametrosInforme
                        );


                if (dsInforme == null ||
                    dsInforme.Tables.Count == 0 ||
                    dsInforme.Tables[0].Rows.Count == 0 ||
                    dsInforme.Tables[0]
                             .Rows[0]["Resultado"]
                             .ToString()
                             .Trim()
                        != "OK-INFORME-CLIENTE")
                {
                    throw new Exception(
                        "La gestión fue guardada, pero no fue posible finalizar el informe."
                    );
                }


                ViewState[claveInforme] =
                    informeHtml;


                CargarSolicitud();


                new Funciones().funShowJSMessage(
                    "Gestión del cliente guardada correctamente.",
                    this
                );
            }
            catch (Exception ex)
            {
                Lblerror.Text =
                    ex.ToString();
            }
        }

        #endregion


        #region Helpers

        private object[] CrearParametrosOperacionSolicitud(
            int tipo,
            int codigoEXSO)
        {
            object[] parametros =
                new object[43];


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


            if (DateTime.TryParse(valor.ToString(),out fecha))
            {
                return fecha.ToString("dd/MM/yyyy");
            }

            return valor.ToString();
        }


        public string FormatearMonto(
            object valor)
        {
            if (valor == null || valor == DBNull.Value)
            {
                return "$ 0,00";
            }


            string texto = valor.ToString().Trim();

            decimal monto = 0m;

            if (!decimal.TryParse(
                    texto,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out monto))
            {
                decimal.TryParse(
                    texto,
                    NumberStyles.Any,
                    new CultureInfo(
                        "es-EC"
                    ),
                    out monto
                );
            }


            return "$ " +
                   monto.ToString(
                       "N2",
                       new CultureInfo(
                           "es-EC"
                       )
                   );
        }


        public bool EsPdf(
            object extension)
        {
            if (extension == null || extension == DBNull.Value)
            {
                return false;
            }


            string ext = extension.ToString().Trim().ToLower();

            return ext == ".pdf" || ext == "pdf";
        }


        public string MostrarTipoArchivo(
            object extension)
        {
            if (extension == null || extension == DBNull.Value)
            {
                return "";
            }


            string ext =
                extension.ToString()
                         .Trim()
                         .ToLower()
                         .Replace(".", "");


            if (ext == "pdf")
                return "PDF";


            if (ext == "xls" ||
                ext == "xlsx")
                return "EXCEL";


            return ext.ToUpper();
        }


        public string ObtenerUrlResultado(object codigoEXRD)
        {
            string codigoEXSO =
                ViewState["CodigoEXSO"] != null
                ? ViewState["CodigoEXSO"]
                    .ToString()
                : "0";

            return ResolveUrl(
                "~/Examenes/VerResultadoExamen.ashx" +
                "?exso=" +
                HttpUtility.UrlEncode(
                    codigoEXSO
                ) +
                "&exrd=" +
                HttpUtility.UrlEncode(
                    codigoEXRD.ToString()
                )
            );
        }


        private DataTable CrearTablaHistorialVacia()
        {
            DataTable tabla =
                new DataTable();


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
        private List<ImagenInformeCliente>ExtraerImagenesCliente(ref string html)
        {
            List<ImagenInformeCliente> imagenes = new List<ImagenInformeCliente>();

            if (string.IsNullOrEmpty(html))
            {
                return imagenes;
            }

            int orden = 0;

            string patron =
                @"src\s*=\s*[""']data:(?<mime>image\/(?:png|jpeg|jpg|gif));base64,(?<data>[^""']+)[""']";


            html =
                Regex.Replace(html,patron,

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
                                Convert.FromBase64String(
                                    base64
                                );
                        }
                        catch
                        {
                            throw new Exception(
                                "No fue posible procesar la imagen " +
                                orden.ToString() +
                                " del informe."
                            );
                        }


                        string extension =
                            ".png";


                        if (mime == "image/jpeg" ||
                            mime == "image/jpg")
                        {
                            extension =
                                ".jpg";
                        }
                        else if (
                            mime == "image/gif")
                        {
                            extension =
                                ".gif";
                        }


                        string token =
                            "__CLIENTE_IMAGEN_" +
                            orden.ToString() +
                            "__";


                        ImagenInformeCliente imagen =
                            new ImagenInformeCliente();


                        imagen.Orden =
                            orden;

                        imagen.Token =
                            token;

                        imagen.Mime =
                            mime;

                        imagen.Extension =
                            extension;

                        imagen.Nombre =
                            "imagen_cliente_" +
                            orden.ToString() +
                            extension;

                        imagen.Bytes =
                            bytes;


                        imagenes.Add(
                            imagen
                        );


                        return
                            "src=\"" +
                            token +
                            "\"";
                    },

                    RegexOptions.IgnoreCase
                );


            return imagenes;
        }

        #endregion


        #region Salir

        protected void BtnSalir_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect("../Examenes/FrmNuevoExamenCliente.aspx", true);
        }

        #endregion

    }
}