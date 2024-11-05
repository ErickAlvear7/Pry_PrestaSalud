<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmNuevoTitular.aspx.cs" Inherits="Pry_PrestasaludWAP.Titulares.FrmNuevoTitular" %>

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
    <link href="../css/jquery.ui.accordion.css" rel="stylesheet" />
    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>

    <script>
        $(function () {
            $("#acordionParametro").accordion();
        });
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


    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#txtFechaNacimiento').datepicker(
                    {
                        //showOn: "both",
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });

                $('#txtFechaNacBen').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });

                $('#TxtFechaIniCobertura').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });

                $('#TxtFechaFinCobertura').datepicker(
                    {
                        inline: true,
                        dateFormat: "mm/dd/yy",
                        monthNames: ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"],
                        monthNamesShort: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                        dayNamesMin: ["Do", "Lu", "Ma", "Mi", "Ju", "Vi", "Sa"],
                        numberOfMonths: 2,
                        showButtonPanel: true,
                        changeMonth: true,
                        changeYear: true,
                        yearRange: "-100:+5"
                    });
            });
        }

        function Calcular_Edad() {
            var today = new Date();
            var birthDate = new Date(document.getElementById("<%=txtFechaNacimiento.ClientID%>").value);
            var age = today.getFullYear() - birthDate.getFullYear();
            var m = today.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }
            document.getElementById("<%=hidEdad.ClientID%>").value = "";
            document.getElementById("<%=txtEdad.ClientID%>").value = age;
            document.getElementById("<%=hidEdad.ClientID%>").value = age;
        }

        function Calcular_EdadB() {
            var today = new Date();
            var birthDate = new Date(document.getElementById("<%=txtFechaNacBen.ClientID%>").value);
            var age = today.getFullYear() - birthDate.getFullYear();
            var m = today.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }
            document.getElementById("<%=hidEdadB.ClientID%>").value = "";
            document.getElementById("<%=txtEdadBen.ClientID%>").value = age;
            document.getElementById("<%=hidEdadB.ClientID%>").value = age;
        }

        function Validar_Cedula() {
            var ddltipodoc = document.getElementById("<%=ddlTipoDocumento.ClientID%>").value;
            var cedula = document.getElementById("<%=txtNumeroDocumento.ClientID%>").value;
            var digito_region = cedula.substring(0, 2);
            if (ddltipodoc == "C") {
                arreglo = cedula.split("");
                num = cedula.length;
                if (digito_region >= 1 && digito_region <= 30) {
                    if (num == 10) {
                        //validar cedula
                        digito = (arreglo[9] * 1);
                        total = 0;
                        for (i = 0; i < (num - 1); i++) {
                            if ((i % 2) != 0) {
                                total = total + (arreglo[i] * 1);
                            } else {
                                mult = arreglo[i] * 2;
                                if (mult > 9) {
                                    total = total + (mult - 9);
                                } else {
                                    total = total + mult;
                                }
                            }
                        }
                        decena = total / 10;
                        decena = Math.floor(decena);
                        decena = (decena + 1) * 10;
                        final = (decena - total);
                        if (final == 10) {
                            final = 0;
                        }
                        if (digito == final) {
                            <%--document.getElementById("<%=txtnombre1.ClientID%>").value = ""--%>
                            return true;
                        } else {
                            alert("Cédula Incorrecta");
                            document.getElementById("<%=txtNumeroDocumento.ClientID%>").value = "";
                            return false;
                        }
                    }
                    else {
                        alert("Cédula Incorrecta");
                        document.getElementById("<%=txtNumeroDocumento.ClientID%>").value = "";
                        document.getElementById("<%=txtNumeroDocumento.ClientID%>").focus;
                    }
                }
                else {
                    document.getElementById("<%=txtNumeroDocumento.ClientID%>").value = "";
                    document.getElementById("<%=txtNumeroDocumento.ClientID%>").focus;
                    alert("Cédula Incorrecta");
                }
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
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="lblerror" runat="server" ForeColor="Red"></asp:Label>
                        <asp:HiddenField ID="hidEdad" runat="server" />
                        <asp:HiddenField ID="hidEdadB" runat="server" />
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
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS TITULAR</h3>
                    <asp:UpdatePanel ID="updCabecera" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%">
                                            <h5>Tipo Documento:</h5>
                                        </td>
                                        <td style="width: 29%">
                                            <asp:DropDownList ID="ddlTipoDocumento" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoDocumento_SelectedIndexChanged" TabIndex="1">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 2%"></td>
                                        <td style="width: 15%;">
                                            <h5>Nro. Documento:</h5>
                                        </td>
                                        <td style="width: 29%">
                                            <asp:TextBox ID="txtNumeroDocumento" runat="server" CssClass="form-control upperCase" MaxLength="20" Width="100%" AutoPostBack="True" OnTextChanged="txtNumeroDocumento_TextChanged" TabIndex="2"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtNumeroDocumento_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtNumeroDocumento">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Primer Nombre:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrimerNombre" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="3"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Segundo Nombre:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSegundoNombre" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="4"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Primer Apellido:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrimerApellido" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="5"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Segundo Apellido:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSegundoApellido" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="6"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Genero:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-control" Width="100%" TabIndex="7">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Estado Civil:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="form-control" Width="100%" TabIndex="8">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Nacimiento:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" Width="100%" TabIndex="9"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Edad:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEdad" runat="server" CssClass="form-control upperCase" MaxLength="2" Width="100%" ReadOnly="True" TabIndex="10"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Provincia:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" TabIndex="11">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Ciudad:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="form-control" Width="100%" TabIndex="12">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Dirección:</h5>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtDireccion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="250" Height="50px" TextMode="MultiLine" TabIndex="13"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Teléfonos:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFonoCasa" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="14"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFonoCasa_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFonoCasa">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="txtFonoOficina" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="15"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtFonoOficina_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFonoOficina">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="16"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtCelular_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtCelular">
                                            </asp:FilteredTextBoxExtender>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td style="text-align: center">
                                            <h5>Casa</h5>
                                        </td>
                                        <td></td>
                                        <td style="text-align: center">
                                            <h5>Oficina</h5>
                                        </td>
                                        <td style="text-align: center">
                                            <h5>Celular</h5>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Email:</h5>
                                        </td>
                                        <td colspan="4">
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="17"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5 runat="server" id="Label3" visible="false">Estado:</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstado" runat="server" AutoPostBack="True" Text="Activo" Checked="True" Visible="False" CssClass="form-control" OnCheckedChanged="chkEstado_CheckedChanged" TabIndex="18" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Inicio Cobertura:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFechaIniCobertura" runat="server" CssClass="form-control" TabIndex="19" Width="100%"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td>
                                            <h5>Fin Cobertura:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="TxtFechaFinCobertura" runat="server" CssClass="form-control" TabIndex="20" Width="100%"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS BENEFICIARIO</h3>
                    <asp:UpdatePanel ID="updDetalle" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 5%"></td>
                                        <td style="width: 15%">
                                            <h5>Tipo Documento:</h5>
                                        </td>
                                        <td style="width: 29%" colspan="2">
                                            <asp:DropDownList ID="ddlTipoDocumentoBen" runat="server" CssClass="form-control" Width="100%" TabIndex="21">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 2%"></td>
                                        <td style="width: 15%;" colspan="2">
                                            <h5>Nro. Documento:</h5>
                                        </td>
                                        <td style="width: 29%">
                                            <asp:TextBox ID="txtNumeroDocumentoBen" runat="server" CssClass="form-control upperCase" MaxLength="20" Width="100%" TabIndex="22"></asp:TextBox>
                                        </td>
                                        <td style="width: 5%"></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Primer Nombre:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPrimerNombreB" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="23"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5>Segundo Nombre:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSegundoNombreB" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="24"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Primer Apellido:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtPrimerApellidoB" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="25"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5>Segundo Apellido:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSegundoApellidoB" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="26"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Genero:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlGeneroBen" runat="server" CssClass="form-control" Width="100%" TabIndex="27">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5>Estado Civil:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEstadoCivilBen" runat="server" CssClass="form-control" Width="100%" TabIndex="28">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Fecha Nacimiento:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFechaNacBen" runat="server" CssClass="form-control" Width="100%" TabIndex="29"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5>Edad:</h5>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEdadBen" runat="server" CssClass="form-control upperCase" MaxLength="2" Width="100%" ReadOnly="True" TabIndex="30"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Provincia:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlProvinciaBen" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="ddlProvinciaBen_SelectedIndexChanged" TabIndex="31">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5>Ciudad:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCiudadBen" runat="server" CssClass="form-control" Width="100%" TabIndex="32">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Dirección:</h5>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtDireccionBen" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="250" Height="50px" TextMode="MultiLine" TabIndex="33"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Teléfonos:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFonoCasaBen" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="34"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtFonoOficBen" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="35"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCelularBen" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="36"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td style="text-align: center" colspan="2">
                                            <h5>Casa</h5>
                                        </td>
                                        <td></td>
                                        <td style="text-align: center" colspan="2">
                                            <h5>Oficina</h5>
                                        </td>
                                        <td style="text-align: center">
                                            <h5>Celular</h5>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Email:</h5>
                                        </td>
                                        <td colspan="6">
                                            <asp:TextBox ID="txtEmailBen" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="37"></asp:TextBox>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Parentesco:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="ddlParentesco" runat="server" CssClass="form-control" Width="100%" TabIndex="38">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td colspan="2">
                                            <h5 runat="server" id="Label4">Estado:</h5>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkEstadoBen" runat="server" AutoPostBack="True" Checked="True" CssClass="form-control" Text="Activo" OnCheckedChanged="chkEstadoBen_CheckedChanged" TabIndex="39" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="9">
                                            <asp:Panel ID="pnlEspacio" runat="server" Height="10px"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="2" style="text-align: center">
                                            <asp:ImageButton ID="imgAgregar" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" OnClick="imgAgregar_Click" TabIndex="40" />
                                        </td>
                                        <td colspan="3" style="text-align: center">
                                            <asp:ImageButton ID="imgModificar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgModificar_Click" TabIndex="41" />
                                        </td>
                                        <td colspan="2" style="text-align: center">
                                            <asp:ImageButton ID="imgCancelar" runat="server" Enabled="False" Height="20px" ImageUrl="~/Botones/cancelar.jpg" OnClick="imgCancelar_Click" TabIndex="42" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="7">
                                            <asp:Panel ID="pnlBeneficiarios" runat="server" Height="250px" ScrollBars="Vertical" GroupingText="Beneficarios">
                                                <asp:GridView ID="grdvDatos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoBen,TipoDocumentoBen,NumeroDocumentoBen,GeneroBen,EstadoCivilBen,FechaNacimientoBen,ProvinciaBen,CiudadBen,DireccionBen,FonoCasaBen,FonoOficinaBen,CelularBen,EmailBen,ParentescoCod,Estado,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido" ShowHeaderWhenEmpty="True" Width="100%" TabIndex="43">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Beneficiario" HeaderText="Beneficiario" />
                                                        <asp:BoundField DataField="Parentesco" HeaderText="Parentesco">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                                        <asp:TemplateField HeaderText="Editar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgSelecc" runat="server" Height="20px" ImageUrl="~/Botones/modificar.png" OnClick="imgSelecc_Click" />
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
                                        <asp:Button ID="btnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="btnGrabar_Click" TabIndex="44" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="btnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="btnSalir_Click" TabIndex="45" />
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
