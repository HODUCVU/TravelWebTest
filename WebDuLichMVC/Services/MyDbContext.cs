using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using WebDuLichMVC.Models;

namespace WebDuLichMVC.Services
{
    public class MyDbContext : DbContext
    {
        public MyDbContext()
        {
            
        }
        // public MyDbContext(Parameters)
        // {
            
        // }
        public DbSet<CustomerProfile> CustomerProfileTbl { get; set; }
        public DbSet<ProviderProfile> ProviderProfileTbl { get; set; }
        public DbSet<Location> LocationTbl { get; set; }
        public DbSet<Package> PackageTbl { get; set; }
        public DbSet<Order> OrderTbl { get; set; }
        public DbSet<Feedback> FeedbackTbl { get; set; }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     base.OnModelCreating(modelBuilder);
        // }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=127.0.0.1,1433;Database=TravelCompany;User Id=sa;Password=123123123Vu; TrustServerCertificate=True; Encrypt=false;");
        }
    }
}