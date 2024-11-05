using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Pry_PrestasaludWAP.Examenes
{
    public partial class FrmCorregirExamen : Page
    {
        #region Variables
        DataSet dts = new DataSet();
        DataSet dtx = new DataSet();
        DataTable dtbexamenes = new DataTable();
        Object[] objparam = new Object[1];
        Byte[] bytes1, bytes2, bytes3, bytes4, bytes5;
        string filePath1 = "", filename1 = "", ext1 = "", type1 = "", name1 = "",
            filePath2 = "", filename2 = "", ext2 = "", type2 = "",
            filePath3 = "", filename3 = "", ext3 = "", type3 = "",
            filePath4 = "", filename4 = "", ext4 = "", type4 = "",
            filePath5 = "", filename5 = "", ext5 = "", type5 = "";
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuCodigo"] == null || Session["usuCodigo"].ToString() == "")
                Response.Redirect("~/Reload.html");
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            if (!IsPostBack)
            {
                try
                {
                    ViewState["CodigoEXSO"] = Request["CodigoEXSO"];
                    FunCargaMantenimiento();
                    Lbltitulo.Text = "Corregir Registro Exámenes";
                }
                catch (Exception ex)
                {
                    Lblerror.Text = ex.ToString();
                }
            }
        }
        #endregion

        #region Procedimientos y Funciones
        private void FunCargaMantenimiento()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[1] = "";
                objparam[2] = 156;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvDatos.DataSource = dts;
                GrdvDatos.DataBind();
                objparam[2] = 143;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                GrdvExamenes.DataSource = dts;
                GrdvExamenes.DataBind();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        private void FunDownloadDocument()
        {
            try
            {
                Array.Resize(ref objparam, 3);
                objparam[0] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[1] = "";
                objparam[2] = 153;
                dts = new Conexion(2, "").funConsultarSqls("sp_ConsultaDatos", objparam);
                type1 = dts.Tables[0].Rows[0]["Tipo"].ToString();
                name1 = dts.Tables[0].Rows[0]["Nombre"].ToString();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = type1;
                Response.AddHeader("content-disposition", "attachment;filename=" + name1);
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.BinaryWrite((byte[])dts.Tables[0].Rows[0]["DataBin"]);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }
        #endregion

        #region Botones y Eventos
        protected void ImgDownload_Click(object sender, ImageClickEventArgs e)
        {
            FunDownloadDocument();
        }

        protected void BtnGrabar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtObservacion.Text.Trim()))
                {
                    new Funciones().funShowJSMessage("Ingrese Observación..!", this);
                    return;
                }

                if (FileUpload1.HasFile == false && FileUpload2.HasFile == false && FileUpload3.HasFile == false
                    && FileUpload4.HasFile == false && FileUpload5.HasFile == false)
                {
                    new Funciones().funShowJSMessage("Seleccione Al Menos un Archivo Exámenes..!", this);
                    return;
                }
                else
                {
                    if (FileUpload1.HasFile == true)
                    {
                        filePath1 = FileUpload1.PostedFile.FileName;
                        filename1 = Path.GetFileName(filePath1);
                        ext1 = Path.GetExtension(filename1);
                        type1 = string.Empty;
                        if (filename1.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo1 Máximo 100 Caractéres..!", this);
                            return;
                        }
                        switch (ext1)
                        {
                            case ".doc":
                            case ".docx":
                                type1 = "application/word";
                                break;
                            case ".pdf":
                                type1 = "application/pdf";
                                break;
                            case ".png":
                                type1 = "application/png";
                                break;
                            case ".jpg":
                                type1 = "application/jpg";
                                break;
                        }
                        if (type1 != string.Empty)
                        {
                            Stream fs = FileUpload1.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes1 = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }
                    if (FileUpload2.HasFile == true)
                    {
                        filePath2 = FileUpload2.PostedFile.FileName;
                        filename2 = Path.GetFileName(filePath2);
                        ext2 = Path.GetExtension(filename2);
                        type2 = string.Empty;
                        if (filename2.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo2 Máximo 100 Caractéres..!", this);
                            return;
                        }
                        switch (ext2)
                        {
                            case ".doc":
                            case ".docx":
                                type2 = "application/word";
                                break;
                            case ".pdf":
                                type2 = "application/pdf";
                                break;
                            case ".png":
                                type2 = "application/png";
                                break;
                            case ".jpg":
                                type2 = "application/jpg";
                                break;
                        }
                        if (type2 != string.Empty)
                        {
                            Stream fs = FileUpload2.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes2 = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }
                    if (FileUpload3.HasFile == true)
                    {
                        filePath3 = FileUpload3.PostedFile.FileName;
                        filename3 = Path.GetFileName(filePath3);
                        ext3 = Path.GetExtension(filename3);
                        type3 = string.Empty;
                        if (filename3.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo3 Máximo 100 Caractéres..!", this);
                            return;
                        }
                        switch (ext3)
                        {
                            case ".doc":
                            case ".docx":
                                type3 = "application/word";
                                break;
                            case ".pdf":
                                type3 = "application/pdf";
                                break;
                            case ".png":
                                type3 = "application/png";
                                break;
                            case ".jpg":
                                type3 = "application/jpg";
                                break;
                        }
                        if (type3 != string.Empty)
                        {
                            Stream fs = FileUpload3.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes3 = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }
                    if (FileUpload4.HasFile == true)
                    {
                        filePath4 = FileUpload4.PostedFile.FileName;
                        filename4 = Path.GetFileName(filePath4);
                        ext4 = Path.GetExtension(filename4);
                        type4 = string.Empty;
                        if (filename4.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo4 Máximo 100 Caractéres..!", this);
                            return;
                        }
                        switch (ext4)
                        {
                            case ".doc":
                            case ".docx":
                                type4 = "application/word";
                                break;
                            case ".pdf":
                                type4 = "application/pdf";
                                break;
                            case ".png":
                                type4 = "application/png";
                                break;
                            case ".jpg":
                                type4 = "application/jpg";
                                break;
                        }
                        if (type4 != string.Empty)
                        {
                            Stream fs = FileUpload4.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes4 = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }
                    if (FileUpload5.HasFile == true)
                    {
                        filePath5 = FileUpload5.PostedFile.FileName;
                        filename5 = Path.GetFileName(filePath5);
                        ext5 = Path.GetExtension(filename5);
                        type5 = string.Empty;
                        if (filename5.Length > 100)
                        {
                            new Funciones().funShowJSMessage("Nombre del Archivo5 Máximo 100 Caractéres..!", this);
                            return;
                        }
                        switch (ext5)
                        {
                            case ".doc":
                            case ".docx":
                                type5 = "application/word";
                                break;
                            case ".pdf":
                                type5 = "application/pdf";
                                break;
                            case ".png":
                                type5 = "application/png";
                                break;
                            case ".jpg":
                                type5 = "application/jpg";
                                break;
                        }
                        if (type5 != string.Empty)
                        {
                            Stream fs = FileUpload5.PostedFile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            bytes5 = br.ReadBytes((Int32)fs.Length);
                        }
                        else
                        {
                            new Funciones().funShowJSMessage("Seleccione Archivos de Tipo (.doc,.docx,.pdf,.png,.jpg", this);
                            return;
                        }
                    }
                }
                Array.Resize(ref objparam, 25);
                objparam[0] = 0;
                objparam[1] = int.Parse(ViewState["CodigoEXSO"].ToString());
                objparam[2] = bytes1 ?? (new Byte[0]);
                objparam[3] = filename1;
                objparam[4] = type1;
                objparam[5] = ext1;
                objparam[6] = bytes2 ?? (new Byte[0]);
                objparam[7] = filename2;
                objparam[8] = type2;
                objparam[9] = ext2;
                objparam[10] = bytes3 ?? (new Byte[0]);
                objparam[11] = filename3;
                objparam[12] = type3;
                objparam[13] = ext3;
                objparam[14] = bytes4 ?? (new Byte[0]);
                objparam[15] = filename4;
                objparam[16] = type4;
                objparam[17] = ext4;
                objparam[18] = bytes5 ?? (new Byte[0]);
                objparam[19] = filename5;
                objparam[20] = type5;
                objparam[21] = ext5;
                objparam[22] = TxtObservacion.Text.Trim().ToUpper();
                objparam[23] = int.Parse(Session["usuCodigo"].ToString());
                objparam[24] = Session["MachineName"].ToString();
                dts = new Conexion(2, "").FunRegistroExamenes(objparam);
                if (dts.Tables[0].Rows[0][0].ToString() == "OK-Update")
                    Response.Redirect("FrmExamenNegadoAdmin.aspx?MensajeRetornado=Guardado con Éxito", true);
                else
                    Response.Redirect("FrmExamenNegadoAdmin.aspx?MensajeRetornado=Si registro Existe un Error", true);
            }
            catch (Exception ex)
            {
                Lblerror.Text = ex.ToString();
            }
        }

        protected void BtnSalir_Click(object sender, EventArgs e)
        {
            Response.Redirect("FrmExamenNegadoAdmin.aspx", true);
        }
        #endregion
    }
}