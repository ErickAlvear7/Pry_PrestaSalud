<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mantenedor_FrmDetalle" Codebehind="FrmDetalle.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <script type="text/jscript">

        function salir() {
            if (confirm("Esta seguro que desea cerrar la sesion???"))
                window.parent.location = "../WFrm_Login.aspx";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="Panel1" runat="server" BorderStyle="Double" BorderColor="#99ccff" Height="100%">
            <table style="width: 100%">
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Image ID="imgLogo" runat="server" Height="450px" ImageUrl="~/Images/LogoGeneral.png" Width="100%" />
                    </td>
                </tr>
            </table>
            <h1 style="text-align:center; color:ActiveCaption">BIENVENIDO AL SISTEMA</h1>
            <h3 style="text-align:center">
                <strong><asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label></strong>
            </h3>
            <h4 style="text-align:center">
                <asp:Label ID="Label1" runat="server" CssClass="Label" Text="Sistema Agendamientos ---&gt; System CopyRigth 2018----&gt; "></asp:Label>
            </h4>
        </asp:Panel>
    </form>
</body>
</html>
