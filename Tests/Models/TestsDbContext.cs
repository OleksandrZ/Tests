using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class TestsDbContext : IdentityDbContext<TestsUser>
    {
        public DbSet<Test> Tests { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<AvailableTests> AvailableTests { get; set; }
        public DbSet<PassedTests> PassedTests { get; set; }
        public TestsDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AvailableTests>(b =>
            {
                b.HasKey(t => new { t.TestId, t.UserId });

                b.HasOne(pt => pt.Test)
                .WithMany(p => p.AvailableTests)
                .HasForeignKey(pt => pt.TestId);

                b.HasOne(pt => pt.User)
                .WithMany(p => p.AvailableTests)
                .HasForeignKey(pt => pt.UserId);
            });
        }
    }
}
