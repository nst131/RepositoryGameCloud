using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;
using TranslaterServiceBL;
using TranslaterServiceDL;
using TranslaterServiceDL.Initializer;
using TranslaterWebApi;
using TranslaterWebApi.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddRegistrationDL(
    Environment.GetEnvironmentVariable("Connection_String") ?? 
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "");

builder.Services.AddRegistrationBL();
builder.Services.AddAutoMapper(typeof(MapperConfigurationBL), typeof(MapperConfigurationUI));

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters()
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "")),
           ValidateLifetime = true
       };
   });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("Admin", policy =>
        policy.RequireRole("Admin")
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
    option.AddPolicy("User", policy =>
       policy.RequireRole("User")
           .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
});

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "TranslaterWebApi", Version = "v1" });
    swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = nameof(Authorization),
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TranslaterService"));

app.UseMiddleware<ErrorHandleMiddleware>();
app.UseCors(s => s.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

await app.Initilization(
    Environment.GetEnvironmentVariable("Connection_String") ?? builder.Configuration.GetConnectionString("DefaultConnection") ?? "",
    Environment.GetEnvironmentVariable("DATASEED_PATH") ?? Path.GetFullPath(builder.Configuration["Paths:DataSeed"] ?? ""));

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


app.Run();
