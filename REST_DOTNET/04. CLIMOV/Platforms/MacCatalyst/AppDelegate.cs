using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ClienteMovil
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
