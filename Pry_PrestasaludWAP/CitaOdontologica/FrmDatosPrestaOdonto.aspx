<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDatosPrestaOdonto.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaOdontologica.FrmDatosPrestaOdonto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Datos Prestadora</title>
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
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 5%"></td>
                            </tr>    
                            <tr>
                                <td></td>
                                <td>
                                    <h5><strong>Dirección:</strong></h5>
                                </td>
                                <td colspan="5">
                                    <h5 runat="server" id="lblDireccion"></h5>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5><strong>Telefono_1:</strong></h5></td>
                                <td><h5 runat="server" id="lblTelefono1"></h5></td>
                                <td><h5><strong>Telefono_2:</strong></h5></td>
                                <td><h5 runat="server" id="lblTelefono2"></h5></td>
                                <td><h5><strong>Telefono_3:</strong></h5></td>
                                <td><h5 runat="server" id="lblTelefono3"></h5></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5><strong>Celular:</strong></h5></td>
                                <td><h5 runat="server" id="lblCelular"></h5></td>
                                <td><h5><strong>Fax:</strong></h5></td>
                                <td><h5 runat="server" id="lblFax"></h5></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5><strong>Email_1:</strong></h5></td>
                                <td colspan="2"><h5 runat="server" id="lblEmail1"></h5></td>
                                <td style="text-align: center"><h5><strong>Email_2:</strong></h5></td>
                                <td colspan="2"><h5 runat="server" id="lblEmail2"></h5></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5><strong>Email_3:</strong></h5></td>
                                <td colspan="2"><h5 runat="server" id="lblEmail3"></h5></td>
                                <td></td>
                                <td></td>
                                <td></td>
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
                                <td style="text-align: center">
                                    <asp:Button ID="btnSalir" runat="server" Text="SALIR" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" />
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
