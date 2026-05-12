using ClienteConsola.Vista;
using ClienteConsola.Controlador;
using ClienteConsola.Modelo;

namespace ClienteConsola
{
    public class AppControlador
    {
        private readonly MenuVista _menuVista;
        private readonly AutenticacionControlador _authControlador;
        private readonly ConversionControlador _conversionControlador;
        private bool _salir;

        public AppControlador()
        {
            _menuVista = new MenuVista();
            _authControlador = new AutenticacionControlador(new AutenticacionVista());
            _conversionControlador = new ConversionControlador(new ConversionVista());
        }

        public void Run()
        {
            while (!_salir)
            {
                int opcion = _menuVista.MostrarMenuPrincipal(SesionUsuario.EstaAutenticado);
                ProcesarOpcion(opcion);

                if (!_salir)
                {
                    _menuVista.Pausa();
                }
            }
        }

        private void ProcesarOpcion(int opcion)
        {
            if (SesionUsuario.EstaAutenticado)
            {
                switch (opcion)
                {
                    case 1:
                        _authControlador.CerrarSesion();
                        break;
                    case 2:
                        _conversionControlador.ConvertirLongitud();
                        break;
                    case 3:
                        _conversionControlador.ConvertirTemperatura();
                        break;
                    case 4:
                        _conversionControlador.ConvertirMasa();
                        break;
                    case 5:
                        _conversionControlador.ValidarCampoNumerico();
                        break;
                    case 6:
                        _salir = true;
                        _menuVista.MostrarMensaje("Saliendo de la aplicacion...");
                        break;
                    default:
                        _menuVista.MostrarError("Opcion invalida.");
                        break;
                }
            }
            else
            {
                switch (opcion)
                {
                    case 1:
                        _authControlador.IniciarSesion();
                        break;
                    case 2:
                        _salir = true;
                        _menuVista.MostrarMensaje("Saliendo de la aplicacion...");
                        break;
                    default:
                        _menuVista.MostrarError("Opcion invalida.");
                        break;
                }
            }
        }
    }
}
