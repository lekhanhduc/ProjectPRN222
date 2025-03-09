using E_Learning.Data;
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

        public async Task<User?> FindUserById(int id)
        {
            return await _context.Users
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
