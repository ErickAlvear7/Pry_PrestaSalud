using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Reportes
{
    public partial class Frm_RepExpertDoctorV1 : Page
    {
        #region Variables
        Object[] objparam = new Object[1];
        DataSet ds = new DataSet();
        string sentencia1 = "", sentencia2 = "", motivoAgenda = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(this.imgExportar);
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
                txtFechaIni.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtFechaFin.Text = DateTime.Now.ToString("yyyy-MM-dd");
                lbltitulo.Text = "Reportes Expert Doctor <BASICO 1.0>";
                FunCascadaCombos(0);
                FunCascadaCombos(1);
            }
            else grdvDatos.DataSource = ViewState["grdvDatos"];
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCascadaCombos(int opcion)
        {
            switch (opcion)
            {
                case 0:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 38;
                    //ddlTipoAgenda.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    //ddlTipoAgenda.DataTextField = "Descripcion";
                    //ddlTipoAgenda.DataValueField = "Codigo";
                    //ddlTipoAgenda.DataBind();
                    break;
                case 1:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 51;
                    ddlCliente.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlCliente.DataTextField = "Descripcion";
                    ddlCliente.DataValueField = "Codigo";
                    ddlCliente.DataBind();
                    break;
            }
        }
        #endregion

        #region Botones y Eventos
        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            //if (ddlTipoAgenda.SelectedValue == "0")
            //{
            //    new Funciones().funShowJSMessage("Seleccione Tipo Agenda..!", this);
            //    return; 
            //}

            if (!new Funciones().IsDateNewx(txtFechaIni.Text))
            {
                new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                return;
            }

            if (!new Funciones().IsDateNewx(txtFechaFin.Text))
            {
                new Funciones().funShowJSMessage("No es una fecha válida..!", this);
                return;
            }

            if (DateTime.ParseExact(txtFechaIni.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture) > DateTime.ParseExact(txtFechaFin.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture))
            {
                new Funciones().funShowJSMessage("La Fecha de Inicio no puede ser mayor a la Fecha de Fin..!", this);
                return;
            }

            #region Programacion Orientada
            //if (rdbFechaRegistro.Checked) sentencia1 = "where convert(date,CI.cita_fechacreacion,101) between ";
            //if (rdbFechaCita.Checked) sentencia1 = "where convert(date,HI.hici_fechacita,101) between ";
            ////CONSULTAR TODOS
            //if (ddlTipoAgenda.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlMotivoCita.SelectedValue == "0") 
            //    sentencia2 = "";

            ////CONSULTAR POR TIPO AGENDA
            //if (ddlTipoAgenda.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlMotivoCita.SelectedValue == "0")
            //{
            //    if (ddlTipoAgenda.SelectedItem.ToString() != "")
            //        //sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue + "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "''";
            //        //sentencia2 = "and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "''";
            //        sentencia2 = "and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue + "''";
            //}

            ////CONSULTAR POR TIPO CLIENTE
            //if (ddlTipoAgenda.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlMotivoCita.SelectedValue == "0")
            //    sentencia2 = "and CI.cita_tipocliente=''" + ddlTipoCliente.SelectedValue + "''";

            ////CONSULTAR TIPO AGENDA Y TIPO CLIENTE
            //if (ddlTipoAgenda.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlMotivoCita.SelectedValue == "0")
            //    sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue +
            //        "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and CI.cita_tipocliente=''" + ddlTipoCliente.SelectedValue + "''";

            ////CONSULTAR TIPO AGENDA Y MOTIVO
            //if (ddlTipoAgenda.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlMotivoCita.SelectedValue != "0")
            //{
            //    if (ddlTipoAgenda.SelectedValue == "C")
            //    {
            //        sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue +
            //            "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and HI.hici_motivo=" +
            //            ddlMotivoCita.SelectedValue;
            //    }
            //    else
            //    {
            //        sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue +
            //            "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and HI.hici_descripcioncita=''" +
            //            ddlMotivoCita.SelectedValue + "''";
            //    }
            //}

            ////CONSULTAR TIPO AGENDA - TIPO CLIENTE - MOTIVO
            //if (ddlTipoAgenda.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlMotivoCita.SelectedValue != "0")
            //{
            //    if (ddlTipoAgenda.SelectedValue == "C")
            //    {
            //        sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue +
            //            "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and CI.cita_tipocliente=''" +
            //            ddlTipoCliente.SelectedValue + "'' and HI.hici_motivo=" + ddlMotivoCita.SelectedValue;
            //    }
            //    else
            //    {
            //        sentencia2 = "and CI.cita_estatuscita=''" + ddlTipoAgenda.SelectedValue +
            //            "'' and HI.hici_estadocita=''" + ddlTipoAgenda.SelectedValue + "'' and CI.cita_tipocliente=''" +
            //            ddlTipoCliente.SelectedValue + "'' and HI.hici_descripcioncita=''" + ddlMotivoCita.SelectedValue + "''";
            //    }
            //}
            ////CONSULTAR POR TIPO DE CLIENTE
            //if (ddlCliente.SelectedValue != "0")
            //{
            //    sentencia2 = " and PR.CAMP_CODIGO=" + ddlCliente.SelectedValue;
            //}
            #endregion

            Array.Resize(ref objparam, 5);
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 0;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 1;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 2;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue == "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 3;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 4;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "A") objparam[0] = 5;
            //if (ddlCliente.SelectedValue == "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 6;
            //if (ddlCliente.SelectedValue != "0" && ddlTipoCliente.SelectedValue != "0" && ddlTipoAgenda.SelectedValue == "C") objparam[0] = 7;

            if (ddlCliente.SelectedValue == "0") objparam[0] = 0;
            if (ddlCliente.SelectedValue != "0")  objparam[0] = 1;

            //System.Threading.Thread.Sleep(500);            
            objparam[1] = txtFechaIni.Text;
            objparam[2] = txtFechaFin.Text;
            objparam[3] = int.Parse(ddlCliente.SelectedValue.ToString());
            objparam[4] = "0";


            ds = new Conexion(2, "").FunReportesDoctorV1(objparam);


            if (ds.Tables[0].Rows.Count > 0 || ds.Tables[0] != null )
            {
                grdvDatos.DataSource = ds;
                grdvDatos.DataBind();
                ViewState["grdvDatos"] = grdvDatos.DataSource;

                imgExportar.Visible = true;
                lblExportar.Visible = true;
            }else
            {
                imgExportar.Visible = false;
                lblExportar.Visible = false;
            }
        }
        
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void imgExportar_Click(object sender, ImageClickEventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            string FileName = "ReporteExpertDoctor_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                grdvDatos.AllowPaging = false;
                grdvDatos.DataSource = (DataSet)ViewState["grdvDatos"];
                grdvDatos.DataBind();                
                //grdvDatos.HeaderRow.BackColor = Color.White;
                foreach (GridViewRow row in grdvDatos.Rows)
                {
                    //row.BackColor = Color.White;
                    row.Cells[7].Style.Add("mso-number-format", "\\@");
                    //row.Cells[2].Style.Add("mso-number-format", "\\@");
                    //row.Cells[3].Style.Add("mso-number-format", "\\@");
                    //row.Cells[15].Style.Add("mso-number-format", "\\@");
                }
                grdvDatos.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        protected void grdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdvDatos.PageIndex = e.NewPageIndex;
            grdvDatos.DataBind();
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mantenedor/FrmDetalle.aspx", true);
        }
        #endregion
    }
}