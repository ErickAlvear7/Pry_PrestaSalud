namespace Pry_PrestasaludWAP.Configuracion
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmNuevoUserAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable _dtbdatos = new DataTable();
        Object[] objparam = new Object[1];
        CheckBox _chkestado = new CheckBox();
        ImageButton _eliminar = new ImageButton();
        string _codigociudad = "", _ciudad = "", _estado = "", _nuevo = "", _codigo = "";
        DataRow _resultado, _filagre;
        int _maxcodigo = 0;
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
                    ViewState["CodigoUSER"] = Request["CodigoUSER"];
                    ViewState["CodigoPERM"] = Request["CodigoPERM"];
                    //ViewState["TipoPresta"] = "M";
                    FunLlenarCombos(0);
                    FunLlenarCombos(1);
                    FunCargarMantenimiento();
                    if (int.Parse(ViewState["CodigoUSER"].ToString()) == 0)
                    {
                        Lbltitulo.Text = "Nuevo Usuario << Administrador Prestadora >>";
                        FunLlenarCombos(3);
                        //funCargarDatosAsignados();
                    }
                    else
                    {
                        Lbltitulo.Text = "Editar Usuario << Administrador Prestadora >>";
                        LblEstado.Visible = true;
                        ChkEstado.Visible = true;
                        DdlUsuario.SelectedValue = ViewState["CodigoUSER"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargarMantenimiento()
        {
            Array.Resize(ref objparam, 3);
            objparam[0] = int.Parse(DdlUsuario.SelectedValue);
            objparam[1] = "";
            objparam[2] = 72;
            dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
            if (dt.Tables[0].Rows.Count > 0)
            {
                ViewState["CodigoPERM"] = dt.Tables[0].Rows[0][0].ToString();
                ChkEstado.Text = dt.Tables[0].Rows[0][2].ToString();
                ChkEstado.Checked = dt.Tables[0].Rows[0][2].ToString() == "Activo" ? true : false;
            }
            FunCargarDatosAsignados();
        }

        private void FunCargarDatosAsignados()
        {
            try
            {
                //funLimpiarCampos();
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoPERM"].ToString());
                objparam[1] = "";
                objparam[2] = 164;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvDatos.DataSource = dt;
                GrdvDatos.DataBind();
                ViewState["DatosPrestadora"] = dt.Tables[0];
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunLlenarCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 52;
                    DdlUsuario.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    DdlUsuario.DataTextField = "Descripcion";
                    DdlUsuario.DataValueField = "Codigo";
                    DdlUsuario.DataBind();
                    break;
                case 1:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = DdlTipoPresta.SelectedValue == "M" ? 53 : 54;
                    DdlPrestadora.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    DdlPrestadora.DataTextField = "Descripcion";
                    DdlPrestadora.DataValueField = "Codigo";
                    DdlPrestadora.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked ? "Activo" : "Inactivo";
        }

        protected void ImgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (DdlUsuario.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Usuario..!", this);
                    return;
                }

                if (DdlPrestadora.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Prestadora..!", this);
                    return;
                }
                //tomar el codigo de ciduad de la prestadora
                Array.Resize(ref objparam, 3);
                objparam[0] = DdlPrestadora.SelectedValue;
                objparam[1] = "";
                objparam[2] = DdlTipoPresta.SelectedValue == "M" ? 165 : 166;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                _codigociudad = dt.Tables[0].Rows[0]["CodigoCIUD"].ToString();
                _ciudad = dt.Tables[0].Rows[0]["Ciudad"].ToString();

                _dtbdatos = (DataTable)ViewState["DatosPrestadora"];
                _resultado = _dtbdatos.Select("CodigoTip='" + DdlTipoPresta.SelectedValue + "' AND CodigoPRES='" + DdlPrestadora.SelectedValue + "'").FirstOrDefault();

                if (_resultado != null)
                {
                    new Funciones().funShowJSMessage("Prestadora ya Existe Agregada..!", this);
                    return;
                }

                if (_dtbdatos.Rows.Count > 0)
                {
                    _maxcodigo = _dtbdatos.AsEnumerable()
                        .Max(row => int.Parse((string)row["CodigoAUXI"]));
                }
                else _maxcodigo = 0;

                _filagre = _dtbdatos.NewRow();
                _filagre["CodigoAUXI"] = _maxcodigo + 1;
                _filagre["CodigoPEDE"] = 0;
                _filagre["CodigoPRES"] = DdlPrestadora.SelectedValue;
                _filagre["CodigoCIUD"] = _codigociudad;
                _filagre["Ciudad"] = _ciudad;
                _filagre["Prestadora"] = DdlPrestadora.SelectedItem.ToString();
                _filagre["Estado"] = "Activo";
                _filagre["CodigoTip"] = DdlTipoPresta.SelectedValue;
                _filagre["TipoPrestadora"] = DdlTipoPresta.SelectedValue == "M" ? "MEDICO" : "ODONTOLOGO";
                _filagre["Nuevo"] = "SI";
                _dtbdatos.Rows.Add(_filagre);
                _dtbdatos.AcceptChanges();
                ViewState["DatosPrestadora"] = _dtbdatos;
                GrdvDatos.DataSource = _dtbdatos;
                GrdvDatos.DataBind();
                DdlPrestadora.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ChkEstadoPres_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoAUXI"].ToString();
                _chkestado = (CheckBox)(gvRow.Cells[3].FindControl("ChkEstadoPres"));
                _dtbdatos = (DataTable)ViewState["DatosPrestadora"];
                _resultado = _dtbdatos.Select("CodigoAUXI='" + _codigo + "'").FirstOrDefault();
                _resultado["Estado"] = _chkestado.Checked ? "Activo" : "Inactivo";
                _dtbdatos.AcceptChanges();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void ImgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoAUXI"].ToString();
                _dtbdatos = (DataTable)ViewState["DatosPrestadora"];
                _resultado = _dtbdatos.Select("CodigoAUXI='" + _codigo + "'").FirstOrDefault();
                _resultado.Delete();
                _dtbdatos.AcceptChanges();
                ViewState["DatosPrestadora"] = _dtbdatos;
                GrdvDatos.DataSource = _dtbdatos;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void DdlTipoPresta_SelectedIndexChanged(object sender, EventArgs e)
        {
            FunLlenarCombos(1);
        }

        protected void GrdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    _chkestado = (CheckBox)(e.Row.Cells[3].FindControl("ChkEstadoPres"));
                    _eliminar = (ImageButton)(e.Row.Cells[4].FindControl("ImgEliminar"));
                    _estado = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    _nuevo = GrdvDatos.DataKeys[e.Row.RowIndex].Values["Nuevo"].ToString();

                    if (_estado == "Activo") _chkestado.Checked = true;
                    if (_nuevo == "SI")
                    {
                        _chkestado.Enabled = false;
                        _eliminar.Enabled = true;
                        _eliminar.ImageUrl = "~/Botones/eliminar.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                _dtbdatos = (DataTable)ViewState["DatosPrestadora"];
                if (_dtbdatos.Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("Ingrese al menos un Prestador..!", this);
                    return;
                }

                //consultar si el usuario ya existe ingresado
                Array.Resize(ref objparam, 14);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoPERM"].ToString());
                objparam[2] = int.Parse(DdlUsuario.SelectedValue);
                objparam[3] = "";
                objparam[4] = ChkEstado.Checked ? "Activo" : "Inactivo";
                objparam[5] = 0;
                objparam[6] = 0;
                objparam[7] = 0;
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = "";
                objparam[11] = "";
                objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                objparam[13] = Session["MachineName"].ToString();

                if (ViewState["CodigoUSER"].ToString() != DdlUsuario.SelectedValue)
                {
                    objparam[0] = 0;
                    dt = new Conexion(2, "").funConsultarSqls("sp_NuevoUsuarioAdmin", objparam);
                    if (dt.Tables[0].Rows[0]["Contar"].ToString() != "0")
                    {
                        new Funciones().funShowJSMessage("Nombre de Usuario Admin Ya Existe..!", this);
                        return;
                    }
                }
                objparam[0] = 1;
                dt = new Conexion(2, "").FunNuevoUsuarioAdmin(objparam);
                objparam[1] = dt.Tables[0].Rows[0]["Codigo"].ToString();
                objparam[0] = 2;
                foreach (DataRow drfila in _dtbdatos.Rows)
                {
                    objparam[5] = drfila["CodigoPEDE"].ToString();
                    objparam[6] = drfila["CodigoPRES"].ToString();
                    objparam[7] = drfila["CodigoCIUD"].ToString();
                    objparam[8] = drfila["Estado"].ToString();
                    objparam[9] = drfila["CodigoTip"].ToString();
                    dt = new Conexion(2, "").FunNuevoUsuarioAdmin(objparam);
                }
                Response.Redirect("FrmCrearUserAdmin.aspx?MensajeRetornado=Guardado con Éxito..!", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCrearUserAdmin.aspx", true);
        } 
        #endregion
    }
}