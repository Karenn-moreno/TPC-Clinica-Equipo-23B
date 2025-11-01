<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="ClinicaWeb.GestionMedicos" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Clínica Salud - Panel de Recepción</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="~/Content/GestionMedicos.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com" rel="preconnect" />
    <link crossorigin="" href="https://fonts.gstatic.com" rel="preconnect" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
   
</head>
<body class="bg-light">
    <div class="d-flex">
        <aside class="sidebar bg-white border-end p-3 d-flex flex-column justify-content-between">
            <div>
                <div class="d-flex align-items-center gap-3 px-2 mb-4">
                    <img alt="Clinic logo" class="rounded-circle" src="https://lh3.googleusercontent.com/aida-public/AB6AXuDeAt9T3P2qjZpQj8lzu2zGENu6a5BQkzQtHA1xL-0Lcho4WiUK6Pny13lFsmCHjJ2gzEBYyWcXdDOtEp9qscD0DSuLvg9RWtTUo8QAP_lEZNqMwetgMh1_2z8c_n-jpRmr-YICTx6OruHIpyu6QscEDCyGdloskjEdVKU3Gw7VS2In2IdxNh-bd7rpVckTaFBuunzKao680qp3s9r4kylecoB4650yW-zaDTru_srBHQUrxPubNgEdLU80hNn9z0Pvp0NuogNBX_Ai" style="width: 40px; height: 40px; object-fit: cover;" />
                    <div>
                        <h1 class="h6 mb-0 fw-bold text-gray-900">Clínica Sanare</h1>
                        <p class="small text-muted mb-0">Panel de Recepción</p>
                    </div>
                </div>
                <nav class="nav flex-column nav-pills gap-1">
                    <a class="nav-link d-flex align-items-center gap-3" href="/GestionTurnos.aspx">
                        <span class="material-symbols-outlined">calendar_month</span>
                        <span>Turnos</span>
                    </a>
                    <a class="nav-link d-flex align-items-center gap-3" href="/GestionPacientes.aspx">
                        <span class="material-symbols-outlined">group</span>
                        <span>Pacientes</span>
                    </a>
                    <a class="nav-link d-flex align-items-center gap-3 active" href="#">
                        <span class="material-symbols-outlined">stethoscope</span>
                        <span>Médicos</span>
                    </a>
                </nav>
            </div>
            <div class="nav flex-column nav-pills gap-1">
                <a class="nav-link d-flex align-items-center gap-3" href="#">
                    <span class="material-symbols-outlined">account_circle</span>
                    <span>Mi Perfil</span>
                </a>
                <a class="nav-link d-flex align-items-center gap-3" href="/Default.aspx">
                    <span class="material-symbols-outlined">logout</span>
                    <span>Cerrar Sesión</span>
                </a>
            </div>
        </aside>
        <main class="main-content p-4 p-md-5">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-12">
                        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4 gap-3">
                            <div>
                                <h1 class="h2 fw-bold mb-1 text-gray-900">Gestión de Médicos</h1>
                                <p class="mb-0 text-gray-500">Busque, vea y administre la información de los médicos.</p>
                            </div>
                            <button class="btn btn-primary d-flex align-items-center gap-2" data-bs-target="#editDoctorModal" data-bs-toggle="modal">
                                <span class="material-symbols-outlined">add</span>
                                <span>Añadir Médico</span>
                            </button>
                        </div>
                        <div class="card shadow-sm border-light-subtle">
                            <div class="card-body">
                                <div class="row g-3 mb-4">
                                    <div class="col-md-6 col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-text bg-light border-end-0"><span class="material-symbols-outlined">search</span></span>
                                            <input class="form-control border-start-0" placeholder="Buscar por nombre o especialidad..." type="text" />
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-lg-4">
                                        <select class="form-select">
                                            <option selected="">Filtrar por especialidad</option>
                                            <option value="1">Cardiología</option>
                                            <option value="2">Dermatología</option>
                                            <option value="3">Pediatría</option>
                                            <option value="4">Traumatología</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="table-responsive">
                                    <table class="table table-hover align-middle">
                                        <thead class="table-light">
                                            <tr>
                                                <th scope="col">Nombre</th>
                                                <th scope="col">Especialidad</th>
                                                <th scope="col">Horario</th>
                                                <th scope="col">Pacientes Asignados</th>
                                                <th class="text-end" scope="col">Acciones</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>Dr. Alejandro García</td>
                                                <td>Cardiología</td>
                                                <td>L-V, 9:00 - 17:00</td>
                                                <td>25</td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#assignPatientModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">person_add</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#doctorDetailsModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">visibility</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary" data-bs-target="#editDoctorModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Dra. Sofía López</td>
                                                <td>Dermatología</td>
                                                <td>M-J, 10:00 - 18:00</td>
                                                <td>18</td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#assignPatientModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">person_add</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#doctorDetailsModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">visibility</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary" data-bs-target="#editDoctorModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Dr. Carlos Martínez</td>
                                                <td>Pediatría</td>
                                                <td>L-M-V, 8:00 - 15:00</td>
                                                <td>32</td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#assignPatientModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">person_add</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#doctorDetailsModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">visibility</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary" data-bs-target="#editDoctorModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">edit</span></button>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Dra. Laura Fernández</td>
                                                <td>Traumatología</td>
                                                <td>L-V, 9:00 - 13:00</td>
                                                <td>21</td>
                                                <td class="text-end">
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#assignPatientModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">person_add</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary me-1" data-bs-target="#doctorDetailsModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">visibility</span></button>
                                                    <button class="btn btn-sm btn-outline-secondary" data-bs-target="#editDoctorModal" data-bs-toggle="modal"><span class="material-symbols-outlined fs-6">edit</span></button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </div>
    <div aria-hidden="true" aria-labelledby="assignPatientModalLabel" class="modal fade" id="assignPatientModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="assignPatientModalLabel">Asignar Paciente a Dr. Alejandro García</h5>
                    <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="mb-3">
                            <label class="form-label" for="patientSearch">Buscar Paciente</label>
                            <input class="form-control" id="patientSearch" placeholder="Escriba el nombre o DNI del paciente..." type="text" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="patientList">Resultados de búsqueda</label>
                            <select class="form-select" id="patientList" size="5">
                                <option>Ana Martínez (DNI: ...456B)</option>
                                <option>Javier Gómez (DNI: ...789C)</option>
                                <option>Lucía Fernández (DNI: ...012D)</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                    <button class="btn btn-primary" type="button">Asignar Paciente</button>
                </div>
            </div>
        </div>
    </div>
    <div aria-hidden="true" aria-labelledby="doctorDetailsModalLabel" class="modal fade" id="doctorDetailsModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="doctorDetailsModalLabel">Detalles del Médico</h5>
                    <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                </div>
                <div class="modal-body">
                    <p><strong>Nombre:</strong> Dr. Alejandro García</p>
                    <p><strong>Especialidad:</strong> Cardiología</p>
                    <p><strong>Email:</strong> a.garcia@clinicasalud.com</p>
                    <p><strong>Teléfono:</strong> +34 123 456 789</p>
                    <p><strong>Horario:</strong> Lunes a Viernes, 9:00 - 17:00</p>
                    <p><strong>Pacientes Asignados:</strong> 25</p>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <div aria-hidden="true" aria-labelledby="editDoctorModalLabel" class="modal fade" id="editDoctorModal" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editDoctorModalLabel">Editar Información del Médico</h5>
                    <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="mb-3">
                            <label class="form-label" for="doctorName">Nombre Completo</label>
                            <input class="form-control" id="doctorName" type="text" value="Dr. Alejandro García" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="doctorSpecialty">Especialidad</label>
                            <select class="form-select" id="doctorSpecialty">
                                <option selected="">Cardiología</option>
                                <option>Dermatología</option>
                                <option>Pediatría</option>
                            </select>
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="doctorEmail">Email</label>
                            <input class="form-control" id="doctorEmail" type="email" value="a.garcia@clinicasalud.com" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="doctorPhone">Teléfono</label>
                            <input class="form-control" id="doctorPhone" type="tel" value="+34 123 456 789" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="doctorHours">Horario</label>
                            <textarea class="form-control" id="doctorHours" rows="3">Lunes a Viernes, 9:00 - 17:00</textarea>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                    <button class="btn btn-primary" type="button">Guardar Cambios</button>
                </div>
            </div>
        </div>
    </div>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
