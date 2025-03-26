using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// Summary description for Funciones
/// </summary>
public class Funciones
{
    #region Variables
    StreamWriter writer;
    private static SymmetricAlgorithm mCSP;
    String strPathLog = ConfigurationManager.AppSettings["PathLogs"].ToString();
    DataSet dt = new DataSet();
    Object[] objparam = new Object[1];
    public enum ConversionEnum { PorOrden, PorNombre };
    #endregion

    #region Funciones
    public void funCargarCombos(DropDownList ddlComos, DataSet dst)
    {
        ddlComos.Items.Clear();
        int intCont = 0;
        while (intCont < dst.Tables[0].Rows.Count)
        {
            ListItem listadatosx = new ListItem();
            listadatosx.Text = dst.Tables[0].Rows[intCont][0].ToString();
            if (dst.Tables[0].Rows[intCont][1].ToString().Trim() == "") listadatosx.Value = intCont.ToString();
            else listadatosx.Value = dst.Tables[0].Rows[intCont][1].ToString();
            ddlComos.Items.Add(listadatosx);
            intCont++;
        }
    }


    public bool IsDate(string strFecha)
    {
        bool bValid;
        try
        {
            DateTime myDT = DateTime.ParseExact(strFecha, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            bValid = true;
        }
        catch
        {
            bValid = false;
        }

        return bValid;
    }

    public bool IsDateNew(string strFecha)
    {
        bool bValid;
        try
        {
            DateTime myDT = DateTime.ParseExact(strFecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            bValid = true;
        }
        catch
        {
            bValid = false;
        }

        return bValid;
    }

    public bool IsDateNewx(string strFecha)
    {
        bool bValid;
        try
        {
            DateTime myDT = DateTime.ParseExact(strFecha, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            bValid = true;
        }
        catch
        {
            bValid = false;
        }

        return bValid;
    }


    public String funCrearLogsAuditoria(String rutalog, String category, String fuente, String description)
    {

        if (File.Exists(strPathLog + rutalog))
        {
            writer = File.AppendText(rutalog);
            writer.WriteLine(category + "|" + DateTime.Now.ToString() + "|" + fuente + "|" + description);
        }
        else
        {
            writer = File.CreateText(rutalog);
            writer.WriteLine("Category" + "|" + "Date and Time" + "|" + "Source" + "|" + "Description");
            writer.WriteLine(category + "|" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + fuente + "|" + description);
        }
        writer.Close();

        return "OK";
    }

    public Boolean ruta_bien_escrita(String rutaPagina)
    {
        if (rutaPagina.Length > 5)
            if (rutaPagina.Substring(rutaPagina.Length - 5, 5) == ".aspx")
                return true;
            else
                return false;
        else
            return false;
    }

    public String EncriptaMD5(string Texto)
    {
        try
        {
            ICryptoTransform ictEncriptado;
            MemoryStream mstMemoria;
            CryptoStream cytFlujo;
            byte[] bytArreglo;

            ictEncriptado = mCSP.CreateEncryptor(mCSP.Key, mCSP.IV);

            bytArreglo = Encoding.UTF8.GetBytes(Texto);

            mstMemoria = new MemoryStream();
            cytFlujo = new CryptoStream(mstMemoria, ictEncriptado, CryptoStreamMode.Write);
            cytFlujo.Write(bytArreglo, 0, bytArreglo.Length);
            cytFlujo.FlushFinalBlock();

            cytFlujo.Close(); cytFlujo = null;
            ictEncriptado.Dispose(); ictEncriptado = null;

            return Convert.ToBase64String(mstMemoria.ToArray());
        }
        catch (Exception ex)
        {
            return "ERROR:" + ex.Message;
        }
    }

    public String funDesencripta(String bynario)
    {
        try
        {
            ICryptoTransform ictEncriptado;
            MemoryStream mstMemoria;
            CryptoStream cytFlujo;
            byte[] bytArreglo;

            ictEncriptado = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);

            bytArreglo = Convert.FromBase64String(bynario);

            mstMemoria = new MemoryStream();
            cytFlujo = new CryptoStream(mstMemoria, ictEncriptado, CryptoStreamMode.Write);
            cytFlujo.Write(bytArreglo, 0, bytArreglo.Length);
            cytFlujo.FlushFinalBlock();

            cytFlujo.Close(); cytFlujo = null;
            ictEncriptado.Dispose(); ictEncriptado = null;

            return Encoding.UTF8.GetString(mstMemoria.ToArray());
        }
        catch (Exception ex)
        {
            return "ERROR:" + ex.Message;
        }
    }

    public Funciones()
    {
        mCSP = new TripleDESCryptoServiceProvider();
        mCSP.Key = new Byte[] { Convert.ToByte("71"), Convert.ToByte("24"), Convert.ToByte("103"), Convert.ToByte("58"), Convert.ToByte("162"), Convert.ToByte("235"), Convert.ToByte("211"), Convert.ToByte("130"), Convert.ToByte("134"), Convert.ToByte("212"), Convert.ToByte("56"), Convert.ToByte("119"), Convert.ToByte("70"), Convert.ToByte("108"), Convert.ToByte("91"), Convert.ToByte("113"), Convert.ToByte("189"), Convert.ToByte("247"), Convert.ToByte("9"), Convert.ToByte("17"), Convert.ToByte("157"), Convert.ToByte("9"), Convert.ToByte("65"), Convert.ToByte("35") };
        mCSP.IV = new Byte[] { Convert.ToByte("230"), Convert.ToByte("128"), Convert.ToByte("180"), Convert.ToByte("179"), Convert.ToByte("98"), Convert.ToByte("247"), Convert.ToByte("139"), Convert.ToByte("137") };
    }

    public void funRemoverElement(ListBox lstBox, ArrayList aryElement)
    {
        foreach (ListItem item in aryElement)
        {
            lstBox.Items.Remove(item);
        }
    }

    public void funOrdenar(ListBox lstbox)
    {
        System.Collections.SortedList sorted = new SortedList();
        foreach (ListItem litem in lstbox.Items)
        {
            sorted.Add(litem.Value, litem.Text);
        }
        lstbox.Items.Clear();
        foreach (String key in sorted.Keys)
        {
            lstbox.Items.Add(new ListItem(sorted[key].ToString(), key.ToString()));
        }
    }

    public void funPasarTodos(ListBox lstOrigen, ListBox lstDestino)
    {
        foreach (ListItem litem in lstOrigen.Items)
        {
            lstDestino.Items.Add(litem);
        }
        lstOrigen.Items.Clear();

    }

    public Boolean email_bien_escrito(String email)
    {
        if (email.Length > 0)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else return true;
    }

    public Boolean cedulaBienEscrita(String cedula)
    {
        int isNumeric;
        var total = 0;
        const int tamanoLongitudCedula = 10;
        int[] coeficientes = { 2, 1, 2, 1, 2, 1, 2, 1, 2 };
        const int numeroProvincias = 24;
        const int tercerDigito = 6;

        if (int.TryParse(cedula, out isNumeric) && cedula.Length == tamanoLongitudCedula)
        {
            var provincia = Convert.ToInt32(string.Concat(cedula[0], cedula[1], string.Empty));
            var digitoTres = Convert.ToInt32(cedula[2] + string.Empty);
            if ((provincia > 0 && provincia <= numeroProvincias) && digitoTres < tercerDigito)
            {
                var digitoVerificadorRecibido = Convert.ToInt32(cedula[9] + string.Empty);
                for (var f = 0; f < coeficientes.Length; f++)
                {
                    var valor = Convert.ToInt32(coeficientes[f] + string.Empty) * Convert.ToInt32(cedula[f] + string.Empty);
                    total = valor >= 10 ? total + (valor - 9) : total + valor;
                }
                var digitoVerificadorObtenido = total >= 10 ? (total % 10) != 0 ? 10 - (total % 10) : (total % 10) : total;
                return digitoVerificadorObtenido == digitoVerificadorRecibido;
            }
            return false;
        }
        return false;
    }

    public bool IsNumber(string strNumero)
    {
        bool bValid;
        try
        {
            int myNU = int.Parse(strNumero);
            bValid = true;
        }
        catch (Exception e)
        {
            bValid = false;
        }

        return bValid;
    }

    public void SetearGrid(GridView grvGrid, ImageButton imgSubir, ImageButton imgBajar, DataTable dtTable)
    {
        if (grvGrid.Rows.Count > 0)
        {
            imgSubir = (ImageButton)grvGrid.Rows[0].Cells[4].FindControl("imgSubirNivel");
            imgSubir.ImageUrl = "~/Botones/desactivada_up.png";
            imgSubir.Enabled = false;

            if (dtTable.Rows.Count == 1)
            {
                imgBajar = (ImageButton)grvGrid.Rows[0].Cells[5].FindControl("imgBajarNivel");
                imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
                imgBajar.Enabled = false;
            }

            if (dtTable.Rows.Count > 1)
            {
                imgBajar = (ImageButton)grvGrid.Rows[dtTable.Rows.Count - 1].Cells[5].FindControl("imgBajarNivel");
                imgBajar.ImageUrl = "~/Botones/desactivada_down.png";
                imgBajar.Enabled = false;
            }
        }
    }

    public void funCrearLogAuditoria(int usu_codigo, string frm, string evento, int linea)
    {
        Array.Resize(ref objparam, 4);
        objparam[0] = usu_codigo;
        objparam[1] = frm;
        objparam[2] = evento.Replace("'", "");
        objparam[3] = linea;
        dt = new Conexion(2, "").funConsultarSqls("sp_Logs", objparam);
    }

    public void funCargarComboHoraMinutos(DropDownList ddlCombo, string tipo)
    {
        switch (tipo)
        { 
            case "HORAS":
                for (int x = 7; x <= 23; x++)
                {
                    ListItem itemDatos = new ListItem();
                    itemDatos.Text = x < 10 ? "0" + x.ToString() : x.ToString();
                    itemDatos.Value = x < 10 ? "0" + x.ToString() : x.ToString();
                    ddlCombo.Items.Add(itemDatos);
                }
                break;
            case "MINUTOS":
                for (int x = 0; x <= 59; x++)
                {
                    ListItem itemDatos = new ListItem();
                    itemDatos.Text = x < 10 ? "0" + x.ToString() : x.ToString();
                    itemDatos.Value = x < 10 ? "0" + x.ToString() : x.ToString();
                    ddlCombo.Items.Add(itemDatos);
                }
                break;
        }
    }

    public static List<T> funDataTableList<T>(DataTable dt, ConversionEnum tipoConvert)
    {
        List<T> data = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            T item = GetItem<T>(row, tipoConvert);
            data.Add(item);
        }
        return data;
    }

    private static T GetItem<T>(DataRow dr, ConversionEnum tipoConversion)
    {
        Type temp = typeof(T);
        T obj = Activator.CreateInstance<T>();

        //solo para tipoConversion por Orden
        int LintContador = 0;
        PropertyInfo[] proOrden = temp.GetProperties();


        foreach (DataColumn column in dr.Table.Columns)
        {
            if (tipoConversion == ConversionEnum.PorNombre)
            {
                //conversion por nombre
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName].ToString() != "")
                        pro.SetValue(obj, dr[column.ColumnName], null);
                }
            }
            else if (tipoConversion == ConversionEnum.PorOrden)
            {
                if (dr[LintContador].ToString() != "")
                {
                    proOrden[LintContador].SetValue(obj, dr[LintContador], null);
                }
                LintContador++;
            }
            else
            {
                throw new NotImplementedException("No implementado para este tipo");
            }
        }
        return obj;
    }

    public int Edad(DateTime dtmFechaNacimiento)
    {
        int Edad = DateTime.Now.Year - dtmFechaNacimiento.Year;
        DateTime fechaNacimiento = dtmFechaNacimiento.AddYears(Edad);
        if (DateTime.Now.CompareTo(fechaNacimiento) < 0) Edad--;
        return Edad;
    }

    public string funCrearArchivoCita(string pathArchivo, object[] objdata)
    {
        string pathReturn = "";
        try
        {
            int pad = 15;
            StreamWriter w;
            w = File.CreateText(pathArchivo);
            w.WriteLine("Cliente" + "".PadRight(pad - "Cliente".Length, ' ') + ":" + "  " + objdata[0].ToString());
            w.WriteLine("Producto" + "".PadRight(pad - "Producto".Length, ' ') + ":" + "  " + objdata[1].ToString());
            w.WriteLine("Código_Cita" + "".PadRight(pad - "Código_Cita".Length, ' ') + ":" + "  " + objdata[2].ToString());
            w.WriteLine("Ciudad_Cita" + "".PadRight(pad - "Ciudad_Cita".Length, ' ') + ":" + "  " + objdata[3].ToString());
            w.WriteLine("Fecha_Cita" + "".PadRight(pad - "Fecha_Cita".Length, ' ') + ":" + "  " + objdata[4].ToString());
            w.WriteLine("Hora_Cita" + "".PadRight(pad - "Hora_Cita".Length, ' ') + ":" + "  " + objdata[5].ToString());
            w.WriteLine("Prestadora" + "".PadRight(pad - "Prestadora".Length, ' ') + ":" + "  " + objdata[6].ToString());
            w.WriteLine("Medico" + "".PadRight(pad - "Medico".Length, ' ') + ":" + "  " + objdata[7].ToString());
            w.WriteLine("Especialidad" + "".PadRight(pad - "Especialidad".Length, ' ') + ":" + "  " + objdata[8].ToString());
            w.WriteLine("Observacion" + "".PadRight(pad - "Observacion".Length, ' ') + ":" + "  " + objdata[9].ToString());
            w.WriteLine("Cedula_Titu" + "".PadRight(pad - "Cedula_Titu".Length, ' ') + ":" + "  " + objdata[10].ToString());
            w.WriteLine("Tipo" + "".PadRight(pad - "Tipo".Length, ' ') + ":" + "  " + objdata[11].ToString());
            w.WriteLine("Paciente" + "".PadRight(pad - "Paciente".Length, ' ') + ":" + "  " + objdata[12].ToString());
            w.WriteLine("Fecha_Naci" + "".PadRight(pad - "Fecha_Naci".Length, ' ') + ":" + "  " + objdata[13].ToString());
            w.WriteLine("Dirección" + "".PadRight(pad - "Dirección".Length, ' ') + ":" + "  " + objdata[14].ToString());
            w.WriteLine("Teléfonos" + "".PadRight(pad - "Teléfonos".Length, ' ') + ":" + "  " + objdata[15].ToString());
            w.Flush();
            w.Close();
            pathReturn = pathArchivo;
        }
        catch (Exception ex)
        {
            pathReturn = ""; 
        }
        return pathReturn;
    }

    public string funCrearArchivoCancel(string pathArchivo, object[] objdata)
    {
        string pathReturn = "";
        try
        {
            int pad = 15;
            StreamWriter w;
            w = File.CreateText(pathArchivo);
            w.WriteLine("Código_Cita" + "".PadRight(pad - "Código_Cita".Length, ' ') + ":" + "  " + objdata[0].ToString());
            w.WriteLine("Ciudad_Cita" + "".PadRight(pad - "Ciudad_Cita".Length, ' ') + ":" + "  " + objdata[1].ToString());
            w.WriteLine("Fecha_Cita" + "".PadRight(pad - "Fecha_Cita".Length, ' ') + ":" + "  " + objdata[2].ToString());
            w.WriteLine("Hora_Cita" + "".PadRight(pad - "Hora_Cita".Length, ' ') + ":" + "  " + objdata[3].ToString());
            w.WriteLine("Prestadora" + "".PadRight(pad - "Prestadora".Length, ' ') + ":" + "  " + objdata[4].ToString());
            w.WriteLine("Medico" + "".PadRight(pad - "Medico".Length, ' ') + ":" + "  " + objdata[5].ToString());
            w.WriteLine("Especialidad" + "".PadRight(pad - "Especialidad".Length, ' ') + ":" + "  " + objdata[6].ToString());
            w.WriteLine("Tipo" + "".PadRight(pad - "Tipo".Length, ' ') + ":" + "  " + objdata[7].ToString());
            w.WriteLine("Paciente" + "".PadRight(pad - "Paciente".Length, ' ') + ":" + "  " + objdata[8].ToString());
            w.WriteLine("Motivo" + "".PadRight(pad - "Motivo".Length, ' ') + ":" + "  " + objdata[9].ToString());
            w.WriteLine("Observación" + "".PadRight(pad - "Observación".Length, ' ') + ":" + "  " + objdata[10].ToString());
            w.Flush();
            w.Close();
            pathReturn = pathArchivo;
        }
        catch (Exception ex)
        {
            pathReturn = "";
        }
        return pathReturn;
    }

    public string funEnviarMail(string mailsTo, string subject, object[] objBody, string emailTemplate,
        string host, int port, bool enableSSl, string usuario, string password, string ePathAttach, string ePathLogo,
        string eAlterMail, string eDocMail, string eUsuMail,string fechaCita, string fechaNaci)
    {
        string mensaje = "";
        try
        {
            string body = ReplaceBody(objBody, emailTemplate,fechaCita, fechaNaci);
            mensaje = SendHtmlEmail(mailsTo, subject, body, host, port, enableSSl, usuario, password, ePathAttach, ePathLogo,
                eAlterMail, eDocMail, eUsuMail);
            
        }
        catch (Exception ex)
        {
            mensaje = ex.Message;
        }
        return mensaje;
    }

    public string funEnviarMailLink(string mailsTo,string subject, object[] objBody,string emailTemplate,
     string host,int port,bool enableSSl,string usuario,string password,string email,string pathLogo, string mailsalterna)
    {
        string mensaje = "";
        try
        {
            string body = ReplaceBodyLink(objBody, emailTemplate);
            mensaje = SendHtmlEmailLink(mailsTo, subject, body, host, port, enableSSl, usuario, password, email, pathLogo, mailsalterna);

        }
        catch (Exception ex)
        {
            mensaje = ex.Message;
            new Funciones().funCrearLogAuditoria(1, "Funciones.cs/funEnviarMailLink", ex.ToString(), 1);
        }
        return mensaje;
    }

    public string funEnviarMailMediLink(string subject, object[] objBody, string emailTemplate,
            string host, int port, bool enableSSl, string usuario, string password,string mailsalterna)
    {
        string mensaje = "";
        try
        {
            string body = ReplaceBodyMediLink(objBody, emailTemplate);
            mensaje = SendHtmlEmailMediLink(subject, body, host, port, enableSSl, usuario, password,mailsalterna);

        }
        catch (Exception ex)
        {
            mensaje = ex.Message;
            new Funciones().funCrearLogAuditoria(1, "Funciones.cs/funEnviarMailLink", ex.ToString(), 1);
        }
        return mensaje;
    }

    private string ReplaceBody(object[] oBody,string eTemplate,string fechaCita,string fechaNaci)
    {
        string body = "";
        using (StreamReader reader = new StreamReader(eTemplate))
        {
            body = reader.ReadToEnd();
        }
        
        body = body.Replace("{Cliente}", oBody[0].ToString());
        body = body.Replace("{Producto}", oBody[1].ToString());
        body = body.Replace("{Medicinas}", oBody[20].ToString());
        body = body.Replace("{CodigoCita}", oBody[2].ToString());
        body = body.Replace("{Ciudad}", oBody[3].ToString());
        body = body.Replace("{FecCita}", fechaCita);
        body = body.Replace("{Horacita}", oBody[5].ToString());
        body = body.Replace("{Prestadora}", oBody[6].ToString());
        body = body.Replace("{Direccion}", oBody[14].ToString());
        body = body.Replace("{Medico}", oBody[7].ToString());
        body = body.Replace("{Especialidad}", oBody[8].ToString());
        body = body.Replace("{Observa}", oBody[9].ToString());
        body = body.Replace("{Detalle}", oBody[22].ToString());
        body = body.Replace("{Cedula}", oBody[10].ToString());
        body = body.Replace("{Tipo}", oBody[11].ToString());
        body = body.Replace("{Paciente}", oBody[12].ToString());
        body = body.Replace("{FecNaci}", fechaNaci);
        body = body.Replace("{Telefonos}", oBody[15].ToString());
        body = body.Replace("{Usuario}", oBody[21].ToString());
        body = body.Replace("{TipoPago}", oBody[23].ToString());
        body = body.Replace("{Pie1}", oBody[16].ToString());
        body = body.Replace("{Pie2}", oBody[17].ToString());
        body = body.Replace("{Pie3}", oBody[18].ToString());
        body = body.Replace("{Pie4}", oBody[19].ToString());
        return body;
    }

    private string ReplaceBodyLink(object[] oBody, string eTemplate)
    {
        string body = "";
        using (StreamReader reader = new StreamReader(eTemplate))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{url}", oBody[0].ToString());
        body = body.Replace("{fecha}", oBody[1].ToString());
        body = body.Replace("{hora}", oBody[2].ToString());
        body = body.Replace("{medico}", oBody[3].ToString());
        body = body.Replace("{motivo}", oBody[4].ToString());
        body = body.Replace("{patient}", oBody[5].ToString());
        body = body.Replace("{documento}", oBody[6].ToString());
        body = body.Replace("{producto}", oBody[7].ToString());
        body = body.Replace("{Pie1}", oBody[8].ToString());
        body = body.Replace("{Pie2}", oBody[9].ToString());
        return body;
    }

    private string ReplaceBodyMediLink(object[] oBody, string eTemplate)
    {
        string body = "";
        using (StreamReader reader = new StreamReader(eTemplate))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{Cliente}", oBody[0].ToString());
        body = body.Replace("{Producto}", oBody[1].ToString());
        body = body.Replace("{Medicinas}", oBody[2].ToString());
        body = body.Replace("{CodigoCita}", oBody[3].ToString());
        body = body.Replace("{Ciudad}", oBody[4].ToString());
        body = body.Replace("{FecCita}", oBody[5].ToString());
        body = body.Replace("{Horacita}", oBody[6].ToString());
        body = body.Replace("{Prestadora}", oBody[7].ToString());
        body = body.Replace("{Direccion}", oBody[8].ToString());
        body = body.Replace("{Medico}", oBody[9].ToString());
        body = body.Replace("{Especialidad}", oBody[10].ToString());
        body = body.Replace("{Cedula}", oBody[11].ToString());
        body = body.Replace("{Tipo}", oBody[12].ToString());
        body = body.Replace("{Paciente}", oBody[13].ToString());
        body = body.Replace("{Telefonos}", oBody[14].ToString());
        body = body.Replace("{TipoPago}", oBody[15].ToString());
        body = body.Replace("{Pie1}", oBody[16].ToString());
        body = body.Replace("{Pie2}", oBody[17].ToString());
        return body;
    }


    public string funEnviarMailCancel(string mailsTo, string subject, object[] objBody, string emailTemplate,
        string host, int port, bool enableSSl, string usuario, string password, string ePathAttach, string ePathLogo,
        string eAlterMail, string eDocMail, string eUsuMail)
    {
        string mensaje = "";
        try
        {
            string body = ReplaceBodyCancel(objBody, emailTemplate);
            mensaje = SendHtmlEmail(mailsTo, subject, body, host, port, enableSSl, usuario, password, ePathAttach, ePathLogo,
                eAlterMail, eDocMail, eUsuMail);
        }
        catch (Exception ex)
        {
            mensaje = ex.ToString();
            funCrearLogAuditoria(1, "Envío Mail -- CANCELAR", ex.ToString(), 1);
        }
        return mensaje;
    }

    private string ReplaceBodyCancel(object[] oBody, string eTemplate)
    {
        string body = "";
        try
        {            
            using (StreamReader reader = new StreamReader(eTemplate))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{CodigoCita}", oBody[0].ToString());
            body = body.Replace("{Ciudad}", oBody[1].ToString());
            body = body.Replace("{FecCita}", oBody[2].ToString());
            body = body.Replace("{Horacita}", oBody[3].ToString());
            body = body.Replace("{Prestadora}", oBody[4].ToString());
            body = body.Replace("{Medico}", oBody[5].ToString());
            body = body.Replace("{Especialidad}", oBody[6].ToString());
            body = body.Replace("{Tipo}", oBody[7].ToString());
            body = body.Replace("{Paciente}", oBody[8].ToString());
            body = body.Replace("{Motivo}", oBody[9].ToString());
            body = body.Replace("{Observacion}", oBody[10].ToString());
            body = body.Replace("{Usuario}", oBody[15].ToString());
            body = body.Replace("{Pie1}", oBody[11].ToString());
            body = body.Replace("{Pie2}", oBody[12].ToString());
            body = body.Replace("{Pie3}", oBody[13].ToString());
            body = body.Replace("{Pie4}", oBody[14].ToString());
            
        }
        catch (Exception ex)
        {
            funCrearLogAuditoria(1, "Envío Mail", ex.ToString(), 1);
        }
        return body;
    }

    private string SendHtmlEmail(string mailTO, string subject, string body, string ehost, int eport, bool eEnableSSL, 
        string eusername, string epassword, string pathAttach, string pathLogo, string mailAlter, string mailDoc, string mailUsu)
    {
        string mensaje = "";
        using (MailMessage mailMessage = new MailMessage())
        {
            try
            {
                //Attachment archivo = new Attachment(pathAttach);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                LinkedResource theEmailImage = new LinkedResource(pathLogo);
                theEmailImage.ContentId = "myImageID";
                htmlView.LinkedResources.Add(theEmailImage);
                mailMessage.AlternateViews.Add(htmlView);
                mailMessage.From = new MailAddress(eusername);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(mailTO))
                {
                    string[] manyMails = mailTO.Split(',');
                    foreach (string toMails in manyMails)
                    {
                        mailMessage.To.Add(new MailAddress(toMails));
                    }
                }
                if (!string.IsNullOrEmpty(mailAlter))
                {
                    string[] alterMails = mailAlter.Split(',');
                    foreach (string alMalis in alterMails)
                    {
                        mailMessage.CC.Add(alMalis);
                    }
                }
                if (!string.IsNullOrEmpty(mailDoc))
                {
                    string[] docMails = mailDoc.Split(',');
                    foreach (string doMails in docMails)
                    {
                        mailMessage.Bcc.Add(doMails);
                    }
                }
                if(!string.IsNullOrEmpty(mailUsu))
                {
                    string[] usuMails = mailUsu.Split(',');
                    foreach (string usMails in usuMails)
                    {
                        mailMessage.Bcc.Add(usMails);
                    }
                }
                //mailMessage.Attachments.Add(archivo);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = eusername;
                NetworkCred.Password = epassword;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = NetworkCred;
                smtp.Host = ehost;
                smtp.Port = eport;
                smtp.EnableSsl = eEnableSSL;
                smtp.Send(mailMessage);
                mensaje = "";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                funCrearLogAuditoria(1, "Envío Mail - Noenvia", mensaje, 1);
            }
            return mensaje;
        }
    }

    private string SendHtmlEmailLink(string mailTO,string subject,string body,string ehost,int eport,bool eEnableSSL,
    string eusername,string epassword,string email,string pathLogo, string mailsalterna)
    {
        string mensaje = "";
        using (MailMessage mailMessage = new MailMessage())
        {
            try
            {
                //Attachment archivo = new Attachment(pathAttach);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                LinkedResource theEmailImage = new LinkedResource(pathLogo);
                theEmailImage.ContentId = "myImageID";
                htmlView.LinkedResources.Add(theEmailImage);
                mailMessage.AlternateViews.Add(htmlView);
                mailMessage.From = new MailAddress(eusername);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(mailTO))
                {
                    mailMessage.To.Add(new MailAddress(mailTO));
                }

                if (!string.IsNullOrEmpty(email))
                {
                    mailMessage.CC.Add(email);
                }

                if (!string.IsNullOrEmpty(mailsalterna))
                {
                    string[] docMails = mailsalterna.Split(',');
                    foreach (string doMails in docMails)
                    {
                        //mailMessage.Bcc.Add(doMails); COPIA OCULTA
                        mailMessage.CC.Add(doMails);
                    }
                }

                //mailMessage.Attachments.Add(archivo);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = eusername;
                NetworkCred.Password = epassword;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = NetworkCred;
                smtp.Host = ehost;
                smtp.Port = eport;
                smtp.EnableSsl = eEnableSSL;
                smtp.Send(mailMessage);
                mensaje = "";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                new Funciones().funCrearLogAuditoria(1, "Funciones.cs/SendHtmlEmailLink", ex.ToString(), 1);
            }
            return mensaje;
        }
    }

    private string SendHtmlEmailMediLink(string subject, string body, string ehost, int eport, bool eEnableSSL,
               string eusername, string epassword,string mailsalterna)
    {
        string mensaje = "";
        using (MailMessage mailMessage = new MailMessage())
        {
            try
            {
                //Attachment archivo = new Attachment(pathAttach);
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                //LinkedResource theEmailImage = new LinkedResource(pathLogo);
                //theEmailImage.ContentId = "myImageID";
                //htmlView.LinkedResources.Add(theEmailImage);
                mailMessage.AlternateViews.Add(htmlView);
                mailMessage.From = new MailAddress(eusername);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
       
                if (!string.IsNullOrEmpty(mailsalterna))
                {
                    string[] docMails = mailsalterna.Split(',');
                    foreach (string doMails in docMails)
                    {
                        //mailMessage.Bcc.Add(doMails); COPIA OCULTA
                        mailMessage.CC.Add(doMails);
                    }
                }

                //mailMessage.Attachments.Add(archivo);
                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                NetworkCred.UserName = eusername;
                NetworkCred.Password = epassword;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = NetworkCred;
                smtp.Host = ehost;
                smtp.Port = eport;
                smtp.EnableSsl = eEnableSSL;
                smtp.Send(mailMessage);
                mensaje = "";
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                new Funciones().funCrearLogAuditoria(1, "Funciones.cs/SendHtmlEmailLink", ex.ToString(), 1);
            }
            return mensaje;
        }
    }
    public void funShowJSMessage(string message, Control pagina)
    {
        ScriptManager.RegisterClientScriptBlock(pagina, pagina.GetType(), "alert", "alert('" + message + "');", true);
    }
    #endregion
}