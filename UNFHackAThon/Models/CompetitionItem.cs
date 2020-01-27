using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models
{
    public class CompetitionItem
    {
        public int  Id { get; set; }

        [Required]
        public string Name { get; set; }

        public String Description { get; set; }

        [Required]
        public DateTime CompetitionDate { get; set; }

        [Required]
        public DateTime CompetitionTime { get; set; }

        public string Rating { get; set; }
        public enum EDiff { NA=0, NotDifficult=1, Difficult=2, VeryDifficult=3}

        public string Image { get; set; }

        [Display(Name="Competition")]
        public int CompetitionId { get; set; }

        [ForeignKey("CompetitionId")]
        public virtual Competition Competition { get; set; }

        [Display(Name = "SubCompetition")]
        public int SubCompetitionId { get; set; }


        [ForeignKey("SubCompetitionId")]
        public virtual SubCompetition SubCompetition { get; set; }


    }
}
