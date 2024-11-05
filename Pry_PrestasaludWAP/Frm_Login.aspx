<%@ Page Language="C#" AutoEventWireup="true" Inherits="Frm_Login" Codebehind="Frm_Login.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/login.css" rel="stylesheet" type="text/css" />
    <link href="~/Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Bootstrap/js/bootstrap.min.js"></script>
</head>
<body style="background-position: center 3%; background-repeat: no-repeat;">
    <form id="form1" runat="server">

        <table style="height: 550px; width: 100%">
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td style="width: 5%"></td>
                <td style="width: 30%"></td>
                <td style="width: 35%">
                    <asp:Panel ID="pnlLogin" runat="server" Width="100%" Height="250px" BorderStyle="Double" BorderColor="#99ccff" GroupingText="Iniciar Sesión">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 20%; text-align: left">
                                    <strong><span>Usuario</span>:</strong>
                                </td>
                                <td style="width: 55%; text-align: left">
                                    <asp:TextBox ID="txtUsuario" Width="100%" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 20%"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left">
                                    <strong><span>Contraseña</span>:</strong>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClave" Width="100%" runat="server" TextMode="Password" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center">
                                    <asp:Button ID="btnIngresar" runat="server" Width="120px" Text="Ingresar" OnClick="btnIngresar_Click" CssClass="btn btn-primary" TabIndex="3" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="text-align: center">
                                    <asp:Label ID="lblmensaje" runat="server" BackColor="Gold" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td style="width: 30%"></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>

    </form>
</body>
</html>
