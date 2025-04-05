using Northwind.DataContext.SqlServer;
using Northwind.EntityModels;

#region  Konfigurera web server host och tjänster
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddNorthwindContext();
builder.Services.AddRequestDecompression();


var app = builder.Build();
#endregion

#region Konfiguration av HTTP pipeline och routing
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.Use(async (HttpContext context, Func<Task> next) =>
{
    RouteEndpoint? rep = context.GetEndpoint() as RouteEndpoint;

    if (rep != null)
    {
        WriteLine($"Endpoint name: {rep.DisplayName}");
        WriteLine($"Endpoint route pattern: {rep.RoutePattern.RawText}");
    }
    if (context.Request.Path == "/Bonjour")
    {
        await context.Response.WriteAsync("Bonjour Monde!");
        return;
    }

    await next();
});


app.UseHttpsRedirection();
app.UseRequestDecompression();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapRazorPages();

app.MapGet("/hello", () => $"Enviroment is {app.Environment.EnvironmentName}");
#endregion

app.Run();
WriteLine("Detta exekveras efter att web server har stoppats!");