using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Horarios_FrmNuevoTurno : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    DataTable tbTurno = new DataTable();
    int maxCodigo = 0;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");

        if (!IsPostBack)
        {
            tbTurno.Columns.Add("Dia");
            tbTurno.Columns.Add("HoraInicio");
            tbTurno.Columns.Add("HoraFin");
            tbTurno.Columns.Add("Intervalo");
            tbTurno.Columns.Add("Estado");
            tbTurno.Columns.Add("Codigo");
            tbTurno.Columns.Add("CodigoDia");
            tbTurno.Columns.Add("CodigoHora");

            ViewState["tblTurno"] = tbTurno;
            ViewState["CodigoTurno"] = Request["Codigo"];
            ViewState["Tipo"] = Request["Tipo"];
            funLlenarCombos();
            if (Request["Tipo"] == "N")
            {
                lbltitulo.Text = "Agregar Nuevo Turno";
            }
            else
            {
                Label7.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Turno";
                funCargaMantenimiento(int.Parse(ViewState["CodigoTurno"].ToString()));
            }
        }
    }
    #endregion

    #region Funciones y Procedimientos
    protected void funCargaMantenimiento(int codigoTurno)
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = codigoTurno;
            dt = new Conexion(2, "").funConsultarSqls("sp_CargarTurnoEdit", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtTurno.Text = dt.Tables[0].Rows[0][0].ToString();
                txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
            }
            if (dt.Tables[1].Rows.Count > 0)
            {
                DataTable dtTurno = dt.Tables[1];
                ViewState["tblTurno"] = dtTurno;
                grdvDatos.DataSource = dtTurno;
                grdvDatos.DataBind();
            }
        }
        catch (Exception ex)
        {
            //StackTrace st = new StackTrace(ex, true);
            //StackFrame frame = st.GetFrame(0);
            //new Funciones().funCrearLogAuditoria(int.Parse(Session["usuCodigo"].ToString()), "frmNuevoTurno", ex.ToString(), frame.GetFileLineNumber());
            lblerror.Text = ex.ToString();
        }
    }
    private void funLlenarCombos()
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = "DIAS SEMANA";
            ddlDia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam).Tables[0];
            ddlDia.DataTextField = "Descripcion";
            ddlDia.DataValueField = "Valor";
            ddlDia.DataBind();

            objparam[0] = 4;
            ddlHorario.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam).Tables[0];
            ddlHorario.DataTextField = "Descripcion";
            ddlHorario.DataValueField = "Codigo";
            ddlHorario.DataBind();
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
        try
        {
            if (grdvDatos.Rows.Count == 0)
            {
                lblerror.Text = "Agregue Turnos por favor..!";
                return;
            }
            Array.Resize(ref objparam, 6);
            objparam[0] = int.Parse(ViewState["CodigoTurno"].ToString());
            objparam[1] = txtTurno.Text.ToUpper();
            objparam[2] = txtDescripcion.Text.ToUpper();
            objparam[3] = chkEstado.Checked;
            objparam[4] = int.Parse(Session["usuCodigo"].ToString());
            objparam[5] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").FunInsertarTurnos(objparam, (DataTable)ViewState["tblTurno"]);
            if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmTurnoAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmTurnoAdmin.aspx");
    }
    protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
    {
        bool lexiste = false;
        lblerror.Text = "";
        try
        {
            if (ViewState["tblTurno"] != null)
            {
                DataTable tblbuscar = (DataTable)ViewState["tblTurno"];
                DataRow result = tblbuscar.Select("Dia='" + ddlDia.SelectedValue + "'").FirstOrDefault();
                tblbuscar.DefaultView.Sort = "Codigo";
                if (result != null) lexiste = true;                
                foreach (DataRow dr in tblbuscar.Rows)
                {
                    maxCodigo = int.Parse(dr[5].ToString());
                }
            }

            if (lexiste)
            {
                lblerror.Text = "Ya existe definido Día para el turno..!";
                return;
            }
            Array.Resize(ref objparam, 3);
            objparam[0] = ddlHorario.SelectedValue;
            objparam[1] = "";
            objparam[2] = 0;
            dt = new Conexion(2,"").funConsultarSqls("sp_ConsultaDatos", objparam);
            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblTurno"];
            DataRow filagre = tblagre.NewRow();
            filagre["Dia"] = ddlDia.SelectedItem.ToString();
            filagre["HoraInicio"] = dt.Tables[0].Rows[0]["HoraInicio"].ToString();
            filagre["HoraFin"] = dt.Tables[0].Rows[0]["HoraFin"].ToString();
            filagre["Intervalo"] = dt.Tables[0].Rows[0]["Intervalo"].ToString(); ;
            filagre["Estado"] = "Activo";
            filagre["Codigo"] = maxCodigo + 1;
            filagre["CodigoDia"] = ddlDia.SelectedValue;
            filagre["CodigoHora"] = ddlHorario.SelectedValue;
            tblagre.Rows.Add(filagre);
            tblagre.DefaultView.Sort = "Dia";
            ViewState["tblTurno"] = tblagre;
            grdvDatos.DataSource = tblagre;
            grdvDatos.DataBind();
            imgCancelar.Enabled = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Label lblest = new Label();
        CheckBox chkest = new CheckBox();
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                chkest = (CheckBox)(e.Row.Cells[4].FindControl("chkEstadoDet"));
                lblest = (Label)(e.Row.Cells[5].FindControl("lblEstado"));

                int codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = "";
                objparam[2] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    chkest.Checked = dt.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    lblest.Text = dt.Tables[0].Rows[0]["Estado"].ToString();
                }
                else
                {
                    chkest.Enabled = false;
                    chkest.Checked = true;
                    lblest.Text = "Activo";
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void chkEstadoDet_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;

        Label lblest = new Label();
        CheckBox chkest = new CheckBox();

        chkest = (CheckBox)(gvRow.Cells[4].FindControl("chkEstadoDet"));
        lblest = (Label)(gvRow.Cells[5].FindControl("lblEstado"));
        lblest.Text = chkest.Checked ? "Activo" : "Inactivo";
        tbTurno = (DataTable)ViewState["tblTurno"];
        int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
        DataRow[] result = tbTurno.Select("Codigo=" + codigo);
        result[0]["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
        tbTurno.AcceptChanges();

    }
    protected void imgSelecc_Click(object sender, ImageClickEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int intIndex = gvRow.RowIndex;
        ViewState["index"] = intIndex;
        ViewState["CodigoDetalle"] = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
        ViewState["CodigoDia"] = grdvDatos.DataKeys[intIndex].Values["CodigoDia"].ToString();
        ViewState["CodigoHora"] = grdvDatos.DataKeys[intIndex].Values["CodigoHora"].ToString();
        ddlDia.SelectedValue = grdvDatos.DataKeys[intIndex].Values["CodigoDia"].ToString();
        ddlHorario.SelectedValue = grdvDatos.DataKeys[intIndex].Values["CodigoHora"].ToString();
        ViewState["Dia"] = grdvDatos.Rows[intIndex].Cells[0].Text;
        imgModificar.Enabled = true;
        imgAgregar.Enabled = false;
        imgCancelar.Enabled = true;
        foreach (GridViewRow row in grdvDatos.Rows)
        {
            grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
        }
        grdvDatos.Rows[intIndex].BackColor = System.Drawing.Color.DarkGray;
    }
    protected void imgModificar_Click(object sender, ImageClickEventArgs e)
    {
        bool lexiste = false;
        lblerror.Text = "";
        try
        {
            if (ViewState["tblTurno"] != null)
            {
                DataTable tblbuscar = (DataTable)ViewState["tblTurno"];
                if (ViewState["Dia"].ToString() != ddlDia.SelectedItem.ToString())
                {
                    DataRow[] result = tblbuscar.Select("Dia='" + ddlDia.SelectedItem.ToString() + "'");
                    lexiste = result.Length == 0 ? false : true;
                }
            }
            if (lexiste)
            {
                lblerror.Text = "Ya existe definido Día para el turno..!";
                return;
            }
            Array.Resize(ref objparam, 3);
            objparam[0] = ddlHorario.SelectedValue;
            objparam[1] = "";
            objparam[2] = 0;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);

            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblTurno"];
            tblagre.Rows.RemoveAt(int.Parse(ViewState["index"].ToString()));
            DataRow filagre = tblagre.NewRow();
            filagre["Dia"] = ddlDia.SelectedItem.ToString();
            filagre["HoraInicio"] = dt.Tables[0].Rows[0]["HoraInicio"].ToString();
            filagre["HoraFin"] = dt.Tables[0].Rows[0]["HoraFin"].ToString();
            filagre["Intervalo"] = dt.Tables[0].Rows[0]["Intervalo"].ToString();
            filagre["Estado"] = "Activo";
            filagre["Codigo"] = int.Parse(ViewState["CodigoDetalle"].ToString());
            filagre["CodigoDia"] = ddlDia.SelectedValue;
            filagre["CodigoHora"] = ddlHorario.SelectedValue;            
            tblagre.Rows.Add(filagre);
            tblagre.DefaultView.Sort = "Dia";
            ViewState["tblTurno"] = tblagre;
            grdvDatos.DataSource = tblagre;
            grdvDatos.DataBind();
            foreach (GridViewRow row in grdvDatos.Rows)
            {
                grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
            }
            ddlDia.SelectedIndex = 0;
            ddlHorario.SelectedIndex = 0;
            imgModificar.Enabled = false;
            imgCancelar.Enabled = false;
            imgAgregar.Enabled = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
    {
        ddlDia.SelectedIndex = 0;
        ddlHorario.SelectedIndex = 0;
        imgAgregar.Enabled = true;
        imgCancelar.Enabled = false;
        imgModificar.Enabled = false;
        tbTurno.Columns.Add("Dia");
        tbTurno.Columns.Add("HoraInicio");
        tbTurno.Columns.Add("HoraFin");
        tbTurno.Columns.Add("Intervalo");
        tbTurno.Columns.Add("Codigo");
        tbTurno.Columns.Add("CodigoDia");
        tbTurno.Columns.Add("CodigoHora");
        ViewState["tblTurno"] = tbTurno;
        grdvDatos.DataSource = null;
        grdvDatos.DataBind();
        funCargaMantenimiento(int.Parse(ViewState["CodigoTurno"].ToString()));
    }
    #endregion
}