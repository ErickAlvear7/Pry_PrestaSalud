﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Tarea_FrmNuevaTarea" Codebehind="FrmNuevaTarea.aspx.cs" %>

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
                                <td style="width: 10%"></td>
                                <td style="width: 15%">
                                    <h5>Nombre Tarea:</h5>
                                </td>
                                <td style="width: 65%">
                                    <asp:TextBox ID="txtTarea" runat="server" Width="100%" MaxLength="80" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 10%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Ruta/Página aspx:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRuta" runat="server" Width="100%" MaxLength="80" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="Label3" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEstado" runat="server" Visible="False" AutoPostBack="True" OnCheckedChanged="chkEstado_CheckedChanged" Checked="True" Text="Activo" CssClass="form-control" TabIndex="3" />
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="4" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" OnClick="btnSalir_Click" CausesValidation="False" CssClass="button" TabIndex="5" />
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
