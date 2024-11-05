<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_Pruebas.aspx.cs" Inherits="Pry_PrestasaludWAP.Pruebas.WFrm_Pruebas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../css/Estilos.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        </div>
        <div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 5%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 25%"></td>
                    <td style="width: 35%"></td>
                    <td style="width: 15%"></td>
                    <td style="width: 5%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="4">
                        <cc1:Editor ID="txtEditor" runat="server" Height="200px" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div>
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
                        <h5>Grabar:</h5>
                    </td>
                    <td>
                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" OnClick="btnGrabar_Click" />
                    </td>
                    <td>
                        <h5>Mostrar:</h5>
                    </td>
                    <td>
                        <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" />
                    </td>
                    <td></td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
