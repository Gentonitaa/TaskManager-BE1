using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.DataContext.Models;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Services;
using System.Text.Json.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.DataContext;
using TaskManager.DataContext.Models;
using TaskManager.Helpers;
using TaskManager.Repositories.Interfaces;
using TaskManager.Repositories.Services;

namespace TaskManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddControllers();

            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IJwtToken, JwtToken>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IIssueRepository, IssueRepository>();
            //  builder.Services.AddSingleton<JwtCheck>();

      
            //identityuser
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Add services to the container.


            //jwt
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                         return Task.CompletedTask;
                     },
                     OnTokenValidated = async context =>
                     {
                         //var jwtChecker = context.HttpContext.RequestServices.GetRequiredService<JwtCheck>();
                         //var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
                         //if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                         //{
                         //    var token = authHeader.Substring("Bearer ".Length).Trim();
                         //    var exist = jwtChecker.Check(token);
                         //    if (exist)
                         //    {
                         //        context.Fail("Unauthorized");
                         //        return Task.CompletedTask;
                         //    }
                         //}

                         //Console.WriteLine("Token validated successfully.");
                         //return Task.CompletedTask;
                     }
                 };
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                     ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
                     ClockSkew = TimeSpan.Zero

                 };

             });

            builder.Services.AddAuthorization();


            var app = builder.Build();
            app.UseCors(x => x.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();



        }
    }
}