using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Login_Log
    {
        public Login_Log()
        {

        }

        public int ID { get; set; }
        public string TimeIn { get; set; }
        public string DateIn { get; set; }
        public string TimeOut { get; set; }
        public string DateOut { get; set; }
        public string IP { get; set; }
        public string BrowserInfo { get; set; }

        [Required]
        public int UserID { get; set; }
        public virtual User User { get; set; }
    }
}