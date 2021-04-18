using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.ViewModels
{
    public class UserViewModel
    {
        public Diseases shpDiseases { get; set; }
        public IList<Hasroot> shpHasraa { set; get; }
        public IList<Hamlata> shpHamlata { set; get; }
        public IList<TakserTable> takserTable { set; get; }

        public maintable maintblSheeps { set; get; }
        public Details shpDetail { set; get; }
        public IList<Vaccinations> shpVac { set; get; }
    }
}