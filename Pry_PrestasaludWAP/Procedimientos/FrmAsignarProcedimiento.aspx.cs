using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Procedimientos
{
    public partial class FrmAsignarProcedimiento : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Funciones fun = new Funciones();
        DataTable tbProcedimiento = new DataTable();
        int maxCodigo = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-EC");
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            txtCostoRed.Attributes.Add("onchange", "ValidarDecimales();");
            txtCostoReal.Attributes.Add("onchange", "ValidarDecimales_1();");
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                tbProcedimiento.Columns.Add("Codigo");
                tbProcedimiento.Columns.Add("CodigoPrestadora");
                tbProcedimiento.Columns.Add("CodigoProcedimiento");
                tbProcedimiento.Columns.Add("CodigoAplica");
                tbProcedimiento.Columns.Add("Procedimiento");
                tbProcedimiento.Columns.Add("CostoRed");
                tbProcedimiento.Columns.Add("CostoReal");
                tbProcedimiento.Columns.Add("Aplica");
                tbProcedimiento.Columns.Add("Estado");
                ViewState["tblProcedimiento"] = tbProcedimiento;
                lbltitulo.Text = "Asignar Procedimientos a Red/Prestadora";

                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());

                funCargarCombos();
                funCargaMantenimiento();
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funLlenarPrestador(TreeNode treenode)
        {
            try
            {
                int idCiudad = 0;
                Array.Resize(ref objparam, 2);
                objparam[0] = 11;
                objparam[1] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && treenode != null)
                {
                    foreach (DataRow dr in dt.Tables[0].Rows)
                    {
                        idCiudad = int.Parse(dr[0].ToString());
                        TreeNode node = new TreeNode(dr[1].ToString(), dr[0].ToString());
                        node = funAgregarPrestadora(node, idCiudad);
                        treenode.ChildNodes.Add(node);
                    }
                    trvPrestadoras.CollapseAll();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funCargarCombos()
        {
            try
            {
                Array.Resize(ref objparam, 1);
                objparam[0] = 9;
                ddlProcedimientos.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlProcedimientos.DataTextField = "Descripcion";
                ddlProcedimientos.DataValueField = "Codigo";
                ddlProcedimientos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 10;
                objparam[1] = ViewState["CodigoPrestadora"] == null ? 0 : int.Parse(ViewState["CodigoPrestadora"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt.Tables[0].Rows.Count > 0) txtPrestadora.Text = dt.Tables[0].Rows[0][0].ToString();
                if (dt.Tables[1].Rows.Count > 0)
                {
                    DataTable dtTurno = dt.Tables[1];
                    ViewState["tblProcedimiento"] = dtTurno;
                    grdvDatos.DataSource = dtTurno;
                    grdvDatos.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funClearObject()
        {
            ddlProcedimientos.SelectedValue = "0";
            txtCostoRed.Text = "0.00";
            txtCostoReal.Text = "0.00";
            imgAgregar.Enabled = true;
            imgModificar.Enabled = false;
            imgCancelar.Enabled = false;
            tbProcedimiento.Columns.Add("Codigo");
            tbProcedimiento.Columns.Add("CodigoPrestadora");
            tbProcedimiento.Columns.Add("CodigoProcedimiento");
            tbProcedimiento.Columns.Add("CodigoAplica");
            tbProcedimiento.Columns.Add("Procedimiento");
            tbProcedimiento.Columns.Add("CostoRed");
            tbProcedimiento.Columns.Add("CostoReal");
            tbProcedimiento.Columns.Add("Aplica");
            tbProcedimiento.Columns.Add("Estado");
            ViewState["tblProcedimiento"] = tbProcedimiento;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void trvPrestadoras_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            switch (e.Node.Depth)
            {
                case 0:
                    funLlenarPrestador(e.Node);
                    break;
            }
        }

        private TreeNode funAgregarPrestadora(TreeNode node, int idCiudad)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = 12;
                objparam[1] = idCiudad;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarCiudadesLista", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drFila in dt.Tables[0].Rows)
                    {
                        TreeNode unNode = new TreeNode(drFila[1].ToString(), drFila[0].ToString());
                        node.ChildNodes.Add(unNode);
                    }
                }
                return node;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
                return node;
            }
        }

        protected void trvPrestadoras_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView arbolPres = (TreeView)(sender);
            TreeNode node = arbolPres.SelectedNode;
            try
            {
                lblerror.Text = "";
                switch (node.Depth)
                {
                    case 2:
                        trvPrestadoras.SelectedNode.Expand();
                        string[] pathPrestadora = node.ValuePath.Split(new char[] { '/' });
                        ViewState["CodigoPrestadora"] = pathPrestadora[3].ToString();
                        lblerror.Text = "";
                        funClearObject();
                        funCargaMantenimiento();
                        break;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void ddlProcedimientos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ddlProcedimientos.SelectedValue);
            objparam[1] = "";
            objparam[2] = 26;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt != null && dt.Tables[0].Rows.Count > 0)
            {
                txtCostoRed.Text = dt.Tables[0].Rows[0][2].ToString().Replace(",", ".");
                txtCostoReal.Text = dt.Tables[0].Rows[0][2].ToString().Replace(",", ".");
                ViewState["Aplica"] = dt.Tables[0].Rows[0][3].ToString();
                ViewState["CodigoAplica"] = dt.Tables[0].Rows[0][4].ToString();
            }
        }

        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool lexiste = false;
                if (string.IsNullOrEmpty(txtPrestadora.Text.Trim()))
                {
                    lblerror.Text = "Seleccione Red/Prestadora..!";
                    return;
                }
                if (ddlProcedimientos.SelectedValue == "0")
                {
                    lblerror.Text = "Seleccione Procedimiento..!";
                    return;
                }
                if (string.IsNullOrEmpty(txtCostoRed.Text))
                {
                    lblerror.Text = "Ingrese Costo del Procedimiento..!";
                    return;
                }
                if (ViewState["tblProcedimiento"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblProcedimiento"];
                    DataRow result = tblbuscar.Select("Procedimiento='" + ddlProcedimientos.SelectedItem.ToString() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "Codigo";
                    if (result != null) lexiste = true;
                    foreach (DataRow dr in tblbuscar.Rows)
                    {
                        maxCodigo = int.Parse(dr[0].ToString());
                    }
                }

                if (lexiste)
                {
                    lblerror.Text = "Procedimiento ya está agregado..!";
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProcedimiento"];
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["CodigoPrestadora"] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                filagre["CodigoProcedimiento"] = int.Parse(ddlProcedimientos.SelectedValue);
                filagre["CodigoAplica"] = ViewState["CodigoAplica"].ToString();
                filagre["Procedimiento"] = ddlProcedimientos.SelectedItem.ToString();
                filagre["CostoRed"] = Math.Round(Convert.ToDecimal(txtCostoRed.Text, CultureInfo.InvariantCulture), 2).ToString("0.00").Replace(",", ".");
                filagre["CostoReal"] = txtCostoReal.Text == "" ? "0.00" : Math.Round(Convert.ToDecimal(txtCostoReal.Text, CultureInfo.InvariantCulture), 2).ToString("0.00").Replace(",", ".");
                filagre["Aplica"] = ViewState["Aplica"].ToString().Trim().ToUpper();
                filagre["Estado"] = "Activo";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Procedimiento";
                ViewState["tblProcedimiento"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                imgCancelar.Enabled = true;
                imgModificar.Enabled = false;
                ddlProcedimientos.SelectedValue = "0";
                txtCostoRed.Text = "0.00";
                txtCostoReal.Text = "0.00";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgModificar_Click(object sender, ImageClickEventArgs e)
        {
            lblerror.Text = "";
            bool lexiste = false;
            if (string.IsNullOrEmpty(txtCostoRed.Text))
            {
                lblerror.Text = "Ingrese Costo del Procedimiento..!";
                return;
            }
            try
            {
                //CONSULTAR QUE HACE ESTO EN EL MODIFICAR
                //Array.Resize(ref objparam, 3);
                //objparam[0] = int.Parse(ViewState["CodigoProced"].ToString());
                //objparam[1] = "";
                //objparam[2] = 26;
                //dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (ViewState["tblProcedimiento"] != null)
                {
                    if (ViewState["ProcedimientoAnterior"].ToString() == ddlProcedimientos.SelectedItem.ToString())
                        lexiste = false;
                    else
                    {
                        DataTable tblbuscar = (DataTable)ViewState["tblProcedimiento"];
                        DataRow result = tblbuscar.Select("Procedimiento='" + ddlProcedimientos.SelectedItem.ToString() + "'").FirstOrDefault();
                        if (result != null) lexiste = true;
                    }
                }
                if (lexiste)
                {
                    lblerror.Text = "Procedimiento ya está agregado..!";
                    return;
                }
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblProcedimiento"];
                tblagre.Rows.RemoveAt(int.Parse(ViewState["index"].ToString()));
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = int.Parse(ViewState["Codigo"].ToString());
                filagre["CodigoPrestadora"] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                filagre["CodigoProcedimiento"] = int.Parse(ViewState["CodigoProced"].ToString());
                filagre["CodigoAplica"] = ViewState["CodigoAplica"].ToString();
                filagre["Procedimiento"] = ddlProcedimientos.SelectedItem.ToString();
                filagre["CostoRed"] = Math.Round(Convert.ToDecimal(txtCostoRed.Text, CultureInfo.InvariantCulture), 2).ToString("0.00").Replace(",", ".");
                filagre["CostoReal"] = txtCostoReal.Text == "" ? "0.00" : Math.Round(Convert.ToDecimal(txtCostoReal.Text, CultureInfo.InvariantCulture), 2).ToString("0.00").Replace(",", ".");
                filagre["Aplica"] = ViewState["Aplica"].ToString().Trim().ToUpper();
                filagre["Estado"] = ViewState["Estado"].ToString();
                tblagre.Rows.Add(filagre);
                //tblagre.DefaultView.Sort = "Prioridad";
                //DataTable tblordenada = tblagre.DefaultView.ToTable();

                ViewState["tblProcedimiento"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
                }
                imgModificar.Enabled = false;
                imgCancelar.Enabled = false;
                imgAgregar.Enabled = true;
                ddlProcedimientos.SelectedValue = "0";
                txtCostoRed.Text = "0.00";
                txtCostoReal.Text = "0.00";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            lblerror.Text = "";
            funClearObject();
            funCargaMantenimiento();
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            if (grdvDatos.Rows.Count == 0)
            {
                lblerror.Text = "Agregue Procedimientos..!";
                return;
            }
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertarProcePresta(objparam, (DataTable)ViewState["tblProcedimiento"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    grdvDatos.DataSource = null;
                    grdvDatos.DataBind();
                    string strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                    Response.Redirect(strResponse, false);
                }
                else
                {
                    lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
                    funClearObject();
                    funCargaMantenimiento();
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }

        protected void imgEditar_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            ViewState["index"] = intIndex;
            ViewState["Codigo"] = grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString();
            ViewState["CodigoProced"] = grdvDatos.DataKeys[intIndex].Values["CodigoProcedimiento"].ToString();
            ViewState["Aplica"] = grdvDatos.DataKeys[intIndex].Values["Aplica"].ToString();
            ViewState["CodigoAplica"] = grdvDatos.DataKeys[intIndex].Values["CodigoAplica"].ToString();
            ViewState["Estado"] = grdvDatos.DataKeys[intIndex].Values["Estado"].ToString();
            ddlProcedimientos.SelectedValue = ViewState["CodigoProced"].ToString();
            txtCostoRed.Text = grdvDatos.Rows[intIndex].Cells[1].Text;
            txtCostoReal.Text = grdvDatos.DataKeys[intIndex].Values["CostoReal"].ToString();
            imgModificar.Enabled = true;
            imgAgregar.Enabled = false;
            imgCancelar.Enabled = true;
            foreach (GridViewRow row in grdvDatos.Rows)
            {
                grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
            }
            grdvDatos.Rows[intIndex].BackColor = System.Drawing.Color.DarkGray;
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ViewState["CodigoProced"].ToString());
            objparam[1] = "";
            objparam[2] = 26;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            ViewState["ProcedimientoAnterior"] = dt.Tables[0].Rows[0][0].ToString();
        }

        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblest = new Label();
            CheckBox chkest = new CheckBox();
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkest = (CheckBox)(e.Row.Cells[4].FindControl("chkEstado"));
                    lblest = (Label)(e.Row.Cells[5].FindControl("lblEstado"));

                    int codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    Array.Resize(ref objparam, 3);
                    objparam[0] = codigo;
                    objparam[1] = "";
                    objparam[2] = 29;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        chkest.Enabled = true;
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
        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            Label lblest = new Label();
            CheckBox chkest = new CheckBox();

            chkest = (CheckBox)(gvRow.Cells[4].FindControl("chkEstado"));
            lblest = (Label)(gvRow.Cells[5].FindControl("lblEstado"));
            lblest.Text = chkest.Checked ? "Activo" : "Inactivo";
            tbProcedimiento = (DataTable)ViewState["tblProcedimiento"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            DataRow[] result = tbProcedimiento.Select("Codigo=" + codigo);
            result[0]["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
            tbProcedimiento.AcceptChanges();
        }
        #endregion
    }
}