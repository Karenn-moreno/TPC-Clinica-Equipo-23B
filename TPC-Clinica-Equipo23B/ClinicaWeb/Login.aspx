<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ClinicaWeb.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Inicio de Sesión - Portal Clínico</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="~/Content/Login.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@200..800&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
</head>
<body>
    <form id="form2" runat="server">
        <div class="container-fluid login-container">
            <div class="px-4 py-5">
                <div class="login-card card p-4 p-md-5">
                    <div class="card-body">
                        <div class="text-center mb-5">
                            <div class="d-inline-flex align-items-center gap-2">
                                <span class="material-symbols-outlined logo-icon">health_and_safety</span>
                            </div>
                        </div>
                        <div class="text-center mb-4">
                            <h1 class="h3 mb-2 fw-bold">Inicio de Sesión del Portal</h1>
                            <p class="text-muted">Bienvenido/a, por favor ingrese sus datos.</p>
                            <asp:Literal ID="litMensajeRegistro" runat="server" EnableViewState="false" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label fw-medium" for="txtEmail">Correo electrónico</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-lg" TextMode="Email" placeholder="Ingrese su correo electrónico"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label class="form-label fw-medium"
                                for="txtPassword">
                                Contraseña</label>
                            <div class="input-group input-group-lg">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Ingrese su contraseña"></asp:TextBox>
                                <button class="btn btn-outline-secondary" type="button" id="btnTogglePassword">
                                    <span class="material-symbols-outlined" id="iconTogglePassword">visibility_off</span>
                                </button>
                            </div>
                        </div>
                        <div class="mb-3">
                            <asp:Literal ID="litErrorLogin" runat="server" EnableViewState="false" />
                        </div>
                        <div class="d-grid">
                            <asp:Button ID="btnIngresar" runat="server" CssClass="btn btn-primary btn-lg fw-semibold" Text="Iniciar sesión" OnClick="btnIngresar_Click" />
                        </div>
                    </div>
                </div>
                <div class="text-center mt-4">
                    <p class="text-muted small">© 2024 ClinicaSanare. Todos los derechos reservados.</p>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var passwordInput = document.getElementById('<%= txtPassword.ClientID %>');
            var toggleButton = document.getElementById('btnTogglePassword');
            var toggleIcon = document.getElementById('iconTogglePassword');

            if (toggleButton && passwordInput && toggleIcon) {
                toggleButton.addEventListener('click', function () {
                    var currentType = passwordInput.getAttribute('type');
                    var newType = currentType === 'password' ? 'text' : 'password';

                
                    passwordInput.setAttribute('type', newType);
                    toggleIcon.innerText = (newType === 'text') ? 'visibility' : 'visibility_off';
                });
            }
        });
    </script>

    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
