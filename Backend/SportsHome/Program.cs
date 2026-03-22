using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using SportsHome.Core.Entities;
using SportsHome.Core.Interfaces;
using SportsHome.Core.Interfaces.Services;
using SportsHome.Core.Services;
using SportsHome.IL.Repository.EF;
using SportsHome.UI.Controllers.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Repositorios y Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicios
builder.Services.AddScoped<ILigasService, LigasService>();

// AutoMapper - escanea todos los perfiles en los assemblies de SportsHome
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
builder.Services.AddDbContext<SportsHomeContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
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