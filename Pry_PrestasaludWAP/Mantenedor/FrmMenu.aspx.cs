using System;
using System.Data;
using System.Web.UI;

public partial class Mantenedor_FrmMenu : Page
{

    #region Variables
    SegPrincipal objfun = new SegPrincipal();
    DataSet dts = new DataSet();
    Object[] objparam = new Object[11];
    string mensaje = "";
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");

        if (!IsPostBack)
        {
            dts = new Conexion(2, "").SIWebseg(int.Parse(Session["usuCodigo"].ToString()));
            if (dts.Tables[0].Rows.Count > 0)
            {
                objfun.crea_menuweb(dts.Tables[0], ref trvmenu);
                trvmenu.ExpandAll();
            }
            Array.Resize(ref objparam, 11);
            objparam[0] = 4;
            objparam[1] = "LOGOSI";
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
            if (dts.Tables[0].Rows.Count > 0) imgLog.ImageUrl = dts.Tables[0].Rows[0]["Valorv"].ToString();
            LblUsuario.Text = Session["usuNombres"].ToString();
        }
    }
    #endregion

    #region Botones y Eventos
    protected void trvmenu_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            if (trvmenu.SelectedNode.NavigateUrl == "")
            {
                trvmenu.CollapseAll();
                trvmenu.SelectedNode.Expand();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        if (Session["SalirAgenda"].ToString() == "NO")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Existe una reserva, realice el agendamiento para poder salir..!');", true);
            return;
        }
        ScriptManager.RegisterStartupScript(this.Page, GetType(), "code", "salir();", true);
    }

    protected void TmrMenu_Tick(object sender, EventArgs e)
    {
        try
        {
            Array.Resize(ref objparam, 11);
            objparam[0] = 29;
            objparam[1] = "";
            objparam[2] = "";
            objparam[3] = "";
            objparam[4] = "";
            objparam[5] = "";
            objparam[6] = int.Parse(Session["usuCodigo"].ToString());
            objparam[7] = 0;
            objparam[8] = 0;
            objparam[9] = 0;
            objparam[10] = 0;
            switch (Session["usuPerfil"].ToString())
            {
                case "3":
                    objparam[2] = "EXR";
                    mensaje = "Tiene Exámenes Cargados Pendientes Por Revisar..!";
                    break;
                case "9":
                    objparam[2] = "SGA";
                    mensaje = "Revisar Exámenes Agendados o Por Corregir..!";
                    break;
                case "14":
                    objparam[2] = "SRR";
                    mensaje = "Tiene Solicitud de Exámenes Realizadas..!";
                    break;
                case "17":
                    objparam[2] = "EPR";
                    mensaje = "Exámenes Finalizados Pendientes de Auditar..!";
                    break;
                default:
                    objparam[2] = "NAD";
                    break;
            }
            dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
            if (dts.Tables[0].Rows.Count > 0)
                SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", mensaje);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion
}