namespace ClienteWeb.Modelo
{
    public class ServicioSesion
    {
        private string? _token;

        public string? Token
        {
            get => _token;
            private set
            {
                _token = value;
                NotifyStateChanged();
            }
        }

        public bool EstaAutenticado => !string.IsNullOrEmpty(Token);

        public event Action? OnChange;

        public void SetToken(string token)
        {
            Token = token;
        }

        public void ClearToken()
        {
            Token = null;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
