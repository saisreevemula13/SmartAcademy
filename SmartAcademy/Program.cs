using Application.AutoMapper;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });
builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CustomStudentValidator>();

// Enable automatic validation for [ApiController] DTOs
builder.Services.AddFluentValidationAutoValidation();

// ---- AutoMapper registration (15.x syntax) ----

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

// ---- Repository & Service DI ----
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// ---- Swagger / API Explorer ----
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---- EF Core DbContext ----
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// ---- Seed database ----
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Initialize(context);
}

// ---- Middleware pipeline ----
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>(); // Global exception handling
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
