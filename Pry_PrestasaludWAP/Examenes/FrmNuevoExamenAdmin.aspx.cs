using System;
using System.Data;
using System.Web.UI;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmNuevoExamenAdmin : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        Object[] objparam = new Object[1];
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            TxtValorExamen.Attributes.Add("onchange", "ValidarDecimales();");
            if (!IsPostBack)
            {
                ViewState["Codigo"] = Request["Codigo"];
                ViewState["NomExamen"] = "";
                FunCargarCombos(0);
                if (ViewState["Codigo"].ToString() == "0")
                {
                    Lbltitulo.Text = "Agregar Nuevo Examen";
                }
                else
                {
                    Label7.Visible = true;
                    ChkEstado.Visible = true;
                    Lbltitulo.Text = "Editar Examen";
                    FunCargaMantenimiento();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        protected void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = ViewState["Codigo"].ToString();
                objparam[1] = "";
                objparam[2] = 134;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                if (dts.Tables[0].Rows.Count > 0)
                {
                    TxtExamen.Text = dts.Tables[0].Rows[0]["Examen"].ToString();
                    ViewState["NomExamen"] = TxtExamen.Text.Trim().ToUpper();
                    TxtDescripcion.Text = dts.Tables[0].Rows[0]["Descripcion"].ToString();
                    DdlCategoria.SelectedValue = dts.Tables[0].Rows[0]["Categoria"].ToString();
                    DdlSubCategoria.SelectedValue = dts.Tables[0].Rows[0]["SubCategoria"].ToString();
                    ChkEstado.Text = dts.Tables[0].Rows[0]["Estado"].ToString();
                    ChkEstado.Checked = dts.Tables[0].Rows[0]["Estado"].ToString() == "Activo" ? true : false;
                    TxtValorExamen.Text = dts.Tables[0].Rows[0]["Valor"].ToString();
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunCargarCombos(int opcion)
        {
            try
            {
                switch (opcion)
                {
                    case 0:
                        Array.Resize(ref objparam, 3);
                        objparam[0] = 0;
                        objparam[1] = "CATEGORIA EXAMENES";
                        objparam[2] = 131;
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlCategoria.DataSource = dts;
                        DdlCategoria.DataTextField = "Descripcion";
                        DdlCategoria.DataValueField = "Codigo";
                        DdlCategoria.DataBind();

                        objparam[1] = "SUBCATEGORIA EXAMENES";
                        dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                        DdlSubCategoria.DataSource = dts;
                        DdlSubCategoria.DataTextField = "Descripcion";
                        DdlSubCategoria.DataValueField = "Codigo";
                        DdlSubCategoria.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }

        }

        #endregion

        #region Botones y Eventos
        protected void ChkEstado_CheckedChanged(object sender, EventArgs e)
        {
            ChkEstado.Text = ChkEstado.Checked == true ? "Activo" : "Inactivo";
        }

        protected void BntGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtExamen.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Nombre del Examen", this);
                    return;
                }
                if (DdlCategoria.SelectedValue == "0")
                {
                    new Funciones().funShowJSMessage("Seleccione Categoria..!", this);
                    return;
                }

                if (ViewState["NomExamen"].ToString() != TxtExamen.Text.Trim().ToUpper())
                {
                    Array.Resize(ref objparam, 3);
                    objparam[0] = int.Parse(ViewState["Codigo"].ToString());
                    objparam[1] = TxtExamen.Text.Trim().ToUpper();
                    objparam[2] = 133;
                    dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                    if (dts.Tables[0].Rows.Count > 0)
                    {
                        new Funciones().funShowJSMessage("Examen ya Existe ingresado..!", this);
                        return;
                    }
                }

                Array.Resize(ref objparam, 20);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["Codigo"].ToString());
                objparam[2] = TxtExamen.Text.ToUpper();
                objparam[3] = TxtDescripcion.Text.ToUpper();
                objparam[4] = DdlCategoria.SelectedValue;
                objparam[5] = DdlSubCategoria.SelectedValue;
                objparam[6] = TxtValorExamen.Text;
                objparam[7] = ChkEstado.Checked ? "Activo" : "Inactivo";
                objparam[8] = "";
                objparam[9] = "";
                objparam[10] = "";
                objparam[11] = "";
                objparam[12] = "";
                objparam[13] = 0;
                objparam[14] = 0;
                objparam[15] = 0;
                objparam[16] = 0;
                objparam[17] = 0;
                objparam[18] = int.Parse(Session["usuCodigo"].ToString());
                objparam[19] = Session["MachineName"].ToString();
                dts = new Conexion(2, "").funConsultarSqls("sp_NuevoExamenPrestador", objparam);
                if (dts.Tables[0].Rows[0][0].ToString() == "OK")
                    Response.Redirect("FrmExamenSegurosAdmin.aspx?MensajeRetornado='Guardado con Éxito'", true);
                else
                    Lblerror.Text = "Error al Grabar..";
                
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmExamenSegurosAdmin.aspx", true);
        }
        #endregion
    }
}