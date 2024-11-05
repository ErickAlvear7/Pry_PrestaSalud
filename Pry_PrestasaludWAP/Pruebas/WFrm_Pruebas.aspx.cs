using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Pruebas
{
    public partial class WFrm_Pruebas : System.Web.UI.Page
    {
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = 8414;
                objparam[2] = txtEditor.Content;
                dt = new Conexion(2, "").FunInsertTextEditor(objparam);
            }
            catch (Exception ex)
            {
                Response.Write(ex.ToString());
            }
        }
    }
}