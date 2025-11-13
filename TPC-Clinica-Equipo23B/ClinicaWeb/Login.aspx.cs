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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            //Limpiar mensajes de error previos
            if (litErrorLogin != null)
                litErrorLogin.Text = "";

            // Validaciones a nivel de la capa de presentación (controles vacíos)
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (litErrorLogin != null)
                    litErrorLogin.Text = "Debe ingresar su correo electrónico y su contraseña.";
                return;
            }

            UsuarioNegocio negocio = new UsuarioNegocio();
            Usuario usuario;

            try
            {
                // Validar credenciales en la capa de negocio
                usuario = negocio.ValidarUsuario(txtEmail.Text, txtPassword.Text);

                if (usuario != null)
                {
                    // 4. Éxito: Iniciar sesión y redirigir
                    Session.Add("usuario", usuario);
                    Response.Redirect("GestionTurnos.aspx", false);
                }
                else
                {
                    // 5. Falla de validación: Credenciales incorrectas
                    if (litErrorLogin != null)
                        litErrorLogin.Text = "Usuario o contraseña incorrectos. Verifique sus credenciales e intente de nuevo.";
                }

            }
            catch (Exception ex)
            {
                //Manejo de errores de sistema (conexión, base de datos, etc.)
                if (litErrorLogin != null)
                    litErrorLogin.Text = "Hubo un error al intentar iniciar sesión. Por favor, intente más tarde.";

            }
        }
    }
}