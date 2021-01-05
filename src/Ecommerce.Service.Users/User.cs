using System;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Service.Users
{
    internal class User
    {
        [Key]
        [StringLength(200)]
        public string Uuid { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
    }
}
