using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Burger> Burgers { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuBurger> MenuBurgers { get; set; }
        public DbSet<Complement> Complements { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Quartier> Quartiers { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LignesCommande { get; set; }
        public DbSet<Paiement> Paiements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Burger>().ToTable("burgers");
            modelBuilder.Entity<Menu>().ToTable("menus");
            modelBuilder.Entity<MenuBurger>().ToTable("menus_burgers");
            modelBuilder.Entity<Complement>().ToTable("complements");
            modelBuilder.Entity<Zone>().ToTable("zones");
            modelBuilder.Entity<Quartier>().ToTable("quartiers");
            modelBuilder.Entity<Commande>().ToTable("commandes");
            modelBuilder.Entity<LigneCommande>().ToTable("lignes_commande");
            modelBuilder.Entity<Paiement>().ToTable("paiements");

            modelBuilder.Entity<MenuBurger>()
                .HasKey(mb => new { mb.IdMenu, mb.IdBurger });

            modelBuilder.Entity<Paiement>()
                .HasIndex(p => p.IdCommande)
                .IsUnique();

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<Commande>()
                .Property(c => c.EtatCommande)
                .HasConversion<string>();

            modelBuilder.Entity<Commande>()
                .Property(c => c.ModeConsommation)
                .HasConversion<string>();

            modelBuilder.Entity<LigneCommande>()
                .Property(l => l.TypeProduit)
                .HasConversion<string>();

            modelBuilder.Entity<Paiement>()
                .Property(p => p.ModePaiement)
                .HasConversion<string>();

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany(cl => cl.Commandes)
                .HasForeignKey(c => c.IdClient)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Zone)
                .WithMany(z => z.Commandes)
                .HasForeignKey(c => c.IdZone)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Quartier>()
                .HasOne(q => q.Zone)
                .WithMany(z => z.Quartiers)
                .HasForeignKey(q => q.IdZone)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LigneCommande>()
                .HasOne<Commande>()
                .WithMany(c => c.LignesCommande)
                .HasForeignKey(lc => lc.IdCommande)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
