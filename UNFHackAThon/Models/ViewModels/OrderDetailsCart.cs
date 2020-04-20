﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models.ViewModels
{
    public class OrderDetailsCart
    {
        public List<CompetitionCart> listCart {get; set;}
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }

    }
}
