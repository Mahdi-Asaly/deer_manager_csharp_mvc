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
            Vaccinations = new HashSet<Vaccinations>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage ="יש להכניס מספר כבש")]
        public int SheepNum { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "יש לבחור סוג דם")]
        [StringLength(10)]
        public string Blood { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "יש לבחור מין")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "יש להכניס מספר קבוצה")]
        public int Group { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "יש לבחור תאריך לידה")]
        [StringLength(10)]
        public string Birthday { get; set; }

        public virtual Details Details { get; set; }

        public virtual Diseases Diseases { get; set; }

        public virtual Hamlatot Hamlatot { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vaccinations> Vaccinations { get; set; }
    }
}
