using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Procedimientos
{
    public partial class FrmProcedimientosAdmin : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton imgSubir = new ImageButton();
        int fila = 0;
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
                lbltitulo.Text = "Administrar Red/Odontológica";
                funCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = 0;
            objparam[2] = 25;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                grdvDatos.UseAccessibleHeader = true;
                grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                ViewState["grdvDatos"] = grdvDatos.DataSource;
                //foreach (GridViewRow row in grdvDatos.Rows)
                //{
                //    if (int.Parse(grdvDatos.DataKeys[row.RowIndex].Values["Prioridad"].ToString()) > 1)
                //    {
                //        imgSubir = row.FindControl("imgPrioridad") as ImageButton;
                //        imgSubir.ImageUrl = "~/Botones/activada_up.png";
                //        imgSubir.Enabled = true;
                //    }
                //    fila = Convert.ToInt16(row.RowIndex);
                //}
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNuevoProcedimiento.aspx?Tipo=" + "N" + "&Codigo=0");
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            var strCodigo = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            Response.Redirect("FrmNuevoProcedimiento.aspx?Tipo=" + "E" + "&Codigo=" + strCodigo);
        }

        //protected void imgPrioridad_Click(object sender, ImageClickEventArgs e)
        //{
        //    GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        //    int intIndex = gvRow.RowIndex;

        //    Array.Resize(ref objparam, 10);
        //    objparam[0] = 1;
        //    objparam[1] = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
        //    objparam[2] = "";
        //    objparam[3] = "";
        //    objparam[4] = 0.00;
        //    objparam[5] = "";
        //    objparam[6] = int.Parse(grdvDatos.DataKeys[intIndex].Values["Prioridad"].ToString());
        //    objparam[7] = 1;
        //    objparam[8] = 0;
        //    objparam[9] = "";
        //    dt = new Conexion(2, "").funConsultarSqls("sp_NuevoProcedimiento", objparam);
        //    funCargaMantenimiento();
        //}
        #endregion
    }
}