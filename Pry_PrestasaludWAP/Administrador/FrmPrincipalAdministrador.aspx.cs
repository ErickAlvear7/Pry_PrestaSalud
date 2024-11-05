using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Administrador
{
    public partial class FrmPrincipalAdministrador : Page
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
}