using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User_Temp
    {
        public User_Temp()
        {
            
        }
        
        public int User_TempID { get; set; }
        public int UserID { get; set; }
        public int TemplateID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public virtual User User { get; set; }
        public virtual Template Template { get; set; }
    }
}