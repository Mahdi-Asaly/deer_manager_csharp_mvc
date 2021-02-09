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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [StringLength(10)]
        public string Id { get; set; }

        public int SheepNum { get; set; }

        [Required]
        [StringLength(10)]
        public string Blood { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        public int Group { get; set; }

        [Required]
        [StringLength(10)]
        public string Birthday { get; set; }

        public int Status { get; set; }

    }
}
