using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaOdontologica
{
    public partial class FrmDatosPrestaOdonto : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["CodigoPrestadora"] = Request["CodigoPrestadora"];
                funCargarMantenimiento();
            }
        }
        #endregion

        #region Funciones y Procedimientos
        private void funCargarMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                objparam[1] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarPrestadoraEdit", objparam);
                lbltitulo.Text = "Prestadora: " + dt.Tables[0].Rows[0][0].ToString();
                lblDireccion.InnerText = dt.Tables[0].Rows[0][1].ToString();
                lblTelefono1.InnerText = dt.Tables[0].Rows[0][2].ToString();
                lblTelefono2.InnerText = dt.Tables[0].Rows[0][3].ToString();
                lblTelefono3.InnerText = dt.Tables[0].Rows[0][4].ToString();
                lblCelular.InnerText = dt.Tables[0].Rows[0][5].ToString();
                lblFax.InnerText = dt.Tables[0].Rows[0][6].ToString();
                lblEmail1.InnerText = dt.Tables[0].Rows[0][7].ToString();
                lblEmail2.InnerText = dt.Tables[0].Rows[0][8].ToString();
                lblEmail3.InnerText = dt.Tables[0].Rows[0][9].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
        #endregion
    }
}