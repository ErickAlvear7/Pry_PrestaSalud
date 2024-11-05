<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAuditarExamen.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmAuditarExamen" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="../Scripts/external/jquery/jquery.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>
    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>

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
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS PACIENTE</h3>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 5%"></td>
                        <td style="width: 15%"></td>
                        <td style="width: 29%"></td>
                        <td style="width: 2%"></td>
                        <td style="width: 15%;"></td>
                        <td style="width: 29%"></td>
                        <td style="width: 5%"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="5">
                            <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                ShowHeaderWhenEmpty="True" TabIndex="1" Width="100%" DataKeyNames="Examen1,Examen2,Examen3,Examen4,Examen5" OnRowDataBound="GrdvDatos_RowDataBound">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="Cedula" HeaderText="Cédula" />
                                    <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                                    <asp:BoundField DataField="FecNaci" HeaderText="Fecha_Nacimiento"></asp:BoundField>
                                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                    <asp:TemplateField HeaderText="Exa.1">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDownload" runat="server" Height="20px" ImageUrl="~/Botones/downloadgris.png" OnClick="ImgDownload_Click" Enabled="False" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exa.2">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDownload1" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/downloadgris.png" OnClick="ImgDownload1_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exa.3">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDownload2" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/downloadgris.png" OnClick="ImgDownload2_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exa.4">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDownload3" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/downloadgris.png" OnClick="ImgDownload3_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exa.5">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgDownload4" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/downloadgris.png" OnClick="ImgDownload4_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Font-Size="X-Small" />
                            </asp:GridView>
                        </td>
                        <td></td>
                    </tr>
                </table>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">EXAMENES REALIZADOS</h3>
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr runat="server" id="TrExamenes">
                                <td>
                                    <asp:Panel ID="PnlExamenes" runat="server" Height="390px" GroupingText="Examenes">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 5%"></td>
                                                <td style="width: 10%"></td>
                                                <td style="width: 30%"></td>
                                                <td style="width: 5%"></td>
                                                <td style="width: 10%"></td>
                                                <td style="width: 10%"></td>
                                                <td style="width: 25%"></td>
                                                <td style="width: 5%"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel4" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="6">
                                                    <asp:Panel ID="PnlExamenesAgregados" runat="server" Height="180px" ScrollBars="Vertical">
                                                        <asp:GridView ID="GrdvExamenes" runat="server" AutoGenerateColumns="False"
                                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" TabIndex="2" Width="100%">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                                <asp:BoundField DataField="Examen" HeaderText="Examen" />
                                                                <asp:BoundField DataField="Adicional" HeaderText="Adicional">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <RowStyle Font-Size="X-Small" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel1" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel2" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <h5>Observación:</h5>
                                                </td>
                                                <td colspan="5">
                                                    <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="3" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel3" runat="server" Height="20px">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                </asp:UpdatePanel>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Auditar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="4" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" OnClick="BtnSalir_Click" TabIndex="5" Text="Salir" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
