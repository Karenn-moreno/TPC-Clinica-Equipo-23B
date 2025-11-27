<%@ Page Title="Mi Perfil" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="ClinicaWeb.Perfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="d-flex flex-column gap-4">
        <div class="d-flex flex-wrap justify-content-between align-items-center gap-3">
            <div class="d-flex flex-column gap-1">
                <h1 class="text-dark h3 fw-bold mb-0">Mi Perfil</h1>
                <p class="text-secondary mb-0">Revisa y actualiza tu información personal.</p>
            </div>
        </div>

        <div class="card card-custom p-4 rounded-xl">
            <h2 class="h5 fw-bold text-dark mb-4">Información de la Cuenta</h2>

            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>Nombre:</strong>
                    <asp:Label ID="lblNombre" runat="server" />
                </div>
                <div class="col-md-6">
                    <strong>Apellido:</strong>
                    <asp:Label ID="lblApellido" runat="server" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>DNI:</strong>
                    <asp:Label ID="lblDni" runat="server" />
                </div>
                <div class="col-md-6">
                    <strong>Correo Electrónico:</strong>
                    <asp:Label ID="lblEmail" runat="server" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>Teléfono:</strong>
                    <asp:Label ID="lblTelefono" runat="server" />
                </div>
                <div class="col-md-6">
                    <strong>Localidad:</strong>
                    <asp:Label ID="lblLocalidad" runat="server" />
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-md-6">
                    <strong>Rol:</strong>
                    <asp:Label ID="lblRol" runat="server" CssClass="badge bg-primary" />
                </div>
            </div>

            <asp:Panel ID="pnlMedico" runat="server" Visible="false">
                <hr />
                <h2 class="h5 fw-bold text-dark mb-4">Información del Médico</h2>
                <div class="row mb-3">
                    <div class="col-md-6">
                        <strong>Matrícula:</strong>
                        <asp:Label ID="lblMatricula" runat="server" />
                    </div>
                    <div class="col-md-6">
                        <strong>Especialidades:</strong>
                        <asp:Label ID="lblEspecialidades" runat="server" />
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-12">
                        <strong>Horarios de Trabajo:</strong>
                        <asp:Label ID="lblHorarios" runat="server" />
                    </div>
                </div>
            </asp:Panel>

            <div class="mt-4">
                <asp:LinkButton ID="btnCambiarPassword" runat="server"
                    CssClass="btn btn-secondary btn-sm"
                    data-bs-toggle="modal"
                    data-bs-target="#modalCambioPassword"
                    CausesValidation="false"
                    UseSubmitBehavior="false"
                    formnovalidate="">Cambiar Contraseña</asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modalCambioPassword" tabindex="-1" aria-labelledby="modalCambioPasswordLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="modalCambioPasswordLabel">Cambiar Contraseña</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <asp:Literal ID="litErrorCambioPassword" runat="server" EnableViewState="false" />

                    <div class="mb-3">
                        <label class="form-label">Contraseña Actual</label>
                        <asp:TextBox ID="txtPasswordActual" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Nueva Contraseña</label>
                        <asp:TextBox ID="txtPasswordNueva" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Confirmar Nueva Contraseña</label>
                        <asp:TextBox ID="txtPasswordConfirmacion" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <asp:Button ID="btnGuardarPassword" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnGuardarPassword_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
