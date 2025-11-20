<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionTurnos.aspx.cs" Inherits="ClinicaWeb.GestionTurnos" %>

<!DOCTYPE html>
<html class="light" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Clínica Salud - Panel de Recepción</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="~/Content/GestionTurnos.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com" rel="preconnect" />
    <link crossorigin="" href="https://fonts.gstatic.com" rel="preconnect" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&amp;display=swap" rel="stylesheet" />
</head>
<body class="bg-background-light dark:bg-background-dark">
    <form id="form3" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="d-flex min-vh-100 w-100 flex-column">
            <div class="d-flex flex-grow-1">
                <aside class="d-flex flex-column aside-custom p-4 justify-content-between sticky-top vh-100" style="width: 250px;">
                    <div class="d-flex flex-column gap-4">
                        <div class="d-flex gap-3 align-items-center px-2">
                            <div class="bg-center bg-no-repeat aspect-square bg-cover rounded-circle" data-alt="Clinic logo" style='width: 40px; height: 40px; background-image: url("https://lh3.googleusercontent.com/aida-public/AB6AXuDeAt9T3P2qjZpQj8lzu2zGENu6a5BQkzQtHA1xL-0Lcho4WiUK6Pny13lFsmCHjJ2gzEBYyWcXdDOtEp9qscD0DSuLvg9RWtTUo8QAP_lEZNqMwetgMh1_2z8c_n-jpRmr-YICTx6OruHIpyu6QscEDCyGdloskjEdVKU3Gw7VS2In2IdxNh-bd7rpVckTaFBuunzKao680qp3s9r4kylecoB4650yW-zaDTru_srBHQUrxPubNgEdLU80hNn9z0Pvp0NuogNBX_Ai");'></div>
                            <div class="d-flex flex-column">
                                <h1 class="text-dark fs-6 fw-bold mb-0">Clínica Sanare</h1>
                                <p class="text-secondary small mb-0">Panel de Recepción</p>
                            </div>
                        </div>
                        <nav class="nav flex-column gap-2">
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg active-custom" href="#">
                                <span class="material-symbols-outlined">calendar_month</span>
                                <p class="mb-0 small fw-medium">Turnos</p>
                            </a>
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="/GestionPacientes.aspx">
                                <span class="material-symbols-outlined">group</span>
                                <p class="mb-0 small fw-medium">Pacientes</p>
                            </a>
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="/GestionMedicos.aspx">
                                <span class="material-symbols-outlined">stethoscope</span>
                                <p class="mb-0 small fw-medium">Médicos</p>
                            </a>
                        </nav>
                    </div>
                    <div class="d-flex flex-column gap-1">
                        <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="#">
                            <span class="material-symbols-outlined">account_circle</span>
                            <p class="mb-0 small fw-medium">Mi Perfil</p>
                        </a>
                        <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="/Default.aspx">
                            <span class="material-symbols-outlined">logout</span>
                            <p class="mb-0 small fw-medium">Cerrar Sesión</p>
                        </a>
                    </div>
                </aside>
                <main class="flex-grow-1 p-4">
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
                                        <input class="form-control form-control-sm w-auto" type="date" value="2024-10-26" id="txtFechaGrilla" runat="server" autopostback="true" ontextchanged="txtFechaGrilla_TextChanged" />
                                    </div>
                                    <div class="w-100 w-md-auto">
                                        <div class="position-relative">
                                            <span class="material-symbols-outlined position-absolute start-0 top-50 translate-middle-y text-muted ps-2">search</span>
                                            <input class="form-control form-control-sm ps-5" placeholder="Buscar paciente..." type="text" />
                                        </div>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    >
                                    <asp:GridView ID="gvTurnos" runat="server"
                                        CssClass="table table-striped table-hover align-middle"
                                        AutoGenerateColumns="False"
                                        GridLines="None"
                                        EmptyDataText="No hay turnos agendados para este día."
                                        OnRowDataBound="gvTurnos_RowDataBound">

                                        <HeaderStyle CssClass="text-muted fw-semibold py-2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Hora">
                                                <ItemTemplate>
                                                    <input class="form-control form-control-sm" type="time"
                                                        name='<%# "txtHoraInicio_" + Eval("IdTurno") %>'
                                                        value='<%# Eval("FechaHoraInicio", "{0:HH:mm}") %>' />
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
                                                    <select class="form-select form-select-sm"
                                                        name='<%# "ddlMedico_" + Eval("IdTurno") %>'
                                                        id='Select1'
                                                        runat="server">
                                                    </select>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Estado">
                                                <ItemTemplate>
                                                    <select class="form-select form-select-sm"
                                                        name='<%# "ddlEstado_" + Eval("IdTurno") %>'
                                                        id='Select2'
                                                        runat="server">
                                                    </select>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                                <ItemTemplate>
                                                    <button class="btn btn-sm btn-link text-primary p-0" type="button" data-bs-toggle="modal" data-bs-target="#viewTurnoModal">
                                                        <span class="material-symbols-outlined">edit</span>
                                                    </button>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="d-flex justify-content-end mt-3">
                                    <asp:Button ID="btnGuardarCambiosGrilla" runat="server"
                                        Text="Guardar Cambios" CssClass="btn btn-primary"
                                        OnClick="btnGuardarCambiosGrilla_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </main>
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
                                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label" for="ddlPaciente">Paciente</label>
                                        <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-control">
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                        <asp:Button ID="btnGuardarTurno" runat="server" Text="Guardar Turno" CssClass="btn btn-primary" OnClick="btnGuardarTurno_Click" />
                        <asp:Label ID="lblErrorNuevoTurno" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
