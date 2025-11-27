<%@ Page Title="Gestión de Turnos" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GestionTurnos.aspx.cs" Inherits="ClinicaWeb.GestionTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionTurnos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <div class="d-flex flex-column gap-4">
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3">
            <div class="d-flex flex-column gap-1">
                <h1 class="text-dark h3 fw-bold mb-0">Gestión de Turnos</h1>
                <p class="text-secondary mb-0">Administra los turnos de los pacientes de forma rápida y sencilla.</p>
            </div>
            <button class="btn btn-primary btn-sm d-flex align-items-center gap-2" data-bs-toggle="modal" data-bs-target="#addTurnoModal" type="button">
                <span class="material-symbols-outlined">add</span>
                <span>Nuevo Turno</span>
            </button>
        </div>
        <div class="card card-custom p-4 rounded-xl">
            <div class="d-flex flex-column gap-4">
                <div class="d-flex flex-wrap align-items-center justify-content-between gap-3">
                    <div class="d-flex align-items-center gap-3">
                        <h2 class="h5 fw-bold text-dark mb-0">Turnos del Día</h2>
                        <asp:TextBox ID="txtFechaGrilla" runat="server"
                            CssClass="form-control form-control-sm w-auto"
                            TextMode="Date"
                            AutoPostBack="true"
                            OnTextChanged="txtFechaGrilla_TextChanged">
                        </asp:TextBox>
                    </div>
                    <div class="w-100 w-md-auto">
                        <div class="position-relative">
                            <span class="material-symbols-outlined position-absolute start-0 top-50 translate-middle-y text-muted ps-2">search</span>
                            <input class="form-control form-control-sm ps-5" placeholder="Buscar paciente..." type="text" />
                        </div>
                    </div>
                </div>
                <div class="table-responsive">

                    <asp:GridView ID="gvTurnos" runat="server"
                        DataKeyNames="IdTurno"
                        CssClass="table table-striped table-hover align-middle"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        EmptyDataText="No hay turnos agendados para este día."
                       >

                        <HeaderStyle CssClass="text-muted fw-semibold py-2" />
                        <Columns>
                            <asp:TemplateField HeaderText="Hora">
                                <ItemTemplate>
                                    <asp:Label ID="lblHora" runat="server" Text='<%# Eval("FechaHoraInicio", "{0:HH:mm}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Paciente">
                                <ItemTemplate>
                                    <asp:Label ID="lblPaciente" runat="server"
                                        Text='<%# Eval("Paciente.Nombre") + " " + Eval("Paciente.Apellido") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Médico">
                                <ItemTemplate>
                                    <asp:Label ID="lblMedico" runat="server" Text='<%# "Dr/a. " + Eval("Medico.Apellido") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <asp:Label ID="lblEstado" runat="server" Text='<%# Eval("EstadoTurno") %>' CssClass="badge bg-secondary text-decoration-none"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-link text-primary p-0"
                                        OnClick="btnEditar_Click"
                                         UseSubmitBehavior="false"
                                         formnovalidate=""
                                        CommandArgument='<%# Eval("IdTurno") %>'>
                                        <span class="material-symbols-outlined">edit</span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>               
            </div>
        </div>
    </div>

    <div aria-hidden="true" aria-labelledby="addTurnoModalLabel" class="modal fade" id="addTurnoModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addTurnoModalLabel">Agendar Nuevo Turno</h5>
                    <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                </div>

                <asp:UpdatePanel ID="upModalTurno" runat="server">
                    <ContentTemplate>

                        <div class="modal-body">
                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="ddlEspecialidad">Especialidad</label>
                                    <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="ddlMedico">Médico</label>
                                    <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-control mi-buscador" AutoPostBack="true" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="ddlPaciente">Paciente</label>
                                    <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-control mi-buscador">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="txtFecha">Fecha</label>
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" AutoPostBack="true" OnTextChanged="txtFecha_TextChanged"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="ddlHorario">Horario Disponible</label>
                                    <asp:DropDownList ID="ddlHorario" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label" for="txtMotivoConsulta">Motivo de la Consulta</label>
                                    <asp:TextBox ID="txtMotivoConsulta" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                            <asp:Button ID="btnGuardarTurno" runat="server" Text="Guardar Turno" CssClass="btn btn-primary" OnClick="btnGuardarTurno_Click" />
                            <asp:Label ID="lblErrorNuevoTurno" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <div class="modal fade" id="editTurnoModal" tabindex="-1" aria-labelledby="editTurnoModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editTurnoModalLabel">Editar Turno</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>

            <asp:UpdatePanel ID="upEditTurno" runat="server" UpdateMode="Conditional">
                <ContentTemplate>

                    <div class="modal-body">
                        <asp:HiddenField ID="hfIdTurnoEditar" runat="server" />

                        <div class="mb-3">
                            <label class="form-label text-muted small">Paciente</label>
                            <asp:TextBox ID="txtEditarPaciente" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Estado</label>
                                <asp:DropDownList ID="ddlEditarEstado" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">Hora</label>
                                <asp:TextBox ID="txtEditarHora" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Observaciones / Motivo</label>
                            <asp:TextBox ID="txtEditarMotivo" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnGuardarEdicion" runat="server" Text="Guardar Cambios" CssClass="btn btn-success" OnClick="btnGuardarEdicion_Click" UseSubmitBehavior="false"
                            formnovalidate="" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
         </div>
     </div>
  </div>
    <script type="text/javascript">
        function iniciarSelect2() {
            $('.mi-buscador').select2({
                dropdownParent: $('#addTurnoModal'),
                width: '100%',
                placeholder: "Buscar por Apellido, Nombre o DNI...",
                allowClear: true,
                language: {
                    noResults: function () { return "No se encontró el paciente"; }
                }
            });
        }

        // Al cargar la página
        $(document).ready(function () {
            iniciarSelect2();
        });

        // Al recargar el UpdatePanel
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            iniciarSelect2();
        });
    </script>

    <style>
        /* Ajuste visual */
        .select2-container .select2-selection--single {
            height: 38px !important;
            padding-top: 5px;
            border: 1px solid #ced4da;
        }
        .select2-container--default .select2-selection--single .select2-selection__arrow {
            height: 36px !important;
        }
    </style>
</asp:Content>