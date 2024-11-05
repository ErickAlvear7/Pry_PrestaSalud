using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmSolicitudOperadorAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton imgdescargar = new ImageButton();
        ImageButton imgview = new ImageButton();
        ImageButton imgagendar = new ImageButton();
        string codigoexso = "", codigopers = "", estado = "", codigotitu = "", type = "", Name = "", extension = "",
            codigoprod = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Session["usuCodigo"] = "2675";
                //Session["MachineName"] = "pc";
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                if (!IsPostBack)
                {
                    ViewState["Conectar"] = ConfigurationManager.ConnectionStrings["ConnecSQL"].ConnectionString;
                    Session["Descargado"] = "NO";
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
            objparam[0] = 0;
            objparam[1] = "";
            objparam[2] = 144;
            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            GrdvDatos.DataSource = dts;
            GrdvDatos.DataBind();
        }

        private void FunActualizarEstado(int codigoexso)
        {
            try
            {
                Array.Resize(ref objparam, 11);
                objparam[0] = 21;
                objparam[1] = "";
                objparam[2] = "SRV";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = int.Parse(Session["usuCodigo"].ToString());
                objparam[7] = codigoexso;
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunDownloadDocument(int codigo)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = "";
                objparam[2] = 161;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                type = dts.Tables[0].Rows[0]["Tipo"].ToString();
                Name = dts.Tables[0].Rows[0]["Nombre"].ToString();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = type;
                Response.AddHeader("content-disposition", "attachment;filename=" + Name);
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dts.Tables[0].Rows[0]["DataBin"]);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                Session["Descargado"] = "SI";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
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
                    imgview = (ImageButton)(e.Row.Cells[7].FindControl("ImgView"));
                    extension = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Ext"].ToString();
                    switch (estado)
                    {
                        case "SRR":
                            e.Row.Cells[5].BackColor = System.Drawing.Color.LightSeaGreen;
                            break;
                        case "SRV":
                            e.Row.Cells[5].BackColor = System.Drawing.Color.Coral;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDescargar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                imgdescargar = (ImageButton)(gvRow.Cells[6].FindControl("ImgDescargar"));
                imgagendar = (ImageButton)(gvRow.Cells[7].FindControl("ImgAgendar"));
                codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
                FunDownloadDocument(int.Parse(codigoexso));
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgAgendar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Session["Descargado"].ToString() == "SI")
                {
                    GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                    codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
                    codigoprod = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPROD"].ToString();
                    FunActualizarEstado(int.Parse(codigoexso));
                    codigopers = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERS"].ToString();
                    codigotitu = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoTITU"].ToString();
                    Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?CodigoTitular=" + codigotitu +
                        "&CodigoProducto=" + codigoprod + "&Regresar=1", true);
                }
                else new Funciones().funShowJSMessage("Descarge el Documento de Autorización..!", this);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}