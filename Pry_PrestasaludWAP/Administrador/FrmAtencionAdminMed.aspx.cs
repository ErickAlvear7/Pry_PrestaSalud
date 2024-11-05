namespace Pry_PrestasaludWAP.Administrador
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmAtencionAdminMed : Page
    {
        #region Variables
        DataSet _dts = new DataSet();
        DataTable _dtb = new DataTable();
        DataTable _tbCIE10 = new DataTable();
        Object[] _objparam = new Object[1];
        string _codigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                _tbCIE10.Columns.Add("Codigo");
                _tbCIE10.Columns.Add("Descripcion");
                ViewState["tblCIE10"] = _tbCIE10;
                ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm:ss");
                ViewState["CodigoCIE10"] = "";
                //ViewState["DiaActual"] = DateTime.Now.ToString("dddd");
                Lbltitulo.Text = "Registro Atención - Médica";
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["CodigoTitu"] = Request["CodigoTitu"];
                ViewState["CodigoBene"] = Request["CodigoBene"];
                ViewState["CodigoMedi"] = Request["CodigoMedi"];
                FunCargaMantenimiento();
            }
        } 
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(ViewState["CodigoTitu"].ToString());
                _objparam[1] = "";
                _objparam[2] = 39;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                GrdvDatosTitular.DataSource = _dts;
                GrdvDatosTitular.DataBind();
                _objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[1] = "";
                _objparam[2] = 40;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                GrdvDatosCita.DataSource = _dts;
                GrdvDatosCita.DataBind();
                Array.Resize(ref _objparam, 9);
                _objparam[0] = 0;
                _objparam[1] = int.Parse(ViewState["CodigoTitu"].ToString());
                _objparam[2] = int.Parse(ViewState["CodigoBene"].ToString());
                _objparam[3] = "";
                _objparam[4] = "";
                _objparam[5] = "";
                _objparam[6] = 0;
                _objparam[7] = 0;
                _objparam[8] = 0;
                _dts = new Conexion(2, "").funConsultarSqls("sp_HistorialCitas", _objparam);
                GrdvDatos.DataSource = _dts;
                GrdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                Lbltitulo.Text = ex.ToString();
            }
        }

        protected void Buscarcie10()
        {
            _dtb = (DataTable)ViewState["CIE10"];
            DataRow[] resul = _dtb.Select("Descripcion Like '%" + TxtBuscar.Text + "%'");
            DataTable tblagre = new DataTable();
            tblagre = (DataTable)ViewState["tblCIE10"];
            tblagre.Clear();
            foreach (DataRow dr in resul)
            {
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = dr[1].ToString();
                filagre["Descripcion"] = dr[0].ToString();
                tblagre.Rows.Add(filagre);
            }
            LstCIE10.DataSource = tblagre;
            LstCIE10.DataTextField = tblagre.Columns[1].ColumnName;
            LstCIE10.DataValueField = tblagre.Columns[0].ColumnName;
            LstCIE10.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void ImgVademecum_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('../Medicos/FrmVademecumAdmin.aspx" + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=900px, height=450px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }

        protected void ImgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            TxtEditor.Content = "";
            LstCIE10.Height = 180;
            ViewState["CodigoCIE10"] = "";
            if (!string.IsNullOrEmpty(TxtBuscar.Text.Trim()))
            {
                Array.Resize(ref _objparam, 3);
                _objparam[0] = 0;
                _objparam[1] = TxtBuscar.Text.Trim().ToUpper();
                _objparam[2] = 118;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                LstCIE10.DataSource = _dts;
                LstCIE10.DataTextField = "Descripcion";
                LstCIE10.DataValueField = "Codigo";
                LstCIE10.DataBind();
            }
            else new Funciones().funShowJSMessage("Ingrese Texto para Buscar..!", this);
        }

        protected void ImgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                _codigo = GrdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                foreach (GridViewRow fr in GrdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                GrdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Teal;
                Array.Resize(ref _objparam, 3);
                _objparam[0] = int.Parse(_codigo);
                _objparam[1] = "";
                _objparam[2] = 121;
                _dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", _objparam);
                TxtEditor.Enabled = false;
                TxtEditor.Content = _dts.Tables[0].Rows[0]["Detalle"].ToString();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void LstCIE10_SelectedIndexChanged(object sender, EventArgs e)
        {
            LstCIE10.Height = 30;
            ViewState["CodigoCIE10"] = LstCIE10.SelectedValue;
            TxtEditor.Content = LstCIE10.SelectedItem.ToString();
            ListItem itemx = new ListItem();
            itemx.Text = LstCIE10.SelectedItem.ToString();
            itemx.Value = ViewState["CodigoCIE10"].ToString();
            LstCIE10.Items.Clear();
            LstCIE10.Items.Add(itemx);
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ViewState["CodigoCIE10"].ToString().Trim()))
                {
                    new Funciones().funShowJSMessage("Seleccione CIE10", this);
                    return;
                }
                Array.Resize(ref _objparam, 14);
                _objparam[0] = 0;
                _objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                _objparam[2] = "T";
                _objparam[3] = 99;
                _objparam[4] = ViewState["CodigoCIE10"].ToString().ToString();
                _objparam[5] = TxtEditor.Content;
                _objparam[6] = DateTime.Now.ToString("MM/dd/yyyy");
                _objparam[7] = ViewState["HoraActual"].ToString();
                _objparam[8] = "";
                _objparam[9] = "";
                _objparam[10] = 0;
                _objparam[11] = 0;
                _objparam[12] = int.Parse(ViewState["CodigoMedi"].ToString());
                _objparam[13] = Session["MachineName"].ToString();
                _dts = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", _objparam);
                if (_dts.Tables[0].Rows.Count > 0) Response.Redirect("FrmCitaAdministrador.aspx?MensajeRetornado=Guardado con Éxito..!", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmCitaAdministrador.aspx", true);
        } 
        #endregion
    }
}