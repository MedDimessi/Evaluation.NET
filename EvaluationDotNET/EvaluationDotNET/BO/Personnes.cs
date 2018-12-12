using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationDotNET.BO
{
    public class Personnes
    {
        public int? ID { get; set; }
        public string CIN { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sexe { get; set; }
        public Personnes IDFather { get; set; }
        public Personnes IDMother { get; set; }

        public Personnes() { }
        public Personnes(Personnes pr)
        {
            this.ID = pr.ID;
            this.CIN = pr.CIN;
            Name = pr.Name;
            BirthDate = pr.BirthDate;
            Sexe = pr.Sexe;
            IDFather = pr.IDFather;
            IDMother = pr.IDMother;
        }
    }

}
