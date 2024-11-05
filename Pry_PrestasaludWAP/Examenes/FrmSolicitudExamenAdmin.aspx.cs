using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmSolicitudExamenAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        string codigoexso = "", codigopers = "", numdocumento = "", estado = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["usuCodigo"] = "2674";
            //Session["MachineName"] = "pc";
            //Session["CodGerencial"] = "78";
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    Lbltitulo.Text = "Administrar Solicitud Exámenes";
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
            objparam[2] = 142;
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
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString();
                    switch (estado)
                    {
                        case "SRR":
                            e.Row.Cells[4].BackColor = System.Drawing.Color.LightSeaGreen;
                            break;
                        case "SRV":
                            e.Row.Cells[4].BackColor = System.Drawing.Color.Coral;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSolicitudExamen.aspx?CodigoPERS=0" +
                "&CodigoEXSO=0&NumDocumento=''", true);
        }

        protected void ImgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
            codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
            numdocumento = GrdvDatos.DataKeys[gvRow.RowIndex].Values["NumDocumento"].ToString();
            Response.Redirect("FrmSolicitudExamen.aspx?CodigoPERS=" + codigopers +
                "&CodigoEXSO=" + codigoexso + "&NumDocumento=" + numdocumento, true);
        }
        #endregion
    }
}