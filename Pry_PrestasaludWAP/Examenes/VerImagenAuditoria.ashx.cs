using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace Pry_PrestasaludWAP.Examenes
{
    public class VerImagenAuditoria : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session["usuCodigo"] == null || string.IsNullOrWhiteSpace(context.Session["usuCodigo"].ToString()))
                {
                    context.Response.StatusCode = 401;
                    return;
                }

                int codigoEXSO = 0;
                int codigoEXAI = 0;

                if (!int.TryParse(context.Request.QueryString["exso"],out codigoEXSO) || codigoEXSO <= 0)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                if (!int.TryParse(context.Request.QueryString["id"],out codigoEXAI) || codigoEXAI <= 0)
                {
                    context.Response.StatusCode = 400;
                    return;
                }

                object[] parametros = new object[43];

                parametros[0] = 26;
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
                parametros[30] = "";
                parametros[31] = "";
                parametros[32] = "";
                parametros[33] = "";
                parametros[34] = "";
                parametros[35] = "";
                parametros[36] = codigoEXAI;
                parametros[37] = 0;
                parametros[38] = 0;
                parametros[39] = 0;
                parametros[40] = 0;
                parametros[41] = Convert.ToInt32(context.Session["usuCodigo"]);
                parametros[42] = context.Session["MachineName"] != null ? context.Session["MachineName"].ToString() : "";

                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                DataRow fila = ds.Tables[0].Rows[0];

                if (fila["DataBin"] == DBNull.Value)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                byte[] imagen = (byte[])fila["DataBin"];

                if (imagen == null || imagen.Length == 0)
                {
                    context.Response.StatusCode = 404;
                    return;
                }

                string tipo = fila["EXAI_TIPO"] != DBNull.Value ? fila["EXAI_TIPO"].ToString().Trim() : "";

                if (string.IsNullOrWhiteSpace(tipo))
                {
                    tipo = "image/png";
                }

                context.Response.Clear();
                context.Response.ContentType = tipo;
                context.Response.Cache.SetCacheability(HttpCacheability.Private);
                context.Response.Cache.SetMaxAge(TimeSpan.FromMinutes(10));
                context.Response.BinaryWrite(imagen);
                context.ApplicationInstance.CompleteRequest();
            }
            catch
            {
                context.Response.StatusCode = 500;
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