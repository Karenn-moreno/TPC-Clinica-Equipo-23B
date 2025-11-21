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
    public partial class GestionMedicos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrillaMedicos();
            }
        }

        private void CargarGrillaMedicos()
        {
            try
            {
                MedicoNegocio negocio = new MedicoNegocio();
                List<Medico> lista = negocio.Listar();
                gvMedicos.DataSource = lista;
                gvMedicos.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al cargar los médicos: " + ex.Message + "');</script>");
            }
        }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GestionarHorarios")
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"GestionHorarios.aspx?idmedico={idMedico}", false);
            }
            else if (e.CommandName == "EditarMedico")
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);
                // Aquí podés redirigir a un modal o página de edición
            }
        }

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                Medico nuevoMedico = new Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    Matricula = txtMatricula.Text,
                    EspecialidadesTexto = txtEspecialidad.Text,
                    Email = txtEmail.Text,
                    Telefono = txtTelefono.Text
                };

                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Agregar(nuevoMedico);

                CargarGrillaMedicos();

                txtNombre.Text = "";
                txtApellido.Text = "";
                txtDni.Text = "";
                txtMatricula.Text = "";
                txtEspecialidad.Text = "";
                txtEmail.Text = "";
                txtTelefono.Text = "";

                Response.Write("<script>alert('Médico agregado correctamente');</script>");
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje completo para saber dónde falla
                Response.Write("<script>alert('Error al guardar el médico: " + ex.ToString() + "');</script>");
            }
        }
    }
}