namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;

    public partial class FrmDetalleAdministrador : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[11];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    lblUsuario.Text = Session["usuNombres"].ToString();
                    if (!string.IsNullOrEmpty(Session["PathLogo"].ToString())) ImgLogo.ImageUrl = Session["PathLogo"].ToString();
                    else
                    {
                        Array.Resize(ref objparam, 11);
                        objparam[0] = 4;
                        objparam[1] = "LOGOBV";
                        objparam[2] = "PATH LOGOS";
                        objparam[3] = "";
                        objparam[4] = "";
                        objparam[5] = "";
                        objparam[6] = 0;
                        objparam[7] = 0;
                        objparam[8] = 0;
                        objparam[9] = 0;
                        objparam[10] = 0;
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                        if (dts.Tables[0].Rows.Count > 0) ImgLogo.ImageUrl = dts.Tables[0].Rows[0]["Valorv"].ToString();
                    }
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