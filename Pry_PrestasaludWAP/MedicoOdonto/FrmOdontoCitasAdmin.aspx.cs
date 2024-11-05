namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmOdontoCitasAdmin : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        Object[] objparam = new Object[1];
        Image imgEstado = new Image();
        ImageButton imgSelecc = new ImageButton();
        ImageButton imgAusent = new ImageButton();
        string fechaCita = "", horaSistema = "", strResponse = "", fechaActual = "";
        TimeSpan horaActual, horaAgenda;
        int codmedico = 0;
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
                if (Request["CodigoMedico"] != null) ViewState["CodigoMedico"] = int.Parse(Request["CodigoMedico"].ToString());
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
                        funCargaMantenimiento(int.Parse(ViewState["CodigoMedico"].ToString()));
                    }
                    else lbltitulo.Text = "Sin Registro del Médico, Consulte con el Administrador..!";
                }
                                
                if (Request["MensajeRetornado"] != null) SIFunBasicas.Basicas.PresentarMensaje(Page, "::PRESTASALUD::", Request["MensajeRetornado"].ToString());
                
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
                objparam[2] = 56;
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
                lbltitulo.Text = "Atención Citas - Odontológicas " + dt.Tables[0].Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void grdvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    horaSistema = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + "00";
                    fechaCita = grdvDatos.DataKeys[e.Row.RowIndex].Values["FechaCodigo"].ToString();
                    fechaActual = DateTime.Now.ToString("MM/dd/yyyy");
                    horaActual = TimeSpan.Parse(horaSistema);
                    string[] hora = e.Row.Cells[6].Text.Split(new char[] { '-' });
                    horaAgenda = TimeSpan.Parse(hora[0].ToString());
                    imgEstado = (Image)(e.Row.Cells[5].FindControl("imgEstado"));
                    imgSelecc = (ImageButton)(e.Row.Cells[6].FindControl("btnselecc"));
                    imgAusent = (ImageButton)(e.Row.Cells[9].FindControl("imgAusente"));
                    if (DateTime.ParseExact(fechaActual, "MM/dd/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(fechaCita, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    {
                        if (horaActual > horaAgenda)
                        {
                            imgEstado.ImageUrl = "~/Botones/relojoff.png";
                            imgEstado.Height = 20;
                        }
                        else
                        {
                            imgEstado.ImageUrl = "~/Botones/relojon.png";
                            imgEstado.Height = 20;
                        }
                    }
                    else
                    {
                        imgSelecc.ImageUrl = "~/Botones/mueladisable.png";
                        imgSelecc.Enabled = false;
                        imgAusent.Enabled = false;
                        imgSelecc.Height = 20;
                    }
                }
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        protected void btnselecc_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            int intIndex = gvRow.RowIndex;
            string strcodigoCita = grdvDatos.DataKeys[intIndex].Values["CodigoCita"].ToString();
            string strtituCodigo = grdvDatos.DataKeys[intIndex].Values["TituCodigo"].ToString();
            string strbeneCodigo = grdvDatos.DataKeys[intIndex].Values["BeneCodigo"].ToString();
            Response.Redirect("FrmAtencionCitaOdonto.aspx?CodigoCita=" + strcodigoCita + "&CodigoTitu=" + strtituCodigo + "&CodigoBene=" + strbeneCodigo + "&CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
            
        }
        protected void tmrdat_Tick(object sender, EventArgs e)
        {
            funCargaMantenimiento(codmedico);
        }
        protected void imgAusente_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int intIndex = gvRow.RowIndex;
                string strcodigoCita = grdvDatos.DataKeys[intIndex].Values["CodigoCita"].ToString();

                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(strcodigoCita);
                objparam[1] = "";
                objparam[2] = 78;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('No se puede poner ausente, Tiene agregados procedimientos..!');", true);
                    strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "No se puede poner ausente, Tiene agreados procedimientos..!");
                    Response.Redirect(strResponse, false);
                    return;
                }

                Array.Resize(ref objparam, 14);
                objparam[0] = 1;
                objparam[1] = int.Parse(strcodigoCita);
                objparam[2] = "S";
                objparam[3] = 98;
                objparam[4] = "S";
                objparam[5] = "PACIENTE NO ASISTE";
                objparam[6] = DateTime.Now.ToString("MM/dd/yyyy");
                objparam[7] = DateTime.Now.ToString("HH:mm");
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = int.Parse(Session["usuCodigo"].ToString());
                objparam[13] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_RegistraCitaAgendada", objparam);
                strResponse = string.Format("{0}?MensajeRetornado={1}", Request.Url.AbsolutePath, "Guardado con Éxito");
                Response.Redirect(strResponse, false);
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }
        #endregion

    }
}