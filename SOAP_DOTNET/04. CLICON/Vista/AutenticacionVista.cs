using System;

namespace ClienteConsola.Vista
{
    public class AutenticacionVista
    {
        public (string usuario, string clave) SolicitarCredenciales()
        {
            Console.Write("Usuario: ");
            string usuario = Console.ReadLine() ?? string.Empty;
            Console.Write("Contrasena: ");
            string clave = LeerContrasena();
            return (usuario, clave);
        }

        public (string usuario, string claveAntigua, string claveNueva) SolicitarCambioClave()
        {
            Console.Write("Usuario: ");
            string usuario = Console.ReadLine() ?? string.Empty;
            Console.Write("Contrasena antigua: ");
            string claveAntigua = LeerContrasena();
            Console.Write("Contrasena nueva: ");
            string claveNueva = LeerContrasena();
            return (usuario, claveAntigua, claveNueva);
        }

        public string SolicitarRecuperarClave()
        {
            Console.Write("Usuario: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public void MostrarResultadoLogin(bool exito, string token, string mensajeError)
        {
            if (exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sesion iniciada correctamente.");
                Console.WriteLine($"Token: {token}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al iniciar sesion: {mensajeError}");
                Console.ResetColor();
            }
        }

        public void MostrarResultadoCambioClave(bool exito, string mensajeError)
        {
            if (exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Contrasena cambiada correctamente.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al cambiar contrasena: {mensajeError}");
                Console.ResetColor();
            }
        }

        public void MostrarResultadoRecuperarClave(bool exito, string claveRecuperada, string mensajeError)
        {
            if (exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Contrasena recuperada: {claveRecuperada}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error al recuperar contrasena: {mensajeError}");
                Console.ResetColor();
            }
        }

        private static string LeerContrasena()
        {
            string clave = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
                {
                    clave += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && clave.Length > 0)
                {
                    clave = clave.Substring(0, clave.Length - 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return clave;
        }
    }
}
