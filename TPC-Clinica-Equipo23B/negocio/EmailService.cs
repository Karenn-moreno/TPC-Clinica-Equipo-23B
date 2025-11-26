using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class EmailService
    {

        private MailMessage email;
        private SmtpClient server;

        public EmailService()
        {
            server = new SmtpClient();
            server.Credentials = new NetworkCredential("programacion.correo.prueba@gmail.com", "pgws xrjh qgrh flrf");
            server.EnableSsl = true;
            server.Port = 587; 
            server.Host = "smtp.gmail.com";
        }

        public void armarCorreo(string emailDestino, string asunto, string cuerpo)
        {
            email = new MailMessage();

            email.From = new MailAddress("programacion.correo.prueba@gmail.com", "Clínica Sanare");
            email.To.Add(emailDestino);
            email.Subject = asunto;
            email.IsBodyHtml = true;
            email.Body = cuerpo;
        }

        public void enviarEmail()
        {
            try
            {
                server.Send(email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al enviar el correo con la configuración de Gmail. Verifique credenciales y contraseña de aplicación.", ex);
            }
        }
    }

}

