using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.ViewModels
{
    public class UserViewModel
    {
        public IList<Diseases> shpDiseases { get; set; }
        public IList<Hamlatot> shpHamlata { set; get; }
        public maintable maintblSheeps { set; get; }
        public Details shpDetail { set; get; }
        public IList<Vaccinations> shpVac { set; get; }
    }
}