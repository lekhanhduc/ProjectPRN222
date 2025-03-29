using E_Learning.Dto.Event;
using E_Learning.Dto.Response;
using E_Learning.Entity;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using Microsoft.AspNetCore.Http;

namespace E_Learning.Servies.Impl
{
    public class CertificateService : ICertificationService
    {

        private readonly CourseRepository courseRepository;
        private readonly UserRepository userRepository;
        private readonly CertificateRepository certificateRepository;
        private readonly LessonProgressRepository lessonProgressRepository;
        private readonly EnrollmentRepository enrollmentRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<CertificateService> log;

        public CertificateService(CourseRepository courseRepository, UserRepository userRepository, CertificateRepository certificateRepository, 
            LessonProgressRepository lessonProgressRepository, EnrollmentRepository enrollmentRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<CertificateService> log)
        {
            this.courseRepository = courseRepository;
            this.userRepository = userRepository;
            this.certificateRepository = certificateRepository;
            this.lessonProgressRepository = lessonProgressRepository;
            this.enrollmentRepository = enrollmentRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.log = log;
        }

        public async Task CreateCrertification(CertificateCreationEvent creationEvent)
        {
            var course = await courseRepository.FindById(creationEvent.CourseId)
                ?? throw new AppException(ErrorCode.COURSE_NOT_EXISTED);

            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }

            var user = await userRepository.FindUserById(long.Parse(userId.Value))
                ?? throw new AppException(ErrorCode.USER_NOT_EXISTED);

            long totalLesson = course.Chapters.Sum(chapter => chapter.Lessons.Count);

            if (!await enrollmentRepository.ExistsByUserAndCourse(user, course))
            {
                throw new AppException(ErrorCode.COURSE_NOT_PURCHASED);
            }

            if (await lessonProgressRepository.TotalLessonComplete(user, course) < totalLesson)
            {
                throw new AppException(ErrorCode.INCOMPLETE_LESSONS);
            }

            // Kiểm tra xem chứng chỉ đã tồn tại chưa
            if (await certificateRepository.ExistsByCourseAndUser(course, user))
            {
                throw new AppException(ErrorCode.CERTIFICATE_EXISTED);
            }

            var certificate = new Certificate
            {
                Name = "DLearning Certificate of Completion",
                User = user,
                Course = course,
                IssueDate = DateTime.Now
            };

            await certificateRepository.Add(certificate);

            log.LogInformation("Certificate created for userId: {UserId}, courseId: {CourseId}", user.Id, course.Id);
        }

        public async Task<List<CertificateResponse>> GetCertificateByUserLogin()
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userId == null)
            {
                throw new AppException(ErrorCode.USER_NOT_EXISTED);
            }
            log.LogInformation("Get certificate by userId: {UserId}", userId.Value);
            var user = await userRepository.FindUserById(long.Parse(userId.Value))
                ?? throw new AppException(ErrorCode.USER_NOT_EXISTED);

            var certificates = await certificateRepository.FindByUserAsync(user);
            log.LogInformation("Get certificate by userId: {UserId} successfully", certificates);
            return certificates.Select(certificate => new CertificateResponse
            {
                CertificateId = certificate.Id,
                CourseName = certificate.Course.Title,
                Author = certificate.Course.Author.Name,
                Email = certificate.User.Email,
                Username = certificate.User.Name,
                CertificateUrl = certificate.CertificateUrl != null ? certificate.CertificateUrl : "",
                IssueDate = certificate.IssueDate != null ? certificate.IssueDate.Value : DateTime.Now
            }).ToList();
        }

    }
}
