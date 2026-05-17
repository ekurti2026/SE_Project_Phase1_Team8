using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDAL.Context;
using KeyShareDAL.Interfaces;
using KeyShareDL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyShareDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KeyShareContext _context;

        public UserRepository(KeyShareContext context)
        {
            _context = context;
        }

        public User? GetUserById(int id)
        {
            return _context.Users
                .Include(u => u.Vehicles)
                .Include(u => u.Bookings)
                .FirstOrDefault(u => u.UserId == id);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User AddUser(User user)
        {
            _context.Users.Add(user);
            return user;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            return user;
        }

        public User DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return user;
        }
    }
}
