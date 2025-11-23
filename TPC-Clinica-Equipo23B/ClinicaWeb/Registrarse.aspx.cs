using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class Registrarse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (litErrorRegistro != null)
                    litErrorRegistro.Text = "";

                CargarRoles();
            }

            if (ddlRol.SelectedValue == "2")
            {
                divMatricula.Visible = true;
            }
            else
            {
                divMatricula.Visible = false;
            }
        }

        private void CargarRoles()
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            List<Rol> listaRoles = negocio.ListarRoles();

            ddlRol.DataSource = listaRoles;
            ddlRol.DataTextField = "TipoRol";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new ListItem("Seleccione un Rol", "0"));
        }

        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMatricula.Text = "";
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (litErrorRegistro != null)
                litErrorRegistro.Text = "";

            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(firstName.Text))
                errores.Add("<li>El campo Nombre es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(lastName.Text))
                errores.Add("<li>El campo Apellido es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(idDocument.Text))
                errores.Add("<li>El campo Documento (DNI) es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(email.Text))
                errores.Add("<li>El campo Correo electrónico es obligatorio.</li>");
            if (ddlRol.SelectedValue == "0")
                errores.Add("<li>Debe seleccionar un Tipo de Usuario (Rol).</li>");
            if (string.IsNullOrWhiteSpace(password.Text))
                errores.Add("<li>El campo Contraseña es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(passwordConfirm.Text))
                errores.Add("<li>El campo Confirmar Contraseña es obligatorio.</li>");
            if (password.Text != passwordConfirm.Text)
                errores.Add("<li>Las contraseñas no coinciden.</li>");
            if (password.Text.Length > 0 && password.Text.Length < 6)
                errores.Add("<li>La contraseña debe tener al menos 6 caracteres.</li>");

            if (ddlRol.SelectedValue == "2" && string.IsNullOrWhiteSpace(txtMatricula.Text))
                errores.Add("<li>Para el rol Médico, el campo Matrícula es obligatorio.</li>");


            if (errores.Count > 0)
            {
                MostrarErrores(errores);
                return;
            }

  
            try
            {
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                string emailTrimmed = email.Text.Trim();
                string dniTrimmed = idDocument.Text.Trim();
                int idRol = int.Parse(ddlRol.SelectedValue);

                if (usuarioNegocio.ExisteUsuario(emailTrimmed, dniTrimmed))
                {
                    string mensaje = "Ya existe una cuenta registrada con el DNI o Correo electrónico proporcionado. Por favor, intente <a href=\"Login.aspx\" class=\"alert-link\">iniciar sesión</a>.";
                    if (litErrorRegistro != null)
                        litErrorRegistro.Text = "<div class='alert alert-warning'>" + mensaje + "</div>";
                    return;
                }

                Persona nuevaPersona = new Persona
                {
                    Nombre = firstName.Text.Trim(),
                    Apellido = lastName.Text.Trim(),
                    Dni = dniTrimmed,
                    Email = emailTrimmed,
                    Telefono = phone.Text.Trim(),
                    Localidad = address.Text.Trim()
                };

                string matricula = (idRol == 2) ? txtMatricula.Text.Trim() : null;
                usuarioNegocio.RegistrarNuevoUsuarioConRol(nuevaPersona, password.Text.Trim(), idRol, matricula);

                Session.Add("registroExitoso", "¡Registro completado! Ahora puede iniciar sesión con su correo y contraseña.");
                Response.Redirect("Login.aspx", false);

            }
            catch (Exception ex)
            {
                List<string> errorFatal = new List<string> { "Ocurrió un error al intentar registrarse. Por favor, intente más tarde. Detalles: " + ex.Message };
                MostrarErrores(errorFatal);
            }
        }

        private void MostrarErrores(List<string> errores)
        {
            if (litErrorRegistro != null)
            {
                litErrorRegistro.Text = "<div class='alert alert-danger'><p>Por favor, corrija los siguientes errores:</p><ul style='list-style-type: disc; margin-left: 20px;'>" + string.Join("", errores) + "</ul></div>";
            }
        }
}
}