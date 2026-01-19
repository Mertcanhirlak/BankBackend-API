using BankBackend.Interfaces;
using BankBackend.Services;
using FluentValidation;           // Validation
using FluentValidation.AspNetCore; // Validation Middleware
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog; // Logging
using System.Text;
using System.Text.Json.Serialization;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// 1. Database Connection
builder.Services.AddDbContext<BankBackend.Data.BankDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BankConnection")));

// SERILOG CONFIGURATION
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


// Add Services (Dependency Injection)
builder.Services.AddScoped<ITokenService, TokenService>(); // Token service added
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IAiAdvisoryService, AiAdvisoryService>();
builder.Services.AddScoped<IMusteriService, MusteriService>();
builder.Services.AddScoped<IHesapService, HesapService>();

// Validation Services (FluentValidation)
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// HEALTH CHECKS
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BankConnection")!)
    .AddRedis(builder.Configuration.GetConnectionString("RedisConnection")!);

// JWT Authentication Settings
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

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

// 2. CORS Settings (Allow All Origins for Development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// 3. Controller and JSON Settings (Handle Circular References)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // To break circular references like User -> Account -> User:
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// 4. Swagger Settings (Adding JWT Button)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "BankBackend", Version = "v1" });

    // Swagger Lock Button
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // Include XML Documentation
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// --- HTTP Pipeline ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global Exception Handling Middleware
app.UseMiddleware<BankBackend.Middleware.ExceptionHandlingMiddleware>();

app.UseSerilogRequestLogging(); // Automatic HTTP request logging

app.MapHealthChecks("/health"); // Health check endpoint



// HTTPS redirect only in production (prevents dev SSL issues)
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Activate CORS (Policy name must match above)
app.UseCors("AllowAll");

app.UseDefaultFiles(); // Looks for index.html as default
app.UseStaticFiles();  // Serves wwwroot folder

app.UseAuthentication(); // 1. First authenticate
app.UseAuthorization();  // 2. Then authorize

app.MapControllers();

app.Run();