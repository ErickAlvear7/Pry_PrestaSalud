using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaOdontologica
{
    public partial class FrmCitaOdontoAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataSet _dts = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton btnselec = new ImageButton();
        string _token = "", _urltoken = "", _urldatos = "", _datositems = "", _fonocasa = "", _fonooficina = "", _celular = "",
            _fechaini = "", _fechafin = "";
        string[] _telefonos, _fechavigencia;
        bool _produccion = true;
        int _idproducto = 0, _titucodigo = 0, _fila = 0, _posicion = 0;
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
                lbltitulo.Text = "Administración Titulares (Agendamiento Odontológico)";
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
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCriterio.Text))
            {
                funCargaMantenimiento();
            }
        }
        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            var strCodigo = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            var strCodProducto = grdvDatos.DataKeys[intIndex].Values["CodigoProducto"].ToString();
            Response.Redirect("FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + strCodigo + "&CodigoProducto=" + strCodProducto);
        }
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    btnselec = (ImageButton)(e.Row.Cells[5].FindControl("btnselecc"));
                    string estado = e.Row.Cells[4].Text;
                    if (estado == "Inactivo")
                    {
                        btnselec.ImageUrl = "~/Botones/citamedicadesac.png";
                        btnselec.Height = 20;
                        btnselec.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}