using E_Learning.Configuration;
using E_Learning.Data;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using E_Learning.Servies;
using E_Learning.Servies.Impl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var allowedOrigins = new[] { "http://localhost:3000", "http://localhost:5173" };


            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                 });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCustomJwtAuthentication(builder.Configuration);  // JWT
            builder.Services.AddCustomCors(allowedOrigins);                     // CORS
            builder.Services.CustomizeRedis(builder.Configuration);            // Redis

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<CloudinaryService>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddSingleton<IEmailService, EmailService>();
            builder.Services.AddSingleton<IRedisService, RedisService>();
            builder.Services.AddScoped<CourseRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<RoleRepository>();
            builder.Services.AddDbContext<ELearningDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("_myAllowSpecificOrigins");
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
