<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="ClinicaWeb.GestionMedicos" %>

<!DOCTYPE html>
<html class="light" lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Clínica Salud - Gestión de Médicos</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
    <link href="~/Content/GestionTurnos.css" rel="stylesheet" type="text/css" />
</head>
<body class="bg-background-light dark:bg-background-dark">
    <form id="form1" runat="server">
        <div class="d-flex min-vh-100 w-100 flex-column">
            <div class="d-flex flex-grow-1">

                <!-- Sidebar -->
                <aside class="d-flex flex-column aside-custom p-4 justify-content-between sticky-top vh-100" style="width: 250px;">
                    <div class="d-flex flex-column gap-4">
                        <div class="d-flex gap-3 align-items-center px-2">
                            <div class="bg-center bg-no-repeat aspect-square bg-cover rounded-circle" style='width:40px;height:40px;background-image:url("tu_logo_aqui");'></div>
                            <div class="d-flex flex-column">
                                <h1 class="text-dark fs-6 fw-bold mb-0">Clínica Sanare</h1>
                                <p class="text-secondary small mb-0">Panel de Médicos</p>
                            </div>
                        </div>
                        <nav class="nav flex-column gap-2">
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="/GestionTurnos.aspx">
                                <span class="material-symbols-outlined">calendar_month</span>
                                <p class="mb-0 small fw-medium">Turnos</p>
                            </a>
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg text-secondary hover-bg-light" href="/GestionPacientes.aspx">
                                <span class="material-symbols-outlined">group</span>
                                <p class="mb-0 small fw-medium">Pacientes</p>
                            </a>
                            <a class="nav-link d-flex align-items-center gap-3 px-3 py-2 rounded-lg active-custom" href="/GestionMedicos.aspx">
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

                <!-- Main -->
                <main class="flex-grow-1 p-4">
                    <div class="d-flex flex-column gap-4">
                        <!-- Header -->
                        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3">
                            <div class="d-flex flex-column gap-1">
                                <h1 class="text-dark h3 fw-bold mb-0">Gestión de Médicos</h1>
                                <p class="text-secondary mb-0">Administra los médicos de la clínica de forma rápida y sencilla.</p>
                            </div>
                            <button class="btn btn-primary btn-sm d-flex align-items-center gap-2" data-bs-toggle="modal" data-bs-target="#addMedicoModal" type="button">
                                <span class="material-symbols-outlined">add</span>
                                <span>Nuevo Médico</span>
                            </button>
                        </div>

                        <!-- Grilla de Médicos -->
                        <div class="card card-custom p-4 rounded-xl">
                            <div class="table-responsive">
                                <asp:GridView ID="gvMedicos" runat="server" AutoGenerateColumns="false" CssClass="table table-hover align-middle" GridLines="None" EmptyDataText="No se encontraron médicos." OnRowCommand="gvMedicos_RowCommand">
                                    <HeaderStyle CssClass="table-light" />
                                    <Columns>
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                        <asp:BoundField DataField="EspecialidadesTexto" HeaderText="Especialidad" />
                                        <asp:BoundField DataField="HorariosTexto" HeaderText="Horario" />
                                        <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnHorarios" runat="server" CssClass="btn btn-sm btn-outline-primary me-1" CommandName="GestionarHorarios" CommandArgument='<%# Eval("IdPersona") %>'>
                                                    <span class="material-symbols-outlined fs-6">calendar_month</span> Horarios
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-sm btn-outline-secondary" CommandName="EditarMedico" CommandArgument='<%# Eval("IdPersona") %>'>
                                                    <span class="material-symbols-outlined fs-6">edit</span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </div>

        <!-- Modal para Nuevo Médico -->
        <div class="modal fade" id="addMedicoModal" tabindex="-1" aria-labelledby="addMedicoModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="addMedicoModalLabel">Agregar Nuevo Médico</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtNombre">Nombre</label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtApellido">Apellido</label>
                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtDni">DNI</label>
                                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtMatricula">Matrícula</label>
                                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <label class="form-label" for="txtEspecialidad">Especialidad</label>
                                <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
    <div class="col-md-6 mb-3">
        <label class="form-label" for="txtEmail">Email</label>
        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="col-md-6 mb-3">
        <label class="form-label" for="txtTelefono">Teléfono</label>
        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
    </div>
</div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        <asp:Button ID="btnGuardarMedico" runat="server" Text="Guardar Médico" CssClass="btn btn-primary" OnClick="btnGuardarMedico_Click" />
                    </div>
                </div>
            </div>
        </div>

    </form>
    <script crossorigin="anonymous" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>