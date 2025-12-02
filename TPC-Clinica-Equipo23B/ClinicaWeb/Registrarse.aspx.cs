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
    public partial class Registrarse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (litErrorRegistro != null)
                    litErrorRegistro.Text = "";

                CargarRoles();
                CargarDias();
                CargarTurnos();
                
                if (Session["ListaJornadas"] == null)
                    Session["ListaJornadas"] = new List<JornadaLaboral>();

                pnlDatosMedico.Visible = false;
            }           
        }

        private void CargarRoles()
        {
            UsuarioNegocio negocio = new UsuarioNegocio();
            List<Rol> listaRoles = negocio.ListarRoles();

            ddlRol.DataSource = listaRoles;
            ddlRol.DataTextField = "TipoRol";
            ddlRol.DataValueField = "IdRol";
            ddlRol.DataBind();
            ddlRol.Items.Insert(0, new ListItem("Seleccione un Rol", "0"));
        }
        private void CargarEspecialidades()
        {
            try
            {
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                List<Especialidad> lista = negocio.listar();
                lbxEspecialidades.DataSource = lista;
                lbxEspecialidades.DataTextField = "Nombre";
                lbxEspecialidades.DataValueField = "IdEspecialidad";
                lbxEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                if (litErrorRegistro != null)
                    litErrorRegistro.Text = "<div class='alert alert-danger'>Error al cargar especialidades: " + ex.Message + "</div>";
            }
        }

        private void CargarDias()
        {
            ListBox2.DataSource = Enum.GetNames(typeof(DiaLaboral));
            ListBox2.DataBind();
        }

        private void CargarTurnos()
        {
            TurnoDeTrabajoNegocio turnoTrabajoNegocio = new TurnoDeTrabajoNegocio();
            ddlTurno.DataSource = turnoTrabajoNegocio.listar();
            ddlTurno.DataTextField = "TipoDeTurno";
            ddlTurno.DataValueField = "IdTurnoTrabajo";
            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, new ListItem("Personalizado / Seleccione", "0"));
        }
        protected void ddlRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMatricula.Text = "";
            if (ddlRol.SelectedValue == "2")
            {
                pnlDatosMedico.Visible = true;
                if (lbxEspecialidades.Items.Count == 0)
                {
                    CargarEspecialidades();
                }
            }
            else
            {
                pnlDatosMedico.Visible = false;
                lbxEspecialidades.ClearSelection();
            }
        }

        protected void btnRegistrarse_Click(object sender, EventArgs e)
        {
            if (litErrorRegistro != null)
                litErrorRegistro.Text = "";

            List<string> errores = new List<string>();

            if (string.IsNullOrWhiteSpace(firstName.Text))
                errores.Add("<li>El campo Nombre es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(lastName.Text))
                errores.Add("<li>El campo Apellido es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(idDocument.Text))
                errores.Add("<li>El campo Documento (DNI) es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(email.Text))
                errores.Add("<li>El campo Correo electrónico es obligatorio.</li>");
            if (ddlRol.SelectedValue == "0")
                errores.Add("<li>Debe seleccionar un Tipo de Usuario (Rol).</li>");
            if (string.IsNullOrWhiteSpace(password.Text))
                errores.Add("<li>El campo Contraseña es obligatorio.</li>");
            if (string.IsNullOrWhiteSpace(passwordConfirm.Text))
                errores.Add("<li>El campo Confirmar Contraseña es obligatorio.</li>");
            if (password.Text != passwordConfirm.Text)
                errores.Add("<li>Las contraseñas no coinciden.</li>");
            if (password.Text.Length > 0 && password.Text.Length < 6)
                errores.Add("<li>La contraseña debe tener al menos 6 caracteres.</li>");

            if (ddlRol.SelectedValue == "2" && string.IsNullOrWhiteSpace(txtMatricula.Text))
                errores.Add("<li>Para el rol Médico, el campo Matrícula es obligatorio.</li>");


            if (errores.Count > 0)
            {
                MostrarErrores(errores);
                return;
            }


            try
            {
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                string emailTrimmed = email.Text.Trim();
                string dniTrimmed = idDocument.Text.Trim();
                int idRol = int.Parse(ddlRol.SelectedValue);

                if (usuarioNegocio.ExisteUsuario(emailTrimmed, dniTrimmed))
                {
                    string mensaje = "Ya existe una cuenta registrada con el DNI o Correo electrónico proporcionado. Por favor, intente <a href=\"Login.aspx\" class=\"alert-link\">iniciar sesión</a>.";
                    if (litErrorRegistro != null)
                        litErrorRegistro.Text = "<div class='alert alert-warning'>" + mensaje + "</div>";
                    return;
                }

                Persona nuevaPersona = new Persona
                {
                    Nombre = firstName.Text.Trim(),
                    Apellido = lastName.Text.Trim(),
                    Dni = dniTrimmed,
                    Email = emailTrimmed,
                    Telefono = phone.Text.Trim(),
                    Localidad = address.Text.Trim()
                };
                string matricula = null;
                List<int> listaEspecialidades = new List<int>();
                List<JornadaLaboral> listaJornadas = new List<JornadaLaboral>();
                if (idRol == 2)
                {
                    matricula = txtMatricula.Text.Trim();

                    foreach (ListItem item in lbxEspecialidades.Items)
                    {
                        if (item.Selected)
                        {
                            listaEspecialidades.Add(int.Parse(item.Value));
                        }
                    }
                    if (Session["ListaJornadas"] != null)
                    {
                        listaJornadas = (List<JornadaLaboral>)Session["ListaJornadas"];
                    }
                }
                usuarioNegocio.RegistrarNuevoUsuarioConRol(
                nuevaPersona,
                password.Text.Trim(),
                idRol,
                matricula,
                listaEspecialidades,
                listaJornadas);

                Session.Add("registroExitoso", "¡Registro completado! Ahora puede iniciar sesión con su correo y contraseña.");
                Response.Redirect("Login.aspx", false);
            }
            catch (Exception ex)
            {
                List<string> errorFatal = new List<string> { "Ocurrió un error al intentar registrarse. Por favor, intente más tarde. Detalles: " + ex.Message };
                MostrarErrores(errorFatal);
            }
        }

        private void MostrarErrores(List<string> errores)
        {
            if (litErrorRegistro != null)
            {
                litErrorRegistro.Text = "<div class='alert alert-danger'><p>Por favor, corrija los siguientes errores:</p><ul style='list-style-type: disc; margin-left: 20px;'>" + string.Join("", errores) + "</ul></div>";
            }
        }

        protected void btnGuardarEspecialidad_Click(object sender, EventArgs e)
        {
            try
            {
                Especialidad nueva = new Especialidad();
                nueva.Nombre = txtNuevaEspecialidad.Text;
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                negocio.agregar(nueva);

                txtNuevaEspecialidad.Text = string.Empty;
                CargarEspecialidades();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CerrarModalScript",
                "var myModalEl = document.getElementById('modalNuevaEspecialidad'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide();", true);

            }
            catch (Exception ex)
            {
                // Si hay error marcar con mensaje
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorEspecialidad",
                    $"alert('Error al guardar: {ex.Message}');", true);
            }

        }

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idTurno = int.Parse(ddlTurno.SelectedValue);

            if (idTurno > 0)
            {
                TurnoDeTrabajoNegocio negocio = new TurnoDeTrabajoNegocio();
                TurnoDeTrabajo turno = negocio.obtenerPorId(idTurno);

                if (turno != null)
                {
                    txtHoraInicio.Text = turno.HoraInicioDefault.ToString(@"hh\:mm");
                    txtHoraFin.Text = turno.HoraFinDefault.ToString(@"hh\:mm");
                }
            }
            else
            {
                txtHoraInicio.Text = "";
                txtHoraFin.Text = "";
            }
        }
     

        protected void btnAgregarHorario_Click(object sender, EventArgs e)
        {
            try 
            { 
            if (string.IsNullOrEmpty(txtHoraInicio.Text) || string.IsNullOrEmpty(txtHoraFin.Text))
            {
                lblErrorHorario.Text = "Debe ingresar hora de inicio y fin.";
                lblErrorHorario.Visible = true;
                return;
            }
            List<JornadaLaboral> listaTemporal = (List<JornadaLaboral>)Session["ListaJornadas"];

            bool haySeleccion = false;
                foreach (ListItem item in ListBox2.Items)
                {
                    if (item.Selected)
                    {
                        haySeleccion = true;
                        JornadaLaboral jornada = new JornadaLaboral();
                        jornada.DiaLaboral = (DiaLaboral)Enum.Parse(typeof(DiaLaboral), item.Text); // O simplemente string si tu propiedad es string
                        jornada.HorarioInicio = TimeSpan.Parse(txtHoraInicio.Text);
                        jornada.HoraFin = TimeSpan.Parse(txtHoraFin.Text);

                        if (ddlTurno.SelectedValue != "0")
                        {
                            jornada.IdTurnoTrabajo = int.Parse(ddlTurno.SelectedValue);
                        }
                        listaTemporal.Add(jornada);
                        item.Selected = false;
                    }
                }

                if (!haySeleccion)
                {
                    lblErrorHorario.Text = "Debe seleccionar al menos un día de la lista.";
                    lblErrorHorario.Visible = true;
                    return;
                }
                Session["ListaJornadas"] = listaTemporal;
                ActualizarGrillaHorarios();

                lblErrorHorario.Visible = false;
            }
            catch (Exception ex)
            {
                lblErrorHorario.Text = "Error al agregar: " + ex.Message;
                lblErrorHorario.Visible = true;
            }
        }

        private void ActualizarGrillaHorarios()
        {
            List<JornadaLaboral> lista = (List<JornadaLaboral>)Session["ListaJornadas"];
            gvHorarios.DataSource = lista;
            gvHorarios.DataBind();
        }
        protected void gvHorarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<JornadaLaboral> lista = (List<JornadaLaboral>)Session["ListaJornadas"];
            lista.RemoveAt(e.RowIndex);

            // Guardar y refrescar!
            Session["ListaJornadas"] = lista;
            ActualizarGrillaHorarios();
        }
    }
}