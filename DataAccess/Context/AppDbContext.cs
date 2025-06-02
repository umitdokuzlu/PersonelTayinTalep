using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Personel> Personeller { get; set; }
        public DbSet<Adliye> Adliyeler { get; set; }
        public DbSet<TayinTalep> TayinTalepleri { get; set; }
        public DbSet<TayinTalepTercih> TayinTalepTercihleri { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TayinTalep>()
                .HasOne(t => t.Personel)
                .WithMany(p => p.TayinTalepleri)
                .HasForeignKey(t => t.PersonelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TayinTalepTercih>()
                .HasOne(tt => tt.TayinTalep)
                .WithMany(t => t.Tercihler)
                .HasForeignKey(tt => tt.TayinTalepId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TayinTalepTercih>()
                .HasOne(tt => tt.Adliye)
                .WithMany(a => a.TayinTalepTercihler)
                .HasForeignKey(tt => tt.AdliyeId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
