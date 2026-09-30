<%@ Page Language="C#" AutoEventWireup="true" Inherits="Mantenedor_FrmMenu" CodeBehind="FrmMenu.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
            var oculto = '18%,*';

            if (oculto == parent.document.getElementById('MenuDetalle').cols) {
                parent.document.getElementById('MenuDetalle').cols = '3%,*'
            } else {
                parent.document.getElementById('MenuDetalle').cols = '18%,*'
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div style="text-align: left">
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
                        <asp:Image ID="imgLog" runat="server" Height="80px" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Cerrar Sesión</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center;">
                        <asp:Timer ID="TmrMenu" runat="server" OnTick="TmrMenu_Tick" Enabled="False">
                        </asp:Timer>
                        <br />
                        <h2 style="left: 16px; width: 186px; top: 46px; text-align: center; color: #20365F;">Usuario</h2>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="text-align: center">
                        <asp:Label ID="LblUsuario" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="#00CCFF" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="vertical-align: top; height: 450px; text-align: left;">
                        <asp:Panel ID="pnlMenu" runat="server" Height="450px" ScrollBars="Vertical">
                            <asp:TreeView ID="trvmenu" runat="server" OnSelectedNodeChanged="trvmenu_SelectedNodeChanged" ImageSet="Arrows" Height="100%">
                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
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
