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
using EvaluationDotNET.ViewModels;

namespace EvaluationDotNET
{
    public partial class DisplayPersons : Form
    {
        DAL.DAL dal = new DAL.DAL();
        public DisplayPersons()
        {
            InitializeComponent();
            prepareGrid();
            DisplayPersonnesOnGrid(dal.toBeDisplayedList());
        }

        private void prepareGrid()
        {
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.TabStop = false;
        }
        private void DisplayPersonnesOnGrid(List<PersonneViewModel> personnes )
        {
            prepareGrid();

            // fac = pr.displayFactures();
            if (personnes.Count == 1)
            {
                dataGridView1.DataSource = personnes;
                dataGridView1.AutoGenerateColumns = true;
            }
            else
            {
                BindingSource bs = new BindingSource();
                foreach (var per in personnes)
                {
                    bs.Add(per);
                }

                dataGridView1.DataSource = bs;
                dataGridView1.AutoGenerateColumns = true;
            }
        }

        private void AddPersonButton_Click(object sender, EventArgs e)
        {
            AddPerson addPerson = new AddPerson();
            addPerson.Show();
        }

        private void GridClicked(object sender, MouseEventArgs e)
        {
            DisplayPersonnesOnGrid(dal.toBeDisplayedList());
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;
            string value = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            int s = dal.DeletePerson(value);
            if (s == 1)
                MessageBox.Show("Person has been deleted successfully!!", "Deleting Row!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        public string DisplayFamilyTree(Personnes P , ref string familyTree)
        {
            //familyTree = "";
            List<Personnes> p = new List<Personnes>();
            p = dal.displayChildrenOfAPerson(P);
            if (p == null)
            {
                return familyTree;

            }
            else
            {
                familyTree += P.Name + ": ";
                for (int i = 0; i < p.Count; i++)
                {
                    familyTree += " " + p[i].Name + " ,";
                    DisplayFamilyTree(p[i], ref familyTree);
                    familyTree += "\n";
                }
            }
            return familyTree;
        }

        private void FamilyTreeButton_Click(object sender, EventArgs e)
        {
            int rowIndex = dataGridView1.CurrentRow.Index;
            string value = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
            Personnes per = new Personnes(dal.GetOnePerson(value));
            string s = "";
            MessageBox.Show(DisplayFamilyTree(per,ref s));
        }
    }
}
