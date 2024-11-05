using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Medicos
{
    public partial class FrmVademecumAdmin : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                if (Session["SalirAgenda"].ToString() == "NO")
                {
                    if (Session["TipoCita"].ToString() == "CitaMedica")
                        Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                    if (Session["TipoCita"].ToString() == "CitaOdontologica")
                        Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                    return;
                }
                lbltitulo.Text = "Listado << VADEMECUM >>";
                FunCargaMantenimiento();
            }
        }
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            Array.Resize(ref objparam, 11);
            objparam[0] = 10;
            objparam[1] = "";
            objparam[2] = "";
            objparam[3] = "";
            objparam[4] = "";
            objparam[5] = "";
            objparam[6] = 0;
            objparam[7] = 0;
            objparam[8] = 0;
            objparam[9] = 0;
            objparam[10] = 0;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
            grdvDatos.DataSource = dt;
            grdvDatos.DataBind();
            if (grdvDatos.Rows.Count > 0)
            {
                grdvDatos.UseAccessibleHeader = true;
                grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion
    }
}