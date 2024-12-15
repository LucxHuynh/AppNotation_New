using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotationApp.Models
{
    public class SharedUserInfo
    {
        public string Email { get; set; }
        public string Permission { get; set; }

        public SharedUserInfo(string email, string permission)
        {
            Email = email;
            Permission = permission;
        }
    }
}
