using System;

namespace ClienteConsola.Vista
{
    public class ConversionVista
    {
        public (double valor, string unidadOrigen, string unidadDestino) SolicitarConversionLongitud()
        {
            Console.WriteLine("Unidades disponibles: centimetros, metros, millas, yardas, pies");
            return SolicitarDatosConversion();
        }

        public (double valor, string unidadOrigen, string unidadDestino) SolicitarConversionTemperatura()
        {
            Console.WriteLine("Unidades disponibles: celsius, fahrenheit, kelvin, newton, reaumur");
            return SolicitarDatosConversion();
        }

        public (double valor, string unidadOrigen, string unidadDestino) SolicitarConversionMasa()
        {
            Console.WriteLine("Unidades disponibles: onzas, libras, quintales, gramos, kilogramos");
            return SolicitarDatosConversion();
        }

        public string SolicitarValidarNumerico()
        {
            Console.Write("Ingrese el valor a validar: ");
            return Console.ReadLine() ?? string.Empty;
        }

        public void MostrarResultadoConversion(bool exito, double valorConvertido, string mensajeError)
        {
            if (exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Resultado: {valorConvertido}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {mensajeError}");
                Console.ResetColor();
            }
        }

        public void MostrarResultadoValidacion(bool exito, bool esValido, string mensajeError)
        {
            if (exito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Es valido: {(esValido ? "Si" : "No")}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {mensajeError}");
                Console.ResetColor();
            }
        }

        private static (double valor, string unidadOrigen, string unidadDestino) SolicitarDatosConversion()
        {
            double valor = 0;
            bool valido = false;
            while (!valido)
            {
                Console.Write("Valor: ");
                valido = double.TryParse(Console.ReadLine(), out valor);
                if (!valido)
                    Console.WriteLine("Valor numerico invalido. Intente de nuevo.");
            }

            Console.Write("Unidad de origen: ");
            string unidadOrigen = Console.ReadLine() ?? string.Empty;

            Console.Write("Unidad de destino: ");
            string unidadDestino = Console.ReadLine() ?? string.Empty;

            return (valor, unidadOrigen, unidadDestino);
        }
    }
}
