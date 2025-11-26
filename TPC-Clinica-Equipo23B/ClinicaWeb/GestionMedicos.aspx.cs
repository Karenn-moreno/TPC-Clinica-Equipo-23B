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
                    // Cargar datos del médico para edición
                    Medico medico = negocio.Listar().FirstOrDefault(m => m.IdPersona == idMedico);
                    if (medico != null)
                    {
                        txtNombre.Text = medico.Nombre;
                        txtApellido.Text = medico.Apellido;
                        txtDni.Text = medico.Dni;
                        txtMatricula.Text = medico.Matricula;
                        txtEmail.Text = medico.Email;
                        txtTelefono.Text = medico.Telefono;

                        // Marcar especialidades
                        foreach (ListItem item in chkEspecialidades.Items)
                        {
                            item.Selected = medico.EspecialidadesTexto.Split(',')
                                              .Contains(item.Value);
                        }

                        // Cargar horarios temporales para edición
                        HorariosTemp = medico.JornadasLaborales.ToList();
                        gvHorariosTemp.DataSource = HorariosTemp;
                        gvHorariosTemp.DataBind();

                        // Abrir modal edición
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "AbrirModal",
                            "$('#addMedicoModal').modal('show');", true);
                    }
                }
                else if (e.CommandName == "EliminarMedico")
                {
                    negocio.Eliminar(idMedico);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Médico eliminado correctamente');", true);
                    CargarGrillaMedicos();
                }
                else if (e.CommandName == "VerDetallesMedico")
                {
                    Medico medico = negocio.Listar().FirstOrDefault(m => m.IdPersona == idMedico);
                    if (medico != null)
                    {
                        lblNombre.Text = medico.Nombre;
                        lblApellido.Text = medico.Apellido;
                        lblDni.Text = medico.Dni;
                        lblMatricula.Text = medico.Matricula;
                        lblEmail.Text = medico.Email;
                        lblTelefono.Text = medico.Telefono;
                        lblEspecialidades.Text = medico.EspecialidadesTexto;
                        lblHorarios.Text = medico.HorariosTexto;

                        // Abrir modal usando Bootstrap 5 puro
                        ScriptManager.RegisterStartupScript(
                            this,
                            this.GetType(),
                            "PopupDetalles",
                            "var myModal = new bootstrap.Modal(document.getElementById('modalDetallesMedico')); myModal.show();",
                            true
                        );
                    }
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
                MedicoNegocio negocio = new MedicoNegocio();
                Medico medico = new Medico
                {
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Dni = txtDni.Text,
                    Matricula = txtMatricula.Text,
                    Email = txtEmail.Text,
                    Telefono = txtTelefono.Text,
                    EspecialidadesTexto = string.Join(",", chkEspecialidades.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value)),
                    JornadasLaborales = HorariosTemp
                };

                // Guardar nuevo médico
                negocio.Agregar(medico);

                // Limpiar formulario y horarios temporales
                LimpiarFormulario();

                // Recargar grilla
                CargarGrillaMedicos();

                // Cerrar modal y mostrar mensaje
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CerrarModalYMensaje",
                    "alert('Médico agregado correctamente con sus horarios'); $('#addMedicoModal').modal('hide');", true);
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al guardar el médico: " + ex.Message + "');</script>");
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtDni.Text = "";
            txtMatricula.Text = "";
            txtEmail.Text = "";
            txtTelefono.Text = "";
            foreach (ListItem item in chkEspecialidades.Items)
                item.Selected = false;

            HorariosTemp.Clear();
            gvHorariosTemp.DataSource = null;
            gvHorariosTemp.DataBind();
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
            var lista = negocio.listar();
            chkEspecialidades.DataSource = lista;
            chkEspecialidades.DataTextField = "Nombre";
            chkEspecialidades.DataValueField = "IdEspecialidad";
            chkEspecialidades.DataBind();
        }

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