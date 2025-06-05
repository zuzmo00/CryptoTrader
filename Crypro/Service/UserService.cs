using AutoMapper;
using Crypro.Context;
using Crypro.DTO;
using Crypro.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crypro.Service
{
    public interface IUserService
    {
        // Define methods for user management
        Task<Guid> CreateUserAsync(UserCreateDto userCreateDto);
        Task<bool> UpdateUserAsync(Guid userId, UserUodateDto userUpdateDto);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<Guid> CreateAdminAsync(UserCreateDto userCreateDto);
        Task<ClaimsIdentity> GetClaimsIdentity(User user);
        Task<string> GenerateTokenAsync(User user);
        Task<string> LoginAsync(UserLoginDto userLoginDto);
    }
    public class UserService : IUserService
    {

        private readonly AppDbContext  _dbContext;
        private readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public UserService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Guid> CreateAdminAsync(UserCreateDto userCreateDto)
        {
            var email = await _dbContext.Users.FirstOrDefaultAsync(x=>x.Email==userCreateDto.Email);
            if (email != null)
            {
                throw new Exception("User already exists");
            }
            var user = _mapper.Map<User>(userCreateDto);
            user.Id = Guid.NewGuid();
            user.Role = UserRole.Admin;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;

        }

        public async Task<Guid> CreateUserAsync(UserCreateDto userCreateDto)
        {
            var email= await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userCreateDto.Email);
            if (email != null)
            {
                throw new Exception("User already exists");
            }
            var user = _mapper.Map<User>(userCreateDto);
            user.Id = Guid.NewGuid();
            user.Role = UserRole.User;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userCreateDto.Password);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user= await GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                throw new Exception($"User with id {userId} not found");
            }
            return user;
        }

        public async Task<bool> UpdateUserAsync(Guid userId, UserUodateDto userUpdateDto)
        {
            var user= await GetUserByIdAsync(userId);
            if(!string.IsNullOrEmpty(userUpdateDto.Name))
            {
                user.Name = userUpdateDto.Name;
            }
            if(!string.IsNullOrEmpty(userUpdateDto.Email))
            {
                user.Email = userUpdateDto.Email;
            }
            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(userUpdateDto.Password);
            }
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var id = await GetClaimsIdentity(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"]));
            var token = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"], id.Claims, expires: exp, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            return Task.FromResult(new ClaimsIdentity(claims, "Token"));
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userLoginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password,user.Password))
            {
                throw new UnauthorizedAccessException("Wrong email or password");
            }
            return await GenerateTokenAsync(user);
        }
    }
}
