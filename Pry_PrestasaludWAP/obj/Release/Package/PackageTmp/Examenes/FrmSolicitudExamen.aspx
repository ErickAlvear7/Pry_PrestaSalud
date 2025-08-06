<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmSolicitudExamen.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmSolicitudExamen" %>

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
    <link rel="stylesheet" href="../Style/chosen.css" />

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
                                <td>
                                    <h5>Producto:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:DropDownList ID="DdlProducto" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="DdlProducto_SelectedIndexChanged" TabIndex="1" Width="100%">
                                    </asp:DropDownList>
                                </td>
                                <td><h5>Monto Solicitado:</h5></td>
                                <td>
                                    <asp:TextBox ID="TxtMonto" runat="server" CssClass="form-control alinearDerecha" MaxLength="6" TabIndex="2" Width="100%">0.00</asp:TextBox>
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
                            <tr>
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
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="TxtNumeroDocumento" />
                    </Triggers>
                </asp:UpdatePanel>
                <h3 runat="server" id="LblTituloExa" visible="false" class="label label-primary" style="font-size: 14px; display: block; text-align: left">SOLICITUD EXAMENES</h3>
                <table style="width: 100%">
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
                </table>
                <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
                <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
                <script type="text/javascript"> $(".chzn-select").chosen(); $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
                <script type="text/javascript" src="../JS/DatePicker/jquery-1.9.1.js"></script>
                <script type="text/javascript" src="../JS/DatePicker/jquery-ui.js"></script>
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
                                    <h5 runat="server" id="LblArchivo">Documento Adjunto:</h5>
                                </td>
                                <td colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" TabIndex="26" />
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
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: right; width: 45%">
                                        <asp:Button ID="BtnGrabar" runat="server" Text="Grabar" Width="120px" CssClass="button" OnClick="BtnGrabar_Click" TabIndex="27" />
                                    </td>
                                    <td style="width: 10%"></td>
                                    <td style="text-align: left; width: 45%">
                                        <asp:Button ID="BtnSalir" runat="server" Text="Salir" Width="120px" CausesValidation="False" CssClass="button" OnClick="BtnSalir_Click" TabIndex="28" />
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
