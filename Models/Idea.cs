using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeltExam.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }

        [Required(ErrorMessage="Must have a creator")]
        public int CreatorId {get;set;}
        [ForeignKey("CreatorId")]
        
        public User Creator {get;set;}

        [Required(ErrorMessage="Must have an idea")]
        public string Description {get;set;}
        
        public List<Liker> Likes {get;set;}

    
        public Idea()
        {
            Likes = new List<Liker>();
        }
        
    }
}