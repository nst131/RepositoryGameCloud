using AuthTraining;
using AuthTraining.Middleware;
using AuthTrainingBL.Login;
using AuthTrainingBL.Registration;
using AuthTrainingDL;
using AuthTrainingDL.Context;
using AuthTrainingDL.Models;
using AuthTrainingDL.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.AddDbContext<AuthTrainingContext>(options =>
     options.UseNpgsql(
         Environment.GetEnvironmentVariable("Connection_String") ??
         builder.Configuration.GetConnectionString("DefaultConnection") ?? ""));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthTrainingContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.Password.RequiredLength = 3;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireDigit = false;
});

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<ILoginHandler, LoginHandler>();
builder.Services.AddScoped<IRegistrationHandler, RegistrationHandler>();
builder.Services.AddScoped<DataSeed>();

builder.Services.AddAutoMapper(typeof(MapperProfile));

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
        policy.RequireRole(Role.Admin)
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
    option.AddPolicy("User", policy =>
       policy.RequireRole(Role.User)
           .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
});

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthAPI", Version = "v1" });
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
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthService"));

app.UseMiddleware<ErrorMiddleware>();
app.UseCors(s => s.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Migration(
    Environment.GetEnvironmentVariable("Connection_String") ??
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "");

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
