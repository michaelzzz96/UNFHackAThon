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

        public DbSet<CompetitionCart> CompetitionCart { get; set; }

        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
    }
}
