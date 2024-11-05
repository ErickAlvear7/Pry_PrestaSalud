using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


/// <summary>
/// Summary description for SegPrincipal
/// </summary>
public class SegPrincipal
{
    #region Variables
    public string gstrusuario = "";
    public string gstrterminal = "";
    public System.Windows.Forms.Form objform = new System.Windows.Forms.Form();
    private string[,] stramenus = new string[1, 3];
    private string[,] stramenustemp;
    int intcontmen = 0;
    #endregion

    #region Procedimientos
    public void crea_menu(DataTable pdtDatos, ref  System.Windows.Forms.MainMenu objmenu)
    {
        System.Windows.Forms.MenuItem objmenp = new System.Windows.Forms.MenuItem();
        System.Windows.Forms.MenuItem objmenh = new System.Windows.Forms.MenuItem();
        string strmenp = "";

        int intindex = 0;
        foreach (DataRow dr in pdtDatos.Rows)
        {
            //Creo menus principales 
            if (strmenp != dr["MEN_CODIGO"].ToString())
            {
                if (dr["MEN_NIVEL"].ToString() == "0")
                {
                    strmenp = dr["MEN_CODIGO"].ToString();
                    objmenp = new System.Windows.Forms.MenuItem();
                    intindex = objmenu.MenuItems.Add(objmenp);
                    objmenp.Text = dr["MEN_DESCRIPCION"].ToString();
                    ingresamenu(intindex, strmenp, "");
                }
                else
                {
                    strmenp = dr["MEN_CODIGO"].ToString();
                    objmenp = new System.Windows.Forms.MenuItem();
                    intindex = int.Parse(buscamenu(dr["MEN_PADRE"].ToString(), 1, 0));
                    intindex = objmenu.MenuItems[intindex].MenuItems.Add(objmenp);
                    objmenp.Text = dr["MEN_DESCRIPCION"].ToString();
                    ingresamenu(intindex, strmenp, "");
                }

            }
            //Creo tareas de menu 
            objmenh = new System.Windows.Forms.MenuItem();
            intindex = objmenp.MenuItems.Add(objmenh);
            objmenh.Text = dr["TAR_DESCRIPCION"].ToString();
            ingresamenu(intindex, dr["TAR_DESCRIPCION"].ToString(), dr["TAR_PROGRAMA"].ToString());
            objmenh.Click += Click_menus;
        }
        strmenp = "alir";
        objmenp = new System.Windows.Forms.MenuItem();
        intindex = objmenu.MenuItems.Add(objmenp);
        objmenp.Text = "Salir";
        ingresamenu(intindex, strmenp, "");
        //Creo Salida
        //Creo tareas de menu 
        objmenh = new System.Windows.Forms.MenuItem();
        intindex = objmenp.MenuItems.Add(objmenh);
        objmenh.Text = "Salir de SICAFM";
        ingresamenu(intindex, "Salir de SICAFM", "UnLoad");
        objmenh.Click += Click_menus;
    }
    public void crea_menuweb(DataTable pdtDatos, ref  System.Web.UI.WebControls.TreeView objmenu)
    {
        System.Web.UI.WebControls.TreeNode objmenpadre = new System.Web.UI.WebControls.TreeNode();
        System.Web.UI.WebControls.TreeNode objmenp = new System.Web.UI.WebControls.TreeNode();
        System.Web.UI.WebControls.TreeNode objmenh = new System.Web.UI.WebControls.TreeNode();
        string strmenp = "", strpadre = "";
        objmenu.Nodes.Clear();

        int intindex = 0, intmenupadre = 0;
        foreach (DataRow dr in pdtDatos.Rows)
        {
            //Crear Menu Padre
            if (int.Parse(dr["MEPA_CODIGO"].ToString()) > 0)
            {
                intmenupadre = 1;
                if (strpadre != dr["MEPA_CODIGO"].ToString())
                {
                    objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                    strpadre = dr["MEPA_CODIGO"].ToString();
                    objmenpadre = new System.Web.UI.WebControls.TreeNode();
                    objmenu.Nodes.Add(objmenpadre);
                    intindex = objmenp.ChildNodes.IndexOf(objmenpadre);
                    objmenpadre.Text = dr["MEPA_DESCRIPCION"].ToString();
                    objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                    objmenpadre.Collapse();
                    ingresamenu(intindex, strpadre, "");
                }
            }
            else intmenupadre = 0;
            
            //Creo menus principales 
            if (strmenp != dr["MEN_CODIGO"].ToString())
            {
                objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                strmenp = dr["MEN_CODIGO"].ToString();
                objmenp = new System.Web.UI.WebControls.TreeNode();
                if (intmenupadre == 1) objmenpadre.ChildNodes.Add(objmenp);
                else objmenu.Nodes.Add(objmenp);
                intindex = objmenp.ChildNodes.IndexOf(objmenp);
                objmenp.Text = dr["MEN_DESCRIPCION"].ToString();
                objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
                objmenp.Collapse();
                ingresamenu(intindex, strmenp, "");
            }
            //Creo tareas de menu 
            objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
            objmenh = new System.Web.UI.WebControls.TreeNode();
            objmenp.ChildNodes.Add(objmenh);
            intindex = objmenp.ChildNodes.IndexOf(objmenh);
            objmenh.Text = dr["TAR_DESCRIPCION"].ToString();
            objmenh.SelectAction = System.Web.UI.WebControls.TreeNodeSelectAction.Select;
            if (dr["TAR_PROGRAMA"].ToString() != "")
            {
                objmenh.NavigateUrl = dr["TAR_PROGRAMA"].ToString();
                objmenh.Target = dr["PANTALLA"].ToString(); //"CENTRO";

                objmenu.SelectedNodeChanged += new EventHandler(this.Click_menuweb);
            }
        }
    }
    private void ingresamenu(int intindex, string strnombre, string strproceso)
    {
        try
        {
            if (intcontmen > 0)
            {
                stramenustemp = new string[intcontmen + 1, 3];
                Array.Copy(stramenus, stramenustemp, Math.Min(stramenus.Length, stramenustemp.Length));
                stramenus = stramenustemp;
            }

            stramenus[intcontmen, 0] = intindex.ToString();
            stramenus[intcontmen, 1] = strnombre;
            stramenus[intcontmen, 2] = strproceso;
            intcontmen = intcontmen + 1;
        }
        catch
        {

        }
    }
    private string buscamenu(string strdato, int intcampo, int intretorno)
    {
        int inttam = 0;
        string strres = "";

        try
        {
            while (inttam < intcontmen)
            {

                if (stramenus[inttam, intcampo] == strdato)
                {
                    strres = stramenus[inttam, intretorno].ToString();
                    inttam = stramenus.Length;
                }
                else
                {
                    inttam = inttam + 1;
                }
            }
            return strres;
        }
        catch
        {
            return "-1";
        }
    }
    private void Click_menus(object sender, System.EventArgs e)
    {
        string strnombre = "";
        string strnomobj = "";
        string strres = "";
        string strfullPath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
        string strconect = strfullPath.Substring(0, strfullPath.LastIndexOf("\\"));
        try
        {
            strnombre = "";
            if (sender is System.Windows.Forms.MenuItem)
            {
                strnombre = ((System.Windows.Forms.MenuItem)sender).Text;
            }
            if (strnombre == "Salir ")
            {
                objform.Close();
            }
            else
            {
                object objUser;
                strnomobj = buscamenu(strnombre, 1, 2);
                System.Type oType = System.Type.GetTypeFromProgID(strnomobj);
                //System.Type oType = System.Type.GetTypeFromProgID(strconect + "\\SIPocketSync.dll");
                objUser = System.Activator.CreateInstance(oType);
                //Instancio el objeto
                oType.InvokeMember("Main", System.Reflection.BindingFlags.InvokeMethod, null, objform, null);
                objform.Show();

            }

        }
        catch (System.Exception er)
        {
            strres = er.Message;
        }
    }
    private void Click_menuweb(object sender, System.EventArgs e)
    {
        System.Web.UI.WebControls.TreeView objtrvmenu = new System.Web.UI.WebControls.TreeView();
        if (sender is System.Web.UI.WebControls.TreeView)
        {
            objtrvmenu = (System.Web.UI.WebControls.TreeView)sender;
            if (objtrvmenu.SelectedNode.NavigateUrl == "")
            {
                objtrvmenu.CollapseAll();
                objtrvmenu.SelectedNode.Expand();
            }
        }
    }
    #endregion
}