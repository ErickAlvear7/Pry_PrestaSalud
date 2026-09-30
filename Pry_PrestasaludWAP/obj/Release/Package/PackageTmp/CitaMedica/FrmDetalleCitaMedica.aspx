<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmDetalleCitaMedica.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmDetalleCitaMedica" %>

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
            </div>
            <div class="panel-body">
                <asp:Panel ID="pnlDetalle" runat="server" Height="100%">
                    <table style="width: 100%">
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
