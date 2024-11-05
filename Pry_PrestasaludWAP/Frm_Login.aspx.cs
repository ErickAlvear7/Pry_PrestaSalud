using System;
using System.Data;
using System.Net;

public partial class Frm_Login : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    String gstrError = "";
    int contador = 0;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //IPHostEntry hostEntry = Dns.GetHostByName(Request.UserHostAddress);
                //Session["IPLocalAdress"] = hostEntry.HostName;
                //Session["usuLogin"] = "";
                //Session["usuCodigo"] = "";
                //IPHostEntry NombreHost = System.Net.Dns.GetHostEntry(Request.UserHostAddress);
                //try
                //{
                //    Session["MachineName"] = NombreHost.HostName.Substring(0, hostEntry.HostName.IndexOf("."));
                //}
                //catch (Exception ex)
                //{
                //    Session["MachineName"] = NombreHost.HostName;
                //}
                Session["usuLogin"] = "";
                Session["usuCodigo"] = "";
                IPHostEntry NombreHost = System.Net.Dns.GetHostEntry(Request.UserHostAddress);
                string IP = NombreHost.HostName;
                Session["MachineName"] = IP.Substring(0, IP.IndexOf("."));
            }
            catch(Exception ex)
            {
                //IPHostEntry NombreHost = System.Net.Dns.GetHostEntry(Request.UserHostAddress);
                Session["MachineName"] = "PC_Externa";
                //throw ex;
            }
        }
    }
    #endregion

    #region Botones y Eventos
    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["MachineName"] == null) funGetTerminal();
            if (!IsPostBack)
            {
                Session["usuLogin"] = txtUsuario.Text;
            }
            else
            {
                contador = Convert.ToInt32(Session["validar"]);
                contador++;
                Session["validar"] = contador;
                if (ValidarUsuario(txtUsuario.Text, txtClave.Text))
                {
                    //Preguntar si en la politica especifica se valide intentos
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
                    if (Session["PoliticasUsuario"] != null && Session["PoliticasUsuario"].ToString() == "SI")
                    {
                        Array.Resize(ref objparam, 3);
                        objparam[0] = txtUsuario.Text;
                        objparam[1] = 0;
                        objparam[2] = 0;
                        dt = new Conexion(2, "").funConsultarSqls("sp_contador", objparam);
                        if (int.Parse(dt.Tables[0].Rows[0][0].ToString()) >= int.Parse(Session["intentos"].ToString()))
                        {
                            lblmensaje.Text = "Usuario Bloqueado consulte con el Administrador";
                            txtUsuario.Enabled = false;
                            txtClave.Enabled = false;
                        }
                        else
                        {
                            if (Session["Perfil"].ToString() == "SUPERVISOR_ODONTO") Response.Redirect("~/MedicoOdonto/FrmPrincipalOdonto.aspx", true);
                            else Response.Redirect("~/Mantenedor/FrmPrincipal.aspx", true);
                        }
                    }
                    else
                    {
                        if (Session["Perfil"].ToString() == "SUPERVISOR_ODONTO") Response.Redirect("~/MedicoOdonto/FrmPrincipalOdonto.aspx", true);
                        else Response.Redirect("~/Mantenedor/FrmPrincipal.aspx", true);
                    }
                }
                else
                {
                    if (Session["PoliticasUsuario"] != null && Session["PoliticasUsuario"].ToString() == "SI")
                    {
                        Array.Resize(ref objparam, 3);
                        objparam[0] = txtUsuario.Text;
                        objparam[1] = contador;
                        objparam[2] = 1;
                        dt = new Conexion(2, "").funConsultarSqls("sp_contador", objparam);
                        if (int.Parse(Session["validar"].ToString()) == int.Parse(Session["intentos"].ToString()))
                        {
                            lblmensaje.Text = "Ha Superado el limite de Intentos consulte con el Administrador";
                            txtUsuario.Text = "";
                            txtClave.Text = "";
                            txtUsuario.Enabled = false;
                            txtClave.Enabled = false;
                        }
                        else
                        {
                            lblError.Text = "Password incorrecto..!";
                            txtUsuario.Text = "";
                            txtClave.Text = "";
                        }
                    }
                    else
                    {
                        lblError.Text = "Usuario o clave incorrectos..!";
                        txtUsuario.Text = "";
                        txtClave.Text = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = ex.ToString();
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
            //Array.Resize(ref objparam, 2);
            //objparam[0] = "5";
            //objparam[1] = "1";
            //dt = new Conexion(2, "").funConsultarSqls("spCargaTotalParaDeta", objparam);
            //if (dt.Tables[0].Rows.Count == 0) Session["intentos"] = "3";
            //else Session["intentos"] = dt.Tables[0].Rows[0][0].ToString();
            //if (!new Funciones().IsNumber(Session["intentos"].ToString())) Session["intentos"] = "3";
            Session["PolicasUsuario"] = "NO";
            Session["intentos"] = "3";
            Array.Resize(ref objparam, 1);
            objparam[0] = usuario;
            dt = new Conexion(2, "").funConsultarSqls("sp_UsuarioLogin", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                if (new Funciones().funDesencripta(dt.Tables[0].Rows[0][2].ToString()) == password.Trim())
                {
                    Session["usuLogin"] = txtUsuario.Text;
                    Session["usuCodigo"] = dt.Tables[0].Rows[0][0].ToString();
                    Session["usuPerfil"] = dt.Tables[0].Rows[0][1].ToString();
                    Session["usuNombres"] = dt.Tables[0].Rows[0][3].ToString();
                    Session["Perfil"] = dt.Tables[0].Rows[0][4].ToString();
                    Session["CodGerencial"] = dt.Tables[0].Rows[0][5].ToString();
                    Session["PathLogo"] = dt.Tables[0].Rows[0][6].ToString();
                    Session["SalirAgenda"] = "SI";
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
}
