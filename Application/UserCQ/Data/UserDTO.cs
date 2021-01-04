using System;
using System.Collections.Generic;
using System.Text;

namespace Application.UserCQ.Data
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Token { get; set; }
    }
}
