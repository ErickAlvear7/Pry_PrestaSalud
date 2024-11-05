<%@ Page Language="C#" AutoEventWireup="true" Inherits="Usuarios_FrmNuevoUsuario" CodeBehind="FrmNuevoUsuario.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Nuevo Usuario</title>
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
    <script src="../Bootstrap/js/bootstrap.min.js"></script>
    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaCaduca').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 1,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }
    </script>
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
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Datos Usuario</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
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
                                        <h5>Nombres:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNombres" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>
                                        Apellidos:</td>
                                    <td>
                                        <asp:TextBox ID="txtApellidos" runat="server" Width="100%" CssClass="form-control upperCase" MaxLength="80" TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Login Usuario:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUser" runat="server" Width="100%" MaxLength="16" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>
                                        Password:</td>
                                    <td>
                                        <asp:TextBox ID="txtPassword" runat="server" Width="100%" MaxLength="50" TextMode="Password" CssClass="form-control" TabIndex="4"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Elija Departamento:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDepartamento" runat="server" Width="100%" CssClass="form-control" TabIndex="5">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <h5>
                                        Perfil:</td>
                                    <td>
                                        <asp:DropDownList ID="ddlPerfil" runat="server" Width="100%" CssClass="form-control" TabIndex="6">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Password Caduca:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCaduca" runat="server" AutoPostBack="True" OnCheckedChanged="chkCaduca_CheckedChanged" Text="No" CssClass="form-control" TabIndex="7" />
                                    </td>
                                    <td>
                                        <h5 runat="server" id="Label10" visible="false">Fecha Caduca:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaCaduca" runat="server" Width="100%" CssClass="form-control" Visible="False" TabIndex="8"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Cambiar Password:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkCambiar" runat="server" AutoPostBack="True" OnCheckedChanged="chkCambiar_CheckedChanged" Text="No" CssClass="form-control" TabIndex="9" />
                                    </td>
                                    <td>
                                        <h5>Permisos Especiales:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkPermisos" runat="server" AutoPostBack="True" CssClass="form-control" OnCheckedChanged="chkPermisos_CheckedChanged" Text="No" TabIndex="10" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Email:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control lowCase" MaxLength="80" TabIndex="11" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5 runat="server" id="Label7" visible="false">Estado:</h5>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" OnCheckedChanged="chkEstado_CheckedChanged" Visible="False" Text="Activo" Checked="True" CssClass="form-control" TabIndex="12" />
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <h5>Logo Bienvenida:</h5>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLogo" runat="server" CssClass="form-control" MaxLength="150" TabIndex="13" Width="100%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <h5>Prestadora:</h5>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCliente" runat="server" CssClass="form-control" TabIndex="14" Width="100%" Visible="False">
                                        </asp:DropDownList>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">Usuario Gerencial</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 40%"></td>
                                        <td style="width: 10%"></td>
                                        <td style="width: 40%"></td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="PnlClientes" runat="server" GroupingText="Clientes" Height="300px" ScrollBars="Auto" TabIndex="9">
                                                <asp:GridView ID="GrdvClientes" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoCLUS,CodigoCAMP,Selecc" ShowHeaderWhenEmpty="True" TabIndex="22" Width="100%" OnRowDataBound="GrdvClientes_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                                        <asp:TemplateField HeaderText="Chk">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkCliente" runat="server" AutoPostBack="True" OnCheckedChanged="ChkCliente_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Selecc">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/seleccgris.png" OnClick="ImgSelecc_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="X-Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:Panel ID="PnlProductos" runat="server" GroupingText="Productos" Height="300px" ScrollBars="Auto" TabIndex="9">
                                                <asp:GridView ID="GrdvProductos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoPRCL,CodigoPROD,Selecc" ShowHeaderWhenEmpty="True" TabIndex="22" Width="100%" OnRowDataBound="GrdvProductos_RowDataBound">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Producto" HeaderText="Productos" />
                                                        <asp:TemplateField HeaderText="Selecc">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="ChkProducto" runat="server" AutoPostBack="True" OnCheckedChanged="ChkProducto_CheckedChanged" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle Font-Size="X-Small" />
                                                    <HeaderStyle Font-Size="X-Small" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <asp:UpdatePanel ID="updOpciones" runat="server">
                <ContentTemplate>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: right; width: 50%">
                                <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" OnClick="btnGrabar_Click" CssClass="button" TabIndex="15" />
                            </td>
                            <td style="width: 10%"></td>
                            <td style="text-align: left; width: 50%">
                                <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" OnClick="btnSalir_Click" CssClass="button" TabIndex="16" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
