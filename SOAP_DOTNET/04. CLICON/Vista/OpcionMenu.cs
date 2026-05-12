namespace ClienteConsola.Vista
{
    public sealed class OpcionMenu
    {
        public static readonly OpcionMenu CONVERTIR_LONGITUD = new OpcionMenu("1", "Convertir Longitud");
        public static readonly OpcionMenu CONVERTIR_MASA = new OpcionMenu("2", "Convertir Masa");
        public static readonly OpcionMenu CONVERTIR_TEMPERATURA = new OpcionMenu("3", "Convertir Temperatura");
        public static readonly OpcionMenu VALIDAR_NUMERICO = new OpcionMenu("4", "Validar Campo Numerico");
        public static readonly OpcionMenu SALIR = new OpcionMenu("5", "Salir");

        private readonly string numero;
        private readonly string descripcion;

        private OpcionMenu(string numero, string descripcion)
        {
            this.numero = numero;
            this.descripcion = descripcion;
        }

        public string getNumero() => numero;
        public string getDescripcion() => descripcion;

        public static OpcionMenu[] values() => new[]
        {
            CONVERTIR_LONGITUD,
            CONVERTIR_MASA,
            CONVERTIR_TEMPERATURA,
            VALIDAR_NUMERICO,
            SALIR
        };
    }
}
