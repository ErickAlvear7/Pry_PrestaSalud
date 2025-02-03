using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pry_PrestasaludWAP.Api;
using System.Data;
using Pry_PrestasaludWAP.Modelo;
using static Pry_PrestasaludWAP.Modelo.MediLinkModel;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmAgendarMediLink : System.Web.UI.Page
    {

        string accessToken = "";
        string idtitular = "";
        string user = "";
        string pass = "";
        Object[] objparam = new Object[1];
        DataSet dt = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {

            idtitular = Request["CodigoTitular"];
            //GET PARAMETROS USUARIO Y PASSWORD MEDILINK
            objparam[0] = 61;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
            if(dt != null && dt.Tables[0].Rows.Count > 0)
            {
                user = dt.Tables[0].Rows[0][1].ToString().Trim();
                pass = dt.Tables[0].Rows[1][1].ToString().Trim();
            }

            var login = new LoginApi
           {
               username = user,
               password = pass
            };

            var data = new JavaScriptSerializer().Serialize(login);
            accessToken = new MediLinkApi().PostAccesLogin("https://testagendamiento.medilink.com.ec/", data);


        }

        //GET CEDULA TITULAR VERIFICAR SI EXISTE EN BDD MEDIKINK





    }
}