namespace E_Learning.Models.Response
{
    public class ResponseData<T>
    {
        public int code { get; set; }
        public string? message { get; set; }
        public T? data { get; set; }

        public ResponseData()
        {
        }

        public ResponseData(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public ResponseData(int code, string message, T data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
    }
}
