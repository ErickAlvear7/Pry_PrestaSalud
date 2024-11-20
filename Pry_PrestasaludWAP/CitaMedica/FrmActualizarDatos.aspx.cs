using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.CitaMedica
{
    public partial class FrmActualizarDatos : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtBene = new DataTable();
        Funciones fun = new Funciones();
        Object[] objparam = new Object[1];
        DataTable tbBeneficiarios = new DataTable();
        int maxCodigo = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                txtFechaNacBen.Text = "0";
                txtFechaNacimiento.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaNacBen.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFechaNacimiento.Attributes.Add("onchange", "Calcular_Edad();");
                txtFechaNacBen.Attributes.Add("onchange", "Calcular_EdadB();");
                TxtFechaIniCobertura.Text = DateTime.Now.ToString("dd/MM/yyyy");
                TxtFechaFinCobertura.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNumeroDocumento.Attributes.Add("onchange", "Validar_Cedula();");
                try
                {
                    tbBeneficiarios.Columns.Add("PrimerNombre");
                    tbBeneficiarios.Columns.Add("SegundoNombre");
                    tbBeneficiarios.Columns.Add("PrimerApellido");
                    tbBeneficiarios.Columns.Add("SegundoApellido");
                    tbBeneficiarios.Columns.Add("Beneficiario");
                    tbBeneficiarios.Columns.Add("Parentesco");
                    tbBeneficiarios.Columns.Add("ParentescoCod");
                    tbBeneficiarios.Columns.Add("Estado");
                    tbBeneficiarios.Columns.Add("CodigoBen");
                    tbBeneficiarios.Columns.Add("TipoDocumentoBen");
                    tbBeneficiarios.Columns.Add("NumeroDocumentoBen");
                    tbBeneficiarios.Columns.Add("GeneroBen");
                    tbBeneficiarios.Columns.Add("EstadoCivilBen");
                    tbBeneficiarios.Columns.Add("FechaNacimientoBen");
                    tbBeneficiarios.Columns.Add("ProvinciaBen");
                    tbBeneficiarios.Columns.Add("CiudadBen");
                    tbBeneficiarios.Columns.Add("DireccionBen");
                    tbBeneficiarios.Columns.Add("FonoCasaBen");
                    tbBeneficiarios.Columns.Add("FonoOficinaBen");
                    tbBeneficiarios.Columns.Add("CelularBen");
                    tbBeneficiarios.Columns.Add("EmailBen");
                    tbBeneficiarios.Columns.Add("Modificado");
                    ViewState["tblBeneficiarios"] = tbBeneficiarios;

                    ViewState["CodProducto"] = Request["CodProducto"];
                    ViewState["CodTitular"] = Request["CodTitular"];
                    ViewState["CodigoPersona"] = 0;
                    ViewState["Regresar"] = Request["Regresar"];
                    funCargarCombos();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["CodProducto"].ToString());
                    objparam[1] = "";
                    objparam[2] = 12;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    txtNumeroDocumento.Enabled = true;
                    Label3.Visible = true;
                    chkEstado.Visible = true;
                    lbltitulo.Text = "Editar Titular " + dt.Tables[0].Rows[0][0].ToString();
                    funCargaMantenimiento(int.Parse(ViewState["CodTitular"].ToString()));

                    if (Session["Perfil"].ToString() == "NOVA")
                    {
                        TxtFechaFinCobertura.Visible = false;
                        lblFinCobertura.Visible = false;
                    }

                }
                catch (Exception ex)
                {
                    lblerror.Text = ex.ToString();
                }
            }
            else
            {
                txtEdad.Text = hidEdad.Value;
                txtEdadBen.Text = hidEdadB.Value;
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funCargaMantenimiento(int codigoTitular)
        {
            try
            {
                Array.Resize(ref objparam, 4);
                objparam[0] = 0;
                objparam[1] = codigoTitular;
                objparam[2] = 0;
                objparam[3] = int.Parse(ViewState["CodProducto"].ToString());
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarTitularEdit", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    ViewState["CodigoPersona"] = dt.Tables[0].Rows[0][21].ToString();
                    ddlTipoDocumento.SelectedValue = dt.Tables[0].Rows[0][18].ToString();
                    if (ddlTipoDocumento.SelectedValue == "C")
                    {
                        txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                        txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                        txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                    }
                    else
                    {
                        txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                        txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                        txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                    }
                    txtNumeroDocumento.Text = dt.Tables[0].Rows[0][1].ToString();
                    txtPrimerNombre.Text = dt.Tables[0].Rows[0][2].ToString();
                    txtSegundoNombre.Text = dt.Tables[0].Rows[0][3].ToString();
                    txtPrimerApellido.Text = dt.Tables[0].Rows[0][4].ToString();
                    txtSegundoApellido.Text = dt.Tables[0].Rows[0][5].ToString();
                    ddlGenero.SelectedValue = dt.Tables[0].Rows[0][6].ToString();
                    ddlEstadoCivil.SelectedValue = dt.Tables[0].Rows[0][7].ToString();
                    txtFechaNacimiento.Text = dt.Tables[0].Rows[0][8].ToString();
                    txtEdad.Text = dt.Tables[0].Rows[0][9].ToString();
                    ddlProvincia.SelectedValue = dt.Tables[0].Rows[0][10].ToString();
                    funCascadaCombos(1);
                    ddlCiudad.SelectedValue = dt.Tables[0].Rows[0][11].ToString();
                    txtDireccion.Text = dt.Tables[0].Rows[0][12].ToString();
                    txtFonoCasa.Text = dt.Tables[0].Rows[0][13].ToString();
                    txtFonoOficina.Text = dt.Tables[0].Rows[0][14].ToString();
                    txtCelular.Text = dt.Tables[0].Rows[0][15].ToString();
                    txtEmail.Text = dt.Tables[0].Rows[0][16].ToString();
                    chkEstado.Text = dt.Tables[0].Rows[0][17].ToString();
                    chkEstado.Checked = dt.Tables[0].Rows[0][17].ToString() == "Activo" ? true : false;
                    TxtFechaIniCobertura.Text = dt.Tables[0].Rows[0][19].ToString();
                    TxtFechaFinCobertura.Text = dt.Tables[0].Rows[0][20].ToString();
                    if (dt.Tables[1].Rows.Count > 0)
                    {
                        DataTable dtBeneficiario = dt.Tables[1];
                        ViewState["tblBeneficiarios"] = dtBeneficiario;
                        grdvDatos.DataSource = dtBeneficiario;
                        grdvDatos.DataBind();
                    }
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
                objparam[0] = 6;
                ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlProvincia.DataTextField = "Descripcion";
                ddlProvincia.DataValueField = "Codigo";
                ddlProvincia.DataBind();

                ddlProvinciaBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                ddlProvinciaBen.DataTextField = "Descripcion";
                ddlProvinciaBen.DataValueField = "Codigo";
                ddlProvinciaBen.DataBind();

                Array.Resize(ref objparam, 3);
                objparam[0] = ddlProvincia.SelectedValue;
                objparam[1] = "";
                objparam[2] = 4;
                ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlCiudad.DataTextField = "Descripcion";
                ddlCiudad.DataValueField = "Codigo";
                ddlCiudad.DataBind();

                objparam[0] = ddlProvinciaBen.SelectedValue;
                objparam[1] = "";
                objparam[2] = 4;
                ddlCiudadBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlCiudadBen.DataTextField = "Descripcion";
                ddlCiudadBen.DataValueField = "Codigo";
                ddlCiudadBen.DataBind();

                Array.Resize(ref objparam, 1);
                objparam[0] = "TIPO DOCUMENTOS";
                ddlTipoDocumento.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlTipoDocumento.DataTextField = "Descripcion";
                ddlTipoDocumento.DataValueField = "Valor";
                ddlTipoDocumento.DataBind();

                ddlTipoDocumentoBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlTipoDocumentoBen.DataTextField = "Descripcion";
                ddlTipoDocumentoBen.DataValueField = "Valor";
                ddlTipoDocumentoBen.DataBind();

                Array.Resize(ref objparam, 1);
                objparam[0] = "GENERO";
                ddlGenero.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlGenero.DataTextField = "Descripcion";
                ddlGenero.DataValueField = "Valor";
                ddlGenero.DataBind();

                ddlGeneroBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlGeneroBen.DataTextField = "Descripcion";
                ddlGeneroBen.DataValueField = "Valor";
                ddlGeneroBen.DataBind();

                objparam[0] = "ESTADO CIVIL";
                ddlEstadoCivil.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlEstadoCivil.DataTextField = "Descripcion";
                ddlEstadoCivil.DataValueField = "Valor";
                ddlEstadoCivil.DataBind();

                ddlEstadoCivilBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlEstadoCivilBen.DataTextField = "Descripcion";
                ddlEstadoCivilBen.DataValueField = "Valor";
                ddlEstadoCivilBen.DataBind();

                objparam[0] = "PARENTESCO";
                ddlParentesco.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlParentesco.DataTextField = "Descripcion";
                ddlParentesco.DataValueField = "Valor";
                ddlParentesco.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        private void funCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    ddlCiudad.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlProvincia.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 4;
                    ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlCiudad.DataTextField = "Descripcion";
                    ddlCiudad.DataValueField = "Codigo";
                    ddlCiudad.DataBind();
                    break;
                case 2:
                    ddlCiudadBen.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlProvinciaBen.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 4;
                    ddlCiudadBen.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlCiudadBen.DataTextField = "Descripcion";
                    ddlCiudadBen.DataValueField = "Codigo";
                    ddlCiudadBen.DataBind();
                    break;
            }
        }

        private void funLimpiarCampos()
        {
            lblerror.Text = "";
            txtEdadBen.Text = "";
            hidEdad.Value = "";
            hidEdadB.Value = "";
            ddlTipoDocumentoBen.SelectedIndex = 0;
            txtNumeroDocumentoBen.Text = "";
            txtPrimerNombreB.Text = "";
            txtSegundoNombreB.Text = "";
            txtPrimerApellidoB.Text = "";
            txtSegundoApellidoB.Text = "";
            ddlGeneroBen.SelectedIndex = 0;
            ddlEstadoCivilBen.SelectedIndex = 0;
            txtFechaNacBen.Text = DateTime.Now.ToString("MM/dd/yyyy");
            txtEdad.Text = "";
            ddlProvinciaBen.SelectedIndex = 0;
            funCascadaCombos(0);
            txtDireccionBen.Text = "";
            txtFonoCasaBen.Text = "";
            txtFonoOficBen.Text = "";
            txtCelularBen.Text = "";
            txtEmailBen.Text = "";
            ddlParentesco.SelectedIndex = 0;
            chkEstadoBen.Checked = true;
            chkEstadoBen.Text = "Activo";
        }

        private void funLimpiarDatos()
        {
            tbBeneficiarios.Columns.Add("PrimerNombre");
            tbBeneficiarios.Columns.Add("SegundoNombre");
            tbBeneficiarios.Columns.Add("PrimerApellido");
            tbBeneficiarios.Columns.Add("SegundoApellido");
            tbBeneficiarios.Columns.Add("Beneficiario");
            tbBeneficiarios.Columns.Add("Parentesco");
            tbBeneficiarios.Columns.Add("ParentescoCod");
            tbBeneficiarios.Columns.Add("Estado");
            tbBeneficiarios.Columns.Add("CodigoBen");
            tbBeneficiarios.Columns.Add("TipoDocumentoBen");
            tbBeneficiarios.Columns.Add("NumeroDocumentoBen");
            tbBeneficiarios.Columns.Add("GeneroBen");
            tbBeneficiarios.Columns.Add("EstadoCivilBen");
            tbBeneficiarios.Columns.Add("FechaNacimientoBen");
            tbBeneficiarios.Columns.Add("ProvinciaBen");
            tbBeneficiarios.Columns.Add("CiudadBen");
            tbBeneficiarios.Columns.Add("DireccionBen");
            tbBeneficiarios.Columns.Add("FonoCasaBen");
            tbBeneficiarios.Columns.Add("FonoOficinaBen");
            tbBeneficiarios.Columns.Add("CelularBen");
            tbBeneficiarios.Columns.Add("EmailBen");
            tbBeneficiarios.Columns.Add("Modificado");
            ViewState["tblBeneficiarios"] = tbBeneficiarios;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
        }

        private void funGenerarArchivoABF()
        {
            try
            {
                Array.Resize(ref objparam, 17);
                objparam[0] = 3;
                objparam[1] = 0;
                objparam[2] = int.Parse(ViewState["CodTitular"].ToString()); ;
                objparam[3] = 0;
                objparam[4] = int.Parse(ViewState["CodProducto"].ToString());
                objparam[5] = txtNumeroDocumento.Text.Trim();
                objparam[6] = txtPrimerNombre.Text.Trim().ToUpper();
                objparam[7] = txtSegundoNombre.Text.Trim().ToUpper();
                objparam[8] = txtPrimerApellido.Text.Trim().ToUpper();
                objparam[9] = txtSegundoApellido.Text.Trim().ToUpper();
                objparam[10] = txtFechaNacimiento.Text.Trim();
                objparam[11] = ddlGenero.SelectedValue;
                objparam[12] = "";
                objparam[13] = "T";
                objparam[14] = 0;
                objparam[15] = int.Parse(Session["usuCodigo"].ToString());
                objparam[16] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_GenerarArchivosABF", objparam);

                string[] columnas = new[] {"PrimerNombre", "SegundoNombre","PrimerApellido", "SegundoApellido", "Beneficiario", "Parentesco", "FechaNacimientoBen",
                "GeneroBen","CodigoBen","Modificado"};
                dtBene = (DataTable)ViewState["tblBeneficiarios"];
                DataView view = new DataView(dtBene);
                dtBene = view.ToTable(true, columnas);
                DataRow[] dr = dtBene.Select("Modificado='SI'");
                foreach (DataRow row in dr)
                {
                    objparam[6] = row[0].ToString();
                    objparam[7] = row[1].ToString();
                    objparam[8] = row[2].ToString();
                    objparam[9] = row[3].ToString();
                    objparam[12] = row[5].ToString();
                    objparam[10] = row[6].ToString();
                    objparam[11] = row[7].ToString() == "MASCULINO" ? "M" : "F";
                    objparam[3] = row[9].ToString() == "NV" ? 0 : int.Parse(row[8].ToString());
                    objparam[13] = "B";
                    dt = new Conexion(2, "").funConsultarSqls("sp_GenerarArchivosABF", objparam);
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        #endregion

        #region Botones y Eventos
        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(1);
        }

        protected void imgSelecc_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                ViewState["index"] = intIndex;
                ddlTipoDocumentoBen.SelectedValue = grdvDatos.DataKeys[intIndex].Values["TipoDocumentoBen"].ToString();
                txtNumeroDocumentoBen.Text = grdvDatos.DataKeys[intIndex].Values["NumeroDocumentoBen"].ToString();
                ViewState["Beneficiario"] = grdvDatos.Rows[intIndex].Cells[0].Text.Trim().Replace("&#209;","Ñ");
                txtPrimerNombreB.Text = grdvDatos.DataKeys[intIndex].Values["PrimerNombre"].ToString();
                txtSegundoNombreB.Text = grdvDatos.DataKeys[intIndex].Values["SegundoNombre"].ToString();
                txtPrimerApellidoB.Text = grdvDatos.DataKeys[intIndex].Values["PrimerApellido"].ToString();
                txtSegundoApellidoB.Text = grdvDatos.DataKeys[intIndex].Values["SegundoApellido"].ToString();
                ddlGeneroBen.SelectedValue = grdvDatos.DataKeys[intIndex].Values["GeneroBen"].ToString();
                ddlEstadoCivilBen.SelectedValue = grdvDatos.DataKeys[intIndex].Values["EstadoCivilBen"].ToString();
                txtFechaNacBen.Text = grdvDatos.DataKeys[intIndex].Values["FechaNacimientoBen"].ToString();
                txtEdad.Text = fun.Edad(DateTime.ParseExact(txtFechaNacBen.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString();
                ddlProvinciaBen.SelectedValue = grdvDatos.DataKeys[intIndex].Values["ProvinciaBen"].ToString();
                funCascadaCombos(2);
                ddlCiudadBen.SelectedValue = grdvDatos.DataKeys[intIndex].Values["CiudadBen"].ToString();
                txtDireccionBen.Text = grdvDatos.DataKeys[intIndex].Values["DireccionBen"].ToString();
                txtFonoCasaBen.Text = grdvDatos.DataKeys[intIndex].Values["FonoCasaBen"].ToString();
                txtFonoOficBen.Text = grdvDatos.DataKeys[intIndex].Values["FonoOficinaBen"].ToString();
                txtCelularBen.Text = grdvDatos.DataKeys[intIndex].Values["CelularBen"].ToString();
                txtEmailBen.Text = grdvDatos.DataKeys[intIndex].Values["EmailBen"].ToString();
                ddlParentesco.SelectedValue = grdvDatos.DataKeys[intIndex].Values["ParentescoCod"].ToString();
                ViewState["Parentesco"] = grdvDatos.Rows[intIndex].Cells[1].Text;
                chkEstadoBen.Text = grdvDatos.Rows[intIndex].Cells[2].Text;
                chkEstadoBen.Checked = grdvDatos.Rows[intIndex].Cells[2].Text == "Activo" ? true : false;
                ViewState["CodigoBeneficiario"] = grdvDatos.DataKeys[intIndex].Values["CodigoBen"].ToString();
                txtEdadBen.Text = new Funciones().Edad(DateTime.ParseExact(txtFechaNacBen.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString();
                imgModificar.Enabled = true;
                imgAgregar.Enabled = false;
                imgCancelar.Enabled = true;
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[intIndex].BackColor = System.Drawing.Color.DarkGray;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            bool lexiste = false;
            lblerror.Text = "";
            if (string.IsNullOrEmpty(txtPrimerNombreB.Text.Trim()))
            {
                lblerror.Text = "Ingrese al menos primer Nombre del Beneficiario..!";
                return;
            }
            if (string.IsNullOrEmpty(txtPrimerApellidoB.Text.Trim()))
            {
                lblerror.Text = "Ingrese al menos primer Apellido del Beneficiario..!";
                return;
            }
            if (!fun.IsDateNew(txtFechaNacBen.Text))
            {
                lblerror.Text = "Fecha de Nacimiento Beneficiario incorrecta..!";
                return;
            }
            if (!string.IsNullOrEmpty(txtFonoCasaBen.Text.Trim()))
            {
                if (txtFonoCasaBen.Text.Trim().Substring(0, 2) == "09" || txtFonoCasaBen.Text.Trim().Length < 7)
                {
                    lblerror.Text = "Teléfono Casa Beneficiario Incorrecto..!";
                    return;
                }
            }
            if (!string.IsNullOrEmpty(txtFonoOficBen.Text.Trim()))
            {
                if (txtFonoOficBen.Text.Trim().Substring(0, 2) == "09" || txtFonoOficBen.Text.Trim().Length < 7)
                {
                    lblerror.Text = "Teléfono Oficina Beneficiario Incorrecto..!";
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtCelularBen.Text.Trim()))
            {
                if (txtCelularBen.Text.Trim().Substring(0, 2) != "09" || txtCelularBen.Text.Trim().Length < 10)
                {
                    lblerror.Text = "Celular Beneficiario Incorrecto..!";
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtEmailBen.Text.Trim()))
            {
                if (!new Funciones().email_bien_escrito(txtEmailBen.Text.Trim()))
                {
                    txtEmailBen.Text = "";
                    lblerror.Text = "Email Beneficiario Incorrecto..!";
                    return;
                }
            }
            DateTime dtmFechaNacimiento = DateTime.ParseExact(string.Format("{0}", txtFechaNacBen.Text), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (dtmFechaNacimiento > dtmFechaActual)
            {
                lblerror.Text = "La Fecha de Nacimiento no puede ser mayor a la actual..!";
                return;
            }
            try
            {
                if (ViewState["tblBeneficiarios"] != null)
                {
                    string beneficiario = txtPrimerApellidoB.Text.Trim().ToUpper() + " " + txtSegundoApellidoB.Text.Trim().ToUpper() +
                        " " + txtPrimerNombreB.Text.Trim().ToUpper() + " " + txtSegundoNombreB.Text.Trim().ToUpper();
                    DataTable tblbuscar = (DataTable)ViewState["tblBeneficiarios"];
                    DataRow result = tblbuscar.Select("Beneficiario='" + beneficiario + "' and Parentesco='" + ddlParentesco.SelectedItem.ToString() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "CodigoBen";
                    if (result != null) lexiste = true;
                    foreach (DataRow dr in tblbuscar.Rows)
                    {
                        maxCodigo = int.Parse(dr[8].ToString());
                    }
                }
                else
                {
                    maxCodigo = 0;
                }

                if (lexiste)
                {
                    lblerror.Text = "Ya existe Beneficario ingresado..!";
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblBeneficiarios"];
                DataRow filagre = tblagre.NewRow();
                filagre["PrimerNombre"] = txtPrimerNombreB.Text.Trim().ToUpper();
                filagre["SegundoNombre"] = txtSegundoNombreB.Text.Trim().ToUpper();
                filagre["PrimerApellido"] = txtPrimerApellidoB.Text.Trim().ToUpper();
                filagre["SegundoApellido"] = txtSegundoApellidoB.Text.Trim().ToUpper();
                filagre["Beneficiario"] = txtPrimerApellidoB.Text.Trim().ToUpper() + " " + txtSegundoApellidoB.Text.Trim().ToUpper() + " " +
                                            txtPrimerNombreB.Text.Trim().ToUpper() + " " + txtSegundoNombreB.Text.Trim().ToUpper();
                filagre["Parentesco"] = ddlParentesco.SelectedItem.ToString();
                filagre["ParentescoCod"] = ddlParentesco.SelectedValue;
                filagre["Estado"] = chkEstadoBen.Checked ? "Activo" : "Inactivo";
                filagre["CodigoBen"] = 0;
                filagre["TipoDocumentoBen"] = ddlTipoDocumentoBen.SelectedValue;
                filagre["NumeroDocumentoBen"] = txtNumeroDocumentoBen.Text;
                filagre["GeneroBen"] = ddlGeneroBen.SelectedValue;
                filagre["EstadoCivilBen"] = ddlEstadoCivilBen.SelectedValue;
                filagre["FechaNacimientoBen"] = DateTime.ParseExact(txtFechaNacBen.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                filagre["ProvinciaBen"] = int.Parse(ddlProvinciaBen.SelectedValue);
                filagre["CiudadBen"] = int.Parse(ddlCiudadBen.SelectedValue);
                filagre["DireccionBen"] = txtDireccionBen.Text.ToUpper();
                filagre["FonoCasaBen"] = txtFonoCasaBen.Text;
                filagre["FonoOficinaBen"] = txtFonoOficBen.Text;
                filagre["CelularBen"] = txtCelularBen.Text;
                filagre["EmailBen"] = txtEmailBen.Text;
                filagre["Modificado"] = "SI";
                tblagre.Rows.Add(filagre);
                ViewState["tblBeneficiarios"] = tblagre;
                tblagre.DefaultView.Sort = "Beneficiario";
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                imgCancelar.Enabled = true;
                funLimpiarCampos();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgModificar_Click(object sender, ImageClickEventArgs e)
        {
            bool lexiste = false;
            lblerror.Text = "";
            if (string.IsNullOrEmpty(txtPrimerNombreB.Text.Trim()))
            {
                lblerror.Text = "Ingrese al menos un Nombre del Beneficiario..!";
                return;
            }
            if (string.IsNullOrEmpty(txtPrimerApellidoB.Text.Trim()))
            {
                lblerror.Text = "Ingrese al menos un Apellido del Beneficiario..!";
                return;
            }
            if (!fun.IsDateNew(txtFechaNacBen.Text))
            {
                lblerror.Text = "Fecha de Nacimiento Beneficiario Incorrecta..!";
                return;
            }
            if (!string.IsNullOrEmpty(txtFonoCasaBen.Text.Trim()))
            {
                if (txtFonoCasaBen.Text.Trim().Substring(0, 2) == "09" || txtFonoCasaBen.Text.Trim().Length < 7)
                {
                    lblerror.Text = "Teléfono Casa Beneficiario Incorrecto..!";
                    return;
                }
            }
            if (!string.IsNullOrEmpty(txtFonoOficBen.Text.Trim()))
            {
                if (txtFonoOficBen.Text.Trim().Substring(0, 2) == "09" || txtFonoOficBen.Text.Trim().Length < 7)
                {
                    lblerror.Text = "Teléfono Oficina Beneficiario Incorrecto..!";
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtCelularBen.Text.Trim()))
            {
                if (txtCelularBen.Text.Trim().Substring(0, 2) != "09" || txtCelularBen.Text.Trim().Length < 10)
                {
                    lblerror.Text = "Celular Beneficiario Incorrecto..!";
                    return;
                }
            }

            if (!string.IsNullOrEmpty(txtEmailBen.Text.Trim()))
            {
                if (!new Funciones().email_bien_escrito(txtEmailBen.Text.Trim()))
                {
                    txtEmailBen.Text = "";
                    lblerror.Text = "Email Beneficiario Incorrecto..!";
                    return;
                }
            }
            DateTime dtmFechaNacimiento = DateTime.ParseExact(string.Format("{0}", txtFechaNacBen.Text), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dtmFechaActual = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (dtmFechaNacimiento > dtmFechaActual)
            {
                lblerror.Text = "La Fecha de Nacimiento no puede ser mayor a la actual..!";
                return;
            }
            try
            {
                if (ViewState["tblBeneficiarios"] != null)
                {
                    if (ViewState["Beneficiario"].ToString() == txtPrimerApellidoB.Text.Trim().ToUpper() + " " + txtSegundoApellidoB.Text.Trim().ToUpper() + " " +
                            txtPrimerNombreB.Text.Trim().ToUpper() + " " + txtSegundoNombreB.Text.Trim().ToUpper()
                                && ViewState["Parentesco"].ToString() == ddlParentesco.SelectedItem.ToString()) lexiste = false;
                    else
                    {
                        DataTable tblbuscar = (DataTable)ViewState["tblBeneficiarios"];
                        DataRow[] result = tblbuscar.Select("Beneficiario='" + txtPrimerApellidoB.Text.Trim().ToUpper() + " " + txtSegundoApellidoB.Text.Trim().ToUpper() + " " +
                                            txtPrimerNombreB.Text.Trim().ToUpper() + " " + txtSegundoNombreB.Text.Trim().ToUpper() +
                                            "' and Parentesco='" + ddlParentesco.SelectedItem.ToString() + "'");
                        lexiste = result.Length == 0 ? false : true;
                    }
                }
                if (lexiste)
                {
                    lblerror.Text = "Ya existe otro Beneficiario con los mismos datos..!";
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblBeneficiarios"];
                tblagre.Rows.RemoveAt(int.Parse(ViewState["index"].ToString()));
                DataRow filagre = tblagre.NewRow();
                filagre["PrimerNombre"] = txtPrimerNombreB.Text.Trim().ToUpper();
                filagre["SegundoNombre"] = txtSegundoNombreB.Text.Trim().ToUpper();
                filagre["PrimerApellido"] = txtPrimerApellidoB.Text.Trim().ToUpper();
                filagre["SegundoApellido"] = txtSegundoApellidoB.Text.Trim().ToUpper();
                filagre["Beneficiario"] = txtPrimerApellidoB.Text.Trim().ToUpper() + " " + txtSegundoApellidoB.Text.Trim().ToUpper() + " " +
                                            txtPrimerNombreB.Text.Trim().ToUpper() + " " + txtSegundoNombreB.Text.Trim().ToUpper();
                filagre["Parentesco"] = ddlParentesco.SelectedItem.ToString();
                filagre["ParentescoCod"] = ddlParentesco.SelectedValue;
                filagre["Estado"] = chkEstadoBen.Checked ? "Activo" : "Inactivo";
                filagre["CodigoBen"] = int.Parse(ViewState["CodigoBeneficiario"].ToString());
                filagre["TipoDocumentoBen"] = ddlTipoDocumentoBen.SelectedValue;
                filagre["NumeroDocumentoBen"] = txtNumeroDocumentoBen.Text;
                filagre["GeneroBen"] = ddlGeneroBen.SelectedValue;
                filagre["EstadoCivilBen"] = ddlEstadoCivilBen.SelectedValue;
                filagre["FechaNacimientoBen"] = DateTime.ParseExact(txtFechaNacBen.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy");
                filagre["ProvinciaBen"] = int.Parse(ddlProvinciaBen.SelectedValue);
                filagre["CiudadBen"] = int.Parse(ddlCiudadBen.SelectedValue);
                filagre["DireccionBen"] = txtDireccionBen.Text.ToUpper();
                filagre["FonoCasaBen"] = txtFonoCasaBen.Text;
                filagre["FonoOficinaBen"] = txtFonoOficBen.Text;
                filagre["CelularBen"] = txtCelularBen.Text;
                filagre["EmailBen"] = txtEmailBen.Text;
                filagre["Modificado"] = "SI";
                tblagre.Rows.Add(filagre);
                ViewState["tblBeneficiarios"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    grdvDatos.Rows[row.RowIndex].BackColor = System.Drawing.Color.White;
                }
                funLimpiarCampos();
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
            funLimpiarCampos();
            imgAgregar.Enabled = true;
            imgCancelar.Enabled = false;
            imgModificar.Enabled = false;
            funLimpiarDatos();
            funCargaMantenimiento(int.Parse(ViewState["CodTitular"].ToString()));
        }

        protected void ddlProvinciaBen_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(2);
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";
                btnGrabar.Enabled = false;
                if (string.IsNullOrEmpty(txtNumeroDocumento.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese No. Documento Titular..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtPrimerNombre.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombres del Titular..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtPrimerApellido.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Apellidos del Titular..!", this);
                    return;
                }
                if (!new Funciones().IsDateNew(txtFechaNacimiento.Text))
                {
                    new Funciones().funShowJSMessage("Fecha de Nacimiento Titular Incorrecta (formato dd-MM-yyyy)..!", this);
                    return;
                }
                if (!string.IsNullOrEmpty(txtFonoCasa.Text.Trim()))
                {
                    if (txtFonoCasa.Text.Trim().Substring(0, 2) == "09" || txtFonoCasa.Text.Trim().Length < 7)
                    {
                        new Funciones().funShowJSMessage("Teléfono Casa Titular incorrecto..!", this);
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(txtFonoOficina.Text.Trim()))
                {
                    if (txtFonoOficina.Text.Trim().Substring(0, 2) == "09" || txtFonoOficina.Text.Trim().Length < 7)
                    {
                        new Funciones().funShowJSMessage("Teléfono Oficina Titular incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(txtCelular.Text.Trim()))
                {
                    if (txtCelular.Text.Trim().Substring(0, 2) != "09" || txtCelular.Text.Trim().Length < 10)
                    {
                        new Funciones().funShowJSMessage("Celular Titular incorrecto..!", this);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(txtEmail.Text.Trim()))
                {
                    if (!new Funciones().email_bien_escrito(txtEmail.Text.Trim()))
                    {
                        txtEmail.Text = "";
                        new Funciones().funShowJSMessage("Email incorrecto..!", this);
                        return;
                    }
                }

                if (fun.Edad(DateTime.ParseExact(string.Format("{0}", txtFechaNacimiento.Text), "dd/MM/yyyy", CultureInfo.InvariantCulture)) < 0)
                {
                    new Funciones().funShowJSMessage("Verifique la Edad del Titular..!", this);
                    return;
                }
                //System.Threading.Thread.Sleep(300);
                Array.Resize(ref objparam, 23);
                objparam[0] = int.Parse(ViewState["CodigoPersona"].ToString());
                objparam[1] = int.Parse(ViewState["CodTitular"].ToString());
                objparam[2] = int.Parse(ViewState["CodProducto"].ToString());
                objparam[3] = ddlTipoDocumento.SelectedValue;
                objparam[4] = txtNumeroDocumento.Text.Trim();
                objparam[5] = txtPrimerNombre.Text.Trim().ToUpper();
                objparam[6] = txtSegundoNombre.Text.Trim().ToUpper();
                objparam[7] = txtPrimerApellido.Text.Trim().ToUpper();
                objparam[8] = txtSegundoApellido.Text.Trim().ToUpper();
                objparam[9] = ddlGenero.SelectedValue;
                objparam[10] = ddlEstadoCivil.SelectedValue;
                objparam[11] = txtFechaNacimiento.Text;//DateTime.ParseExact(txtFechaNacimiento.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                objparam[12] = int.Parse(ddlCiudad.SelectedValue);
                objparam[13] = txtDireccion.Text.Trim().ToUpper();
                objparam[14] = txtFonoCasa.Text.Trim();
                objparam[15] = txtFonoOficina.Text.Trim();
                objparam[16] = txtCelular.Text.Trim();
                objparam[17] = txtEmail.Text.Trim();
                objparam[18] = chkEstado.Checked;
                objparam[19] = TxtFechaIniCobertura.Text;
                objparam[20] = TxtFechaFinCobertura.Text;
                objparam[21] = int.Parse(Session["usuCodigo"].ToString());
                objparam[22] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertarTitulares(objparam, (DataTable)ViewState["tblBeneficiarios"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "")
                {
                    funGenerarArchivoABF();
                    ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.opener.location='FrmAgendarCitaMedica.aspx?CodigoTitular=" +
                        ViewState["CodTitular"].ToString() + "&CodigoProducto=" + ViewState["CodProducto"].ToString() + "&Regresar=" + ViewState["Regresar"].ToString() +
                        "&MensajeRetornado=Actualizado con Éxito" + "';window.close();", true);
                }
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.opener.location='FrmAgendarCitaMedica?CodigoTitular=" + ViewState["CodTitular"].ToString() + "&CodigoProducto=" + ViewState["CodProducto"].ToString() + "';window.close();", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "pop", "javascript:window.close();", true);
        }

        protected void chkEstadoBen_CheckedChanged(object sender, EventArgs e)
        {
            chkEstadoBen.Text = chkEstadoBen.Checked ? "Activo" : "Inactivo";
        }

        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked ? "Activo" : "Inactivo";
        }
        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNumeroDocumento.Text = "";
            if (ddlTipoDocumento.SelectedValue == "C")
            {
                txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
            }
            else
            {
                txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
            }
        }
        protected void txtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            funLimpiarCampos();
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ViewState["CodProducto"].ToString());
            objparam[1] = txtNumeroDocumento.Text.Trim();
            objparam[2] = 23;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                lblerror.Text = "Ya existe un registro con ese número de documento..!";
                txtNumeroDocumento.Text = "";
                return;
            }

            objparam[0] = 0;
            objparam[1] = txtNumeroDocumento.Text.Trim();
            objparam[2] = 22;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                ddlTipoDocumento.SelectedValue = dt.Tables[0].Rows[0][0].ToString();
                if (ddlTipoDocumento.SelectedValue == "C")
                {
                    txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.ValidChars;
                    txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-";
                    txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Numbers;
                }
                else
                {
                    txtNumeroDocumento_FilteredTextBoxExtender.FilterMode = AjaxControlToolkit.FilterModes.InvalidChars;
                    txtNumeroDocumento_FilteredTextBoxExtender.InvalidChars = ".-*/{{}}[[]]\\";
                    txtNumeroDocumento_FilteredTextBoxExtender.FilterType = AjaxControlToolkit.FilterTypes.Custom;
                }
                txtNumeroDocumento.Text = dt.Tables[0].Rows[0][1].ToString();
                txtPrimerNombre.Text = dt.Tables[0].Rows[0][2].ToString();
                txtSegundoNombre.Text = dt.Tables[0].Rows[0][3].ToString();
                txtPrimerApellido.Text = dt.Tables[0].Rows[0][4].ToString();
                txtSegundoApellido.Text = dt.Tables[0].Rows[0][5].ToString();
                ddlGenero.SelectedValue = dt.Tables[0].Rows[0][6].ToString();
                ddlEstadoCivil.SelectedValue = dt.Tables[0].Rows[0][7].ToString();
                txtFechaNacimiento.Text = dt.Tables[0].Rows[0][8].ToString();
                txtEdad.Text = dt.Tables[0].Rows[0][9].ToString();
                ddlProvincia.SelectedValue = dt.Tables[0].Rows[0][10].ToString();
                funCascadaCombos(1);
                ddlCiudad.SelectedValue = dt.Tables[0].Rows[0][11].ToString();
                txtDireccion.Text = dt.Tables[0].Rows[0][12].ToString();
                txtFonoCasa.Text = dt.Tables[0].Rows[0][13].ToString();
                txtFonoOficina.Text = dt.Tables[0].Rows[0][14].ToString();
                txtCelular.Text = dt.Tables[0].Rows[0][15].ToString();
                txtEmail.Text = dt.Tables[0].Rows[0][16].ToString();
                funLimpiarDatos();
            }
        }
        #endregion

    }
}