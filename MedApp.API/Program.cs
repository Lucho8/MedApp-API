using System.Text;
using MedApp.Application.Features.Auth.Commands;
using MedApp.Application.Common.Interfaces;
using MedApp.Infrastructure.Persistence;
using MedApp.Infrastructure.Repositories;
using MedApp.Infrastructure.Services;
using MedApp.Domain.Interfaces;
using MedApp.Infrastructure.Hubs;
using MedApp.API.Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorio y UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<INotificationService, SignalRNotificationService>();

builder.Services.AddScoped<IPaymentService, MercadoPagoService>();

builder.Services.AddHostedService<ReminderBackgroundService>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(RegisterCommand).Assembly));

builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://tu-app.vercel.app")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// JWT
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });



builder.Services.AddHttpClient("MercadoPago");


builder.Services.AddAuthorization();





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors("FrontendPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AppointmentHub>("/hubs/appointments");

app.Run();