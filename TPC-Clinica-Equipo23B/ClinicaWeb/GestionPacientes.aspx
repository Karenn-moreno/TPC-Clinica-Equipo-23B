<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="ClinicaWeb.GestionPacientes" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Clínica Salud - Panel de Recepción</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="~/Content/GestionPacientes.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />

</head>
<body>
    <form id="form5" runat="server">
        <div class="d-flex">
            <aside class="sidebar p-3 d-flex flex-column justify-content-between">
                <div>
                    <div class="d-flex align-items-center mb-4 p-2">
                        <div class="bg-center bg-no-repeat aspect-square bg-cover rounded-circle me-3" style="width: 40px; height: 40px; background-image: url('https://lh3.googleusercontent.com/aida-public/AB6AXuDeAt9T3P2qjZpQj8lzu2zGENu6a5BQkzQtHA1xL-0Lcho4WiUK6Pny13lFsmCHjJ2gzEBYyWcXdDOtEp9qscD0DSuLvg9RWtTUo8QAP_lEZNqMwetgMh1_2z8c_n-jpRmr-YICTx6OruHIpyu6QscEDCyGdloskjEdVKU3Gw7VS2In2IdxNh-bd7rpVckTaFBuunzKao680qp3s9r4kylecoB4650yW-zaDTru_srBHQUrxPubNgEdLU80hNn9z0Pvp0NuogNBX_Ai');"></div>
                        <div>
                            <h5 class="mb-0 fw-bold">Clínica Sanare</h5>
                            <p class="text-muted mb-0 small">Panel de Recepción</p>
                        </div>
                    </div>
                    <nav class="nav flex-column nav-pills">
                        <a class="nav-link d-flex align-items-center py-2" href="/GestionTurnos.aspx">
                            <span class="material-symbols-outlined">calendar_month</span> Turnos
                        </a>
                        <a class="nav-link active d-flex align-items-center py-2" href="#">
                            <span class="material-symbols-outlined">group</span> Pacientes
                        </a>
                        <a class="nav-link d-flex align-items-center py-2" href="/GestionMedicos.aspx">
                            <span class="material-symbols-outlined">stethoscope</span> Médicos
                        </a>
                    </nav>
                </div>
                <div class="nav flex-column nav-pills">
                    <a class="nav-link d-flex align-items-center py-2" href="#">
                        <span class="material-symbols-outlined">account_circle</span> Mi Perfil
                    </a>
                    <a class="nav-link d-flex align-items-center py-2" href="/Default.aspx">
                        <span class="material-symbols-outlined">logout</span> Cerrar Sesión
                    </a>
                </div>
            </aside>
            <main class="main-content p-4 p-md-5">
                <!-- Botón para agregar paciente -->
                <div classclass="d-flex justify-content-between align-items-center mb-3">
                    <div>
                        <h3 class="fw-bold">Gestión de Pacientes</h3>
                        <p class="text-muted mb-0">Administra los pacientes de la clínica de manera rápida.</p>
                    </div>

                    <button id="btnNuevoPaciente" runat="server"
                        class="btn btn-primary btn-sm d-flex align-items-center gap-2"
                        data-bs-toggle="modal"
                        data-bs-target="#addPatientModal"
                        type="button">

                        <span class="material-symbols-outlined">add</span>
                        <span>Nuevo Paciente</span>
                    </button>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="card shadow-sm border-light-subtle">
                                <div class="card-body">
                                    <div class="table-responsive">

                                        <asp:GridView ID="gvPacientes" runat="server"
                                            CssClass="table table-hover align-middle"
                                            AutoGenerateColumns="false"
                                            GridLines="None"
                                            EmptyDataText="No se encontraron pacientes."
                                            OnRowCommand="gvPacientes_RowCommand">

                                            <HeaderStyle CssClass="table-light" />
                                            <Columns>

                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />

                                                <asp:BoundField DataField="DNI" HeaderText="DNI" />
                                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" />

                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                                    <ItemTemplate>

                                                        <asp:LinkButton ID="btnVer" runat="server"
                                                            CssClass="btn btn-sm btn-outline-secondary me-1"
                                                            CommandName="VerPaciente"
                                                            CommandArgument='<%# Eval("IdPersona") %>'>
                                                <span class="material-symbols-outlined fs-6">visibility</span>
                                            </asp:LinkButton>

                                                        <asp:LinkButton ID="btnEditar" runat="server"
                                                            CssClass="btn btn-sm btn-outline-primary"
                                                            CommandName="EditarPaciente"
                                                            CommandArgument='<%# Eval("IdPersona") %>'>
                                                <span class="material-symbols-outlined fs-6">edit</span>
                                            </asp:LinkButton>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>
        <div aria-hidden="true" aria-labelledby="addPatientModalLabel" class="modal fade" id="addPatientModal" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="addPatientModalLabel">Agregar Nuevo Paciente</h5>
                        <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                    </div>
                    <div class="modal-body">

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddNombre">Nombre</label>
                                <asp:TextBox ID="txtAddNombre" runat="server" CssClass="form-control" required=""></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddApellido">Apellido</label>
                                <asp:TextBox ID="txtAddApellido" runat="server" CssClass="form-control" required=""></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddDNI">DNI</label>
                                <asp:TextBox ID="txtAddDNI" runat="server" CssClass="form-control" required=""></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddNacimiento">Fecha de Nacimiento</label>
                                <asp:TextBox ID="txtAddNacimiento" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddTelefono">Teléfono</label>
                                <asp:TextBox ID="txtAddTelefono" runat="server" CssClass="form-control" TextMode="Phone"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label" for="txtAddEmail">Email</label>
                                <asp:TextBox ID="txtAddEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3">
                            <label class="form-label" for="txtAddDireccion">Dirección</label>
                            <asp:TextBox ID="txtAddDireccion" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="modal-footer">

                            <asp:Literal ID="litModalError" runat="server" EnableViewState="false" />

                            <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>

                            <asp:Button ID="btnGuardarPaciente" runat="server"
                                Text="Guardar Paciente"
                                CssClass="btn btn-primary"
                                OnClick="btnGuardarPaciente_Click" />
                        </div>
                        <div aria-hidden="true" aria-labelledby="viewPatientModalLabel" class="modal fade" id="viewPatientModal" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="viewPatientModalLabel">Detalles del Paciente</h5>
                                        <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                                    </div>
                                    <div class="modal-body">
                                        <p><strong>Nombre:</strong> Carlos Rodríguez</p>
                                        <p><strong>DNI:</strong> 12.345.678</p>
                                        <p><strong>Fecha de Nacimiento:</strong> 15/05/1980</p>
                                        <p><strong>Teléfono:</strong> (11) 2345-6789</p>
                                        <p><strong>Email:</strong> carlos.rodriguez@example.com</p>
                                        <p><strong>Dirección:</strong> Av. Siempre Viva 742</p>
                                    </div>
                                    <div class="modal-footer">
                                        <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cerrar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div aria-hidden="true" aria-labelledby="editPatientModalLabel" class="modal fade" id="editPatientModal" tabindex="-1">
                            <div class="modal-dialog modal-dialog-centered modal-lg">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="editPatientModalLabel">Editar Información del Paciente</h5>
                                        <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                                    </div>
                                    <div class="modal-body">

                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editNombre">Nombre</label>
                                                <input class="form-control" id="editNombre" required="" type="text" value="Carlos" />
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editApellido">Apellido</label>
                                                <input class="form-control" id="editApellido" required="" type="text" value="Rodríguez" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editDNI">DNI</label>
                                                <input class="form-control" id="editDNI" required="" type="text" value="12.345.678" />
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editNacimiento">Fecha de Nacimiento</label>
                                                <input class="form-control" id="editNacimiento" type="date" value="1980-05-15" />
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editTelefono">Teléfono</label>
                                                <input class="form-control" id="editTelefono" type="tel" value="(11) 2345-6789" />
                                            </div>
                                            <div class="col-md-6 mb-3">
                                                <label class="form-label" for="editEmail">Email</label>
                                                <input class="form-control" id="editEmail" type="email" value="carlos.rodriguez@example.com" />
                                            </div>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label" for="editDireccion">Dirección</label>
                                            <input class="form-control" id="editDireccion" type="text" value="Av. Siempre Viva 742" />
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                                        <button class="btn btn-primary" type="button">Guardar Cambios</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                     </div>
                 </div>
             </div>
    </form>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
