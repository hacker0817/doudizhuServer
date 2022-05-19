using System.ComponentModel.DataAnnotations;

namespace doudizhuServer.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(100)]
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
