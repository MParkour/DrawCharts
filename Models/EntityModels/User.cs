using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class User
    {
        public User()
        {

        }

        public int UserID { get; set; }
        
        public string UserName { get; set; }

        public string UserPassword { get; set; }
       
        public Utility.UserType UserType { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public byte[] UserPic { get; set; }
        public Utility.UserStatus StatusCode { get; set; }

        public virtual ICollection<Login_Log> Login_Logs { get; set; }
        public virtual ICollection<User_Temp> User_Temps { get; set; }
    }
}