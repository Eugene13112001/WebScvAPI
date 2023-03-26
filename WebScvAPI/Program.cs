using WebScvAPI.Settings;
using WebScvAPI.Containers;
using WebScvAPI.Models;
using Microsoft.Extensions.Options;
using static WebScvAPI.Containers.ICSVServiceData;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using static IdentityModel.ClaimComparer;
using Microsoft.AspNetCore.Authorization;
using WebScvAPI.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CSVDatabaseSettings>(
        builder.Configuration.GetSection(nameof(CSVDatabaseSettings)));

builder.Services.AddSingleton<ICSVDatabaseSettings>(sp =>
    sp.GetRequiredService<IOptions<CSVDatabaseSettings>>().Value);
builder.Services.AddSingleton<ICSVServiceData , CSVServiceData>();
builder.Services.AddSingleton<ICSVServiceReader, CSVServiceReader>();
builder.Services.AddSingleton<IAuthData, AuthData>();
builder.Services.AddSingleton<ICSVServiceFiltr, CSVServiceFiltr>();

builder.Services.AddSingleton<ICSVServiceWriter, CSVServiceWriter>();
var key = Encoding.ASCII.GetBytes("My very secure secret key for Development environment, do not share");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddCookie()
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true
                };
            });

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Event Service", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme(Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
