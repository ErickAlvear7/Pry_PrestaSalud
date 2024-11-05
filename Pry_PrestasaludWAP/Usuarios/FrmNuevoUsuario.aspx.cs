using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Linq;

public partial class Usuarios_FrmNuevoUsuario : Page
{
    #region Variables
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    CheckBox ChkSelecc = new CheckBox();
    ImageButton ImgSelecc = new ImageButton();
    string Codigo = "", Selecc = "", CodigoSELECC = "";
    DataTable DtbCliente = new DataTable();
    DataTable DtbProductos = new DataTable();
    DataTable DtbProductosTemp = new DataTable();
    DataRow Resultado, FileAgre;
    DataRow[] Result, resulcliente;
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
            Response.Redirect("~/Reload.html");

        if (!IsPostBack)
        {
            txtFechaCaduca.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ViewState["CodigoUsuario"] = Request["Codigo"];
            FunCargarCombos(0);
            if (ViewState["CodigoUsuario"].ToString() == "0")
            {
                lbltitulo.Text = "Agregar Nuevo Usuario";
            }
            else
            {
                Label7.Visible = true;
                chkEstado.Visible = true;
                lbltitulo.Text = "Editar Usuarios";
                FunCargaMantenimiento(int.Parse(ViewState["CodigoUsuario"].ToString()));
            }
        }
    }
    #endregion
    
    #region Funciones y Procedimientos
    protected void FunCargaMantenimiento(int intCodUser)
    {
        try
        {
            Array.Resize(ref objparam, 1);
            objparam[0] = intCodUser;
            dt = new Conexion(2, "").funConsultarSqls("sp_UsuEditRead", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                txtNombres.Text = dt.Tables[0].Rows[0][1].ToString();
                txtApellidos.Text = dt.Tables[0].Rows[0][2].ToString();
                ddlDepartamento.SelectedValue = dt.Tables[0].Rows[0][3].ToString();
                txtUser.Text = dt.Tables[0].Rows[0][4].ToString();
                txtPassword.Attributes.Add("Value", new Funciones().funDesencripta(dt.Tables[0].Rows[0][5].ToString()));
                ddlPerfil.SelectedValue = dt.Tables[0].Rows[0][6].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][7].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][7].ToString() == "Activo" ? true : false;
                chkCaduca.Text = dt.Tables[0].Rows[0][8].ToString();
                chkCaduca.Checked = dt.Tables[0].Rows[0][8].ToString() == "Si" ? true : false;
                Label10.Visible = chkCaduca.Checked;
                txtFechaCaduca.Visible = chkCaduca.Checked;
                txtFechaCaduca.Text = dt.Tables[0].Rows[0][9].ToString();
                chkCambiar.Text = dt.Tables[0].Rows[0][10].ToString();
                chkCambiar.Checked = dt.Tables[0].Rows[0][10].ToString() == "Si" ? true : false;
                chkPermisos.Checked = dt.Tables[0].Rows[0][11].ToString() == "Si" ? true : false;
                chkPermisos.Text = dt.Tables[0].Rows[0][11].ToString();
                txtEmail.Text = dt.Tables[0].Rows[0][12].ToString();
                txtLogo.Text = dt.Tables[0].Rows[0][13].ToString();
                ddlCliente.SelectedValue = dt.Tables[0].Rows[0][14].ToString();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    private void FunCargarCombos(int opcion)
    {
        try
        {
            switch (opcion)
            {
                case 0:
                    objparam[0] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlPerfil.DataSource = dt;
                    ddlPerfil.DataTextField = "per_Descripcion";
                    ddlPerfil.DataValueField = "per_Codigo";
                    ddlPerfil.DataBind();

                    objparam[0] = 1;
                    dt = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlDepartamento.DataSource = dt;
                    ddlDepartamento.DataTextField = "Descripcion";
                    ddlDepartamento.DataValueField = "Codigo";
                    ddlDepartamento.DataBind();

                    Array.Resize(ref objparam, 11);
                    objparam[0] = 11;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(ViewState["CodigoUsuario"].ToString());
                    objparam[7] = 0;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    ViewState["Cliente"] = dt.Tables[0];
                    GrdvClientes.DataSource = dt;
                    GrdvClientes.DataBind();
                    break;
                case 1:
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 12;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(ViewState["CodigoCAMP"].ToString());
                    objparam[7] = int.Parse(ViewState["CodigoUsuario"].ToString());
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    ViewState["Producto"] = dt.Tables[0];
                    ViewState["ProductoTemp"] = dt.Tables[0].Clone();
                    GrdvProductos.DataSource = dt;
                    GrdvProductos.DataBind();
                    break;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
 
    }

    private void FunActualizarDatos(int Tipo, int Codigo, string Selecc)
    {
        try
        {
            Array.Resize(ref objparam, 14);
            objparam[0] = Tipo;
            objparam[1] = Codigo;
            objparam[2] = int.Parse(ViewState["CodigoCAMP"].ToString()); ;
            objparam[3] = int.Parse(ViewState["CodigoUsuario"].ToString());
            objparam[4] = 0;
            objparam[4] = Selecc;            
            objparam[5] = Codigo;
            objparam[6] = 0;
            objparam[7] = Selecc;
            objparam[8] = "";
            objparam[9] = "";
            objparam[10] = "";
            objparam[11] = 0;
            objparam[12] = 0;
            objparam[13] = 0;
            new Conexion(2, "").funConsultarSqls("sp_NuevoClienteUsuarios", objparam);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion

    #region Botones y Eventos
    protected void btnGrabar_Click(object sender, EventArgs e)
    {
        try
        {
            if(string.IsNullOrEmpty(txtNombres.Text.Trim()))
            {                
                new Funciones().funShowJSMessage("Ingrese Nombre del usuario..!", this);
                return;
            }
            if (string.IsNullOrEmpty(txtApellidos.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Apellido del usuario..!", this);
                return;
            }
            if (string.IsNullOrEmpty(txtUser.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Login del usuario..!", this);
                return;
            }
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                new Funciones().funShowJSMessage("Ingrese Password..!", this);
                return;
            }
            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                if (new Funciones().email_bien_escrito(txtEmail.Text) == false)
                {
                    new Funciones().funShowJSMessage("Ingrese un e-mail válido..!", this);
                    return;
                }
            }
            if (!new Funciones().IsDate(txtFechaCaduca.Text))
            {
                new Funciones().funShowJSMessage("Fecha no válida..!", this);
                return;
            }
            
            if (ddlDepartamento.SelectedItem.ToString() == "--Seleccione Departamento--")
            {
                new Funciones().funShowJSMessage("Seleccione Departamento..!", this);
                return;
            }

            if (ddlPerfil.SelectedItem.ToString() == "--Seleccione Perfil--")
            {
                new Funciones().funShowJSMessage("Seleccione Perfil..!", this);
                return;
            }
            Array.Resize(ref objparam, 17);
            objparam[0] = ViewState["CodigoUsuario"] == null ? 0 : int.Parse(ViewState["CodigoUsuario"].ToString());
            objparam[1] = int.Parse(ddlPerfil.SelectedValue);
            objparam[2] = txtNombres.Text.ToUpper();
            objparam[3] = txtApellidos.Text.ToUpper();
            objparam[4] = int.Parse(ddlDepartamento.SelectedValue);
            objparam[5] = txtUser.Text;
            objparam[6] = new Funciones().EncriptaMD5(txtPassword.Text);
            objparam[7] = chkEstado.Checked;
            objparam[8] = chkCaduca.Checked;
            objparam[9] = txtFechaCaduca.Text;
            objparam[10] = chkCambiar.Checked;
            objparam[11] = chkPermisos.Checked;
            objparam[12] = txtEmail.Text.Trim();
            objparam[13] = txtLogo.Text.Trim();
            objparam[14] = 0;
            objparam[15] = int.Parse(Session["usuCodigo"].ToString());
            objparam[16] = Session["MachineName"].ToString();
            dt = new Conexion(2, "").funConsultarSqls("sp_UsuarioNuevo", objparam);
            if (dt.Tables[0].Rows[0][0].ToString() == "Existe") new Funciones().funShowJSMessage("Login Usuario ya existe..!", this);
            else
            {
                ViewState["CodigoUsuario"] = dt.Tables[0].Rows[0]["Codigo"].ToString();
                DtbCliente = (DataTable)ViewState["Cliente"];
                resulcliente = DtbCliente.Select("Selecc='SI'");
                if (ViewState["Producto"] != null)
                {
                    foreach (DataRow DrFila in resulcliente)
                    {
                        ViewState["CodigoCAMP"] = DrFila["CodigoCAMP"].ToString();
                        Array.Resize(ref objparam, 14);
                        objparam[0] = 0;
                        objparam[1] = int.Parse(DrFila["CodigoCLUS"].ToString());
                        objparam[2] = int.Parse(DrFila["CodigoCAMP"].ToString());
                        objparam[3] = int.Parse(ViewState["CodigoUsuario"].ToString());
                        objparam[4] = DrFila["Selecc"].ToString();
                        objparam[5] = 0;
                        objparam[6] = 0;
                        objparam[7] = "";
                        objparam[8] = "";
                        objparam[9] = "";
                        objparam[10] = "";
                        objparam[11] = 0;
                        objparam[12] = 0;
                        objparam[13] = 0;
                        DtbProductos = (DataTable)ViewState["Producto"];
                        Result = DtbProductos.Select("CodigoCAMP='" + DrFila["CodigoCAMP"].ToString() + "'");
                        foreach (DataRow DrFilap in Result)
                        {
                            objparam[5] = int.Parse(DrFilap["CodigoPRCL"].ToString());
                            objparam[6] = int.Parse(DrFilap["CodigoPROD"].ToString());
                            objparam[7] = DrFilap["Selecc"].ToString();
                            dt = new Conexion(2, "").funConsultarSqls("sp_NuevoClienteUsuarios", objparam);
                        }
                    }
                }
                Response.Redirect("FrmUsuarioAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
            }                
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void btnSalir_Click(object sender, EventArgs e)
    {
        Response.Redirect("FrmUsuarioAdmin.aspx", true);
    }

    protected void chkEstado_CheckedChanged(object sender, EventArgs e)
    {
        chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
    }

    protected void chkCaduca_CheckedChanged(object sender, EventArgs e)
    {
        txtFechaCaduca.Text = DateTime.Now.ToString("MM/dd/yyyy");
        chkCaduca.Text = chkCaduca.Checked == true ? "Si" : "No";
        Label10.Visible = chkCaduca.Checked;
        txtFechaCaduca.Visible = chkCaduca.Checked;
    }

    protected void chkCambiar_CheckedChanged(object sender, EventArgs e)
    {
        chkCambiar.Text = chkCambiar.Checked == true ? "Si" : "No";
    }

    protected void chkPermisos_CheckedChanged(object sender, EventArgs e)
    {
        chkPermisos.Text = chkPermisos.Checked == true ? "Si" : "No";
    }

    protected void ChkCliente_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            GrdvProductos.DataSource = null;
            GrdvProductos.DataBind();
            ChkSelecc = (CheckBox)(gvRow.Cells[1].FindControl("ChkCliente"));
            ImgSelecc = (ImageButton)(gvRow.Cells[2].FindControl("ImgSelecc"));
            Codigo = GrdvClientes.DataKeys[gvRow.RowIndex].Values["CodigoCAMP"].ToString();
            CodigoSELECC = GrdvClientes.DataKeys[gvRow.RowIndex].Values["CodigoCLUS"].ToString();
            ViewState["CodigoCAMP"] = GrdvClientes.DataKeys[gvRow.RowIndex].Values["CodigoCAMP"].ToString();
            if (CodigoSELECC != "0") FunActualizarDatos(1, int.Parse(CodigoSELECC), ChkSelecc.Checked ? "SI" : "NO");
            DtbCliente = (DataTable)ViewState["Cliente"];
            Resultado = DtbCliente.Select("CodigoCAMP='" + Codigo + "'").FirstOrDefault();
            Resultado["Selecc"] = ChkSelecc.Checked ? "SI" : "NO";
            GrdvClientes.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.White;
            if (!ChkSelecc.Checked)
            {
                foreach (GridViewRow fr in GrdvClientes.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                ImgSelecc.ImageUrl = "~/Botones/seleccgris.png";
                ImgSelecc.Enabled = false;
                GrdvProductos.DataSource = null;
                GrdvProductos.DataBind();
                if (ViewState["Producto"] != null)
                {
                    DtbProductos = (DataTable)ViewState["Producto"];
                    Result = DtbProductos.Select("CodigoCAMP='" + Codigo + "'");
                    foreach (DataRow produc in Result)
                    {
                        produc["Selecc"] = "NO";
                    }
                }
            }
            else
            {
                ImgSelecc.ImageUrl = "~/Botones/selecc.png";
                ImgSelecc.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void GrdvClientes_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                ChkSelecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkCliente"));
                ImgSelecc = (ImageButton)(e.Row.Cells[2].FindControl("ImgSelecc"));
                Selecc = GrdvClientes.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();
                if (Selecc == "SI")
                {
                    ChkSelecc.Checked = true;
                    ImgSelecc.ImageUrl = "~/Botones/selecc.png";
                    ImgSelecc.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void ImgSelecc_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow fr in GrdvClientes.Rows)
            {
                fr.Cells[0].BackColor = System.Drawing.Color.White;
            }
            GrdvClientes.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Coral;
            ViewState["CodigoCAMP"] = GrdvClientes.DataKeys[gvRow.RowIndex].Values["CodigoCAMP"].ToString();
            if (ViewState["Producto"] == null) FunCargarCombos(1);
            else
            {
                DtbProductos = (DataTable)ViewState["Producto"];
                DtbProductosTemp = (DataTable)ViewState["ProductoTemp"];
                DtbProductosTemp.Clear();
                Result = DtbProductos.Select("CodigoCAMP='" + ViewState["CodigoCAMP"].ToString() + "'");
                if (Result.Count() == 0)
                {
                    Array.Resize(ref objparam, 11);
                    objparam[0] = 12;
                    objparam[1] = "";
                    objparam[2] = "";
                    objparam[3] = "";
                    objparam[4] = "";
                    objparam[5] = "";
                    objparam[6] = int.Parse(ViewState["CodigoCAMP"].ToString());
                    objparam[7] = int.Parse(ViewState["CodigoUsuario"].ToString()); ;
                    objparam[8] = 0;
                    objparam[9] = 0;
                    objparam[10] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos1", objparam);
                    foreach (DataRow DrFila in dt.Tables[0].Rows)
                    {
                        FileAgre = DtbProductos.NewRow();
                        FileAgre["CodigoPRCL"] = DrFila["CodigoPRCL"].ToString();
                        FileAgre["CodigoCLUS"] = DrFila["CodigoCLUS"].ToString();
                        FileAgre["CodigoPROD"] = DrFila["CodigoPROD"].ToString();
                        FileAgre["CodigoCAMP"] = DrFila["CodigoCAMP"].ToString();
                        FileAgre["Producto"] = DrFila["Producto"].ToString();
                        FileAgre["Selecc"] = DrFila["Selecc"].ToString();
                        DtbProductos.Rows.Add(FileAgre);
                    }
                    ViewState["Producto"] = DtbProductos;
                    DtbProductosTemp = (DataTable)ViewState["ProductoTemp"];
                    DtbProductosTemp.Clear();
                    Result = DtbProductos.Select("CodigoCAMP='" + ViewState["CodigoCAMP"].ToString() + "'");
                    foreach (DataRow DrFila in Result)
                    {
                        FileAgre = DtbProductosTemp.NewRow();
                        FileAgre["CodigoPRCL"] = DrFila["CodigoPRCL"].ToString();
                        FileAgre["CodigoCLUS"] = DrFila["CodigoCLUS"].ToString();
                        FileAgre["CodigoPROD"] = DrFila["CodigoPROD"].ToString();
                        FileAgre["CodigoCAMP"] = DrFila["CodigoCAMP"].ToString();
                        FileAgre["Producto"] = DrFila["Producto"].ToString();
                        FileAgre["Selecc"] = DrFila["Selecc"].ToString();
                        DtbProductosTemp.Rows.Add(FileAgre);
                    }
                    GrdvProductos.DataSource = DtbProductosTemp;
                    GrdvProductos.DataBind();
                }
                else
                {
                    foreach (DataRow DrFila in Result)
                    {
                        FileAgre = DtbProductosTemp.NewRow();
                        FileAgre["CodigoPRCL"] = DrFila["CodigoPRCL"].ToString();
                        FileAgre["CodigoCLUS"] = DrFila["CodigoCLUS"].ToString();
                        FileAgre["CodigoPROD"] = DrFila["CodigoPROD"].ToString();
                        FileAgre["CodigoCAMP"] = DrFila["CodigoCAMP"].ToString();
                        FileAgre["Producto"] = DrFila["Producto"].ToString();
                        FileAgre["Selecc"] = DrFila["Selecc"].ToString();
                        DtbProductosTemp.Rows.Add(FileAgre);
                    }
                    GrdvProductos.DataSource = DtbProductosTemp;
                    GrdvProductos.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void ChkProducto_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            ChkSelecc = (CheckBox)(gvRow.Cells[1].FindControl("ChkProducto"));
            Codigo = GrdvProductos.DataKeys[gvRow.RowIndex].Values["CodigoPROD"].ToString();
            CodigoSELECC = GrdvProductos.DataKeys[gvRow.RowIndex].Values["CodigoPRCL"].ToString();
            if (CodigoSELECC != "0") FunActualizarDatos(2, int.Parse(CodigoSELECC), ChkSelecc.Checked ? "SI" : "NO");
            DtbProductos = (DataTable)ViewState["Producto"];
            Resultado = DtbProductos.Select("CodigoPROD='" + Codigo + "'").FirstOrDefault();
            Resultado["Selecc"] = ChkSelecc.Checked ? "SI" : "NO";
            DtbProductos.AcceptChanges();
            ViewState["Producto"] = DtbProductos;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }

    protected void GrdvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                ChkSelecc = (CheckBox)(e.Row.Cells[1].FindControl("ChkProducto"));
                Selecc = GrdvProductos.DataKeys[e.Row.RowIndex].Values["Selecc"].ToString();
                if (Selecc == "SI") ChkSelecc.Checked = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
        }
    }
    #endregion
}