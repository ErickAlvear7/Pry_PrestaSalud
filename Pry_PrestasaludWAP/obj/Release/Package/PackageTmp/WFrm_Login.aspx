<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_Login.aspx.cs" Inherits="Pry_PrestasaludWAP.WFrm_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/Logueo.css" rel="stylesheet" type="text/css" />
    <link href="css/Estilos.css" rel="stylesheet" type="text/css" />
    <link href="~/Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="~/Bootstrap/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--<div style="background-image:url('/css/Images/FondoWebPresta.png');width:100%;height:100%;min-height:300px;min-width:300px;background-repeat:no-repeat;">--%>
            <div id="tkid" runat="server" ></div>
            <asp:Panel ID="pnlLogueo" runat="server" Width="100%">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%"></td>
                        <td style="width: 35%"></td>
                        <td style="width: 30%"></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlDiv1" runat="server" Height="10px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Panel ID="pnlLogin" runat="server" Height="350px" Width="100%">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 25%"></td>
                                        <td style="width: 5%"></td>
                                        <td style="width: 35%"></td>
                                        <td style="width: 35%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Panel ID="pnlDatosLogin" runat="server" Height="150px" Width="100%">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 30%"></td>
                                                        <td style="width: 70%"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Panel ID="pnlDiv2" runat="server" Height="185px"></asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <h5 style="color: #007082; font-weight: bold">Usuario:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtukyxz" runat="server" CssClass="form-control" autocomplete="off" MaxLength="80" TabIndex="1" Width="90%"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <h5 style="color: #007082; font-weight: bold;">Password:</h5>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtppkd" runat="server" CssClass="form-control" MaxLength="20" TabIndex="2" Width="90%" TextMode="Password"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:ImageButton ID="btnIngresar" runat="server" ImageUrl="~/Botones/btnInciarsesion.png" OnClick="btnIngresar_Click" TabIndex="3" Width="50%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="text-align: center">
                                                            <asp:Label ID="lblmensaje" runat="server" BackColor="Gold" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Panel ID="pnlDiv0" runat="server" Height="50px"></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2" style="text-align: center">
                            <asp:Label ID="lblPie1" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="20pt" ForeColor="#007082" Text="Sistema de Agendamientos Médicos"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
