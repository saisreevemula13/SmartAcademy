using Application.AutoMapper;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// jwt validation here
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
//step-2 Add authorization
builder.Services.AddAuthorization();


// Step 1️⃣: Configure Serilog early (before building the app)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .CreateLogger();
// Step 2️⃣: Tell the host to use Serilog
builder.Host.UseSerilog();

// Everything below is wrapped in try–catch
try
{
    Log.Information("Starting SmartAcademy API...");

    //Add services to the container
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
        });

    //Register FluentValidation validators
    builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseValidator>();
    builder.Services.AddValidatorsFromAssemblyContaining<CustomStudentValidator>();
    builder.Services.AddFluentValidationAutoValidation();

    //AutoMapper registration
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

    //Repositories & Services
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
    builder.Services.AddScoped<IStudentService, StudentService>();
    builder.Services.AddScoped<ICourseRepository, CourseRepository>();
    builder.Services.AddScoped<ICourseService, CourseService>();
    builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
    builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

    //Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //EF Core DbContext
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

    //Build app (can throw exceptions if DI or config fails)
    var app = builder.Build();

    //Serilog HTTP request logging
    app.UseSerilogRequestLogging();

    //Database seeding
    //using (var scope = app.Services.CreateScope())
    //{
    //    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //    DbInitializer.Initialize(context);
    //}

    // Middleware pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>(); // global exception handler
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Run the application
    app.Run();
}
catch (Exception ex)
{
    //  Logs any startup or fatal crash
    Log.Fatal(ex, "SmartAcademy API terminated unexpectedly during startup");
}
finally
{
    // Always flush remaining log entries
    Log.CloseAndFlush();
}
