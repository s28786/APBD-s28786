using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task9.Models
{
    [Table("Users")]
    [PrimaryKey("IdUser")]
    public class AppUser
    {
        [Key]
        public int IdUser { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExp { get; set; }
    }
}