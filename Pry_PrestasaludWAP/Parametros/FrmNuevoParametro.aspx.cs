using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Parametros_FrmNuevoParametro : System.Web.UI.Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    DataTable tbParametro = new DataTable();
    DataRow resultado;
    ImageButton imgSubir = new ImageButton();
    ImageButton imgBajar = new ImageButton();
    int orden = 0, maxCodigo = 0, codigo = 0;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");

        if (!IsPostBack)
        {
            ViewState["Mostrar"] = null;
            tbParametro.Columns.Add("Detalle");
            tbParametro.Columns.Add("Valor");
            tbParametro.Columns.Add("Estado");
            tbParametro.Columns.Add("Codigo");
            tbParametro.Columns.Add("Orden");
            grdvDatos.Columns[2].Visible = false;
            ViewState["tblParametro"] = tbParametro;
            ViewState["Tipo"] = Request["Tipo"];
            ViewState["CodigoParametro"] = Request["Codigo"];
            funCargaMantenimiento(int.Parse(ViewState["CodigoParametro"].ToString()));
            if (ViewState["Tipo"].ToString() == "N")
            {
                lbltitulo.Text = "Agregar Nuevo Parámetro";
            }
            else
            {
                txtParametro.Text = ViewState["ParametroAnterior"].ToString();
                txtDescripcion.Text = ViewState["Descripcion"].ToString();
                Label3.Visible = true;
                chkEstadoPar.Visible = true;
                chkEstadoPar.Text = ViewState["Estado"].ToString();
                chkEstadoPar.Checked = ViewState["Estado"].ToString() == "Activo" ? true : false;
                lbltitulo.Text = "Editar Parámetro";                
            }
        }
        else grdvDatos.DataSource = Session["grdvDatos"];

    }
    #endregion

    #region Funciones y Procedimientos
    protected void funCargaMantenimiento(int intCodParametro)
    {
        try
        {
            if (intCodParametro > 0)
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = intCodParametro;
                objparam[1] = "";
                objparam[2] = 14;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ViewState["ParametroAnterior"] = dt.Tables[0].Rows[0][0].ToString();
                    ViewState["Descripcion"] = dt.Tables[0].Rows[0][1].ToString();
                    ViewState["Estado"] = dt.Tables[0].Rows[0][2].ToString();
                }
                Array.Resize(ref objparam, 1);
                objparam[0] = intCodParametro;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametro", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                Session["grdvDatos"] = dt;
                ViewState["tblParametro"] = dt.Tables[0];
                new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, dt.Tables[0]);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Botones y Eventos
    protected void grdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvDatos.PageIndex = e.NewPageIndex;
        grdvDatos.DataBind();
    }
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txtParametro.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Nombre del Parámetro..!", this);
                return;
            }
            System.Threading.Thread.Sleep(100);
            Array.Resize(ref objparam, 6);
            objparam[0] = int.Parse(ViewState["CodigoParametro"].ToString());
            objparam[1] = txtParametro.Text.Trim().ToUpper();
            objparam[2] = txtDescripcion.Text.ToUpper();
            objparam[3] = chkEstadoPar.Checked;
            objparam[4] = int.Parse(Session["usuCodigo"].ToString());
            objparam[5] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").FunInsertarParametros(objparam, (DataTable)ViewState["tblParametro"]);
            if (dt.Tables[0].Rows[0][0].ToString() == "") Response.Redirect("FrmParametrosAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmParametrosAdmin.aspx");
    }

    protected void chkEstadoPar_CheckedChanged(object sender, EventArgs e)
    {
        chkEstadoPar.Text = chkEstadoPar.Checked == true ? "Activo" : "Inactivo";
    }
    protected void chkEstadoDet_CheckedChanged(object sender, EventArgs e)
    {
        chkEstadoDet.Text = chkEstadoDet.Checked == true ? "Activo" : "Inactivo";
    }
    protected void btnselecc_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            ViewState["index"] = intIndex;
            ViewState["CodigoDetalle"] = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            txtDetalle.Text = grdvDatos.Rows[intIndex].Cells[0].Text;
            ViewState["DetalleAnterior"] = grdvDatos.Rows[intIndex].Cells[0].Text;
            txtValor.Text = grdvDatos.Rows[intIndex].Cells[1].Text;
            ViewState["Valor"] = grdvDatos.Rows[intIndex].Cells[1].Text;
            ViewState["Orden"] = grdvDatos.DataKeys[intIndex].Values["Orden"].ToString();
            chkEstadoDet.Text = grdvDatos.Rows[intIndex].Cells[3].Text;
            chkEstadoDet.Checked = grdvDatos.Rows[intIndex].Cells[3].Text == "Activo" ? true : false;
            Label6.Visible = true;
            chkEstadoDet.Visible = true;
            imgModificar.Enabled = true;
            imgAgregar.Enabled = false;
            imgCancelar.Enabled = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    protected void imgSubirNivel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            tbParametro = (DataTable)ViewState["tblParametro"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            int orden = int.Parse(grdvDatos.DataKeys[intIndex].Values["Orden"].ToString());

            DataRow[] result = tbParametro.Select("Codigo=" + codigo);
            result[0]["Orden"] = orden - 1;
            tbParametro.AcceptChanges();

            int codigoAnt = int.Parse(grdvDatos.DataKeys[intIndex - 1].Values["Codigo"].ToString());
            int ordenAnt = int.Parse(grdvDatos.DataKeys[intIndex - 1].Values["Orden"].ToString());
            DataRow[] nuevoResult = tbParametro.Select("Codigo=" + codigoAnt);
            nuevoResult[0]["Orden"] = ordenAnt + 1;
            tbParametro.AcceptChanges();

            tbParametro.Select("Orden=0", "Orden ASC");
            tbParametro.DefaultView.Sort = "Orden ASC";
            grdvDatos.DataSource = tbParametro;
            grdvDatos.DataBind();
            new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, tbParametro);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    
    }
    protected void imgBajarNivel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            tbParametro = (DataTable)ViewState["tblParametro"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            int orden = int.Parse(grdvDatos.DataKeys[intIndex].Values["Orden"].ToString());

            DataRow[] result = tbParametro.Select("Codigo=" + codigo);
            result[0]["Orden"] = orden + 1;
            tbParametro.AcceptChanges();

            int codigoAnt = int.Parse(grdvDatos.DataKeys[intIndex + 1].Values["Codigo"].ToString());
            int ordenAnt = int.Parse(grdvDatos.DataKeys[intIndex + 1].Values["Orden"].ToString());
            DataRow[] nuevoResult = tbParametro.Select("Codigo=" + codigoAnt);
            nuevoResult[0]["Orden"] = ordenAnt - 1;
            tbParametro.AcceptChanges();

            tbParametro.Select("Orden=0", "Orden ASC");
            tbParametro.DefaultView.Sort = "Orden ASC";
            grdvDatos.DataSource = tbParametro;
            grdvDatos.DataBind();
            new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, tbParametro);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }
    }
    
    protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            bool lexiste = false;
            lblerror.Text = "";
            if (txtDetalle.Text == "")
            {                
                new Funciones().funShowJSMessage("Ingrese Detalle del Parámetro..!", this);
                return;
            }
            if (txtValor.Text == "")
            {
                new Funciones().funShowJSMessage("Ingrese Valor del Parámetro..!", this);
                return;
            }
            if (ViewState["tblParametro"] != null)
            {
                DataTable tblbuscar = (DataTable)ViewState["tblParametro"];
                DataRow result = tblbuscar.Select("Detalle='" + txtDetalle.Text.ToUpper().Trim() + "' OR Valor='" + txtValor.Text.Trim() + "'").FirstOrDefault();
                tblbuscar.DefaultView.Sort = "Codigo";
                if (result != null) lexiste = true;
                foreach (DataRow dr in tblbuscar.Rows)
                {
                    maxCodigo = int.Parse(dr[3].ToString());
                }
                orden = Convert.ToInt32(tblbuscar.AsEnumerable().Max(row => row["Orden"])) + 1;
            }

            if (lexiste)
            {
                new Funciones().funShowJSMessage("Ya existe definido un Detalle/Valor para el parámetro..!", this);
                return;
            }

            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblParametro"];
            DataRow filagre = tblagre.NewRow();
            filagre["Detalle"] = txtDetalle.Text.ToUpper().Trim();
            filagre["Valor"] = txtValor.Text.Trim();
            filagre["Estado"] = "Activo";
            filagre["Codigo"] = maxCodigo + 1;
            filagre["Orden"] = orden;
            tblagre.Rows.Add(filagre);
            tblagre.DefaultView.Sort = "Orden";
            ViewState["tblParametro"] = tblagre;
            grdvDatos.DataSource = tblagre;
            grdvDatos.DataBind();
            new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, tblagre);
            txtDetalle.Text = "";
            txtValor.Text = "";
            imgCancelar.Enabled = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }

    }
    protected void imgModificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            int valorIndex = int.Parse(ViewState["Orden"].ToString());
            bool lexiste = false;
            lblerror.Text = "";
            if (txtDetalle.Text == "")
            {
                new Funciones().funShowJSMessage("Ingrese Detalle del Parámetro..!", this);
                return;
            }
            if (txtValor.Text == "")
            {
                new Funciones().funShowJSMessage("Ingrese Valor del Parámetro..!", this);
                return;
            }
            if (ViewState["tblParametro"] != null)
            {
                DataTable tblbuscar = (DataTable)ViewState["tblParametro"];
                if (ViewState["DetalleAnterior"].ToString() != txtDetalle.Text.ToUpper() && ViewState["Valor"].ToString().Trim() == txtValor.Text.Trim())
                {
                    DataRow[] result = tblbuscar.Select("Detalle='" + txtDetalle.Text.ToUpper() + "'");
                    lexiste = result.Length == 0 ? false : true;
                }
                if (ViewState["DetalleAnterior"].ToString() != txtDetalle.Text.ToUpper() && ViewState["Valor"].ToString().Trim() != txtValor.Text.Trim())
                {
                    DataRow[] result = tblbuscar.Select("Detalle='" + txtDetalle.Text.ToUpper() + "' AND Valor='" + txtValor.Text.Trim() + "'");
                    lexiste = result.Length == 0 ? false : true;
                }
                if (ViewState["DetalleAnterior"].ToString() == txtDetalle.Text.ToUpper() && ViewState["Valor"].ToString().Trim() != txtValor.Text.Trim())
                {
                    DataRow[] result = tblbuscar.Select("Valor='" + txtValor.Text.Trim() + "'");
                    lexiste = result.Length == 0 ? false : true;
                }
            }

            if (lexiste)
            {
                new Funciones().funShowJSMessage("Ya existe definido un Detalle/Valor para el parámetro..!", this);
                return;
            }

            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblParametro"];
            DataRow[] rows;
            rows = tblagre.Select("Detalle='" + ViewState["DetalleAnterior"].ToString() + "'");
            foreach (DataRow row in rows)
            {
                tblagre.Rows.Remove(row);
            }            
            DataRow filagre = tblagre.NewRow();
            filagre["Detalle"] = txtDetalle.Text.ToUpper().Trim();
            filagre["Valor"] = txtValor.Text.Trim();
            filagre["Orden"] = ViewState["Orden"].ToString();
            filagre["Estado"] = chkEstadoDet.Checked ? "Activo" : "Inactivo";
            filagre["Codigo"] = ViewState["CodigoDetalle"].ToString();
            tblagre.Rows.Add(filagre);
            ViewState["tblParametro"] = tblagre;
            tblagre.Select("Orden=1", "Orden ASC");
            tblagre.DefaultView.Sort = "Orden ASC";
            grdvDatos.DataSource = tblagre;
            grdvDatos.DataBind();
            new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, tblagre);
            txtDetalle.Text = "";
            txtValor.Text = "";
            imgModificar.Enabled = false;
            imgAgregar.Enabled = true;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }

    }
    protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
    {
        txtDetalle.Text = "";
        txtValor.Text = "";
        imgAgregar.Enabled = true;
        imgCancelar.Enabled = false;
        imgModificar.Enabled = false;
        funCargaMantenimiento(int.Parse(ViewState["CodigoParametro"].ToString()));

    }
    protected void imgDel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            codigo = int.Parse(grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
            tbParametro = (DataTable)ViewState["tblParametro"];
            resultado = tbParametro.Select("Codigo='" + codigo + "'").FirstOrDefault();
            resultado.Delete();
            tbParametro.AcceptChanges();
            Array.Resize(ref objparam, 11);
            objparam[0] = 5;
            objparam[1] = "";
            objparam[2] = "";
            objparam[3] = "";
            objparam[4] = "";
            objparam[5] = "";
            objparam[6] = int.Parse(ViewState["CodigoParametro"].ToString());
            objparam[7] = codigo;
            objparam[8] = 0;
            objparam[9] = 0;
            objparam[10] = 0;
            new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
            tbParametro.Select("Orden=1", "Orden ASC");
            tbParametro.DefaultView.Sort = "Orden ASC";
            ViewState["tblParametro"] = tbParametro;
            grdvDatos.DataSource = tbParametro;
            grdvDatos.DataBind();
            new Funciones().SetearGrid(grdvDatos, imgSubir, imgBajar, tbParametro);

        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }    
    #endregion
}