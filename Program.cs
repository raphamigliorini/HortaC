#region IMPORTAÇÃO REFERENTE AO BANCO DE DADOS
using Fiap.Web.Alunos.Data.Contexts;
using FiapWebAluno.Service.Implementations;
using FiapWebAluno.Service.Interface;
using Microsoft.EntityFrameworkCore;
#endregion

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEspecieService, EspecieService>();
builder.Services.AddScoped<ICanteiroService, CanteiroService>();
builder.Services.AddScoped<IIrrigacaoService, IrrigacaoService>();
builder.Services.AddScoped<ISensorService, SensorService>();


var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<DatabaseContext>(opt =>
{
    opt.UseOracle(connectionString);

    // opcional: logs sensíveis só no dev
    if (builder.Environment.IsDevelopment())
    {
        opt.EnableSensitiveDataLogging(true);
    }
});

builder.Services.AddScoped<ICanteiroService, CanteiroService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    try
    {
        var ok = db.Database.CanConnect();
        Console.WriteLine($"✅ Conectou no Oracle? {ok}");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Falhou ao conectar no Oracle:");
        Console.WriteLine(ex.Message);
    }
}


app.Run();
