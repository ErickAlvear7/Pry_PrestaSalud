<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmCitaMedicaAdmin.aspx.cs" Inherits="Pry_PrestasaludWAP.CitaMedica.FrmCitaMedicaAdmin" %>

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

    <style type="text/css">
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }
        .overlay {
            position: fixed;
            z-index: 98;
            top: 0px;
            left: 0px;
            right: 0px;
            bottom: 0px;
            background-color: #aaa;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .overlayContent {
            z-index: 99;
            margin: 250px auto;
            width: 80px;
            height: 80px;
        }

        .overlayContent h2 {
            font-size: 18px;
            font-weight: bold;
            color: #000;
        }

        .overlayContent img {
            width: 80px;
            height: 80px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updPrincipal">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <asp:UpdatePanel ID="updPrincipal" runat="server">
                    <ContentTemplate>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 25%">
                                    <h5>Criterio Cédula/Nombres-Apellidos:</h5>
                                </td>
                                <td style="width: 45%">
                                    <asp:TextBox ID="txtCriterio" runat="server" CssClass="form-control upperCase" MaxLength="150" Width="100%" TabIndex="1"></asp:TextBox>
                                </td>
                                <td style="width: 20%; text-align: center;">
                                    <asp:Button ID="btnBuscar" runat="server" CssClass="button" Text="Buscar" OnClick="btnBuscar_Click" TabIndex="2" />
                                    <br /><br />
                                    <asp:Button ID="BtnBuscarWS" runat="server" CssClass="button" Text="Buscar WS"  TabIndex="2" BackColor="#6666FF" OnClick="BtnBuscarWS_Click" Visible="False" />
                                </td>
                                <td style="width: 5%"></td>
                            </tr>
                        </table>
                        <table class="table table-bordered table-condensed table-hover table-responsive">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlDatos" runat="server" Height="450px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="table table-condensed table-bordered table-hover table-responsive"
                                            ShowHeaderWhenEmpty="True" DataKeyNames="Codigo,CodigoProducto,FechaCobertura" TabIndex="3" OnRowDataBound="grdvDatos_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="NumeroDocumento" HeaderText="Identificación" />
                                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                                <asp:BoundField DataField="Titular" HeaderText="Titular" />
                                                <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                <asp:TemplateField HeaderText="Agendar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/citamedica.png" OnClick="btnselecc_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle Font-Size="X-Small" />
                                        </asp:GridView>
                                    </asp:Panel>
                                    <%--                                    <script>
                                        $(document).ready(function () {
                                            $('#grdvDatos').dataTable();
                                        });
                                    </script>--%>
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
