using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeltExam.Models
{
    public class Liker
    {

        [Key]
        public int LikerId {get;set;}

        public int UserId {get;set;}
        [ForeignKey("UserId")]
        public User User {get;set;}
        public int IdeaId {get;set;}
        [ForeignKey("IdeaId")]
        public Idea Idea {get;set;}
        
       

    }
}