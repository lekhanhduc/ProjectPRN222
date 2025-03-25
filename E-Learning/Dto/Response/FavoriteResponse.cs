namespace E_Learning.Dto.Response
{
    public class FavoriteResponse
    {
        public long FavoriteId { get; set; }
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Thumbnail { get; set; }
        public decimal Price { get; set; }
    }
}
