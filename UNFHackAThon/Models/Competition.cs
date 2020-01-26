﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UNFHackAThon.Models
{
    public class Competition
    {
        [Key]
        public int Id { get; set; }

        [Display(Name="Competition List Name")]
        [Required]
        public string Name { get; set; }


    }
}
