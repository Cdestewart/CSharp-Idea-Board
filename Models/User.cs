using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeltExam.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage="Must have a first name")]
        [MinLength(2, ErrorMessage ="Name must be more than 2 characters")]
        public string FName { get; set; }
        [Required(ErrorMessage="Must have a last name")]
        [MinLength(2, ErrorMessage ="Name must be more than 2 characters")]
        public string LName { get; set; }
        [EmailAddress]
        [Required]
        [MinLength(2, ErrorMessage ="Email must be more than 2 characters")]
        public string Email { get; set; }
        [Required(ErrorMessage="Must have a password")]
        [MinLength(8, ErrorMessage="Password must be 8 characters or longer!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords must match")]
        [Required(ErrorMessage="Please enter a confirmation password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public DateTime Created_at {get;set;} = DateTime.Now;
        public DateTime Updated_at {get;set;} = DateTime.Now;
        public List<Liker> Likes {get;set;}


    }
    public class LoginUser
    {
        [Required(ErrorMessage="Please enter an email")]
        public string Lemail {get; set;}
        [Required(ErrorMessage="Please enter a password")]
        [DataType(DataType.Password)]
        public string Lpassword { get; set; }
    }
}