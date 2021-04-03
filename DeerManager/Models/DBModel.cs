using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DeerManager.Models
{
    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Details> Details { get; set; }
        public virtual DbSet<Diseases> Diseases { get; set; }
        public virtual DbSet<Hamlata> Hamlata { get; set; }
        public virtual DbSet<Hasroot> Hasroot { get; set; }
        public virtual DbSet<maintable> maintable { get; set; }
        public virtual DbSet<Medicine> Medicine { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TakserTable> TakserTable { get; set; }
        public virtual DbSet<Vaccinations> Vaccinations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<maintable>()
                .Property(e => e.Blood)
                .IsFixedLength();

            modelBuilder.Entity<maintable>()
                .Property(e => e.Gender)
                .IsFixedLength();

            modelBuilder.Entity<maintable>()
                .Property(e => e.Birthday)
                .IsFixedLength();

            modelBuilder.Entity<maintable>()
                .HasOptional(e => e.Details)
                .WithRequired(e => e.maintable)
                .WillCascadeOnDelete();

            modelBuilder.Entity<maintable>()
                .HasOptional(e => e.Diseases)
                .WithRequired(e => e.maintable)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Settings>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Settings>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<Settings>()
                .Property(e => e.Password)
                .IsFixedLength();

            modelBuilder.Entity<Settings>()
                .Property(e => e.Address)
                .IsFixedLength();
        }
    }
}
