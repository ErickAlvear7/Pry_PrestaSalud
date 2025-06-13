using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
/// <summary>
/// Summary description for Conexion
/// </summary>
public class Conexion
{
    #region Variables
    public SqlConnection Sqlcn;
    public MySqlConnection MySqlcn;
    DataSet ds = new DataSet();
    SegPrincipal objfun = new SegPrincipal();
    DataSet dsDataMenu = new DataSet();
    DataSet dsData = new DataSet();
    String strSelectCommandText = "", mensaje = "", strResul = "";
    DataSet dt = new DataSet();
    int codigoCita = 0, codigo = 0;
    #endregion

    #region Procedimientos y Funciones
    private void ConectarMySqlBDD(String strConexion)
    {
        try
        {
            MySqlcn = new MySqlConnection(strConexion);
            //MySqlcn.Open();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            //Agregar un metodo para crear logs de auditoria 
        }
    }

    private void ConectarSqlBDD()
    {
        try
        {
            String conn = ConfigurationManager.ConnectionStrings["ConnecSQL"].ToString();
            Sqlcn = new SqlConnection(new Funciones().funDesencripta(conn));
        }
        catch (Exception ex)
        {
            new Funciones().funCrearLogAuditoria(1, "Conexion-ConectarSqlBDD", ex.ToString(), 0);
            //Console.WriteLine(ex.Message);
        }
    }

    public Conexion(int intTipoConex, String strCadenaConexion)
    {
        switch (intTipoConex)
        {
            case 1:
                ConectarMySqlBDD(strCadenaConexion);
                break;
            case 2:
                ConectarSqlBDD();
                break;
        }
    }
    public String FunBorrarTemporal()
    {        
        try
        {
            String SQL = "Delete from gtx_locks";
            using (SqlCommand command = new SqlCommand(SQL, Sqlcn))
            {
                Sqlcn.Open();
                command.ExecuteNonQuery();
            }
            strResul = "OK";
        }
        catch (Exception ex)
        {

            strResul = ex.Message;
        }
        return strResul;
    }

    
    public DataSet SIWebseg(int intUserID)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("spCarMenXUsr", Sqlcn))
            {
                Sqlcn.Open();
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@usuCodigo", intUserID);
                SqlDataAdapter sqlda = new SqlDataAdapter(command);
                sqlda.Fill(dsDataMenu);
            }
            Sqlcn.Close();
            return dsDataMenu;
        }
        catch (Exception ex)
        {
            return dsDataMenu = null;
        }
    }

    public DataSet funConsultarSqls(String strSP, object[] objparam)
    {
        try
        {
            strSelectCommandText = "exec " + strSP.ToString() + " ";
            for (int i = 0; i < objparam.GetLength(0); i++)
            {
                if (objparam[i].GetType().ToString() == "System.String")
                {
                    if (i == 0)
                    {
                        strSelectCommandText = strSelectCommandText + "'" + objparam[i].ToString() + "'";
                    }
                    else
                    {
                        strSelectCommandText = strSelectCommandText + ",'" + objparam[i].ToString() + "'";
                    }
                }
                else if (objparam[i].GetType().ToString() == "System.DateTime")
                {
                    if (i == 0)
                    {
                        strSelectCommandText = strSelectCommandText + "'" + objparam[i].ToString() + "'";
                    }
                    else
                    {
                        strSelectCommandText = strSelectCommandText + ",'" + objparam[i].ToString() + "'";
                    }
                }
                else if (i == 0)
                {
                    strSelectCommandText = strSelectCommandText + objparam[i].ToString();
                }
                else
                {
                    strSelectCommandText = strSelectCommandText + "," + objparam[i].ToString();
                }
            }
            new SqlDataAdapter(strSelectCommandText, Sqlcn).Fill(dsData);            
            Sqlcn.Close();
            return dsData;
        }
            catch(Exception ex)
        {
            return dsData = null;
        }
    }

    public DataSet FunInsertDatatableSQL(object[] objparam, DataTable dt)
    {        
        try
        {            
            using (SqlCommand cmd = new SqlCommand("sp_NuevoHorario"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_horacod", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_nombre", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_descripcion", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_intervalo", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_horadesde", TimeSpan.ParseExact(objparam[4].ToString(), @"hh\:mm", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@in_horahasta", TimeSpan.ParseExact(objparam[5].ToString(), @"hh\:mm", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@in_estado", objparam[6]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[7]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[8]);
                cmd.Parameters.AddWithValue("@EmptyHorario", dt);
                Sqlcn.Open();
                //cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    public DataSet FunConsultarSQLNOVA(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_ReportesExpertDoctorNova"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.CommandTimeout = 500;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_fechadesde", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_fechahasta", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_codigocamp", objparam[3].ToString());
                Sqlcn.Open();
                //cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunReportesDoctorV1(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_ReportesExpertDoctor"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.CommandTimeout = 500; //este es el timeout
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_fechadesde", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_fechahasta", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_codigocamp", objparam[3]);
                cmd.Parameters.AddWithValue("@in_tipocliente", objparam[4].ToString());
                Sqlcn.Open();
                //cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }

        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    public DataSet FunInsertarTurnos(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoTurno"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_turncodigo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_turnnombre", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_turndescripcion", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_turnestado", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[4]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[5]);
                cmd.Parameters.AddWithValue("@EmptyTurno", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        } 
    }

    public DataSet FunInsertarPrestadoraEspeci(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevaEspeciPrestadora"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_prescodigo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[1]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[2]);
                //cmd.Parameters.AddWithValue("@in_tipo", objparam[3]);
                cmd.Parameters.AddWithValue("@EmptyEspeciPres", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarTurnoMedico(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoTurnoMedico"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_medicod", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_prescodigo", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[3]);
                cmd.Parameters.AddWithValue("@EmptyTurnoMed", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarTitulares(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoTitular"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_codigopersona", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigotitular", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[2].ToString()));
                cmd.Parameters.AddWithValue("@in_tipoidentificacion", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_numerodocumento", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_primernombre", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_segundonombre", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_primerapellido", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_segundopellido", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_genero", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_estadocivil", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_fechanacimiento", objparam[11].ToString());
                cmd.Parameters.AddWithValue("@in_ciudcod", int.Parse(objparam[12].ToString()));
                cmd.Parameters.AddWithValue("@in_direccion", objparam[13].ToString());
                cmd.Parameters.AddWithValue("@in_telefonocasa", objparam[14].ToString());
                cmd.Parameters.AddWithValue("@in_telefonooficina", objparam[15].ToString());
                cmd.Parameters.AddWithValue("@in_celular", objparam[16].ToString());
                cmd.Parameters.AddWithValue("@in_email", objparam[17].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[18]);
                cmd.Parameters.AddWithValue("@in_fechainicobertura", objparam[19]);
                cmd.Parameters.AddWithValue("@in_fechafincobertura", objparam[20]);
                cmd.Parameters.AddWithValue("@in_usucodigo", int.Parse(objparam[21].ToString()));
                cmd.Parameters.AddWithValue("@in_terminal", objparam[22].ToString());
                cmd.Parameters.AddWithValue("@EmptyBeneficiario", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    public DataSet FunNewTitular(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NewTitularHU"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_tipodocumento", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_numerodocumento", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_nombres", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_primernom", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_segundonom", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_primerape", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_segundoape", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_email", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_fonocasa", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_fonooficina", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_celular", objparam[11].ToString());
                cmd.Parameters.AddWithValue("@in_fechanacimiento", objparam[12].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[13].ToString());
                cmd.Parameters.AddWithValue("@in_genero", objparam[14].ToString());
                cmd.Parameters.AddWithValue("@in_contrato", int.Parse(objparam[15].ToString()));
                cmd.Parameters.AddWithValue("@in_fechavigenciaini", objparam[16]);
                cmd.Parameters.AddWithValue("@in_fechavigenciafin", objparam[17]);
                cmd.Parameters.AddWithValue("@in_tipoplan", objparam[18].ToString());
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[19].ToString()));
                cmd.Parameters.AddWithValue("@in_codigotitular", int.Parse(objparam[20].ToString()));
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[21].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[22].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[23].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[24].ToString());
                cmd.Parameters.AddWithValue("@in_auxv5", objparam[25].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", int.Parse(objparam[26].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi2", int.Parse(objparam[27].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi3", int.Parse(objparam[28].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi4", int.Parse(objparam[29].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi5", int.Parse(objparam[30].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", int.Parse(objparam[31].ToString()));
                cmd.Parameters.AddWithValue("@in_terminal", objparam[32].ToString());

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            if (!File.Exists(@"F:\log_errores\error_trycatch.txt"))
            {
                StreamWriter _log = File.AppendText(@"F:\log_errores\error_trycatch.txt");
                _log.WriteLine("Error" + "|" + "Fecha_Registro");
                _log.Close();
            }

            StreamWriter _writer = File.AppendText(@"F:\log_errores\error_trycatch.txt");

            _writer.WriteLine(ex.Message + "|" + DateTime.Now);
            _writer.Close();
            return ds = null;
        }
    }

    public DataSet FunInsertarParametros(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoParametro"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_codparametro", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_parametro", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_descripcion", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_estadoparametro", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[4]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[5]);
                cmd.Parameters.AddWithValue("@EmptyParametro", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    public DataSet FunSelectDatos(String strSql, String strTipoConex)
    {
        
        try
        {
            switch (strTipoConex)
            {
                case "MYSQL":
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(strSql, MySqlcn))
                    {
                        adapter.Fill(dt);
                    }
                    MySqlcn.Close();
                    break;
                case "SQL":
                    using (SqlDataAdapter adapter = new SqlDataAdapter(strSql, Sqlcn))
                    {
                        adapter.Fill(dt);
                    }
                    Sqlcn.Close();
                    break;
            }
            return dt;
        }
        catch (Exception ex)
        {
            return dt = null;
        }
    }

    public DataSet FunInsertarClienteProducto(object[] objparam, DataTable dt)
    {
        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoClienteProducto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_codigocamp", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_nombrecamp", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_descripcioncamp", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_logoc", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_logop", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_estadocamp", objparam[5]);
                cmd.Parameters.AddWithValue("@in_codigogrupo", int.Parse(objparam[6].ToString()));
                cmd.Parameters.AddWithValue("@in_asisanual", int.Parse(objparam[7].ToString()));
                cmd.Parameters.AddWithValue("@in_asismes", int.Parse(objparam[8].ToString()));
                cmd.Parameters.AddWithValue("@in_tipofecha", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[11].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[12].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[13].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", int.Parse(objparam[14].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi2", int.Parse(objparam[15].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi3", int.Parse(objparam[16].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi4", int.Parse(objparam[17].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", int.Parse(objparam[18].ToString()));
                cmd.Parameters.AddWithValue("@in_terminal", objparam[19].ToString());
                cmd.Parameters.AddWithValue("@in_tipopres", objparam[20].ToString());
                cmd.Parameters.AddWithValue("@EmptyProducto", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch(Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarMedicoEspePresta(object[] objparam, DataTable dt)
    {
        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoMedico"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_medicod", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_medinombre", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_mediapellido", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_medigenero", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_medidireccion", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_meditelefono1", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_meditelefono2", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_medicelular1", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_medicelular2", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_mediemail1", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_mediemail2", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[11]);
                cmd.Parameters.AddWithValue("@in_enviar1", objparam[12]);
                cmd.Parameters.AddWithValue("@in_enviar2", objparam[13]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[14]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[15].ToString());
                cmd.Parameters.AddWithValue("@in_tipo", objparam[16].ToString());
                cmd.Parameters.AddWithValue("@EmptyMediEspePresta", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch(Exception ex)
        {
            new Funciones().funCrearLogAuditoria(1, "FrmNuevoMedico", ex.ToString(), 1);
            return ds = null;
        }
    }

    public DataSet FunInsertarTurnoOdonto(object[] objparam, DataTable dt)
    {
        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoTurnoOdonto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_medicod", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_prescodigo", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[3]);
                cmd.Parameters.AddWithValue("@EmptyTurnoMed", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarProcePresta(object[] objparam, DataTable dt)
    {
       
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoProcePrestadora"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[1]);
                cmd.Parameters.AddWithValue("@EmptyProcePresta", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch
        {
            return ds = null;
        }
    }

    public string FunAgendaCitaMedica(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgendaMedica"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[1].ToString()));  
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[3]);                
                cmd.Parameters.AddWithValue("@in_observacion", objparam[4]);
                cmd.Parameters.AddWithValue("@in_fuente", objparam[5]);
                cmd.Parameters.AddWithValue("@in_tipopago", objparam[6]);
                cmd.Parameters.AddWithValue("@EmptyCitaMedica", dt);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
            }
        }
        catch (Exception ex)
        {
            mensaje = ex.ToString();
        }
        return mensaje;
    }
    public DataSet FunCodigoCita(object[] objparam, DataTable dt)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgendaMedica"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[3]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[4]);
                cmd.Parameters.AddWithValue("@in_fuente", objparam[5]);
                cmd.Parameters.AddWithValue("@in_tipopago", objparam[6]);
                cmd.Parameters.AddWithValue("@EmptyCitaMedica", dt);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
                //int codCita = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
            return ds;
        }

        catch (Exception ex)
        {
            return ds = null;
        }

    }
    public DataSet FunCodigoCitalINK(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgendaMedicaLink"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_preecod", objparam[1]);
                cmd.Parameters.AddWithValue("@in_medicod", objparam[2]);
                cmd.Parameters.AddWithValue("@in_especod", objparam[3]);
                cmd.Parameters.AddWithValue("@in_tipocliente", objparam[4]);
                cmd.Parameters.AddWithValue("@in_titucod", objparam[5]);
                cmd.Parameters.AddWithValue("@in_benecod", objparam[6]);
                cmd.Parameters.AddWithValue("@in_paren", objparam[7]);
                cmd.Parameters.AddWithValue("@in_fechacita", objparam[8]);
                cmd.Parameters.AddWithValue("@in_diacita", objparam[9]);
                cmd.Parameters.AddWithValue("@in_horacita", objparam[10]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[11]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[12]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[13]);
                cmd.Parameters.AddWithValue("@in_codigoproducto", objparam[14]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
            }
            return ds;
        }

        catch (Exception ex)
        {
            return ds = null;
        }

    }

    public DataSet FunCodigoCitaMedilink(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgendaMedicaMedilink"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_preecod", objparam[1]);
                cmd.Parameters.AddWithValue("@in_especod", objparam[2]);
                cmd.Parameters.AddWithValue("@in_medicod", objparam[3]);
                cmd.Parameters.AddWithValue("@in_tipocliente", objparam[4]);
                cmd.Parameters.AddWithValue("@in_titucod", objparam[5]);
                cmd.Parameters.AddWithValue("@in_benecod", objparam[6]);
                cmd.Parameters.AddWithValue("@in_paren", objparam[7]);
                cmd.Parameters.AddWithValue("@in_fechacita", objparam[8]);
                cmd.Parameters.AddWithValue("@in_diacita", objparam[9]);
                cmd.Parameters.AddWithValue("@in_horacita", objparam[10]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[11]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[12]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[13]);
                cmd.Parameters.AddWithValue("@in_codigoproducto", objparam[14]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
            }
            return ds;
        }

        catch (Exception ex)
        {
            return ds = null;
        }

    }
    public string FunAgendaCitaOdonto(object[] objparam, DataTable dt)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_AgendaOdonto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[1].ToString()));  
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[3]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[4]);
                cmd.Parameters.AddWithValue("@EmptyCitaMedica", dt);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
            }
        }
        catch (Exception ex)
        {
            mensaje = ex.ToString();
        }
        return mensaje;
    }

    public int FunGetCodigoCita(object[] objparam)
    {
        
        try
        {            
            using (SqlCommand cmd = new SqlCommand("sp_CargarAgendarHoras"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_preecod", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_medicod", int.Parse(objparam[2].ToString()));
                cmd.Parameters.AddWithValue("@in_especod", int.Parse(objparam[3].ToString()));
                cmd.Parameters.AddWithValue("@in_fechacita", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_diacita", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_horacita", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_detallecita", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_hodecodigo", int.Parse(objparam[8].ToString()));
                cmd.Parameters.AddWithValue("@in_tipocita", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_titucita", int.Parse(objparam[10].ToString()));
                cmd.Parameters.AddWithValue("@in_benecita", int.Parse(objparam[11].ToString()));
                cmd.Parameters.AddWithValue("@in_parencodigo", objparam[12].ToString());
                cmd.Parameters.AddWithValue("@in_longitud", objparam[13].ToString());
                cmd.Parameters.AddWithValue("@in_latitud", objparam[14].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", int.Parse(objparam[15].ToString()));
                cmd.Parameters.AddWithValue("@in_terminal", objparam[16].ToString());
                cmd.Parameters.AddWithValue("@in_observacion", objparam[16].ToString());
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                codigoCita = int.Parse(ds.Tables[0].Rows[0][0].ToString());
            }
        }
        catch 
        {
            codigoCita = 0;
        }
        return codigoCita; 
    }

    public DataSet FunGetDatosTituBene(object[] objparam)
    {       
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_CargarTitularEdit"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoTitular", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoPersona", int.Parse(objparam[2].ToString()));
                cmd.Parameters.AddWithValue("@in_codigoProducto", int.Parse(objparam[3].ToString()));
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarProductoProcedimiento(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoProceProducto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_usucodigo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_terminal", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_codigoproducto", int.Parse(objparam[2].ToString()));
                cmd.Parameters.AddWithValue("@in_asisanual", int.Parse(objparam[3].ToString()));
                cmd.Parameters.AddWithValue("@in_asismes", int.Parse(objparam[4].ToString()));
                cmd.Parameters.AddWithValue("@in_tipo", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_tipofecha", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", int.Parse(objparam[11].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi2", int.Parse(objparam[12].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi3", int.Parse(objparam[13].ToString()));
                cmd.Parameters.AddWithValue("@in_auxi4", int.Parse(objparam[14].ToString()));
                cmd.Parameters.AddWithValue("@EmptyProceProducto", dt);

                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch
        {
            return ds = null;
        }
    }

    public DataSet FunInsertarPresupuesto(object[] objparam, DataTable dt)
    {        
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_RegistrarPresupuesto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigocita", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_codigomedico", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_tiporegistro", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_observacion", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[6]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[7]);
                cmd.Parameters.AddWithValue("@EmptyPresupuesto", dt);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunInsertPresupuestoCab(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_InserPresupuestoCab"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigocita", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_codigomedico", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_tiporegistro", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[11].ToString());
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[12].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[13]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[14]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public string FunInsertPresupuestoDet(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_InsertPresupuestoDet"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigocabecera", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_codigoprestadora", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_codigocita", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_codigohico", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_observacion", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_codigopsde", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_codigoprocedimiento", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_procedimiento", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_pieza", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_pvp", objparam[10].ToString());
                cmd.Parameters.AddWithValue("@in_costo", objparam[11].ToString());
                cmd.Parameters.AddWithValue("@in_cobertura", objparam[12].ToString());
                cmd.Parameters.AddWithValue("@in_total", objparam[13].ToString());
                cmd.Parameters.AddWithValue("@in_prioridad", objparam[14].ToString());
                cmd.Parameters.AddWithValue("@in_autorizado", objparam[15].ToString());
                cmd.Parameters.AddWithValue("@in_realizado", objparam[16].ToString());
                cmd.Parameters.AddWithValue("@in_inicial", objparam[17].ToString());
                cmd.Parameters.AddWithValue("@in_realizar", objparam[18].ToString());
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[19].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[20].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[21].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[22].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[23].ToString());
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[24].ToString());
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[25].ToString());
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[26].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[27]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[28]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds.Tables[0].Rows[0][0].ToString();
            }
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public string FunInsertarSupervisorOdonto(object[] objparam, DataTable dt)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoSupervisorOdonto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_codsupervisor", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_descripcion", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_estado", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[3]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[4]);
                cmd.Parameters.AddWithValue("@EmptySupervisorOdonto", dt);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                mensaje = "";
            }
        }
        catch (Exception ex)
        {
            mensaje = ex.ToString();
        }
        return mensaje;
    }

    public DataSet FunInsertTextEditor(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_InsertEditor"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", int.Parse(objparam[0].ToString()));
                cmd.Parameters.AddWithValue("@in_codigo", int.Parse(objparam[1].ToString()));
                cmd.Parameters.AddWithValue("@in_texteditor", objparam[2].ToString());
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
        return ds;
    }

    public DataSet FunConsultaDatos1(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_ConsultaDatos1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.CommandTimeout = 200;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0].ToString());
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[1].ToString());
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[2].ToString());
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[3].ToString());
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[4].ToString());
                cmd.Parameters.AddWithValue("@in_auxv5", objparam[5].ToString());
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[6].ToString());
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[7].ToString());
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[8].ToString());
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[9].ToString());
                cmd.Parameters.AddWithValue("@in_auxi5", objparam[10].ToString());
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
        return ds;
    }

    public DataSet FunRepGerenCabecera(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_RepGerenMedCab1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_codigocamp", objparam[1]);
                cmd.Parameters.AddWithValue("@in_codigoprod", objparam[2]);
                cmd.Parameters.AddWithValue("@in_cliente", objparam[3]);
                cmd.Parameters.AddWithValue("@in_producto", objparam[4]);
                cmd.Parameters.AddWithValue("@in_tipoagenda", objparam[5]);
                cmd.Parameters.AddWithValue("@in_tipocliente", objparam[6]);
                cmd.Parameters.AddWithValue("@in_codtipocli", objparam[7]);
                cmd.Parameters.AddWithValue("@in_fechadesde", objparam[8]);
                cmd.Parameters.AddWithValue("@in_fechahasta", objparam[9]);
                cmd.Parameters.AddWithValue("@in_logocab", objparam[10]);
                cmd.Parameters.AddWithValue("@in_logopie", objparam[11]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[12]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[13]);
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[14]);                
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[15]);
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[16]);
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[17]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunRepGerenMedicos(string Sp, object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand(Sp))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_codigocamp", objparam[1]);
                cmd.Parameters.AddWithValue("@in_codigoprod", objparam[2]);                
                cmd.Parameters.AddWithValue("@in_fechadesde", objparam[3]);
                cmd.Parameters.AddWithValue("@in_fechahasta", objparam[4]);
                cmd.Parameters.AddWithValue("@in_codtipocli", objparam[5]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[6]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[7]);
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[8]);
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[9]);
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[10]);
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[11]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    public DataSet FunInsertSolictudExamen(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevaSolicitudExamen"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_prodcodigo", objparam[1]);
                cmd.Parameters.AddWithValue("@in_tipoidentificacion", objparam[2]);
                cmd.Parameters.AddWithValue("@in_numerodocumento", objparam[3]);
                cmd.Parameters.AddWithValue("@in_primernombre", objparam[4]);
                cmd.Parameters.AddWithValue("@in_segundonombre", objparam[5]);
                cmd.Parameters.AddWithValue("@in_primerapellido", objparam[6]);
                cmd.Parameters.AddWithValue("@in_segundopellido", objparam[7]);
                cmd.Parameters.AddWithValue("@in_genero", objparam[8]);
                cmd.Parameters.AddWithValue("@in_estadocivil", objparam[9]);
                cmd.Parameters.AddWithValue("@in_fechanacimiento", objparam[10]);
                cmd.Parameters.AddWithValue("@in_ciudcod", objparam[11]);
                cmd.Parameters.AddWithValue("@in_direccion", objparam[12]);
                cmd.Parameters.AddWithValue("@in_telefonocasa", objparam[13]);
                cmd.Parameters.AddWithValue("@in_telefonooficina", objparam[14]);
                cmd.Parameters.AddWithValue("@in_celular", objparam[15]);
                cmd.Parameters.AddWithValue("@in_email", objparam[16]);
                cmd.Parameters.AddWithValue("@in_exgccodigo", objparam[17]);
                cmd.Parameters.AddWithValue("@in_codigousuario", objparam[18]);
                cmd.Parameters.AddWithValue("@in_fechasolcita", objparam[19]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[20]);
                cmd.Parameters.AddWithValue("@in_docautoizacion", objparam[21]);
                cmd.Parameters.AddWithValue("@in_namedoc", objparam[22]);
                cmd.Parameters.AddWithValue("@in_typedoc", objparam[23]);
                cmd.Parameters.AddWithValue("@in_ext", objparam[24]);
                cmd.Parameters.AddWithValue("@in_exsecodigo", objparam[25]);
                cmd.Parameters.AddWithValue("@in_costo", objparam[26]);
                cmd.Parameters.AddWithValue("@in_pvp", objparam[27]);
                cmd.Parameters.AddWithValue("@in_adicional", objparam[28]);
                cmd.Parameters.AddWithValue("@in_exsocodigo", objparam[29]);
                cmd.Parameters.AddWithValue("@in_estado", objparam[30]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[31]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[32]);
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[33]);
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[34]);
                cmd.Parameters.AddWithValue("@in_auxv5", objparam[35]);
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[36]);
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[37]);
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[38]);
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[39]);
                cmd.Parameters.AddWithValue("@in_auxi5", objparam[40]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[41]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[42]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunInsertGrupoExamen(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoGrupoExamen"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_prescodigo", objparam[1]);
                cmd.Parameters.AddWithValue("@in_grexcodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_exascodigo", objparam[3]);
                cmd.Parameters.AddWithValue("@in_exsecodigo", objparam[4]);
                cmd.Parameters.AddWithValue("@in_grupoexamen", objparam[5]);
                cmd.Parameters.AddWithValue("@in_descripcion", objparam[6]);
                cmd.Parameters.AddWithValue("@in_estado", objparam[7]);
                cmd.Parameters.AddWithValue("@in_valor", objparam[8]);
                cmd.Parameters.AddWithValue("@in_pvp", objparam[9]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[10]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[11]);
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[12]);
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[13]);
                cmd.Parameters.AddWithValue("@in_auxv5", objparam[14]);
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[15]);
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[16]);
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[17]);
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[18]);
                cmd.Parameters.AddWithValue("@in_auxi5", objparam[19]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[20]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[21]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }

    public int FunNewGrupoExamenProducto(object[] objparam)
    {

        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoGrupoExamenProducto"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_exgccodigo", objparam[1]);
                cmd.Parameters.AddWithValue("@in_prodcodigo", objparam[2]);
                cmd.Parameters.AddWithValue("@in_grupoexamen", objparam[3]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[4]);
                cmd.Parameters.AddWithValue("@in_estado", objparam[5]);
                cmd.Parameters.AddWithValue("@in_exvacodigo", objparam[6]);
                cmd.Parameters.AddWithValue("@in_campo", objparam[7]);
                cmd.Parameters.AddWithValue("@in_field", objparam[8]);
                cmd.Parameters.AddWithValue("@in_operador", objparam[9]);
                cmd.Parameters.AddWithValue("@in_valor", objparam[10]);
                cmd.Parameters.AddWithValue("@in_exaccodigo", objparam[11]);
                cmd.Parameters.AddWithValue("@in_exsecodigo", objparam[12]);
                cmd.Parameters.AddWithValue("@in_costo", objparam[13]);
                cmd.Parameters.AddWithValue("@in_pvp", objparam[14]);
                cmd.Parameters.AddWithValue("@in_monto", objparam[15]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[16]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[17]);
                cmd.Parameters.AddWithValue("@in_auxv3", objparam[18]);
                cmd.Parameters.AddWithValue("@in_auxv4", objparam[19]);
                cmd.Parameters.AddWithValue("@in_auxv5", objparam[20]);
                cmd.Parameters.AddWithValue("@in_auxi1", objparam[21]);
                cmd.Parameters.AddWithValue("@in_auxi2", objparam[22]);
                cmd.Parameters.AddWithValue("@in_auxi3", objparam[23]);
                cmd.Parameters.AddWithValue("@in_auxi4", objparam[24]);
                cmd.Parameters.AddWithValue("@in_auxi5", objparam[25]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[26]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[27]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                codigo = int.Parse(ds.Tables[0].Rows[0]["Codigo"].ToString());
            }
        }
        catch(Exception ex)
        {
            codigo = 0;
        }
        return codigo;
    }

    public DataSet FunRegistroExamenes(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_RegistroExamenes"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_exsocodigo", objparam[1]);
                cmd.Parameters.AddWithValue("@in_examen1", objparam[2]);
                cmd.Parameters.AddWithValue("@in_nameexamen1", objparam[3]);
                cmd.Parameters.AddWithValue("@in_typeexamen1", objparam[4]);
                cmd.Parameters.AddWithValue("@in_extexamen1", objparam[5]);
                cmd.Parameters.AddWithValue("@in_examen2", objparam[6]);
                cmd.Parameters.AddWithValue("@in_nameexamen2", objparam[7]);
                cmd.Parameters.AddWithValue("@in_typeexamen2", objparam[8]);
                cmd.Parameters.AddWithValue("@in_extexamen2", objparam[9]);
                cmd.Parameters.AddWithValue("@in_examen3", objparam[10]);
                cmd.Parameters.AddWithValue("@in_nameexamen3", objparam[11]);
                cmd.Parameters.AddWithValue("@in_typeexamen3", objparam[12]);
                cmd.Parameters.AddWithValue("@in_extexamen3", objparam[13]);
                cmd.Parameters.AddWithValue("@in_examen4", objparam[14]);
                cmd.Parameters.AddWithValue("@in_nameexamen4", objparam[15]);
                cmd.Parameters.AddWithValue("@in_typeexamen4", objparam[16]);
                cmd.Parameters.AddWithValue("@in_extexamen4", objparam[17]);
                cmd.Parameters.AddWithValue("@in_examen5", objparam[18]);
                cmd.Parameters.AddWithValue("@in_nameexamen5", objparam[19]);
                cmd.Parameters.AddWithValue("@in_typeexamen5", objparam[20]);
                cmd.Parameters.AddWithValue("@in_extexamen5", objparam[21]);
                cmd.Parameters.AddWithValue("@in_observacion", objparam[22]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[23]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[24]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch(Exception ex)
        {
            return ds = null;
        }
    }

    public DataSet FunNuevoUsuarioAdmin(object[] objparam)
    {
        try
        {
            using (SqlCommand cmd = new SqlCommand("sp_NuevoUsuarioAdmin"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = Sqlcn;
                cmd.Parameters.AddWithValue("@in_tipo", objparam[0]);
                cmd.Parameters.AddWithValue("@in_codigoperm", objparam[1]);
                cmd.Parameters.AddWithValue("@in_codigousua", objparam[2]);
                cmd.Parameters.AddWithValue("@in_descripcion", objparam[3]);
                cmd.Parameters.AddWithValue("@in_estado", objparam[4]);
                cmd.Parameters.AddWithValue("@in_codigopede", objparam[5]);
                cmd.Parameters.AddWithValue("@in_codigopresta", objparam[6]);
                cmd.Parameters.AddWithValue("@in_codigociudad", objparam[7]);
                cmd.Parameters.AddWithValue("@in_estadopresta", objparam[8]);
                cmd.Parameters.AddWithValue("@in_codigotip", objparam[9]);
                cmd.Parameters.AddWithValue("@in_auxv1", objparam[10]);
                cmd.Parameters.AddWithValue("@in_auxv2", objparam[11]);
                cmd.Parameters.AddWithValue("@in_usucodigo", objparam[12]);
                cmd.Parameters.AddWithValue("@in_terminal", objparam[13]);
                Sqlcn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
        }
        catch (Exception ex)
        {
            return ds = null;
        }
    }
    #endregion
}