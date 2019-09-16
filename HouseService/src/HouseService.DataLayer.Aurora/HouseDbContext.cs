using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseService.DataLayer.Aurora
{
    public class HouseDbContext : DbContext
    {
        public DbSet<HouseDTO> Houses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DbConnection.GetConnectionString());

        }

     

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HouseDTO>(a => a.HasKey(h => h.Id));
            modelBuilder.Entity<HouseDTO>().Property(a => a.Description).IsRequired(false).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<HouseDTO>().Property(a => a.NoOfGarage).IsRequired();
            modelBuilder.Entity<HouseDTO>().Property(a => a.NoOfRooms).IsRequired();
            modelBuilder.Entity<HouseDTO>().Property(a => a.Postcode).IsRequired();
            modelBuilder.Entity<HouseDTO>().Property(a => a.SizeInSquareFeet).IsRequired();
            modelBuilder.Entity<HouseDTO>().Property(a => a.StreetAddress).IsRequired().HasMaxLength(2000);
            modelBuilder.Entity<HouseDTO>().Property(a => a.Title).IsRequired().HasMaxLength(255);

        
            base.OnModelCreating(modelBuilder);
        }

    }
}
