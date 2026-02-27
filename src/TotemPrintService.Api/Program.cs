var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Totem Print Service");

app.Run("http://localhost:3000");
