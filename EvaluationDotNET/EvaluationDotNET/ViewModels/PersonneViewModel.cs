using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluationDotNET.ViewModels
{
    class PersonneViewModel
    {
        public string CIN { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Sexe { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public PersonneViewModel() { }
        public PersonneViewModel(PersonneViewModel PVM)
        {
            CIN = PVM.CIN;
            Name = PVM.Name;
            BirthDate = PVM.BirthDate;
            Sexe = PVM.Sexe;
            FatherName = PVM.FatherName;
            MotherName = PVM.MotherName;
        }
    }
}
