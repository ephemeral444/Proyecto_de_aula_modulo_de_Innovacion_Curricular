using ApiInnovacion.Repositorios;
using ApiInnovacion.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// INYECCI�N DE DEPENDENCIAS
// 1. Repositorios
builder.Services.AddScoped<IRepositorioUniversidad, RepositorioUniversidadSqlServer>();
builder.Services.AddScoped<IRepositorioAliado, RepositorioAliadoSqlServer>();
builder.Services.AddScoped<IRepositorioAreaConocimiento, RepositorioAreaConocimientoSqlServer>();
builder.Services.AddScoped<IRepositorioAspectoNormativo, RepositorioAspectoNormativoSqlServer>();
builder.Services.AddScoped<IRepositorioPracticaEstrategia, RepositorioPracticaEstrategiaSqlServer>();
builder.Services.AddScoped<IRepositorioEnfoque, RepositorioEnfoqueSqlServer>();
builder.Services.AddScoped<IRepositorioCarInnovacion, RepositorioCarInnovacionSqlServer>();

// 2. Servicios
builder.Services.AddScoped<IServicioUniversidad, ServicioUniversidad>();
builder.Services.AddScoped<IServicioAliado, ServicioAliado>();
builder.Services.AddScoped<IServicioAreaConocimiento, ServicioAreaConocimiento>();
builder.Services.AddScoped<IServicioAspectoNormativo, ServicioAspectoNormativo>();
builder.Services.AddScoped<IServicioPracticaEstrategia, ServicioPracticaEstrategia>();
builder.Services.AddScoped<IServicioEnfoque, ServicioEnfoque>();
builder.Services.AddScoped<IServicioCarInnovacion, ServicioCarInnovacion>();

var app = builder.Build();

// Configuraci�n de Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Innovaci�n v1");
    c.RoutePrefix = "swagger";
});

// ORDEN CR�TICO DE MIDDLEWARES
app.UseCors("PermitirTodo");
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();