using System;
using System.Data;

namespace Pry_PrestasaludWAP.Medicos
{
    public partial class FrmDetalleObservaGestiones : System.Web.UI.Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["Codigo"] = Request["Codigo"];
                lbltitulo.Text = "OBSERVACION REGISTRADA";
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
                objparam[0] = int.Parse(ViewState["Codigo"].ToString());
                objparam[1] = "";
                objparam[2] = 121;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                Repeater1.DataSource = dts;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}