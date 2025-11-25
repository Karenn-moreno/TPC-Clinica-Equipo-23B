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
            if (!IsPostBack)
            {

                if (!IsPostBack)
                {

                    if (Session["registroExitoso"] != null)
                    {
                        if (litMensajeRegistro != null)
                        {
                            litMensajeRegistro.Text = $"<div class='alert alert-success mt-3' role='alert'>{Session["registroExitoso"]}</div>";
                            Session.Remove("registroExitoso");
                        }
                    }
                }

            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {

            if (litErrorLogin != null)
                litErrorLogin.Text = "";


            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                if (litErrorLogin != null)
                    litErrorLogin.Text = "Debe ingresar su correo electrónico y su contraseña.";
                return;
            }

            UsuarioNegocio negocio = new UsuarioNegocio();
            try
            {

                Usuario usuario = negocio.ValidarUsuario(txtEmail.Text, txtPassword.Text);

                if (usuario != null)
                {
                    Session.Add("usuario", usuario);
                    string rolNombre = "USUARIO";

                    if (usuario.UsuarioRoles != null && usuario.UsuarioRoles.Count > 0)
                    {
                        // Obtenemos el primer rol de la lista
                        var primerRol = usuario.UsuarioRoles.First().Rol;

                        // Usamos la propiedad TipoRol 
                        if (primerRol != null && !string.IsNullOrEmpty(primerRol.TipoRol))
                        {
                            rolNombre = primerRol.TipoRol.ToUpper();
                        }
                    }
                    Session.Add("rol", rolNombre);
                    Response.Redirect("GestionTurnos.aspx", false);
                }
                else
                {

                    if (litErrorLogin != null)
                        litErrorLogin.Text = "Usuario o contraseña incorrectos. Verifique sus credenciales e intente de nuevo.";
                }

            }
            catch (Exception ex)
            {
                if (litErrorLogin != null)
                    litErrorLogin.Text = "Hubo un error al intentar iniciar sesión. Por favor, intente más tarde.";

            }

        }
    }
}
