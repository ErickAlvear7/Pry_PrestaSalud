using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmAuditarExamen : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbexamenes = new DataTable();
        Object[] objparam = new Object[1];
        ImageButton imgexa1, imgexa2, imgexa3, imgexa4, imgexa5;
        string type = "", name = "", exa1 = "", exa2 = "", exa3 = "", exa4 = "", exa5 = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                try
                {
                    ViewState["CodigoEXSO"] = Request["CodigoEXSO"];
                    FunCargaMantenimiento();
                    Lbltitulo.Text = "Auditar Registro Exámenes";
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[1] = "";
                objparam[2] = 156;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvDatos.DataSource = dts;
                GrdvDatos.DataBind();
                objparam[2] = 143;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvExamenes.DataSource = dts;
                GrdvExamenes.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunDownloadDocument(int opcion)
        {
            try
            {
                Array.Resize(ref objparam, 11);
                objparam[0] = 28;
                objparam[1] = "";
                objparam[2] = "";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                switch (opcion)
                {
                    case 0:
                        objparam[6] = 0;
                        break;
                    case 1:
                        objparam[6] = 1;
                        break;
                    case 2:
                        objparam[6] = 2;
                        break;
                    case 3:
                        objparam[6] = 3;
                        break;
                    case 4:
                        objparam[6] = 4;
                        break;
                }
                dtx = new Conexion(2, "").FunConsultaDatos1(objparam);
                type = dtx.Tables[0].Rows[0]["Tipo"].ToString();
                name = dtx.Tables[0].Rows[0]["Nombre"].ToString();
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = type;
                Response.AddHeader("content-disposition", "attachment;filename=" + name);
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dtx.Tables[0].Rows[0]["DataBin"]);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtObservacion.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Observación..!", this);
                    return;
                }

                Array.Resize(ref objparam, 11);
                objparam[0] = 26;
                objparam[1] = TxtObservacion.Text.Trim().ToUpper();
                objparam[2] = "EAU";
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = int.Parse(Session["usuCodigo"].ToString()); 
                objparam[7] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[8] = 0;
                objparam[9] = 0;
                objparam[10] = 0;
                dts = new Conexion(2, "").FunConsultaDatos1(objparam);
                Response.Redirect("FrmAuditarExamenAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    imgexa1 = (ImageButton)(e.Row.Cells[4].FindControl("ImgDownload"));
                    imgexa2 = (ImageButton)(e.Row.Cells[5].FindControl("ImgDownload1"));
                    imgexa3 = (ImageButton)(e.Row.Cells[6].FindControl("ImgDownload2"));
                    imgexa4 = (ImageButton)(e.Row.Cells[7].FindControl("ImgDownload3"));
                    imgexa5 = (ImageButton)(e.Row.Cells[8].FindControl("ImgDownload4"));

                    exa1 = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Examen1"].ToString();
                    exa2 = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Examen2"].ToString();
                    exa3 = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Examen3"].ToString();
                    exa4 = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Examen4"].ToString();
                    exa5 = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Examen5"].ToString();
                    if (!string.IsNullOrEmpty(exa1))
                    {
                        imgexa1.Enabled = true;
                        imgexa1.ImageUrl = "~/Botones/downloadcolor.png";
                    }
                    if (!string.IsNullOrEmpty(exa2))
                    {
                        imgexa2.Enabled = true;
                        imgexa2.ImageUrl = "~/Botones/downloadcolor.png";
                    }
                    if (!string.IsNullOrEmpty(exa3))
                    {
                        imgexa3.Enabled = true;
                        imgexa3.ImageUrl = "~/Botones/downloadcolor.png";
                    }
                    if (!string.IsNullOrEmpty(exa4))
                    {
                        imgexa4.Enabled = true;
                        imgexa4.ImageUrl = "~/Botones/downloadcolor.png";
                    }
                    if (!string.IsNullOrEmpty(exa5))
                    {
                        imgexa5.Enabled = true;
                        imgexa5.ImageUrl = "~/Botones/downloadcolor.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(0);
        }

        protected void ImgDownload1_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(1);
        }

        protected void ImgDownload2_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(2);
        }

        protected void ImgDownload3_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(3);
        }

        protected void ImgDownload4_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument(4);
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmAuditarExamenAdmin.aspx", true);
        }
        #endregion
    }
}