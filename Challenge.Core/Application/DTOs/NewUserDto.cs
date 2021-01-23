using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Core.Application.DTOs
{
    public class NewUserDto
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }
    }
}
