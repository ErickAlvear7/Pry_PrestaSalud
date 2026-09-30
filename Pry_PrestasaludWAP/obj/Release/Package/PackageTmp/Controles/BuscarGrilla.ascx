<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controles_BuscarGrilla" Codebehind="BuscarGrilla.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style type="text/css">
    .auto-style1
    {
        height: 37px;
        width: 802px;
    }
    .auto-style2
    {
        width: 802px;
    }
    .auto-style3
    {
        height: 37px;
        width: 325px;
    }
</style>


<table runat="server" border="0" cellpadding="0" cellspacing="0" id="tblprinc" style="width:589px;">
  <tr >
    <td style="color: black; font-weight: bold; " class="auto-style1" >Buscar por:&nbsp;
        <asp:DropDownList ID="ddlCampos" runat="server" AutoPostBack="True" Width="136px" OnSelectedIndexChanged="ddlCampos_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:TextBox ID="txtBuscar" runat="server" OnTextChanged="txtBuscar_TextChanged"></asp:TextBox>&nbsp;
        </td>
      <td>   
          <asp:Button ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar" />
      </td>
    <td valign="top" class="auto-style3">   
        &nbsp;</td>
  </tr>
  <tr>
  <td valign="top" class="auto-style2">
    <table id="tblfechas" runat="server" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td style="width: 100px; text-align: left" valign="top">
            &nbsp;</td>
        <td style="width: 129px; text-align: center" valign="middle">
				  <asp:TextBox ID="txtFechaIni" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
				  <cc1:calendarextender id="calextFechaIni" runat="server" targetcontrolid="txtFechaIni"></cc1:calendarextender>
		</td>
	  </tr>
      <tr>
        <td style="width: 100px; text-align: left" valign="top">
            &nbsp;</td>
        <td style="width: 129px; text-align: center" valign="middle">
				  <asp:TextBox ID="txtFechaFin" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
					<cc1:calendarextender id="calextFechaFin" runat="server" targetcontrolid="txtFechaFin"></cc1:calendarextender>
				</td>
      </tr>
    </table>
  </td></tr>
</table>