using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models.ViewModels
{
    public class CompetitionItemViewModel
    {
        public CompetitionItem CompetitionItem { get; set; }
        public IEnumerable<Competition> Competition { get; set; }
        public IEnumerable<SubCompetition> SubCompetition { get; set; }
    }
}
