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
              
                Paciente nuevoPaciente = new Paciente();

                nuevoPaciente.Nombre = txtAddNombre.Text;
                nuevoPaciente.Apellido = txtAddApellido.Text;
                nuevoPaciente.Dni = txtAddDNI.Text;
                nuevoPaciente.Email = txtAddEmail.Text;
                nuevoPaciente.Telefono = txtAddTelefono.Text;
                nuevoPaciente.Localidad = txtAddDireccion.Text;

                if (!string.IsNullOrEmpty(txtAddNacimiento.Text))
                {
                    nuevoPaciente.FechaNacimiento = DateTime.Parse(txtAddNacimiento.Text);
                }

                
                PacienteNegocio negocio = new PacienteNegocio();
                negocio.Agregar(nuevoPaciente); 


                CargarGrillaPacientes(); 

       
                LimpiarModal();
            }
            catch (Exception ex)
            {
                
                string errorScript = $"<div class='alert alert-danger'>{ex.Message}</div>";
            }
        }

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

