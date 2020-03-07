using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OAuthServer.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string CardNumber { get; set; }
        public string NickName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string University { get; set; }
    }
}
