namespace ClienteMovil.Modelo
{
    public enum TipoConversion
    {
        Longitud,
        Temperatura,
        Masa
    }

    public static class UnidadesConversion
    {
        public static readonly Dictionary<TipoConversion, List<string>> Unidades = new()
        {
            [TipoConversion.Longitud] = new() { "centimetros", "metros", "millas", "yardas", "pies" },
            [TipoConversion.Temperatura] = new() { "celsius", "fahrenheit", "kelvin", "newton", "reaumur" },
            [TipoConversion.Masa] = new() { "onzas", "libras", "quintales", "gramos", "kilogramos" }
        };
    }
}