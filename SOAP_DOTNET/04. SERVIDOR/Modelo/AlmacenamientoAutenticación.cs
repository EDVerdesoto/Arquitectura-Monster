using System.Collections.Generic;

namespace ConversionesServerWCF.Modelo
{
    public static class AlmacenamientoAutenticación
    {
        // Datos quemados de usuario
        public static Dictionary<string, string> Usuarios = new Dictionary<string, string>()
        {
            { "monster", "monster9" },
        };

        // Tokens activos en memoria
        public static List<string> TokensActivos = new List<string>();

        public static bool ValidarToken(string token)
        {
            return !string.IsNullOrEmpty(token) && TokensActivos.Contains(token);
        }
    }
}
