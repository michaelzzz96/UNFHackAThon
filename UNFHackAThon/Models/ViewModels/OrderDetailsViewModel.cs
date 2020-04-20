using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UNFHackAThon.Models;

namespace UNFHackAThon.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
        public List<CompetitionCart> listCart { get; set; }
    }
}