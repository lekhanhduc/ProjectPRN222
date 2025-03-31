using E_Learning.Data;
﻿using E_Learning.Data;
using E_Learning.Dto.Response.admin;
using E_Learning.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Learning.Repositories
{
    public class UserRepository
    {

        private readonly ELearningDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ELearningDbContext context, ILogger<UserRepository> _logger)
        {
            _context = context;
            this._logger = _logger;
        }

        public UserRepository(ELearningDbContext context)
        {
            _context = context;
        }

        public async Task<User?> FindUserByEmail(string email)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllUsers(int page, int size)
        {
            return await _context.Users
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> GetTotalUsersCount()
        {
            return await _context.Users.CountAsync();  
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            _logger.LogInformation("Updating user: {Email}", user.Email);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User updated successfully: {Email}", user.Email);
        }

        public async Task<User?> FindUserById(long id)
        {
            return await _context.Users
                .Include(u => u.Role)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<PaginatedList<AdminUserResponse>> GetAllUsersAsync(int pageNumber, int pageSize)
        {
            var query = _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new AdminUserResponse
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Gender = u.Gender != null ? u.Gender.ToString() : null,
                    Enabled = u.Enabled,
                    Role = u.Role.Name,
                    CreateAt = u.CreatedAt
                });
            return await PaginatedList<AdminUserResponse>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PaginatedList<AdminUserResponse>> SearchByMultipleKeywordsAsync(string[] keywords, int pageNumber, int pageSize)
        {
            var query = _context.Users.AsQueryable();

            if (keywords != null && keywords.Length > 0)
            {
                query = query.Where(u => keywords.Any(k =>
                    EF.Functions.Like(u.Name.ToLower(), "%" + k.ToLower() + "%") ||
                    EF.Functions.Like(u.Email.ToLower(), "%" + k.ToLower() + "%")
                ));
            }

            // Chuyển đổi query từ IQueryable<User> thành IQueryable<AdminUserResponse>
            var queryResponse = query.Select(u => new AdminUserResponse
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Gender = u.Gender.HasValue ? u.Gender.Value.ToString() : null,
                Enabled = u.Enabled,
                Role = u.Role.Name,
                CreateAt = u.CreatedAt
            });

            queryResponse = queryResponse.OrderByDescending(u => u.CreateAt);

            return await PaginatedList<AdminUserResponse>.CreateAsync(queryResponse, pageNumber, pageSize);
        }
    }
}
