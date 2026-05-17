using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.UserDTOs
{
    public class AuthResultDTO
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public UserRole Role { get; set; }
        public required string Token { get; set; }
    }
}
