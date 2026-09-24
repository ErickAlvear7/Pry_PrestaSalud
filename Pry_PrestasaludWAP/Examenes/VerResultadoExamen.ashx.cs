using System;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace Pry_PrestasaludWAP.Examenes
{
    public class VerResultadoExamen : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
           
                if (context.Session == null || context.Session["usuCodigo"] == null || context.Session["usuCodigo"].ToString() == "")
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Sesión no válida.");
                    return;
                }

                int codigoEXSO = 0;
                int codigoEXRD = 0;


                if (!int.TryParse(context.Request.QueryString["exso"],out codigoEXSO) || codigoEXSO <= 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Código de solicitud no válido.");
                    return;
                }


                if (!int.TryParse(context.Request.QueryString["exrd"],out codigoEXRD) || codigoEXRD <= 0)
                {
                    context.Response.StatusCode = 400;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Código de documento no válido.");
                    return;
                }


                // ==========================================
                // CONSULTAR DOCUMENTO - TIPO 21
                // ==========================================

                object[] parametros = CrearParametrosOperacionSolicitud(context,21,codigoEXSO);

                parametros[36] = codigoEXRD;

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("No se encontró el documento.");

                    return;
                }

                DataRow fila = ds.Tables[0].Rows[0];


                // ==========================================
                // VALIDAR QUE SEA PDF
                // ==========================================

                string extension = fila["EXRD_EXTENSION"].ToString().Trim().ToLower();

                if (extension != ".pdf" && extension != "pdf")
                {
                    context.Response.StatusCode = 415;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("La vista previa solamente está disponible para archivos PDF.");

                    return;
                }

                // ==========================================
                // VALIDAR BINARIO
                // ==========================================

                if (fila["EXRD_DOCUMENTO"] == DBNull.Value)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("El documento se encuentra vacío.");

                    return;
                }


                byte[] archivo = (byte[])fila["EXRD_DOCUMENTO"];
               
                if (archivo == null || archivo.Length == 0)
                {
                    context.Response.StatusCode = 404;
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("El documento se encuentra vacío.");

                    return;
                }

                archivo = PrepararPdfParaVista(archivo);

                string nombre = fila["EXRD_NOMBRE"].ToString().Trim();

                nombre = nombre.Replace("\"", "").Replace("\r", "").Replace("\n", "");

                if (string.IsNullOrWhiteSpace(nombre))
                {
                    nombre ="resultado.pdf";
                }

                context.Response.Clear();
                context.Response.Buffer =true;
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Disposition","inline; filename=\"" + nombre + "\"");
                context.Response.AddHeader("X-Content-Type-Options","nosniff");
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.Response.Cache.SetNoStore();
                context.Response.BinaryWrite(archivo);
                context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception)
            {
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                context.Response.Write("No fue posible visualizar el documento.");
            }
        }

        private object[] CrearParametrosOperacionSolicitud(HttpContext context,int tipo,int codigoEXSO)
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
            parametros[18] = Convert.ToInt32(context.Session["usuCodigo"]);
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
            parametros[41] = Convert.ToInt32(context.Session["usuCodigo"]);
            parametros[42] = context.Session["MachineName"] != null ? context.Session["MachineName"].ToString() : "";

            return parametros;
        }

        private byte[] PrepararPdfParaVista(byte[] archivoOriginal)
        {
            if (archivoOriginal == null || archivoOriginal.Length == 0)
            {
                return archivoOriginal;
            }

            try
            {
                using (MemoryStream entrada = new MemoryStream(archivoOriginal))
                {
                    PdfDocument documento = PdfReader.Open(entrada,PdfDocumentOpenMode.Modify);

                    documento.PageMode = PdfPageMode.UseNone;

                    using (MemoryStream salida = new MemoryStream())
                    {
                        documento.Save(salida,false);
                        documento.Close();

                        return salida.ToArray();
                    }
                }
            }
            catch
            {
             
                return archivoOriginal;
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}