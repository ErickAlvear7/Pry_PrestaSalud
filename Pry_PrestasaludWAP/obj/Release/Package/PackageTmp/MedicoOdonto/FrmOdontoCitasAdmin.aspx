<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmOdontoCitasAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.MedicoOdonto.FrmOdontoCitasAdmin" %>

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
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updTimer" runat="server">
                <ContentTemplate>
                    <asp:Timer ID="tmrdat" runat="server" OnTick="tmrdat_Tick" Interval="300000">
                    </asp:Timer>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-body">

                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="table table-condensed table-bordered table-hover table-responsive"
                                        ShowHeaderWhenEmpty="True" DataKeyNames="CodigoCita,TituCodigo,BeneCodigo,FechaCodigo" OnRowDataBound="grdvDatos_RowDataBound">
                                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <Columns>
                                            <asp:BoundField DataField="CodigoGenerado" HeaderText="Código" />
                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                            <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                            <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                                            <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                            <asp:BoundField DataField="FechaCita" HeaderText="Fecha" />
                                            <asp:BoundField DataField="HoraCita" HeaderText="Hora" />
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgEstado" runat="server" Height="20px" ImageUrl="~/Botones/relojoff.png" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Atención">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/muelaenable.png" OnClick="btnselecc_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ausente">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgAusente" runat="server" Height="20px" ImageUrl="~/Botones/ausente.png" OnClick="imgAusente_Click" OnClientClick="if ( !confirm('Esta Seguro de registrar Ausente?')) return false;" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
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
