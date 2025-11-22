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
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
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
                        <!--Botón para agregar paciente -->
                        <div class="d-flex justify-content-between align-items-center mb-5">
                            <div class="w-75 me-3">
                                <h3 class="fw-bold">Gestión de Pacientes</h3>
                                <p class="text-muted mb-0">
                                    Este espacio permite la gestión integral de los registros de pacientes, incluyendo su creación, consulta, actualización y eliminación, 
                           garantizando un manejo seguro y ordenado de la información clínica
                                </p>
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
                                            <div class="mb-4">
                                                <div class="input-group">
                                                    <!-- Campo para el DNI -->
                                                    <asp:TextBox ID="txtFiltroDNI" runat="server"
                                                        CssClass="form-control"
                                                        placeholder="Buscar paciente por número de DNI..."
                                                        MaxLength="10">
                                             </asp:TextBox>
                                                    <!-- Botón de búsqueda -->
                                                    <asp:LinkButton ID="btnBuscar" runat="server"
                                                        CssClass="btn btn-secondary"
                                                        CommandName="FiltrarPacientes"
                                                        OnClick="btnBuscar_Click">
                                                <span class="material-symbols-outlined">search</span>
                                          </asp:LinkButton>
                                                </div>
                                            </div>
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
                                                                    CommandArgument='<%# Eval("IdPersona") %>'
                                                                    CausesValidation="false">
                                                <span class="material-symbols-outlined fs-6">visibility</span>
                                                                </asp:LinkButton>

                                                                <asp:LinkButton ID="btnEditar" runat="server"
                                                                    CssClass="btn btn-sm btn-outline-primary"
                                                                    CommandName="EditarPaciente"
                                                                    CommandArgument='<%# Eval("IdPersona") %>'
                                                                    CausesValidation="false">
                                                <span class="material-symbols-outlined fs-6">edit</span>
                                                                </asp:LinkButton>

                                                                <asp:LinkButton ID="btnBorrar" runat="server"
                                                                    CssClass="btn btn-sm btn-outline-danger"
                                                                    OnClientClick='<%# "abrirModal(" + Eval("IdPersona") + "); return false;" %>'>                                                          
                                                           <span class="material-symbols-outlined fs-6">delete</span>
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
                <asp:HiddenField ID="hfIdPacienteEliminar" runat="server" />
                <div class="modal fade" id="modalConfirmarEliminar" tabindex="-1" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header bg-danger text-white">
                                <h5 class="modal-title">
                                    <span class="material-symbols-outlined align-middle me-2">warning</span>
                                    Confirmar Eliminación
                                </h5>
                                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                            </div>
                            <div class="modal-body">
                                <p>¿Está seguro que desea eliminar este registro de forma permanente?</p>
                                <p class="text-muted small">Esta acción no se puede deshacer.</p>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Eliminar"
                                    CssClass="btn btn-danger" OnClick="btnConfirmarEliminar_Click"
                                    CausesValidation="false"
                                    formnovalidate="" />
                            </div>
                        </div>
                    </div>
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
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hfIdPacienteEditar" runat="server" />
                <div aria-hidden="true" aria-labelledby="viewPatientModalLabel" class="modal fade" id="viewPatientModal" tabindex="-1">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="viewPatientModalLabel">Detalles del Paciente</h5>
                                <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                            </div>
                            <div class="modal-body">
                                <p>
                                    <strong>Nombre:</strong>
                                    <asp:Label ID="lblVerNombre" runat="server" />
                                </p>
                                <p>
                                    <strong>Apellido:</strong>
                                    <asp:Label ID="lblVerApellido" runat="server" />
                                </p>
                                <p>
                                    <strong>DNI:</strong>
                                    <asp:Label ID="lblVerDNI" runat="server" />
                                </p>
                                <p>
                                    <strong>Fecha de Nacimiento:</strong>
                                    <asp:Label ID="lblVerNacimiento" runat="server" />
                                </p>
                                <p>
                                    <strong>Teléfono:</strong>
                                    <asp:Label ID="lblVerTelefono" runat="server" />
                                </p>
                                <p>
                                    <strong>Email:</strong>
                                    <asp:Label ID="lblVerEmail" runat="server" />
                                </p>
                                <p>
                                    <strong>Dirección:</strong>
                                    <asp:Label ID="lblVerDireccion" runat="server" />
                                </p>
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
                                        <label class="form-label">Nombre</label>
                                        <asp:TextBox ID="txtEditNombre" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Apellido</label>
                                        <asp:TextBox ID="txtEditApellido" runat="server" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">DNI</label>
                                        <asp:TextBox ID="txtEditDNI" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Fecha de Nacimiento</label>
                                        <asp:TextBox ID="txtEditNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Teléfono</label>
                                        <asp:TextBox ID="txtEditTelefono" runat="server" CssClass="form-control" />
                                    </div>
                                    <div class="col-md-6 mb-3">
                                        <label class="form-label">Email</label>
                                        <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Dirección</label>
                                    <asp:TextBox ID="txtEditDireccion" runat="server" CssClass="form-control" />
                                </div>
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                                <asp:Button ID="btnGuardarEdicion" runat="server" Text="Guardar Cambios"
                                    CssClass="btn btn-primary" OnClick="btnGuardarEdicion_Click"
                                    CausesValidation="false"
                                    formnovalidate="" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        function limpiarFondosResiduales() {
            var backdrops = document.querySelectorAll('.modal-backdrop');
            backdrops.forEach(function (backdrop) {
                backdrop.remove();
            });
            document.body.classList.remove('modal-open');
            document.body.style.removeProperty('padding-right');
            document.body.style.removeProperty('overflow');
        }

        function abrirModal(idPaciente) {
            limpiarFondosResiduales();
            var hiddenField = document.getElementById('<%= hfIdPacienteEliminar.ClientID %>');
            hiddenField.value = idPaciente;
            var myModal = new bootstrap.Modal(document.getElementById('modalConfirmarEliminar'));
            myModal.show();
        }
        function abrirModalEditar() {
            limpiarFondosResiduales();
            var myModal = new bootstrap.Modal(document.getElementById('editPatientModal'));
            myModal.show();
        }
        function abrirModalVer() {
            limpiarFondosResiduales();
            var myModal = new bootstrap.Modal(document.getElementById('viewPatientModal'));
            myModal.show();
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function (sender, args) {
            limpiarFondosResiduales();
        });
</script>
</body>
</html>