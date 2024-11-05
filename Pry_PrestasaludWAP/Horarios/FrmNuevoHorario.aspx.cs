using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Horarios_FrmNuevoHorario : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    DataTable tbHoras = new DataTable();
    TimeSpan intervalo;
    int inter = 0;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");
        if (!IsPostBack)
        {
            tbHoras.Columns.Add("HoraInicio");
            tbHoras.Columns.Add("HoraFin");
            tbHoras.Columns.Add("Orden");

            ViewState["tblHoras"] = tbHoras;
            ViewState["CodigoHorario"] = Request["Codigo"];
            ViewState["Tipo"] = Request["Tipo"];
            funLlenarCombos();
            if (Request["Tipo"] == "N")
            {
                lbltitulo.Text = "Agregar Nuevo Horario";
            }
            else
            {
                Label7.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Horario";
                funCargaMantenimiento(int.Parse(ViewState["CodigoHorario"].ToString()));
            }
        }
    }
    #endregion

    #region Funciones y Procedimientos
    protected void funCargaMantenimiento(int codigoHorario)
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = codigoHorario;
            dt = new Conexion(2, "").funConsultarSqls("sp_HoraEditRead", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtHorario.Text = dt.Tables[0].Rows[0][0].ToString();
                txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                ddlIntervalo.SelectedValue = dt.Tables[0].Rows[0][2].ToString();
                ddlHoraIni.SelectedValue = dt.Tables[0].Rows[0][3].ToString().Substring(0, 2);
                ddlMinutoIni.SelectedValue = dt.Tables[0].Rows[0][3].ToString().Substring(3, 2);
                ddlHoraFin.SelectedValue = dt.Tables[0].Rows[0][4].ToString().Substring(0, 2);
                ddlMinutoFin.SelectedValue = dt.Tables[0].Rows[0][4].ToString().Substring(3, 2);
                chkEstado.Text = dt.Tables[0].Rows[0][5].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][5].ToString() == "Activo" ? true : false;
            }
            if (dt.Tables[1].Rows.Count > 0)
            {
                DataTable dtHoras = dt.Tables[1];
                ViewState["tblHoras"] = dtHoras;
                grdvDatos.DataSource = dtHoras;
                grdvDatos.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    private void funLlenarCombos()
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = "INTERVALO TURNOS";
            ddlIntervalo.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam).Tables[0];
            ddlIntervalo.DataTextField = "Descripcion";
            ddlIntervalo.DataValueField = "Valor";
            ddlIntervalo.DataBind();

            new Funciones().funCargarComboHoraMinutos(ddlHoraIni, "HORAS");
            new Funciones().funCargarComboHoraMinutos(ddlHoraFin, "HORAS");
            new Funciones().funCargarComboHoraMinutos(ddlMinutoIni, "MINUTOS");
            new Funciones().funCargarComboHoraMinutos(ddlMinutoFin, "MINUTOS");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    private void funCargarHorasMinutos(string HoraInicial, string HoraFinal, int orden)
    {
        try
        {
            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblHoras"];
            DataRow filagre = tblagre.NewRow();
            filagre["HoraInicio"] = HoraInicial;
            filagre["HoraFin"] = HoraFinal;
            filagre["Orden"] = orden;
            tblagre.Rows.Add(filagre);
            ViewState["tblHoras"] = tblagre;
            grdvDatos.DataSource = tblagre;
            grdvDatos.DataBind();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Botones y Eventos
    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked ? "Activo" : "Inactivo";
    }
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(txtHorario.Text.Trim()))
        {
            lblerror.Text = "Ingrese Nombre del Horario..!";
            return;
        }
        if (grdvDatos.Rows.Count == 0)
        {
            lblerror.Text = "Procese horarios..!";
            return;
        }
        try
        {
            Array.Resize(ref objparam, 10);
            objparam[0] = int.Parse(ViewState["CodigoHorario"].ToString());
            objparam[1] = txtHorario.Text.ToUpper();
            objparam[2] = txtDescripcion.Text.ToUpper();
            objparam[3] = ddlIntervalo.SelectedValue;
            objparam[4] = ddlHoraIni.SelectedItem.ToString() + ":" + ddlMinutoIni.SelectedItem.ToString(); //+ ":" + "00";
            objparam[5] = ddlHoraFin.SelectedItem.ToString() + ":" + ddlMinutoFin.SelectedItem.ToString(); //+ ":" + "00";
            objparam[6] = chkEstado.Checked;
            objparam[7] = int.Parse(Session["usuCodigo"].ToString());
            objparam[8] = Session["MachineName"].ToString();
            objparam[9] = (DataTable)ViewState["tblHoras"];
            dt = new Conexion(2, "").FunInsertDatatableSQL(objparam, (DataTable)ViewState["tblHoras"]);
            if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmHorarioAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmHorarioadmin.aspx");
    }
    protected void imgProcesar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            TimeSpan horaInicio = TimeSpan.Parse(ddlHoraIni.SelectedItem.ToString() + ":" + ddlMinutoIni.SelectedItem.ToString());
            TimeSpan horaFin = TimeSpan.Parse(ddlHoraFin.SelectedItem.ToString() + ":" + ddlMinutoFin.SelectedItem.ToString());
            if (horaInicio > horaFin)
            {
                lblerror.Text = "La Hora inicio no puede ser menor a la Hora fin..!";
                return;
            }
            if (horaInicio == horaFin)
            {
                lblerror.Text = "La Hora inicio y la Hora fin son iguales..!";
                return;
            }
            intervalo = horaFin - horaInicio;
            inter = (int)intervalo.TotalMinutes;
            if (int.Parse(ddlIntervalo.SelectedValue) > inter)
            {
                lblerror.Text = "El intervalo entre las horas es menor al valor del Intervalo";
                return;
            }
            tbHoras.Columns.Add("HoraInicio");
            tbHoras.Columns.Add("HoraFin");
            tbHoras.Columns.Add("Orden");
            ViewState["tblHoras"] = tbHoras;
            grdvDatos.DataSource = tbHoras;
            grdvDatos.DataBind();
            TimeSpan intervalMinu = TimeSpan.FromMinutes(int.Parse(ddlIntervalo.SelectedValue));
            string strHoraInicial, strHoraFinal = "";
            funCargarHorasMinutos(horaInicio.ToString(@"hh\:mm"), horaInicio.Add(intervalMinu).ToString(@"hh\:mm"), 0);
            horaInicio = horaInicio.Add(intervalMinu);
            int orden = 0;
            while (horaInicio != horaFin)
            {
                if (horaInicio > horaFin)
                {
                    horaInicio = horaFin;
                    ddlMinutoFin.SelectedValue = strHoraFinal.Substring(3, 2);
                }
                else
                {
                    intervalo = horaFin - horaInicio;
                    inter = (int)intervalo.TotalMinutes;
                    if (int.Parse(ddlIntervalo.SelectedValue) <= inter)
                    {
                        strHoraInicial = horaInicio.ToString(@"hh\:mm");
                        horaInicio = horaInicio.Add(intervalMinu);
                        strHoraFinal = horaInicio.ToString(@"hh\:mm");
                        orden++;
                        funCargarHorasMinutos(strHoraInicial, strHoraFinal, orden);
                    }
                    else horaInicio = horaInicio.Add(intervalMinu);

                }                
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void ddlIntervalo_SelectedIndexChanged(object sender, EventArgs e)
    {
        tbHoras.Columns.Add("HoraInicio");
        tbHoras.Columns.Add("HoraFin");
        tbHoras.Columns.Add("Orden");
        ViewState["tblHoras"] = tbHoras;
        grdvDatos.DataSource = tbHoras;
        grdvDatos.DataBind();
    }
    #endregion
}