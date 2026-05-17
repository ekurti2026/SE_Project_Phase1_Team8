using KeyShareDL.Entities;

namespace KeyShareDAL.Interfaces
{
    public interface IUserRepository
    {
        User? GetUserById(int id);
        User? GetUserByEmail(string email);
        List<User> GetAllUsers();
        User AddUser(User user);
        User UpdateUser(User user);
        User DeleteUser(User user);
    }
}