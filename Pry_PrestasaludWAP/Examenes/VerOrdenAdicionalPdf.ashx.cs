using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace Pry_PrestasaludWAP.Examenes
{
    public class VerOrdenAdicionalPdf : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable { get { return false; } }
        public void ProcessRequest(HttpContext contexto)
        {
            contexto.Response.Cache.SetCacheability(HttpCacheability.NoCache); contexto.Response.Cache.SetNoStore();
            contexto.Response.AppendHeader("X-Content-Type-Options", "nosniff");
            int usuario, orden;
            if (contexto.Session == null || !int.TryParse(Convert.ToString(contexto.Session["usuCodigo"]), out usuario) || usuario <= 0)
            { Error(contexto, 401, "La sesión expiró."); return; }
            if (!int.TryParse(contexto.Request.QueryString["orden"], out orden) || orden <= 0)
            { Error(contexto, 400, "Orden no válida."); return; }
            try
            {
                bool agenda = contexto.Request.QueryString["ambito"] == "agenda";
                byte[] contenido; string nombre;
                if (agenda)
                {
                    DataRow r = Fila(new Conexion(2, "").FunOrdenAdicionalAgenda(usuario, "PDF", orden));
                    contenido = r["DataBin"] as byte[]; nombre = Convert.ToString(r["Nombre"]);
                }
                else
                {
                    DataRow meta = Fila(new Conexion(2, "").FunOrdenAdicionalCliente(usuario, orden, "PREPARAR"));
                    if (Convert.ToString(meta["Estado"]) == "BORRADOR")
                    { contenido = OrdenAdicionalPdf.Crear(meta); nombre = "Borrador_Orden_" + orden + ".pdf"; }
                    else
                    {
                        DataRow r = Fila(new Conexion(2, "").FunOrdenAdicionalCliente(usuario, orden, "PDF"));
                        contenido = r["DataBin"] as byte[]; nombre = Convert.ToString(r["Nombre"]);
                    }
                }
                if (contenido == null || contenido.Length == 0) { Error(contexto, 404, "La orden no tiene un PDF disponible."); return; }
                contexto.Response.ContentType = "application/pdf";
                contexto.Response.AddHeader("Content-Disposition", "inline; filename=\"orden-adicional.pdf\"; filename*=UTF-8''" + Uri.EscapeDataString(nombre));
                contexto.Response.BinaryWrite(contenido);
                if (agenda) contexto.Session["OrdenAdicionalPdfDescargada_" + orden] = true;
            }
            catch (Exception) { Error(contexto, 403, "No se pudo consultar el PDF con los permisos actuales."); }
        }
        private static DataRow Fila(DataSet ds)
        { if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count != 1) throw new InvalidOperationException("Orden no disponible."); return ds.Tables[0].Rows[0]; }
        private static void Error(HttpContext c, int codigo, string mensaje)
        { c.Response.StatusCode = codigo; c.Response.TrySkipIisCustomErrors = true; c.Response.ContentType = "text/plain; charset=utf-8"; c.Response.Write(mensaje); }
    }
}
