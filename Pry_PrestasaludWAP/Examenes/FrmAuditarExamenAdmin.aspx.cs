using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmAuditarExamenAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        Image ImgEstado = new Image();
        string codigoexso = "";
        int codigocamp = 0;
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
                    Lbltitulo.Text = "Auditoria de Exámenes";
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
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 145;//cambio aki camp codigo 24
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if(dts.Tables[0].Rows.Count>0)
                codigocamp = int.Parse(dts.Tables[0].Rows[0]["CodigoCAMP"].ToString());

                objparam[0] = codigocamp;
                objparam[1] = "";
                objparam[2] = 159;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = dts;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnAuditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
                Response.Redirect("FrmAuditarExamen.aspx?CodigoEXSO=" + codigoexso, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender,GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType != DataControlRowType.DataRow)
                {
                    return;
                }

                string estado = "";

                if (GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"] != null)
                {
                    estado =GrdvDatos.DataKeys[e.Row.RowIndex].Values["EstadoCodigo"].ToString().Trim().ToUpper();
                }

                // Columna Estado
                TableCell celdaEstado = e.Row.Cells[3];


                switch (estado)
                {
                    // =====================================
                    // PENDIENTE DE AUDITAR
                    // =====================================
                    case "EXR":

                        celdaEstado.BackColor = System.Drawing.Color.Aquamarine;
                        celdaEstado.ForeColor = System.Drawing.Color.DarkSlateGray;
                        break;

                    // =====================================
                    // AUDITADO ACEPTADO
                    // =====================================
                    case "AUA":

                        celdaEstado.BackColor = System.Drawing.Color.LightGreen;
                        celdaEstado.ForeColor = System.Drawing.Color.DarkGreen;
                        break;


                    // =====================================
                    // AUDITADO RECHAZADO
                    // =====================================
                    case "AUR":

                        celdaEstado.BackColor = System.Drawing.Color.LightCoral;
                        celdaEstado.ForeColor = System.Drawing.Color.DarkRed;
                        break;
                }

                celdaEstado.Font.Bold = true;
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}