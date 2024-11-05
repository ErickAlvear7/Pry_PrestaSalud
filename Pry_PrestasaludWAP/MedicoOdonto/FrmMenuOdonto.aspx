<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMenuOdonto.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmMenuOdonto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/jscript">

        function salir() {
            if (confirm("Esta seguro que desea cerrar la sesion???"))
                window.parent.location = "../WFrm_Login.aspx";
        }

        function hideLeftPanel() {
            var oculto = '25%,*';

            if (oculto == parent.document.getElementById('MenuOdonto').cols) {
                parent.document.getElementById('MenuOdonto').cols = '3%,*'
            } else {
                parent.document.getElementById('MenuOdonto').cols = '25%,*'
            }
        }


    </script>
    <style type="text/css">
*{-webkit-box-sizing:border-box;-moz-box-sizing:border-box;box-sizing:border-box}*,:after,:before{color:#000!important;text-shadow:none!important;background:0 0!important;-webkit-box-shadow:none!important;box-shadow:none!important}</style>
</head>
<%--<body style="text-align: left; background-image:url('../Images/fondo_lateral.jpg')">--%>
<body>
    <form id="form1" runat="server">
        <div style="text-align: left">
            <%--<table border="0" cellpadding="0" cellspacing="0" style="/*border-style: none; background-image: url('../Images/fondo_lateral.jpg'); height: 100%; width: 200px;*/">--%>
            <table border="0" cellpadding="0" cellspacing="0" style="border-style: none; height: 100%; width: 200px;">
                <tr>
                    <td style="width:10px"></td>
                    <td style="width:200px"></td>
                </tr>
                <tr>
                    <td>
                        <asp:ImageButton ID="ImageButton1" runat="server" OnClientClick="return hideLeftPanel()" Height="30px" ImageUrl="~/Botones/ocultarFrame.png" Width="20px" />
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:Image ID="ImgLogo" runat="server" Height="80px" Width="150px" />
                        <%--<asp:Image ID="Image2" runat="server" Height="80px" ImageUrl="~/Images/logo1_gestiona.jpg" Width="150px" />--%>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="return salir()">Cerrar Sesión</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center;">
                        <br />
                       <h2 style="left: 16px; width: 186px; top: 46px; text-align: center; color: #20365F;">PrestaSalud</h2>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:Label ID="LblUsuario" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Aqua" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="vertical-align: top; height: 450px; text-align: left;">
                        <asp:Panel ID="pnlMenu" runat="server" Height="450px" ScrollBars="Vertical">
                            <asp:TreeView ID="trvLista" runat="server" OnSelectedNodeChanged="trvLista_SelectedNodeChanged" ImageSet="Arrows" OnTreeNodePopulate="trvLista_TreeNodePopulate">
                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                <Nodes>
                                    <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Lista de Trabajo" Value="Lista de Trabajo"></asp:TreeNode>
                                </Nodes>
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                <ParentNodeStyle Font-Bold="False" />
                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                            </asp:TreeView>
                        </asp:Panel>

                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
