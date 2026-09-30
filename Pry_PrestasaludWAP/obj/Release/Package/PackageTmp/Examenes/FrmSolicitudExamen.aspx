<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSolicitudExamen.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmSolicitudExamen" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <%--  <title></title>
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
    <link rel="stylesheet" href="../Style/chosen.css" />--%>
    <title></title>

    <link href="../css/DatePicker/jquery-ui.css" rel="stylesheet" />
    <link href="../css/Estilos.css" rel="stylesheet" />
    <link href="../Scripts/Tables/jquery.DataTable.min.css" rel="stylesheet" />
    <link href="../Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="../Style/chosen.css" />

    <!-- JQUERY: CARGAR UNA SOLA VEZ -->
    <script src="../Scripts/jquery-1.10.2.min.js"></script>

    <!-- JQUERY UI -->
    <script src="../Scripts/jquery-ui.min.js"></script>

    <!-- BOOTSTRAP: SIEMPRE DESPUES DE JQUERY -->
    <script src="../Bootstrap/js/bootstrap.min.js"></script>

    <!-- DATATABLES -->
    <script src="../Scripts/Tables/DataTables.js"></script>
    <script src="../Scripts/Tables/dataTable.bootstrap.min.js"></script>

    <!-- CHOSEN -->
    <script src="../Scripts/chosen.jquery.js"></script>

    <%--    <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
    <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>--%>

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

        .zona-examenes {
            margin-top: 15px;
        }

        .tabla-examen {
            width: 100% !important;
            table-layout: fixed;
        }


            .tabla-examen th,
            .tabla-examen td {
                vertical-align: middle !important;
                white-space: normal !important;
                word-wrap: break-word;
                overflow-wrap: break-word;
            }


        .tabla-examen-scroll {
            width: 100%;
            overflow-x: auto;
        }


        .paginador-examen {
            padding: 8px;
        }


            .paginador-examen table {
                margin: auto;
            }


            .paginador-examen td {
                padding: 3px;
            }


            .paginador-examen a,
            .paginador-examen span {
                display: inline-block;
                padding: 5px 9px;
                margin: 2px;
                border: 1px solid #ddd;
                border-radius: 4px;
                text-decoration: none;
            }

            .paginador-examen span {
                font-weight: bold;
                background-color: #eee;
            }

        .popupCodepFondo {
            background-color: #000;
            opacity: 0.55;
            filter: alpha(opacity=55);
        }



        .popupCodepMover {
            font-size: 11px;
            font-weight: normal;
            opacity: 0.8;
            cursor: move;
        }

        .popupCodepSubtitulo {
            font-size: 14px;
            font-weight: bold;
            color: #337ab7;
            margin-bottom: 15px;
        }


        .popupCodepPie .btn {
            margin-left: 6px;
        }

        @media (max-width: 768px) {

            .popupCodep {
                width: calc(100vw - 20px);
                max-width: calc(100vw - 20px);
                max-height: 88vh;
            }

            .popupCodepCuerpo {
                padding: 15px;
            }

            .popupCodepPie {
                padding: 10px 15px;
            }

                .popupCodepPie .btn {
                    margin-top: 5px;
                }

            .popupCodepMover {
                display: none;
            }
        }

        .popupCodep {
            width: 760px;
            max-width: calc(100vw - 30px);
            background-color: #ffffff;
            border: 1px solid #337ab7;
            border-radius: 5px;
            overflow: hidden;
            box-shadow: 0 6px 20px rgba(0, 0, 0, 0.45);
        }

        .popupCodepCabecera {
            background-color: #337ab7;
            color: #ffffff;
            font-size: 15px;
            font-weight: bold;
            padding: 12px 16px;
            cursor: move;
            display: flex;
            justify-content: space-between;
            align-items: center;
            user-select: none;
        }

        .popupCodepCuerpo {
            padding: 20px;
            background-color: #ffffff;
            max-height: calc(100vh - 230px);
            overflow-y: auto;
            overflow-x: hidden;
        }

        .popupCodepSubtitulo {
            font-size: 14px;
            font-weight: bold;
            color: #337ab7;
            margin-bottom: 15px;
        }

        .popupCodepPie {
            padding: 12px 20px;
            text-align: right;
            background-color: #f5f5f5;
            border-top: 1px solid #dddddd;
        }


            .popupCodepPie .btn {
                margin-left: 6px;
            }


        .popupCodepMover {
            font-size: 11px;
            font-weight: normal;
            opacity: 0.80;
            cursor: move;
        }


        /* FONDO */
        .popupCodepFondo {
            background-color: #000000;
            opacity: 0.55;
            filter: alpha(opacity=55);
        }
    </style>
    <script type="text/javascript">
        function pageLoad(sender, arg) {
            $(document).ready(function () {
                $.datepicker.setDefaults($.datepicker.regional['es']);
                $('#TxtFechaNacimiento').datepicker(
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

                $('#TxtFechaSolicitud').datepicker(
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
            var birthDate = new Date(document.getElementById("<%=TxtFechaNacimiento.ClientID%>").value);
            var age = today.getFullYear() - birthDate.getFullYear();
            var m = today.getMonth() - birthDate.getMonth();
            if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }
            document.getElementById("<%=hidEdad.ClientID%>").value = "0";
            document.getElementById("<%=TxtEdad.ClientID%>").value = age;
            document.getElementById("<%=hidEdad.ClientID%>").value = age;
        }

        function Validar_Cedula() {
            var ddltipodoc = document.getElementById("<%=DdlTipoDocumento.ClientID%>").value;
            var cedula = document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value;
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
                            document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                            return false;
                        }
                    }
                    else {
                        alert("Cédula Incorrecta");
                        document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                        <%--document.getElementById("<%=TxtNumeroDocumento.ClientID%>").focus;--%>
                    }
                }
                else {
                    document.getElementById("<%=TxtNumeroDocumento.ClientID%>").value = "";
                    <%--document.getElementById("<%=TxtNumeroDocumento.ClientID%>").focus;--%>
                    alert("Cédula Incorrecta");
                }
            }
        }

<%--        function ValidarDecimales() {
            var numero = document.getElementById("<%=TxtPvp.ClientID%>").value;
            if (!/^([0-9])*[.]?[0-9]*$/.test(numero)) {
                alert("El valor " + numero + " no es un número válido. Ejemplo 157.68");
                document.getElementById("<%=TxtPvp.ClientID%>").value = "0.00";
                return false;
            }
        }--%>
    </script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="panel panel-primary">
            <div class="panel-heading">
                <asp:Label ID="Lbltitulo" runat="server"></asp:Label>
            </div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <asp:UpdatePanel ID="updError" runat="server">
                <ContentTemplate>
                    <div style="background-color: beige; text-align: left; width: 100%; font-size: 25px">
                        <asp:Label ID="Lblerror" runat="server" ForeColor="Red"></asp:Label>
                        <asp:HiddenField ID="hidEdad" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <%--            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
                    <ProgressTemplate>
                        <div class="overlay" />
                        <div class="overlayContent">
                            <h2>Procesando..</h2>
                            <img src="../Images/load.gif" alt="Loading" border="1" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>--%>
            <div class="panel-body">
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">DATOS TITULAR</h3>
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%; table-layout: fixed;">
                            <tr>
                                <td style="width: 5%"></td>
                                <td style="width: 15%"></td>
                                <td style="width: 29%"></td>
                                <td style="width: 2%"></td>
                                <td style="width: 15%;"></td>
                                <td style="width: 27%"></td>
                                <td style="width: 5%"></td>
                            </tr>
                            <tr>
                                <td></td>
                                <%--   <td>
                                    <h5>Producto:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlProducto" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlProducto_SelectedIndexChanged" TabIndex="1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td><h5>Monto Solicitado:</h5></td>
                                <td>
                                    <asp:TextBox ID="TxtMonto" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" TabIndex="2" Width="100%">0.00</asp:TextBox>
                                </td>--%>
                                <td>
                                    <h5>Campaing:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCampaign" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%" OnSelectedIndexChanged="DdlCampaign_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Canal:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProducto" runat="server" AutoPostBack="True" CssClass="form-control" TabIndex="1" Width="100%" OnSelectedIndexChanged="DdlProducto_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Tipo Documento:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlTipoDocumento" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" TabIndex="2" OnSelectedIndexChanged="DdlTipoDocumento_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Nro. Documento:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtNumeroDocumento" runat="server" CssClass="form-control upperCase" MaxLength="20" Width="100%" AutoPostBack="True" TabIndex="3" OnTextChanged="TxtNumeroDocumento_TextChanged"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="TxtNumeroDocumento_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="TxtNumeroDocumento">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Primer Nombre:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPrimerNombre" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="4"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Segundo Nombre:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtSegundoNombre" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="5"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Primer Apellido:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtPrimerApellido" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="6"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Segundo Apellido:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtSegundoApellido" runat="server" CssClass="form-control upperCase" MaxLength="80" Width="100%" TabIndex="7"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Genero:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlGenero" runat="server" CssClass="form-control" Width="100%" TabIndex="8">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Estado Civil:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlEstadoCivil" runat="server" CssClass="form-control" Width="100%" TabIndex="9">
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
                                    <asp:TextBox ID="TxtFechaNacimiento" runat="server" CssClass="form-control" Width="100%" TabIndex="10"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Edad:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtEdad" runat="server" CssClass="form-control upperCase" MaxLength="2" Width="100%" ReadOnly="True" TabIndex="11"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Provincia:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlProvincia" runat="server" CssClass="form-control" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlProvincia_SelectedIndexChanged" TabIndex="12">
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Ciudad:</h5>
                                </td>
                                <td>
                                    <asp:DropDownList ID="DdlCiudad" runat="server" CssClass="form-control" Width="100%" TabIndex="13">
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
                                    <asp:TextBox ID="TxtDireccion" runat="server" onkeydown="return (event.keyCode!=13);" Width="100%" CssClass="form-control upperCase" MaxLength="250" Height="50px" TextMode="MultiLine" TabIndex="14"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Teléfonos:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFonoCasa" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="15"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtFonoCasa_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFonoCasa">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td></td>
                                <td>
                                    <asp:TextBox ID="TxtFonoOficina" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="16"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtFonoOficina_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtFonoOficina">
                                    </asp:FilteredTextBoxExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtCelular" runat="server" CssClass="form-control upperCase" MaxLength="10" Width="100%" TabIndex="17"></asp:TextBox>
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
                                    <asp:TextBox ID="TxtEmail" runat="server" CssClass="form-control lowCase" MaxLength="80" Width="100%" TabIndex="18"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr style="display: none;">
                                <td></td>
                                <td>
                                    <h5>Fecha Solicitud:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtFechaSolicitud" runat="server" CssClass="form-control" TabIndex="19" Width="100%"></asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5 runat="server" id="LblEstado" visible="false">Estado Solicitud:</h5>
                                </td>
                                <td>
                                    <asp:CheckBox ID="ChkEstado" runat="server" AutoPostBack="True" Text="Activo" Checked="True" Visible="False" CssClass="form-control" TabIndex="20" OnCheckedChanged="ChkEstado_CheckedChanged" />
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <h5>Monto Anterior:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtMonto" runat="server" CssClass="form-control alinearDerecha" MaxLength="12" TabIndex="2" Width="100%" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');">0.00</asp:TextBox>
                                </td>
                                <td></td>
                                <td>
                                    <h5>Monto Total:</h5>
                                </td>
                                <td>
                                    <asp:TextBox ID="TxtMontoAc" runat="server" CssClass="form-control alinearDerecha" MaxLength="12" TabIndex="2" Width="100%" AutoPostBack="True" OnTextChanged="TxtMontoAc_TextChanged" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');">0.00</asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="TxtNumeroDocumento" />
                    </Triggers>
                </asp:UpdatePanel>
                <%--                <h3 id="TituloCodependiente" runat="server" class="label label-primary"
                    style="font-size: 14px; display: block; text-align: left; margin-top: 10px;">CODEPENDIENTE
                </h3>
                <asp:UpdatePanel
                    ID="UpdatePanel1"
                    runat="server">
                    <ContentTemplate>
                        <div class="panel panel-default"
                            style="margin-top: 10px;">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Button
                                            ID="Button1"
                                            runat="server"
                                            Text="Agregar Codependiente"
                                            CssClass="btn btn-primary"
                                            Enabled="false"
                                            CausesValidation="false"
                                            OnClick="BtnAgregarCodependiente_Click" />
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Panel
                                            ID="Panel1"
                                            runat="server"
                                            Visible="false">
                                            <div class="alert alert-info" style="margin-bottom: 0px;">
                                                <strong>Codependiente:</strong>
                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                                <strong>Documento:</strong>
                                                <asp:Label ID="Label2" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="Button2" runat="server" Text="Quitar"
                                                    CssClass="btn btn-danger btn-xs"
                                                    CausesValidation="false"
                                                    OnClick="BtnQuitarCodependiente_Click" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Label
                                            ID="Label3"
                                            runat="server"
                                            Text="No se ha agregado codependiente."
                                            ForeColor="Gray">
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left; margin-top: 10px;">CODEPENDIENTE</h3>
                <asp:UpdatePanel ID="UpdCodependiente" runat="server">
                    <ContentTemplate>
                        <div class="panel panel-default" style="margin-top: 10px;">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:Button ID="BtnAgregarCodependiente" runat="server"
                                            Text="Agregar Codeudor"
                                            CssClass="btn btn-primary"
                                            Enabled="false"
                                            CausesValidation="false"
                                            OnClick="BtnAgregarCodependiente_Click" />
                                    </div>
                                    <div class="col-md-9">
                                        <asp:Panel
                                            ID="PnlCodependienteSeleccionado"
                                            runat="server"
                                            Visible="false">
                                            <div class="alert alert-info" style="margin-bottom: 0px;">
                                                <strong>Codependiente:</strong>
                                                <asp:Label ID="LblNombreCodependiente" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                                <strong>Documento:</strong>
                                                <asp:Label ID="LblDocumentoCodependiente" runat="server"></asp:Label>
                                                &nbsp;&nbsp;
                                                <asp:Button ID="BtnQuitarCodependiente" runat="server"
                                                    Text="Quitar"
                                                    CssClass="btn btn-danger btn-xs"
                                                    CausesValidation="false"
                                                    OnClick="BtnQuitarCodependiente_Click" />
                                            </div>
                                        </asp:Panel>
                                        <asp:Label
                                            ID="LblSinCodependiente"
                                            runat="server"
                                            Text="No se ha agregado codependiente."
                                            ForeColor="Gray">
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <h3 runat="server" id="LblTituloExa" visible="false" class="label label-primary" style="font-size: 14px; display: block; text-align: left">SOLICITUD EXAMENES</h3>
                <%-- <table style="width: 100%">
                    <tr runat="server" id="TrExamenes" visible="false">
                        <td>
                            <asp:Panel ID="PnlExamenes" runat="server" Height="380px" GroupingText="Examenes">
                                <table class="nav-justified">
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
                                        <td></td>
                                        <td>
                                            <h5>Grupo:</h5>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DdlGrupoExamen" runat="server" class="chzn-select" TabIndex="21" Width="100%" AutoPostBack="True" OnSelectedIndexChanged="DdlGrupoExamen_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                        <td></td>
                                        <td>
                                            <h5>Examen:</h5>
                                        </td>
                                        <td colspan="2">
                                            <asp:DropDownList ID="DdlExamen" runat="server" class="chzn-select" TabIndex="22" Width="100%">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <h5>Observación:</h5>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="TxtObservacion" runat="server" onkeydown="return (event.keyCode!=13);" CssClass="form-control upperCase" Height="50px" MaxLength="250" TabIndex="23" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ImgAddExamen" runat="server" Height="20px" ImageUrl="~/Botones/agregar.jpg" TabIndex="24" OnClick="ImgAddExamen_Click" />
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="8">
                                            <asp:Panel ID="Panel4" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td colspan="6">
                                            <asp:Panel ID="PnlExamenesAgregados" runat="server" Height="180px" ScrollBars="Vertical">
                                                <asp:GridView ID="GrdvExamenes" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover table-responsive" DataKeyNames="CodigoEXSE,Adicional" OnRowDataBound="GrdvExamenes_RowDataBound" ShowHeaderWhenEmpty="True" TabIndex="25" Width="100%">
                                                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                    <Columns>
                                                        <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                        <asp:BoundField DataField="Examen" HeaderText="Examen" />
                                                        <asp:BoundField DataField="Adicional" HeaderText="Adicional">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Eliminar">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDelExamen" runat="server" Height="20px" ImageUrl="~/Botones/eliminaroff.jpg" Enabled="False" OnClick="ImgDelExamen_Click" />
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
                                        <td colspan="8">
                                            <asp:Panel ID="Panel1" runat="server" Height="20px"></asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>--%>
                <%--EXAMENES--%>
                <%-- <div class="row zona-examenes" id="panel" runat="server">
                    <div class="col-md-6">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <strong>Exámenes disponibles</strong>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="TxtBuscarExamen" runat="server" CssClass="form-control" placeholder="Buscar examen..."></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Button ID="BtnBuscarExamen" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="BtnBuscarExamen_Click" />
                                        <asp:Button ID="BtnLimpiarExamen" runat="server" Text="Limpiar" CssClass="btn btn-default" OnClick="BtnLimpiarExamen_Click" />
                                    </div>
                                </div>
                                <br />
                                <!-- GRID -->
                                <div class="tabla-examen-scroll">
                                    <asp:GridView ID="GrdvExamenesDisponibles" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover tabla-examen"
                                        DataKeyNames="EXPR_CODIGO" AllowPaging="True" PageSize="8"
                                        OnPageIndexChanging="GrdvExamenesDisponibles_PageIndexChanging"
                                        OnRowCommand="GrdvExamenesDisponibles_RowCommand"
                                        ShowHeaderWhenEmpty="True">
                                        <Columns>
                                            <asp:BoundField DataField="EXAMEN" HeaderText="Examen">
                                                <HeaderStyle Width="88%" />
                                                <ItemStyle Width="88%" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="">
                                                <HeaderStyle Width="12%" />
                                                <ItemStyle Width="12%" HorizontalAlign="Center" />

                                                <ItemTemplate>
                                                    <asp:LinkButton ID="BtnAgregarExamen" runat="server" Text="+" CssClass="btn btn-primary btn-sm" CommandName="Agregar" ToolTip="Agregar examen"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="«" LastPageText="»" PageButtonCount="5" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="paginador-examen" />
                                        <EmptyDataTemplate>
                                            <div class="alert alert-info">No se encontraron exámenes.</div>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="panel panel-primary">
                            <div class="panel-heading">
                                <strong>Exámenes seleccionados</strong>
                                <asp:Label ID="LblCantidadExamenes" runat="server" CssClass="pull-right" Text="0 examen(es)"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <asp:GridView ID="GrdvExamenesSeleccionados" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover tabla-examen"
                                    DataKeyNames="EXPR_CODIGO"
                                    OnRowCommand="GrdvExamenesSeleccionados_RowCommand"
                                    ShowHeaderWhenEmpty="True">
                                    <Columns>
                                        <asp:BoundField DataField="EXAMEN" HeaderText="Examen">
                                            <HeaderStyle Width="88%" />
                                            <ItemStyle Width="88%" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="">
                                            <HeaderStyle Width="12%" />
                                            <ItemStyle Width="12%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="BtnQuitarExamen" runat="server" Text="X" CssClass="btn btn-danger btn-sm" CommandName="Quitar" ToolTip="Quitar examen"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">No se han seleccionado exámene</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>--%>
                <h3
                    class="label label-primary" style="font-size: 14px; display: block; text-align: left; margin-top: 15px;">REQUISITOS / EXÁMENES A REALIZARSE
                </h3>
                <asp:UpdatePanel ID="UpdRequisitos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:Label ID="LblEstadoRequisitos" runat="server" Font-Bold="True"></asp:Label>
                                    </div>
                                    <div class="col-md-4 text-right">
                                        <asp:Button ID="BtnConsultarRequisitos" runat="server" Text="Consultar requisitos"
                                            CssClass="btn btn-primary"
                                            OnClick="BtnConsultarRequisitos_Click" />
                                    </div>
                                </div>
                                <br />
                                <asp:GridView ID="GrdvRequisitos" runat="server" AutoGenerateColumns="False" CssClass="table table-condensed table-bordered table-hover"
                                    DataKeyNames="ASRQ_CODIGO"
                                    ShowHeaderWhenEmpty="True"
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="GRUPO" HeaderText="Grupo">
                                            <HeaderStyle Width="10%" />
                                            <ItemStyle Width="10%" HorizontalAlign="Center" Font-Bold="True" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="REQUISITO" HeaderText="Requisito / Examen">
                                            <HeaderStyle Width="90%" />
                                            <ItemStyle Width="90%" />
                                        </asp:BoundField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">Consulte los requisitos correspondientes al Canal, Monto Total y Edad.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div style="text-align: right;">
                                    <asp:Label ID="LblCantidadRequisitos" runat="server" Font-Bold="True" Text="0 requisito(s)"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
                <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
                <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
                <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
                <%--<asp:UpdatePanel ID="UpdExamenes" runat="server">
                    <ContentTemplate>
                        <table class="nav-justified">
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
                            <tr id="TrDocumentoAdjunto" runat="server" visible="false">
                                <td></td>
                                <td colspan="2">
                                    <h5 runat="server" id="LblArchivo">Formulario de Declaración de Salud:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="26" />
                                    <asp:Panel ID="PnlArchivoDaquilema" runat="server" Visible="false">
                                        <div style="margin-top: 15px;">
                                            <label>Documento Adicional DAQUILEMA:</label>
                                            <asp:FileUpload ID="FileUploadDaquilema" runat="server" />
                                        </div>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <asp:UpdatePanel ID="UpdExamenes" runat="server">
                    <ContentTemplate>
                        <table class="nav-justified">
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
                                <td></td>
                                <td colspan="2">
                                    <h5>Examen adicional (opcional):</h5>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="TxtExamenAdicional" runat="server" CssClass="form-control upperCase"
                                        Width="100%"
                                        MaxLength="250"
                                        onkeydown="return (event.keyCode!=13);"
                                        placeholder="Ingrese un examen adicional si aplica">
                                    </asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Panel ID="PnlEspacioExamenAdicional" runat="server" Height="15px"></asp:Panel>
                                </td>
                            </tr>
                            <tr id="TrDocumentoAdjunto" runat="server" visible="false">
                                <td></td>
                                <td colspan="2">
                                    <h5 runat="server" id="LblArchivo">Formulario de Declaración de Salud:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="26" />
                                    <asp:Panel ID="PnlArchivoDaquilema" runat="server" Visible="false">
                                        <div style="margin-top: 15px;">
                                            <label>Documento Adicional DAQUILEMA:</label>
                                            <asp:FileUpload ID="FileUploadDaquilema" runat="server" />
                                        </div>
                                    </asp:Panel>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <asp:Panel ID="Panel2" runat="server" Height="20px"></asp:Panel>
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
                            <%--<table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="160px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="27" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="160px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="28" />
                                    </td>
                                </tr>
                            </table>--%>
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center; width: 33%">
                                        <asp:Button ID="BtnGrabar" runat="server"
                                            Text="Grabar"
                                            Width="160px"
                                            CssClass="button"
                                            OnClick="BtnGrabar_Click"
                                            TabIndex="27" />
                                    </td>
                                    <td style="text-align: center; width: 34%">
                                        <asp:Button ID="BtnCancelarSolicitud" runat="server"
                                            Text="Cancelar Solicitud"
                                            Width="160px"
                                            CssClass="button"
                                            CausesValidation="False"
                                            Visible="False"
                                            OnClick="BtnCancelarSolicitud_Click"
                                            OnClientClick="return confirm('¿Está seguro de cancelar esta solicitud? La solicitud quedará inactiva y deberá crear una nueva si necesita corregir el monto.');"
                                            TabIndex="28" />
                                    </td>
                                    <td style="text-align: center; width: 33%">
                                        <asp:Button ID="BtnSalir" runat="server"
                                            Text="Salir"
                                            Width="160px"
                                            CausesValidation="False"
                                            CssClass="button"
                                            OnClick="BtnSalir_Click"
                                            TabIndex="29" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="BtnGrabar" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
        <asp:Button ID="BtnDummyCodependiente" runat="server" Style="display: none;" />
        <asp:ModalPopupExtender ID="MpeCodependiente" runat="server"
            TargetControlID="BtnDummyCodependiente"
            PopupControlID="PnlModalCodependiente"
            BackgroundCssClass="popupCodepFondo"
            PopupDragHandleControlID="PnlCabeceraCodependiente">
        </asp:ModalPopupExtender>
        <asp:Panel ID="PnlModalCodependiente" runat="server" CssClass="popupCodep" Style="display: none;">
            <asp:Panel ID="PnlCabeceraCodependiente" runat="server" CssClass="popupCodepCabecera">
                <span>Agregar Codeudor</span>
                <span class="popupCodepMover">Mover</span>
            </asp:Panel>
            <asp:UpdatePanel ID="UpdModalCodependiente" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="popupCodepContenido">
                        <div class="popupCodepCuerpo">
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Tipo Documento:</label>
                                        <asp:DropDownList ID="DdlTipoDocumentoCodep" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label>Nro. Documento:</label>
                                        <asp:TextBox ID="TxtNumeroDocumentoCodep" runat="server"
                                            CssClass="form-control upperCase"
                                            MaxLength="20">
                                        </asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FteNumeroDocumentoCodep" runat="server"
                                            Enabled="True"
                                            FilterType="Numbers"
                                            TargetControlID="TxtNumeroDocumentoCodep">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <asp:Button ID="BtnBuscarCodependiente" runat="server" Text="Buscar"
                                            CssClass="btn btn-primary btn-block"
                                            CausesValidation="false"
                                            OnClick="BtnBuscarCodependiente_Click" />
                                    </div>
                                </div>
                            </div>
                            <asp:Label ID="LblMensajeCodependiente" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                            <asp:Panel ID="PnlCodependienteEncontrado" runat="server" Visible="false">
                                <div class="alert alert-success" style="margin-top: 15px; margin-bottom: 0;">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <strong>Documento:</strong>
                                            <asp:Label ID="LblDocumentoCodepEncontrado" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <strong>Nombre:</strong>
                                            <asp:Label ID="LblNombreCodepEncontrado" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row" style="margin-top: 10px;">
                                        <div class="col-md-6">
                                            <strong>Fecha Nacimiento:</strong>
                                            <asp:Label ID="LblFechaNacimientoCodepEncontrado" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <strong>Email:</strong>
                                            <asp:Label ID="LblEmailCodepEncontrado" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PnlNuevoCodependiente" runat="server" Visible="false">
                                <hr />
                                <div class="popupCodepSubtitulo">
                                    Datos del nuevo codeudor
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Primer Nombre:</label>
                                            <asp:TextBox ID="TxtPrimerNombreCodep" runat="server"
                                                CssClass="form-control upperCase"
                                                MaxLength="80">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Segundo Nombre:</label>
                                            <asp:TextBox ID="TxtSegundoNombreCodep" runat="server"
                                                CssClass="form-control upperCase"
                                                MaxLength="80">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Primer Apellido:</label>
                                            <asp:TextBox ID="TxtPrimerApellidoCodep" runat="server"
                                                CssClass="form-control upperCase"
                                                MaxLength="80">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Segundo Apellido:</label>
                                            <asp:TextBox ID="TxtSegundoApellidoCodep" runat="server"
                                                CssClass="form-control upperCase"
                                                MaxLength="80">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Género:</label>
                                            <asp:DropDownList ID="DdlGeneroCodep" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Estado Civil:</label>
                                            <asp:DropDownList ID="DdlEstadoCivilCodep" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Fecha Nacimiento:</label>
                                            <asp:TextBox ID="TxtFechaNacimientoCodep" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                            <asp:HiddenField ID="HdnFechaNacimientoCodep" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Email:</label>
                                            <asp:TextBox ID="TxtEmailCodep" runat="server" CssClass="form-control lowCase" MaxLength="80"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Provincia:</label>
                                            <asp:DropDownList ID="DdlProvinciaCodep" runat="server" CssClass="form-control"
                                                AutoPostBack="true"
                                                OnSelectedIndexChanged="DdlProvinciaCodep_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Ciudad:</label>
                                            <asp:DropDownList ID="DdlCiudadCodep" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label>Dirección:</label>
                                            <asp:TextBox ID="TxtDireccionCodep" runat="server" CssClass="form-control upperCase"
                                                MaxLength="250"
                                                TextMode="MultiLine"
                                                Rows="2">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Teléfono Casa:</label>
                                            <asp:TextBox ID="TxtFonoCasaCodep" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Teléfono Oficina:</label>
                                            <asp:TextBox ID="TxtFonoOficinaCodep" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Celular:</label>
                                            <asp:TextBox ID="TxtCelularCodep" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="popupCodepPie">
                            <asp:Button ID="BtnCancelarCodependiente" runat="server" Text="Cancelar"
                                CssClass="btn btn-default"
                                CausesValidation="false"
                                OnClick="BtnCancelarCodependiente_Click" />
                            <asp:Button ID="BtnSeleccionarCodependiente" runat="server" Text="Agregar Codependiente"
                                CssClass="btn btn-primary"
                                Visible="false"
                                CausesValidation="false"
                                OnClick="BtnSeleccionarCodependiente_Click" />
                            <asp:Button ID="BtnCrearCodependiente" runat="server" Text="Crear y Agregar"
                                CssClass="btn btn-success"
                                Visible="false"
                                CausesValidation="false"
                                OnClick="BtnCrearCodependiente_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </form>
    <script>
        function Close() {
            window.top.location.reload();
        }
        /*Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);*/
        //function endRequestHandler() {
        //    $(".chzn-select").chosen({ width: "95%" });
        //    $(".chzn-container").css({ "width": "95%" });
        //    $(".chzn-drop").css({ "width": "95%" });
        //}
        function abrirModalCodependiente() {
            $('#modalCodependiente').modal('show');
        }
        function cerrarModalCodependiente() {
            $('#modalCodependiente').modal('hide');
        }
    </script>
</body>
</html>
