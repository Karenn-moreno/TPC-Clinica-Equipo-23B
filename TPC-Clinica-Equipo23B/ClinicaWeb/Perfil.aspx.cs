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

    }
}