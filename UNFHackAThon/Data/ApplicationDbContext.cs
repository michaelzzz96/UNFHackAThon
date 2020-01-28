using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNFHackAThon.Models;

namespace UNFHackAThon.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Competition> Competition { get; set; }
        public DbSet<SubCompetition> SubCompetition { get; set; }
        public DbSet<CompetitionItem> CompetitionItem { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }




    }
}
