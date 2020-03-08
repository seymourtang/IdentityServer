using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        [DisplayName("用户名")]
        public string Username { get; set; }
        public string Email { get; set; }
        [DisplayName("昵称")]
        public string Nickname { get; set; }
        [DisplayName("手机号码")]
        public string PhoneNumber { get; set; }
        [DisplayName("学号")]
        public string CardNumber { get; set; }
        [DisplayName("性别")]
        public string Gender { get; set; }
        [DisplayName("学校")]
        public string University { get; set; }
        [DisplayName("所在城市")]
        public string City { get; set; }
    }
}
