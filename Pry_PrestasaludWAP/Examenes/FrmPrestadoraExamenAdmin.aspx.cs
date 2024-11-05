using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmPrestadoraExamenAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Image ImgEstado = new Image();
        ImageButton imgSelecc = new ImageButton();
        ImageButton imgAusent = new ImageButton();
        string strResponse = "", codigocita = "", codigoexso = "";
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
                    Lbltitulo.Text = "Registro Solicitud de Exámenes";
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
                objparam[2] = 154;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    GrdvDatos.DataSource = dt;
                    GrdvDatos.DataBind();
                    GrdvDatos.UseAccessibleHeader = true;
                    GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                objparam[2] = 115;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) Lbltitulo.Text = "Realizar Examen ---> " + dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        #endregion

        #region Botones y Eventos
        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //try
            //{
            //    if (e.Row.RowIndex >= 0)
            //    {
            //        horaSistema = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "00";
            //        fechaCita = GrdvDatos.DataKeys[e.Row.RowIndex].Values["FechaCodigo"].ToString();
            //        fechaActual = DateTime.Now.ToString("MM/dd/yyyy");
            //        horaActual = TimeSpan.Parse(horaSistema);
            //        hora = e.Row.Cells[3].Text.Split(new char[] { '-' });
            //        horaAgenda = TimeSpan.Parse(hora[0].ToString());
            //        ImgEstado = (Image)(e.Row.Cells[5].FindControl("ImgEstado"));
            //        imgSelecc = (ImageButton)(e.Row.Cells[6].FindControl("BtnExamen"));
            //        imgAusent = (ImageButton)(e.Row.Cells[7].FindControl("BtnAusente"));
            //        if (DateTime.ParseExact(fechaActual, "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(fechaCita, "MM/dd/yyyy", CultureInfo.InvariantCulture))
            //        {
            //            if (horaActual > horaAgenda)
            //            {
            //                ImgEstado.ImageUrl = "~/Botones/relojoff.png";
            //                ImgEstado.Height = 20;
            //            }
            //            else
            //            {
            //                ImgEstado.ImageUrl = "~/Botones/relojon.png";
            //                ImgEstado.Height = 20;
            //            }
            //        }
            //        else
            //        {
            //            imgSelecc.ImageUrl = "~/Botones/atencionmedicaoff.png";
            //            imgSelecc.Enabled = false;
            //            imgSelecc.Height = 20;
            //            imgAusent.Enabled = false;
            //            imgSelecc.Height = 20;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Lblerror.Text = ex.ToString();
            //}
        }

        protected void BtnAusente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigocita = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCita"].ToString();
                codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(codigoexso);
                objparam[1] = "SAU";
                objparam[2] = 155;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

                Array.Resize(ref objparam, 14);
                objparam[0] = 0;
                objparam[1] = int.Parse(codigocita);
                objparam[2] = "S";
                objparam[3] = 98;
                objparam[4] = "S";
                objparam[5] = "PACIENTE NO ASISTE";
                objparam[6] = DateTime.Now.ToString("MM/dd/yyyy");
                objparam[7] = DateTime.Now.ToString("HH:mm");
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                objparam[13] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", objparam);
                strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(strResponse, false);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnExamen_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigoexso = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoEXSO"].ToString();
                Response.Redirect("FrmAtencionExamen.aspx?CodigoEXSO=" + codigoexso, true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}