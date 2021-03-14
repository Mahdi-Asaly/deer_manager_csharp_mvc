using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.Classes
{
    public class VacsAlert
    {
        public int Id { get; set; }
        public DateTime NextDate = new DateTime();
        public int Group;
        public bool flag { get; set; } //flag to upcoming vacs
        public int days { get; set; }
    }
}