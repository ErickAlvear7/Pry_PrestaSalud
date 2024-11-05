namespace Pry_PrestasaludWAP.Medicos
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmGestionesRealizadasAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        int intIndex = 0;
        string strCodigo = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                objparam[1] = "";
                objparam[2] = 115;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                ViewState["CodigoMedico"] = dt.Tables[0].Rows[0]["Codigo"].ToString();
                ViewState["NombreMedico"] = dt.Tables[0].Rows[0]["Medico"].ToString();
                funCargaMantenimiento(int.Parse(ViewState["CodigoMedico"].ToString()));
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimiento
        protected void funCargaMantenimiento(int codigomedico)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = codigomedico;
                objparam[1] = "";
                objparam[2] = 116;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatos.DataSource = dt;
                    grdvDatos.DataBind();
                    grdvDatos.UseAccessibleHeader = true;
                    grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                ViewState["grdvDatos"] = grdvDatos.DataSource;
                lbltitulo.Text = "Citas-Registradas " + ViewState["NombreMedico"];
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            strCodigo = grdvDatos.DataKeys[gvRow.RowIndex].Values["CitaCodigo"].ToString();
            Response.Redirect("FrmDetalleGestionesRealizadas.aspx?CodigoCita=" + strCodigo + "&Regresar=0", true);
        }
        protected void imgPrint_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            strCodigo = grdvDatos.DataKeys[gvRow.RowIndex].Values["CitaCodigo"].ToString();
            Response.Redirect("RptRegistroAgendas.aspx?CodigoCita=" + strCodigo + "&Regresar=0", true);

        }
        #endregion
    }
}