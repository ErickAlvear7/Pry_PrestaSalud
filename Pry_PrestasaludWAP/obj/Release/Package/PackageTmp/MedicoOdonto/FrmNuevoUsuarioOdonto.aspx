<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoUsuarioOdonto.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmNuevoUsuarioOdonto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />    
    <link href="../css/Estilos.css" rel="stylesheet" />    
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
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
                                <td><h5>Criterio:</h5></td>
                                <td>
                                    <asp:TextBox ID="txtCriterio" runat="server" CssClass="form-control" TabIndex="1" Width="100%"></asp:TextBox>
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgBuscar" runat="server" Height="20px" ImageUrl="~/Botones/Buscar.png" OnClick="imgBuscar_Click" TabIndex="2" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Asignar Usuario:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="ddlAsignarUsuario" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlAsignarUsuario_SelectedIndexChanged" TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="lblNombres" visible="false">Nombres:</h5>
                                </td>
                                <td>
                                    <h5 runat="server" id="txtNombres" visible="false"></h5>
                                </td>
                                <td>
                                    <h5 runat="server" id="lblApellidos" visible="false">Apellidos:</h5>
                                </td>
                                <td>
                                    <h5 runat="server" id="txtApellidos" visible="false"></h5>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="lbllogin" visible="false">Login:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLogin" runat="server" Width="100%" MaxLength="16" CssClass="form-control" Visible="False" TabIndex="4"></asp:TextBox>
                                </td>
                                <td>
                                    <h5 runat="server" id="lblpassword" visible="false">Password:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" Width="100%" MaxLength="16" TextMode="Password" CssClass="form-control" Visible="False" TabIndex="5"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updBotones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="5" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="6" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
