using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Template
    {
        public Template()
        {

        }
        
        public int TemplateID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string IP { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string dbName { get; set; }
        public string TableName { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Calculation { get; set; }
        public string dbType { get; set; }

        public virtual ICollection<User_Temp> User_Temps { get; set; }
    }
}