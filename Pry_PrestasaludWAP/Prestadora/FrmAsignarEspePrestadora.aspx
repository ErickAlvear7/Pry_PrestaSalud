<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAsignarEspePrestadora.aspx.cs" Inherits="Pry_PrestasaludWAP.Prestadora.FrmAsignarEspePrestadora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
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
        function ValidarDecimales() {
            var numero = document.getElementById("<%=txtPvp.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=txtPvp.ClientID%>").value = "";
                return false;
            }
        }
        function ValidarDecimales() {
            var numero = document.getElementById("<%=txtCosto.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=txtCosto.ClientID%>").value = "";
                return false;
            }
        }
        function CloseFrame() {
            window.top.location.reload();
        }
    </script>  
</head>
<body>
    <form id="form1" runat="server">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
           <%-- <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="updOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Espere..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>--%>
            <div class="panel-body">
                <asp:UpdatePanel ID="updCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 10%"></td>
                                <td style="width: 35%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Especialidad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="1" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" Text="Activo" Visible="False" TabIndex="2" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><h5>PVP:</h5></td>
                                <td>
                                    <asp:TextBox ID="txtPvp" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="3" Width="100%">0.00</asp:TextBox>
                                </td>
                                <td><h5>Costo Red:</h5></td>
                                <td>
                                    <asp:TextBox ID="txtCosto" runat="server" CssClass="form-control alinearDerecha" MaxLength="10" TabIndex="4" Width="100%">0.00</asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="Panel1" runat="server" Height="30px">
                                    </asp:Panel>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="5" />
                                </td>
                                <td style="text-align: center">
                                    <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="6" />
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="4">
                                    <asp:Panel ID="pnlEspecialidades" runat="server" GroupingText="Especialidades Asignadas" Height="250px" ScrollBars="Vertical">
                                        <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="Codigo,CodigoEspe" ShowHeaderWhenEmpty="True" Width="100%" OnRowDataBound="grdvDatos_RowDataBound" TabIndex="7">
                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <Columns>
                                                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                                                <asp:BoundField DataField="Pvp" HeaderText="PVP" >
                                                <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Costo" HeaderText="Costo Red" >
                                                <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Activo">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkEstadoDet" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstadoDet_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Eliminar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgEliminar" runat="server" Height="15px" ImageUrl="~/Botones/eliminar.png" OnClick="imgEliminar_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Selecc">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgSeleec" runat="server" Height="15px" ImageUrl="~/Botones/selecc.png" OnClick="imgSeleec_Click" />
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
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="panel panel-default">
                <asp:UpdatePanel ID="updOpciones" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr>
                                <td style="text-align: right; width: 45%">
                                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="8" />
                                </td>
                                <td style="width:10%"></td>
                                <td style="text-align: left; width: 45%">
                                    <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="8" />
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
