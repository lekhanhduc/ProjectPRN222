using System.Net;

namespace E_Learning.Middlewares
{
    public class ErrorCode
    {
        public int Code { get; }
        public string Message { get; }
        public HttpStatusCode StatusCode { get; }

        private ErrorCode(int Code, string Message, HttpStatusCode StatusCode)
        {
            this.Code = Code;
            this.Message = Message;
            this.StatusCode = StatusCode;
        }

        public static readonly ErrorCode USER_NOT_EXISTED = new ErrorCode(404, "User not existed", HttpStatusCode.NotFound);
        public static readonly ErrorCode UNAUTHORIZED = new ErrorCode(401, "Unauthorized", HttpStatusCode.Unauthorized);
        public static readonly ErrorCode ROLE_EXISTED = new ErrorCode(400, "Role existed", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ROLE_NOT_EXISTED = new ErrorCode(400, "Role not existed", HttpStatusCode.NotFound);
        public static readonly ErrorCode OTP_INVALID = new ErrorCode(400, "Otp invalid", HttpStatusCode.BadRequest);
        public static readonly ErrorCode ACCOUNT_LOCKED = new ErrorCode(401, "Your account has been locked", HttpStatusCode.Unauthorized);
        public static readonly ErrorCode CREATE_COURSE_ERRORR = new ErrorCode(400, "Create course error", HttpStatusCode.BadRequest);
        public static readonly ErrorCode COURSE_NOT_EXISTED = new ErrorCode(400, "Course not existed", HttpStatusCode.NotFound);
        public static readonly ErrorCode FAVORITE_EXISTED = new ErrorCode(400, "Favorite existed", HttpStatusCode.BadRequest);
        public static readonly ErrorCode REFRESH_TOKEN_MISSING = new ErrorCode(400, "Refresh token missing", HttpStatusCode.BadRequest);
        public static readonly ErrorCode INVALID_REFRESH_TOKEN = new ErrorCode(400, "Invalid Refresh Token", HttpStatusCode.BadRequest);
        public static readonly ErrorCode CHAPTER_NOT_EXIST = new ErrorCode(400, "Chapter not existed", HttpStatusCode.BadRequest);
        public static readonly ErrorCode LESSON_NOT_EXISTED = new ErrorCode(400, "Lesson not existed", HttpStatusCode.BadRequest);
        public static readonly ErrorCode COURSE_ACCESS_DENIED = new ErrorCode(400, "Access denied", HttpStatusCode.Forbidden);
        public static readonly ErrorCode LESSON_NOT_EXIST = new ErrorCode(400, "Lesson not existed", HttpStatusCode.BadRequest);
    }
}
