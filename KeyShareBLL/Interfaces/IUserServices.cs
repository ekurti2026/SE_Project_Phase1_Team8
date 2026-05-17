using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeySharePL.DTOs.UserDTOs;

namespace KeyShareBLL.Interfaces
{
    public interface IUserServices
    {
        AuthResultDTO Register(RegisterUserDTO dto);
        AuthResultDTO Login(LoginUserDTO dto);
        BasicUserDTO GetUser(int id);
        List<BasicUserDTO> GetAllUsers();
        BasicUserDTO UpdateUser(int id, UpdateUserDTO dto);
        BasicUserDTO DeleteUser(int id);
    }
}