using ClienteWeb.Components;
using ClienteWeb.Modelo;
using ClienteWeb.Controlador;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<AuthControlador>(client =>
    client.BaseAddress = new Uri("https://localhost:5001/"));
builder.Services.AddHttpClient<ConversionControlador>(client =>
    client.BaseAddress = new Uri("https://localhost:5001/"));

builder.Services.AddScoped<ServicioSesion>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();