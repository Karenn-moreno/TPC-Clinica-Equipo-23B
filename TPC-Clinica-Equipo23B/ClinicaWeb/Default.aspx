<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClinicaWeb.Default" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Clínica Sanare - Portal Administrativo</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="~/Content/Default.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com" rel="preconnect" />
    <link crossorigin="" href="https://fonts.gstatic.com" rel="preconnect" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;700;800&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark position-absolute w-100" style="z-index: 10;">
        <div class="container-fluid">
            <a class="navbar-brand d-flex align-items-center" href="#">
            <div class="svg-icon d-inline-block align-text-top me-2">
                <span class="material-symbols-outlined" style="font-size: 28px; color: #0d6efd;">
                    health_and_safety
                </span>
            </div>
            <span class="fs-4 fw-bold">Clinica Sanare</span>
        </a>
            <button aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation" class="navbar-toggler" data-bs-target="#navbarNav" data-bs-toggle="collapse" type="button">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse justify-content-end" id="navbarNav">
                <ul class="navbar-nav">
                    <li class="nav-item me-2">                    
                    </li>
                    <li class="nav-item">     
                    </li>
                </ul>
            </div>
        </div>
    </nav>
    <main class="flex-grow-1 hero-section">
        <div class="hero-content">
            <div class="card text-center shadow-lg p-4" style="max-width: 400px; width: 90%;">
                <div class="card-body">
                    <h5 class="card-title mb-4">Portal Administrativo</h5>
                    <p class="card-text mb-4">Accede o regístrate para gestionar la clínica Sanare.</p>
                    <div class="d-grid gap-3">
                       <asp:Button ID="btnAcceder" runat="server" Text="Acceder" CssClass="btn btn-primary btn-lg" OnClick="btnAcceder_Click" />
                <asp:Button ID="btnRegistrar" runat="server" Text="Registrarse" CssClass="btn btn-secondary btn-lg" OnClick="btnRegistrar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </main>
          </form>
    <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
