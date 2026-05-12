namespace ConversionesServerAPI.Modelo
{
    public static class AlmacenamientoAutenticacion
    {
        public static Dictionary<string, string> Usuarios = new()
        {
            { "monster", "monster9" },
        };

        public static List<string> TokensActivos = [];

        public static bool ValidarToken(string? token)
        {
            return !string.IsNullOrEmpty(token) && TokensActivos.Contains(token);
        }
    }
}