using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Medicos
{
    public partial class FrmMedicoUsuarioAdmin : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton imgAsigna = new ImageButton();
        ImageButton imgQuita = new ImageButton();
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
                lbltitulo.Text = "Asignar Usuario - Medico";
                funCargaMantenimiento();

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 2;
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
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    imgAsigna = (ImageButton)(e.Row.Cells[2].FindControl("imgAsignarUsu"));
                    imgQuita = (ImageButton)(e.Row.Cells[3].FindControl("imgDesasignaUsu"));

                    int valor = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Asignado"].ToString());
                    if (valor == 0)
                    {
                        imgAsigna.ImageUrl = "~/Botones/agregar_usuario.jpg";
                        imgAsigna.Height = 20;
                        imgAsigna.Enabled = true;
                        imgQuita.ImageUrl = "~/Botones/sin_usuario.png";
                        imgQuita.Height = 20;
                        imgQuita.Enabled = false;
                    }
                    else
                    {
                        imgAsigna.ImageUrl = "~/Botones/con_usuario.jpg";
                        imgAsigna.Height = 20;
                        imgAsigna.Enabled = false;
                        imgQuita.ImageUrl = "~/Botones/quitar_usuario.jpg";
                        imgQuita.Height = 20;
                        imgQuita.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void imgAsignarUsu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                string strCodigomedico = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
                string strCodigousu = grdvDatos.DataKeys[intIndex].Values["CodigoUsu"].ToString();
                Response.Redirect("FrmNuevoUsuarioMedico.aspx?CodigoMedico=" + strCodigomedico + "&CodigoUsuario=" + strCodigousu);
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }

        protected void imgDesasignaUsu_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                string strCodigomedico = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
                Array.Resize(ref objparam, 9);
                objparam[0] = 2;
                objparam[1] = 0;
                objparam[2] = int.Parse(strCodigomedico);
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = int.Parse(Session["usuCodigo"].ToString());
                objparam[8] = Session["MachineName"].ToString();
                dt = new Conexion(2, "2").funConsultarSqls("sp_CrearUsuarioMedico", objparam);
                string redirect = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Exito..");
                Response.Redirect(redirect);
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion
    }
}