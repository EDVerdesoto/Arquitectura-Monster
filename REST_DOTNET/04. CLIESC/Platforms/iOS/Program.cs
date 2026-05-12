using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ClienteEscritorio
{
    public class Program
    {
        static void Main(string[] args)
        {
            var app = new MauiProgram().CreateMauiApp();
            app.Run(args);
        }
    }
}
