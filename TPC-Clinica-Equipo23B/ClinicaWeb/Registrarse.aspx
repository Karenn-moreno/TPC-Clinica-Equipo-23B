<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrarse.aspx.cs" Inherits="ClinicaWeb.Registrarse" %>

<!DOCTYPE html>

<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>Registro - Portal Clínico</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/Login.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@200..800&display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container-fluid login-container">
            <div class="px-4 py-5">
                <div class="login-card card p-4 p-md-5 shadow-lg" style="max-width: 800px; margin: auto;">
                    <div class="card-body">
                        <div class="text-center mb-5">
                            <div class="d-inline-flex align-items-center gap-2">
                                <span class="material-symbols-outlined logo-icon" style="font-size: 48px; color: #0d6efd;">health_and_safety</span>
                            </div>
                        </div>

                        <div class="text-center mb-4">
                            <h1 class="h3 mb-2 fw-bold">Registrar nuevo usuario</h1>
                            <p class="text-muted">Complete los datos para crear una cuenta.</p>
                        </div>

                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Nombre</label>
                                <asp:TextBox ID="firstName" runat="server" CssClass="form-control form-control-lg" placeholder="Ingrese el nombre"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Apellido</label>
                                <asp:TextBox ID="lastName" runat="server" CssClass="form-control form-control-lg" placeholder="Ingrese el apellido"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Fecha de nacimiento</label>
                                <asp:TextBox ID="birthDate" runat="server" CssClass="form-control form-control-lg" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Documento</label>
                                <asp:TextBox ID="idDocument" runat="server" CssClass="form-control form-control-lg" placeholder="Solo números, sin puntos"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Tipo de Usuario (Rol)</label>
                                <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control form-control-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlRol_SelectedIndexChanged" />
                            </div>
                            <div class="col-md-12">
                                <asp:UpdatePanel ID="upMatricula" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlDatosMedico" runat="server" Visible="false">
                                            <div class="row g-3 md-4">
                                                <div class="col-md-6">
                                                    <label class="form-label fw-medium">Matrícula</label>
                                                    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control form-control-lg" placeholder="MN 123456"></asp:TextBox>
                                                </div>
                                                <div class="col-md-6">
                                                    <label class="form-label fw-medium">Especialidades</label>
                                                    <div class="input-group">
                                                        <asp:ListBox ID="lbxEspecialidades" runat="server"
                                                            CssClass="form-control select-especialidad"
                                                            SelectionMode="Multiple"
                                                            Rows="1"></asp:ListBox>

                                                        <button class="btn btn-outline-secondary" type="button" data-bs-toggle="modal" data-bs-target="#modalNuevaEspecialidad">
                                                            <span class="material-symbols-outlined" style="font-size: 20px; vertical-align: middle;">add</span>
                                                        </button>
                                                    </div>
                                                    <small class="text-muted">Mantenga presionado (Ctrl+ click) para seleccionar varias</small>
                                                </div>



                                                <div class="col-12">
                                                    <hr class="my-4 text-muted" />
                                                    <h5 class="mb-3 text-secondary fw-bold">Disponibilidad Horaria</h5>

                                                    <div class="row g-3">
                                                        <!-- dia, values coinciden con Enum DiaLaboral -->
                                                        <div class="col-md-6 ">
                                                            <label class="small text-muted">Días (Ctrl + Click)</label>
                                                            <div style="width: 100%;">
                                                                <asp:ListBox ID="ListBox2" runat="server"
                                                                    CssClass="form-control select-especialidad"
                                                                    SelectionMode="Multiple"
                                                                    Rows="1"
                                                                    Style="width: 100%;"></asp:ListBox>
                                                            </div>
                                                            <!-- turno vacio -->
                                                            <div class="col-md-6">
                                                                <label class="small text-muted">Turno Base</label>
                                                                <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>

                                                            <!-- horas: editables -->

                                                            <div class="col-md-4">
                                                                <label class="form-label fw-medium">Desde</label>
                                                                <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <label class="form-label fw-medium">Hasta</label>
                                                                <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control" TextMode="Time"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4 d-flex align-items-end">
                                                                <asp:Button ID="btnAgregarHorario" runat="server" Text="Agregar(+)"
                                                                    CssClass="btn btn-secondary w-100" OnClick="btnAgregarHorario_Click" />
                                                            </div>

                                                        </div>

                                                        <!-- grilla de horarios agregados -->
                                                        <div class="table-responsive mt-4">
                                                            <asp:GridView ID="gvHorarios" runat="server"
                                                                CssClass="table table-sm table-bordered bg-white mb-0"
                                                                AutoGenerateColumns="false"
                                                                OnRowDeleting="gvHorarios_RowDeleting">
                                                                <Columns>
                                                                    <asp:BoundField DataField="DiaLaboral" HeaderText="Día" />
                                                                    <asp:BoundField DataField="IdTurnoTrabajo" HeaderText="Turno" />
                                                                    <asp:BoundField DataField="HorarioInicio" HeaderText="Inicio" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:BoundField DataField="HoraFin" HeaderText="Fin" ItemStyle-HorizontalAlign="Center" />
                                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Acción">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="btnEliminar" runat="server"
                                                                                CommandName="Delete"
                                                                                CssClass="text-danger text-decoration-none">
                                                                                    <span class="material-symbols-outlined" style="font-size: 18px;">delete</span>
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <div class="text-center text-muted p-2 small">No hay horarios asignados aún.</div>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </div>

                                                        <asp:Label ID="lblErrorHorario" runat="server" CssClass="text-danger small mt-2 d-block" Visible="false"></asp:Label>
                                                    </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlRol" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="btnGuardarEspecialidad" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Correo electrónico</label>
                                <asp:TextBox ID="email" runat="server" CssClass="form-control form-control-lg" TextMode="Email" placeholder="usuario@dominio.com"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Teléfono</label>
                                <asp:TextBox ID="phone" runat="server" CssClass="form-control form-control-lg" TextMode="Phone" placeholder="1172345678"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Dirección</label>
                                <asp:TextBox ID="address" runat="server" CssClass="form-control form-control-lg" TextMode="MultiLine" Rows="3" placeholder="Calle, Altura, Localidad"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Contraseña</label>
                                <asp:TextBox ID="password" runat="server" CssClass="form-control form-control-lg" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Confirmar contraseña</label>
                                <asp:TextBox ID="passwordConfirm" runat="server" CssClass="form-control form-control-lg" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <asp:Literal ID="litErrorRegistro" runat="server" EnableViewState="false" />
                            </div>
                            <div class="col-12 d-grid mt-3">
                                <asp:Button ID="btnRegistrarse" runat="server" CssClass="btn btn-primary btn-lg fw-semibold" Text="Registrar" OnClick="btnRegistrarse_Click" />
                            </div>
                        </div>

                        <p class="text-center mt-4 mb-0 text-muted">
                            ¿Ya tienes una cuenta? 
                           
                            <a href="Login.aspx" class="text-primary fw-semibold">Acceder</a>
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="modalNuevaEspecialidad" tabindex="-1" aria-labelledby="modalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabel">Nueva Especialidad</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="upModalEspecialidad" runat="server">
                            <ContentTemplate>
                                <div class="mb-3">
                                    <label for="txtNuevaEspecialidad" class="form-label">Nombre de la Especialidad</label>
                                    <asp:TextBox ID="txtNuevaEspecialidad" runat="server" CssClass="form-control" placeholder="Ej: Neurología"></asp:TextBox>

                                    <asp:RequiredFieldValidator ID="rfvEspecialidad" runat="server"
                                        ControlToValidate="txtNuevaEspecialidad"
                                        ErrorMessage="El nombre es obligatorio."
                                        ValidationGroup="NuevaEspecialidad"
                                        Display="Dynamic" CssClass="text-danger small mt-1" />
                                </div>
                                <div class="d-grid">
                                    <asp:Button ID="btnGuardarEspecialidad" runat="server" Text="Guardar Especialidad"
                                        CssClass="btn btn-primary"
                                        OnClick="btnGuardarEspecialidad_Click"
                                        ValidationGroup="NuevaEspecialidad" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script crossorigin="anonymous" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
