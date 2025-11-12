<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionMedicos.aspx.cs" Inherits="ClinicaWeb.GestionMedicos" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta content="width=device-width, initial-scale=1.0" name="viewport" />
    <title>Clínica Salud - Panel de Recepción</title>
    <link crossorigin="anonymous" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" rel="stylesheet" />
    <link href="~/Content/GestionMedicos.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com" rel="preconnect" />
    <link crossorigin="" href="https://fonts.gstatic.com" rel="preconnect" />
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined" rel="stylesheet" />

</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="d-flex">

             <aside class="sidebar bg-white border-end p-3 d-flex flex-column justify-content-between">
            <div>
                <div class="d-flex align-items-center gap-3 px-2 mb-4">
                    <img alt="Clinic logo" class="rounded-circle" src="https://lh3.googleusercontent.com/aida-public/AB6AXuDeAt9T3P2qjZpQj8lzu2zGENu6a5BQkzQtHA1xL-0Lcho4WiUK6Pny13lFsmCHjJ2gzEBYyWcXdDOtEp9qscD0DSuLvg9RWtTUo8QAP_lEZNqMwetgMh1_2z8c_n-jpRmr-YICTx6OruHIpyu6QscEDCyGdloskjEdVKU3Gw7VS2In2IdxNh-bd7rpVckTaFBuunzKao680qp3s9r4kylecoB4650yW-zaDTru_srBHQUrxPubNgEdLU80hNn9z0Pvp0NuogNBX_Ai" style="width: 40px; height: 40px; object-fit: cover;" />
                    <div>
                        <h1 class="h6 mb-0 fw-bold text-gray-900">Clínica Sanare</h1>
                        <p class="small text-muted mb-0">Panel de Recepción</p>
                    </div>
                </div>
                <nav class="nav flex-column nav-pills gap-1">
                    <a class="nav-link d-flex align-items-center gap-3" href="/GestionTurnos.aspx">
                        <span class="material-symbols-outlined">calendar_month</span>
                        <span>Turnos</span>
                    </a>
                    <a class="nav-link d-flex align-items-center gap-3" href="/GestionPacientes.aspx">
                        <span class="material-symbols-outlined">group</span>
                        <span>Pacientes</span>
                    </a>
                    <a class="nav-link d-flex align-items-center gap-3 active" href="#">
                        <span class="material-symbols-outlined">stethoscope</span>
                        <span>Médicos</span>
                    </a>
                </nav>
            </div>
            <div class="nav flex-column nav-pills gap-1">
                <a class="nav-link d-flex align-items-center gap-3" href="#">
                    <span class="material-symbols-outlined">account_circle</span>
                    <span>Mi Perfil</span>
                </a>
                <a class="nav-link d-flex align-items-center gap-3" href="/Default.aspx">
                    <span class="material-symbols-outlined">logout</span>
                    <span>Cerrar Sesión</span>
                </a>
            </div>
        </aside>
            <main class="main-content p-4 p-md-5">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-12">
                            <div class="card shadow-sm border-light-subtle">
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvMedicos" runat="server" 
                                            CssClass="table table-hover align-middle" 
                                            AutoGenerateColumns="false" 
                                            GridLines="None"
                                            EmptyDataText="No se encontraron médicos."
                                            OnRowCommand="gvMedicos_RowCommand"> 
                                            
                                            <HeaderStyle CssClass="table-light" />
                                            <Columns>
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                                                
                                                <asp:BoundField DataField="EspecialidadesTexto" HeaderText="Especialidad" />

                                                <asp:BoundField DataField="HorariosTexto" HeaderText="Horario" />
                                                
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-CssClass="text-end">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnHorarios" runat="server" 
                                                            CssClass="btn btn-sm btn-outline-primary me-1" 
                                                            CommandName="GestionarHorarios"
                                                            CommandArgument='<%# Eval("IdPersona") %>'>
                                                            <span class="material-symbols-outlined fs-6">calendar_month</span> Horarios
                                                        </asp:LinkButton>
                                                        
                                                        <asp:LinkButton ID="btnEditar" runat="server" 
                                                            CssClass="btn btn-sm btn-outline-secondary" 
                                                            CommandName="EditarMedico"
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

        </form> <script crossorigin="anonymous" integrity="sha384-YvpcrYf0tY3lHB60NNkmXc5s9fDVZLESaAA55NDzOxhy9GkcIdslK1eN7N6jIeHz" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

</body>
</html>
