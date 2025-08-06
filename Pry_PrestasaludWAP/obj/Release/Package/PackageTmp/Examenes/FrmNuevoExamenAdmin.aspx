<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoExamenAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmNuevoExamenAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }

        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

        .overlayContent h2 {
            font-size: 18px;
            font-weight: bold;
            color: #000;
        }

        .overlayContent img {
            width: 80px;
            height: 80px;
        }
    </style>
    <script>
        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtValorExamen.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtValorExamen.ClientID%>").value = "";
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 30%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Examen:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="TxtExamen" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="150" TabIndex="1"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Descripción:</h5>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="TxtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="250" TabIndex="2" Width="100%" Height="80px" TextMode="MultiLine"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Categoría:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCategoria" runat="server" Width="100%" CssClass="form-control" TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5>
                                    Sub Categoría:</td>
                                <td>
                                    <asp:DropDownList ID="DdlSubCategoria" runat="server" Width="100%" CssClass="form-control" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Valor Examen:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtValorExamen" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="5" Width="100%">0.00</asp:TextBox>
                                </td>
                                <td>
                                    <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" Visible="False" Text="Activo" Checked="True" CssClass="form-control" TabIndex="6" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="panel panel-default">
            <asp:UpdatePanel ID="UpdOpciones" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 50%">
                                <asp:Button ID="BntGrabar" runat="server" Text="Grabar" Width="120px" OnClick="BntGrabar_Click" CssClass="button" TabIndex="7" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 50%">
                                <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="BtnSalir_Click" CssClass="button" TabIndex="8" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
