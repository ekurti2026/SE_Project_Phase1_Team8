using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KeyShareDL.Enums;


namespace KeySharePL.DTOs.UserDTOs
{
    public class UpdateUserDTO
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
