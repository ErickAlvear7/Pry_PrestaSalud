using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace Pry_PrestasaludWAP.Examenes
{
    public class VerAdjuntoAuditorCliente : IHttpHandler, IReadOnlySessionState
    {
        public bool IsReusable { get { return false; } }
        public void ProcessRequest(HttpContext context)
        {
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.Cache.SetNoStore();
            context.Response.AppendHeader("X-Content-Type-Options", "nosniff");
            int usuario, solicitud, persona;
            if (context.Session == null || !int.TryParse(Convert.ToString(context.Session["usuCodigo"]), out usuario) || usuario <= 0)
            { Error(context, 401, "La sesión expiró."); return; }
            if (!int.TryParse(context.Request.QueryString["exso"], out solicitud) || solicitud <= 0
                || !int.TryParse(context.Request.QueryString["pers"], out persona) || persona <= 0)
            { Error(context, 400, "Solicitud o paciente no válido."); return; }
            try
            {
                DataSet ds = new Conexion(2, "").FunInformeOrdenCliente(usuario, solicitud, persona, "ADJUNTO");
                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                { Error(context, 404, "Adjunto no disponible."); return; }
                DataRow r = ds.Tables[0].Rows[0];
                byte[] contenido = r["EXRA_DOCUMENTO"] as byte[];
                if (contenido == null || contenido.Length == 0)
                { Error(context, 404, "No existe un archivo adjunto."); return; }
                string extension = Convert.ToString(r["EXRA_EXTENSION"]).Trim().ToLowerInvariant().TrimStart('.');
                bool pdf = extension == "pdf";
                bool descargar = context.Request.QueryString["descargar"] == "1" || !pdf;
                context.Response.ContentType = pdf ? "application/pdf" : extension == "xlsx"
                    ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    : extension == "xls" ? "application/vnd.ms-excel" : "application/octet-stream";
                string nombre = System.IO.Path.GetFileName(Convert.ToString(r["EXRA_NOMBRE_DOC"]));
                if (string.IsNullOrWhiteSpace(nombre)) nombre = "informe-auditor";
                context.Response.AddHeader("Content-Disposition", (descargar ? "attachment" : "inline")
                    + "; filename=\"informe-auditor\"; filename*=UTF-8''" + Uri.EscapeDataString(nombre));
                context.Response.BinaryWrite(contenido);
            }
            catch (Exception)
            { Error(context, 403, "No se pudo consultar el adjunto para su campaña."); }
        }
        private static void Error(HttpContext context, int codigo, string mensaje)
        { context.Response.StatusCode = codigo; context.Response.TrySkipIisCustomErrors = true; context.Response.ContentType = "text/plain; charset=utf-8"; context.Response.Write(mensaje); }
    }
}
