using DeerManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeerManager.ViewModels
{
    public class UserViewModel
    {
        public Details shpDetail { set; get; }
        public Diseases shpDiseases { set; get; }
        public Hamlatot shpHamlata { set; get; }
        public maintable maintblSheeps { set; get; }
        public Vaccinations shpVac { set; get; }
    }
}