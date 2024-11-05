using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Menu_FrmMenuAdmin : System.Web.UI.Page
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
            lbltitulo.Text = "Administrar Menú";
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
        ImageButton imgSubir = new ImageButton();
        ImageButton imgBajar = new ImageButton();

        Array.Resize(ref objparam, 1);
        objparam[0] = 0;
        ds = new Conexion(2, "").funConsultarSqls("sp_MenuAdminReadAll", objparam);
        grdvDatos.DataSource = ds;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        
        Session["grdvDatos"] = grdvDatos.DataSource;
        //ctrlbuscar.GrdGrillaBusqueda = grdvDatos;
        var intlastrow = grdvDatos.Rows.Count - 1;

        imgSubir = (ImageButton)grdvDatos.Rows[0].Cells[3].FindControl("imgSubirNivel");
        imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
        imgSubir.Enabled = false;

        imgBajar = (ImageButton)grdvDatos.Rows[intlastrow].Cells[3].FindControl("imgBajarNivel");
        imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
        imgBajar.Enabled = false;

        //ctrlbuscar.CargarComponente();
        //grdvDatos.Columns[0].Visible = false;
    }
    #endregion

    #region Botones y Eventos
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmNuevoMenu.aspx");
    }
    //protected void btnSalir_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/Mantenedor/FrmDetalle.aspx");
    //}
    protected void grdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDatos.PageIndex = e.NewPageIndex;
        //ctrlbuscar.GrdGrillaBusqueda = grdvDatos;
        grdvDatos.DataBind();
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;
        //String strCodigoMenu = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
        Response.Redirect("FrmEditarMenu.aspx?codigo=" + grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
    }
    protected void imgSubirNivel_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;

        Array.Resize(ref objparam, 2);
        objparam[0] = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
        objparam[1] = 0;
        new Conexion(2, "").funConsultarSqls("sp_CamOrdenMenu", objparam);
        funCargaMantenimiento();
    }
    protected void imgBajarNivel_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;

        //var strCodMenu = grdvDatos.DataKeys[intIndex].Values["Codigo"];
        Array.Resize(ref objparam, 2);
        objparam[0] = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
        objparam[1] = 1;
        new Conexion(2, "").funConsultarSqls("sp_CamOrdenMenu", objparam);
        funCargaMantenimiento();
    }
    #endregion
}