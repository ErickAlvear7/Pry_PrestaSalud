<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMenuCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmMenuCitaMedica" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

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
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>                
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updMenu" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPrestadoras" runat="server" Height="650px" ScrollBars="Vertical">
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 100%">
                                        <asp:TreeView ID="trvPrestadoras" runat="server" ImageSet="Arrows" OnTreeNodePopulate="trvPrestadoras_TreeNodePopulate" OnSelectedNodeChanged="trvPrestadoras_SelectedNodeChanged">
                                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                            <Nodes>
                                                <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Cliente/Productos" Value="Prestardoras/Clínicas"></asp:TreeNode>
                                            </Nodes>
                                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                            <ParentNodeStyle Font-Bold="False" />
                                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                        </asp:TreeView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>                        
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
