using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmResultadoExamen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                if (Session["usuCodigo"] == null ||
                    Session["usuCodigo"].ToString() == "")
                {
                    Response.Redirect("~/Reload.html");
                    return;
                }

                if (!IsPostBack)
                {
                    CargarConfiguracionCorreo();
                    int codigoEXSO = 0;

                    if (!int.TryParse(Request["CodigoEXSO"],out codigoEXSO))
                    {
                        Lblerror.Text = "No se recibió una solicitud válida.";
                        return;
                    }

                    ViewState["CodigoEXSO"] = codigoEXSO;
                    CargarSolicitud(codigoEXSO);
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void CargarSolicitud(int codigoEXSO)
        {
            object[] parametros = CrearParametrosOperacionSolicitud(19,codigoEXSO);

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count < 2)
            {
                throw new Exception("No fue posible obtener los datos de la solicitud.");
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No se encontró la solicitud.");
            }

            DataRow cabecera = ds.Tables[0].Rows[0];

            LblSolicitud.Text = cabecera["EXSO_CODIGO"].ToString();
            LblProducto.Text = cabecera["PROD_CODIGO"].ToString();
            LblEstado.Text = cabecera["ESTADO_SOLICITUD"].ToString();

            DateTime fechaSolicitud;

            if (DateTime.TryParse(cabecera["FECHA_SOLICITUD"].ToString(),out fechaSolicitud))
            {
                LblFechaSolicitud.Text = fechaSolicitud.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                LblFechaSolicitud.Text = cabecera["FECHA_SOLICITUD"].ToString();
            }

            GrdvPacientes.DataSource = ds.Tables[1];
            GrdvPacientes.DataBind();

            ConfigurarBotonEnvio(ds.Tables[1]);
        }

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

        protected void BtnSalir_Click(object sender,EventArgs e)
        {
            Response.Redirect("FrmSolicitudOperadorAdmin.aspx",true);
        }

        protected void BtnCargar_Click(object sender, EventArgs e)
        {
            try
            {
                Button boton = sender as Button;

                if (boton == null)
                    return;

                GridViewRow fila = boton.NamingContainer as GridViewRow;

                if (fila == null)
                    return;

                int indice = fila.RowIndex;
                string persCodigo = GrdvPacientes.DataKeys[indice].Values["PERS_CODIGO"].ToString();
                string tituCodigo = GrdvPacientes.DataKeys[indice].Values["TITU_CODIGO"].ToString();
                string tipoPersona = GrdvPacientes.DataKeys[indice].Values["TIPO_PERSONA"].ToString();

                ViewState["ResultadoPersCodigo"] = persCodigo;
                ViewState["ResultadoTituCodigo"] = tituCodigo;
                ViewState["ResultadoTipoPersona"] = tipoPersona;

                LblTipoPersonaSeleccionada.Text = tipoPersona;
                LblDocumentoSeleccionado.Text = fila.Cells[1].Text;
                LblPacienteSeleccionado.Text = Server.HtmlDecode(fila.Cells[2].Text);

                PnlCarga.Visible = true;
                CargarArchivosPaciente(Convert.ToInt32(ViewState["CodigoEXSO"]),Convert.ToInt32(persCodigo));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        protected void BtnCancelarCarga_Click(object sender,EventArgs e)
        {
            PnlCarga.Visible = false;

            ViewState.Remove("ResultadoPersCodigo");
            ViewState.Remove("ResultadoTituCodigo");
            ViewState.Remove("ResultadoTipoPersona");

            TxtObservacion.Text = "";
        }
        protected void BtnGuardarResultados_Click(object sender,EventArgs e)
        {
            try
            {
                Lblerror.Text = "";

                if (ViewState["CodigoEXSO"] == null)
                {
                    new Funciones().funShowJSMessage("No se encontró el código de la solicitud.",this);
                    return;
                }

                if (ViewState["ResultadoPersCodigo"] == null)
                {
                    new Funciones().funShowJSMessage("Seleccione un paciente.",this);
                    return;
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);
                int codigoPERS = Convert.ToInt32(ViewState["ResultadoPersCodigo"]);

                if (!FileResultado1.HasFile && !FileResultado2.HasFile && !FileResultado3.HasFile)
                {
                    new Funciones().funShowJSMessage("Seleccione al menos un archivo de resultados.",this);
                    return;
                }

                ValidarArchivoResultado(FileResultado1);
                ValidarArchivoResultado(FileResultado2);
                ValidarArchivoResultado(FileResultado3);

                int codigoEXRE = GuardarCabeceraResultado(codigoEXSO,codigoPERS,TxtObservacion.Text.Trim());

                if (codigoEXRE <= 0)
                {
                    throw new Exception("No fue posible obtener el código del resultado.");
                }

                if (FileResultado1.HasFile)
                {
                    GuardarDocumentoResultado(codigoEXSO,codigoEXRE,1,FileResultado1);
                }

                if (FileResultado2.HasFile)
                {
                    GuardarDocumentoResultado(codigoEXSO,codigoEXRE,2,FileResultado2);
                }

                if (FileResultado3.HasFile)
                {
                    GuardarDocumentoResultado(codigoEXSO,codigoEXRE,3,FileResultado3);
                }

                PnlCarga.Visible = false;
                ViewState.Remove("ResultadoPersCodigo");
                ViewState.Remove("ResultadoTituCodigo");
                ViewState.Remove("ResultadoTipoPersona");
                TxtObservacion.Text = "";

                CargarSolicitud(codigoEXSO);

                new Funciones().funShowJSMessage("Resultados guardados correctamente.",this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void ValidarArchivoResultado(FileUpload archivo)
        {
            if (archivo == null || !archivo.HasFile)
                return;

            string nombre = Path.GetFileName(archivo.PostedFile.FileName);

            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new Exception("Uno de los archivos seleccionados no tiene un nombre válido.");
            }

            if (nombre.Length > 150)
            {
                throw new Exception("El nombre del archivo no puede superar los 150 caracteres.");
            }

            string extension =Path.GetExtension(nombre).ToLower();

            if (extension != ".pdf" && extension != ".xls" && extension != ".xlsx")
            {
                throw new Exception("Solo se permiten archivos PDF, XLS o XLSX.");
            }

            if (archivo.PostedFile.ContentLength <= 0)
            {
                throw new Exception("El archivo " + nombre + " está vacío.");
            }
        }

        private int GuardarCabeceraResultado(int codigoEXSO,int codigoPERS,string observacion)
        {
            object[] parametros = CrearParametrosOperacionSolicitud(15,codigoEXSO);
            parametros[0] = 15;
            parametros[20] = observacion != null ? observacion.Trim().ToUpper() : "";
            parametros[29] = codigoEXSO;
            parametros[36] = codigoPERS;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No fue posible crear el registro de resultados.");
            }

            DataRow fila = ds.Tables[0].Rows[0];
            string resultado = fila["Resultado"].ToString().Trim();

            if (resultado != "OK-RESULTADO" && resultado != "OK-RESULTADO-ACTUALIZADO")
            {
                throw new Exception("No fue posible registrar el resultado. " + resultado);
            }

            int codigoEXRE = 0;

            if (!int.TryParse(fila["CodigoEXRE"].ToString(),out codigoEXRE))
            {
                codigoEXRE = 0;
            }

            return codigoEXRE;
        }

        private void GuardarDocumentoResultado(int codigoEXSO,int codigoEXRE,int orden,FileUpload archivo)
        {
            string nombreArchivo = Path.GetFileName(archivo.PostedFile.FileName);
            string extension = Path.GetExtension(nombreArchivo).ToLower();
            string mime = ObtenerMimeResultado(extension);
            byte[] documento;

            using (BinaryReader br = new BinaryReader(archivo.PostedFile.InputStream))
            {
                documento = br.ReadBytes(archivo.PostedFile.ContentLength);
            }

            object[] parametros = CrearParametrosOperacionSolicitud(16,codigoEXSO);
            parametros[0] = 16;
            parametros[21] = documento;
            parametros[22] = nombreArchivo;
            parametros[23] = mime;
            parametros[24] = extension;
            parametros[29] = codigoEXSO;
            parametros[36] = codigoEXRE;
            parametros[37] = orden;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("No fue posible guardar el archivo " + nombreArchivo + ".");
            }

            string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();

            if (resultado != "OK-DOCUMENTO")
            {
                throw new Exception("No fue posible guardar el archivo " + nombreArchivo + ". Respuesta: " + resultado);
            }
        }

        private string ObtenerMimeResultado(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";

                case ".xls":
                    return "application/vnd.ms-excel";

                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                default:
                    return "application/octet-stream";
            }
        }
        private void CargarArchivosPaciente(int codigoEXSO,int codigoPERS)
        {
            LimpiarArchivosVisuales();

            object[] parametros = CrearParametrosOperacionSolicitud(20,codigoEXSO);
            parametros[0] = 20;
            parametros[36] = codigoPERS;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null)
            {
                throw new Exception("No fue posible consultar los resultados del paciente.");
            }

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow cabecera = ds.Tables[0].Rows[0];
                ViewState["ResultadoEXRE"] = cabecera["EXRE_CODIGO"].ToString();
                TxtObservacion.Text = cabecera["EXRE_OBSERVACION"].ToString();
            }
            else
            {
                ViewState.Remove("ResultadoEXRE");
                TxtObservacion.Text = "";
            }

            if (ds.Tables.Count < 2)
                return;

            foreach (DataRow fila in ds.Tables[1].Rows)
            {
                int orden = Convert.ToInt32(fila["EXRD_ORDEN"]);
                string nombre = fila["EXRD_NOMBRE"].ToString();

                switch (orden)
                {
                    case 1:

                        LblArchivo1.Text = nombre;
                        LblArchivo1.ForeColor = System.Drawing.Color.Green;
                        BtnEliminarArchivo1.Visible = true;
                        break;

                    case 2:

                        LblArchivo2.Text = nombre;
                        LblArchivo2.ForeColor = System.Drawing.Color.Green;
                        BtnEliminarArchivo2.Visible = true;
                        break;

                    case 3:

                        LblArchivo3.Text = nombre;
                        LblArchivo3.ForeColor = System.Drawing.Color.Green;
                        BtnEliminarArchivo3.Visible = true;
                        break;
                }
            }
        }

        private void LimpiarArchivosVisuales()
        {
            LblArchivo1.Text = "Sin archivo";
            LblArchivo2.Text = "Sin archivo";
            LblArchivo3.Text = "Sin archivo";

            LblArchivo1.ForeColor = System.Drawing.Color.Gray;
            LblArchivo2.ForeColor = System.Drawing.Color.Gray;
            LblArchivo3.ForeColor = System.Drawing.Color.Gray;

            BtnEliminarArchivo1.Visible = false;
            BtnEliminarArchivo2.Visible = false;
            BtnEliminarArchivo3.Visible = false;
        }

        protected void BtnEliminarArchivo_Click(object sender,EventArgs e)
        {
            try
            {
                LinkButton boton = sender as LinkButton;

                if (boton == null)
                    return;

                if (ViewState["CodigoEXSO"] == null || ViewState["ResultadoPersCodigo"] == null ||
                    ViewState["ResultadoEXRE"] == null)
                {
                    new Funciones().funShowJSMessage("No se encontró el resultado seleccionado.",this);
                    return;
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);
                int codigoPERS = Convert.ToInt32(ViewState["ResultadoPersCodigo"]);
                int codigoEXRE = Convert.ToInt32(ViewState["ResultadoEXRE"]);
                int orden = Convert.ToInt32(boton.CommandArgument);

                object[] parametros = CrearParametrosOperacionSolicitud(17,codigoEXSO);
                parametros[0] = 17;
                parametros[36] = codigoEXRE;
                parametros[37] = orden;

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    throw new Exception("No fue posible eliminar el documento.");
                }

                string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();

                if (resultado != "OK-DOCUMENTO-ELIMINADO")
                {
                    throw new Exception("No fue posible eliminar el documento. " + resultado);
                }

                CargarArchivosPaciente(codigoEXSO,codigoPERS);
                CargarSolicitud(codigoEXSO);
                PnlCarga.Visible = true;

                new Funciones().funShowJSMessage("Documento eliminado correctamente.",this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void ConfigurarBotonEnvio(DataTable pacientes)
        {

            BtnEnviarResultados.Enabled = false;
            BtnEnviarResultados.CssClass = "btn btn-default";

            if (pacientes == null || pacientes.Rows.Count == 0)
            {
                BtnEnviarResultados.Text = "Sin Pacientes";
                return;
            }

            bool todosCargados = true;
            bool todosEnviados = true;

            foreach (DataRow fila in pacientes.Rows)
            {

                string estadoResultado = fila["ESTADO_RESULTADO"].ToString().Trim().ToUpper();

                if (estadoResultado != "CARGADO")
                {
                    todosCargados = false;
                }

                if (estadoResultado != "ENVIADO")
                {
                    todosEnviados = false;
                }
            }

            if (todosEnviados)
            {
                BtnEnviarResultados.Enabled = false;
                BtnEnviarResultados.Text = "Resultados Enviados";
                BtnEnviarResultados.CssClass = "btn btn-info";

                return;
            }

            if (todosCargados)
            {
                BtnEnviarResultados.Enabled = true;
                BtnEnviarResultados.Text = "Enviar Resultados";
                BtnEnviarResultados.CssClass = "btn btn-success";
                return;
            }

            BtnEnviarResultados.Enabled = false;
            BtnEnviarResultados.Text = "Faltan Resultados";
            BtnEnviarResultados.CssClass = "btn btn-default";
        }

        protected void BtnEnviarResultados_Click(object sender,EventArgs e)
        {
            try
            {
                Lblerror.Text = "";

                if (ViewState["CodigoEXSO"] == null)
                {
                    throw new Exception("No se encontró el código de la solicitud.");
                }

                int codigoEXSO = Convert.ToInt32(ViewState["CodigoEXSO"]);
                object[] parametros = CrearParametrosOperacionSolicitud(19,codigoEXSO);

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count < 2 || ds.Tables[1].Rows.Count == 0)
                {
                    throw new Exception("No fue posible obtener los pacientes de la solicitud.");
                }

                DataTable pacientes = ds.Tables[1];

                foreach (DataRow fila in pacientes.Rows)
                {
                    string estado = fila["ESTADO_RESULTADO"].ToString().Trim().ToUpper();

                    if (estado != "CARGADO")
                    {
                        new Funciones().funShowJSMessage("Todos los pacientes deben tener resultados cargados antes de realizar el envío.",this);
                        return;
                    }
                }

                string body = ConstruirCorreoResultados(pacientes);
                string subject = "RESULTADOS DE EXÁMENES";
                string mensajeCorreo = EnviarCorreoResultados(subject,body);

                if (!string.IsNullOrWhiteSpace(mensajeCorreo))
                {
                    new Funciones().funShowJSMessage("No fue posible enviar el correo. " + mensajeCorreo,this);
                    return;
                }

                MarcarResultadosEnviados(codigoEXSO);
                PnlCarga.Visible = false;
                CargarSolicitud(codigoEXSO);

                new Funciones().funShowJSMessage("Resultados enviados correctamente.",this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private string ConstruirCorreoResultados(DataTable pacientes)
        {

            StringBuilder body = new StringBuilder();
            string fechaEnvio = DateTime.Now.ToString("dd/MM/yyyy");

            body.Append(
                "<div style='" +
                "font-family:Arial,Helvetica,sans-serif;" +
                "font-size:12px;" +
                "color:#000000;" +
                "'>"
            );

            body.Append(
                "<p style='margin:0 0 15px 0;'>" +
                "Estimados," +
                "</p>"
            );

            body.Append(
                "<p style='margin:0 0 20px 0;'>" +
                "Se confirma el agendamiento, la realización y la entrega " +
                "de los examenes de asegurabilidad del cliente de acuerdo al siguiente detalle:" +
                "</p>"
            );

            foreach (DataRow fila in pacientes.Rows)
            {

                string nombre = fila["PACIENTE"].ToString().Trim().ToUpper();
                string cedula = fila["NUM_DOCUMENTO"].ToString().Trim();

                body.Append(
                    "<table " +
                    "cellpadding='0' " +
                    "cellspacing='0' " +
                    "style='" +
                        "border-collapse:collapse;" +
                        "width:445px;" +
                        "margin:0 0 20px 0;" +
                        "font-family:Arial,Helvetica,sans-serif;" +
                        "font-size:12px;" +
                        "color:#000000;" +
                    "'>"
                );

                body.Append(
                    "<tr>" +
                        "<td colspan='2' " +
                            "style='" +
                                "background-color:#BDD7EE;" +
                                "border:1px solid #000000;" +
                                "padding:3px 7px;" +
                                "font-weight:bold;" +
                                "height:18px;" +
                            "'>" +
                            "CLIENTE" +
                        "</td>" +
                    "</tr>"
                );

                body.Append(
                    "<tr>" +

                        "<td style='" +
                            "width:110px;" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            "NOMBRE" +
                        "</td>" +

                        "<td style='" +
                            "width:335px;" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            Server.HtmlEncode(nombre) +
                        "</td>" +

                    "</tr>"
                );

                body.Append(
                    "<tr>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            "CEDULA" +
                        "</td>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            Server.HtmlEncode(cedula) +
                        "</td>" +

                    "</tr>"
                );

                body.Append(
                    "<tr>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            "FECHA" +
                        "</td>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            fechaEnvio +
                        "</td>" +

                    "</tr>"
                );

                body.Append(
                    "<tr>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            "ESTADO" +
                        "</td>" +

                        "<td style='" +
                            "border:1px solid #000000;" +
                            "padding:3px 7px;" +
                        "'>" +
                            "EXÁMENES COMPLETADOS" +
                        "</td>" +

                    "</tr>"
                );

                body.Append(
                    "</table>"
                );
            }

            body.Append(
                "</div>"
            );

            return body.ToString();
        }

        private void MarcarResultadosEnviados(int codigoEXSO)
        {

            object[] parametros = CrearParametrosOperacionSolicitud(18,codigoEXSO);
            parametros[0] = 18;
            parametros[29] = codigoEXSO;

            DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                throw new Exception("El correo fue enviado, pero no fue posible actualizar el estado de los resultados.");
            }

            string resultado = ds.Tables[0].Rows[0]["Resultado"].ToString().Trim();

            if (resultado != "OK-RESULTADOS-ENVIADOS")
            {
                throw new Exception("El correo fue enviado, pero no fue posible marcar los resultados como enviados. Respuesta: " + resultado);
            }
        }

        private string EnviarCorreoResultados(string subject,string body)
        {
            if (ViewState["Host"] == null || ViewState["Port"] == null || ViewState["EnableSSl"] == null || ViewState["Usuario"] == null || ViewState["Password"] == null)
            {
                throw new Exception("No se encuentra configurado el servidor de correo.");
            }

            string host = ViewState["Host"].ToString();
            int port = 0;

            if (!int.TryParse(ViewState["Port"].ToString(),out port))
            {
                throw new Exception("El puerto SMTP configurado no es válido.");
            }

            bool enableSsl = false;

            if (!bool.TryParse(ViewState["EnableSSl"].ToString(),out enableSsl))
            {
                throw new Exception("El parámetro EnableSSL no es válido.");
            }

            string usuario =ViewState["Usuario"].ToString();
            string password = ViewState["Password"].ToString();
            string mailTo = "vroldan@prestasalud.com";
            string mailCC = "";
            string mailsAlternos = "ealvear@prestasalud.com,vgavilanez@prestasalud.com";

            return new Funciones().SendHtmlEmailExamen(mailTo,subject,body,host,port,enableSsl,usuario,password,mailCC,"",mailsAlternos);
        }
        private void CargarConfiguracionCorreo()
        {

            object[] parametros = new object[1];
            parametros[0] = 13;

            DataSet dsCorreo = new Conexion(2, "").funConsultarSqls("sp_CargaCombos",parametros);

            if (dsCorreo == null || dsCorreo.Tables.Count == 0 || dsCorreo.Tables[0].Rows.Count < 5)
            {
                throw new Exception("No se encontró la configuración para envío de correos.");
            }

            ViewState["Host"] = dsCorreo.Tables[0].Rows[0][0].ToString().Trim().ToLower();
            ViewState["Port"] = dsCorreo.Tables[0].Rows[1][0].ToString().Trim();
            ViewState["EnableSSl"] = dsCorreo.Tables[0].Rows[2][0].ToString().Trim().ToLower();
            ViewState["Usuario"] = dsCorreo.Tables[0].Rows[3][0].ToString().Trim().ToLower();
            ViewState["Password"] = dsCorreo.Tables[0].Rows[4][0].ToString().Trim();
        }

    }

}
