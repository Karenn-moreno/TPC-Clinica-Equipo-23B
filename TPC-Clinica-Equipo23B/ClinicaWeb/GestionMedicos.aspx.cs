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

                // Guardar especialidades seleccionadas
                var especialidadesSeleccionadas = chkEspecialidades.Items
                    .Cast<ListItem>()
                    .Where(i => i.Selected)
                    .Select(i => i.Text);

                nuevoMedico.EspecialidadesTexto = string.Join(",", especialidadesSeleccionadas);

                // Guardar el médico en la base
                MedicoNegocio negocio = new MedicoNegocio();
                negocio.Agregar(nuevoMedico);   // 👈 Aquí se genera el IdMedico

                // Guardar los horarios cargados temporalmente
                JornadaLaboralNegocio jornadaNegocio = new JornadaLaboralNegocio();
                foreach (var j in HorariosTemp)
                {
                    j.IdMedico = nuevoMedico.IdPersona;   // asignar ID del médico
                    jornadaNegocio.AgregarJornada(j);     // guardar en BD
                }

                // Limpiar lista temporal
                HorariosTemp.Clear();

                // Refrescar grilla de médicos
                CargarGrillaMedicos();

                // Limpiar controles del formulario
                txtNombre.Text = "";
                txtApellido.Text = "";
                txtDni.Text = "";
                txtMatricula.Text = "";
                txtEmail.Text = "";
                txtTelefono.Text = "";
                gvHorariosTemp.DataSource = null;
                gvHorariosTemp.DataBind();

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
            var lista = negocio.listar(); // devolviendo IdEspecialidad y Nombre

            chkEspecialidades.DataSource = lista;
            chkEspecialidades.DataTextField = "Nombre";
            chkEspecialidades.DataValueField = "IdEspecialidad";
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