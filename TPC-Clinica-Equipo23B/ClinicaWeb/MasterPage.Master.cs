using dominio;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClinicaWeb
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null || Session["rol"] == null)
            {
                if (!(Page is Login) && !(Page is Registrarse))
                {
                    Response.Redirect("Login.aspx", false);
                }
                return;
            }

            if (!IsPostBack)
            {               
                Usuario user = (Usuario)Session["usuario"];
                string rol = Session["rol"].ToString().ToUpper();

                menuTurnos.Visible = true;
                
                menuPacientes.Visible = false;
                menuMedicos.Visible = false;
                menuUsuarios.Visible = false;
                //desocultamos menus segun rol
                if (rol == "ADMINISTRADOR" || rol == "RECEPCIONISTA")
                {
                    menuPacientes.Visible = true;
                    menuMedicos.Visible = true;
                }
                if (rol == "ADMINISTRADOR")
                {
                    menuUsuarios.Visible = true;
                }
            }
        }
        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }
    }
}