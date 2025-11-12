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
                CargarGrillaMedicos();
            }

        }
        private void CargarGrillaMedicos()
        {
            MedicoNegocio negocio = new MedicoNegocio();
            List<Medico> lista = negocio.Listar();
            gvMedicos.DataSource = lista;
            gvMedicos.DataBind();
        }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GestionarHorarios")
            {
                // Obtenemos el ID del médico
                int idMedico = Convert.ToInt32(e.CommandArgument);

                // Redirigimos a la página de horarios
                Response.Redirect($"GestionHorarios.aspx?idmedico={idMedico}", false);
            }

            if (e.CommandName == "EditarMedico")
            {
                // Lógica para el modal de editar
            }
        }
    }
}