using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("users")]
    public class User : IdentityUser<Guid>
    {
        [Column("RefreshToken")]
        public string? RefreshToken { get; set; }
        [Column("RefreshTokenExpired")]
        public DateTime? RefreshTokenExpired { get; set; }
    }
}
