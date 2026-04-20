using System.Security.Claims;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using KeycloakAuth.Extensions; 
using KeycloakAuth.Data;
using KeycloakAuth.Entities;
using Microsoft.EntityFrameworkCore;
using KeycloakAuth.Filters;
using KeycloakAuth.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithAuthSupport(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<SyncKeycloakUserFilter>();
builder.Services.AddScoped<ITaskService, TaskServices>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/auth-demo"; 
        options.Audience = "account";
        options.RequireHttpsMetadata = false;
    });

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("KeycloakAuth"))
    .WithTracing(tracing =>
    {
        tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();
    });

WebApplication app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/users/me", (ClaimsPrincipal claimsPrincipal) =>
{
    return claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value);
})
.RequireAuthorization();

app.Run();