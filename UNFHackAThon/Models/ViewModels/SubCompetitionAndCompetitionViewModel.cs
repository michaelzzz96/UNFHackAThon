using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models.ViewModels
{
    public class SubCompetitionAndCompetitionViewModel
    {
        public IEnumerable<Competition> CompetitionList { get; set; }
        public SubCompetition SubCompetition { get; set; }
        public List<string> SubCompetitionList { get; set; }
        public string StatusMessage { get; set; }
    }
}
