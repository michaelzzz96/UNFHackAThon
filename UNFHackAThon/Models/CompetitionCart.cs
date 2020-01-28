using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models
{
    public class CompetitionCart
    {
        public CompetitionCart()
        {
            Count = 1;
        }


        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [NotMapped]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        public int CompetitionItemId { get; set; }


        [NotMapped]
        [ForeignKey("CompetitionItemId")]
        public virtual CompetitionItem CompetitionItem { get; set; }


        [Range(1,int.MaxValue, ErrorMessage ="Please enter a value greater than equal to {1}")]
        public int Count { get; set; }
    }
}
