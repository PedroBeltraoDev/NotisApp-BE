using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NotesApp.Api.Data;
using NotesApp.Api.Repositories;
using NotesApp.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NoteesApp API",
        Version = "v1",
        Description = "API RESTful para gerenciamento de notas com organização por pastas e tags",
        Contact = new OpenApiContact
        {
            Name = "Pedro Beltrão GitHub",
            Email = "pedrobeltraodev@gmail.com",
            Url = new Uri("https://github.com/PedroBeltraoDev")
        }
    });
});

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

// Entity Framework com PostgreSQL (Neon)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "https://notees-app-ui.vercel.app",
            "http://localhost:5173",
            "http://localhost:4173"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

//Swagger
if (app.Environment.IsDevelopment() || Environment.GetEnvironmentVariable("ENABLE_SWAGGER") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteesApp API v1");
        c.RoutePrefix = string.Empty;
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();