namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vaccinations
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Medicine { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string NextVaccinationDate { get; set; }

        public int? isEnabled { get; set; }

        public virtual maintable maintable { get; set; }
    }
}
