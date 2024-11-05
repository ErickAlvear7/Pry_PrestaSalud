namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmPresupuestosRegistrados : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        ImageButton imgDetalle = new ImageButton();
        string Realizado = "", codigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                
                ViewState["CodigoCabecera"] = Request["CodigoCabecera"];
                ViewState["CodigoMedico"] = Request["CodigoMedico"];
                ViewState["TituCodigo"] = Request["TituCodigo"];
                ViewState["BeneCodigo"] = Request["BeneCodigo"];
                ViewState["CodigoPrestadora"] = Request["CodigoPrestadora"];
                ViewState["Regresar"] = Request["Regresar"];
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
                objparam[0] = int.Parse(ViewState["CodigoCabecera"].ToString());
                objparam[1] = ViewState["CodigoPrestadora"].ToString();
                objparam[2] = 63;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvPresupuesto.DataSource = dt;
                    grdvPresupuesto.DataBind();
                }
                ViewState["grdvDatos"] = grdvDatos.DataSource;
                //Consultar Nombre si es TITULAR o BENEFICIARIO
                if (int.Parse(ViewState["BeneCodigo"].ToString()) == 0)
                {
                    objparam[0] = int.Parse(ViewState["TituCodigo"].ToString());
                    objparam[1] = "";
                    objparam[2] = 76;
                }
                else
                {
                    objparam[0] = int.Parse(ViewState["BeneCodigo"].ToString());
                    objparam[1] = "";
                    objparam[2] = 77;
                }
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "PRESUPUESTOS - REGISTRADOS >> (" + dt.Tables[0].Rows[0][0].ToString() + ")";
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }

        protected void funHistorialProcedimientos(int codigoDetalle)
        {
            try
            {
                Array.Resize(ref objparam, 7);
                objparam[0] = 8;
                objparam[1] = codigoDetalle;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                grdvDatos.DataSource = dt;
                grdvDatos.DataBind();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        #endregion

        #region Botones y Eventos
        protected void imgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            foreach (GridViewRow fr in grdvPresupuesto.Rows)
            {
                fr.Cells[0].BackColor = System.Drawing.Color.White;
            }
            grdvPresupuesto.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Teal;
            codigo = grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoDetalle"].ToString();
            funHistorialProcedimientos(int.Parse(codigo));
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            switch (ViewState["Regresar"].ToString())
            {
                case "0":
                    Response.Redirect("FrmPrespuestoRealizadoAdmin.aspx?CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                    break;
                case "1":
                    Response.Redirect("../Administrador/FrmPresuRealizadoAdministrador.aspx", true);
                    break;
            }            
        }
        protected void grdvPresupuesto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    Realizado = grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Estado"].ToString();
                    imgDetalle = (ImageButton)(e.Row.Cells[7].FindControl("imgDetalle"));
                    if (Realizado == "REALIZADO")
                    {
                        imgDetalle.ImageUrl = "~/Botones/Buscar.png";
                        imgDetalle.Enabled = true;
                        imgDetalle.Height = 20;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void imgVer_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                foreach (GridViewRow fr in grdvDatos.Rows)
                {
                    fr.Cells[0].BackColor = System.Drawing.Color.White;
                }
                grdvDatos.Rows[gvRow.RowIndex].Cells[0].BackColor = System.Drawing.Color.Teal;
                codigo = grdvDatos.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString();
                ScriptManager.RegisterStartupScript(this.updHistorial, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDetalleCitaOdonto.aspx?Codigo=" + codigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=750px, height=400px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}