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
                // Crear objeto médico con los datos del modal
                Medico nuevoMedico = new Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    Matricula = txtMatricula.Text,
                    EspecialidadesTexto = txtEspecialidad.Text
                };

                // Llamar a la lógica de negocio para agregar
                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Agregar(nuevoMedico);

                // Recargar la grilla
                CargarGrillaMedicos();

                // Limpiar modal
                txtNombre.Text = "";
                txtApellido.Text = "";
                txtDni.Text = "";
                txtMatricula.Text = "";
                txtEspecialidad.Text = "";

                // Opcional: mostrar mensaje de éxito
                Response.Write("<script>alert('Médico agregado correctamente');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al guardar el médico: " + ex.Message + "');</script>");
            }
        }
    }
}