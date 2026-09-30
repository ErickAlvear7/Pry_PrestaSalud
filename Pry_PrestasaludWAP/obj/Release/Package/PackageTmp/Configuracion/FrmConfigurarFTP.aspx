<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmConfigurarFTP.aspx.cs" Inherits="Pry_PrestasaludWAP.Configuracion.FrmConfigurarFTP" %>

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
                                        <h5>Cliente:</h5>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:DropDownList ID="ddlCliente" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="1">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 15%">
                                        <h5>Ruta Archivo:</span></h5>
                                    </td>
                                    <td style="width: 35%">
                                        <asp:TextBox ID="txtRuta" runat="server" CssClass="form-control" MaxLength="250" TabIndex="2" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <h5>Nombre Archivo:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" MaxLength="150" Width="100%" TabIndex="3"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Extensión:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlExtension" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="4">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><h5>Formato Fecha:</h5></td>
                                    <td>
                                        <asp:DropDownList ID="ddlFormato" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" TabIndex="5">
                                        </asp:DropDownList>
                                    </td>
                                    <td><h5>Delimitado:</h5></td>
                                    <td>
                                        <asp:DropDownList ID="ddlDelimitado" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="6" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td><h5>Hora Inicio:</h5></td>
                                    <td>
                                        <asp:DropDownList ID="ddlHoraInicio" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="7" Width="100%">
                                        </asp:DropDownList>
                                    </td>
                                    <td><h5>UsuarioFTP:</h5></td>
                                    <td>
                                        <asp:TextBox ID="txtUsuarioFTP" runat="server" CssClass="form-control" MaxLength="20" TabIndex="8" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td><h5>Enviar Mail:</h5></td>
                                    <td>
                                        <asp:CheckBox ID="chkEnviar" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkEnviar_CheckedChanged" TabIndex="9" Text="NO"/>
                                    </td>
                                    <td><h5 runat="server" id="lblEstado" visible="false">Estado:</h5></td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" TabIndex="10" Visible="False" OnCheckedChanged="chkEstado_CheckedChanged" />
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
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" TabIndex="11" OnClick="btnGrabar_Click" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" TabIndex="12" OnClick="btnSalir_Click" />
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
