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
    public partial class Perfil : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarDatosPerfil();
            }
        }

        private void CargarDatosPerfil()
        {
            Usuario user = (Usuario)Session["usuario"];
            string rol = Session["rol"].ToString();

            lblNombre.Text = user.Nombre;
            lblApellido.Text = user.Apellido;
            lblDni.Text = user.Dni;
            lblEmail.Text = user.Email;
            lblTelefono.Text = string.IsNullOrEmpty(user.Telefono) ? "N/A" : user.Telefono;
            lblLocalidad.Text = string.IsNullOrEmpty(user.Localidad) ? "N/A" : user.Localidad;
            lblRol.Text = rol;

            if (rol.ToUpper() == "MEDICO")
            {
                pnlMedico.Visible = true;
                MedicoNegocio medicoNegocio = new MedicoNegocio();

                
                Medico medicoCompleto = medicoNegocio.Listar()
                    .FirstOrDefault(m => m.IdPersona == user.IdPersona);

                if (medicoCompleto != null)
                {
                    lblMatricula.Text = medicoCompleto.Matricula;
                    lblEspecialidades.Text = medicoCompleto.EspecialidadesTexto;
                
                    lblHorarios.Text = medicoCompleto.HorariosTexto.Replace(", ", "<br/>");
                }
            }
        }

        protected void btnGuardarPassword_Click(object sender, EventArgs e)
        {
            if (litErrorCambioPassword != null)
                litErrorCambioPassword.Text = "";

            Usuario user = (Usuario)Session["usuario"];
            UsuarioNegocio negocio = new UsuarioNegocio();
            List<string> errores = new List<string>();

            string currentPass = txtPasswordActual.Text.Trim();
            string newPass = txtPasswordNueva.Text.Trim();
            string confirmPass = txtPasswordConfirmacion.Text.Trim();

           
            if (string.IsNullOrEmpty(currentPass))
                errores.Add("<li>Debe ingresar su contraseña actual.</li>");
            if (string.IsNullOrEmpty(newPass))
                errores.Add("<li>Debe ingresar la nueva contraseña.</li>");
            if (string.IsNullOrEmpty(confirmPass))
                errores.Add("<li>Debe confirmar la nueva contraseña.</li>");
            if (newPass.Length > 0 && newPass.Length < 6)
                errores.Add("<li>La nueva contraseña debe tener al menos 6 caracteres.</li>");
            if (newPass != confirmPass)
                errores.Add("<li>La nueva contraseña y la confirmación no coinciden.</li>");


            if (errores.Any())
            {
                MostrarErrorPassword(errores);
                return;
            }

            try
            {
        
                if (!negocio.VerificarPassword(user.IdPersona, currentPass))
                {
                    errores.Add("<li>La contraseña actual ingresada es incorrecta.</li>");
                    MostrarErrorPassword(errores);
                    return;
                }

                
                negocio.ModificarPassword(user.IdPersona, newPass);

                Session.Clear();
                Session.Abandon();

                Session.Add("registroExitoso", "Contraseña modificada con éxito. Por favor, ingrese con su nueva contraseña.");
                Response.Redirect("Login.aspx", false);

            }
            catch (Exception ex)
            {
                errores.Add("<li>Ocurrió un error al intentar modificar la contraseña. Por favor, intente más tarde. Detalles: " + ex.Message + "</li>");
                MostrarErrorPassword(errores);
            }
        }

        private void MostrarErrorPassword(List<string> errores)
        {
            if (litErrorCambioPassword != null)
            {
                litErrorCambioPassword.Text = "<div class='alert alert-danger mb-3'><p class='fw-bold'>Por favor, corrija los siguientes errores:</p><ul style='list-style-type: disc; margin-left: 20px;'>" + string.Join("", errores) + "</ul></div>";

    
                string script = "var myModal = new bootstrap.Modal(document.getElementById('modalCambioPassword')); myModal.show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reabrirModalPass", script, true);
            }
        }

}
}