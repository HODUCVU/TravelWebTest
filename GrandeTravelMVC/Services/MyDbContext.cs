// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravelMVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GrandeTravelMVC.Services
{
    public class MyDbContext: DbContext
    {
        public DbSet<CustomerProfile> CustomerProfileTbl { get; set; }
        public DbSet<ProviderProfile> ProviderProfileTbl { get; set; }
        public DbSet<Location> LocationTbl { get; set; }
        public DbSet<Package> PackageTbl { get; set; }
        public DbSet<Order> OrderTbl { get; set; }
        public DbSet<Feedback> FeedbackTbl { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder option) 
        {
            option.UseSqlServer(@"Server=127.0.0.1,1433; Database=TravelCompany; Trusted_Connection=True;");
        }
    }
}
