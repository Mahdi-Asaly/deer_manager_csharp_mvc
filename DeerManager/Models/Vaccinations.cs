namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Vaccinations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(10)]
        public string DateOfVaccination { get; set; }

        [StringLength(10)]
        public string Medicine { get; set; }

        [StringLength(10)]
        public string NextVaccinationDate { get; set; }

        public virtual maintable maintable { get; set; }
    }
}
