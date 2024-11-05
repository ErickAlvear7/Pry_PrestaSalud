<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WFrm_PruebaTokenHumana.aspx.cs" Inherits="Pry_PrestasaludWAP.Pruebas.WFrm_PruebaTokenHumana" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    </head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%">
                <tr>
                    <td style="width:25%"></td>
                    <td style="width:25%"></td>
                    <td style="width:25%"></td>
                    <td style="width:25%"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="BtnToken" runat="server" Height="83px" OnClick="BtnToken_Click" Text="Get Token" Width="158px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
