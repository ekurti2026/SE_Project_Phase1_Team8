using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.UserDTOs
{
    public class BasicUserDTO
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public string Status { get; set; } = "Active";
    }
}
