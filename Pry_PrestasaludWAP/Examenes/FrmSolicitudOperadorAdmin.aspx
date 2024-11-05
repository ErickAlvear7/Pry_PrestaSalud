<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSolicitudOperadorAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmSolicitudOperadorAdmin" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <asp:GridView ID="GrdvDatos" runat="server" Width="100%"
                                AutoGenerateColumns="False" DataKeyNames="CodigoEXSO,CodigoPERS,EstadoCodigo,CodigoTITU,Ext,CodigoPROD"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                ShowHeaderWhenEmpty="True" EmptyDataText="No existen datos para mostrar" OnRowDataBound="GrdvDatos_RowDataBound">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="FecSolicitud" HeaderText="Fecha_Solicitud" />
                                    <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario_Solicita" />
                                    <asp:BoundField DataField="NumDocumento" HeaderText="No.Documento" />
                                    <asp:BoundField DataField="Cliente" HeaderText="Paciente" />
                                    <asp:BoundField DataField="Proceso" HeaderText="Proceso" />
                                    <asp:TemplateField HeaderText="Descargar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDescargar" runat="server" Height="20px" ImageUrl="~/Botones/downloadcolor.png" OnClick="ImgDescargar_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Agendar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgAgendar" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="ImgAgendar_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Font-Size="X-Small" />
                            </asp:GridView>
                            <script>
                                $(document).ready(function () {
                                    $('#GrdvDatos').dataTable();
                                });
                            </script>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
