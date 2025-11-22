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
            if (e.CommandName == "VerPaciente" || e.CommandName == "EditarPaciente")
            {
                int idPaciente = Convert.ToInt32(e.CommandArgument);
                PacienteNegocio negocio = new PacienteNegocio();
                List<Paciente> lista = negocio.Listar();
                Paciente seleccionado = lista.Find(x => x.IdPersona == idPaciente);

                if (seleccionado != null)
                {
                    if (e.CommandName == "VerPaciente")
                    {  
                        lblVerNombre.Text = seleccionado.Nombre;
                        lblVerApellido.Text = seleccionado.Apellido;
                        lblVerDNI.Text = seleccionado.Dni;
                        lblVerEmail.Text = seleccionado.Email;
                        lblVerTelefono.Text = seleccionado.Telefono;
                        lblVerDireccion.Text = seleccionado.Localidad; 
                        lblVerNacimiento.Text = seleccionado.FechaNacimiento.ToString("dd/MM/yyyy");

                        string script = "new bootstrap.Modal(document.getElementById('viewPatientModal')).show();";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "verModal", script, true);
                    }
                    else if (e.CommandName == "EditarPaciente")
                    {
                           
                        hfIdPacienteEditar.Value = idPaciente.ToString();                        
                        txtEditNombre.Text = seleccionado.Nombre;
                        txtEditApellido.Text = seleccionado.Apellido;
                        txtEditDNI.Text = seleccionado.Dni;
                        txtEditEmail.Text = seleccionado.Email;
                        txtEditTelefono.Text = seleccionado.Telefono;
                        txtEditDireccion.Text = seleccionado.Localidad;
                     
                        txtEditNacimiento.Text = seleccionado.FechaNacimiento.ToString("yyyy-MM-dd");

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "abrirModalEditar();", true);
                    }
                }
            }
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
        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int idPaciente = Convert.ToInt32(hfIdPacienteEliminar.Value);           
                negocio.PacienteNegocio negocio = new negocio.PacienteNegocio();
                negocio.Eliminar(idPaciente);

                CargarGrillaPacientes();
            }
            catch (Exception ex)
            {
                Session.Add("error", "No se pudo eliminar el paciente: " + ex.Message);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string filtro = txtFiltroDNI.Text;
                
                PacienteNegocio negocio = new PacienteNegocio();
                List<Paciente> listaCompleta = negocio.Listar();
                List<Paciente> listaFiltrada;
                
                if (!string.IsNullOrEmpty(filtro))
                {                    
                    listaFiltrada = listaCompleta.FindAll(x => x.Dni.Contains(filtro));
                }
                else
                {
                   
                    listaFiltrada = listaCompleta;
                } 
                gvPacientes.DataSource = listaFiltrada;
                gvPacientes.DataBind();
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al filtrar: " + ex.Message);
            }

        }

      
        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                dominio.Paciente pacienteModificado = new dominio.Paciente();
                pacienteModificado.IdPersona = int.Parse(hfIdPacienteEditar.Value);

                pacienteModificado.Nombre = txtEditNombre.Text;
                pacienteModificado.Apellido = txtEditApellido.Text;
                pacienteModificado.Dni = txtEditDNI.Text;
                pacienteModificado.Email = txtEditEmail.Text;
                pacienteModificado.Telefono = txtEditTelefono.Text;
                pacienteModificado.Localidad = txtEditDireccion.Text;
                pacienteModificado.FechaNacimiento = DateTime.Parse(txtEditNacimiento.Text);

                negocio.PacienteNegocio negocio = new negocio.PacienteNegocio();
                negocio.Modificar(pacienteModificado);            
                Response.Redirect(Request.RawUrl);      
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al modificar: " + ex.Message);
                string script = $"alert('ERROR AL GUARDAR: {ex.Message}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorAlert", script, true);
            }
        }      
    }
 }

