using E_Learning.Common;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;

namespace E_Learning.Servies.Impl
{
    public class TeacherService : ITeacherService
    {

        private readonly CourseRepository courseRepository;
        private readonly EnrollmentRepository enrollmentRepository;
        private readonly UserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TeacherService(CourseRepository courseRepository, EnrollmentRepository enrollmentRepository, 
            IHttpContextAccessor httpContextAccessor, UserRepository userRepository)
        {
            this.courseRepository = courseRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }
        public async Task<PageResponse<StudentResponse>> GetStudentsByPurchasedCourses(int page, int size)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            
            var user = await userRepository.FindUserById(long.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            if (user.Role.Name != DefinitionRole.TEACHER)
            {
                throw new AppException(ErrorCode.FORBIDDEN);
            }

            var totalEnrollments = await enrollmentRepository.FindPurchasedUsersByTeacherId(user.Id, page, size);
            var totalElements =  await enrollmentRepository.GetTotalEnrollmentsCount(user.Id);


            return new PageResponse<StudentResponse>
            {
                PageSize = size,
                CurrentPage = page,
                TotalElemets = totalElements,
                TotalPages = (int)Math.Ceiling((double)totalElements / size),
                Data = totalEnrollments.Select(enrollment => new StudentResponse
                {
                    Name = enrollment.User.Name,
                    Email = enrollment.User.Email,
                    Avatar = enrollment.User.Avatar,
                    CourseName = enrollment.Course.Title,
                    CreatedAt = enrollment.CreatedAt
                }).ToList()
            };
        }

    }
}
