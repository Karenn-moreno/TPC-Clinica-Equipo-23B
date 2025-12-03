<%@ Page Title="Gestión de Pacientes" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GestionPacientes.aspx.cs" Inherits="ClinicaWeb.GestionPacientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionPacientes.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      <!-- msj error -->
    <asp:Literal ID="litErrorRol" runat="server" EnableViewState="false"></asp:Literal>

<asp:Panel ID="pnlContenido" runat="server">
    

</asp:Panel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <main class="main-content p-4 p-md-5">
                <!-- Descripcion y botón para agregar paciente -->
                <div class="d-flex justify-content-between align-items-center mb-5">
                    <div class="w-75 me-3">
                        <h3 class="fw-bold">Gestión de Pacientes</h3>
                        <p class="text-muted mb-0">
                            Este espacio permite la gestión integral de los registros de todos los pacientes.
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
                                            OnRowCommand="gvPacientes_RowCommand"
                                            OnRowDataBound="gvPacientes_RowDataBound">

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
                                            <p>¿Está seguro que desea eliminar este registro?</p>
                                            <p class="text-muted small">Esta acción deshabilitara el registro</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Eliminar"
                                                CssClass="btn btn-danger" OnClick="btnConfirmarEliminar_Click"
                                                CausesValidation="false"
                                                UseSubmitBehavior="false"
                                                formnovalidate="" />
                                            <asp:Button ID="btnEliminarFisico" runat="server" Text="Eliminar Definitivamente"
                                                CssClass="btn btn-danger" OnClick="btnEliminarFisico_Click"
                                                Style="display: none"
                                                CausesValidation="false"
                                                UseSubmitBehavior="false"
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
                            <div class="modal fade" id="errorModal" tabindex="-1" aria-labelledby="errorModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger text-white">
                                            <h5 class="modal-title" id="errorModalLabel">
                                                <span class="material-symbols-outlined align-middle me-2">error</span>
                                                 Atención
                                                </h5>
                                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <p id="mensajeErrorBody" class="fs-5 text-center"></p>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

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
        function mostrarMensajeError(mensaje) {

            document.getElementById('mensajeErrorBody').innerText = mensaje;


            var myModal = new bootstrap.Modal(document.getElementById('errorModal'));
            myModal.show();
        }
    </script>
</asp:Content>
