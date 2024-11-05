namespace Pry_PrestasaludWAP.Configuracion
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmCrearUserAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        string _codigouser = "", _estado = "", _codigoperm = "";
        CheckBox _chkestado = new CheckBox();
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                Lbltitulo.Text = "Configurar Usuario-Administrador PRESTADORAS";
                FunCargaMantenimiento();
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
        } 
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = 0;
            objparam[1] = "";
            objparam[2] = 162;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                GrdvDatos.DataSource = dt;
                GrdvDatos.DataBind();
                GrdvDatos.UseAccessibleHeader = true;
                GrdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmNuevoUserAdmin.aspx?CodigoUSER=0&CodigoPERM=0", true);
        }

        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigouser = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERM"].ToString();
                _chkestado = (CheckBox)(gvRow.Cells[1].FindControl("ChkEstado"));
                Array.Resize(ref objparam, 3);
                objparam[0] = _codigouser;
                objparam[1] = _chkestado.Checked ? "Activo" : "Inactivo";
                objparam[2] = 163;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
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
                    _chkestado = (CheckBox)(e.Row.Cells[2].FindControl("ChkEstado"));
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();

                    if (_estado == "Activo")
                    {
                        _chkestado.Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void Btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            _codigouser = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoUSER"].ToString();
            _codigoperm = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPERM"].ToString();
            Response.Redirect("FrmNuevoUserAdmin.aspx?CodigoUSER=" + _codigouser + "&CodigoPERM=" + _codigoperm, true);
        } 
        #endregion
    }
}