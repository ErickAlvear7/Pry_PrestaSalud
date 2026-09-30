<%@ Page Language="C#" AutoEventWireup="true" Inherits="Parametros_FrmNuevoParametro" CodeBehind="FrmNuevoParametro.aspx.cs" %>

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

    <script src="../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <link href="../css/jquery.ui.accordion.css" rel="stylesheet" />

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
    <script>
        $(function () {
            $("#acordionParametro").accordion();
        });
        function asegurar() {
            rc = confirm("¿Seguro que desea Eliminar?");
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
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Espere..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">PARAMETROS</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td style="width: 20%">
                                            <h5>Parámetro:</h5>
                                        </td>
                                        <td style="width: 45%">
                                            <asp:TextBox ID="txtParametro" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="1"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%">
                                            &nbsp;</td>
                                        <td style="width: 10%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDescripcion" runat="server" onkeydown = "return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="80" Height="50px" TextMode="MultiLine" TabIndex="2"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="Label3" visible="false">Estado:</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstadoPar" runat="server" AutoPostBack="True" Text="Activo" Checked="True" OnCheckedChanged="chkEstadoPar_CheckedChanged" Visible="False" CssClass="form-control" TabIndex="3" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DETALLE DE PARAMETROS</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 10%"></td>
                                        <td style="width: 20%">
                                            <h5>Detalle:</h5>
                                        </td>
                                        <td style="width: 45%">
                                            <asp:TextBox ID="txtDetalle" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="150" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td style="width: 15%; text-align: center;">
                                            <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="7" />
                                        </td>
                                        <td style="width: 10%">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Valor Parámetro:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtValor" runat="server" Width="100%" MaxLength="80" CssClass="form-control" TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="8" />
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="Label6" visible="false">Estado</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstadoDet" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstadoDet_CheckedChanged" Text="Activo" Checked="True" Visible="False" CssClass="form-control" TabIndex="6" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="imgCancelar" runat="server" CausesValidation="False" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="9" />
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="text-align: center">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="3">
                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="230px" GroupingText="Parámetros">
                                                <asp:GridView ID="grdvDatos" runat="server" Width="100%" ForeColor="#333333"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo,Orden"
                                                    AutoGenerateColumns="False" PageSize="5" OnPageIndexChanging="grdvDatos_PageIndexChanging" TabIndex="10">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Detalle" HeaderText="Detalle" />
                                                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                        <asp:BoundField DataField="Orden" HeaderText="Orden" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:TemplateField HeaderText="Subir Nivel">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSubirNivel" runat="server" Height="15px" ImageUrl="~/Botones/activada_up.png" OnClick="imgSubirNivel_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bajar Nivel">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgBajarNivel" runat="server" Height="15px" ImageUrl="~/Botones/activada_down.png" OnClick="imgBajarNivel_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Selecc">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnselecc" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="btnselecc_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgDel" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="imgDel_Click" OnClientClick="return asegurar();" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="11" />
                                    </td>
                                    <td style="width:10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="12" />
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
