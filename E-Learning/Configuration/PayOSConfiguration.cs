using Net.payOS;

namespace E_Learning.Configuration
{
    public static class PayOSConfiguration
    {
        public static void AddPayOS(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<PayOS>(provider =>
            {
                var clientId = configuration["PayOS:ClientId"];
                var apiKey = configuration["PayOS:ApiKey"];
                var checksumKey = configuration["PayOS:ChecksumKey"];

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
                {
                    throw new Exception("PayOS configuration is missing ApiKey, ApiSecret, or ChecksumKey.");
                }

                var payOS = new PayOS(clientId, apiKey, checksumKey);
                return payOS;
            });
        }
    }
}
