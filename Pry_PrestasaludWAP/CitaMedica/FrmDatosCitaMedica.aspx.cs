using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmDatosCitaMedica : System.Web.UI.Page
    {

        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int codigoCita = int.Parse(Request["CodigoCita"]);
                funCargarMantenimiento(codigoCita);
            }

        }

        #region Funciones y Procedimientos
        private void funCargarMantenimiento(int cod)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = cod;
                objparam[1] = "";
                objparam[2] = 172;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lblCodCita.Text = dt.Tables[0].Rows[0][1].ToString();
                lblCiudad.Text = dt.Tables[0].Rows[0][2].ToString();
                lblFechaCita.Text = dt.Tables[0].Rows[0][3].ToString();
                lblHoraCita.Text = dt.Tables[0].Rows[0][4].ToString();
                lblPrestadora.Text = dt.Tables[0].Rows[0][5].ToString();
                lblMedico.Text = dt.Tables[0].Rows[0][6].ToString();
                lblEspecialidad.Text = dt.Tables[0].Rows[0][7].ToString();
                lblDetalle.Text = dt.Tables[0].Rows[0][8].ToString();
                lblCedula.Text = dt.Tables[0].Rows[0][9].ToString();
                lblTipo.Text = dt.Tables[0].Rows[0][10].ToString();
                lblPaciente.Text = dt.Tables[0].Rows[0][11].ToString();
                lblFechaNaci.Text = dt.Tables[0].Rows[0][12].ToString();
                lblDirec.Text = dt.Tables[0].Rows[0][13].ToString();
                lblTelefono.Text = dt.Tables[0].Rows[0][14].ToString();
                lblObserva.Text = dt.Tables[0].Rows[0][15].ToString();
            }
            catch (Exception ex)
            {
                //lblerror.Text = ex.ToString();
            }
        }
        #endregion

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }
    }
}