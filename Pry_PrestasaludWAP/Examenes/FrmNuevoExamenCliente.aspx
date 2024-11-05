<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoExamenCliente.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmNuevoExamenCliente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../Style/chosen.css" />

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
    <script type="text/javascript">
        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtMonto.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtMonto.ClientID%>").value = "0.00";
                return false;
            }
        }
    </script>
    <script>
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
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <div class="panel-body">
                <asp:UpdatePanel ID="UpdError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-info">
                    <asp:UpdateProgress ID="UpdProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                    <asp:UpdatePanel ID="UpdCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 5%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 10%"></td>
                                    <td style="width: 20%"></td>
                                    <td style="width: 40%"></td>
                                    <td style="width: 5%"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Producto:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="DdlProducto" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%" OnSelectedIndexChanged="DdlProducto_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Grupo Examen:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtGrupoExamen" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="3" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Observación:</h5>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="4" Width="100%"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5 runat="server" id="LblEstado" visible="false">Estado:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="ChkEstadoGrupo" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" TabIndex="5" Text="Activo" Visible="False" OnCheckedChanged="ChkEstadoGrupo_CheckedChanged" />
                                    </td>
                                    <td></td>
                                    <td>
                                        <asp:TextBox ID="TxtMonto" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" TabIndex="2" Visible="False" Width="100%">0.00</asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" TabIndex="6" OnClick="ImgAgregar_Click" />
                                    </td>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ImgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" TabIndex="7" OnClick="ImgModificar_Click" />
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrGrupoExamen" visible="false">
                                    <td></td>
                                    <td colspan="4">
                                        <asp:Panel ID="PnlGrupoExamenes" runat="server" GroupingText="Grupo Exámenes" Height="250px" ScrollBars="Vertical">
                                            <asp:GridView ID="GrdvGrupoExamen" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,ConExamen,ConVariables,Estado" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="8" OnRowDataBound="GrdvGrupoExamen_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                <Columns>
                                                    <asp:BoundField DataField="GrupoExamen" HeaderText="Grupo_Examen" />
                                                    <asp:TemplateField HeaderText="Variables">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgVariables" runat="server" Height="15px" ImageUrl="~/Botones/variablesgris.png" OnClick="ImgVariables_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Exámenes">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgExamenes" runat="server" Height="15px" ImageUrl="~/Botones/notepadgray.png" OnClick="ImgExamenes_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Selecc">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgSeleccGrupo" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccGrupo_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Estado">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Eliminar">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgEliminar" runat="server" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminar_Click" Height="15px" OnClientClick="return asegurar();" />
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
                                <tr>
                                    <td colspan="6" style="text-align: center"></td>
                                </tr>
                                <tr runat="server" id="TrVariables" visible="false">
                                    <td></td>
                                    <td colspan="4">
                                        <asp:Panel ID="PnlVariables" runat="server" Height="350px" GroupingText="Configuración Variables">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 10%"></td>
                                                    <td style="width: 25%"></td>
                                                    <td style="width: 10%"></td>
                                                    <td style="width: 25%"></td>
                                                    <td style="width: 10%"></td>
                                                    <td style="width: 20%"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Campo:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlCampos" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlCampos_SelectedIndexChanged" TabIndex="9">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <h5>Operador:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="DdlOperador" runat="server" AutoPostBack="True" CssClass="form-control" Width="100%" OnSelectedIndexChanged="DdlOperador_SelectedIndexChanged" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <h5>Valor:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtValor" runat="server" CssClass="form-control" TabIndex="11" Width="100%"></asp:TextBox>
                                                        <asp:PlaceHolder ID="PlaceTxt" runat="server"></asp:PlaceHolder>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Panel ID="Panel7" runat="server" Height="20px"></asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td></td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgAddCampo" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddCampo_Click" TabIndex="12" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgModCampo" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModCampo_Click" TabIndex="13" />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Panel ID="Panel5" runat="server" Height="20px"></asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <asp:Panel ID="PnlVariablesAdd" runat="server" Height="180px" ScrollBars="Vertical">
                                                            <asp:GridView ID="GrdvVariables" runat="server" AutoGenerateColumns="False"
                                                                CssClass="table table-condensed table-bordered table-hover table-responsive"
                                                                DataKeyNames="Codigo,Estado" ForeColor="#333333" OnRowDataBound="GrdvVariables_RowDataBound"
                                                                PageSize="5" TabIndex="14" Width="100%">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Campo" HeaderText="Campo" />
                                                                    <asp:BoundField DataField="Operador" HeaderText="Operador" />
                                                                    <asp:BoundField DataField="Valor" HeaderText="Valor" />
                                                                    <asp:TemplateField HeaderText="Estado">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkEstadoCampo" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstadoCampo_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Selecc">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgSeleccCampo" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccCampo_Click" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgDelCampo" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelCampo_Click" OnClientClick="return asegurar();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <HeaderStyle Font-Size="Small" />
                                                                <RowStyle Font-Size="X-Small" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <asp:Panel ID="Panel6" runat="server" Height="20px">
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr runat="server" id="TrExamenes" visible="false">
                                    <td></td>
                                    <td colspan="4">
                                        <asp:Panel ID="PnlExamenes" runat="server" GroupingText="Examenes" Height="480px">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 20%"></td>
                                                    <td style="width: 30%"></td>
                                                    <td style="width: 20%"></td>
                                                    <td style="width: 30%"></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Examenes:</h5>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:DropDownList ID="DdlExamen" runat="server" AutoPostBack="True" class="chzn-select" OnSelectedIndexChanged="DdlExamen_SelectedIndexChanged" TabIndex="15" Width="100%">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Categoria:</h5>
                                                    </td>
                                                    <td colspan="2">
                                                        <asp:TextBox ID="TxtCategoria" runat="server" CssClass="form-control" ReadOnly="True" TabIndex="16" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <h5>Costo Examen:</h5>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="TxtValorExamen" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" TabIndex="17" Width="100%">0.00</asp:TextBox>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="Panel3" runat="server" Height="20px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td style="text-align: center">
                                                        <asp:ImageButton ID="ImgAddExamen" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="ImgAddExamen_Click" TabIndex="18" />
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ImgModExamen" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="ImgModExamen_Click" TabIndex="19" />
                                                    </td>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="Panel4" runat="server" Height="20px">
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <asp:Panel ID="PnlExamenesAgregados" runat="server" Height="230px" ScrollBars="Vertical">
                                                            <asp:GridView ID="GrdvExamenes" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoEXSE,Estado" OnRowDataBound="GrdvExamenes_RowDataBound" ShowHeaderWhenEmpty="True" TabIndex="20" Width="100%">
                                                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                                    <asp:BoundField DataField="Examen" HeaderText="Examen" />
                                                                    <asp:BoundField DataField="Costo" HeaderText="Costo">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Estado">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="ChkEstExamen" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstExamen_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Selecc">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgSeleccExa" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccExa_Click" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Eliminar">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ImgDelExamen" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelExamen_Click" OnClientClick="return asegurar();" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle Font-Size="X-Small" />
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%"></td>
                                <td style="width: 80%"></td>
                            </tr>
                        </table>
                    </div>
                </div>
                <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
                <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" OnClick="BtnGrabar_Click" Text="Grabar" Width="120px" TabIndex="21" />
                                    </td>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" Text="Salir" Width="120px" TabIndex="22" OnClick="BtnSalir_Click" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>
    <script>
        function Close() {
            window.top.location.reload();
        }
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        function endRequestHandler() {
            $(".chzn-select").chosen({ width: "95%" });
            $(".chzn-container").css({ "width": "95%" });
            $(".chzn-drop").css({ "width": "95%" });
        }
    </script>
</body>
</html>
