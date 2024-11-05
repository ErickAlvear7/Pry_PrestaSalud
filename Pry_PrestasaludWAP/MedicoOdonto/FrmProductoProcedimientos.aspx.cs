using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.MedicoOdonto
{
    public partial class FrmProductoProcedimientos : System.Web.UI.Page
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
                ViewState["CodigoCita"] = Request["CodigoCita"];
                funCargaMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[1] = "";
                objparam[2] = 80;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatos.DataSource = dt.Tables[0];
                grdvDatos.DataBind();
                //if (dt.Tables[0].Rows.Count > 0)
                //{
                //    grdvDatos.DataSource = dt.Tables[0];
                //    grdvDatos.DataBind();
                //    grdvDatos.UseAccessibleHeader = true;
                //    grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                //}
                lbltitulo.Text = "PROCEDIMIENTOS - COBERTURA >>" + dt.Tables[1].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
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