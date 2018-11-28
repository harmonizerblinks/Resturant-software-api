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
        public DbSet<Item> Item { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderList> OrderList { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SmsApi> SmsApi { get; set; }
        public DbSet<Sms> Sms { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockLog> StockLog { get; set; }
        public DbSet<Nominal> Nominal { get; set; }
        public DbSet<Teller> Teller { get; set; }
        public DbSet<Transit> Transit { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<AppUser>().HasData(
            //   new AppUser { UserName = "Harmony", PhoneNumber = "0238288675", Email = "harmonizerblinks@gmail.com",
            //       EmailConfirmed = true, Login = DateTime.UtcNow, LogOut = DateTime.UtcNow, UserType = "Admin",
            //       MUserId = "807ba6c0-e845-4695-847e-92edca9d66db", MDate = DateTime.UtcNow, IsLoggedIn = false,
            //       PasswordHash = "AQAAAAEAACcQAAAAEOrzhtqT+9ZuBKNmqNY/4xVf8ruHsdTHMfE8KCVsipUBA9CjcZMVjYRaR0Nzl8jgKQ=="
            //   });
            //builder.Entity<AppUser>(i =>
            //{
            //    //i.HasKey(u => u.AppUserId);
            //    i.HasIndex(u => u.AppUserId);
            //});
            //builder.Entity<AppUser>().HasKey(u => new { u.Id, u.AppUserId });

            base.OnModelCreating(builder);
        }
    }
}
