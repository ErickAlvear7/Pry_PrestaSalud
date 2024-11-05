namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using System;
    using System.Data;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmPrespuestoRealizadoAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        int intIndex = 0;
        string strCodigo = "", strPaciente = "", strTituCodigo = "", strBeneCodigo = "", strMedico = "", strCodigoPrestadora = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                if (Session["SalirAgenda"].ToString() == "NO")
                {
                    if (Session["TipoCita"].ToString() == "CitaMedica")
                        Response.Redirect("~/CitaMedica/FrmAgendarCitaMedica.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);

                    if (Session["TipoCita"].ToString() == "CitaOdontologica")
                        Response.Redirect("~/CitaOdontologica/FrmAgendarCitaOdonto.aspx?Tipo=" + "E" + "&CodigoTitular=" + Session["CodigoTitular"].ToString() + "&CodigoProducto=" + Session["CodigoProducto"].ToString(), true);
                    return;
                }
                if (Request["CodigoMedico"] != null)
                {
                    ViewState["CodigoMedico"] = Request["CodigoMedico"];
                    FunCargaMantenimiento(int.Parse(ViewState["CodigoMedico"].ToString()));
                }
                else
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[1] = "";
                    objparam[2] = 68;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        ViewState["CodigoMedico"] = dt.Tables[0].Rows[0][0].ToString();
                        FunCargaMantenimiento(int.Parse(ViewState["CodigoMedico"].ToString()));
                    }
                    else lbltitulo.Text = "Sin Registro del Médico, Consulte con el Administrador..!";
                }                
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Funciones y Procedimiento
        protected void FunCargaMantenimiento(int codigomedico)
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = codigomedico;
                objparam[1] = "";
                objparam[2] = 62;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    grdvDatos.DataSource = dt;
                    grdvDatos.DataBind();
                    grdvDatos.UseAccessibleHeader = true;
                    grdvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;
                }
                ViewState["grdvDatos"] = grdvDatos.DataSource;
                objparam[2] = 69;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "Presupuesto-Registrados " +dt.Tables[0].Rows[0][0].ToString();
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
            intIndex = gvRow.RowIndex;
            strCodigo = grdvDatos.DataKeys[intIndex].Values["CodigoCabecera"].ToString();
            strTituCodigo = grdvDatos.DataKeys[intIndex].Values["TituCodigo"].ToString();
            strBeneCodigo = grdvDatos.DataKeys[intIndex].Values["BeneCodigo"].ToString();
            strCodigoPrestadora = grdvDatos.DataKeys[intIndex].Values["CodigoPrestadora"].ToString();
            Response.Redirect("FrmPresupuestosRegistrados.aspx?CodigoCabecera=" + strCodigo + "&CodigoMedico=" + ViewState["CodigoMedico"].ToString() + "&TituCodigo=" + strTituCodigo + "&BeneCodigo=" + strBeneCodigo + "&CodigoPrestadora=" + strCodigoPrestadora + "&Regresar=0", true);
            
        }
        protected void imgPrint_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            strCodigo = grdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString();
            strPaciente = grdvDatos.DataKeys[gvRow.RowIndex].Values["Paciente"].ToString();
            strMedico = grdvDatos.DataKeys[gvRow.RowIndex].Values["Medico"].ToString();
            strCodigoPrestadora = grdvDatos.DataKeys[gvRow.RowIndex].Values["CodigoPrestadora"].ToString();
            Response.Redirect("RptPresupuestoRegistrado.aspx?CodigoCabecera=" + strCodigo + "&CodigoPrestadora=" + strCodigoPrestadora + "&Regresar=0", true);
        }
        #endregion
    }
}