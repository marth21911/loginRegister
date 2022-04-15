using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace loginRegisterDemo.Models
{
    public class LogUser
    {
        [Key]
        [Required]
        [EmailAddress]
        public string LogEmail {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string LogPassword {get;set;}
    }
}