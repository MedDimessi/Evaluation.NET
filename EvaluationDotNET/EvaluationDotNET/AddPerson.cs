using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EvaluationDotNET.BO;
using EvaluationDotNET.DAL; 

namespace EvaluationDotNET
{
    public partial class AddPerson : Form
    {

        DAL.DAL dal = new DAL.DAL();
        public AddPerson()
        {
            InitializeComponent();
            Object[] m = convertListToObject(getMaleNames());
            FatherComboBox.Items.AddRange(m);
            Object[] f = convertListToObject(getFemaleNames());
            MotherComboBox.Items.AddRange(f);
        }

        private void ValidateAddingButton_Click(object sender, EventArgs e)
        {
            Personnes personnes = new Personnes();
            personnes.CIN = CINTextBox.Text;
            personnes.Name = NameTextBox.Text;
            personnes.BirthDate = DateTime.Parse(BirthDateDatePicker.Text);
            if (MaleRadioButton.Checked)
            {
                personnes.Sexe = "M";
            }else if (FemaleRadioButton.Checked)
            {
                personnes.Sexe = "F";
            }
            if (FatherComboBox.SelectedItem == null)
            {
                personnes.IDFather = null;
            }
            else
            {
                Personnes father = new Personnes();
                father = dal.GetIdOutOfName(FatherComboBox.SelectedItem.ToString());
                personnes.IDFather = father;
            }
            if (MotherComboBox.SelectedItem == null)
            {
                personnes.IDMother = null;
            }
            else
            {
                Personnes Mother = new Personnes();
                Mother = dal.GetIdOutOfName(MotherComboBox.SelectedItem.ToString());
                personnes.IDMother = Mother;
            }
           int a =  dal.AddPerson(personnes);
            if (a == 1) MessageBox.Show("Person Added Successfully!!");
            this.Hide();
        }
        private List<string> names = new List<string>();
        public List<string> getMaleNames()
        {
           return dal.GetMaleNames();
        }
        public List<string> getFemaleNames()
        {
            return dal.GetFemaleNames();
        }
            
        private Object[] convertListToObject(List<string> li)
        {
            Object[] r = new Object[li.Count];
            for (int i = 0; i < li.Count; i++)
                r[i] = li[i];
            return r;

        }



    }
}
