using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.Models
{
    public class maintable
    {
        public int RowNum { get; set; }
        public int Id { get; set; }
        public int SheepNum { get; set; }
        public string BloodType { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int Group { get; set; }
        public Boolean IsAlive { set; get; }
    }
}