using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.UserDTOs
{
    public class LoginUserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
