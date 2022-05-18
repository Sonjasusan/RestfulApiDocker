using System;
using System.Collections.Generic;

namespace RestfulApi.Models
{
    public partial class User
    {
        public int UserId { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string? Email { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int AcceslevelId { get; set; }
    }
}
