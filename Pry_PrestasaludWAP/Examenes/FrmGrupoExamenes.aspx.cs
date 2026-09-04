using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmGrupoExamenes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPrestadores();
                NuevoGrupo();
            }

        }


        private string Conexion
        {
            get
            {
                string cadenaEncriptada = ConfigurationManager.ConnectionStrings["ConnecSQL"].ConnectionString;
                return new Funciones().funDesencripta(cadenaEncriptada);
            }
        }

        private int GrupoId
        {
            get
            {
                if (ViewState["GrupoId"] == null)
                {
                    return 0;
                }

                return Convert.ToInt32(ViewState["GrupoId"]);
            }

            set
            {
                ViewState["GrupoId"] = value;
            }
        }

        private DataTable CrearTablaDetalle()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EXPR_CODIGO",typeof(int));
            dt.Columns.Add("EXAMEN",typeof(string));
            dt.Columns.Add("PVP",typeof(decimal));
            dt.Columns.Add("VALOR_RED",typeof(decimal));
            return dt;
        }

        private DataTable DetalleGrupo
        {
            get
            {
                if (ViewState["DetalleGrupo"] == null)
                {
                    ViewState["DetalleGrupo"] = CrearTablaDetalle();
                }

                return (DataTable)ViewState["DetalleGrupo"];
            }

            set
            {
                ViewState["DetalleGrupo"] = value;
            }
        }
        //cargar combo prestadores
        private void CargarPrestadores()
        {
            using (SqlConnection cn = new SqlConnection(Conexion))
            {
                using (SqlCommand cmd =new SqlCommand("sp_GE_Prestadores_Listar",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlPrestador.DataSource = dt;
                    ddlPrestador.DataTextField = "pres_nombre";
                    ddlPrestador.DataValueField = "PRES_CODIGO";
                    ddlPrestador.DataBind();

                    ddlPrestador.Items.Insert(0,new ListItem("-- Seleccione --","0"));
                }
            }
        }

        protected void ddlPrestador_SelectedIndexChanged(object sender, EventArgs e)
        {
            GrupoId = 0;
          
            txtBuscarExamen.Text = "";
            ddlFiltroAsignacion.SelectedValue = "TODOS";
            gvDisponibles.PageIndex = 0;

            txtNombreGrupo.Text ="";
            txtDescripcion.Text ="";

            DetalleGrupo = CrearTablaDetalle();

            BindDetalle();
            CargarGrupos();
            CargarExamenesDisponibles();
        }
        private void CargarGrupos()
        {
            ddlGrupo.Items.Clear();

            int prestadorId = Convert.ToInt32(ddlPrestador.SelectedValue);

            if (prestadorId == 0)
            {
                ddlGrupo.Items.Add(new ListItem("-- Nuevo grupo --","0"));
                return;
            }

            using (SqlConnection cn = new SqlConnection(Conexion))
            {
                using (SqlCommand cmd =new SqlCommand("sp_GE_Grupos_Listar",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PRES_CODIGO",SqlDbType.Int).Value = prestadorId;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlGrupo.DataSource = dt;
                    ddlGrupo.DataTextField = "GRUP_NOMBRE";
                    ddlGrupo.DataValueField = "GRUP_CODIGO";
                    ddlGrupo.DataBind();
                    ddlGrupo.Items.Insert(0,new ListItem("-- Nuevo grupo --","0"));
                }
            }
        }

        private void CargarExamenesDisponibles()
        {
            if (ddlPrestador.SelectedValue == "0")
            {
                gvDisponibles.DataSource =null;
                gvDisponibles.DataBind();
                return;
            }

            int prestadorId =Convert.ToInt32(ddlPrestador.SelectedValue);

            using (SqlConnection cn = new SqlConnection(Conexion))
            {
                using (SqlCommand cmd =new SqlCommand("sp_GE_ExamenesPrestador_Listar",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PRES_CODIGO",SqlDbType.Int).Value = prestadorId;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    /* ====================================
                       SACAR LOS QUE YA ESTÁN AGREGADOS
                       ==================================== */

                    for (int i = dt.Rows.Count - 1;i >= 0;i--)
                    {
                        int examenId = Convert.ToInt32(dt.Rows[i]["EXPR_CODIGO"]);

                        DataRow[] encontrado = DetalleGrupo.Select("EXPR_CODIGO = " + examenId.ToString());

                        if (encontrado.Length > 0)
                        {
                            dt.Rows.RemoveAt(i);
                        }
                    }

                    string buscar = txtBuscarExamen.Text.Trim();
                    string filtroAsignacion = ddlFiltroAsignacion.SelectedValue;
                    DataTable dtFiltrado = dt.Clone();
                    foreach (DataRow fila in dt.Rows)
                    {
                        string examen =
                            fila["EXAMEN"] == DBNull.Value
                            ? ""
                            : fila["EXAMEN"].ToString();


                        int asignado = fila["ASIGNADO"] == DBNull.Value ? 0 : Convert.ToInt32(fila["ASIGNADO"]);

                        bool coincideBusqueda = true;


                        if (!string.IsNullOrWhiteSpace(buscar))
                        {
                            coincideBusqueda = examen.IndexOf(buscar, StringComparison.OrdinalIgnoreCase) >= 0;
                        }

                        bool coincideEstado = true;


                        if (filtroAsignacion == "1")
                        {
                            coincideEstado = asignado == 1;
                        }
                        else if (filtroAsignacion == "0")
                        {
                            coincideEstado = asignado == 0;
                        }

                        if (coincideBusqueda && coincideEstado)
                        {
                            dtFiltrado.ImportRow(fila);
                        }
                    }

                    int totalRegistros = dtFiltrado.Rows.Count;
                    int totalPaginas = 0;

                    if (gvDisponibles.PageSize > 0)
                    {
                        totalPaginas =
                            (int)Math.Ceiling(
                                (double)totalRegistros
                                /
                                gvDisponibles.PageSize
                            );
                    }


                    if (totalPaginas == 0)
                    {
                        gvDisponibles.PageIndex = 0;
                    }
                    else if (gvDisponibles.PageIndex >= totalPaginas)
                    {
                        gvDisponibles.PageIndex = totalPaginas - 1;
                    }

                    gvDisponibles.DataSource = dtFiltrado;
                    gvDisponibles.DataBind();

                }
            }
        }

        private void NuevoGrupo()
        {
            // Indicamos que no estamos editando
            // ningún grupo existente.
            GrupoId = 0;

            // Limpiamos los datos del grupo.
            txtNombreGrupo.Text = "";
            txtDescripcion.Text = "";

            // Creamos un detalle completamente nuevo.
            DetalleGrupo = CrearTablaDetalle();

            // Refrescamos la tabla derecha.
            BindDetalle();

            // Si ya cargamos el combo de grupos,
            // seleccionamos "Nuevo grupo".
            if (ddlGrupo.Items.Count > 0)
            {
                ListItem itemNuevo = ddlGrupo.Items.FindByValue("0");

                if (itemNuevo != null)
                {
                    ddlGrupo.SelectedValue = "0";
                }
            }

            // Si hay prestador seleccionado,
            // volvemos a cargar sus exámenes.
            if (ddlPrestador.SelectedValue != "0")
            {
                CargarExamenesDisponibles();
            }
            else
            {
                gvDisponibles.DataSource = null;
                gvDisponibles.DataBind();
            }
        }

        private void BindDetalle()
        {
            gvDetalle.DataSource = DetalleGrupo;
            gvDetalle.DataBind();
            CalcularTotalPvp();
        }

        private void CalcularTotalPvp()
        {
            decimal totalPvp = 0;

            foreach (DataRow fila in DetalleGrupo.Rows)
            {
                if (fila["PVP"] != DBNull.Value)
                {
                    totalPvp += Convert.ToDecimal(fila["PVP"]);
                }
            }

            lblTotalPvp.Text = "$ " + totalPvp.ToString("N2");
        }

        protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int grupoId = Convert.ToInt32(ddlGrupo.SelectedValue);

                if (grupoId == 0)
                {
                    NuevoGrupo();
                    return;
                }

                CargarGrupo(grupoId);
        }

        protected void gvDisponibles_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName != "Agregar")
            {
                return;
            }

            // Guardamos primero cualquier PVP o Red
            // que el usuario haya modificado en el grupo.
            CapturarDetalleDesdeGrid();

            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            int index = row.RowIndex;
            int examenId =Convert.ToInt32(gvDisponibles.DataKeys[index].Values["EXPR_CODIGO"]);
            decimal pvp = Convert.ToDecimal(gvDisponibles.DataKeys[index].Values["PVP"]);
            decimal valorRed = Convert.ToDecimal(gvDisponibles.DataKeys[index].Values["VALOR_RED"]);
            string examen = HttpUtility.HtmlDecode(row.Cells[0].Text);
            DataTable dt = DetalleGrupo;

            // Validamos que no esté repetido.
            DataRow[] existe = dt.Select("EXPR_CODIGO = " + examenId.ToString());

            if (existe.Length > 0)
            {
                //MostrarMensaje("El examen ya se encuentra agregado.",false);
                return;
            }

            DataRow nuevaFila = dt.NewRow();

            nuevaFila["EXPR_CODIGO"] = examenId;
            nuevaFila["EXAMEN"] = examen;
            nuevaFila["PVP"] = pvp;
            nuevaFila["VALOR_RED"] = valorRed;

            dt.Rows.Add(nuevaFila);

            DetalleGrupo = dt;

            // Actualiza la tabla derecha.
            BindDetalle();

            // Actualiza la izquierda.
            // El examen agregado desaparecerá de ahí.
            CargarExamenesDisponibles();

            //MostrarMensaje("",true);
        }

        protected void gvDetalle_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName != "Eliminar")
            {
                return;
            }

            // Guardamos primero valores modificados.
            CapturarDetalleDesdeGrid();

            GridViewRow row = (GridViewRow)((Control)e.CommandSource).NamingContainer;

            int examenId = Convert.ToInt32(gvDetalle.DataKeys[row.RowIndex].Value);

            DataTable dt = DetalleGrupo;

            DataRow[] filas = dt.Select("EXPR_CODIGO = " + examenId.ToString());

            if (filas.Length > 0)
            {
                dt.Rows.Remove(filas[0]);
            }

            DetalleGrupo = dt;

            BindDetalle();
            CargarExamenesDisponibles();

        }

        protected void btnNuevoGrupo_Click(object sender, EventArgs e)
        {
            NuevoGrupo();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            CapturarDetalleDesdeGrid();

            if (ddlPrestador.SelectedValue == "0")
            {
                //MostrarMensaje("Debe seleccionar un prestador.",false);
                return;
            }


            if (string.IsNullOrWhiteSpace(txtNombreGrupo.Text))
            {
                //MostrarMensaje("Debe ingresar el nombre del grupo.",false);
                return;
            }


            if (DetalleGrupo.Rows.Count == 0)
            {
                //MostrarMensaje("Debe agregar al menos un examen al grupo.",false);
                return;
            }


            int prestadorId = Convert.ToInt32(ddlPrestador.SelectedValue);

            /* ==============================
               ARMAMOS TABLE VALUED PARAMETER
               ============================== */

            DataTable dtDetalle = new DataTable();

            dtDetalle.Columns.Add("EXPR_CODIGO",typeof(int));
            dtDetalle.Columns.Add("PVP",typeof(decimal));
            dtDetalle.Columns.Add("VALOR_RED",typeof(decimal));

            foreach (DataRow fila in DetalleGrupo.Rows)
            {
                dtDetalle.Rows.Add(Convert.ToInt32(fila["EXPR_CODIGO"]),
                Convert.ToDecimal(fila["PVP"]),
                Convert.ToDecimal(fila["VALOR_RED"]));
            }


            try
            {
                using (SqlConnection cn = new SqlConnection(Conexion))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GE_Grupo_Guardar",cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        /* GRUPO */

                        SqlParameter pGrupo = new SqlParameter("@GRUP_CODIGO",SqlDbType.Int);
                        pGrupo.Direction = ParameterDirection.InputOutput;
                        pGrupo.Value = GrupoId;
                        cmd.Parameters.Add(pGrupo);


                        /* PRESTADOR */

                        cmd.Parameters.Add("@PRES_CODIGO",SqlDbType.Int).Value = prestadorId;
                        cmd.Parameters.Add("@GRUP_NOMBRE",SqlDbType.VarChar,150).Value = txtNombreGrupo.Text.Trim();
                        SqlParameter pDescripcion = cmd.Parameters.Add("@GRUP_DESCRIPCION",SqlDbType.VarChar,300);

                        if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                        {
                            pDescripcion.Value = DBNull.Value;
                        }
                        else
                        {
                            pDescripcion.Value = txtDescripcion.Text.Trim();
                        }


                        /* DETALLE */

                        SqlParameter pDetalle = cmd.Parameters.Add("@DETALLE",SqlDbType.Structured);
                        pDetalle.TypeName = "dbo.TT_GrupoExamenDetalle";
                        pDetalle.Value = dtDetalle;
                        cn.Open();
                        cmd.ExecuteNonQuery();

                        /* RECUPERAMOS ID */

                        GrupoId = Convert.ToInt32(pGrupo.Value);
                    }
                }


                /* ==============================
                   RECARGAMOS
                   ============================== */

                CargarGrupos();
                ListItem item = ddlGrupo.Items.FindByValue(GrupoId.ToString());

                if (item != null)
                {
                    ddlGrupo.SelectedValue = GrupoId.ToString();
                }

                CargarGrupo(GrupoId);


                //MostrarMensaje("Grupo guardado correctamente.",true);
            }
            catch (SqlException ex)
            {
                //MostrarMensaje(ex.Message,false);
            }
            catch (Exception ex)
            {
                //MostrarMensaje(ex.Message,false);
            }
        }

        private void CapturarDetalleDesdeGrid()
        {
            DataTable dt = DetalleGrupo;

            foreach (GridViewRow row in gvDetalle.Rows)
            {
                int examenId = Convert.ToInt32(gvDetalle.DataKeys[row.RowIndex].Value);

                TextBox txtPvp = (TextBox)row.FindControl("txtPvp");
                TextBox txtRed = (TextBox)row.FindControl("txtRed");
                decimal pvp = ConvertirDecimal(txtPvp.Text);
                decimal red = ConvertirDecimal(txtRed.Text);

                DataRow[] filas = dt.Select("EXPR_CODIGO = " + examenId.ToString());

                if (filas.Length > 0)
                {
                    filas[0]["PVP"] = pvp;

                    filas[0]["VALOR_RED"] = red;
                }
            }

            DetalleGrupo = dt;
        }

        private decimal ConvertirDecimal(string valor)
        {
            decimal resultado;

            if (string.IsNullOrWhiteSpace(valor))
            {
                return 0;
            }

            // Intenta según configuración del servidor.
            if (decimal.TryParse(valor,NumberStyles.Any,CultureInfo.CurrentCulture,out resultado))
            {
                return resultado;
            }

            // Permite también 23.50 aunque
            // el servidor use coma decimal.
            string valorNormalizado = valor.Replace(",",".");

            if (decimal.TryParse(valorNormalizado,NumberStyles.Any,CultureInfo.InvariantCulture,out resultado))
            {
                return resultado;
            }

            return 0;
        }

        private void CargarGrupo(int grupoId)
        {
            using (SqlConnection cn = new SqlConnection(Conexion))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GE_Grupo_Obtener",cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@GRUP_CODIGO",SqlDbType.Int).Value = grupoId;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    if (ds.Tables.Count < 2)
                    {
                        //MostrarMensaje("No se pudo obtener el grupo.",false);
                        return;
                    }

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        //MostrarMensaje("El grupo seleccionado no existe.",false);
                        return;
                    }

                    DataRow cabecera = ds.Tables[0].Rows[0];
                    GrupoId = grupoId;
                    txtNombreGrupo.Text = cabecera["GRUP_NOMBRE"].ToString();
                    txtDescripcion.Text = cabecera["GRUP_DESCRIPCION"].ToString();

                    // El segundo resultado del SP
                    // contiene EXPR_CODIGO,
                    // EXAMEN, PVP, VALOR_RED.
                    DetalleGrupo = ds.Tables[1].Copy();
                    BindDetalle();
                    CargarExamenesDisponibles();
                }
            }
        }

      
        protected void btnBuscarExamen_Click(object sender,EventArgs e)
        {
            /*
             * Cada búsqueda nueva empieza
             * desde la página 1.
             */

            gvDisponibles.PageIndex = 0;
            CargarExamenesDisponibles();
        }
        protected void btnLimpiarBusqueda_Click(object sender,EventArgs e)
        {
            txtBuscarExamen.Text = "";
            ddlFiltroAsignacion.SelectedValue = "TODOS";
            gvDisponibles.PageIndex = 0;
            CargarExamenesDisponibles();
        }
        protected void ddlFiltroAsignacion_SelectedIndexChanged(object sender,EventArgs e)
        {
            gvDisponibles.PageIndex = 0;
            CargarExamenesDisponibles();
        }
        protected void gvDisponibles_PageIndexChanging(object sender,GridViewPageEventArgs e)
        {
            gvDisponibles.PageIndex = e.NewPageIndex;
            CargarExamenesDisponibles();
        }

    }

}