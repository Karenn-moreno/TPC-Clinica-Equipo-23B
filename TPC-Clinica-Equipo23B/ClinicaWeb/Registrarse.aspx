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
        <div class="container-fluid login-container">
            <div class="px-4 py-5">
                <div class="login-card card p-4 p-md-5 shadow-lg" style="max-width: 500px; margin:auto;">
                    <div class="card-body">
                        <div class="text-center mb-5">
                            <div class="d-inline-flex align-items-center gap-2">
                                <span class="material-symbols-outlined logo-icon" style="font-size:48px; color:#0d6efd;">health_and_safety</span>       
                            </div>
                        </div>

                        <div class="text-center mb-4">
                            <h1 class="h3 mb-2 fw-bold">Registro del Portal</h1>
                            <p class="text-muted">Complete sus datos para crear una cuenta.</p>
                        </div>

                        <div class="row g-3">
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Nombre</label>
                                <asp:TextBox ID="firstName" runat="server" CssClass="form-control form-control-lg" placeholder="Tu nombre"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Apellido</label>
                                <asp:TextBox ID="lastName" runat="server" CssClass="form-control form-control-lg" placeholder="Tu apellido"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Fecha de nacimiento</label>
                                <asp:TextBox ID="birthDate" runat="server" CssClass="form-control form-control-lg" TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-medium">Documento</label>
                                <asp:TextBox ID="idDocument" runat="server" CssClass="form-control form-control-lg" placeholder="Ej: DNI 12345678"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Género</label>
                                <asp:DropDownList ID="gender" runat="server" CssClass="form-select form-select-lg">
                                    <asp:ListItem Text="Seleccionar..." />
                                    <asp:ListItem Text="Masculino" />
                                    <asp:ListItem Text="Femenino" />
                                    <asp:ListItem Text="Prefiero no decirlo" />
                                </asp:DropDownList>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Correo electrónico</label>
                                <asp:TextBox ID="email" runat="server" CssClass="form-control form-control-lg" TextMode="Email" placeholder="tu@correo.com"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Teléfono</label>
                                <asp:TextBox ID="phone" runat="server" CssClass="form-control form-control-lg" TextMode="Phone" placeholder="011-12345678"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Dirección</label>
                                <asp:TextBox ID="address" runat="server" CssClass="form-control form-control-lg" TextMode="MultiLine" Rows="3" placeholder="Tu dirección completa"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Contraseña</label>
                                <asp:TextBox ID="password" runat="server" CssClass="form-control form-control-lg" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="form-label fw-medium">Confirmar contraseña</label>
                                <asp:TextBox ID="passwordConfirm" runat="server" CssClass="form-control form-control-lg" TextMode="Password" placeholder="••••••••"></asp:TextBox>
                            </div>

                            <div class="col-12 d-grid mt-3">
                                <asp:Button ID="btnRegistrarse" runat="server" CssClass="btn btn-primary btn-lg fw-semibold" Text="Registrarse" OnClick="btnRegistrarse_Click" />
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
    </form>

    <script crossorigin="anonymous" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>