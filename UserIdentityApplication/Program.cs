using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Text;
using UserIdentityApplication.Data;
using UserIdentityApplication.Interfaces;
using UserIdentityApplication.Services;

namespace UserIdentityApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure culture settings (Optional)
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            // Add services to the container
            builder.Services.AddControllers();

            // Get JWT settings from appsettings.json
            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]); 

            // Configure DbContext with SQL Server
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

            builder.Services.AddScoped<IAuthService, AuthService>();
            // Add Swagger/OpenAPI for API documentation
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UserIdentityAPI", Version = "v1" });
            });

            // Add authentication with JWT Bearer
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtConfig["Audience"], 
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key), 
                        ClockSkew = TimeSpan.Zero  
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UserIdentityAPI v1"));
            }

            app.UseHttpsRedirection();
            // This needs to be added to authenticate the JWT token
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
