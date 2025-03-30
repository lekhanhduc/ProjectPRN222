using E_Learning.Configuration;
using E_Learning.Data;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using E_Learning.Services.admin;
using E_Learning.Servies;
using E_Learning.Servies.admin;
using E_Learning.Servies.Impl;
using E_Learning.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var allowedOrigins = new[] { "http://localhost:3000", "http://localhost:5173" };


            builder.Services.AddControllers()
                 .AddJsonOptions(options =>
                 {
                     options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                 });

            builder.Services.AddPayOS(builder.Configuration);
            builder.Services.AddHttpClient<GoogleAuthClient>(client =>
            {
                client.BaseAddress = new Uri("https://oauth2.googleapis.com/");
            });

            builder.Services.AddHttpClient<GoogleUserInfoClient>(client =>
            {
                client.BaseAddress = new Uri("https://www.googleapis.com/");
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCustomJwtAuthentication(builder.Configuration);  // JWT
            builder.Services.AddCustomCors(allowedOrigins);                     // CORS
            builder.Services.CustomizeRedis(builder.Configuration);            // Redis
            builder.Services.AddElasticSearch(builder.Configuration);         // ElasticSearch

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
            builder.Services.AddScoped<IFavoriteService, FavoriteService>();
            builder.Services.AddScoped<IProfileService, ProfileService>();
            builder.Services.AddScoped<CourseRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<FavoriteRepository>();
            builder.Services.AddScoped<EnrollmentRepository>();
            builder.Services.AddScoped<PaymentRepository>();
            builder.Services.AddScoped<SearchRepository>();
            builder.Services.AddScoped<RoleRepository>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<AdminTeacherService>();
            builder.Services.AddScoped<AdminCourseService>();
            builder.Services.AddScoped<RegisterTeacherService>();
            builder.Services.AddScoped<FileService>();
            builder.Services.AddDbContext<ELearningDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
            app.UseCors("_myAllowSpecificOrigins");
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
