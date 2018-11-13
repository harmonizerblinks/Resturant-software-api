using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resturant.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Sequence> Sequence { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Discount> Discount { get; set; }
        public DbSet<Activity> Activity { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Nominal> Nominal { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SmsApi> SmsApi { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<Teller> Teller { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
