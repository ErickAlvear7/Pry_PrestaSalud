namespace Pry_PrestasaludWAP.MedicoOdonto
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public partial class FrmAtencionCitaOdonto : Page
    {
        #region Variables
        DataSet dt = new DataSet();
        DataTable dtOrdenado = new DataTable();
        DataTable dtGrabarPresupuesto = new DataTable();
        Object[] objparam = new Object[1];
        DataTable tbPresupuesto = new DataTable();
        ImageButton imgEliminar = new ImageButton();
        ImageButton imgPrioridad = new ImageButton();
        ImageButton imgDetalle = new ImageButton();
        CheckBox chkRealizar = new CheckBox();
        string codigo = "";
        //private decimal _Total = 0;
        int Codigo = 0, CodigoCabecera = 0, CodigoProcedimiento = 0, cobertura = 0, contar = 0, maxCodigo = 0, codigoPrestadora = 0,
            prioridad = 0, codigoanterior = 0, prioridadanterior = 0;
        string inicial = "", Realizado = "", mensaje = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            if (!IsPostBack)
            {
                ViewState["CodigoMedico"] = Request["CodigoMedico"];
                tbPresupuesto.Columns.Add("Codigo");
                tbPresupuesto.Columns.Add("CodigoCabecera");
                tbPresupuesto.Columns.Add("CodigoProcedimiento");
                tbPresupuesto.Columns.Add("Procedimiento");
                tbPresupuesto.Columns.Add("Pieza");
                tbPresupuesto.Columns.Add("Pvp");
                tbPresupuesto.Columns.Add("Costo");
                tbPresupuesto.Columns.Add("Cobertura");
                tbPresupuesto.Columns.Add("Total");                
                tbPresupuesto.Columns.Add("Prioridad");
                tbPresupuesto.Columns.Add("Autorizado");
                tbPresupuesto.Columns.Add("Realizado");
                tbPresupuesto.Columns.Add("Inicial");
                tbPresupuesto.Columns.Add("Realizar");
                tbPresupuesto.Columns.Add("CodigoPrestadora");
                ViewState["tbPresupuesto"] = tbPresupuesto;

                ViewState["HoraActual"] = DateTime.Now.ToString("HH:mm:ss");
                ViewState["CodigoCita"] = Request["CodigoCita"];
                ViewState["CodigoTitu"] = Request["CodigoTitu"];
                ViewState["CodigoBene"] = Request["CodigoBene"];
                ViewState["RegistraHistorial"] = "NO";
                funCargarCombos(1);
                funCargarCombos(3);
                funCargarCombos(4);
                ListItem item = new ListItem();
                item.Text = "--Seleccione Pieza--";
                item.Value = "-1";
                ddlPieza.Items.Add(item);
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
                objparam[0] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[1] = "";
                objparam[2] = 65;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) ViewState["Medico"] = dt.Tables[0].Rows[0][0].ToString();
                else ViewState["Medico"] = "SIN MEDICO/COMUNICAR AL ADMINISTRADOR DEL SISTEMA";

                objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[1] = "";
                objparam[2] = 58;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) ViewState["Paciente"] = dt.Tables[0].Rows[0][0].ToString();
                else ViewState["Paciente"] = "SIN PACIENTE/COMUNICAR AL ADMINISTRADOR DEL SISTEMA";
                objparam[2] = 114;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dt.Tables[0].Rows.Count > 0) ViewState["CodigoPrestadora"] = dt.Tables[0].Rows[0][0].ToString();
                else ViewState["CodigoPrestadora"] = "0";
                objparam[2] = 79;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                lbltitulo.Text = "Cita Odontológica - Paciente: " + ViewState["Paciente"].ToString() + " - Producto: " + dt.Tables[0].Rows[0][0].ToString();

                //LLENAR LOS PRESUPUESTOS REGISTRADOS SI YA EXISTEN
                Array.Resize(ref objparam, 7);
                objparam[0] = 15;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                objparam[0] = 0;
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (dt.Tables[0].Rows.Count == 0) funLlenarPrimerRegistro();
                else
                {
                    prioridad = 0;
                    foreach (DataRow drrow in dt.Tables[0].Rows)
                    {
                        drrow["Prioridad"] = prioridad;
                        prioridad++;
                        dt.Tables[0].AcceptChanges(); 
                    }
                    ViewState["tbPresupuesto"] = dt.Tables[0];
                }
                tbPresupuesto = (DataTable)ViewState["tbPresupuesto"];
                grdvPresupuesto.DataSource = tbPresupuesto;
                grdvPresupuesto.DataBind();
                if (grdvPresupuesto.Rows.Count > 0)
                {
                    imgPrioridad = (ImageButton)grdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    imgPrioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lbltitulo.Text = ex.ToString();
            }
        }

        private void funCargarCombos(int opcion)
        {
            switch (opcion)
            {
                case 1:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                    objparam[1] = "";
                    objparam[2] = 61;
                    ddlProcedimiento.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlProcedimiento.DataTextField = "Descripcion";
                    ddlProcedimiento.DataValueField = "Codigo";
                    ddlProcedimiento.DataBind();
                    break;
                case 2:
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ddlProcedimiento.SelectedValue);
                    objparam[1] = ddlOndotograma.SelectedValue;
                    objparam[2] = 60;
                    ddlPieza.DataSource = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    ddlPieza.DataTextField = "Descripcion";
                    ddlPieza.DataValueField = "Codigo";
                    ddlPieza.DataBind();
                    break;
                case 3:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 28;
                    ddlRegistroOdonto.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlRegistroOdonto.DataTextField = "Descripcion";
                    ddlRegistroOdonto.DataValueField = "Codigo";
                    ddlRegistroOdonto.DataBind();
                    break;
                case 4:
                    Array.Resize(ref objparam, 1);
                    objparam[0] = 32;
                    ddlOndotograma.DataSource = new Conexion(2, "").funConsultarSqls("sp_CargaCombos", objparam);
                    ddlOndotograma.DataTextField = "Descripcion";
                    ddlOndotograma.DataValueField = "Codigo";
                    ddlOndotograma.DataBind();
                    break;
            }
        }

        private void funLlenarPrimerRegistro()
        {
            try
            {
                Array.Resize(ref objparam, 7);
                objparam[0] = 1;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                objparam[4] = 0;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (dt.Tables[0].Rows.Count > 0)
                {
                    DataTable tblagre = new DataTable();
                    tblagre = (DataTable)ViewState["tbPresupuesto"];
                    DataRow filagre = tblagre.NewRow();
                    filagre["Codigo"] = dt.Tables[0].Rows[0][0].ToString();
                    filagre["CodigoCabecera"] = dt.Tables[0].Rows[0][1].ToString();
                    filagre["CodigoProcedimiento"] = dt.Tables[0].Rows[0][2].ToString();
                    filagre["Procedimiento"] = dt.Tables[0].Rows[0][3].ToString();
                    filagre["Pieza"] = dt.Tables[0].Rows[0][4].ToString();
                    filagre["Pvp"] = dt.Tables[0].Rows[0][5].ToString();
                    filagre["Costo"] = dt.Tables[0].Rows[0][6].ToString();
                    filagre["Cobertura"] = dt.Tables[0].Rows[0][7].ToString();
                    filagre["Total"] = dt.Tables[0].Rows[0][8].ToString();
                    filagre["Prioridad"] = dt.Tables[0].Rows[0][9].ToString();
                    filagre["Autorizado"] = dt.Tables[0].Rows[0][10].ToString();
                    filagre["Realizado"] = dt.Tables[0].Rows[0][11].ToString();
                    filagre["Inicial"] = dt.Tables[0].Rows[0][12].ToString();
                    filagre["Realizar"] = "SI";
                    filagre["CodigoPrestadora"] = ViewState["CodigoPrestadora"].ToString();
                    tblagre.Rows.Add(filagre);
                    ViewState["tbPresupuesto"] = tblagre;
                    grdvPresupuesto.DataSource = tblagre;
                    grdvPresupuesto.DataBind();
                    imgPrioridad = (ImageButton)grdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    imgPrioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString(); 
            }
        }
        protected void funHistorialProcedimientos(int codCabecera, int codProcedimiento)
        {
            try
            {
                Array.Resize(ref objparam, 7);
                objparam[0] = 8;
                objparam[1] = codCabecera;
                objparam[2] = 0;
                objparam[3] = 0;
                objparam[4] = codProcedimiento;
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
        protected void ddlProcedimiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProcedimiento.SelectedValue != "0" && ddlOndotograma.SelectedValue != "0") funCargarCombos(2);
        }

        protected void grdvPresupuesto_RowDataBound(object sender, GridViewRowEventArgs e)
        {            
            try
            {
                if (e.Row.RowIndex >= 0)
                {
                    inicial = grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Inicial"].ToString();
                    Realizado = grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Realizado"].ToString();
                    Codigo = int.Parse(grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["Codigo"].ToString());
                    CodigoCabecera = int.Parse(grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoCabecera"].ToString());
                    CodigoProcedimiento = int.Parse(grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoProcedimiento"].ToString());
                    codigoPrestadora = int.Parse(grdvPresupuesto.DataKeys[e.Row.RowIndex].Values["CodigoPrestadora"].ToString());
                    imgEliminar = (ImageButton)(e.Row.Cells[0].FindControl("imgEliminar"));
                    imgPrioridad = (ImageButton)(e.Row.Cells[7].FindControl("imgPrioridad"));
                    chkRealizar = (CheckBox)(e.Row.Cells[8].FindControl("chkRealizar"));
                    imgDetalle = (ImageButton)(e.Row.Cells[9].FindControl("imgDetalle"));                    
                    if (inicial == "DF")
                    {
                        imgEliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        imgEliminar.Height = 20;
                        imgEliminar.Enabled = false;
                        imgPrioridad.Enabled = false;
                        imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                        imgPrioridad.Height = 20;
                        chkRealizar.Checked = true;
                        chkRealizar.Enabled = false;
                        if (Realizado == "SI")
                        {
                            imgDetalle.ImageUrl = "~/Botones/Buscar.png";
                            imgDetalle.Enabled = true;
                            imgDetalle.Height = 20;
                        }
                    }
                    else if (Realizado == "SI")
                    {
                        imgEliminar.ImageUrl = "~/Botones/eliminaroff.jpg";
                        imgEliminar.Height = 20;
                        imgEliminar.Enabled = false;
                        imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                        imgPrioridad.Enabled = false;
                        imgPrioridad.Height = 20;
                        imgDetalle.ImageUrl = "~/Botones/Buscar.png";
                        imgDetalle.Enabled = true;
                        imgDetalle.Height = 20;
                        chkRealizar.Checked = true;
                        chkRealizar.Enabled = false;
                    }
                    if (codigoPrestadora != int.Parse(ViewState["CodigoPrestadora"].ToString()))
                    {
                        e.Row.Cells[4].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[5].ForeColor = System.Drawing.Color.White;
                        e.Row.Cells[6].ForeColor = System.Drawing.Color.White;
                    }
                    //ACTIVAR EL BOTON DETALLE SI EXISTE REGISTRO DEL PROCEDIMIENTO
                }
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    var valor = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Total")).Replace(".",",");
                //    _Total += Convert.ToDecimal(valor);
                //}
                //else if (e.Row.RowType == DataControlRowType.Footer)
                //{
                //    e.Row.Cells[4].Text = "TOTAL:";
                //    e.Row.Cells[6].Text = _Total.ToString();
                //    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                //    e.Row.Font.Bold = true;
                //}
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }

        protected void imgAgregar_Click(object sender, ImageClickEventArgs e)
        {
            //PREGUNTAR CUANTOS EVENTOS TIENE REALIZADO EL PROCEDIMIENTO
            try
            {
                bool lexiste = false;
                lblerror.Text = "";
                grdvDatos.DataSource = null;
                grdvDatos.DataBind();
                txtEditor.Content = "";
                if (ddlOndotograma.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Odontograma..!", this);
                    return;
                }
                if (ddlProcedimiento.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Procedimiento..!", this);
                    return;
                }
                if (ddlPieza.SelectedValue == "-1")
                {
                    new Funciones().funShowJSMessage("Seleccione Pieza Dental..!", this);
                    return;
                }

                Array.Resize(ref objparam, 7);
                objparam[0] = 2;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoTitu"].ToString());
                objparam[3] = int.Parse(ViewState["CodigoBene"].ToString());
                objparam[4] = int.Parse(ddlProcedimiento.SelectedValue);
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                if (dt.Tables[0].Rows.Count == 0)
                {
                    new Funciones().funShowJSMessage("No existe definido Procedimiento en la Prestadora..!", this);
                    return;
                }
                if (ViewState["tbPresupuesto"] != null)
                {                    
                    DataTable tblbuscar = (DataTable)ViewState["tbPresupuesto"];
                    if (tblbuscar.Rows.Count > 0)
                    {
                        maxCodigo = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Codigo"]));
                        prioridad = tblbuscar.AsEnumerable()
                            .Max(row => int.Parse((string)row["Prioridad"]));
                    }
                    else
                    {
                        maxCodigo = 0;
                        prioridad = 0;
                    }
                    DataRow result = tblbuscar.Select("CodigoProcedimiento=" + ddlProcedimiento.SelectedValue + " and Pieza='" + ddlPieza.SelectedValue + "'").FirstOrDefault();
                    if (result != null) lexiste = true;
                }

                if (lexiste)
                {
                    new Funciones().funShowJSMessage("Ya existe Ingresado Procedimiento para la misma Pieza Dental..!", this);
                    return;
                }

                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tbPresupuesto"];
                DataRow filagre = tblagre.NewRow();
                filagre["Codigo"] = maxCodigo + 1;
                filagre["CodigoCabecera"] = 0;
                filagre["CodigoProcedimiento"] = ddlProcedimiento.SelectedValue;
                filagre["Procedimiento"] = ddlProcedimiento.SelectedItem.ToString();
                filagre["Pieza"] = ddlPieza.SelectedItem.ToString();
                filagre["Pvp"] = dt.Tables[0].Rows[0][0].ToString();
                filagre["Costo"] = dt.Tables[0].Rows[0][1].ToString();
                filagre["Cobertura"] = dt.Tables[0].Rows[0][2].ToString();
                filagre["Total"] = dt.Tables[0].Rows[0][3].ToString();
                filagre["Prioridad"] = prioridad + 1; 
                filagre["Autorizado"] = dt.Tables[0].Rows[0][5].ToString();
                filagre["Realizado"] = dt.Tables[0].Rows[0][6].ToString();
                filagre["Inicial"] = dt.Tables[0].Rows[0][7].ToString();
                filagre["Realizar"] = "NO";
                filagre["CodigoPrestadora"] = ViewState["CodigoPrestadora"].ToString();
                tblagre.Rows.Add(filagre);
                tblagre.DefaultView.Sort = "Realizado";
                //tblagre.DefaultView.Sort = "Prioridad";
                //dtOrdenado = tblagre.DefaultView.ToTable();
                ViewState["tbPresupuesto"] = tblagre;
                grdvPresupuesto.DataSource = tblagre;
                grdvPresupuesto.DataBind();
                if (grdvPresupuesto.Rows.Count > 0)
                {
                    imgPrioridad = (ImageButton)grdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    imgPrioridad.Enabled = false;
                }
            }                
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
            ddlProcedimiento.SelectedValue = "0";
            ddlPieza.SelectedValue = "-1";
        }

        protected void imgEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                int codigo = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                int codigoCabecera = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString());
                int codigoProcedimiento = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoProcedimiento"].ToString());
                grdvDatos.DataSource = null;
                grdvDatos.DataBind();
                txtEditor.Content = "";
                Array.Resize(ref objparam, 7);
                objparam[0] = 10;
                objparam[1] = codigoCabecera;
                objparam[2] = codigo;
                objparam[3] = 0;
                objparam[4] = codigoProcedimiento;
                objparam[5] = int.Parse(Session["usuCodigo"].ToString());
                objparam[6] = Session["MachineName"].ToString();
                new Conexion(2, "").funConsultarSqls("sp_ProcedimientosProcesos", objparam);
                DataTable tblagre = new DataTable();
                tblagre = (DataTable)ViewState["tbPresupuesto"];
                DataRow[] rows;
                rows = tblagre.Select("Codigo='" + codigo.ToString() + "'");
                foreach (DataRow row in rows)
                {
                    tblagre.Rows.Remove(row);
                }
                tblagre.DefaultView.Sort = "Realizado";
                prioridad = 0;
                foreach (DataRow row in tblagre.Rows)
                {
                    row["Prioridad"] = prioridad;
                    prioridad++;
                    tblagre.AcceptChanges();
                }                
                ViewState["tbPresupuesto"] = tblagre;
                grdvPresupuesto.DataSource = tblagre;
                grdvPresupuesto.DataBind();
                if (grdvPresupuesto.Rows.Count > 0)
                {
                    imgPrioridad = (ImageButton)grdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    imgPrioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        protected void chkRealizar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblerror.Text = "";
                txtEditor.Content = "";
                grdvDatos.DataSource = null;
                grdvDatos.DataBind();
                CheckBox chkest = new CheckBox();
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                //int intIndex = gvRow.RowIndex;
                chkest = (CheckBox)(gvRow.Cells[7].FindControl("chkRealizar"));
                //ViewState["RegistraHistorial"] = "NO";
                ViewState["Checked"] = chkest.Checked;
                tbPresupuesto = (DataTable)ViewState["tbPresupuesto"];
                Codigo = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                cobertura = int.Parse(grdvPresupuesto.Rows[gvRow.RowIndex].Cells[5].Text);
                DataRow[] result = tbPresupuesto.Select("Codigo='" + Codigo.ToString() + "'");
                if (chkest.Checked == true)
                {
                    if (cobertura > 0)
                    {
                        contar = tbPresupuesto.Select("Cobertura>0 and Realizar='SI' and Realizado='NO' and Inicial='0'").Length;
                        if (contar > 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", "alert('Solo se puede realizar un procedimiento con cobertura!..');", true);
                            new Funciones().funShowJSMessage("Solo se puede realizar un procedimiento con cobertura!..", this);
                            chkest.Checked = false;
                            ViewState["Checked"] = chkest.Checked;
                        }
                    }
                }                
                tbPresupuesto.AcceptChanges();
                chkRealizar = (CheckBox)(gvRow.Cells[7].FindControl("chkRealizar"));
                chkRealizar.Checked = (bool)ViewState["Checked"];
                if (chkRealizar.Checked)
                {
                    grdvPresupuesto.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.Bisque;
                    result[0]["Realizar"] = "SI";
                    tbPresupuesto.AcceptChanges();
                }
                else
                {
                    grdvPresupuesto.Rows[gvRow.RowIndex].Cells[1].BackColor = System.Drawing.Color.White;
                    result[0]["Realizar"] = "NO";
                    tbPresupuesto.AcceptChanges();
                }
                ViewState["tbPresupuesto"] = tbPresupuesto;
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                btnGrabar.Enabled = false;
                tbPresupuesto = (DataTable)ViewState["tbPresupuesto"];
                string[] columnas = new[] { "Codigo", "CodigoCabecera","CodigoProcedimiento", "Procedimiento", "Pieza", "Pvp", "Costo",
                "Cobertura","Total","Prioridad","Autorizado","Realizado","Inicial","Realizar"};
                DataView view = new DataView(tbPresupuesto);
                tbPresupuesto = view.ToTable(true, columnas);
                contar = tbPresupuesto.Select("Realizar='SI' and Realizado='NO'").Length;
                //System.Threading.Thread.Sleep(300);
                Array.Resize(ref objparam, 3);
                if (contar > 0)
                {
                    ViewState["RegistraHistorial"] = "SI";
                    objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                    objparam[1] = "F";
                    objparam[2] = 75;
                    dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                }
                else ViewState["RegistraHistorial"] = "NO";
                Array.Resize(ref objparam, 15);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString()); 
                objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString()); 
                objparam[3] = ViewState["RegistraHistorial"].ToString();
                objparam[4] = "T";
                objparam[5] = "";
                objparam[6] = "";
                objparam[7] = "";
                objparam[8] = "";
                objparam[9] = 0;
                objparam[10] = 0;
                objparam[11] = 0;
                objparam[12] = 0;
                objparam[13] = int.Parse(Session["usuCodigo"].ToString());
                objparam[14] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertPresupuestoCab(objparam);
                CodigoCabecera = int.Parse(dt.Tables[0].Rows[0]["CodigoPSTO"].ToString());
                Codigo = int.Parse(dt.Tables[0].Rows[0]["CodigoHICO"].ToString());
                if (CodigoCabecera > 0)
                {
                    Array.Resize(ref objparam, 29);
                    objparam[0] = 0;
                    objparam[1] = CodigoCabecera;
                    objparam[2] = int.Parse(ViewState["CodigoPrestadora"].ToString());
                    objparam[3] = int.Parse(ViewState["CodigoCita"].ToString());
                    objparam[4] = Codigo;
                    objparam[5] = txtEditor.Content;
                    objparam[19] = "";
                    objparam[20] = "";
                    objparam[21] = "";
                    objparam[22] = "";
                    objparam[23] = 0;
                    objparam[24] = 0;
                    objparam[25] = 0;
                    objparam[26] = 0;
                    objparam[27] = int.Parse(Session["usuCodigo"].ToString());
                    objparam[28] = Session["MachineName"].ToString();
                    foreach (DataRow drpres in tbPresupuesto.Rows)
                    {
                        objparam[6] = int.Parse(drpres["Codigo"].ToString());
                        objparam[7] = int.Parse(drpres["CodigoProcedimiento"].ToString());
                        objparam[8] = drpres["Procedimiento"].ToString();
                        objparam[9] = drpres["Pieza"].ToString();
                        objparam[10] = drpres["Pvp"].ToString();
                        objparam[11] = drpres["Costo"].ToString();
                        objparam[12] = int.Parse(drpres["Cobertura"].ToString());
                        objparam[13] = drpres["Total"].ToString();
                        objparam[14] = int.Parse(drpres["Prioridad"].ToString());
                        objparam[15] = drpres["Autorizado"].ToString();
                        objparam[16] = drpres["Realizado"].ToString();
                        objparam[17] = drpres["Inicial"].ToString();
                        objparam[18] = drpres["Realizar"].ToString();
                        mensaje = new Conexion(2, "").FunInsertPresupuestoDet(objparam);
                    }
                    Response.Redirect("FrmPrespuestoRealizadoAdmin.aspx?MensajeRetornado=" + mensaje + "&CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                }
                else lblerror.Text = "Problemas de Inserción en Base de Datos..!";
                //Array.Resize(ref objparam, 8);
                //objparam[0] = 0;
                //objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                //objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString());
                //objparam[3] = ViewState["RegistraHistorial"].ToString();
                //objparam[4] = "T";
                //objparam[5] = txtEditor.Content;
                //objparam[6] = int.Parse(Session["usuCodigo"].ToString());
                //objparam[7] = Session["MachineName"].ToString();
                //dt = new Conexion(2, "").funInsertarPresupuesto(objparam, tbPresupuesto);
                //if (dt.Tables[0].Rows.Count > 0) Response.Redirect("FrmPrespuestoRealizadoAdmin.aspx?MensajeRetornado='Guardado con Éxito'" + "&CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                //else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmOdontoCitasAdmin.aspx?&CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
        }
        protected void ddlOndotograma_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPieza.Items.Clear();
            ListItem item = new ListItem();
            item.Text = "--Seleccione Pieza--";
            item.Value = "0";
            ddlPieza.Items.Add(item);
            ddlProcedimiento.SelectedValue = "0";
            if (ddlOndotograma.SelectedValue != "0")
            {
                switch (ddlOndotograma.SelectedValue)
                {
                    case "A":
                        imgOdontoGrama.ImageUrl = "~/Images/Odonto_Adultos.jpg";
                        break;
                    case "P":
                        imgOdontoGrama.ImageUrl = "~/Images/Odonto_Pediatrico.jpg";
                        break;
                }
            }
        }
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                btnFinalizar.Enabled = false;
                bool continuar;
                tbPresupuesto = (DataTable)ViewState["tbPresupuesto"];
                DataRow result = tbPresupuesto.Select("Cobertura>0 and Realizar='SI'").FirstOrDefault();
                if (result == null) continuar = false;
                else contar = tbPresupuesto.Select("Cobertura>0 and Realizar='SI'").Length;
                if (contar < 2) continuar = false;
                else continuar = true;
                if (continuar == false)
                {
                    new Funciones().funShowJSMessage("Para Finalizar al menos debe realizar un Procedimiento con Cobertura", this);
                    return; 
                }
                string[] columnas = new[] { "Codigo", "CodigoCabecera","CodigoProcedimiento", "Procedimiento", "Pieza", "Pvp", "Costo",
                "Cobertura","Total","Prioridad","Autorizado","Realizado","Inicial","Realizar"};
                DataView view = new DataView(tbPresupuesto);
                tbPresupuesto = view.ToTable(true, columnas);
                System.Threading.Thread.Sleep(3000);
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[1] = "F";
                objparam[2] = 75;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                Array.Resize(ref objparam, 8);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoCita"].ToString());
                objparam[2] = int.Parse(ViewState["CodigoMedico"].ToString());
                objparam[3] = ViewState["RegistraHistorial"].ToString();
                objparam[4] = "F";
                objparam[5] = txtEditor.Content;
                objparam[6] = int.Parse(Session["usuCodigo"].ToString());
                objparam[7] = Session["MachineName"].ToString();
                dt = new Conexion(2, "").FunInsertarPresupuesto(objparam, tbPresupuesto);
                if (dt.Tables[0].Rows.Count > 0) Response.Redirect("FrmPrespuestoRealizadoAdmin.aspx?MensajeRetornado='Guardado con Éxito'" + "&CodigoMedico=" + ViewState["CodigoMedico"].ToString(), true);
                else lblerror.Text = dt.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        protected void imgDetalle_Click(object sender, ImageClickEventArgs e)
        {
            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            txtEditor.Content = "";
            foreach (GridViewRow fr in grdvPresupuesto.Rows)
            {
                fr.Cells[2].BackColor = System.Drawing.Color.White;
            }
            grdvPresupuesto.Rows[gvRow.RowIndex].Cells[2].BackColor = System.Drawing.Color.Teal;
            CodigoCabecera = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoCabecera"].ToString());
            Codigo = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
            CodigoProcedimiento = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["CodigoProcedimiento"].ToString());
            funHistorialProcedimientos(Codigo, CodigoProcedimiento);
        }
        protected void imgProductos_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.updPresupuesto, GetType(), "Productos", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmProductoProcedimientos.aspx?CodigoCita=" + ViewState["CodigoCita"].ToString() + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=900px, height=450px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
        }
        protected void imgPrioridad_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
                grdvDatos.DataSource = null;
                grdvDatos.DataBind();
                txtEditor.Content = "";
                tbPresupuesto = (DataTable)ViewState["tbPresupuesto"];
                Codigo = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Codigo"].ToString());
                prioridad = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex].Values["Prioridad"].ToString());
                if (prioridad - 1 > 0)
                {
                    DataRow[] result = tbPresupuesto.Select("Codigo=" + Codigo);
                    result[0]["Prioridad"] = prioridad - 1;
                    //tbPresupuesto.AcceptChanges();

                    codigoanterior = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex - 1].Values["Codigo"].ToString());
                    prioridadanterior = int.Parse(grdvPresupuesto.DataKeys[gvRow.RowIndex - 1].Values["Prioridad"].ToString());
                    DataRow[] nuevoResult = tbPresupuesto.Select("Codigo=" + codigoanterior);
                    nuevoResult[0]["Prioridad"] = prioridadanterior + 1;
                    tbPresupuesto.AcceptChanges();
                }
                tbPresupuesto.DefaultView.Sort = "Prioridad";
                ViewState["tbPresupuesto"] = tbPresupuesto;
                grdvPresupuesto.DataSource = tbPresupuesto;
                grdvPresupuesto.DataBind();
                if (grdvPresupuesto.Rows.Count > 0)
                {
                    imgPrioridad = (ImageButton)grdvPresupuesto.Rows[0].Cells[7].FindControl("imgPrioridad");
                    imgPrioridad.ImageUrl = "~/Botones/desactivada_up.png";
                    imgPrioridad.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.Message;
            }
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
                objparam[2] = 120;
                dt = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                txtEditor.Enabled = false;
                txtEditor.Content = dt.Tables[0].Rows[0]["Detalle"].ToString();
                //ScriptManager.RegisterStartupScript(this.updHistorial, GetType(), "Visualizar", "javascript: var posicion_x; var posicion_y; posicion_x=(screen.width/2)-(900/2); posicion_y=(screen.height/2)-(600/2); window.open('FrmDetalleCitaOdonto.aspx?Codigo=" + codigo + "',null,'left=' + posicion_x + ', top=' + posicion_y + ', width=1024px, height=600px, status=no,resizable= yes, scrollbars=yes, toolbar=no, location=no, menubar=no,titlebar=0');", true);
            }
            catch (Exception ex)
            {
                lblerror.Text = ex.ToString();
            }
        }
        #endregion
    }
}