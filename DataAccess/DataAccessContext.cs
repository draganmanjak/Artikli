using DataAccess.Infrastructure.Models;
using DataAccess.Infrastructure.ViewModels;
using DataAccess.UserManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace DataAccess
{
    public class DataAccessContext : DbContext
    {
        public DbSet<JediniceMjere> JediniceMjere { get; set; }
        public DbSet<ArtikliModel> Artikli { get; set; }
        public DbSet<Atributi> Atributi { get; set; }

        public DbSet<AtributiArtikla> AtributiArtikla { get; set; }

        public DbSet<ApplicationUser> Users { get; set; }

        public DataAccessContext(DbContextOptions<DataAccessContext> options) : base(options)
        {
         
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ArtikliSearchViewModel>();
            modelBuilder.Ignore<ArtikliViewModel>();
            modelBuilder.Ignore<AtributiArtiklaViewModel>();
            modelBuilder.Ignore<AtributiViewModel>();
            modelBuilder.Ignore<AtributKeyValueModel>();
            modelBuilder.Ignore<JediniceMjereViewModel>();
            modelBuilder.Ignore<PaginatedListViewModel>();
            modelBuilder.Entity<JediniceMjere>().HasData(
                new { PkJedinicaMjereId =(long) 1, Naziv = "kg" },
                new { PkJedinicaMjereId = (long)2, Naziv = "l" },
                new { PkJedinicaMjereId = (long)3, Naziv = "m" },
                new { PkJedinicaMjereId = (long)4, Naziv = "cm" },
                new { PkJedinicaMjereId = (long) 5, Naziv = "dm" });
            // Artikli i Atributi više na više
            modelBuilder.Entity<AtributiArtikla>()
                 .HasKey(a => new { a.PkFkArtikalId, a.PkFkAtributId });

            modelBuilder.Entity<AtributiArtikla>()
              .HasOne<ArtikliModel>(aa => aa.Artikal)
              .WithMany(a => a.AtributiArtikla)
              .HasForeignKey(aa => aa.PkFkArtikalId);

            modelBuilder.Entity<AtributiArtikla>()
                .HasOne<Atributi>(aa => aa.Atribut)
                .WithMany(a => a.ArtikliAtributa)
                .HasForeignKey(aa => aa.PkFkAtributId);

        }

    }
}
