using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace negocio
{
    public class SeguridadService
    {
        //HASH es para que no puedan descubrir la contraseña desde la base de datos (segun lo que entedi)
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Función para verificar una contraseña ingresada con el hash almacenado
        public static bool VerificarPassword(string passwordIngresada, string hashAlmacenado)
        {
            if (string.IsNullOrEmpty(hashAlmacenado))
                return false;

            string hashIngresado = HashPassword(passwordIngresada);

            return hashIngresado.Equals(hashAlmacenado, StringComparison.OrdinalIgnoreCase);
        }
    }

}

