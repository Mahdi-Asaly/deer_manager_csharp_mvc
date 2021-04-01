namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Medicine")]
    public partial class Medicine
    {
        [Key]
        [StringLength(50)]
        [Required(ErrorMessage ="שם תרופה חובה",AllowEmptyStrings =false)]
        public string MedName { get; set; }

        [StringLength(50)]
        public string Info { get; set; }

        public int? isGovermental { get; set; }
    }
}
