<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAuditarExamen.aspx.cs" Inherits="Pry_PrestasaludWAP.Examenes.FrmAuditarExamen" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="cc1" %>

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

        .info-auditoria {
            font-size: 12px;
        }

            .info-auditoria .panel-heading {
                padding: 8px 12px;
            }

            .info-auditoria .panel-title {
                font-size: 12px;
                font-weight: bold;
            }

            .info-auditoria .panel-body {
                padding: 10px 15px;
            }

            .info-auditoria .info-label {
                font-weight: bold;
                color: #444;
            }

            .info-auditoria .info-fila {
                margin-bottom: 7px;
            }

            .info-auditoria .info-valor {
                color: #333;
            }

        .modal-pdf-fondo {
            display: none;
            position: fixed;
            z-index: 9999;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.65);
            overflow: auto;
            padding: 12px;
        }


        /* ==========================================
        CONTENEDOR
        PANTALLAS GRANDES
        ========================================== */

        .modal-pdf-contenedor {
            width: 96%;
            max-width: 1400px;
            margin: 0 auto;
            background-color: #fff;
            border-radius: 4px;
            box-shadow: 0 4px 18px rgba(0, 0, 0, 0.35);
            overflow: hidden;
        }


        /* ==========================================
        CABECERA
        ========================================== */

        .modal-pdf-cabecera {
            position: relative;
            min-height: 42px;
            padding: 11px 48px 10px 15px;
            background-color: #337ab7;
            color: #fff;
            font-size: 13px;
            font-weight: bold;
            line-height: 20px;
        }


        .modal-pdf-cerrar {
            position: absolute;
            top: 4px;
            right: 8px;
            width: 36px;
            height: 34px;
            padding: 0;
            border: 0;
            background: transparent;
            color: #fff;
            font-size: 27px;
            line-height: 30px;
            cursor: pointer;
        }


        /* ==========================================
        CUERPO
        ========================================== */

        .modal-pdf-cuerpo {
            width: 100%;
            padding: 0;
            background-color: #d9d9d9;
            overflow: hidden;
        }


        #frameResultadoPdf {
            display: block;
            width: 100%;
            height: calc(100vh - 125px);
            min-height: 400px;
            border: 0;
            background-color: #fff;
        }


        /* ==========================================
        PIE
        ========================================== */

        .modal-pdf-pie {
            min-height: 48px;
            padding: 7px 12px;
            text-align: right;
            border-top: 1px solid #ddd;
            background-color: #fff;
        }


        /* ==========================================
        TABLET
        ========================================== */

        @media (max-width: 991px) {

            .modal-pdf-fondo {
                padding: 8px;
            }

            .modal-pdf-contenedor {
                width: 100%;
                max-width: none;
            }

            #frameResultadoPdf {
                height: calc(100vh - 120px);
                min-height: 350px;
            }
        }


        /* ==========================================
        CELULAR / PANTALLA PEQUEÑA
        ========================================== */

        @media (max-width: 767px) {

            .modal-pdf-fondo {
                padding: 4px;
            }

            .modal-pdf-contenedor {
                width: 100%;
                margin: 0;
                border-radius: 2px;
            }

            .modal-pdf-cabecera {
                min-height: 44px;
                padding: 11px 48px 9px 10px;
                font-size: 12px;
            }

            .modal-pdf-cerrar {
                width: 42px;
                height: 40px;
                top: 1px;
                right: 3px;
            }

            #frameResultadoPdf {
                width: 100%;
                height: calc(100vh - 116px);
                min-height: 280px;
            }

            .modal-pdf-pie {
                min-height: 52px;
                padding: 7px;
                text-align: center;
            }

                .modal-pdf-pie .btn {
                    width: 100%;
                    min-height: 38px;
                }
        }


        /* ==========================================
        CELULARES MUY PEQUEÑOS
        ========================================== */

        @media (max-width: 480px) {

            .modal-pdf-fondo {
                padding: 2px;
            }

            .modal-pdf-cabecera {
                font-size: 11px;
            }

            #frameResultadoPdf {
                height: calc(100vh - 112px);
                min-height: 250px;
            }
        }

        .modal-pdf-cuerpo {
            width: 100%;
            height: 75vh;
            min-height: 400px;
            background: #ddd;
        }

        #visorResultadoAuditoria {
            width: 100%;
            height: 100%;
        }

        @media (max-width: 767px) {

            .modal-pdf-contenedor {
                width: 98%;
                margin: 5px auto;
            }

            .modal-pdf-cuerpo {
                height: 70vh;
                min-height: 300px;
            }
        }
    </style>

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
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="panel-info">
                <asp:UpdateProgress ID="updProgress" runat="server" DisplayAfter="0" AssociatedUpdatePanelID="UpdOpciones">
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
                <h3 class="label label-primary" style="font-size: 13px; display: block; text-align: left; margin-bottom: 10px;">INFORMACIÓN</h3>
                <div id="accordionInformacion" class="panel-group info-auditoria" role="tablist">
                    <asp:Repeater ID="RptPacientes" runat="server" OnItemDataBound="RptPacientes_ItemDataBound">
                        <ItemTemplate>
                            <div class="panel panel-default">
                                <div class="panel-heading" role="tab">
                                    <h4 class="panel-title">
                                        <a data-toggle="collapse" data-parent="#accordionInformacion" href='<%# "#infoPaciente" + Container.ItemIndex %>'>INFORMACIÓN -
                                            <%# Eval("TIPO_PERSONA").ToString() == "CODEPENDIENTE" ? "CODEUDOR" : Eval("TIPO_PERSONA").ToString() %>
                                            &nbsp;|&nbsp;
                                            <%# Eval("PACIENTE") %>
                                        </a>
                                    </h4>
                                </div>
                                <div id='<%# "infoPaciente" + Container.ItemIndex %>' class="<%# ClaseAcordeonPaciente(Eval("PERS_CODIGO")) %>">
                                    <div class="panel-body">
                                        <div class="row info-fila">
                                            <div class="col-sm-2 info-label">Nombres:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# Eval("NOMBRES") %>
                                            </div>
                                            <div class="col-sm-2 info-label">Apellidos:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# Eval("APELLIDOS") %>
                                            </div>
                                        </div>
                                        <div class="row info-fila">
                                            <div class="col-sm-2 info-label">Edad:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# Eval("EDAD") %> años
                                            </div>
                                            <div class="col-sm-2 info-label">Fecha nacimiento:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# FormatearFecha(Eval("FECHA_NACIMIENTO")) %>
                                            </div>
                                        </div>
                                        <div class="row info-fila">
                                            <div class="col-sm-2 info-label">Ciudad:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# Eval("CIUDAD") %>
                                            </div>
                                            <div class="col-sm-2 info-label">Monto Total:</div>
                                            <div class="col-sm-4 info-valor">
                                                <%# FormatearMonto(Eval("MONTO_TOTAL")) %>
                                            </div>
                                        </div>
                                        <div class="row info-fila">
                                            <div class="col-sm-2 info-label">Dirección:</div>
                                            <div class="col-sm-10 info-valor">
                                                <%# Eval("DIRECCION") %>
                                            </div>
                                        </div>
                                        <hr style="margin-top: 10px; margin-bottom: 10px;" />
                                        <div style="font-size: 12px; font-weight: bold; margin-bottom: 6px;">
                                            RESULTADOS ADJUNTOS
                                        </div>
                                        <asp:Repeater ID="RptResultados" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-condensed table-bordered" style="font-size: 11px; margin-bottom: 5px;">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 8%; text-align: center;">#</th>
                                                            <th style="width: 50%;">Archivo</th>
                                                            <th style="width: 12%; text-align: center;">Tipo</th>
                                                            <th style="width: 15%; text-align: center;">Ver</th>
                                                            <th style="width: 15%; text-align: center;">Descargar</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center;">
                                                        <%# Eval("EXRD_ORDEN") %>
                                                    </td>
                                                    <td>
                                                        <%# Eval("EXRD_NOMBRE") %>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%# MostrarTipoArchivo(Eval("EXRD_EXTENSION")) %>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <%--<asp:LinkButton ID="BtnVerResultado" runat="server"
                                                            CssClass="btn btn-primary btn-xs"
                                                            Text="Ver"
                                                            CommandArgument='<%# Eval("EXRD_CODIGO") %>'
                                                            Visible='<%# EsPdf(Eval("EXRD_EXTENSION")) %>'
                                                            OnCommand="BtnVerResultado_Command"
                                                            CausesValidation="false">
                                                        </asp:LinkButton>--%>
                                                        <a href="javascript:void(0);"
                                                            class="btn btn-primary btn-xs"
                                                            style='<%# EsPdf(Eval("EXRD_EXTENSION")) ? "": "display:none;" %>'
                                                            data-url='<%# ObtenerUrlResultado(Eval("EXRD_CODIGO")) %>'
                                                            onclick="
                                                               var visor = document.getElementById('visorResultadoAuditoria');
                                                               var modal = document.getElementById('modalResultadoPdf');

                                                               if (visor != null && modal != null) {
                                                                   visor.setAttribute('src', this.getAttribute('data-url'));
                                                                   modal.style.display = 'block';
                                                                   document.body.style.overflow = 'hidden';
                                                               }

                                                               return false;
                                                           ">Ver
                                                        </a>
                                                        <asp:Label ID="LblSinVista" runat="server"
                                                            Text="Vista no disponible"
                                                            ForeColor="Gray"
                                                            Visible='<%# !EsPdf(Eval("EXRD_EXTENSION")) %>'>
                                                        </asp:Label>
                                                    </td>
                                                    <td style="text-align: center;">
                                                        <asp:LinkButton ID="BtnDescargarResultado" runat="server"
                                                            CssClass="btn btn-default btn-xs"
                                                            Text="Bloqueado"
                                                            Enabled="false"
                                                            ToolTip="La descarga se encuentra bloqueada">
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                             </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <hr style="margin-top: 12px; margin-bottom: 10px;" />
                                        <div class="panel panel-default panel-auditoria">
                                            <div class="panel-heading"
                                                style="padding: 7px 10px;">
                                                <strong style="font-size: 12px;">REVISIÓN DEL AUDITOR</strong>
                                                <asp:Label ID="LblEstadoAuditoria" runat="server"
                                                    CssClass="pull-right"
                                                    Style="font-size: 11px; font-weight: bold;">
                                                </asp:Label>
                                            </div>
                                            <div class="panel-body"
                                                style="padding: 10px; font-size: 12px;">
                                                <asp:HiddenField
                                                    ID="HfCodigoPERS"
                                                    runat="server"
                                                    Value='<%# Eval("PERS_CODIGO") %>' />
                                                <div class="row"
                                                    style="margin-bottom: 10px;">
                                                    <div class="col-sm-2">
                                                        <strong>Resultado:</strong>
                                                    </div>
                                                    <div class="col-sm-10">
                                                        <asp:RadioButton ID="RdbAceptado"
                                                            runat="server"
                                                            GroupName="EstadoAuditoria"
                                                            Text=" ACEPTADO" />
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:RadioButton ID="RdbRechazado"
                                                            runat="server"
                                                            GroupName="EstadoAuditoria"
                                                            Text=" RECHAZADO" />
                                                    </div>
                                                </div>
                                                <%--  <div class="row"
                                                    style="margin-bottom: 10px;">
                                                    <div class="col-sm-2">
                                                        <strong>Observación:</strong>
                                                    </div>
                                                    <div class="col-sm-10">
                                                        <asp:TextBox ID="TxtObservacionAuditor" runat="server"
                                                            CssClass="form-control"
                                                            TextMode="MultiLine"
                                                            Rows="3"
                                                            MaxLength="1000"
                                                            Width="100%">
                                                        </asp:TextBox> 
                                                    </div>
                                                </div>--%>
                                                <div class="row" style="margin-bottom: 10px;">
                                                    <div class="col-sm-2">
                                                        <strong>Informe:</strong>
                                                    </div>
                                                    <div class="col-sm-10">
                                                        <asp:HiddenField ID="HfObservacionAuditor" runat="server" />
                                                        <asp:Button
                                                            ID="BtnEditarObservacion"
                                                            runat="server"
                                                            Text="Editar informe"
                                                            CssClass="btn btn-primary btn-xs"
                                                            CommandArgument='<%# Eval("PERS_CODIGO") %>'
                                                            OnCommand="BtnEditarObservacion_Command"
                                                            CausesValidation="false" />
                                                        &nbsp;
                                                        <asp:Label
                                                            ID="LblResumenObservacion"
                                                            runat="server"
                                                            Text="Sin informe"
                                                            ForeColor="Gray"
                                                            Style="font-size: 11px;">
                                                        </asp:Label>
                                                    </div>
                                                </div>
                                                <div class="row"
                                                    style="margin-bottom: 10px;">
                                                    <div class="col-sm-2">
                                                        <strong>Adjunto:</strong>
                                                    </div>
                                                    <div class="col-sm-10">
                                                        <asp:FileUpload
                                                            ID="FupAdjuntoAuditor"
                                                            runat="server" />
                                                        <div style="margin-top: 5px;">
                                                            <asp:Label
                                                                ID="LblAdjuntoAuditor"
                                                                runat="server"
                                                                ForeColor="Gray"
                                                                Style="font-size: 11px;">
                                                            </asp:Label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-sm-12"
                                                        style="text-align: right;">
                                                        <asp:Button ID="BtnGuardarAuditoria" runat="server"
                                                            Text="Guardar Auditoría"
                                                            CssClass="btn btn-success btn-sm"
                                                            CommandArgument='<%# Eval("PERS_CODIGO") %>'
                                                            OnCommand="BtnGuardarAuditoria_Command"
                                                            CausesValidation="false" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Label ID="LblSinResultados" runat="server"
                                            Visible="false"
                                            Text="No existen resultados adjuntos."
                                            ForeColor="Gray"
                                            Style="font-size: 11px;">
                                        </asp:Label>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="PnlEditorAuditoria" runat="server" Visible="false" CssClass="panel panel-default" Style="margin-top: 10px;">
                        <div class="panel-heading">
                            <strong>INFORME DEL AUDITOR</strong>
                            &nbsp;|&nbsp;
                            <asp:Label ID="LblPacienteEditor" runat="server"></asp:Label>
                        </div>
                        <div class="panel-body">
                            <cc1:Editor ID="EditorObservacionAuditor" runat="server"
                                Height="300px"
                                Width="100%"
                                AutoFocus="False" />
                            <div style="text-align: right; margin-top: 10px;">
                                <asp:Button ID="BtnAceptarObservacion" runat="server"
                                    Text="Aceptar observación"
                                    CssClass="btn btn-success btn-sm"
                                    CausesValidation="false"
                                    OnClick="BtnAceptarObservacion_Click" />
                                     &nbsp;
                                <asp:Button ID="BtnCerrarEditor" runat="server"
                                    Text="Cerrar"
                                    CssClass="btn btn-default btn-sm"
                                    CausesValidation="false"
                                    OnClick="BtnCerrarEditor_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="modalResultadoPdf"
                        class="modal-pdf-fondo">
                        <div class="modal-pdf-contenedor">
                            <div class="modal-pdf-cabecera">
                                <span>VISUALIZACIÓN DE RESULTADO</span>
                                <button type="button"
                                    class="modal-pdf-cerrar"
                                    onclick="
                                        var visor = document.getElementById('visorResultadoAuditoria');
                                        var modal = document.getElementById('modalResultadoPdf');

                                        if (visor != null) {
                                            visor.removeAttribute('src');
                                        }

                                        if (modal != null) {
                                            modal.style.display = 'none';
                                        }

                                        document.body.style.overflow = '';

                                        return false;
                                    ">
                                    ×
                                </button>
                            </div>
                            <div class="modal-pdf-cuerpo">
                                <embed id="visorResultadoAuditoria" type="application/pdf" style="width: 100%; height: 100%; border: 0;" />
                            </div>
                            <div class="modal-pdf-pie">
                                <button type="button"
                                    class="btn btn-default btn-sm"
                                    onclick="
                                            var visor = document.getElementById('visorResultadoAuditoria');
                                            var modal = document.getElementById('modalResultadoPdf');

                                            if (visor != null) {
                                                visor.removeAttribute('src');
                                            }

                                            if (modal != null) {
                                                modal.style.display = 'none';
                                            }

                                            document.body.style.overflow = '';

                                            return false;
                                        ">
                                    Cerrar
                                </button>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="PnlSinPacientes" runat="server" Visible="false">
                        <div class="alert alert-info" style="font-size: 12px; margin-bottom: 10px;">
                            No existe información de pacientes para esta solicitud.
                        </div>
                    </asp:Panel>
                </div>
                <%-- <h3 class="label label-primary" style="font-size: 14px; display: block; text-align: left">EXAMENES REALIZADOS</h3>
                <asp:UpdatePanel ID="UpdCabecera" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%">
                            <tr runat="server" id="TrExamenes">
                                <td>
                                    <asp:Panel ID="PnlExamenes" runat="server" Height="390px" GroupingText="Examenes">
                                        <table style="width: 100%">
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
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel4" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="6">
                                                    <asp:Panel ID="PnlExamenesAgregados" runat="server" Height="180px" ScrollBars="Vertical">
                                                        <asp:GridView ID="GrdvExamenes" runat="server" AutoGenerateColumns="False"
                                                            CssClass="table table-condensed table-bordered table-hover table-responsive" ShowHeaderWhenEmpty="True" TabIndex="2" Width="100%">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                                                                <asp:BoundField DataField="Examen" HeaderText="Examen" />
                                                                <asp:BoundField DataField="Adicional" HeaderText="Adicional">
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <RowStyle Font-Size="X-Small" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel1" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <asp:Panel ID="Panel2" runat="server" Height="20px">
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <h5>Observación:</h5>
                                                </td>
                                                <td colspan="5">
                                                    <asp:TextBox ID="TxtObservacion" runat="server" CssClass="form-control upperCase" Height="50px" MaxLength="250" onkeydown="return (event.keyCode!=13);" TabIndex="3" TextMode="MultiLine" Width="100%"></asp:TextBox>
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel3" runat="server" Height="20px">
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnGrabar" />
                    </Triggers>
                </asp:UpdatePanel>--%>
                <div class="panel panel-default">
                    <asp:UpdatePanel ID="UpdOpciones" runat="server">
                        <ContentTemplate>
                            <div style="width: 100%; text-align: center; padding: 12px 0;">
                                <asp:Button ID="BtnSalir" runat="server"
                                    CssClass="button"
                                    OnClick="BtnSalir_Click"
                                    CausesValidation="false"
                                    TabIndex="5"
                                    Text="Salir"
                                    Width="120px" />

                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
