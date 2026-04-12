using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Services;
using SportsHome.IL.Repository.EF;
using SportsHome.UI.API;
using SportsHome.UI.Controllers.Helpers;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Repositorios y Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicios
builder.Services.AddScoped<ILigasService, LigasService>();
builder.Services.AddScoped<IEquiposService, EquiposService>();
builder.Services.AddScoped<IJugadoresService, JugadoresService>();
builder.Services.AddScoped<IPartidosService, PartidosService>();
builder.Services.AddScoped<IEventosPartidosService, EventosPartidosService>();
builder.Services.AddScoped<IEstadisticasJugadoresService, EstadisticasJugadoresService>();
builder.Services.AddScoped<IClasificacionesService, ClasificacionesService>();
builder.Services.AddScoped<ISyncLogsService, SyncLogsService>();

// Servicio para consumir API Football
builder.Services.AddHttpClient<IApiFootball, ApiFootballService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiFootball:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("x-apisports-key", builder.Configuration["ApiFootball:ApiKey"]);
});

// Sincronización automática al arrancar (respeta cooldowns)
builder.Services.AddHostedService<BackgroundSyncService>();

// AutoMapper - registrar perfiles automáticamente
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Auto-descubrimiento de controladores: carga todos los assemblies SportsHome.*.dll
// del directorio de salida que contengan controladores. Así no hay que añadirlos uno a uno.
var mvcBuilder = builder.Services.AddControllers();

var appDir = AppDomain.CurrentDomain.BaseDirectory;
Directory.GetFiles(appDir, "SportsHome.*.dll")
    .Select(path =>
    {
        try { return Assembly.LoadFrom(path); }
        catch { return null; }
    })
    .Where(a => a is not null &&
                a.GetTypes().Any(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract))
    .ToList()
    .ForEach(a => mvcBuilder.AddApplicationPart(a!));

mvcBuilder
    .AddOData(options => options
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .Count()
        .SetMaxTop(null)
        .AddRouteComponents("api/odata", GetEdmModel())
    )
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    // XmlSerializerFormatters eliminado: incompatible con los tipos internos de OData
    ;

// API Explorer necesario para que Swagger descubra los endpoints
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // OData genera rutas duplicadas en api/odata/* que colisionan con los métodos
    // REST del mismo controlador. Se toma el primer descriptor en caso de conflicto.
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

// DbContext + MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Use explicit MySQL server version to avoid AutoDetect (which opens a connection at startup)
var serverVersion = new MySqlServerVersion(new Version(5, 7, 0));
builder.Services.AddDbContext<SportsHomeContext>(options =>
    options.UseMySql(connectionString, serverVersion, mySqlOptions =>
    {
        // Enable transient error resiliency for transient DB failures
        mySqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
    })
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information)
);

var app = builder.Build();

// Swagger UI accesible en la raíz: https://localhost:{puerto}/
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SportsHome API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseCors("Angular");
app.UseAuthorization();
app.MapControllers();

app.Run();

static IEdmModel GetEdmModel()
{
    var odataBuilder = new ODataConventionModelBuilder();
    // Clave definida explícitamente porque la convención espera "LigasId" pero la propiedad es "LigaId"
    odataBuilder.EntitySet<Ligas>("Ligas").EntityType.HasKey(l => l.LigaId);
    return odataBuilder.GetEdmModel();
}