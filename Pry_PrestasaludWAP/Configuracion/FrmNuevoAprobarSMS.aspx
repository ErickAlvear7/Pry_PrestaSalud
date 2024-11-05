<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoAprobarSMS.aspx.cs" Inherits="Pry_PrestasaludWAP.Configuracion.FrmNuevoAprobarSMS" %>

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
                                    <td style="width: 5%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 35%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Cliente:</h5>
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlCliente" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Variables:</h5>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lstVariableA" runat="server" Width="100%" Height="150px" AutoPostBack="True" OnSelectedIndexChanged="lstVariableA_SelectedIndexChanged" TabIndex="2"></asp:ListBox>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><h5>texto SMS:</h5></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSMS" runat="server" CssClass="form-control" MaxLength="500" TabIndex="3" Width="100%" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><h5 runat="server" id="lblEstado" visible="false">Estado:</h5></td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="chkEstado_CheckedChanged" TabIndex="4" Text="Activo" Visible="False" />
                                    </td>
                                    <td></td>
                                    <td></td>
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
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="5" OnClick="btnGrabar_Click" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="6" OnClick="btnSalir_Click" />
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
