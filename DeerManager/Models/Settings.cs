namespace DeerManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Settings
    {

        [StringLength(10)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        public int AlertPopup { get; set; }

        [StringLength(10)]
        public string Address { get; set; }
        [Key]
        public int Id { get; set; }
    }
}
