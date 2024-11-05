using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Tarea_FrmTareaAdmin : System.Web.UI.Page
{
    #region Variables
    DataSet ds = new DataSet();
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
            lbltitulo.Text = "Administrar Tareas";
            funCargaMantenimiento();
            if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
        }
        else
        {
            grdvDatos.DataSource = Session["grdvDatos"];
            //ctrlbuscar.GrdGrillaBusqueda = grdvDatos;
        }
    }
    #endregion

    #region Funciones y Procedimiento
    protected void funCargaMantenimiento()
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = 0;
        ds = new Conexion(2, "").funConsultarSqls("sp_TareaAdminReadAll", objparam);

        if (ds.Tables[0].Rows.Count > 0)
        {
            grdvDatos.DataSource = ds;
            grdvDatos.DataBind();
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        Session["grdvDatos"] = grdvDatos.DataSource;
    }
    #endregion

    #region Botones y Eventos
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmNuevaTarea.aspx?Tipo=" + "N" + "&Codigo=0");
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mantenedor/FrmDetalle.aspx");
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;
        String strCodigoMenu = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
        Response.Redirect("FrmNuevaTarea.aspx?Tipo=" + "M" + "&Codigo=" + strCodigoMenu);
    }
    #endregion
}