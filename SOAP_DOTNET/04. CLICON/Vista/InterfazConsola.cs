using System;

namespace ClienteConsola.Vista
{
    public class InterfazConsola
    {
        public OpcionMenu? seleccionarConversion()
        {
            Console.WriteLine("\n--- Menu de Conversion de Unidades ---");

            foreach (OpcionMenu opcion in OpcionMenu.values())
            {
                Console.WriteLine(opcion.getNumero() + ". " + opcion.getDescripcion());
            }

            Console.Write("Selecciona una opcion: ");
            string seleccion = Console.ReadLine() ?? string.Empty;

            foreach (OpcionMenu opcion in OpcionMenu.values())
            {
                if (opcion.getNumero().Equals(seleccion))
                {
                    return opcion;
                }
            }

            Console.WriteLine("Opcion invalida. Intenta de nuevo.");
            return null;
        }

        public T? seleccionarUnidad<T>(string titulo, T[] opciones) where T : struct, Enum
        {
            Console.WriteLine("\n--- " + titulo + " ---");
            for (int i = 0; i < opciones.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + opciones[i].ToString());
            }
            Console.Write("Elige una unidad: ");

            try
            {
                int seleccion = int.Parse(Console.ReadLine() ?? string.Empty);
                if (seleccion >= 1 && seleccion <= opciones.Length)
                {
                    return opciones[seleccion - 1];
                }
            }
            catch (FormatException)
            {
                // Ignorar para que caiga en el error de abajo
            }

            Console.WriteLine("Unidad invalida. Operacion cancelada.");
            return null;
        }

        public double? pedirValorAConvertir()
        {
            Console.Write("\nIngresa el valor a convertir: ");
            try
            {
                return double.Parse(Console.ReadLine() ?? string.Empty);
            }
            catch (FormatException)
            {
                Console.WriteLine("Debe ser un numero valido.");
                return null;
            }
        }

        public string pedirTexto(string mensaje)
        {
            Console.Write(mensaje);
            return (Console.ReadLine() ?? string.Empty).Trim();
        }
    }
}
