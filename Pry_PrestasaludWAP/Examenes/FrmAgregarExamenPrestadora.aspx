<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAgregarExamenPrestadora.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmAgregarExamenPrestadora" %>

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
            var numero = document.getElementById("<%=TxtPvp.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtPvp.ClientID%>").value = "0.00";
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 20%">
                                        <div style="display: inline-block;">
                                            <asp:TreeView ID="TrvPrestadoras" runat="server" ImageSet="Arrows" TabIndex="1" OnSelectedNodeChanged="TrvPrestadoras_SelectedNodeChanged" OnTreeNodePopulate="TrvPrestadoras_TreeNodePopulate">
                                                <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                <Nodes>
                                                    <asp:TreeNode PopulateOnDemand="True" SelectAction="Expand" Text="Prestardoras/Clínicas" Value="Prestardoras/Clínicas"></asp:TreeNode>
                                                </Nodes>
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                                <ParentNodeStyle Font-Bold="False" />
                                                <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                            </asp:TreeView>
                                        </div>
                                    </td>
                                    <td style="width: 80%">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 20%"></td>
                                                <td style="width: 30%"></td>
                                                <td style="width: 20%"></td>
                                                <td style="width: 30%"></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Prestadora:</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TxtPrestadora" runat="server" CssClass="form-control" ReadOnly="True" Width="100%" TabIndex="2" Font-Bold="True" Font-Size="12pt" ForeColor="#0099CC"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Grupo Exámen:</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TxtGrupoExamen" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="3" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Descripción:</h5>
                                                </td>
                                                <td colspan="3">
                                                    <asp:TextBox ID="TxtDescripcion" runat="server" CssClass="form-control upperCase" MaxLength="150" TabIndex="4" Width="100%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" TabIndex="6" OnClick="ImgAgregar_Click" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="ImgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" TabIndex="6" OnClick="ImgModificar_Click" />
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="PnlGrupoExamenes" runat="server" GroupingText="Grupo Exámenes" Height="250px" ScrollBars="Vertical" Visible="False">
                                                        <asp:GridView ID="GrdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,ConExamen,Estado" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="7" OnRowDataBound="GrdvDatos_RowDataBound">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="GrupoExamen" HeaderText="Grupo" />
                                                                <asp:TemplateField HeaderText="Exámenes">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgExamenes" runat="server" Height="15px" ImageUrl="~/Botones/notepadgray.png" OnClick="ImgExamenes_Click" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Estado">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Selecc">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgSeleccGrupo" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="ImgSeleccGrupo_Click" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Eliminar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgEliminar" runat="server" ImageUrl="~/Botones/eliminar.png" OnClick="ImgEliminar_Click" Height="15px" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle Font-Size="X-Small" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="text-align: center"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="PnlExamenes" runat="server" Height="480px" GroupingText="Examenes" Visible="False">
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
                                                                <td colspan="3">
                                                                    <asp:DropDownList ID="DdlExamen" runat="server" class="chzn-select" TabIndex="2" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlExamen_SelectedIndexChanged">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Categoria:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtCategoria" runat="server" CssClass="form-control" ReadOnly="True" TabIndex="2" Width="100%"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <h5>SubCategoria:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtSubCategoria" runat="server" CssClass="form-control" ReadOnly="True" TabIndex="2" Width="100%"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <h5>Valor Examen:</h5>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtValor" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" ReadOnly="True" TabIndex="5" Width="100%">0.00</asp:TextBox>
                                                                </td>
                                                                <td>Pvp Examen:</td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtPvp" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" TabIndex="4" Width="100%">0.00</asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Panel ID="Panel3" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td style="text-align: center">
                                                                    <asp:ImageButton ID="ImgAddExamen" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" TabIndex="6" OnClick="ImgAddExamen_Click" />
                                                                </td>
                                                                <td>
                                                                    <asp:ImageButton ID="ImgModExamen" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" TabIndex="6" OnClick="ImgModExamen_Click" />
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <asp:Panel ID="PnlExamenesAgregados" runat="server" Height="230px" ScrollBars="Vertical">
                                                                        <asp:GridView ID="GrdvExamenes" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" TabIndex="7" Width="100%" OnRowDataBound="GrdvExamenes_RowDataBound" DataKeyNames="Codigo,CodigoEXSE,Estado">
                                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                                            <Columns>
                                                                                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                                                <asp:BoundField DataField="Examen" HeaderText="Examen" />
                                                                                <asp:BoundField DataField="Valor" HeaderText="Valor">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:BoundField DataField="Pvp" HeaderText="PVP">
                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                </asp:BoundField>
                                                                                <asp:TemplateField HeaderText="Estado">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="ChkEstExamen" runat="server" OnCheckedChanged="ChkEstExamen_CheckedChanged" AutoPostBack="True" />
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
                                                                                        <asp:ImageButton ID="ImgDelExamen" runat="server" ImageUrl="~/Botones/eliminar.png" OnClick="ImgDelExamen_Click" Height="15px" />
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
                                            </tr>
                                        </table>
                                    </td>
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
                                        <asp:Button ID="BtnGrabar" runat="server" CssClass="button" OnClick="BtnGrabar_Click" Text="Grabar" Width="120px" TabIndex="8" />
                                    </td>
                                    <td style="width: 50%; text-align: center;">
                                        <asp:Button ID="BtnSalir" runat="server" CssClass="button" Text="Salir" Width="120px" TabIndex="8" OnClick="BtnSalir_Click" />
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
