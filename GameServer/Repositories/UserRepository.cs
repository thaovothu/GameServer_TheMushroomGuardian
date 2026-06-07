using GameServer.Data;
using GameServer.Models;
using Microsoft.EntityFrameworkCore;

namespace GameServer.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.Include(u => u.Player).FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByUsernameAsync(string username) =>
        await _context.Users.Include(u => u.Player).FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User?> GetByDeviceIdAsync(string deviceId) =>
        await _context.Users.Include(u => u.Player).FirstOrDefaultAsync(u => u.DeviceId == deviceId);

    public async Task<User?> GetByIdAsync(int id) =>
        await _context.Users.Include(u => u.Player).FirstOrDefaultAsync(u => u.Id == id);

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllAsync() =>
        await _context.Users.Include(u => u.Player).OrderBy(u => u.Id).ToListAsync();

    public async Task<bool> EmailExistsAsync(string email) =>
        await _context.Users.AnyAsync(u => u.Email == email);

    public async Task<bool> UsernameExistsAsync(string username) =>
        await _context.Users.AnyAsync(u => u.Username == username);
}
