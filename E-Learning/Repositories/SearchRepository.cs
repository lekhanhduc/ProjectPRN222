using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Models.Response;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace E_Learning.Repositories
{
    public class SearchRepository
    {
        private readonly ElasticsearchClient elasticsearchClient;

        public SearchRepository(ElasticsearchClient elasticsearchClient)
        {
            this.elasticsearchClient = elasticsearchClient;
        }

        public async Task<PageResponse<CourseResponse>> GetAllWithSearch(int page, int size, string? keyword, string? level, double? minPrice, double? maxPrice)
        {
            var queries = new List<Query>();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                queries.Add(Query.MultiMatch(new MultiMatchQuery
                {
                    Query = keyword,
                    Fields = new Field[]
                    {
                        Infer.Field<CourseElasticSearch>(x => x.Title),
                        Infer.Field<CourseElasticSearch>(x => x.AuthorName),
                        Infer.Field<CourseElasticSearch>(x => x.Language),
                        Infer.Field<CourseElasticSearch>(x => x.Description),
                    },
                    Fuzziness = new Fuzziness("AUTO"),
                }
                ));
            }

            //if (!string.IsNullOrWhiteSpace(keyword))
            //{
            //    queries.Add(Query.QueryString(new QueryStringQuery
            //    {
            //        Query = $"*{keyword.ToLower()}*", // giống like '%keyword%'
            //        Fields = new Field[]
            //        {
            //Infer.Field<CourseElasticSearch>(x => x.Title),
            //Infer.Field<CourseElasticSearch>(x => x.AuthorName),
            //Infer.Field<CourseElasticSearch>(x => x.Description)
            //        },
            //        DefaultOperator = Operator.And
            //    }));
            //}

            if (!string.IsNullOrWhiteSpace(level))
            {
                queries.Add(Query.Match(new MatchQuery(Infer.Field<CourseElasticSearch>(x => x.Level))
                {
                    Query = level,
                    Fuzziness = new Fuzziness(2)
                }));
            }

            if (minPrice.HasValue || maxPrice.HasValue)
            {
                queries.Add(Query.Range(new NumberRangeQuery(Infer.Field<CourseElasticSearch>(x => x.Price))
                {
                    Gte = minPrice,
                    Lte = maxPrice
                }));
            }

            Query finalQuery = queries.Count > 1
                ? Query.Bool(new BoolQuery { Must = queries }) // Nếu có nhiều điều kiện
                : queries.FirstOrDefault() ?? Query.MatchAll(new MatchAllQuery());

            var response = await elasticsearchClient.SearchAsync<CourseElasticSearch>(new SearchRequest("courses")
            {
                Query = finalQuery,
                From = (page - 1) * size,
                Size = size
            });

            if (response == null || response.Documents == null)
            {
                return new PageResponse<CourseResponse>
                {
                    CurrentPage = page,
                    PageSize = size,
                    TotalPages = 0,
                    TotalElemets = 0,
                    Data = []
                };
            }

            var courses = response.Documents.Select(course => new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Author = course.AuthorName,
                Description = course.Description,
                Duration = course.Duration,
                Price = (decimal)course.Price,
                Language = course.Language,
                Level = course.Level,
                Thumbnail = course.Thumbnail

            }).ToList();

            return new PageResponse<CourseResponse>
            {
                CurrentPage = page,
                PageSize = size,
                TotalPages = (int)Math.Ceiling((double)response.Total / size),
                TotalElemets = response.Total,
                Data = courses
            };

        }
    }
}
