using System;
using System.Collections.Generic;

namespace doudizhuServer.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
    }
}
