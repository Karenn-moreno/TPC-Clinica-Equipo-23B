using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class GestionTurnos : System.Web.UI.Page
    {
        private const int DURACION_TURNO_MINUTOS = 60;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // La primera carga de la página
                CargarDesplegablesModal();
                DateTime fechaInicial = DateTime.Today;
                string fechaParam = Request.QueryString["fecha"];

                if (!string.IsNullOrEmpty(fechaParam) && DateTime.TryParse(fechaParam, out DateTime fechaUrl))
                {
                    fechaInicial = fechaUrl;
                }
                txtFechaGrilla.Text = fechaInicial.ToString("yyyy-MM-dd");
                CargarGrillaTurnos(fechaInicial);
            }
        }
        

        private void CargarDesplegablesModal()
        {
            try
            {

                PacienteNegocio pacNegocio = new PacienteNegocio();
                ddlPaciente.DataSource = pacNegocio.Listar();
                ddlPaciente.DataTextField = "Nombre";
                ddlPaciente.DataValueField = "IdPersona";
                ddlPaciente.DataBind();
                ddlPaciente.Items.Insert(0, new ListItem("Seleccione Paciente", "0"));

                EspecialidadNegocio espNegocio = new EspecialidadNegocio();
                ddlEspecialidad.DataSource = espNegocio.listar();
                ddlEspecialidad.DataTextField = "Nombre";
                ddlEspecialidad.DataValueField = "IdEspecialidad";
                ddlEspecialidad.DataBind();
                ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione Especialidad", "0"));

                CargarMedicos(0);
            }
            catch (Exception ex)
            {
                Session["Error"] = "Error al inicializar la gestión de turnos: " + ex.Message;
            }
        }

        private void CargarMedicos(int id_especialidad)
        {
            ddlMedico.Items.Clear();
            ddlHorario.Items.Clear();

            MedicoNegocio medicoNegocio = new MedicoNegocio();
            List<Medico> listaMedicos = medicoNegocio.Listar();

            var medicosFiltrados = listaMedicos
                .Where(m => id_especialidad == 0 || m.EspecialidadesTexto.Contains(ddlEspecialidad.SelectedItem.Text))
                .Select(m => new
                {
                    IdMedico = m.IdPersona,
                    NombreCompleto = "Dr/a. " + m.Nombre + " " + m.Apellido
                })
                .ToList();

            ddlMedico.DataSource = medicosFiltrados;
            ddlMedico.DataTextField = "NombreCompleto";
            ddlMedico.DataValueField = "IdMedico";
            ddlMedico.DataBind();
            ddlMedico.Items.Insert(0, new ListItem("Seleccione Médico", "0"));
        }

        private void CargarHorariosDisponibles()
        {
            ddlHorario.Items.Clear();

            if (!DateTime.TryParse(txtFecha.Text, out DateTime fechaSeleccionada))
                return;

            if (!int.TryParse(ddlMedico.SelectedValue, out int idMedico) || idMedico == 0)
                return;

            try
            {

                TurnoNegocio negocio = new TurnoNegocio();
                List<string> horariosLibres = negocio.ObtenerHorariosDisponibles(idMedico, fechaSeleccionada);

                if (horariosLibres.Count > 0)
                {
                    ddlHorario.DataSource = horariosLibres;
                    ddlHorario.DataBind();
                    ddlHorario.Items.Insert(0, new ListItem("Seleccione Horario", "0"));
                }
                else
                {
                    ddlHorario.Items.Insert(0, new ListItem("No hay horarios disponibles", "0"));
                }
            }
            catch (Exception ex)
            {
                ddlHorario.Items.Insert(0, new ListItem("Error al cargar horarios", "0"));
                Session["Error"] = ex.Message;
            }

        }
        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (int.TryParse(ddlEspecialidad.SelectedValue, out int id_especialidad))
            {
                CargarMedicos(id_especialidad);
            }
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMedico.SelectedValue != "0" && !string.IsNullOrEmpty(txtFecha.Text))
            {
                CargarHorariosDisponibles();
            }
            else
            {
                ddlHorario.Items.Clear();
                ddlHorario.Items.Insert(0, new ListItem("Seleccione Fecha y Médico", "0"));
            }


        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {

            if (ddlMedico.SelectedValue != "0" && !string.IsNullOrEmpty(txtFecha.Text))
            {
                CargarHorariosDisponibles();
            }
            else
            {
                ddlHorario.Items.Clear();
                ddlHorario.Items.Insert(0, new ListItem("Seleccione Médico", "0"));
            }

        }


        protected void btnGuardarTurno_Click(object sender, EventArgs e)
        {

            if (lblErrorNuevoTurno != null)
            {
                lblErrorNuevoTurno.Text = "";
                lblErrorNuevoTurno.Visible = false;
            }
            List<string> errores = new List<string>();

            if (ddlEspecialidad.SelectedValue == "0")
            {
                errores.Add("<li>Debe seleccionar una Especialidad.</li>");
            }
            if (ddlMedico.SelectedValue == "0")
            {
                errores.Add("<li>Debe seleccionar un Médico.</li>");
            }
            if (ddlPaciente.SelectedValue == "0")
            {
                errores.Add("<li>Debe seleccionar un Paciente.</li>");
            }
            if (string.IsNullOrEmpty(txtFecha.Text))
            {
                errores.Add("<li>Debe ingresar una Fecha.</li>");
            }
            if (ddlHorario.SelectedValue == "0")
            {
                errores.Add("<li>Debe seleccionar un Horario disponible.</li>");
            }

            if (errores.Count > 0)
            {
                if (lblErrorNuevoTurno != null)
                    lblErrorNuevoTurno.Text = "<p class='text-danger'>Por favor, complete los siguientes campos obligatorios:</p><ul class='text-danger'>" + string.Join("", errores) + "</ul>";
                return;
            }


            try
            {

                int idMedico = int.Parse(ddlMedico.SelectedValue);
                int idPaciente = int.Parse(ddlPaciente.SelectedValue);
                DateTime fechaTurno = DateTime.Parse(txtFecha.Text);
                TimeSpan horaTurno = TimeSpan.Parse(ddlHorario.SelectedValue);

                // Creación del objeto Turno
                Turno nuevoTurno = new Turno
                {
                    IdMedico = idMedico,
                    IdPaciente = idPaciente,
                    // Combinar fecha (sin hora) con la hora seleccionada (TimeSpan)
                    FechaHoraInicio = fechaTurno.Add(horaTurno),
                    FechaHoraFin = fechaTurno.Add(horaTurno).Add(TimeSpan.FromMinutes(DURACION_TURNO_MINUTOS)),
                    MotivoDeConsulta = txtMotivoConsulta.Text,
                    Diagnostico = null,
                    EstadoTurno = EstadoTurno.Nuevo
                };
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                List<Turno> ocupados = turnoNegocio.ListarTurnosOcupados(idMedico, fechaTurno);
                if (ocupados.Any(t => t.FechaHoraInicio == nuevoTurno.FechaHoraInicio))
                {
                    lblErrorNuevoTurno.Text = " El horario fue ocupado";
                    lblErrorNuevoTurno.Visible = true;
                    // Refrescamos los horarios 
                    CargarHorariosDisponibles();
                    return;
                }

                turnoNegocio.Agregar(nuevoTurno);

                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                List<Paciente> listaPacientes = pacienteNegocio.Listar();
                Paciente paciente = listaPacientes.FirstOrDefault(p => p.IdPersona == idPaciente);

                MedicoNegocio medicoNegocio = new MedicoNegocio();
                List<Medico> listaMedicos = medicoNegocio.Listar();
                Medico medico = listaMedicos.FirstOrDefault(m => m.IdPersona == idMedico);

                string especialidad = ddlEspecialidad.SelectedItem.Text;

                if (paciente != null && medico != null && !string.IsNullOrEmpty(paciente.Email))
                {
                    EmailService emailService = new EmailService();

                    string asunto = "Confirmación de Turno en Clínica Sanare";
                    string fechaHora = nuevoTurno.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm");
                    string nombrePaciente = paciente.Nombre + " " + paciente.Apellido;
                    string nombreMedico = "Dr/a. " + medico.Nombre + " " + medico.Apellido;

                    // Cuerpo del correo en formato HTML
                    string cuerpoHTML = $@"
                        <html>
                        <body>
                            <h2>¡Turno Confirmado!</h2>
                            <p>Estimado/a {nombrePaciente},</p>
                            <p>Su turno ha sido agendado exitosamente:</p>
                            <ul>
                                <li><strong>Fecha y Hora:</strong> {fechaHora} hs</li>
                                <li><strong>Profesional:</strong> {nombreMedico}</li>
                                <li><strong>Especialidad:</strong> {especialidad}</li>
                                <li><strong>Motivo:</strong> {nuevoTurno.MotivoDeConsulta}</li>
                            </ul>
                            <p>Por favor, sea puntual. Atentamente, el Equipo de Clínica Sanare.</p>
                        </body>
                        </html>";

                    emailService.armarCorreo(paciente.Email, asunto, cuerpoHTML);
                    emailService.enviarEmail();
                }

                txtFechaGrilla.Text = fechaTurno.ToString("yyyy-MM-dd");
                CargarGrillaTurnos(fechaTurno);

                ddlPaciente.SelectedIndex = 0;
                txtMotivoConsulta.Text = "";

                //Resetear Horario y Médico
                ddlHorario.Items.Clear();
                ddlMedico.SelectedIndex = 0; 

                CargarHorariosDisponibles();

                //Cerrar el modal usando JavaScript 
                string script = @"
            var myModalEl = document.getElementById('addTurnoModal');
            var modal = bootstrap.Modal.getInstance(myModalEl);
            if (modal) { modal.hide(); }
            else { new bootstrap.Modal(myModalEl).hide(); }";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", script, true);
            }
            catch (Exception ex)
            {
                if (lblErrorNuevoTurno != null)
                {
                    lblErrorNuevoTurno.Text = "<p class='text-danger'>Error al guardar: " + ex.Message + "</p>";
                    lblErrorNuevoTurno.Visible = true;
                }
            }
        }

        private void CargarGrillaTurnos(DateTime fecha)
        {
            try
            {
                TurnoNegocio negocio = new TurnoNegocio();
                List<Turno> listaTurnos = negocio.ListarPorFecha(fecha);


                Session["ListaTurnosDia"] = listaTurnos;

                gvTurnos.DataSource = listaTurnos;
                gvTurnos.DataBind();

            }
            catch (Exception ex)
            {
                Session["Error"] = "Error al cargar la grilla de turnos: " + ex.Message;

            }
        }
        protected void btnGuardarCambiosGrilla_Click(object sender, EventArgs e)
        {


            DateTime fechaBase = DateTime.Today;
            if (DateTime.TryParse(txtFechaGrilla.Text, out DateTime f)) fechaBase = f;

            TurnoNegocio negocio = new TurnoNegocio();          
            List<Turno> listaTurnos = negocio.ListarPorFecha(fechaBase);
            bool cambiosGuardados = false;

            try
            {
                foreach (Turno turno in listaTurnos)
                {
                    int idTurno = turno.IdTurno;
                    string horaInicioStr = Request.Form[$"txtHoraInicio_{idTurno}"];
                    string estadoStr = Request.Form[$"ddlEstado_{idTurno}"];
                    string idMedicoStr = Request.Form[$"ddlMedico_{idTurno}"];


                    if (!string.IsNullOrEmpty(horaInicioStr) && !string.IsNullOrEmpty(estadoStr))
                    {
                        bool modificado = false;


                        if (TimeSpan.TryParse(horaInicioStr, out TimeSpan nuevaHoraInicio))
                        {
                            DateTime nuevaFechaInicio = fechaBase.Date.Add(nuevaHoraInicio);

                            if (turno.FechaHoraInicio != nuevaFechaInicio)
                            {
                                turno.FechaHoraInicio = nuevaFechaInicio;
                                turno.FechaHoraFin = nuevaFechaInicio.AddMinutes(DURACION_TURNO_MINUTOS);
                                modificado = true;
                            }
                        }

                        if (Enum.TryParse(estadoStr, true, out EstadoTurno nuevoEstado) && turno.EstadoTurno != nuevoEstado)
                        {
                            turno.EstadoTurno = nuevoEstado;
                            modificado = true;
                        }

                        if (int.TryParse(idMedicoStr, out int nuevoIdMedico) && turno.IdMedico != nuevoIdMedico)
                        {
                            turno.IdMedico = nuevoIdMedico;
                            modificado = true;
                        }

                        if (modificado)
                        {
                            negocio.Modificar(turno);
                            cambiosGuardados = true;
                        }
                    }
                }
                if (cambiosGuardados)
                {
                    CargarGrillaTurnos(fechaBase);
                }
            }
            catch (Exception ex)
            {
                Session["Error"] = "Error al guardar cambios: " + ex.Message;

            }
        }



        protected void txtFechaGrilla_TextChanged(object sender, EventArgs e)
        {
            DateTime fechaInicial = DateTime.Today;
            if (DateTime.TryParse(txtFechaGrilla.Text, out DateTime fechaSeleccionada))
            {
                CargarGrillaTurnos(fechaSeleccionada);
            }
            else
            {
                // Manejar error de formato de fecha o recargar con fecha actual/default
                CargarGrillaTurnos(DateTime.Today);
                txtFechaGrilla.Text = fechaInicial.ToString("yyyy-MM-dd");
            }
        }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Turno turno = (Turno)e.Row.DataItem;

                HtmlSelect ddlMedico = (HtmlSelect)e.Row.FindControl("ddlMedicoRow");
                HtmlSelect ddlEstado = (HtmlSelect)e.Row.FindControl("ddlEstadoRow");

                if (ddlMedico != null)
                {
                    MedicoNegocio negocio = new MedicoNegocio();
                    List<Medico> medicos = negocio.Listar();

                    ddlMedico.Items.Clear();
                    foreach (Medico m in medicos)
                    {
                        ListItem item = new ListItem(m.Nombre + " " + m.Apellido, m.IdPersona.ToString());

                        if (m.IdPersona == turno.IdMedico)
                            item.Selected = true;
                            ddlMedico.Items.Add(item);
                    }
                }

                if (ddlEstado != null)
                {
                    ddlEstado.Items.Clear();
                    string[] estados = Enum.GetNames(typeof(EstadoTurno));
                    foreach (string est in estados)
                    {
                        ListItem item = new ListItem(est);
                        if (est == turno.EstadoTurno.ToString())
                            item.Selected = true;
                        ddlEstado.Items.Add(item);
                    }
                }
            }
        }
    }

}