using Dapper;
using ApiInnovacion.Repositorios;
using ApiInnovacion.Servicios;


DefaultTypeMap.MatchNamesWithUnderscores = true;
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
builder.Services.AddScoped<IRepositorioFacultad, RepositorioFacultadSqlServer>();
builder.Services.AddScoped<IRepositorioPrograma, RepositorioProgramaSqlServer>();
builder.Services.AddScoped<IRepositorioAcreditacion, RepositorioAcreditacionSqlServer>();
builder.Services.AddScoped<IRepositorioRegistroCalificado, RepositorioRegistroCalificadoSqlServer>();
builder.Services.AddScoped<IServicioRegistroCalificado, ServicioRegistroCalificado>();
builder.Services.AddScoped<IRepositorioPasantia, RepositorioPasantiaSqlServer>();
builder.Services.AddScoped<IRepositorioActivAcademica, RepositorioActivAcademicaSqlServer>();
builder.Services.AddScoped<IRepositorioPremio, RepositorioPremioSqlServer>();
builder.Services.AddScoped<IRepositorioDocenteDepartamento, RepositorioDocenteDepartamentoSqlServer>();
builder.Services.AddScoped<IRepositorioAlianza, RepositorioAlianzaSqlServer>();

// Tablas Puente
builder.Services.AddScoped<IRepositorioTablaPuente, RepositorioTablaPuenteSqlServer>();

// 2. Servicios
builder.Services.AddScoped<IServicioUniversidad, ServicioUniversidad>();
builder.Services.AddScoped<IServicioAliado, ServicioAliado>();
builder.Services.AddScoped<IServicioAreaConocimiento, ServicioAreaConocimiento>();
builder.Services.AddScoped<IServicioAspectoNormativo, ServicioAspectoNormativo>();
builder.Services.AddScoped<IServicioPracticaEstrategia, ServicioPracticaEstrategia>();
builder.Services.AddScoped<IServicioEnfoque, ServicioEnfoque>();
builder.Services.AddScoped<IServicioCarInnovacion, ServicioCarInnovacion>();
builder.Services.AddScoped<IServicioFacultad, ServicioFacultad>();
builder.Services.AddScoped<IServicioPrograma, ServicioPrograma>();
builder.Services.AddScoped<IServicioAcreditacion, ServicioAcreditacion>();
builder.Services.AddScoped<IServicioActivAcademica, ServicioActivAcademica>();
builder.Services.AddScoped<IServicioPasantia, ServicioPasantia>();
builder.Services.AddScoped<IServicioPremio, ServicioPremio>();
builder.Services.AddScoped<IServicioDocenteDepartamento, ServicioDocenteDepartamento>();
builder.Services.AddScoped<IServicioAlianza, ServicioAlianza>();
builder.Services.AddScoped<IServicioTablaPuente, ServicioTablaPuente>();


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