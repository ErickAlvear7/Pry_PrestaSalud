using System;

namespace Pry_PrestasaludWAP.MedicoOdonto
{
    public partial class FrmDetalleOdonto : System.Web.UI.Page
    {
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblUsuario.Text = Session["usuNombres"].ToString();
                    if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                        Response.Redirect("~/Reload.html");
                }
                catch (Exception)
                {
                    Response.Redirect("~/Reload.html");
                }
            }
        }
        #endregion
    }
}