<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoSecuencialxD.aspx.cs" Inherits="Pry_PrestasaludWAP.Configuracion.FrmNuevoSecuencialxD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%; height: 100%">
                                <tr>
                                    <td style="width: 15%">
                                    </td>
                                    <td style="width: 35%">
                                    </td>
                                    <td style="width: 15%"></td>
                                    <td style="width: 35%"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Producto:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Secuencial:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSecuencial" runat="server" CssClass="form-control alinearDerecha" MaxLength="5" Width="50%" TabIndex="2"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtSecuencial_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtSecuencial">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                    <td>
                                        <h5>Medicamento:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlMedicamento" runat="server" AutoPostBack="True" CssClass="form-control" Width="50%" TabIndex="3">
                                            <asp:ListItem Value="0">--Medicamento--</asp:ListItem>
                                            <asp:ListItem Value="SI">SI</asp:ListItem>
                                            <asp:ListItem Value="NO">NO</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 50%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="GRABAR" Width="120px" CssClass="button" TabIndex="10" OnClick="btnGrabar_Click" />
                                    </td>
                                    <td style="text-align: left; width: 50%">
                                        <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" TabIndex="11" OnClick="btnSalir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
