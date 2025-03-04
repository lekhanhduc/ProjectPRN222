using Elastic.Clients.Elasticsearch;

namespace E_Learning.Configuration
{
    public static class ElasticSearchConfiguration
    {

        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticUri = configuration.GetValue<string>("ElasticsearchUri") ?? "http://localhost:9200";

            var settings = new ElasticsearchClientSettings(new Uri(elasticUri))
                .DefaultIndex("courses");

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }

    }
}
