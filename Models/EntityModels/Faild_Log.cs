using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Faild_Log
    {
        public Faild_Log()
        {
                        
        }
        
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DateTime_In { get; set; }
        public string IP { get; set; }
        public string BrowserInfo { get; set; }
    }    
}