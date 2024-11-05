namespace Pry_PrestasaludWAP.SupervisionOdonto
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmSuperOdontoAdmin : Page
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
                lbltitulo.Text = "Administrar Supervisor-Odontológico";
                funCargaMantenimiento();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else
            {
                grdvDatos.DataSource = ViewState["grdvDatos"];
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = "";
            objparam[2] = 73;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
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
            Response.Redirect("FrmSuperOdonto.aspx?CodigoSupervisor=0", true);
        }

        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            string strCodigoSupervisor = grdvDatos.DataKeys[intIndex].Values["CodigoSupervisor"].ToString();
            Response.Redirect("FrmSuperOdonto.aspx?CodigoSupervisor=" + strCodigoSupervisor, true);
        }
        #endregion
    }
}