using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class EnrollmentService : IEnrollmentService
    {

        private readonly EnrollmentRepository enrollmentRepository;
        private readonly CourseRepository courseRepository;
        private readonly UserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public EnrollmentService(EnrollmentRepository enrollmentRepository, CourseRepository courseRepository, IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            this.enrollmentRepository = enrollmentRepository;
            this.courseRepository = courseRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public async Task<List<BuyCourseResponse>> GetCourseByUserCurrent()
        {
            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var userId = long.Parse(userIdClaim.Value);
            var user = await userRepository.FindUserById(userId);

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var enrollments = await enrollmentRepository.FindCourseByUser(user);

            var buyCourseResponses = enrollments.Select(enrollment => new BuyCourseResponse
            {
                CourseId = enrollment.CourseId,
                Title = enrollment.Course.Title,
                Author = enrollment.Course.Author.Name,
                CourseLevel = enrollment.Course.Level,
                Thumbnail = enrollment.Course.Thumbnail,
                Price = enrollment.Course.Price,
                CreatedAt = enrollment.Course.CreatedAt
            }).ToList();

            return buyCourseResponses;
        }


        public async Task<IsCourseCompleteResponse> IsCompleteCourse(IsCourseCompleteRequest request)
        {
            var course = await courseRepository.FindById(request.CourseId);
            if (course == null)
            {
                throw new AppException(ErrorCode.COURSE_NOT_EXISTED);
            }

            var userIdClaim = httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var userId = long.Parse(userIdClaim.Value);
            var user = await userRepository.FindUserById(userId);

            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            // Kiểm tra khóa học có hoàn thành hay không
            var isComplete = await enrollmentRepository.IsCourseCompleteByUser(user, course);

            return new IsCourseCompleteResponse
            {
                IsComplete = isComplete
            };
        }

    }
}
