using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaOdontologica
{
    public partial class FrmDatosOdontoHorarios : System.Web.UI.Page
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
                ViewState["CodigoMedico"] = Request["CodigoMedico"];
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
                objparam[0] = 14;
                objparam[1] = int.Parse(ViewState["CodigoMedico"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt.Tables[0].Rows.Count > 0) lbltitulo.Text = dt.Tables[0].Rows[0][0].ToString();
                if (dt.Tables[1].Rows.Count > 0)
                {
                    DataTable dtTurno = dt.Tables[1];
                    grdvDatos.DataSource = dtTurno;
                    grdvDatos.DataBind();
                }
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