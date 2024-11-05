using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmExamenRealizadoAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        string codigoexso = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Administrar Exámenes Realizados";
                    FunCargaMantenimiento();
                    if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(Session["usuCodigo"].ToString());
            objparam[1] = "";
            objparam[2] = 157;
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
        protected void ImgRevisar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
            Response.Redirect("FrmRevisionExamen.aspx?CodigoEXSO=" + codigoexso, true);
        }
        #endregion
    }
}