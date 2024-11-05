using System;

public partial class Mantenedor_FrmPrincipal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                    Response.Redirect("~/Reload.html");
                
            }
            catch (Exception)
            {
                Response.Redirect("~/Reload.html");
            }
        }
        else
        {
            Response.Redirect("~/Reload.html");
        }
    }
}