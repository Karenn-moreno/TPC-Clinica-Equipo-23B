using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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

            // Validar que haya sesion
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }

            // Validar rol del usuario
            if (Session["rol"] != null && Session["rol"].ToString().ToUpper() == "MEDICO")
            {
                // Guardar mensaje en sesion
                Session["error"] = "No tienes permisos para acceder a la Gestión de Pacientes.";

               
                Response.Redirect("GestionTurnos.aspx", false);
                return;
            }



            ConfigurarPermisosBaja();
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
        private void ConfigurarPermisosBaja()
        {

            btnEliminarFisico.Style.Add("display", "none");


            if (Session["rol"] != null)
            {

                string rolActual = Session["rol"].ToString().ToUpper();


                if (rolActual == "ADMINISTRADOR")
                {
                    btnEliminarFisico.Style.Remove("display");
                }
            }
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
            List<string> errores = new List<string>();
            DateTime fechaNacimiento = DateTime.MinValue;

        
            if (string.IsNullOrWhiteSpace(txtAddNombre.Text))
                errores.Add("<li>El campo Nombre es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtAddApellido.Text))
                errores.Add("<li>El campo Apellido es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtAddDNI.Text))
                errores.Add("<li>El campo DNI es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtAddNacimiento.Text))
                errores.Add("<li>El campo Fecha de Nacimiento es obligatorio.</li>");


            if (!string.IsNullOrWhiteSpace(txtAddDNI.Text))
            {
                if (txtAddDNI.Text.Length != 8 || !txtAddDNI.Text.All(char.IsDigit))
                {
                    errores.Add("<li>El DNI debe contener exactamente 8 dígitos.</li>");
                }
            }

            if (!string.IsNullOrEmpty(txtAddNacimiento.Text))
            {
                if (!DateTime.TryParse(txtAddNacimiento.Text, out fechaNacimiento))
                {
                    errores.Add("<li>El formato de la Fecha de Nacimiento no es válido.</li>");
                }
                else if (fechaNacimiento >= DateTime.Today)
                {
                    errores.Add("<li>La Fecha de Nacimiento no puede ser hoy o una fecha futura.</li>");
                }
            }

         
            if (errores.Count > 0)
            {
                litModalError.Text = "<div class='alert alert-danger mb-3'><p class='fw-bold'>Por favor, corrija los siguientes errores:</p><ul style='list-style-type: disc; margin-left: 20px;'>" + string.Join("", errores) + "</ul></div>";
                string script = "new bootstrap.Modal(document.getElementById('addPatientModal')).show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reabrirModal", script, true);
                return;
            }

            try
            {

                Paciente nuevoPaciente = new Paciente();

                nuevoPaciente.Nombre = txtAddNombre.Text.Trim();
                nuevoPaciente.Apellido = txtAddApellido.Text.Trim();
                nuevoPaciente.Dni = txtAddDNI.Text.Trim();
                nuevoPaciente.Email = string.IsNullOrEmpty(txtAddEmail.Text) ? null : txtAddEmail.Text.Trim();
                nuevoPaciente.Telefono = string.IsNullOrEmpty(txtAddTelefono.Text) ? null : txtAddTelefono.Text.Trim();
                nuevoPaciente.Localidad = string.IsNullOrEmpty(txtAddDireccion.Text) ? null : txtAddDireccion.Text.Trim();

                nuevoPaciente.FechaNacimiento = fechaNacimiento;


                PacienteNegocio negocio = new PacienteNegocio();
                negocio.Agregar(nuevoPaciente);


                CargarGrillaPacientes();
                LimpiarModal();

                string scriptCerrar = "var myModalEl = document.getElementById('addPatientModal'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales(); alert('Paciente agregado correctamente.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModalExito", scriptCerrar, true);
            }
            catch (Exception ex)
            {
                string mensajeError = "Error al guardar: " + ex.Message.Replace("'", "").Replace("\n", " ");
                litModalError.Text = $"<div class='alert alert-danger'>{mensajeError}</div>";
                string script = "new bootstrap.Modal(document.getElementById('addPatientModal')).show();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "reabrirModalErrorNegocio", script, true);
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
                negocio.EliminarLogico(idPaciente);

                CargarGrillaPacientes();
                string scriptCerrar = "var myModalEl = document.getElementById('modalConfirmarEliminar'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", scriptCerrar, true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
        var modalEl = document.getElementById('modalConfirmarEliminar');
        var modalInstance = bootstrap.Modal.getInstance(modalEl);
        
        if (modalInstance) {{
            modalInstance.hide();
        }} else {{
            
            var backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) backdrop.remove();
            document.body.classList.remove('modal-open');
            document.body.style = '';
        }}

        mostrarMensajeError('{mensaje}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", script, true);
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
            List<string> errores = new List<string>();
            DateTime fechaNacimiento = DateTime.MinValue;
            int idPaciente;


            if (!int.TryParse(hfIdPacienteEditar.Value, out idPaciente))
            {
                errores.Add("<li>Error al obtener el ID del paciente para editar.</li>");
            }


            if (string.IsNullOrWhiteSpace(txtEditNombre.Text))
                errores.Add("<li>El campo Nombre es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtEditApellido.Text))
                errores.Add("<li>El campo Apellido es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtEditDNI.Text))
                errores.Add("<li>El campo DNI es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(txtEditNacimiento.Text))
                errores.Add("<li>El campo Fecha de Nacimiento es obligatorio.</li>");


            if (!string.IsNullOrWhiteSpace(txtEditDNI.Text))
            {
                if (txtEditDNI.Text.Length != 8 || !txtEditDNI.Text.All(char.IsDigit))
                {
                    errores.Add("<li>El DNI debe contener exactamente 8 dígitos.</li>");
                }
            }

            if (!string.IsNullOrWhiteSpace(txtEditEmail.Text))
            {
                if (!txtEditEmail.Text.Contains("@") || !txtEditEmail.Text.Contains("."))
                    errores.Add("<li>El formato del Email no es válido.</li>");
            }


            if (!string.IsNullOrEmpty(txtEditNacimiento.Text))
            {
                if (!DateTime.TryParse(txtEditNacimiento.Text, out fechaNacimiento))
                {
                    errores.Add("<li>El formato de la Fecha de Nacimiento no es válido.</li>");
                }
                else if (fechaNacimiento >= DateTime.Today)
                {
                    errores.Add("<li>La Fecha de Nacimiento no puede ser hoy o una fecha futura.</li>");
                }
            }


            if (errores.Count > 0)
            {
                string mensaje = "Por favor, corrija los siguientes errores:<ul style='text-align: left;'>" + string.Join("", errores) + "</ul>";

                string script = $@"
                    mostrarMensajeError('{mensaje}');
abrirModalEditar();
                    ";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorValidacionEdicion", script, true);
                return;
            }

            try
            {
                UsuarioNegocio userNegocio = new UsuarioNegocio();
                string dni = txtEditDNI.Text.Trim();
                string email = txtEditEmail.Text.Trim();

                if (userNegocio.ExisteUsuarioMismoDniOEmail(idPaciente, email, dni))
                {
                    string mensajeDuplicado = "El DNI o Correo Electrónico ingresado ya está registrado para otro usuario/paciente. Por favor, verifique los datos.";

                    string script = $@"
                        var modalEditEl = document.getElementById('editPatientModal');
                        var modalEditInstance = bootstrap.Modal.getInstance(modalEditEl);
                        if (modalEditInstance) {{ modalEditInstance.hide(); }}
                        mostrarMensajeError('{mensajeDuplicado}');
abrirModalEditar();
                        limpiarFondosResiduales();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorDuplicadoEdicion", script, true);
                    return;
                }
    

                dominio.Paciente pacienteModificado = new dominio.Paciente();
                pacienteModificado.IdPersona = idPaciente;
                pacienteModificado.Nombre = txtEditNombre.Text.Trim();
                pacienteModificado.Apellido = txtEditApellido.Text.Trim();
                pacienteModificado.Dni = txtEditDNI.Text.Trim();
                pacienteModificado.Email = string.IsNullOrEmpty(txtEditEmail.Text) ? null : txtEditEmail.Text.Trim();
                pacienteModificado.Telefono = string.IsNullOrEmpty(txtEditTelefono.Text) ? null : txtEditTelefono.Text.Trim();
                pacienteModificado.Localidad = string.IsNullOrEmpty(txtEditDireccion.Text) ? null : txtEditDireccion.Text.Trim();

                pacienteModificado.FechaNacimiento = fechaNacimiento;

                negocio.PacienteNegocio negocio = new negocio.PacienteNegocio();
                negocio.Modificar(pacienteModificado);

                CargarGrillaPacientes();
                string scriptExito = "var myModalEl = document.getElementById('editPatientModal'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales(); alert('Paciente modificado correctamente.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModalExito", scriptExito, true);
            }
            catch (Exception ex)
            {

                string mensajeError = "Error al guardar: " + ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
                    var modalEditEl = document.getElementById('editPatientModal');
                    var modalEditInstance = bootstrap.Modal.getInstance(modalEditEl);
                    if (modalEditInstance) {{ modalEditInstance.hide(); }}
                    mostrarMensajeError('{mensajeError}');
abrirModalEditar();
                    limpiarFondosResiduales();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorAlert", script, true);
            }

        }

        protected void gvPacientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        protected void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            try
            {

                int idPaciente = Convert.ToInt32(hfIdPacienteEliminar.Value);
                negocio.PacienteNegocio negocio = new negocio.PacienteNegocio();
                negocio.EliminarFisico(idPaciente);

                CargarGrillaPacientes();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "var myModalEl = document.getElementById('modalConfirmarEliminar'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertExito", "alert('El registro fue eliminado definitivamente de la base de datos.');", true);

            }
            catch (Exception ex)
            {

                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
        var modalEl = document.getElementById('modalConfirmarEliminar');
        var modalInstance = bootstrap.Modal.getInstance(modalEl);
        
        if (modalInstance) {{
            modalInstance.hide();
        }} else {{
            
            var backdrop = document.querySelector('.modal-backdrop');
            if (backdrop) backdrop.remove();
            document.body.classList.remove('modal-open');
            document.body.style = '';
        }}

        mostrarMensajeError('{mensaje}');";
                ;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", script, true);
            }
        }
    }
}

