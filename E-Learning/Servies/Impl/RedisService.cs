
using StackExchange.Redis;

namespace E_Learning.Servies.Impl
{
    public class RedisService : IRedisService
    {

        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisService> logger)
        {
            this._connectionMultiplexer = connectionMultiplexer;
            this._logger = logger;
        }

        public void DeleteOtp(string email)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.KeyDelete(email);
            _logger.LogInformation("Delete Otp Successfully");
        }

        public string GetOtp(string email)
        {
            _logger.LogInformation("Get Otp From Redis");
            var db = _connectionMultiplexer.GetDatabase();
            var otp = db.StringGet(email);
            return otp.HasValue ? otp.ToString() : string.Empty;
        }

        public void SaveOtp(string email, string otp, TimeSpan timeSpan)
        {
            var db = _connectionMultiplexer.GetDatabase();
            db.StringSet(email, otp, timeSpan);
            _logger.LogInformation("Save Otp Successfully");
        }
    }
}
