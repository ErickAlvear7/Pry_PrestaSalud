<%@ Page Language="C#" AutoEventWireup="true" Inherits="Reportes_BI_FrmReportBICargaFTP" Codebehind="FrmReportBICargaFTP.aspx.cs" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../css/Estilos.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; color: #20365F; font-family: Aparajita; font-size: 25px; font-weight: bold; text-transform: uppercase; text-align:center; text-decoration: underline;">
            <asp:Label ID="lbltitulo" runat="server"></asp:Label>
        </div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <hr />
        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
        </div>
        <div style="height:110px; width:100%; background-color: #F4F4F4;">
            <asp:Panel ID="Panel1" runat="server">
                <asp:UpdateProgress runat="server" id="PageUpdateProgress" AssociatedUpdatePanelID="UpdatePanel1">                                                                
                    <ProgressTemplate>
                            <div id="divProgress" >                                            
                                <h2>Generando...Espere un momento!...</h2>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/loadingbar.gif" Height="70px" Width="153px" />
                            </div>                                    
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width:100%">
                            <tr>
                                <td></td>
                                <td style="text-align: right;">
                                    <asp:Label ID="Label1" runat="server" CssClass="Label" Text="Seleccione Campain:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCampainIni" runat="server" Width="350px">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: left;">
                                    <asp:Label ID="Label2" runat="server" CssClass="Label" Text="Fecha Inicio:"></asp:Label>
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtFechaIni" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="txtFechaIni">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="txtFechaIni_MaskedEditExtender" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaIni">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width:10%"></td>
                                <td style="width: 15%;"></td>
                                <td style="width:30%; text-align: center;">
                                    <asp:Label ID="Label8" runat="server" CssClass="Label" Text="Comparar Con"></asp:Label>
                                </td>
                                <td style="width: 10%;"></td>
                                <td style="width: 10%;"></td>
                                <td style="width:25%">
                                    <asp:Label ID="Label5" runat="server" CssClass="Label" Text="Condición"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label3" runat="server" CssClass="Label" Text="Seleccione Campain:"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCampainFin" runat="server" Width="350px">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="Label" Text="Fecha Fin:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaFin" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" Enabled="True" Format="MM/dd/yyyy" TargetControlID="txtFechaFin">
                                    </asp:CalendarExtender>
                                    <asp:MaskedEditExtender ID="txtFechaFin_MaskedEditExtender" runat="server" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Enabled="True" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFin">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCondicion" runat="server">
                                        <asp:ListItem Value="=">=</asp:ListItem>
                                        <asp:ListItem Value="&gt;">&gt;</asp:ListItem>
                                        <asp:ListItem Value="&gt;=">&gt;=</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <table style="width:100%">
                <tr>
                    <td style="width:10%"></td>
                    <td style="text-align: right; width:35%">
                        <asp:Button ID="btnProcesar" runat="server" Text="Procesar" Width="100px" OnClick="btnProcesar_Click" />
                    </td>
                    <td style="width:30%">
                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="100px" OnClick="btnSalir_Click" />
                    </td>
                    <td style="width:25%"></td>
                </tr>
            </table>
            <hr />
            <table style="width:100%">
                <tr>
                    <td style="width:10%"></td>
                    <td style="width:80%" colspan="3"></td>
                    <td style="width:10%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:ImageButton ID="imgExportar" runat="server" ImageUrl="~/Images/excel.png" OnClick="imgExportar_Click" Visible="False" Width="40px" ToolTip="Exportar a Excel" />
                                    <asp:Label ID="Label6" runat="server" CssClass="Label" Text="Exportar Excel" Visible="False"></asp:Label>
                        <asp:ImageButton ID="imgExpCsv" runat="server" ImageUrl="~/Images/csv.jpg" OnClick="imgExpCsv_Click" Visible="False" ToolTip="Exportar a CSV" />
                                    <asp:Label ID="Label7" runat="server" CssClass="Label" Text="Exportar CSV" Visible="False"></asp:Label>
                    </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>                                
                                <asp:Panel ID="Panel2" runat="server" Height="250px" ScrollBars="Auto">
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False" 
                                        CellPadding="4" ForeColor="#333333" EmptyDataText="No existen Datos para mostrar" >
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Fecha_Registro" HeaderText="Fecha Carga" />
                                            <asp:BoundField DataField="Nombre_Campain" HeaderText="Campain" />
                                            <asp:BoundField DataField="Insert_Titular" HeaderText="Tit. Insertados" />
                                            <asp:BoundField DataField="Update_Titular" HeaderText="Tit. Acutalizados" />
                                            <asp:BoundField DataField="Insert_Beneficiario" HeaderText="Benef. Inserdatos" />
                                            <asp:BoundField DataField="Update_Beneficiario" HeaderText="Benef. Actualizados" />
                                        </Columns>
                                        <EditRowStyle BackColor="#999999" />
                                        <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle ForeColor="#333333" Font-Size="Small" BackColor="#F7F6F3" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                                    </asp:GridView>
                                </asp:Panel>                           
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
