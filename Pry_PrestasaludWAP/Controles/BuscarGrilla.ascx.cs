using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
public partial class Controles_BuscarGrilla : System.Web.UI.UserControl
{
    #region Variables
    GridView _grdGrillaBusqueda = new GridView();
    private DataSet _dsDatosGrilla;
    private const string cstrSeparador = "|";
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    #endregion

    #region Funciones y Procedimientos

    public void CargarComponente()
    {
        DataSet dsDatosGrilla = new DataSet();
        ListItem liItemCombo;

        calextFechaIni.Format = "dd/mm/yyyy";
        calextFechaFin.Format = "dd/mm/yyyy";
        txtFechaIni.Text = DateTime.Now.ToString("dd/mm/yyyy");
        txtFechaIni.Text = DateTime.Now.ToString("dd/mm/yyyy");

        _dsDatosGrilla = (DataSet)_grdGrillaBusqueda.DataSource;
        Session["dsDatosGrilla"] = (DataSet)_grdGrillaBusqueda.DataSource;
        ddlCampos.Items.Clear();

        if (_grdGrillaBusqueda.Columns[0].HeaderText != "Seleccionar")
        {
            for (int intI = 0; intI < _grdGrillaBusqueda.Columns.Count; intI++)
            {
                if (_grdGrillaBusqueda.Columns[intI].Visible == true)
                {

                    liItemCombo = new ListItem();
                    liItemCombo.Text = _grdGrillaBusqueda.Columns[intI].HeaderText;
                    liItemCombo.Value = _dsDatosGrilla.Tables[0].Columns[intI].DataType.ToString() + cstrSeparador + _dsDatosGrilla.Tables[0].Columns[intI].ColumnName.ToString() + cstrSeparador + intI.ToString();
                    ddlCampos.Items.Add(liItemCombo);
                }
            }
        }
        else
        {
            for (int intI = 1; intI < _grdGrillaBusqueda.Columns.Count; intI++)
            {
                if (_grdGrillaBusqueda.Columns[intI].Visible == true)
                {
                    liItemCombo = new ListItem();
                    liItemCombo.Text = _grdGrillaBusqueda.Columns[intI].HeaderText;
                    liItemCombo.Value = _dsDatosGrilla.Tables[0].Columns[intI - 1].DataType.ToString() + cstrSeparador + _dsDatosGrilla.Tables[0].Columns[intI - 1].ColumnName.ToString() + cstrSeparador + intI.ToString();
                    ddlCampos.Items.Add(liItemCombo);
                }
            }
        }

        funMostrarOcultarFechas(ddlCampos.SelectedValue);
    }

    private void funMostrarOcultarFechas(String pstrSelectedValue)
    {
        String[] strValores = pstrSelectedValue.Split(cstrSeparador.ToCharArray());
        if (strValores[0].ToString() == "System.DateTime")
        {
            //tbltexto.Visible = false;
            tblfechas.Visible = true;
        }
        else
        {
            //tbltexto.Visible = true;
            tblfechas.Visible = false;
        }
    }

    private void funBusqueda(String pstrNombreCol, String pstrTexto)
    {
        if (pstrTexto != String.Empty)
        {
            DataSet dsDatos = (DataSet)Session["dsDatosGrilla"];
            DataView dvVistaDatos = dsDatos.Tables[0].DefaultView;
            dvVistaDatos.RowFilter = string.Format("CONVERT(" + pstrNombreCol + ", System.String) LIKE '%{0}%'", pstrTexto.ToString());
            _grdGrillaBusqueda.DataSource = dvVistaDatos;
            //_grdGrillaBusqueda.AllowPaging = true;
            _grdGrillaBusqueda.DataBind();
            _grdGrillaBusqueda.AllowPaging = true;

        }
        else
        {
            _grdGrillaBusqueda.DataSource = (DataSet)Session["dsDatosGrilla"];
            _grdGrillaBusqueda.DataBind();
        }
    }

    private void funBusqueda(String pstrNombreCol, String pstrFechaIni, String pstrFechaFin)
    {
        DataSet dsDatos = (DataSet)Session["dsDatosGrilla"];
        //dsDatos.Locale = CultureInfo.CurrentCulture;
        DataView dvVistaDatos = dsDatos.Tables[0].DefaultView;
        DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
        dtfi.ShortDatePattern = "dd/MM/yyyy";
        dtfi.DateSeparator = "/";
        String strFechaIni = DateTime.Parse(pstrFechaIni, dtfi).ToString("MM/dd/yyyy");
        String strFechaFin = DateTime.Parse(pstrFechaFin, dtfi).ToString("MM/dd/yyyy");
        dvVistaDatos.RowFilter = pstrNombreCol + " >= #" + strFechaIni + "# AND " + pstrNombreCol + " <= #" + strFechaFin + "#";
        _grdGrillaBusqueda.DataSource = dvVistaDatos;

        //_grdGrillaBusqueda.AllowPaging = true;
        _grdGrillaBusqueda.DataBind();
        _grdGrillaBusqueda.AllowPaging = true;
    }

    public GridView GrdGrillaBusqueda
    {
        get { return _grdGrillaBusqueda; }
        set { _grdGrillaBusqueda = value; }
    }

    #endregion

    #region Botones y Eventos
    protected void ddlCampos_SelectedIndexChanged(object sender, EventArgs e)
    {
        funMostrarOcultarFechas(ddlCampos.SelectedValue);
    }
    protected void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        String[] strValues = ddlCampos.SelectedValue.Split(cstrSeparador.ToCharArray());
        if (strValues[0].ToString() == "System.DateTime")
        {
            funBusqueda(strValues[1], txtFechaIni.Text, txtFechaFin.Text);
        }
        else
        {
            funBusqueda(strValues[1], txtBuscar.Text.Trim());
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        String[] strValues = ddlCampos.SelectedValue.Split(cstrSeparador.ToCharArray());
        if (strValues[0].ToString() == "System.DateTime")
        {
            funBusqueda(strValues[1], txtFechaIni.Text, txtFechaFin.Text);
        }
        else
        {
            funBusqueda(strValues[1], txtBuscar.Text.Trim());
        }
    }
    #endregion
}