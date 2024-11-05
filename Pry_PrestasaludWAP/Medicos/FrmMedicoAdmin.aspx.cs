using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Medicos_FrmMedicoAdmin : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    Image imgSemEspe = new Image();
    Image imgSemHora = new Image();
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
            lbltitulo.Text = "Administrar Médicos";
            funCargaMantenimiento();

            if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
        }
        else grdvDatos.DataSource = ViewState["grdvDatos"];
    }
    #endregion

    #region Funciones y Procedimiento
    protected void funCargaMantenimiento()
    {
        Array.Resize(ref objparam, 1);
        objparam[0] = 0;
        dt = new Conexion(2, "").funConsultarSqls("sp_CargarMedicoAdmin", objparam);
        if (dt.Tables[0].Rows.Count > 0)
        {
            grdvDatos.DataSource = dt;
            grdvDatos.DataBind();
            grdvDatos.UseAccessibleHeader = true;
            grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        ViewState["grdvDatos"] = grdvDatos.DataSource;
    }
    #endregion

    #region Botones y Eventos
    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmNuevoMedico.aspx?Tipo=" + "N" + "&Codigo=0");
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;
        String strCodigoMedico = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
        Response.Redirect("FrmNuevoMedico.aspx?Tipo=" + "M" + "&Codigo=" + strCodigoMedico);
    }

    protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                imgSemEspe = (Image)(e.Row.Cells[3].FindControl("imgSemEspe"));
                imgSemHora = (Image)(e.Row.Cells[4].FindControl("imgSemHora"));

                int codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = "";
                objparam[2] = 17;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt != null)
                {
                    if (dt.Tables[0].Rows.Count > 0) imgSemEspe.ImageUrl = "~/Botones/semaforoVerde.png";
                    if (dt.Tables[1].Rows.Count > 0) imgSemHora.ImageUrl = "~/Botones/semaforoVerde.png";
                }
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}