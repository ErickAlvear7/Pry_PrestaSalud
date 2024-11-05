using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Prestadora
{
    public partial class FrmPrestaEspeAdmin : System.Web.UI.Page
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
                lbltitulo.Text = "Administrar Especialidad";
                funCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarEspePrestadoraAdmin", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatos.DataSource = dt;
                    grdvDatos.DataBind();
                    grdvDatos.UseAccessibleHeader = true;
                    grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNuevaPrestaEspe.aspx?Tipo=" + "N" + "&Codigo=0");
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            var strCodigo = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            Response.Redirect("FrmNuevaPrestaEspe.aspx?Tipo=" + "E" + "&Codigo=" + strCodigo);
        }
        #endregion
    }
}