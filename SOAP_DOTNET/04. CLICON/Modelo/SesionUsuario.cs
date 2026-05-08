namespace ClienteConsola.Modelo
{
    public static class SesionUsuario
    {
        public static string? Token { get; set; }
        public static bool EstaAutenticado => !string.IsNullOrEmpty(Token);
    }
}
