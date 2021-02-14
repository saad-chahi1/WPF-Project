using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFchart3DV2
{
    class ViewModel
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-MUMHPSL\\SQLEXPRESS;Initial Catalog=student_management;Integrated Security=True");
        public List<notreClass> Data { get; set; }
        public ViewModel()
        {
            /*Data = new List<Sales>()
            {
                new Sales { Filiere="2014",M=300,F=200},
                new Sales { Filiere="2015",M=450,F=500},
                new Sales {Filiere="2016",M=400,F=300},
                new Sales {Filiere="2017",M=550,F=500},
                new Sales {Filiere="2018",M=650,F=450}
            };*/
            List<string> filiere_name = new List<string>();
            Dictionary<string, double> statistique = new Dictionary<string, double>();
            con.Open();
            SqlCommand filieresCmd = new SqlCommand("SELECT * FROM Filiere", con);
            SqlDataReader filieres = filieresCmd.ExecuteReader();

            while (filieres.Read())
            {
                filiere_name.Add(filieres[1].ToString());
            }

            con.Close();

            Data = new List<notreClass>();
            foreach (string f in filiere_name)
            {
                Data.Add(new notreClass { Filiere = f, nombresEtudiant = nombre_etudiant(f) });
            }
        }
        private int nombre_etudiant(string filiere)
        {
            con.Open();

            SqlCommand EtdCmd = new SqlCommand("SELECT COUNT(*) AS count FROM etudiant WHERE filieres=@nom_filiere", con);
            EtdCmd.Parameters.AddWithValue("@nom_filiere", filiere);
            SqlDataReader Etd = EtdCmd.ExecuteReader();

            while (Etd.Read())
            {
                total_etudiant_par_filiere = Convert.ToInt32(Etd[0]);
            }
            con.Close();

            return total_etudiant_par_filiere;
        }

        private int totalFiliere()
        {

            con.Open();
            SqlCommand EtdCmd = new SqlCommand("SELECT COUNT(*) AS total FROM etudiant", con);
            SqlDataReader Etd = EtdCmd.ExecuteReader();

            while (Etd.Read())
            {
                total_etudiant = Convert.ToInt32(Etd[0]);
            }
            con.Close();

            return total_etudiant;

        }
        public int total_etudiant = 0;
        public int total_etudiant_par_filiere = 0;
        public int stat_par_filiere;
    }
}
