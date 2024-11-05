namespace Pry_PrestasaludWAP.Medicos
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmAtencionCitaMedica : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtb = new DataTable();
        DataTable tbCIE10 = new DataTable();
        Object[] objparam = new Object[1];
        string codigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                tbCIE10.Columns.Add("Codigo");
                tbCIE10.Columns.Add("Descripcion");
                ViewState["tblCIE10"] = tbCIE10;
                ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm:ss");
                ViewState["CodigoCIE10"] = "";
                //ViewState["DiaActual"] = DateTime.Now.ToString("dddd");
                lbltitulo.Text = "Registro Atención - Médica";
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["CodigoTitu"] = Request["CodigoTitu"];
                ViewState["CodigoBene"] = Request["CodigoBene"];
                funCargaMantenimiento();
            }            
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[1] = "";
                objparam[2] = 39;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatosTitular.DataSource = dt;
                grdvDatosTitular.DataBind();
                objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[1] = "";
                objparam[2] = 40;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                grdvDatosCita.DataSource = dt;
                grdvDatosCita.DataBind();
                Array.Resize(ref objparam, 9);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoBene"].ToString());
                objparam[3] = "";
                objparam[4] = "";
                objparam[5] = "";
                objparam[6] = 0;
                objparam[7] = 0;
                objparam[8] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_HistorialCitas", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        protected void buscarcie10()
        {
            dtb = (DataTable)ViewState["CIE10"];
            DataRow[] resul = dtb.Select("Descripcion Like '%" + txtBuscar.Text + "%'");
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
            lstCIE10.DataSource = tblagre;
            lstCIE10.DataTextField = tblagre.Columns[1].ColumnName;
            lstCIE10.DataValueField = tblagre.Columns[0].ColumnName;
            lstCIE10.DataBind();
        }
        #endregion

        #region Botones y Eventos
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(ViewState["CodigoCIE10"].ToString().Trim()))
                {
                    new Funciones().funShowJSMessage("Seleccione CIE10", this);
                    return;
                }
                Array.Resize(ref objparam, 14);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[2] = "T";
                objparam[3] = 99;
                objparam[4] = ViewState["CodigoCIE10"].ToString().ToString();
                objparam[5] = txtEditor.Content;
                objparam[6] = DateTime.Now.ToString("MM/dd/yyyy");
                objparam[7] = ViewState["HoraActual"].ToString();
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                objparam[13] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", objparam);
                if (dt.Tables[0].Rows.Count > 0) Response.Redirect("FrmMedicoCitasAdmin.aspx?MensajeRetornado='Guardado con Éxito..!'", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }

        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmMedicoCitasAdmin.aspx", true);
        }
        protected void imgBuscar_Click(object sender, ImageClickEventArgs e)
        {
            txtEditor.Content = "";
            lstCIE10.Height = 180;
            ViewState["CodigoCIE10"] = "";
            if (!string.IsNullOrEmpty(txtBuscar.Text.Trim()))
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = 0;
                objparam[1] = txtBuscar.Text.Trim().ToUpper();
                objparam[2] = 118;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lstCIE10.DataSource = dt;
                lstCIE10.DataTextField = "Descripcion";
                lstCIE10.DataValueField = "Codigo";
                lstCIE10.DataBind();
            }
            else new Funciones().funShowJSMessage("Ingrese Texto para Buscar..!", this);
        }

        protected void lstCIE10_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCIE10.Height = 30;
            ViewState["CodigoCIE10"] = lstCIE10.SelectedValue;
            txtEditor.Content = lstCIE10.SelectedItem.ToString();
            ListItem itemx = new ListItem();
            itemx.Text = lstCIE10.SelectedItem.ToString();
            itemx.Value = ViewState["CodigoCIE10"].ToString();
            lstCIE10.Items.Clear();
            lstCIE10.Items.Add(itemx);
        }
        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                codigo = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                foreach (GridViewRow fr in grdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Teal;
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(codigo);
                objparam[1] = "";
                objparam[2] = 121;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                txtEditor.Enabled = false;
                txtEditor.Content = dt.Tables[0].Rows[0]["Detalle"].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void ImgVademecum_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updCabecera, GetType(), "Mostrar Datos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmVademecumAdmin.aspx" + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=900px, height=450px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }
        #endregion
    }
}