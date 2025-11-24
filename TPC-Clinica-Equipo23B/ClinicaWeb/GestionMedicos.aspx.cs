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
                CargarEspecialidades();
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
            try
            {
                int idMedico = Convert.ToInt32(e.CommandArgument);
                MedicoNegocio negocio = new MedicoNegocio();

                if (e.CommandName == "EditarMedico")
                {
                    // Aquí podes cargar los datos del médico en el modal de edición si querés
                }
                else if (e.CommandName == "EliminarMedico")
                {
                    negocio.Eliminar(idMedico);

                    // Mostrar mensaje tipo alert
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Médico eliminado correctamente');", true);

                    // Recargar grilla para reflejar cambios
                    CargarGrillaMedicos();
                }
                else if (e.CommandName == "VerDetallesMedico")
                {
                    // Obtener datos del médico
                    Medico medico = negocio.Listar().FirstOrDefault(m => m.IdPersona == idMedico);
                    if (medico != null)
                    {
                        lblNombre.Text = medico.Nombre;
                        lblApellido.Text = medico.Apellido;
                        lblDni.Text = medico.Dni;
                        lblEmail.Text = medico.Email;
                        lblMatricula.Text = medico.Matricula;
                        lblTelefono.Text = medico.Telefono;

                        // Mostrar especialidades como texto
                        lblEspecialidades.Text = medico.EspecialidadesTexto;
                    }

                    // Abrir modal de detalles
                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "Popup",
                        "$('#modalDetallesMedico').modal('show');",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error",
                    $"alert('Error al procesar la acción: {ex.Message}');", true);
            }
        }

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear el médico a partir del formulario
                Medico nuevoMedico = new Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    Matricula = txtMatricula.Text,
                    Email = txtEmail.Text,
                    Telefono = txtTelefono.Text
                };

                // Guardar especialidades seleccionadas usando Id
                var especialidadesSeleccionadas = chkEspecialidades.Items
                    .Cast<ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => i.Value);

                nuevoMedico.EspecialidadesTexto = string.Join(",", especialidadesSeleccionadas);

                // Guardar el médico en la base
                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Agregar(nuevoMedico);   // genera IdPersona y lo asigna a nuevoMedico

                // Guardar los horarios cargados temporalmente
                JornadaLaboralNegocio jornadaNegocio = new JornadaLaboralNegocio();
                foreach (var j in HorariosTemp)
                {
                    j.IdMedico = nuevoMedico.IdPersona;   // asignar ID del médico
                    jornadaNegocio.AgregarJornada(j);     // guardar en BD
                }

                // Limpiar lista temporal
                HorariosTemp.Clear();
                gvHorariosTemp.DataSource = null;
                gvHorariosTemp.DataBind();

                // Limpiar controles del formulario
                txtNombre.Text = "";
                txtApellido.Text = "";
                txtDni.Text = "";
                txtMatricula.Text = "";
                txtEmail.Text = "";
                txtTelefono.Text = "";
                foreach (ListItem item in chkEspecialidades.Items)
                    item.Selected = false;

                // Refrescar grilla de médicos
                CargarGrillaMedicos();

                Response.Write("<script>alert('Médico agregado correctamente con sus horarios');</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al guardar el médico: " + ex.Message + "');</script>");
            }
        }


        private List<JornadaLaboral> HorariosTemp
        {
            get
            {
                if (ViewState["HorariosTemp"] == null)
                    ViewState["HorariosTemp"] = new List<JornadaLaboral>();
                return (List<JornadaLaboral>)ViewState["HorariosTemp"];
            }
            set
            {
                ViewState["HorariosTemp"] = value;
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            var lista = negocio.listar(); // devuelve IdEspecialidad y Nombre

            chkEspecialidades.DataSource = lista;
            chkEspecialidades.DataTextField = "Nombre";
            chkEspecialidades.DataValueField = "IdEspecialidad"; // importante: usar Id
            chkEspecialidades.DataBind();
        }

        // Botón Agregar Horario
        protected void btnAgregarHorario_Click(object sender, EventArgs e)
        {
            try
            {
                JornadaLaboral jornada = new JornadaLaboral
                {
                    DiaLaboral = (DiaLaboral)Enum.Parse(typeof(DiaLaboral), ddlDiaLaboral.SelectedValue),
                    HorarioInicio = TimeSpan.Parse(txtHoraInicio.Text),
                    HoraFin = TimeSpan.Parse(txtHoraFin.Text)
                };

                HorariosTemp.Add(jornada);

                gvHorariosTemp.DataSource = HorariosTemp;
                gvHorariosTemp.DataBind();

                txtHoraInicio.Text = "";
                txtHoraFin.Text = "";
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al agregar horario: " + ex.Message + "');</script>");
            }
        }

        // Eliminar horario del GridView temporal
        protected void gvHorariosTemp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                HorariosTemp.RemoveAt(index);
                gvHorariosTemp.DataSource = HorariosTemp;
                gvHorariosTemp.DataBind();
            }
        }

    }
}