using ApiGateway;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ���������� ����� ������������ Ocelot
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// �������� ���������� ������ ��� CORS
var corsAllowedHosts = builder.Configuration.GetSection("Cors_client").Get<string[]>();

// �������� �������� JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// ��������� �������������� JWT
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = jwtSettings.Authority; // URL  Identity Service
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Authority,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey))
        };
    });

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS_POLICY", policyConfig =>
    {
        policyConfig.WithOrigins(corsAllowedHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
    });
});

// ���������� Ocelot � �������
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// ����������� �������� ��������� ������������
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogWarning($"Security Protocol: {ServicePointManager.SecurityProtocol}");

// �������� ��������� HTTP-��������
app.UseHttpsRedirection();
app.UseCors("CORS_POLICY");
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

// ������ Ocelot Middleware
await app.UseOcelot();
app.Run();
