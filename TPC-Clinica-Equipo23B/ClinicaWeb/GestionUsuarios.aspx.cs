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
    public partial class GestionUsuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx", false);
                return;
            }


            string rol = Session["rol"]?.ToString().ToUpper();
            if (rol != "ADMINISTRADOR")
            {
                Session.Add("error", "Solo los Administradores tienen permiso para acceder a la Gestión de Usuarios.");
                Response.Redirect("GestionTurnos.aspx", false);
                return;
            }

            ConfigurarPermisosBaja();

            if (!IsPostBack)
            {
                CargarGrillaUsuarios();
            }
        }

        private void ConfigurarPermisosBaja()
        {
            if (btnEliminarFisico != null)
            {
                btnEliminarFisico.Style.Add("display", "none");
            }

            
            if (Session["rol"] != null && Session["rol"].ToString().ToUpper() == "ADMINISTRADOR")
            {
                if (btnEliminarFisico != null)
                {
                    btnEliminarFisico.Style.Remove("display");
                }
            }
        }

        private void CargarGrillaUsuarios()
        {
            try
            {
                UsuarioNegocio userNegocio = new UsuarioNegocio();

                List<UsuarioConRol> listaCompleta = userNegocio.ListarUsuariosConRol();

                Session["ListaUsuarios"] = listaCompleta;

                gvUsuarios.DataSource = listaCompleta;
                gvUsuarios.DataBind();
            }
            catch (Exception ex)
            {
                string mensaje = "Error al cargar los usuarios: " + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorCarga", $"mostrarMensajeError('{mensaje.Replace("'", "")}');", true);
            }
        }

        private UsuarioConRol ObtenerUsuarioPorId(int id)
        {
            List<UsuarioConRol> lista = (List<UsuarioConRol>)Session["ListaUsuarios"];
            return lista?.FirstOrDefault(u => u.IdPersona == id);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {

            List<UsuarioConRol> listaCompleta = (List<UsuarioConRol>)Session["ListaUsuarios"];
            string filtro = txtFiltroEmail.Text.Trim().ToLower();

            List<UsuarioConRol> listaFiltrada;

            if (!string.IsNullOrEmpty(filtro) && listaCompleta != null)
            {
                // para filtrar 
                listaFiltrada = listaCompleta.FindAll(u =>
                    (u.Email != null && u.Email.ToLower().Contains(filtro)) ||
                    (u.Apellido != null && u.Apellido.ToLower().Contains(filtro)) ||
                    (u.RolNombre != null && u.RolNombre.ToLower().Contains(filtro))
                );
            }
            else
            {
                listaFiltrada = listaCompleta;
            }

            gvUsuarios.DataSource = listaFiltrada;
            gvUsuarios.DataBind();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(e.CommandArgument);
                UsuarioConRol seleccionado = ObtenerUsuarioPorId(idUsuario);

                if (seleccionado == null) return;

                if (e.CommandName == "VerUsuario")
                {

                    lblVerNombre.Text = seleccionado.Nombre;
                    lblVerApellido.Text = seleccionado.Apellido;
                    lblVerDNI.Text = seleccionado.Dni;
                    lblVerEmail.Text = seleccionado.Email;
                    lblVerTelefono.Text = seleccionado.Telefono;
                    lblVerLocalidad.Text = seleccionado.Localidad;
                    lblVerRol.Text = seleccionado.RolNombre;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "verModal", "abrirModalVer();", true);
                }
                else if (e.CommandName == "EditarUsuario")
                {
                    hfIdUsuarioEditar.Value = idUsuario.ToString();
                    txtEditNombre.Text = seleccionado.Nombre;
                    txtEditApellido.Text = seleccionado.Apellido;
                    txtEditDNI.Text = seleccionado.Dni;
                    txtEditEmail.Text = seleccionado.Email;
                    txtEditTelefono.Text = seleccionado.Telefono;
                    txtEditLocalidad.Text = seleccionado.Localidad;
                    txtEditRol.Text = seleccionado.RolNombre;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "editarModal", "abrirModalEditar();", true);
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", $"mostrarMensajeError('{mensaje}');", true);
            }
        }

        protected void btnGuardarEdicion_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = int.Parse(hfIdUsuarioEditar.Value);
                List<string> errores = new List<string>();

                if (string.IsNullOrWhiteSpace(txtEditNombre.Text))
                    errores.Add("<li>El campo Nombre es obligatorio.</li>");
                if (string.IsNullOrWhiteSpace(txtEditApellido.Text))
                    errores.Add("<li>El campo Apellido es obligatorio.</li>");
                if (string.IsNullOrWhiteSpace(txtEditDNI.Text))
                    errores.Add("<li>El campo DNI es obligatorio.</li>");
                else if (txtEditDNI.Text.Length > 0 && (txtEditDNI.Text.Length < 7 || txtEditDNI.Text.Length > 8 || !txtEditDNI.Text.All(char.IsDigit)))
                    errores.Add("<li>El DNI debe contener 7 u 8 dígitos.</li>");
                if (string.IsNullOrWhiteSpace(txtEditEmail.Text))
                    errores.Add("<li>El campo Email es obligatorio.</li>");
                if (!string.IsNullOrWhiteSpace(txtEditNombre.Text))
                {
                    if (!txtEditNombre.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                        errores.Add("<li>El Nombre solo puede contener letras y espacios.</li>");
                }

                if (!string.IsNullOrWhiteSpace(txtEditApellido.Text))
                {
                    if (!txtEditApellido.Text.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                        errores.Add("<li>El Apellido solo puede contener letras y espacios.</li>");
                }

                if (!txtEditEmail.Text.Contains("@") || !txtEditEmail.Text.Contains("."))
                    errores.Add("<li>El formato del Email no es válido.</li>");

      
                if (errores.Any())
                {
                    string mensajeValidacion = "Por favor, corrija los siguientes errores:<ul style='text-align: left;'>" + string.Join("", errores) + "</ul>";
                    string scriptErrorValidacion = $@"
                        mostrarMensajeError('{mensajeValidacion}');
                        abrirModalEditar();
                        ";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorValidacionEdicion", scriptErrorValidacion, true);
                    return;
                }


                UsuarioNegocio userNegocio = new UsuarioNegocio();
                string dni = txtEditDNI.Text.Trim();
                string email = txtEditEmail.Text.Trim();

                if (userNegocio.ExisteUsuarioMismoDniOEmail(idUsuario, email, dni))
                {
                    string mensajeDuplicado = "El DNI o Correo Electrónico ingresado ya está registrado para otro usuario/paciente. Por favor, verifique los datos.";

                    string script = $@"
                        var modalEditEl = document.getElementById('editUserModal');
                        var modalEditInstance = bootstrap.Modal.getInstance(modalEditEl);
                        if (modalEditInstance) {{ modalEditInstance.hide(); }}
                        mostrarMensajeError('{mensajeDuplicado}');
                       abrirModalEditar();
                        limpiarFondosResiduales();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorDuplicadoEdicion", script, true);
                    return;
                }


                Persona personaModificada = new Persona
                {
                    IdPersona = idUsuario,
                    Nombre = txtEditNombre.Text.Trim(),
                    Apellido = txtEditApellido.Text.Trim(),
                    Dni = txtEditDNI.Text.Trim(),
                    Email = txtEditEmail.Text.Trim(),
                    Telefono = string.IsNullOrEmpty(txtEditTelefono.Text) ? null : txtEditTelefono.Text.Trim(),
                    Localidad = string.IsNullOrEmpty(txtEditLocalidad.Text) ? null : txtEditLocalidad.Text.Trim()
                }; 


                userNegocio.ModificarPersona(personaModificada);


                CargarGrillaUsuarios();
                string scriptExito = "var myModalEl = document.getElementById('editUserModal'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales(); alert('Usuario modificado correctamente.');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModalExito", scriptExito, true);
            }
            catch (Exception ex)
            {
                string mensajeError = "Error al guardar: " + ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
                    var modalEditEl = document.getElementById('editUserModal');
                    var modalEditInstance = bootstrap.Modal.getInstance(modalEditEl);
                    if (modalEditInstance) {{ modalEditInstance.hide(); }}
                    mostrarMensajeError('{mensajeError}');
                    limpiarFondosResiduales();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorAlert", script, true);
            }
        }

    
        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(hfIdUsuarioEliminar.Value);

                UsuarioNegocio userNegocio = new UsuarioNegocio();
                userNegocio.EliminarLogicoUsuario(idUsuario);

                CargarGrillaUsuarios();
                string scriptCerrar = "var myModalEl = document.getElementById('modalConfirmarEliminar'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales();";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", scriptCerrar, true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
                    var modalEl = document.getElementById('modalConfirmarEliminar');
                    var modalInstance = bootstrap.Modal.getInstance(modalEl);
                    if (modalInstance) {{ modalInstance.hide(); }}
                    mostrarMensajeError('{mensaje}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", script, true);
            }
        }

      
        protected void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            try
            {
                int idUsuario = Convert.ToInt32(hfIdUsuarioEliminar.Value);

                UsuarioNegocio userNegocio = new UsuarioNegocio();
                userNegocio.EliminarFisicoUsuario(idUsuario);

                string scriptCerrar = "var myModalEl = document.getElementById('modalConfirmarEliminar'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales();";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", scriptCerrar, true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertExito", "alert('El usuario fue eliminado definitivamente de la base de datos.');", true);
            }
            catch (Exception ex)
            {
                string mensaje = "Error al eliminar definitivamente el usuario: " + ex.Message.Replace("'", "").Replace("\n", " ");
                string script = $@"
                    var modalEl = document.getElementById('modalConfirmarEliminar');
                    var modalInstance = bootstrap.Modal.getInstance(modalEl);
                    if (modalInstance) {{ modalInstance.hide(); }}
                    mostrarMensajeError('{mensaje}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", script, true);
            }
        }

    }
}