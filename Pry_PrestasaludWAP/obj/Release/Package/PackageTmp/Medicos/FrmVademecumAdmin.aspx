<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmVademecumAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Medicos.FrmVademecumAdmin" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script>
        function asegurar() {
            rc = confirm("¿Seguro que desea Resetear?");
            return rc;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
<%--                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <button id="btnNuevo" runat="server" type="submit" class="btn btn-primary"
                                        onserverclick="btnNuevo_Click">
                                        <span aria-hidden="true"
                                            class="glyphicon glyphicon-plus"></span>
                                    </button>
                                </td>
                            </tr>
                        </table>--%>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%"
                                        AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" EmptyDataText="No existen usuario creados">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                            <asp:BoundField DataField="Presentacion" HeaderText="Presentacion" />
                                            <%--<asp:BoundField DataField="Especificacion" HeaderText="Especificacion" />--%>
                                           <%-- <asp:BoundField DataField="Medicamento" HeaderText="Medicamento" />--%>
                                            <asp:BoundField DataField="Clase" HeaderText="Clase" />
                                            <asp:BoundField DataField="Subclase" HeaderText="Subclase" />
                                        </Columns>
                                        <RowStyle Font-Size="X-Small" />
                                    </asp:GridView>
                                    <script>
                                        $(document).ready(function () {
                                            $('#grdvDatos').dataTable();
                                        });
                                    </script>
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
