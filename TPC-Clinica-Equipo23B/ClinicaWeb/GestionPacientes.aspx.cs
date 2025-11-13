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
    public partial class GestionPacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrillaPacientes();
            }
        }
        private void CargarGrillaPacientes()
        {
            PacienteNegocio negocio = new PacienteNegocio();
            List<Paciente> lista = negocio.Listar();
            gvPacientes.DataSource = lista;
            gvPacientes.DataBind();
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

        protected void btnGuardarPaciente_Click(object sender, EventArgs e)
        {
            litModalError.Text = "";
            try
            {
                // 1. Creamos un nuevo objeto Paciente con los datos del modal
                // (Asumiendo que tienes una clase Paciente en tu proyecto)
                Paciente nuevoPaciente = new Paciente();

                // 2. Leemos los valores de los TextBox (gracias al Paso 1)
                nuevoPaciente.Nombre = txtAddNombre.Text;
                nuevoPaciente.Apellido = txtAddApellido.Text;
                nuevoPaciente.Dni = txtAddDNI.Text;
                nuevoPaciente.Email = txtAddEmail.Text;
                nuevoPaciente.Telefono = txtAddTelefono.Text;
                nuevoPaciente.Localidad = txtAddDireccion.Text;

                // Para la fecha de nacimiento, es bueno convertirla
                if (!string.IsNullOrEmpty(txtAddNacimiento.Text))
                {
                    nuevoPaciente.FechaNacimiento = DateTime.Parse(txtAddNacimiento.Text);
                }

                // 3. Llamamos a la lógica de negocio para guardarlo
                // (Esto es un ejemplo, debes adaptarlo a tu proyecto)
                PacienteNegocio negocio = new PacienteNegocio();
                negocio.Agregar(nuevoPaciente); // Asumiendo que tienes un método Agregar

                // 4. (IMPORTANTE) Volvemos a cargar la grilla para ver el nuevo paciente
                CargarGrillaPacientes(); // Necesitarás un método que cargue la grilla

                // (Opcional) Limpiar los campos del modal después de agregar
                LimpiarModal();
            }
            catch (Exception ex)
            {
                // (Importante) Manejar cualquier error
                // Podrías mostrar un mensaje de error en la página
                // lblError.Text = "Error al guardar el paciente: " + ex.Message;
                string errorScript = $"<div class='alert alert-danger'>{ex.Message}</div>";
            }
        }

        // (Opcional pero recomendado) Método para limpiar el modal
        private void LimpiarModal()
        {
            txtAddNombre.Text = "";
            txtAddApellido.Text = "";
            txtAddDNI.Text = "";
            txtAddNacimiento.Text = "";
            txtAddTelefono.Text = "";
            txtAddEmail.Text = "";
            txtAddDireccion.Text = "";
        }
    }
 }

