using System;
using System.Data;
using System.Web;

namespace Pry_PrestasaludWAP.Examenes
{
    public class VerImagenCliente : IHttpHandler,System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Session["usuCodigo"] == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }


                int codigoEXSO = 0;
                int codigoEXCI = 0;


                int.TryParse(context.Request["exso"],out codigoEXSO);
                int.TryParse(context.Request["id"],out codigoEXCI);


                if (codigoEXSO <= 0 || codigoEXCI <= 0)
                {
                    context.Response.StatusCode = 400;
                    return;
                }


                object[] parametros = CrearParametros(context,31,codigoEXSO);
                parametros[36] =codigoEXCI;


                DataSet ds = new Conexion(2, "").FunInsertSolictudExamen(parametros);


                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    context.Response.StatusCode = 404;
                    return;
                }


                DataRow fila = ds.Tables[0].Rows[0];


                byte[] imagen =
                    (byte[])fila[
                        "EXCI_IMAGEN"
                    ];


                string tipo =
                    fila["EXCI_TIPO"]
                        .ToString();


                context.Response.Clear();

                context.Response.ContentType =
                    tipo;

                context.Response.BinaryWrite(
                    imagen
                );

                context.Response.End();
            }
            catch
            {
                context.Response.StatusCode = 500;
            }
        }


        private object[] CrearParametros(HttpContext context,int tipo,int codigoEXSO)
        {
            object[] p = new object[43];

            p[0] = tipo;
            p[1] = 0;
            p[2] = "";
            p[3] = "";
            p[4] = "";
            p[5] = "";
            p[6] = "";
            p[7] = "";
            p[8] = "";
            p[9] = "";
            p[10] = "";
            p[11] = 0;
            p[12] = "";
            p[13] = "";
            p[14] = "";
            p[15] = "";
            p[16] = "";
            p[17] = 0;
            p[18] = Convert.ToInt32(context.Session["usuCodigo"]);
            p[19] = DateTime.Now;
            p[20] = "";
            p[21] = new byte[0];
            p[22] = "";
            p[23] = "";
            p[24] = "";
            p[25] = 0;
            p[26] = "0.00";
            p[27] = "0.00";
            p[28] = "0";
            p[29] = codigoEXSO;
            p[30] = "Activo";
            p[31] = "";
            p[32] = "";
            p[33] = "";
            p[34] = "";
            p[35] = "";
            p[36] = 0;
            p[37] = 0;
            p[38] = 0;
            p[39] = 0;
            p[40] = 0;
            p[41] = Convert.ToInt32(context.Session["usuCodigo"]);
            p[42] = context.Session["MachineName"] != null ? context.Session["MachineName"].ToString() : "";

            return p;
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