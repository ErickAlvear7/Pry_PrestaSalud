using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmExamenSegurosAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        string codigo;
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
                Lbltitulo.Text = "Administrar Examenes";
                FunCargaMantenimiento();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        }
        #endregion

        #region Procedimiento y Funciones
        protected void FunCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = "";
            objparam[2] = 132;
            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            GrdvDatos.DataSource = dts;
            GrdvDatos.DataBind();
            if (GrdvDatos.Rows.Count > 0)
            {
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNuevoExamenAdmin.aspx?Codigo=0", true);
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
            Response.Redirect("FrmNuevoExamenAdmin.aspx?Codigo=" + codigo, true);
        }
        #endregion
    }
}