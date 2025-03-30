using E_Learning.Common;
using E_Learning.Data;
using E_Learning.Dto.Request;
using E_Learning.Dto.Response;
using E_Learning.Middlewares;
using E_Learning.Repositories;
using E_Learning.Servies.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Learning.Servies
{
    public class RegisterTeacherService
    {
        private readonly ELearningDbContext _context;
        private readonly FileService _fileService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CloudinaryService cloudinaryService;

        public RegisterTeacherService(
            ELearningDbContext context,
            FileService fileService,
            IHttpContextAccessor httpContextAccessor,
            CloudinaryService cloudinaryService
        )
        {
            _context = context;
            _fileService = fileService;
            this.httpContextAccessor = httpContextAccessor;
            this.cloudinaryService = cloudinaryService;
        }

        // Get all teacher registrations
        public async Task<List<UserRegisterTeacherResponse>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Where(user => user.RegistrationStatus == RegistrationStatus.Pending && user.Role.Name == "USER")
                .Select(user => new UserRegisterTeacherResponse
                {
                    Email = user.Email,
                    Name = user.Name,
                    Phone = user.Phone,
                    Expertise = user.Expertise,
                    YearsOfExperience = user.YearsOfExperience,
                    Bio = user.Bio,
                    FacebookLink = user.FacebookLink,
                    RegistrationStatus = user.RegistrationStatus.ToString(),
                    Certificate = user.Certificate,
                    CvUrl = user.CvUrl
                }).ToListAsync();
        }

        // Register teacher logic
        public async Task<UserRegisterTeacherResponse> RegisterTeacher(UserRegisterTeacherRequest request)
        {
            var userId = httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "userId");

            if (userId == null)
            {
                throw new AppException("User is not authenticated.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId.Value));
            if (user == null)
            {
                throw new AppException("User not found.");
            }

            // Now handle the file storage for CV and certificate
            var cvFilePath = await _fileService.StoreAsync(request.Cv, "cvFolder");
            var certificateFilePath = await _fileService.StoreAsync(request.Certificate, "certificateFolder");

            user.Phone = request.Phone;
            user.Expertise = request.Expertise;
            user.YearsOfExperience = request.YearsOfExperience;
            user.Bio = request.Bio;
            user.FacebookLink = request.FacebookLink;
            user.Certificate = certificateFilePath;
            user.CvUrl = cvFilePath;
            user.RegistrationStatus = RegistrationStatus.Pending;

            using var stream = request.Qr.OpenReadStream();
            var imageUrl = await cloudinaryService.UploadImageAsync(stream, request.Qr.Name);

            user.Qr = imageUrl;

            await _context.SaveChangesAsync();

            return new UserRegisterTeacherResponse
            {
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Expertise = user.Expertise,
                YearsOfExperience = user.YearsOfExperience,
                Bio = user.Bio,
                FacebookLink = user.FacebookLink,
                Certificate = user.Certificate,
                CvUrl = user.CvUrl,
                RegistrationStatus = user.RegistrationStatus.ToString(),
                Qr = user.Qr
            };
        }


        // Save teacher (approve)
        public async Task<UserRegisterTeacherResponse> ApproveTeacher(long id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new AppException("Error");
            }

            if (user.RegistrationStatus == RegistrationStatus.Pending && user.Role.Name == "USER")
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name == "TEACHER");

                if (role == null)
                {
                    throw new AppException("Error");
                }

                user.RegistrationStatus = RegistrationStatus.Approved;
                user.Role = role;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserRegisterTeacherResponse
                {
                    Email = user.Email,
                    Name = user.Name,
                    Phone = user.Phone,
                    Expertise = user.Expertise,
                    YearsOfExperience = user.YearsOfExperience,
                    Bio = user.Bio,
                    FacebookLink = user.FacebookLink,
                    RegistrationStatus = user.RegistrationStatus.ToString(),
                    Certificate = user.Certificate,
                    CvUrl = user.CvUrl
                };
            }

            throw new AppException("Error");
        }

        // Reject teacher registration
        public async Task<UserRegisterTeacherResponse> RejectTeacher(long id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new AppException("Error");
            }

            if (user.RegistrationStatus == RegistrationStatus.Pending && user.Role.Name == "USER")
            {
                var role = await _context.Roles
                    .FirstOrDefaultAsync(r => r.Name == "USER");

                if (role == null)
                {
                    throw new AppException("Error");
                }

                user.RegistrationStatus = RegistrationStatus.Rejected;
                user.Role = role;
                user.CvUrl = null;
                user.Certificate = null;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return new UserRegisterTeacherResponse
                {
                    Email = user.Email,
                    Name = user.Name,
                    Phone = user.Phone,
                    Expertise = user.Expertise,
                    YearsOfExperience = user.YearsOfExperience,
                    Bio = user.Bio,
                    FacebookLink = user.FacebookLink,
                    RegistrationStatus = user.RegistrationStatus.ToString(),
                    Certificate = user.Certificate,
                    CvUrl = user.CvUrl
                };
            }

            throw new AppException("Error");
        }
    }
}
