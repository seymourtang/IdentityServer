using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace OAuthServer.Models
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("用户名")]
        public string Username { get; set; }
        [DisplayName("密码")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        [DisplayName("记住我的登录")]
        public bool RememberLogin { get; set; }
        public IEnumerable<AuthenticationScheme> ExternalProviders { get; set; }
    }
}
