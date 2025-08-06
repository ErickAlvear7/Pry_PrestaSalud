<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDetallePrestaEspe.aspx.cs" Inherits="Pry_PrestasaludWAP.Prestadora.FrmDetallePrestaEspe" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
                <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>--%>
            </div>
            <div class="panel-body">
<%--                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                        <asp:Panel ID="pnlDetalle" runat="server" Height="100%">
                            <table style="width: 100%">
                                <tr>
                                    <td></td>
                                </tr>
                            </table>
                        </asp:Panel>
<%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>

            </div>
        </div>
    </form>
</body>
</html>
