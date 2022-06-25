using System;
using System.Collections.Generic;

namespace KimeBenzerRest.Models
{
    public partial class UserTable
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }
}
