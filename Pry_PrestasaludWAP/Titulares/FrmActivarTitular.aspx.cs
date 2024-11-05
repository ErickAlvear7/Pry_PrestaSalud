using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Titulares
{
    public partial class FrmActivarTitular : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        CheckBox chkS = new CheckBox();
        Label lblE = new Label();
        string estado = "";
        int codigotitular = 0, codigoproducto = 0, nuevoestado = 0;
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
                lbltitulo.Text = "Administración Titulares (Activar - Inactivar)";
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
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = txtCriterio.Text.Trim().ToUpper();
                objparam[2] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCitaAdmin", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["grdvDatos"] = dt;
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
                    chkS = (CheckBox)(e.Row.Cells[4].FindControl("chkSelecc"));
                    lblE = (Label)(e.Row.Cells[5].FindControl("lblEstado"));
                    estado = grdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    lblE.Text = estado;
                    if (estado == "Activo") chkS.Checked = true;
                    else chkS.Checked = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCriterio.Text)) funCargaMantenimiento();
        }

        protected void chkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                chkS = (CheckBox)(gvRow.Cells[4].FindControl("chkSelecc"));
                lblE = (Label)(gvRow.Cells[5].FindControl("lblEstado"));
                codigotitular = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                codigoproducto = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoProducto"].ToString());
                Array.Resize(ref objparam, 4);
                objparam[0] = 5;
                objparam[1] = codigotitular;
                objparam[2] = chkS.Checked ? 1 : 0;
                objparam[3] = codigoproducto;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularEdit", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                    lblE.Text = chkS.Checked ? "Activo" : "Inactivo";
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
               
            }
        }
        #endregion
    }
}