namespace Pry_PrestasaludWAP
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Net;
    using System.Web.Helpers;
    using System.Web.UI;

    public partial class WFrm_Login : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        String gstrError = "";
        int contador = 0;
        #endregion

        #region Load

        protected void Page_Init(object sender, EventArgs e)
        {
            tkid.InnerHtml = AntiForgery.GetHtml().ToString();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    Session["usuLogin"] = "";
                    Session["usuCodigo"] = "";
                    IPHostEntry NombreHost = Dns.GetHostEntry(Request.UserHostAddress);
                    string IP = NombreHost.HostName;
                    Session["MachineName"] = IP.Substring(0, IP.IndexOf("."));
                }
                catch (Exception ex)
                {
                    Session["MachineName"] = "PC_Externa";
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private Boolean ValidarUsuario(String usuario, String password)
        {
            Boolean validado = false;
            try
            {
                //TRAER LAS POLITICAS DE LOS USUARIOS (CANTIDAD DE INTENTOS)
                Array.Resize(ref objparam, 1);
                objparam[0] = "POLITICAS USUARIO";
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                if (dt.Tables[0].Rows.Count == 0) ViewState["intentos"] = "3";
                else
                {
                    ViewState["PolicasUsuario"] = dt.Tables[0].Rows[0][1].ToString();
                    ViewState["intentos"] = dt.Tables[0].Rows[1][1].ToString();
                }
                if (!new Funciones().IsNumber(ViewState["intentos"].ToString())) ViewState["intentos"] = "3";
                //ViewState["PolicasUsuario"] = "NO";
                //ViewState["intentos"] = "3";
                Array.Resize(ref objparam, 1);
                objparam[0] = usuario;
                dt = new Conexion(2, "").funConsultarSqls("sp_UsuarioLogin", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    if (new Funciones().funDesencripta(dt.Tables[0].Rows[0][2].ToString()) == password.Trim())
                    {
                        Session["usuLogin"] = txtukyxz.Text;
                        Session["usuCodigo"] = dt.Tables[0].Rows[0]["Codigo"].ToString();
                        Session["usuPerfil"] = dt.Tables[0].Rows[0]["Perfil"].ToString();
                        Session["Clave"] = dt.Tables[0].Rows[0]["Clave"].ToString();
                        Session["usuNombres"] = dt.Tables[0].Rows[0]["Usuario"].ToString();
                        Session["Perfil"] = dt.Tables[0].Rows[0]["Descipcion"].ToString();
                        ViewState["Caduca"] = dt.Tables[0].Rows[0]["Caduca"].ToString();
                        ViewState["FechaCaduca"] = dt.Tables[0].Rows[0]["FechaCaduca"].ToString();
                        Session["CambiarPass"] = dt.Tables[0].Rows[0]["Cambiarpass"].ToString();
                        Session["CodGerencial"] = dt.Tables[0].Rows[0]["CodigoGeren"].ToString();
                        Session["PathLogo"] = dt.Tables[0].Rows[0]["PathLogo"].ToString();
                        Session["SalirAgenda"] = "SI";
                        Session["Regresar"] = "0";
                        validado = true;
                    }
                    else
                    {
                        gstrError = "Paswword Inválido, ingrese nuevamente";
                        validado = false;
                    }
                }
                else
                {
                    gstrError = "El Usuario que esta intentando ingresar al sistema no existe" + Environment.NewLine
                        + "Por favor comuníquese con el administrador.";
                    validado = false;
                }
                return validado;
            }
            catch
            {
                return false;
            }
        }

        protected void funGetTerminal()
        {
            IPHostEntry NombreHost = System.Net.Dns.GetHostEntry(Request.UserHostAddress);
            string IP = NombreHost.HostName;
            if (IP.IndexOf(".") > 0) Session["MachineName"] = IP.Substring(0, IP.IndexOf("."));
            else Session["MachineName"] = IP;
        }
        #endregion

        #region Botones y Eventos
        protected void btnIngresar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                AntiForgery.Validate();
                lblError.Text = "";
                lblmensaje.Text = "";
                if (Session["MachineName"] == null) funGetTerminal();
                if (!IsPostBack)
                {
                    Session["usuLogin"] = txtukyxz.Text;
                }
                else
                {
                    contador = Convert.ToInt32(Session["validar"]);
                    contador++;
                    Session["validar"] = contador;
                    if (ValidarUsuario(txtukyxz.Text, txtppkd.Text))
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
                        dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                        if (dt.Tables[0].Rows.Count > 0) Session["LogoC"] = dt.Tables[0].Rows[0]["Valorv"].ToString();
                        else Session["LogoC"] = "";
                        //Preguntar si en la politica especifica se valide intentos
                        if (bool.Parse(ViewState["Caduca"].ToString()))
                        {
                            if (DateTime.ParseExact(ViewState["FechaCaduca"].ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture) <=
                               DateTime.ParseExact(DateTime.Now.ToString("MM/dd/yyyy"), "MM/dd/yyyy", CultureInfo.InvariantCulture))
                            {
                                lblError.Text = "";
                                lblmensaje.Text = "Expiró Acceso al Usuario, consulte con el Administrador..!";
                                return;
                            } 
                        }
                        if (ViewState["PoliticasUsuario"] != null && ViewState["PoliticasUsuario"].ToString() == "SI")
                        {
                            Array.Resize(ref objparam, 3);
                            objparam[0] = txtukyxz.Text;
                            objparam[1] = 0;
                            objparam[2] = 0;
                            dt = new Conexion(2, "").funConsultarSqls("sp_contador", objparam);
                            if (int.Parse(dt.Tables[0].Rows[0][0].ToString()) >= int.Parse(ViewState["intentos"].ToString()))
                            {
                                lblmensaje.Text = "Usuario Bloqueado consulte con el Administrador";
                                txtukyxz.Enabled = false;
                                txtppkd.Enabled = false;
                            }
                            else
                            {
                                switch (Session["Perfil"].ToString())
                                {
                                    case "SUPERVISOR_ODONTO":
                                        Response.Redirect("~/MedicoOdonto/FrmPrincipalOdonto.aspx", true);
                                        break;
                                    case "ADMINISTRADOR PRESTADORA":
                                        Response.Redirect("~/Administrador/FrmPrincipalAdministrador.aspx", true);
                                        break;
                                    default:
                                        Response.Redirect("~/Mantenedor/FrmPrincipal.aspx", true);
                                        break;
                                }
                            }
                        }
                        else
                        {
                            switch (Session["Perfil"].ToString())
                            {
                                case "SUPERVISOR_ODONTO":
                                    Response.Redirect("~/MedicoOdonto/FrmPrincipalOdonto.aspx", true);
                                    break;
                                case "ADMINISTRADOR PRESTADORA":
                                    Response.Redirect("~/Administrador/FrmPrincipalAdministrador.aspx", true);
                                    break;
                                default:
                                    Response.Redirect("~/Mantenedor/FrmPrincipal.aspx", true);
                                    
                                    /*string postbackUrl = "Mantenedor/FrmPrincipal.aspx";
                                    Response.Clear();
                                    StringBuilder sb = new StringBuilder();
                                    sb.Append("<html>");
                                    sb.AppendFormat(@"<body onload='document.forms[""form""].submit()'>");
                                    sb.AppendFormat("<form name='form' action='{0}' method='post'>", postbackUrl);
                                    sb.AppendFormat("<input type='hidden' runat='server' name='username' value='{0}'>", txtUsuario.Text);
                                    
                                    sb.Append("</form>");
                                    sb.Append("</body>");
                                    sb.Append("</html>");

                                    Response.Write(sb.ToString());

                                    Response.End();*/
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (ViewState["PoliticasUsuario"] != null && ViewState["PoliticasUsuario"].ToString() == "SI")
                        {
                            Array.Resize(ref objparam, 3);
                            objparam[0] = txtukyxz.Text;
                            objparam[1] = contador;
                            objparam[2] = 1;
                            dt = new Conexion(2, "").funConsultarSqls("sp_contador", objparam);
                            if (int.Parse(Session["validar"].ToString()) == int.Parse(ViewState["intentos"].ToString()))
                            {
                                lblmensaje.Text = "Ha Superado el limite de Intentos consulte con el Administrador";
                                txtukyxz.Text = "";
                                txtppkd.Text = "";
                                txtukyxz.Enabled = false;
                                txtppkd.Enabled = false;
                            }
                            else
                            {
                                lblError.Text = "Password incorrecto..!";
                                txtukyxz.Text = "";
                                txtppkd.Text = "";
                            }
                        }
                        else
                        {
                            lblError.Text = "Usuario o clave incorrectos..!";
                            txtukyxz.Text = "";
                            txtppkd.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                new Funciones().funCrearLogAuditoria(1, "FrmLogin", ex.ToString(), 1);
                new Funciones().funShowJSMessage(ex.ToString(), this);
            }
        }
        #endregion
    }
}