using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EvaluationDotNET.BO;
using System.Windows.Forms;
using EvaluationDotNET.ViewModels;

namespace EvaluationDotNET.DAL
{
    class DAL
    {

        SqlConnection Connection;
        string CnxString = "Data Source=DESKTOP-PROIPHB\\DAMSONSERVER;Initial Catalog=Personne;User ID=sa;Password=root;integrated security=true;MultipleActiveResultSets=true;";
        public DAL()
        {
            Connection = new SqlConnection(CnxString);
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {

                throw e;

            }

            Connection.Close();
        }
        public int AddPerson(Personnes pr)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Personnes(CIN,Nom,BirthDate,Sexe,IDFather,IDMother) VALUES(@CIN,@Name,@BirthDate,@Sexe,@IDFather,@IDMother)", Connection);
               // cmd.Parameters.AddWithValue("@ID", pr.ID);
                cmd.Parameters.AddWithValue("@CIN", pr.CIN);
                cmd.Parameters.AddWithValue("@Name", pr.Name);
                cmd.Parameters.AddWithValue("BirthDate", pr.BirthDate);
                cmd.Parameters.AddWithValue("Sexe", pr.Sexe);
                switch (pr.IDFather)
                {
                    case null:
                        cmd.Parameters.AddWithValue("@IDFather", DBNull.Value);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@IDFather", pr.IDFather.ID);
                        break;
                }

                switch (pr.IDMother)
                {
                    case null:
                        cmd.Parameters.AddWithValue("@IDMother", DBNull.Value);
                        break;
                    default:
                        cmd.Parameters.AddWithValue("@IDMother", pr.IDMother.ID);
                        break;
                }
                if (Connection.State == System.Data.ConnectionState.Closed)
                {
                    Connection.Open();
                }
                int Success = cmd.ExecuteNonQuery();
                Connection.Close();
                return Success;
            }catch(Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return 0;
            }


        }
        public Personnes GetOnePerson(string value)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) * FROM Personnes WHERE CIN=@CIN", Connection);
            cmd.Parameters.AddWithValue("@CIN", value);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            SqlDataReader data = cmd.ExecuteReader();
            if (data.Read())
            {
                Personnes per = new Personnes();
                per.ID = data.GetInt32(0);
                per.CIN = data.GetString(1);
                per.Name = data.GetString(2);
                per.BirthDate = data.GetDateTime(3);
                per.Sexe = data.GetString(4);
                //father insertion
                if (data.IsDBNull(5))
                {
                    per.IDFather = null;
                }
                else
                {
                    Personnes father = new Personnes();
                    father.ID = data.GetInt32(5);
                    per.IDFather = father;
                }

                //mother insertion
                if (data.IsDBNull(6))
                {
                    per.IDMother = null;
                }
                else
                {
                    Personnes mother = new Personnes();
                    mother.ID = data.GetInt32(6);
                    per.IDMother = mother;
                }
                return per;
            }
            return null;
        }

        public int DeletePerson(string value)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Personnes WHERE CIN=@CIN", Connection);
            cmd.Parameters.AddWithValue("@CIN", value);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            int success = cmd.ExecuteNonQuery();
            return success;
        }
        public List<Personnes> displayChildrenOfAPerson(Personnes P)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personnes WHERE IDFather=@ID",Connection);
            cmd.Parameters.AddWithValue("@ID", P.ID);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }

            SqlDataReader data = cmd.ExecuteReader();
            
            List<Personnes> children = new List<Personnes>();
            while (data.HasRows)
            {
                children = toPersonnesList(data);
                Connection.Close();
                return children;
            }
            return null;
        }

        public List<string> GetMaleNames()
        {
            SqlCommand cmd = new SqlCommand("SELECT Nom FROM Personnes WHERE Sexe=@sexe", Connection);
            cmd.Parameters.AddWithValue("@sexe", "M");
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
           SqlDataReader data =  cmd.ExecuteReader();
            List<string> MaleNames = new List<string>();
            while (data.Read())
            {
                MaleNames.Add(data.GetString(0));
               
            }
            return MaleNames;
            
        }
        public List<string> GetFemaleNames()
        {
            SqlCommand cmd = new SqlCommand("SELECT Nom FROM Personnes WHERE Sexe=@sexe", Connection);
            cmd.Parameters.AddWithValue("@sexe", "F");
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            SqlDataReader data = cmd.ExecuteReader();
            List<string> FemaleNames = new List<string>();
            while (data.Read())
            {
                FemaleNames.Add(data.GetString(0));

            }
            return FemaleNames;

        }


        public Personnes GetIdOutOfName(string v)
        {
            SqlCommand cmd = new SqlCommand("SELECT ID FROM Personnes WHERE Nom=@Name", Connection);
            cmd.Parameters.AddWithValue("@Name", v);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            SqlDataReader data = cmd.ExecuteReader();
            if (data.Read())
            {
                Personnes pr = new Personnes();
                pr.ID = data.GetInt32(0);
                return pr;
            }
            else
            {
                return null;
            }
            

        }

        public List<Personnes> GetAllPersonnes()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personnes", Connection);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            SqlDataReader data = cmd.ExecuteReader();
            List<Personnes> temp = new List<Personnes>();
            if (data.HasRows)
            {
                temp = toPersonnesList(data);
                Connection.Close();
                return temp;
            }
            else
            {
                Connection.Close();
                return temp;
            }
        }
        public List<PersonneViewModel> toBeDisplayedList()
        {
            List<Personnes> list = GetAllPersonnes();
            List<PersonneViewModel> listModelView = new List<PersonneViewModel>();
             
            for(int i = 0; i< list.Count; i++)
            {
                PersonneViewModel PVM = new PersonneViewModel();
                PVM.CIN = list[i].CIN;
                PVM.Name = list[i].Name;
                PVM.BirthDate = list[i].BirthDate;
                PVM.Sexe = list[i].Sexe;
                if (list[i].IDFather == null)
                {
                    PVM.FatherName = "";
                }
                else {
                    PVM.FatherName = GetNameOutOfId(list[i].IDFather);
                }

                if (list[i].IDMother == null)
                {
                    PVM.MotherName = "";
                }
                else
                {
                    PVM.MotherName = GetNameOutOfId(list[i].IDMother);
                }
                listModelView.Add(new PersonneViewModel(PVM));
            }
            return listModelView;
        }

        private string GetNameOutOfId(Personnes per)
        {
            SqlCommand cmd = new SqlCommand("SELECT Nom FROM Personnes WHERE ID=@ID", Connection);
            cmd.Parameters.AddWithValue("@ID", per.ID);
            if (Connection.State == System.Data.ConnectionState.Closed)
            {
                Connection.Open();
            }
            SqlDataReader data = cmd.ExecuteReader();
            if (data.Read())
            {
                
                string name = data.GetString(0);
                return name;
            }
            else
            {
                return null;
            }


        }

        private List<Personnes> toPersonnesList(SqlDataReader data)
        {
            List<Personnes> temp = new List<Personnes>();
            Personnes per = new Personnes();
            while (data.Read())
            {
                per.ID = data.GetInt32(0);
                per.CIN = data.GetString(1);
                per.Name = data.GetString(2);
                per.BirthDate = data.GetDateTime(3);
                per.Sexe = data.GetString(4);
                //father insertion
                if (data.IsDBNull(5))
                {
                    per.IDFather = null;
                }
                else
                {
                    Personnes father = new Personnes();
                    father.ID = data.GetInt32(5);
                    per.IDFather = father;
                }

                //mother insertion
                if (data.IsDBNull(6))
                {
                    per.IDMother = null;
                }
                else
                {
                    Personnes mother = new Personnes();
                    mother.ID = data.GetInt32(6);
                    per.IDMother = mother;
                }

               
                temp.Add(new Personnes(per));
            }
            return temp;
        }
    }
}
