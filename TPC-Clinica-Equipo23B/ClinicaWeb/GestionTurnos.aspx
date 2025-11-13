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
                                        <input class="form-control form-control-sm w-auto" type="date" value="2024-10-26" />
                                    </div>
                                    <div class="w-100 w-md-auto">
                                        <div class="position-relative">
                                            <span class="material-symbols-outlined position-absolute start-0 top-50 translate-middle-y text-muted ps-2">search</span>
                                            <input class="form-control form-control-sm ps-5" placeholder="Buscar paciente..." type="text" />
                                        </div>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <table class="table table-striped table-hover align-middle">
                                        <thead>
                                            <tr>
                                                <th class="text-muted fw-semibold py-2" scope="col">Hora</th>
                                                <th class="text-muted fw-semibold py-2" scope="col">Paciente</th>
                                                <th class="text-muted fw-semibold py-2" scope="col">Médico</th>
                                                <th class="text-muted fw-semibold py-2" scope="col">Estado</th>
                                                <th class="text-muted fw-semibold py-2 text-end" scope="col">Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <input class="form-control form-control-sm" type="time" value="09:00" /></td>
                                                <td class="text-dark">Carlos Rodríguez</td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option>Dr. García</option>
                                                        <option>Dra. López</option>
                                                        <option>Dr. Martínez</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option class="status-success" selected="">Confirmado</option>
                                                        <option class="status-warning">Pendiente</option>
                                                        <option class="status-danger">Cancelado</option>
                                                    </select>
                                                </td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-link text-primary p-0"><span class="material-symbols-outlined">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input class="form-control form-control-sm" type="time" value="09:30" /></td>
                                                <td class="text-dark">Ana Martínez</td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option>Dr. García</option>
                                                        <option selected="">Dra. López</option>
                                                        <option>Dr. Martínez</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option class="status-success">Confirmado</option>
                                                        <option class="status-warning" selected="">Pendiente</option>
                                                        <option class="status-danger">Cancelado</option>
                                                    </select>
                                                </td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-link text-primary p-0"><span class="material-symbols-outlined">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input class="form-control form-control-sm" type="time" value="11:00" /></td>
                                                <td class="text-dark">Javier Gómez</td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option selected="">Dr. García</option>
                                                        <option>Dra. López</option>
                                                        <option>Dr. Martínez</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option class="status-success" selected="">Confirmado</option>
                                                        <option class="status-warning">Pendiente</option>
                                                        <option class="status-danger">Cancelado</option>
                                                    </select>
                                                </td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-link text-primary p-0"><span class="material-symbols-outlined">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input class="form-control form-control-sm" type="time" value="12:00" /></td>
                                                <td class="text-dark">Lucía Fernández</td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option>Dr. García</option>
                                                        <option selected="">Dra. López</option>
                                                        <option>Dr. Martínez</option>
                                                    </select>
                                                </td>
                                                <td>
                                                    <select class="form-select form-select-sm">
                                                        <option class="status-success">Confirmado</option>
                                                        <option class="status-warning">Pendiente</option>
                                                        <option class="status-danger" selected="">Cancelado</option>
                                                    </select>
                                                </td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-link text-primary p-0"><span class="material-symbols-outlined">edit</span></button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="d-flex justify-content-end mt-3">
                                    <button class="btn btn-primary">Guardar Cambios</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </main>
            </div>
        </div>

    </form>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
