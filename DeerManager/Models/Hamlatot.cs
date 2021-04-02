namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hamlatot")]
    public partial class Hamlatot
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(50)]
        public string DateOfHamlata { get; set; }

        [StringLength(50)]
        public string DateOfTakser { get; set; }

        public virtual maintable maintable { get; set; }
    }
}
