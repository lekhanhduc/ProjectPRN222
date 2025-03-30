namespace E_Learning.Configuration
{
    public static class CorsConfiguration
    {
        public static void AddCustomCors(this IServiceCollection services, string[] allowedOrigins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .AllowAnyMethod();
                });
            });
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyOrigin() // Cho phép bất kỳ nguồn nào
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
