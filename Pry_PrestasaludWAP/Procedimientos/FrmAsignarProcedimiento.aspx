<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAsignarProcedimiento.aspx.cs" Inherits="Pry_PrestasaludWAP.Procedimientos.FrmAsignarProcedimiento" %>

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
    <style>
        legend {
            color: darkblue;
            font-size: 14px;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        function ValidarDecimales() {
            var numero = document.getElementById("<%=txtCostoRed.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=txtCostoRed.ClientID%>").value = "0.00";
                return false;
            }
        }
        function ValidarDecimales_1() {
            var numero = document.getElementById("<%=txtCostoReal.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=txtCostoReal.ClientID%>").value = "00.0";
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
            <div class="panel-body">
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:UpdatePanel ID="updError" runat="server">
                    <ContentTemplate>
                        <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                            <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="panel-body">
                    <asp:UpdatePanel ID="updCabecera" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table style="width: 100%" class="table table-bordered table-responsive">
                                <tr>
                                    <td style="width: 20%">
                                        <div style="display: inline-block;">
                                            <asp:TreeView ID="trvPrestadoras" runat="server" ImageSet="Arrows" OnTreeNodePopulate="trvPrestadoras_TreeNodePopulate" OnSelectedNodeChanged="trvPrestadoras_SelectedNodeChanged" TabIndex="1">
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
                                        <table style="width: 100%" class="table table-bordered table-responsive">
                                            <tr>
                                                <td style="width: 20%">
                                                    <h5>Prestadora:</h5>
                                                </td>
                                                <td style="width: 80%">
                                                    <asp:TextBox ID="txtPrestadora" runat="server" CssClass="form-control upperCase" MaxLength="50" ReadOnly="True" Width="100%" TabIndex="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <h5>Procedimiento:</h5>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlProcedimientos" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlProcedimientos_SelectedIndexChanged" Width="100%" TabIndex="3">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
<%--                                            <tr>
                                                <td>
                                                    <h5 runat="server" id="lblProcedimiento" visible="false">Procedimiento:<span style="color: red">*</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtProcedimiento" runat="server" CssClass="form-control upperCase" MaxLength="250" Width="100%" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>--%>
<%--                                            <tr>
                                                <td>
                                                    <h5 runat="server" id="lbldescripcion" visible="false">Descripción:</h5>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescripcion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" MaxLength="500" Width="100%" Height="50px" TextMode="MultiLine" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>--%>
                                        </table>
                                        <table style="width: 100%" class="table table-bordered table-responsive">
                                            <tr>
                                                <td style="width:20%">
                                                    <h5>Costo Red:<span style="color: red">*</h5>
                                                </td>
                                                <td style="width:30%">
                                                    <asp:TextBox ID="txtCostoRed" runat="server" CssClass="form-control upperCase" MaxLength="6" Width="100%" TabIndex="4">0.00</asp:TextBox>
                                                </td>
                                                <td style="width:20%">
                                                    <h5>PVP:</h5>
                                                </td>
                                                <td style="width:30%">
                                                    <asp:TextBox ID="txtCostoReal" runat="server" CssClass="form-control upperCase" MaxLength="6" Width="100%" ReadOnly="True" TabIndex="5">0.00</asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="6" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="7" />
                                                </td>
                                                <td style="text-align: center">
                                                    <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="8" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:Panel ID="pnlProcedimientos" runat="server" GroupingText="Procedimientos Asignados" Height="250px" ScrollBars="Vertical">
                                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoPrestadora,CodigoProcedimiento,CodigoAplica,Aplica,CostoReal,Estado" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="9">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Procedimiento" HeaderText="Procedimiento" />
                                                                <asp:BoundField DataField="CostoRed" HeaderText="Costo" >
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="CostoReal" HeaderText="Pvp" >
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Aplica" HeaderText="Aplica a" >
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Estado">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstado_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Etiqueta">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Editar">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imgEditar" runat="server" Height="20px" ImageUrl="~/Botones/seleccionar.png" OnClick="imgEditar_Click" />
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
                                                <td colspan="4" style="text-align: center">
                                                    <asp:Button ID="btnGrabar" runat="server" CssClass="button" OnClick="btnGrabar_Click" Text="Grabar" Width="120px" TabIndex="10" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="updOpciones" runat="server">
                        <ContentTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 100%"></td>
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
    </script>
</body>
</html>
