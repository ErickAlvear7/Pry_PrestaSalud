
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevaCampaign.aspx.cs" Inherits="Pry_PrestasaludWAP.Clientes.FrmNuevaCampaign" %>

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

        function ValidarDecimales() {
            var numero = document.getElementById("<%=txtCosto.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=txtCosto.ClientID%>").value = "";
                return false;
            }
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
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="panel-body">
                <div id="acordionParametro">
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">CLIENTE</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 10%">
                                            <h5>Cliente:<span style="color: red">*</span></h5>
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="txtCliente" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="250" TabIndex="1"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%">
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="2" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Cabecera:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCabecera" runat="server" CssClass="form-control" MaxLength="250" TabIndex="3" Width="100%">~/Images/</asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Pie:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPie" runat="server" CssClass="form-control" MaxLength="250" TabIndex="4" Width="100%">~/Images/</asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 5%"></td>
		                                <td style="width: 10%">
			                                <h5>Tipo:</h5>
		                                </td>
		                                 <td style="width: 35%">
		                                   <asp:DropDownList ID="ddlTipoCli" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
			                                   <asp:ListItem Value=""><--Seleccione Tipo--></asp:ListItem>
			                                   <asp:ListItem Value="ONLINE">ONLINE</asp:ListItem>
			                                   <asp:ListItem Value="OFFLINE">OFFLINE</asp:ListItem>
			                                </asp:DropDownList>
		                                </td>
		                                <td></td>
	                               </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="Label3" visible="false">Estado:</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstadoCliente" runat="server" AutoPostBack="True" Text="Activo" Checked="True" OnCheckedChanged="chkEstadoCliente_CheckedChanged" Visible="False" CssClass="form-control" TabIndex="5" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">PRODUCTOS</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 15%"></td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Producto:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProducto" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="250" TabIndex="6"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Descripción:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDetalle" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="250" Width="100%" TextMode="MultiLine" TabIndex="7"></asp:TextBox></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Costo:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCosto" runat="server" Width="100%" MaxLength="10" CssClass="form-control" TabIndex="8"></asp:TextBox>
                                        </td>
                                        <td>
                                            <h5>Grupo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrupoProducto" runat="server" CssClass="form-control" TabIndex="9" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlGrupoProducto_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Asistencia Mes:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAsisMensual" runat="server" CssClass="form-control alinearDerecha" MaxLength="4" TabIndex="10" Width="100%">1</asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtAsisMensual_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAsisMensual">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <h5>Asistencia Anual:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAsisAnual" runat="server" CssClass="form-control alinearDerecha" MaxLength="4" TabIndex="11" Width="100%">1</asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtAsisAnual_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtAsisAnual">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td></td>
                                    </tr>
                                     <tr>
                                        <td style="width: 5%"></td>
		                                <td style="width: 10%">
			                                <h5>Tipo:</h5>
		                                </td>
		                                 <td style="width: 35%">
		                                   <asp:DropDownList ID="ddlTipoPro" runat="server" CssClass="form-control" TabIndex="2" Width="100%">
			                                   <asp:ListItem Value=""><--Seleccione Tipo--></asp:ListItem>
			                                   <asp:ListItem Value="ONLINE">ONLINE</asp:ListItem>
			                                   <asp:ListItem Value="OFFLINE">OFFLINE</asp:ListItem>
			                                </asp:DropDownList>
		                                </td>
		                                <td></td>
	                               </tr>

                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkFechaCobertura" runat="server" AutoPostBack="True" Font-Size="Smaller" TabIndex="12" Text="Fec.Cobertura" OnCheckedChanged="chkFechaCobertura_CheckedChanged" />
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:CheckBox ID="chkFechaSistema" runat="server" AutoPostBack="True" Font-Size="Smaller" TabIndex="13" Text="Fec. Sistema" OnCheckedChanged="chkFechaSistema_CheckedChanged" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="Label6" visible="false">Estado</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstadoProducto" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstadoProducto_CheckedChanged" Text="Activo" Checked="True" Visible="False" CssClass="form-control" TabIndex="14" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td style="text-align: right">&nbsp;</td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="15" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="16" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgCancelar" runat="server" CausesValidation="False" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="17" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Height="230px" GroupingText="Productos">
                                                <asp:GridView ID="grdvDatos" runat="server" Width="100%" ForeColor="#333333"
                                                    CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                    DataKeyNames="Codigo,Gerencial"
                                                    AutoGenerateColumns="False" PageSize="5" TabIndex="18" OnRowDataBound="grdvDatos_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                                        <asp:BoundField DataField="Producto" HeaderText="Producto" />
                                                        <asp:BoundField DataField="Costo" HeaderText="Costo" />
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                                        <asp:TemplateField HeaderText="Gerencial">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkGerencial" runat="server" AutoPostBack="True" OnCheckedChanged="chkGerencial_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnselecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccionar.png" OnClick="btnselecc_Click" />
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
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="19" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="20" />
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
