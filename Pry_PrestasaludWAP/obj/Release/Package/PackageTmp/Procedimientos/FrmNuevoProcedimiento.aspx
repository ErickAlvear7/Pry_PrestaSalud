<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoProcedimiento.aspx.cs" Inherits="Pry_PrestasaludWAP.Procedimientos.FrmNuevoProcedimiento" %>

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
    <script type="text/javascript">
        function ValidarDecimales() {
            var numero = document.getElementById("<%=txtCosto.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157,68");
                document.getElementById("<%=txtCosto.ClientID%>").value = "";
                return false;
            }
        }
    </script>
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
                                        <h5>Procedimiento:</h5>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtProcedimiento" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td style="width: 15%">
                                        <h5>Descripción:</span></h5>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="500" onkeydown="return (event.keyCode!=13);" TextMode="MultiLine" Width="100%" TabIndex="2"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>PVP:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCosto" runat="server" CssClass="form-control upperCase" MaxLength="6" Width="100%" TabIndex="3"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Aplica a:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlAplica" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="4">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><h5>Tipo:</h5></td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="5">
                                        </asp:DropDownList>
                                    </td>
                                    <td><h5 runat="server" id="Label11" visible="false">Estado:</h5></td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" OnCheckedChanged="chkEstado_CheckedChanged" Visible="False" TabIndex="6" />
                                    </td>
                                </tr>
                                <tr>
                                    <td><h5>Por Defecto:</h5></td>
                                    <td>
                                        <asp:DropDownList ID="ddlPorDefetco" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="7">
                                        </asp:DropDownList>
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
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="8" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="9" />
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
