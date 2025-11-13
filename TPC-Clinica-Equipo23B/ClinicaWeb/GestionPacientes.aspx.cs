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
    public partial class GestionPacientes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }
    }
  }
