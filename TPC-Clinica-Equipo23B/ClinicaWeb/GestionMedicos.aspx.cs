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
            ConfigurarPermisosBaja();
            if (!IsPostBack)
            {
                CargarEspecialidades();
                CargarGrillaMedicos();
            }
        }

        private void ConfigurarPermisosBaja()
        {
            btnEliminarFisico.Style.Add("display", "none");

            if (Session["rol"] != null && Session["rol"].ToString().ToUpper() == "ADMINISTRADOR")
            {
                btnEliminarFisico.Style.Remove("display");
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
                    negocio.EliminarLogico(idMedico);
                    ClientScript.RegisterStartupScript(this.GetType(), "alert",
                        "alert('Médico dado de baja (lógica) correctamente');", true);
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
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");

                string script = $"mostrarMensajeError('{mensaje}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", script, true);
            }
        }

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {
            try
            {
                //Validaciones
                List<string> errores = new List<string>();

                // Campos obligatorios
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    errores.Add("El nombre es obligatorio.");
                if (string.IsNullOrWhiteSpace(txtApellido.Text))
                    errores.Add("El apellido es obligatorio.");
                if (string.IsNullOrWhiteSpace(txtDni.Text))
                    errores.Add("El DNI es obligatorio.");
                else if (!long.TryParse(txtDni.Text, out _))
                    errores.Add("El DNI debe ser numérico.");

                // Email válido
                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    try
                    {
                        var addr = new System.Net.Mail.MailAddress(txtEmail.Text);
                    }
                    catch
                    {
                        errores.Add("Email inválido.");
                    }
                }

                // Teléfono válido
                if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && !System.Text.RegularExpressions.Regex.IsMatch(txtTelefono.Text, @"^\+?\d+$"))
                    errores.Add("Teléfono inválido, solo números y opcional + inicial.");

                // Horarios
                if (HorariosTemp.Count == 0)
                    errores.Add("Debe agregar al menos un horario de atención.");
                else
                {
                    foreach (var j in HorariosTemp)
                    {
                        if (j.HorarioInicio >= j.HoraFin)
                        {
                            errores.Add($"Horario inválido para {j.DiaLaboral}: la hora de inicio debe ser menor a la hora de fin.");
                        }
                    }
                }

                // Mostrar errores si hay y salir
                if (errores.Any())
                {
                    string mensaje = string.Join("\\n", errores);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Validaciones", $"mostrarMensajeError('{mensaje}');", true);
                    return;
                }

                
                
                MedicoNegocio negocio = new MedicoNegocio();
                Medico medico = new Medico
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Dni = txtDni.Text.Trim(),
                    Matricula = txtMatricula.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    EspecialidadesTexto = string.Join(",", chkEspecialidades.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value)),
                    JornadasLaborales = HorariosTemp
                };

                string mensajeExito = "";

                if (!string.IsNullOrEmpty(hfIdMedicoEditar.Value)) // Edición
                {
                    medico.IdPersona = int.Parse(hfIdMedicoEditar.Value);
                    negocio.Modificar(medico);
                    mensajeExito = "Médico modificado correctamente";
                }
                else // Nuevo
                {
                    negocio.Agregar(medico);
                    mensajeExito = "Médico agregado correctamente";
                }

                // Limpiar formulario y recargar grilla
                LimpiarFormulario();
                CargarGrillaMedicos();

                // ---------------------------
                // CERRAR MODAL Y MOSTRAR MENSAJE
                // ---------------------------
                string scriptCerrar = $@"
            var modalEl = document.getElementById('addMedicoModal');
            var modal = bootstrap.Modal.getInstance(modalEl);
            if(modal) {{ modal.hide(); }}
            alert('{mensajeExito}');
        ";
                ScriptManager.RegisterStartupScript(upModalMedico, upModalMedico.GetType(), "CerrarModalYMensaje", scriptCerrar, true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ErrorUI", $"mostrarMensajeError('{mensaje}');", true);
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

        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int idMedico = Convert.ToInt32(hfIdMedicoEliminar.Value);
                MedicoNegocio negocio = new MedicoNegocio();

                negocio.EliminarLogico(idMedico);

                CargarGrillaMedicos();
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
                int idMedico = Convert.ToInt32(hfIdMedicoEliminar.Value);
                MedicoNegocio negocio = new MedicoNegocio();

                // Lógica de eliminación física (solo Admin)
                negocio.EliminarFisico(idMedico);

                CargarGrillaMedicos();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "cerrarModal", "var myModalEl = document.getElementById('modalConfirmarEliminar'); var modal = bootstrap.Modal.getInstance(myModalEl); modal.hide(); limpiarFondosResiduales();", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertExito", "alert('El registro fue eliminado definitivamente de la base de datos.');", true);

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message.Replace("'", "").Replace("\n", " ");
                // Cerrar modal de confirmación y abrir modal de error
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