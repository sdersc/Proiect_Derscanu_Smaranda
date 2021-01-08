using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HaberdasheryModel.Models;

namespace HaberdasheryModel.Data
{
    public class HaberdasheryContext : DbContext
    {
        public HaberdasheryContext(DbContextOptions<HaberdasheryContext> options) :base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Fabric> Fabrics { get; set; }
        public DbSet<Weaver> Weavers { get; set; }
        public DbSet<FabricWeaved> FabricWeaved { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Fabric>().ToTable("Fabric");
            modelBuilder.Entity<Weaver>().ToTable("Weaver");
            modelBuilder.Entity<FabricWeaved>().ToTable("FabricWeaved");

            modelBuilder.Entity<FabricWeaved>()
            .HasKey(c => new { c.FabricID, c.WeaverID });//configureaza cheia primara compusa
        }
    }
}
