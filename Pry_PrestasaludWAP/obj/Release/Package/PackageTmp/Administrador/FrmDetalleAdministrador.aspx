<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDetalleAdministrador.aspx.cs" Inherits="Pry_PrestasaludWAP.Administrador.FrmDetalleAdministrador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <script type="text/jscript">

        function salir() {
            if (confirm("Esta seguro que desea cerrar la sesion???"))
                window.parent.location = "../Frm_Login.aspx";
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-size: 14px; font-style: normal; font-weight: bold; color: #000080;">
        </div>
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
                        <asp:Image ID="ImgLogo" runat="server" Height="450px" ImageUrl="~/Images/LogoGeneral.png" Width="100%" />
                        <%--<asp:Image ID="Image2" runat="server" Height="320px" ImageUrl="../Images/LogoGes.png" Width="680px" />--%>
                    </td>
                </tr>
            </table>
            <h1 style="text-align:center; color:ActiveCaption">BIENVENIDO AL SISTEMA</h1>
            <h3 style="text-align:center">
                <strong><asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label></strong>
            </h3>
            <h4 style="text-align:center">
                <asp:Label ID="Label1" runat="server" CssClass="Label" Text="PRESTASALUD S.A. ---&gt; Sistema eXpert ----&gt; Agendamientos Médicos"></asp:Label>
            </h4>
        </asp:Panel>
    </form>
</body>
</html>
