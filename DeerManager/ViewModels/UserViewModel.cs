using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<Diseases> shpDiseases { get; set; }
        public IEnumerable<Details> shpDetail { set; get; }
        public IEnumerable<Hamlatot> shpHamlata { set; get; }
        public maintable maintblSheeps { set; get; }
        public IEnumerable<Vaccinations> shpVac { set; get; }
    }
}