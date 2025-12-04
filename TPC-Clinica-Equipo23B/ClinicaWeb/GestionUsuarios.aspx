<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GestionUsuarios.aspx.cs" Inherits="ClinicaWeb.GestionUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionUsuarios.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Literal ID="litErrorRol" runat="server" EnableViewState="false"></asp:Literal>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <main class="main-content p-4 p-md-5">
                <div class="d-flex justify-content-between align-items-center mb-5">
                    <div class="w-75 me-3">
                        <h3 class="fw-bold">Gestión de Usuarios</h3>
                        <p class="text-muted mb-0">Gestión de usuarios administrativos, médicos y recepcionistas.</p>
                    </div>

                    <a href="Registrarse.aspx" class="btn btn-primary btn-sm d-flex align-items-center gap-2" role="button">
                        <span class="material-symbols-outlined">add</span>
                        <span>Nuevo Usuario</span>
                    </a>
                </div>

                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="card shadow-sm border-light-subtle">
                                <div class="card-body">

                                    <div class="mb-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFiltroEmail" runat="server"
                                                CssClass="form-control"
                                                placeholder="Buscar usuario por Email..."
                                                TextMode="Email">
                                            </asp:TextBox>
                                            <asp:LinkButton ID="btnBuscar" runat="server"
                                                CssClass="btn btn-secondary"
                                                OnClick="btnBuscar_Click">
                                                <span class="material-symbols-outlined">search</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvUsuarios" runat="server"
                                            CssClass="table table-hover align-middle"
                                            AutoGenerateColumns="false"
                                            GridLines="None"
                                            EmptyDataText="No se encontraron usuarios."
                                            OnRowCommand="gvUsuarios_RowCommand">

                                            <HeaderStyle CssClass="table-light" />
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                                <asp:BoundField DataField="RolNombre" HeaderText="Rol" />
                                                
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnVer" runat="server"
                                                            CssClass="btn btn-sm btn-outline-secondary me-1"
                                                            CommandName="VerUsuario"
                                                            CommandArgument='<%# Eval("IdPersona") %>'
                                                            CausesValidation="false">
                                                            <span class="material-symbols-outlined fs-6">visibility</span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnEditar" runat="server"
                                                            CssClass="btn btn-sm btn-outline-primary"
                                                            CommandName="EditarUsuario"
                                                            CommandArgument='<%# Eval("IdPersona") %>'
                                                            CausesValidation="false">
                                                            <span class="material-symbols-outlined fs-6">edit</span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton ID="btnBorrar" runat="server"
                                                            CssClass="btn btn-sm btn-outline-danger ms-1"
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

                            <asp:HiddenField ID="hfIdUsuarioEliminar" runat="server" />
                            <asp:HiddenField ID="hfIdUsuarioEditar" runat="server" />

                            <div class="modal fade" id="modalConfirmarEliminar" tabindex="-1" aria-hidden="true">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header bg-danger text-white">
                                            <h5 class="modal-title">
                                                <span class="material-symbols-outlined align-middle me-2">warning</span>
                                                Confirmar Baja
                                            </h5>
                                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <p>¿Está seguro que desea dar de baja a este usuario?</p>
                                            <p class="text-muted small">Esta acción deshabilitara el registro</p>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                            <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Eliminar"
                                                CssClass="btn btn-danger" OnClick="btnConfirmarEliminar_Click"
                                                CausesValidation="false" UseSubmitBehavior="false" formnovalidate="" />
                                            <asp:Button ID="btnEliminarFisico" runat="server" Text="Eliminar Definitivamente"
                                                CssClass="btn btn-danger" OnClick="btnEliminarFisico_Click"
                                                Style="display: none"
                                                CausesValidation="false" UseSubmitBehavior="false" formnovalidate="" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div aria-hidden="true" aria-labelledby="editUserModalLabel" class="modal fade" id="editUserModal" tabindex="-1">
                                <div class="modal-dialog modal-dialog-centered modal-lg">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="editUserModalLabel">Editar Información del Usuario</h5>
                                            <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                                        </div>
                                        <div class="modal-body">
                                            
                                            <div class="mb-3">
                                                <label class="form-label">Tipo de Usuario (Rol)</label>
                                                <asp:TextBox ID="txtEditRol" runat="server" CssClass="form-control" ReadOnly="true" />
                                            </div>

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
                                                    <label class="form-label">Email</label>
                                                    <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                                </div>
                                            </div>
                                            
                                            <div class="row">
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Teléfono</label>
                                                    <asp:TextBox ID="txtEditTelefono" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <label class="form-label">Dirección</label>
                                                    <asp:TextBox ID="txtEditLocalidad" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cancelar</button>
                                            <asp:Button ID="btnGuardarEdicion" runat="server" Text="Guardar Cambios"
                                                CssClass="btn btn-primary" OnClick="btnGuardarEdicion_Click"
                                                CausesValidation="false" formnovalidate="" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div aria-hidden="true" aria-labelledby="viewUserModalLabel" class="modal fade" id="viewUserModal" tabindex="-1">
                                <div class="modal-dialog modal-dialog-centered">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="viewUserModalLabel">Detalles del Usuario</h5>
                                            <button aria-label="Close" class="btn-close" data-bs-dismiss="modal" type="button"></button>
                                        </div>
                                        <div class="modal-body">
                                            <p><strong>Rol:</strong> <asp:Label ID="lblVerRol" runat="server" CssClass="badge bg-primary" /></p>
                                            <p><strong>Nombre:</strong> <asp:Label ID="lblVerNombre" runat="server" /></p>
                                            <p><strong>Apellido:</strong> <asp:Label ID="lblVerApellido" runat="server" /></p>
                                            <p><strong>DNI:</strong> <asp:Label ID="lblVerDNI" runat="server" /></p>
                                            <p><strong>Email:</strong> <asp:Label ID="lblVerEmail" runat="server" /></p>
                                            <p><strong>Teléfono:</strong> <asp:Label ID="lblVerTelefono" runat="server" /></p>
                                            <p><strong>Localidad:</strong> <asp:Label ID="lblVerLocalidad" runat="server" /></p>
                                        </div>
                                        <div class="modal-footer">
                                            <button class="btn btn-secondary" data-bs-dismiss="modal" type="button">Cerrar</button>
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
                        </div>
                    </div>
                </div>
            </main>
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
        function abrirModal(idUsuario) {
            limpiarFondosResiduales();
            var hiddenField = document.getElementById('<%= hfIdUsuarioEliminar.ClientID %>');
            hiddenField.value = idUsuario;
            var myModal = new bootstrap.Modal(document.getElementById('modalConfirmarEliminar'));
            myModal.show();
        }
        function abrirModalEditar() {
            limpiarFondosResiduales();
            var myModal = new bootstrap.Modal(document.getElementById('editUserModal'));
            myModal.show();
        }
        function abrirModalVer() {
            limpiarFondosResiduales();
            var myModal = new bootstrap.Modal(document.getElementById('viewUserModal'));
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