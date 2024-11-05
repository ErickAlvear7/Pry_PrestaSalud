using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Regiones_FrmCiudadAdmin : System.Web.UI.Page
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
            lbltitulo.Text = "Administrar Ciudades";
            funCargaMantenimiento();

            if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
        }
        else
        {
            grdvDatos.DataSource = Session["grdvDatos"];
        }
    }
    #endregion

    #region Funciones y Procedimiento
    protected void funCargaMantenimiento()
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = 2;
        dt = new Conexion(2, "").funConsultarSqls("sp_CargarRegion", objparam);
        grdvDatos.DataSource = dt;
        grdvDatos.DataBind();
        if (grdvDatos.Rows.Count > 0)
        {
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        Session["grdvDatos"] = grdvDatos.DataSource;

    }
    #endregion

    #region Botones y Eventos
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmNuevaCiudad.aspx?Tipo=" + "N");
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Mantenedor/FrmDetalle.aspx");
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;
        var strCodigo = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
        var strProvin = grdvDatos.DataKeys[intIndex].Values["CodProv"].ToString();
        var strCiudad = grdvDatos.DataKeys[intIndex].Values["Ciudad"].ToString();
        var strEstado = grdvDatos.DataKeys[intIndex].Values["Estado"].ToString();
        Response.Redirect("FrmNuevaCiudad.aspx?Tipo=" + "E" + "&Codigo=" + strCodigo + "&Ciudad=" + strCiudad + "&Provincia=" + strProvin + "&Estado=" + strEstado);
    }
    #endregion
}