namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("maintable")]
    public partial class maintable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public maintable()
        {
            Hamlata = new HashSet<Hamlata>();
            Hasroot = new HashSet<Hasroot>();
            TakserTable = new HashSet<TakserTable>();
            Vaccinations = new HashSet<Vaccinations>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int SheepNum { get; set; }

        [Required]
        [StringLength(10)]
        public string Blood { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public int Group { get; set; }

        [StringLength(10)]
        public string Birthday { get; set; }

        public virtual Details Details { get; set; }

        public virtual Diseases Diseases { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hamlata> Hamlata { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hasroot> Hasroot { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TakserTable> TakserTable { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vaccinations> Vaccinations { get; set; }
    }
}
