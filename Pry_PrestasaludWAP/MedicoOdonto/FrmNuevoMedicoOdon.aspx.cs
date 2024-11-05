using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.MedicoOdonto
{
    public partial class FrmNuevoMedicoOdon : System.Web.UI.Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Funciones fun = new Funciones();
        DataTable tbMedEspePresta = new DataTable();
        int maxCodigo = 0;
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                try
                {
                    ViewState["codigomedico"] = Request["Codigo"];
                    tbMedEspePresta.Columns.Add("Prestadora");
                    tbMedEspePresta.Columns.Add("Especialidad");
                    tbMedEspePresta.Columns.Add("medicod");
                    tbMedEspePresta.Columns.Add("preecodigo");
                    tbMedEspePresta.Columns.Add("Codigo");
                    tbMedEspePresta.Columns.Add("Estado");
                    ViewState["tblMesEspePresta"] = tbMedEspePresta;
                    funCargarCombo();
                    if (Request["Tipo"] == "N")
                    {
                        lbltitulo.Text = "Agregar Nuevo Médico Odontológico";
                    }
                    else
                    {
                        Label11.Visible = true;
                        chkEstado.Visible = true;
                        lbltitulo.Text = "Editar Médico Odontológico";
                        funCargaMantenimiento(int.Parse(ViewState["codigomedico"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void funCargaMantenimiento(int codigo)
        {
            try
            {
                Array.Resize(ref objparam, 2);
                objparam[0] = codigo;
                objparam[1] = 1;
                dt = new Conexion(2, "").funConsultarSqls("sp_CargarMedicoEdit", objparam);
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    txtNombres.Text = dt.Tables[0].Rows[0][0].ToString();
                    txtApellidos.Text = dt.Tables[0].Rows[0][1].ToString();
                    ddlGenero.SelectedValue = dt.Tables[0].Rows[0][2].ToString();
                    txtDireccion.Text = dt.Tables[0].Rows[0][3].ToString();
                    txtFono1.Text = dt.Tables[0].Rows[0][4].ToString();
                    txtFono2.Text = dt.Tables[0].Rows[0][5].ToString();
                    txtCelular1.Text = dt.Tables[0].Rows[0][6].ToString();
                    txtCelular2.Text = dt.Tables[0].Rows[0][7].ToString();
                    txtEmail1.Text = dt.Tables[0].Rows[0][8].ToString();
                    txtEmail2.Text = dt.Tables[0].Rows[0][9].ToString();
                    chkEstado.Text = dt.Tables[0].Rows[0][10].ToString();
                    chkEstado.Checked = dt.Tables[0].Rows[0][10].ToString() == "Activo" ? true : false;
                    chkEnviar1.Checked = dt.Tables[0].Rows[0][11].ToString() == "Si" ? true : false;
                    chkEnviar2.Checked = dt.Tables[0].Rows[0][12].ToString() == "Si" ? true : false;
                }
                Session["grdvDatos"] = dt.Tables[1];
                ViewState["tblMesEspePresta"] = dt.Tables[1];
                grdvDatos.DataSource = dt.Tables[1];
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funCargarCombo()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = "";
                objparam[2] = 19;
                ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlPrestadora.DataTextField = "Descripcion";
                ddlPrestadora.DataValueField = "Codigo";
                ddlPrestadora.DataBind();

                objparam[0] = 0;
                objparam[1] = "";
                objparam[2] = 9;
                ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ddlEspecialidad.DataTextField = "Descripcion";
                ddlEspecialidad.DataValueField = "Codigo";
                ddlEspecialidad.DataBind();

                Array.Resize(ref objparam, 1);
                objparam[0] = "GENERO";
                ddlGenero.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargarDetalleParametroporNombre", objparam);
                ddlGenero.DataTextField = "Descripcion";
                ddlGenero.DataValueField = "Valor";
                ddlGenero.DataBind();
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
                case 0:
                    break;
                case 1:
                    ddlPrestadora.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = 0;
                    objparam[1] = "";
                    objparam[2] = 19;
                    ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlPrestadora.DataTextField = "Descripcion";
                    ddlPrestadora.DataValueField = "Codigo";
                    ddlPrestadora.DataBind();
                    break;
                case 3:
                    ddlEspecialidad.Items.Clear();
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlPrestadora.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 9;
                    ddlEspecialidad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlEspecialidad.DataTextField = "Descripcion";
                    ddlEspecialidad.DataValueField = "Codigo";
                    ddlEspecialidad.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void chkEnviar1_CheckedChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (chkEnviar1.Checked)
            {
                if (new Funciones().email_bien_escrito(txtEmail1.Text) == false || txtEmail1.Text == "")
                {
                    lblerror.Text = "Ingrese un mail válido por favor..!";
                    chkEnviar1.Checked = false;
                }
            }
        }

        protected void chkEnviar2_CheckedChanged(object sender, EventArgs e)
        {
            lblerror.Text = "";
            if (chkEnviar2.Checked)
            {
                if (new Funciones().email_bien_escrito(txtEmail2.Text) == false || txtEmail2.Text == "")
                {
                    lblerror.Text = "Ingrese un mail válido por favor..!";
                    chkEnviar2.Checked = false;
                }
            }
        }

        protected void ddlPrestadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            //funCascadaCombos(3);
        }

        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                bool lexiste = false;
                lblerror.Text = "";

                if (ddlPrestadora.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Prestadora..!", this);
                    return;
                }
                if (ddlEspecialidad.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Especialidad..!", this);
                    return;
                }

                if (ViewState["tblMesEspePresta"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tblMesEspePresta"];
                    DataRow result = tblbuscar.Select("Prestadora='" + ddlPrestadora.SelectedItem.ToString() + "' and Especialidad='" + ddlEspecialidad.SelectedItem.ToString() + "'").FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "Codigo";
                    if (result != null) lexiste = true;
                    foreach (DataRow dr in tblbuscar.Rows)
                    {
                        maxCodigo = int.Parse(dr[4].ToString());
                    }
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe Asignado El Médico a Especialidad/Prestadora..!", this);
                    return;
                }
                System.Threading.Thread.Sleep(100);
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tblMesEspePresta"];
                DataRow filagre = tblagre.NewRow();
                filagre["Prestadora"] = ddlPrestadora.SelectedItem.ToString();
                filagre["Especialidad"] = ddlEspecialidad.SelectedItem.ToString();
                filagre["medicod"] = ViewState["codigomedico"].ToString();
                filagre["preecodigo"] = ddlPrestadora.SelectedValue;
                filagre["Codigo"] = maxCodigo + 1;
                filagre["Estado"] = "Activo";
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Prestadora";
                ViewState["tblMesEspePresta"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                ddlEspecialidad.SelectedIndex = 0;
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
            if (e.Row.RowIndex >= 0)
            {
                chkest = (CheckBox)(e.Row.Cells[2].FindControl("chkEstadoMed"));
                lblest = (Label)(e.Row.Cells[3].FindControl("lblEstado"));

                int codigo = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                Array.Resize(ref objparam, 3);
                objparam[0] = codigo;
                objparam[1] = "";
                objparam[2] = 20;
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

        protected void chkEstadoMed_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            Label lblest = new Label();
            CheckBox chkest = new CheckBox();

            chkest = (CheckBox)(gvRow.Cells[2].FindControl("chkEstadoMed"));
            lblest = (Label)(gvRow.Cells[3].FindControl("lblEstado"));
            lblest.Text = chkest.Checked ? "Activo" : "Inactivo";
            tbMedEspePresta = (DataTable)ViewState["tblMesEspePresta"];
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["Codigo"].ToString());
            DataRow[] result = tbMedEspePresta.Select("Codigo=" + codigo);
            result[0]["Estado"] = chkest.Checked ? "Activo" : "Inactivo";
            tbMedEspePresta.AcceptChanges();
        }

        protected void imgCancelar_Click(object sender, ImageClickEventArgs e)
        {
            ddlPrestadora.SelectedIndex = 0;
            ddlEspecialidad.SelectedIndex = 0;
            imgCancelar.Enabled = false;
            tbMedEspePresta.Columns.Add("Prestadora");
            tbMedEspePresta.Columns.Add("Especialidad");
            tbMedEspePresta.Columns.Add("medicod");
            tbMedEspePresta.Columns.Add("preecodigo");
            tbMedEspePresta.Columns.Add("Codigo");
            tbMedEspePresta.Columns.Add("Estado");
            ViewState["tblMesEspePresta"] = tbMedEspePresta;
            grdvDatos.DataSource = null;
            grdvDatos.DataBind();
            funCargaMantenimiento(int.Parse(ViewState["codigomedico"].ToString()));
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNombres.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre del Médico..!", this);
                    return;
                }
                if (string.IsNullOrEmpty(txtApellidos.Text))
                {
                    new Funciones().funShowJSMessage("Ingrese Apellido del Médico..!", this);
                    return;
                }
                Array.Resize(ref objparam, 17);
                objparam[0] = int.Parse(ViewState["codigomedico"].ToString());
                objparam[1] = txtNombres.Text.ToUpper();
                objparam[2] = txtApellidos.Text.ToUpper();
                objparam[3] = ddlGenero.SelectedValue;
                objparam[4] = txtDireccion.Text.ToUpper();
                objparam[5] = txtFono1.Text;
                objparam[6] = txtFono2.Text;
                objparam[7] = txtCelular1.Text;
                objparam[8] = txtCelular2.Text;
                objparam[9] = txtEmail1.Text;
                objparam[10] = txtEmail2.Text;
                objparam[11] = chkEstado.Checked == true ? 1 : 0;
                objparam[12] = chkEnviar1.Checked;
                objparam[13] = chkEnviar2.Checked;
                objparam[14] = int.Parse(Session["usuCodigo"].ToString());
                objparam[15] = Session["MachineName"].ToString();
                objparam[16] = 1;
                dt = new Conexion(2, "").FunInsertarMedicoEspePresta(objparam, (DataTable)ViewState["tblMesEspePresta"]);
                if (dt.Tables[0].Rows[0][0].ToString() == "Existe") lblerror.Text = "Ya existe médico con el mismo nombre y apellido, ingrese uno nuevo..!";
                else Response.Redirect("FrmMedicoOdonAdmin.aspx?MensajeRetornado='Guardado con Éxito'", false);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMedicoOdonAdmin.aspx", false);
        }
        #endregion
    }
}