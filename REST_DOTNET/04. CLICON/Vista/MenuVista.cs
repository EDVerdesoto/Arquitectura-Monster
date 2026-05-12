using System;

namespace ClienteConsola.Vista
{
    public class MenuVista
    {
        public void MostrarEncabezado()
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("   CLIENTE CONSOLA - WCF CONVERSIONES   ");
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        public int MostrarMenuPrincipal(bool autenticado)
        {
            MostrarEncabezado();

            if (autenticado)
            {
                Console.WriteLine("1. Cerrar Sesion");
                Console.WriteLine("2. Conversion de Longitud");
                Console.WriteLine("3. Conversion de Temperatura");
                Console.WriteLine("4. Conversion de Masa");
                Console.WriteLine("5. Validar Campo Numerico");
                Console.WriteLine("6. Salir");
            }
            else
            {
                Console.WriteLine("1. Iniciar Sesion");
                Console.WriteLine("2. Salir");
            }

            Console.WriteLine();
            Console.Write("Seleccione una opcion: ");
            var entrada = Console.ReadLine();
            if (int.TryParse(entrada, out int opcion))
                return opcion;
            return -1;
        }

        public void MostrarMensaje(string mensaje)
        {
            Console.WriteLine(mensaje);
        }

        public void MostrarError(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {error}");
            Console.ResetColor();
        }

        public void MostrarExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public void Pausa()
        {
            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para continuar...");
            Console.ReadKey(true);
        }
    }
}
