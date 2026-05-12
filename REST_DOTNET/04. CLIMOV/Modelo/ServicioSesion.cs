using System.ComponentModel;

namespace ClienteMovil.Modelo
{
    public class ServicioSesion : INotifyPropertyChanged
    {
        private string? _token;

        public string? Token
        {
            get => _token;
            private set
            {
                if (_token != value)
                {
                    _token = value;
                    OnPropertyChanged(nameof(Token));
                    OnPropertyChanged(nameof(EstaAutenticado));
                }
            }
        }

        public bool EstaAutenticado => !string.IsNullOrEmpty(Token);

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetToken(string token)
        {
            Token = token;
        }

        public void ClearToken()
        {
            Token = null;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
