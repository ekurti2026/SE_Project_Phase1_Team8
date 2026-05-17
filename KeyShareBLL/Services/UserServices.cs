using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KeyShareBLL.Interfaces;
using KeyShareDAL.UnitOfWork;
using KeyShareDL.Entities;
using KeyShareDL.Enums;
using KeySharePL.DTOs.UserDTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KeyShareBLL.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserServices(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        public AuthResultDTO Register(RegisterUserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required.");

            var existing = _unitOfWork.Users.GetUserByEmail(dto.Email);
            if (existing != null)
                throw new InvalidOperationException("A user with this email already exists.");

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            _unitOfWork.Users.AddUser(user);
            _unitOfWork.Save();

            var token = GenerateToken(user);

            return new AuthResultDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public AuthResultDTO Login(LoginUserDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Password is required.");

            var user = _unitOfWork.Users.GetUserByEmail(dto.Email);
            if (user == null)
                throw new KeyNotFoundException("No account found with this email.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new ArgumentException("Invalid credentials.");

            if (user.Status != "Active")
                throw new InvalidOperationException("This account has been suspended.");

            var token = GenerateToken(user);

            return new AuthResultDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Token = token
            };
        }

        public BasicUserDTO GetUser(int id)
        {
            var user = _unitOfWork.Users.GetUserById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return _mapper.Map<BasicUserDTO>(user);
        }

        public List<BasicUserDTO> GetAllUsers()
        {
            var users = _unitOfWork.Users.GetAllUsers();
            if (users == null || users.Count == 0)
                throw new KeyNotFoundException("No users found.");

            return _mapper.Map<List<BasicUserDTO>>(users);
        }

        public BasicUserDTO UpdateUser(int id, UpdateUserDTO dto)
        {
            var user = _unitOfWork.Users.GetUserById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            bool hasChanges = false;

            if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name != user.Name)
            {
                user.Name = dto.Name;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber) && dto.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = dto.PhoneNumber;
                hasChanges = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                hasChanges = true;
            }

            if (!hasChanges)
                throw new ArgumentException("Nothing to update.");

            _unitOfWork.Users.UpdateUser(user);
            _unitOfWork.Save();

            return _mapper.Map<BasicUserDTO>(user);
        }

        public BasicUserDTO DeleteUser(int id)
        {
            var user = _unitOfWork.Users.GetUserById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            _unitOfWork.Users.DeleteUser(user);
            _unitOfWork.Save();

            return _mapper.Map<BasicUserDTO>(user);
        }

        // ── private helpers ──────────────────────────────────────────────────

        private string GenerateToken(User user)
        {
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Name,           user.Name),
                new Claim(ClaimTypes.Role,           user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}