namespace Pry_PrestasaludWAP.SupervisionOdonto
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmSuperOdonto : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        DataTable tbDatosAsignados = new DataTable();
        DataTable tbNuevosDatosAsignados = new DataTable();
        ListItem presta = new ListItem();
        ListItem ciud = new ListItem();
        ListItem medico = new ListItem();
        Image imgEstado = new Image();
        CheckBox chkselecc = new CheckBox();
        string estado = "", mensaje = "";
        int codigoPermiso = 0, codigoDetalle = 0, maxCodigo = 0;
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
                    tbDatosAsignados.Columns.Add("Ciudad");
                    tbDatosAsignados.Columns.Add("Prestadora");
                    tbDatosAsignados.Columns.Add("Medico");
                    tbDatosAsignados.Columns.Add("Logueo");
                    tbDatosAsignados.Columns.Add("Estado");
                    tbDatosAsignados.Columns.Add("CodigoCiudad");
                    tbDatosAsignados.Columns.Add("CodigoPrestadora");
                    tbDatosAsignados.Columns.Add("CodigoMedico");
                    tbDatosAsignados.Columns.Add("CodigoPerm");
                    tbDatosAsignados.Columns.Add("CodigoDeta");
                    ViewState["tbDatosAsignados"] = tbDatosAsignados;
                    grdvDatos.DataSource = tbDatosAsignados;
                    grdvDatos.DataBind();
                    funCascadaCombos(0);
                    ViewState["CodigoSupervisor"] = Request["CodigoSupervisor"];
                    ViewState["CodigoPerm"] = "0";
                    if (int.Parse(ViewState["CodigoSupervisor"].ToString()) == 0)
                    {
                        lbltitulo.Text = "Asignar Supervisor - Odontólogo";
                        funCascadaCombos(3);
                        funCargarDatosAsignados();
                    }
                    else
                    {
                        lbltitulo.Text = "Modificar Supervisor - Odontólogo";
                        ddlSupervisor.Enabled = false;
                        lblEstado.Visible = true;
                        chkEstado.Visible = true;
                        funCascadaCombos(4);
                        ddlSupervisor.SelectedValue = ViewState["CodigoSupervisor"].ToString();
                        funCargarMantenimiento();
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
        private void funCargarMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(ddlSupervisor.SelectedValue);
            objparam[1] = "";
            objparam[2] = 72;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                ViewState["CodigoPerm"] = dt.Tables[0].Rows[0][0].ToString();
                txtDescripcion.Text = dt.Tables[0].Rows[0][1].ToString();
                chkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                chkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
                funCargarDatosAsignados();
            }
        }

        private void funCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    ddlCiudad.Items.Clear();
                    ciud.Text = "--Seleccione Ciudad--";
                    ciud.Value = "0";
                    ddlCiudad.Items.Add(ciud);

                    ddlPrestadora.Items.Clear();
                    presta.Text = "--Seleccione Prestadora--";
                    presta.Value = "0";
                    ddlPrestadora.Items.Add(presta);

                    ddlMedicos.Items.Clear();
                    medico.Text = "--Seleccione Medico--";
                    medico.Value = "0";
                    ddlMedicos.Items.Add(medico);

                    Array.Resize(ref objparam, 1);
                    objparam[0] = 10;
                    ddlProvincia.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlProvincia.DataTextField = "Descripcion";
                    ddlProvincia.DataValueField = "Codigo";
                    ddlProvincia.DataBind();

                    break;
                case 1:
                    ddlPrestadora.Items.Clear();
                    presta.Text = "--Seleccione Prestadora--";
                    presta.Value = "0";
                    ddlPrestadora.Items.Add(presta);

                    ddlMedicos.Items.Clear();
                    medico.Text = "--Seleccione Medico--";
                    medico.Value = "0";
                    ddlMedicos.Items.Add(medico);

                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ddlProvincia.SelectedValue);
                    objparam[1] = "";
                    objparam[2] = 30;
                    ddlCiudad.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlCiudad.DataTextField = "Descripcion";
                    ddlCiudad.DataValueField = "Codigo";
                    ddlCiudad.DataBind();
                    break;
                case 2:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = ddlCiudad.SelectedValue;
                    objparam[1] = "";
                    objparam[2] = 49;
                    ddlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlPrestadora.DataTextField = "Descripcion";
                    ddlPrestadora.DataValueField = "Codigo";
                    ddlPrestadora.DataBind();
                    break;
                case 3:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 30;
                    ddlSupervisor.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlSupervisor.DataTextField = "Descripcion";
                    ddlSupervisor.DataValueField = "Codigo";
                    ddlSupervisor.DataBind();
                    break;
                case 4:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 31;
                    ddlSupervisor.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlSupervisor.DataTextField = "Descripcion";
                    ddlSupervisor.DataValueField = "Codigo";
                    ddlSupervisor.DataBind();
                    break;
                case 5:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ddlPrestadora.SelectedValue);
                    objparam[1] = "";
                    objparam[2] = 95;
                    ddlMedicos.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlMedicos.DataTextField = "Descripcion";
                    ddlMedicos.DataValueField = "Codigo";
                    ddlMedicos.DataBind();
                    break;

            }
        }

        private void funCargarDatosAsignados()
        {
            try
            {
                funLimpiarCampos();
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPerm"].ToString());
                objparam[1] = "";
                objparam[2] = 70;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
                ViewState["tbDatosAsignados"] = dt.Tables[0];
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        private void funLimpiarCampos()
        {
            tbDatosAsignados.Clear();
            ViewState["tbDatosCita"] = tbDatosAsignados;
            grdvDatos.DataSource = tbDatosAsignados;
            grdvDatos.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void chkSelecc_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;

            Image imgEstado = new Image();
            CheckBox chkest = new CheckBox();
            tbDatosAsignados = (DataTable)ViewState["tbDatosAsignados"];

            chkest = (CheckBox)(gvRow.Cells[2].FindControl("chkSelecc"));
            imgEstado = (Image)(gvRow.Cells[3].FindControl("imgEstado"));
            if (chkest.Checked)
            {
                imgEstado.ImageUrl = "~/Botones/medico.png";
                imgEstado.Height = 20;
            }
            else
            {
                imgEstado.ImageUrl = "~/Botones/sin_usuario.png";
                imgEstado.Height = 20;
            }
            int codigo = int.Parse(grdvDatos.DataKeys[intIndex].Values["CodigoDeta"].ToString());
            DataRow[] result = tbDatosAsignados.Select("CodigoDeta=" + codigo);
            result[0]["Estado"] = chkest.Checked ? "Asignado" : "No Asignado";
            tbDatosAsignados.AcceptChanges();

        }

        protected void ddlProvincia_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(1);
        }

        protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            funCascadaCombos(2);
        }

        protected void ddlSupervisor_SelectedIndexChanged(object sender, EventArgs e)
        {            
            if (ddlSupervisor.SelectedValue != "0")
            {
                lblerror.Text = "";
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ddlSupervisor.SelectedValue);
                objparam[1] = "";
                objparam[2] = 97;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    lblerror.Text = "Supervisor ya tiene asignaciones..!";
                    ddlSupervisor.SelectedValue = "0";
                }             
            }
        }

        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    chkselecc = (CheckBox)(e.Row.Cells[4].FindControl("chkSelecc"));
                    imgEstado = (Image)(e.Row.Cells[5].FindControl("imgEstado"));
                    codigoPermiso = int.Parse(grdvDatos.DataKeys[e.Row.RowIndex].Values["CodigoPerm"].ToString());
                    estado = grdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    if (codigoPermiso == 0)
                    {
                        chkselecc.Enabled = false;
                        chkselecc.Checked = true;
                    }
                    switch (estado)
                    {
                        case "Asignado":
                            chkselecc.Checked = true;
                            imgEstado.ImageUrl = "~/Botones/medico.png";
                            imgEstado.Height = 20;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void ddlPrestadora_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSupervisor.SelectedValue != "0") funCascadaCombos(5);
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                string[] columnas = new[] { "CodigoPerm", "CodigoMedico", "CodigoCiudad", "CodigoPrestadora", "Estado" };
                tbDatosAsignados = (DataTable)ViewState["tbDatosAsignados"];
                DataView view = new DataView(tbDatosAsignados);
                tbNuevosDatosAsignados = view.ToTable(true, columnas);
                Array.Resize(ref objparam, 5);
                objparam[0] = int.Parse(ddlSupervisor.SelectedValue);
                objparam[1] = txtDescripcion.Text.Trim().ToUpper();
                objparam[2] = chkEstado.Checked;
                objparam[3] = int.Parse(Session["usuCodigo"].ToString());
                objparam[4] = Session["MachineName"].ToString();
                mensaje = new Conexion(2, "").FunInsertarSupervisorOdonto(objparam, tbNuevosDatosAsignados);
                if (mensaje == "") mensaje = "Guardado con Éxito";
                else mensaje = "No se guardó, consulte con el Administrador...";
                Response.Redirect("FrmSuperOdontoAdmin.aspx?MensajeRetornado=" + mensaje, true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void chkEstado_CheckedChanged(object sender, EventArgs e)
        {
            chkEstado.Text = chkEstado.Checked == true ? "Activo" : "Inactivo";
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmSuperOdontoAdmin.aspx", true);
        }
        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            bool lexiste = false;
            lblerror.Text = "";
            if (ddlSupervisor.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Supervisor..!";
                return;
            }

            if (ddlPrestadora.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Prestadora..!";
                return;
            }

            if (ddlMedicos.SelectedValue == "0")
            {
                lblerror.Text = "Seleccione Médico..!";
                return;
            }
            try
            {
                if (ViewState["tbDatosAsignados"] != null)
                {
                    DataTable tblbuscar = (DataTable)ViewState["tbDatosAsignados"];
                    DataRow result = tblbuscar.Select("CodigoPrestadora=" + int.Parse(ddlPrestadora.SelectedValue) + " and CodigoMedico=" + int.Parse(ddlMedicos.SelectedValue)).FirstOrDefault();
                    tblbuscar.DefaultView.Sort = "CodigoDeta";
                    if (result != null) lexiste = true;
                    foreach (DataRow dr in tblbuscar.Rows)
                    {
                        maxCodigo = int.Parse(dr[9].ToString());
                    }
                }

                if (lexiste)
                {
                    lblerror.Text = "Medico en Prestadora ya existe agregado..!";
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tbDatosAsignados"];
                DataRow filagre = tblagre.NewRow();
                filagre["Ciudad"] = ddlCiudad.SelectedItem.ToString();
                filagre["Prestadora"] = ddlPrestadora.SelectedItem.ToString();
                filagre["Medico"] = ddlMedicos.SelectedItem.ToString();
                filagre["Logueo"] = ViewState["Login"].ToString();
                filagre["Estado"] = "Asignado";
                filagre["CodigoCiudad"] = int.Parse(ddlCiudad.SelectedValue);
                filagre["CodigoPrestadora"] = int.Parse(ddlPrestadora.SelectedValue);
                filagre["CodigoMedico"] = int.Parse(ddlMedicos.SelectedValue);
                filagre["CodigoPerm"] = int.Parse(ViewState["CodigoPerm"].ToString());
                filagre["CodigoDeta"] = maxCodigo + 1;
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Medico";
                ViewState["tbDatosAsignados"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
                ddlMedicos.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void ddlMedicos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ddlMedicos.SelectedValue);
                objparam[1] = "";
                objparam[2] = 96;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["Login"] = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                int codigoPerm = int.Parse(grdvDatos.DataKeys[intIndex].Values["CodigoPerm"].ToString());
                int codigoDeta = int.Parse(grdvDatos.DataKeys[intIndex].Values["CodigoDeta"].ToString());
                Array.Resize(ref objparam, 7);
                objparam[0] = 11;
                objparam[1] = codigoPerm;
                objparam[2] = codigoDeta;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tbDatosAsignados"];
                DataRow[] rows;
                rows = tblagre.Select("CodigoDeta=" + codigoDeta + "");
                foreach (DataRow row in rows)
                {
                    tblagre.Rows.Remove(row);
                }
                ViewState["tbDatosAsignados"] = tblagre;
                grdvDatos.DataSource = tblagre;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        #endregion

    }
}