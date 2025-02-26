using StackExchange.Redis;

namespace E_Learning.Configuration
{
    public static class RedisConfiguration
    {
        public static void CustomizeRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = configuration.GetSection("Redis");
            var redisConnectionString = $"{redisConfig["Host"]}:{redisConfig["Port"]}";
            if (!string.IsNullOrEmpty(redisConfig["Password"]))
            {
                redisConnectionString += $",password={redisConfig["Password"]}";
            }
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));
        }
    }
}
