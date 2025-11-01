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
                    </div>
                    
                        <div class="mb-3">
                            <label class="form-label fw-medium" for="emailInput">Correo electrónico</label>
                            <input class="form-control form-control-lg" id="emailInput" placeholder="Ingrese su correo electrónico" type="email" />
                        </div>
                        <div class="mb-3">
                            <label class="form-label fw-medium" for="passwordInput">Contraseña</label>
                            <input class="form-control form-control-lg" id="passwordInput" placeholder="Ingrese su contraseña" type="password" />
                        </div>
                        <div class="text-end mb-4">
                            <a class="small" href="#">¿Olvidaste tu contraseña?</a>
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
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
