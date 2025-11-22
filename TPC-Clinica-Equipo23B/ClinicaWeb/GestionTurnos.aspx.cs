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
                CargarDatosEstaticos();
                txtFechaGrilla.Value = DateTime.Today.ToString("yyyy-MM-dd");
                CargarGrillaTurnos(DateTime.Today);
            }
        }

        private void CargarDatosEstaticos()
        {
            try
            {

                CargarEspecialidades();
                CargarPacientes();
                CargarMedicos(0);
                CargarMedicosParaGrilla();
                CargarEstadosParaGrilla();
            }
            catch (Exception ex)
            {
                Session["Error"] = "Error al inicializar la gestión de turnos: " + ex.Message;

            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
            List<Especialidad> lista = especialidadNegocio.listar();
            ddlEspecialidad.DataSource = lista;
            ddlEspecialidad.DataTextField = "Nombre";
            ddlEspecialidad.DataValueField = "IdEspecialidad";
            ddlEspecialidad.DataBind();
            ddlEspecialidad.Items.Insert(0, new ListItem("Seleccione Especialidad", "0"));
        }

        private void CargarPacientes()
        {
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            List<Paciente> lista = pacienteNegocio.Listar();

            ddlPaciente.DataSource = lista.Select(p => new
            {
                Id = p.IdPersona,
                NombreCompleto = p.Nombre + " " + p.Apellido
            });
            ddlPaciente.DataTextField = "NombreCompleto";
            ddlPaciente.DataValueField = "Id";
            ddlPaciente.DataBind();
            ddlPaciente.Items.Insert(0, new ListItem("Seleccione Paciente", "0"));
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

            string diaString = fechaSeleccionada.DayOfWeek.ToString();
            if (!Enum.TryParse(diaString, true, out DiaLaboral dia))
                return;

            JornadaLaboralNegocio jornadaNegocio = new JornadaLaboralNegocio();
            List<JornadaLaboral> jornadas = jornadaNegocio.ListarPorMedico(idMedico); 

            
            var jornadaHoy = jornadas.FirstOrDefault(j => j.DiaLaboral == dia);

            if (jornadaHoy == null)
            {
                ddlHorario.Items.Insert(0, new ListItem("No trabaja este día", "0"));
                return;
            }

            
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            List<Turno> turnosOcupados = turnoNegocio.ListarTurnosOcupados(idMedico, fechaSeleccionada);

            TimeSpan inicio = jornadaHoy.HorarioInicio;
            TimeSpan fin = jornadaHoy.HoraFin;


            while (inicio.Add(TimeSpan.FromMinutes(DURACION_TURNO_MINUTOS)) <= fin)
            {
                TimeSpan horaFinTurno = inicio.Add(TimeSpan.FromMinutes(DURACION_TURNO_MINUTOS));

                
                bool ocupado = turnosOcupados.Any(t =>
                    t.FechaHoraInicio.TimeOfDay == inicio); 

                if (!ocupado)
                {
                    string textoHora = inicio.ToString(@"hh\:mm") + " - " + horaFinTurno.ToString(@"hh\:mm");
                    ddlHorario.Items.Add(new ListItem(textoHora, inicio.ToString(@"hh\:mm")));
                }

                inicio = horaFinTurno;
            }

            if (ddlHorario.Items.Count == 0)
            {
                ddlHorario.Items.Insert(0, new ListItem("No hay horarios disponibles", "0"));
            }
            else
            {
                ddlHorario.Items.Insert(0, new ListItem("Seleccione Horario", "0"));
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
                lblErrorNuevoTurno.Text = "";

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
                Response.Redirect("GestionTurnos.aspx", false);

            }
            catch (Exception ex)
            {
                // En caso de fallo de DB o email
                Session["Error"] = "Error al guardar el turno o enviar el correo: " + ex.Message;
             

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
            if (DateTime.TryParse(txtFechaGrilla.Value, out DateTime fechaSeleccionada))
            {
                fechaBase = fechaSeleccionada;
            }

            if (Session["ListaTurnosDia"] == null)
            {
                // Manejo de error si no hay turnos cargados
                return;
            }

            List<Turno> listaTurnos = (List<Turno>)Session["ListaTurnosDia"];
            TurnoNegocio turnoNegocio = new TurnoNegocio();
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
                        // Conversión de la hora
                        if (TimeSpan.TryParse(horaInicioStr, out TimeSpan nuevaHoraInicio))
                        {
                            // La fecha base se combina con la nueva hora
                            DateTime nuevaFechaHoraInicio = fechaBase.Date.Add(nuevaHoraInicio);


                            DateTime nuevaFechaHoraFin = nuevaFechaHoraInicio.Add(TimeSpan.FromMinutes(DURACION_TURNO_MINUTOS));

                            // Conversión del estado
                            if (Enum.TryParse(estadoStr, true, out EstadoTurno nuevoEstado))
                            {

                                turno.FechaHoraInicio = nuevaFechaHoraInicio;
                                turno.FechaHoraFin = nuevaFechaHoraFin;
                                turno.EstadoTurno = nuevoEstado;

                                if (int.TryParse(idMedicoStr, out int nuevoIdMedico))
                                {
                                    turno.IdMedico = nuevoIdMedico;
                                }
                                // Mantenemos el ID de paciente.



                                turnoNegocio.Modificar(turno);
                                cambiosGuardados = true;
                            }
                        }
                    }
                }

                if (cambiosGuardados)
                {
                    // Recargar la grilla para reflejar los cambios guardados
                    CargarGrillaTurnos(fechaBase);

                }

            }
            catch (Exception ex)
            {
                Session["Error"] = "Error al guardar los cambios en la grilla: " + ex.Message;
                Response.Redirect("Error.aspx");
            }
        }

        private void CargarMedicosParaGrilla()
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            List<Medico> listaMedicos = medicoNegocio.Listar();

            // Crear una lista anónima para el DDL (Id, NombreCompleto)
            var medicosDDL = listaMedicos
               .Select(m => new
               {
                   IdMedico = m.IdPersona,
                   NombreCompleto = "Dr/a. " + m.Nombre + " " + m.Apellido
               })
               .ToList();

            Session["ListaMedicosGrilla"] = medicosDDL;
        }

        // Nuevo método para cargar estados de turno para el DropDownList de la grilla
        private void CargarEstadosParaGrilla()
        {

            List<string> estados = Enum.GetNames(typeof(EstadoTurno)).ToList();
            Session["ListaEstadosTurno"] = estados;
        }

        protected void txtFechaGrilla_TextChanged(object sender, EventArgs e)
        {
            if (DateTime.TryParse(txtFechaGrilla.Value, out DateTime fechaSeleccionada))
            {
                CargarGrillaTurnos(fechaSeleccionada);
            }
            else
            {
                // Manejar error de formato de fecha o recargar con fecha actual/default
                CargarGrillaTurnos(DateTime.Today);
                txtFechaGrilla.Value = DateTime.Today.ToString("yyyy-MM-dd");
            }
        }

        protected void gvTurnos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Turno turno = (Turno)e.Row.DataItem;

                HtmlSelect ddlMedico = (HtmlSelect)e.Row.FindControl("ddlMedicoGrid_" + turno.IdTurno);
                if (ddlMedico != null && Session["ListaMedicosGrilla"] != null)
                {
                    var listaMedicos = (IEnumerable<dynamic>)Session["ListaMedicosGrilla"];

                    ddlMedico.Items.Clear();
                    foreach (var medico in listaMedicos)
                    {
                        ListItem item = new ListItem(medico.NombreCompleto, medico.IdMedico.ToString());
                        if (medico.IdMedico == turno.IdMedico)
                        {
                            item.Selected = true;
                        }
                        ddlMedico.Items.Add(item);
                    }
                }

                //Configurar DropDownList de Estado 
                HtmlSelect ddlEstado = (HtmlSelect)e.Row.FindControl("ddlEstadoGrid_" + turno.IdTurno);
                if (ddlEstado != null && Session["ListaEstadosTurno"] != null)
                {
                    List<string> listaEstados = (List<string>)Session["ListaEstadosTurno"];

                    ddlEstado.Items.Clear();
                    foreach (string estado in listaEstados)
                    {
                        ListItem item = new ListItem(estado, estado);
                        if (estado == turno.EstadoTurno.ToString())
                        {
                            item.Selected = true;
                        }
                        ddlEstado.Items.Add(item);
                    }
                }
            }
        }

}
       
}