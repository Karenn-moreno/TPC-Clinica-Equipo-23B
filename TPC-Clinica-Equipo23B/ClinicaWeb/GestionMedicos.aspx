<%@ Page Title="Gestión de Médicos" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="ClinicaWeb.GestionMedicos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/GestionMedicos.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!--agrega updatePanel -->
    <asp:UpdatePanel ID="UpdatePanelMedico" runat="server">
        <ContentTemplate>

            <main class="main-content p-4 p-md-5">
                <!-- Header -->
                <div class="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-5">
                    <div class="w-75 me-3">
                        <h1 class="text-dark h3 fw-bold mb-2">Gestión de Médicos</h1>
                        <p class="text-secondary mb-0">Administra los médicos de la clínica de forma rápida y sencilla.</p>
                    </div>
                    <button class="btn btn-primary btn-sm d-flex align-items-center gap-2"
                        onclick="abrirModalAgregarEditar();" type="button">
                        <span class="material-symbols-outlined">add</span>
                        <span>Nuevo Médico</span>
                    </button>
                </div>
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="card shadow-sm border-light-subtle">
                                <div class="card-body">

                                    <div class="mb-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                                                placeholder="Buscar por nombre, apellido o especialidad..."></asp:TextBox>
                                            <asp:LinkButton ID="btnBuscar" runat="server" CssClass="btn btn-secondary"
                                                OnClick="btnBuscar_Click"> 
                                                <span class="material-symbols-outlined">search</span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <!-- Grilla de Médicos -->
                                    <div class="card card-custom p-4 rounded-xl">
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvMedicos" runat="server" AutoGenerateColumns="false"
                                                CssClass="table table-hover align-middle" GridLines="None"
                                                EmptyDataText="No se encontraron médicos."
                                                OnRowCommand="gvMedicos_RowCommand">

                                                <HeaderStyle CssClass="table-light" />

                                                <Columns>
                                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                    <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                                    <asp:BoundField DataField="EspecialidadesTexto" HeaderText="Especialidad" />
                                                    <asp:BoundField DataField="HorariosTexto" HeaderText="Horario" HtmlEncode="false" />

                                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                                        <ItemTemplate>
                                                            <!-- VER DETALLES -->
                                                            <asp:LinkButton ID="btnVerDetalles" runat="server"
                                                                CssClass="btn btn-sm btn-outline-info me-2"
                                                                CommandName="VerDetallesMedico"
                                                                CommandArgument='<%# Eval("IdPersona") %>'> 
    <span class="material-symbols-outlined fs-6">visibility</span>
                                                            </asp:LinkButton>

                                                            <!-- EDITAR -->
                                                            <asp:LinkButton ID="btnEditar" runat="server"
                                                                CssClass="btn btn-sm btn-outline-secondary"
                                                                CommandName="EditarMedico"
                                                                CommandArgument='<%# Eval("IdPersona") %>'>
                                    <span class="material-symbols-outlined fs-6">edit</span>
                                                            </asp:LinkButton>

                                                            <!-- ELIMINAR -->
                                                            <asp:LinkButton ID="btnEliminar" runat="server"
                                                                CssClass="btn btn-sm btn-outline-danger ms-2"
                                                                OnClientClick='<%# "abrirModalEliminar(" + Eval("IdPersona") + "); return false;" %>'>
                                            <span class="material-symbols-outlined fs-6">delete</span>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>

                                <asp:HiddenField ID="hfIdMedicoEliminar" runat="server" />

                                <asp:HiddenField ID="HiddenField1" runat="server" />

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
                                                <p>¿Está seguro que desea dar de baja este registro?</p>
                                                <p class="text-muted small">Esta accion deshabilitara el registro</p>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>

                                                <asp:Button ID="btnConfirmarEliminar" runat="server" Text="Dar de Baja"
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

                                <!-- Modal Agregar / Editar Médico -->
                                <div class="modal fade" id="addMedicoModal" tabindex="-1" aria-labelledby="addMedicoModalLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-dialog-centered modal-lg">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="addMedicoModalLabel">Agregar / Editar Médico</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                            </div>
                                            <asp:UpdatePanel ID="upModalMedico" runat="server">
                                                <ContentTemplate>
                                                    <div class="modal-body">
                                                        <asp:HiddenField ID="hfIdMedicoEditar" runat="server" />
                                                        <!-- Formulario -->
                                                        <div class="row">
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">Nombre</label>
                                                                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">Apellido</label>
                                                                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">DNI</label>
                                                                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">Matrícula</label>
                                                                <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">Email</label>
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-6 mb-3">
                                                                <label class="form-label">Teléfono</label>
                                                                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="mb-3">
                                                            <label class="form-label">Especialidades</label>
                                                            <asp:CheckBoxList ID="chkEspecialidades" runat="server" RepeatDirection="Vertical" CssClass="form-control"></asp:CheckBoxList>
                                                        </div>

                                                        <hr />
                                                        <h5>Horarios de Atención</h5>
                                                        <div class="row mb-3">
                                                            <div class="col-md-4 mb-3">
                                                                <label class="form-label">Día</label>
                                                                <asp:DropDownList ID="ddlDiaLaboral" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Text="Lunes" Value="Lunes" />
                                                                    <asp:ListItem Text="Martes" Value="Martes" />
                                                                    <asp:ListItem Text="Miércoles" Value="Miercoles" />
                                                                    <asp:ListItem Text="Jueves" Value="Jueves" />
                                                                    <asp:ListItem Text="Viernes" Value="Viernes" />
                                                                    <asp:ListItem Text="Sábado" Value="Sabado" />
                                                                    <asp:ListItem Text="Domingo" Value="Domingo" />
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <label class="form-label">Hora Inicio</label>
                                                                <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" placeholder="HH:MM"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <label class="form-label">Hora Fin</label>
                                                                <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" placeholder="HH:MM"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2 d-flex align-items-end">
                                                                <asp:Button ID="btnAgregarHorario" runat="server" Text="Agregar" CssClass="btn btn-secondary btn-sm" OnClick="btnAgregarHorario_Click" />
                                                            </div>
                                                        </div>

                                                        <div class="table-responsive">
                                                            <asp:GridView ID="gvHorariosTemp" runat="server" AutoGenerateColumns="false" CssClass="table table-sm" OnRowCommand="gvHorariosTemp_RowCommand">
                                                                <Columns>
                                                                    <asp:BoundField DataField="DiaLaboral" HeaderText="Día" />
                                                                    <asp:BoundField DataField="HorarioInicio" HeaderText="Inicio" DataFormatString="{0:hh\\:mm}" />
                                                                    <asp:BoundField DataField="HoraFin" HeaderText="Fin" DataFormatString="{0:hh\\:mm}" />
                                                                    <asp:ButtonField Text="Eliminar" CommandName="Eliminar" ButtonType="Button" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                    <div class="modal-footer">
                                                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                                        <asp:Button ID="btnGuardarMedico" runat="server" Text="Guardar Médico" CssClass="btn btn-primary" OnClick="btnGuardarMedico_Click" UseSubmitBehavior="false" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <!-- Modal Ver Detalles -->

                                <div class="modal fade" id="modalDetallesMedico" tabindex="-1" aria-labelledby="modalDetallesMedicoLabel" aria-hidden="true">
                                    <div class="modal-dialog modal-dialog-centered modal-lg">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="modalDetallesMedicoLabel">Detalles del Médico</h5>
                                                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                                            </div>
                                            <div class="modal-body">
                                                <div class="row mb-2">
                                                    <div class="col-md-6">
                                                        <strong>Nombre:</strong>
                                                        <asp:Label ID="lblNombre" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <strong>Apellido:</strong>
                                                        <asp:Label ID="lblApellido" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row mb-2">
                                                    <div class="col-md-6">
                                                        <strong>DNI:</strong>
                                                        <asp:Label ID="lblDni" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <strong>Matrícula:</strong>
                                                        <asp:Label ID="lblMatricula" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row mb-2">
                                                    <div class="col-md-6">
                                                        <strong>Email:</strong>
                                                        <asp:Label ID="lblEmail" runat="server" />
                                                    </div>
                                                    <div class="col-md-6">
                                                        <strong>Teléfono:</strong>
                                                        <asp:Label ID="lblTelefono" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row mb-2">
                                                    <div class="col-md-12">
                                                        <strong>Especialidades:</strong>
                                                        <asp:Label ID="lblEspecialidades" runat="server" />
                                                    </div>
                                                </div>
                                                <div class="row mb-2">
                                                    <div class="col-md-12">
                                                        <strong>Horarios:</strong>
                                                        <asp:Label ID="lblHorarios" runat="server" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="modal-footer">
                                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Scripts JavaScript -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
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

    function abrirModalAgregarEditar() {
        limpiarFondosResiduales();
        var myModal = new bootstrap.Modal(document.getElementById('addMedicoModal'));
        myModal.show();
    }

    function abrirModalVerDetalles() {
        limpiarFondosResiduales();
        var myModal = new bootstrap.Modal(document.getElementById('modalDetallesMedico'));
        myModal.show();
    }

    function abrirModalEliminar(idMedico) {
        limpiarFondosResiduales();
        var hiddenField = document.getElementById('<%= hfIdMedicoEliminar.ClientID %>');
        if (hiddenField) {
            hiddenField.value = idMedico;
        }
        var myModal = new bootstrap.Modal(document.getElementById('modalConfirmarEliminar'));
        myModal.show();
    }

    function mostrarMensajeError(mensaje) {
        var labelError = document.getElementById('mensajeErrorBody');
        if (labelError) {
            labelError.innerText = mensaje;
        }
        var myModal = new bootstrap.Modal(document.getElementById('errorModal'));
        myModal.show();
    }

    // Lógica para el UpdatePanel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function (sender, args) {
        limpiarFondosResiduales();
    });
</script>
</asp:Content>
