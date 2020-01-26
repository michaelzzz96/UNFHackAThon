using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models
{
    public class SubCompetition
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "SubCompetition List Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Competition")]
        public int CompetitionId { get; set; }

        [ForeignKey("CompetitionId")]
        public virtual Competition Competition { get; set; }

    }
}
