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
        }
        public virtual DbSet<maintable> maintables { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<maintable>()
                .Property(e => e.Blood)
                .IsFixedLength();

            modelBuilder.Entity<maintable>()
                .Property(e => e.Gender)
                .IsFixedLength();

        }
    }
}
