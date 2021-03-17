using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFchart3DV2
{
    /// <summary>
    /// Interaction logic for ModEtudiant.xaml
    /// </summary>
    public partial class ModEtudiant : Window
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-MUMHPSL\\SQLEXPRESS;Initial Catalog=wpf_base;Integrated Security=True");

        public ModEtudiant(string v)
        {
            InitializeComponent();
            btn_name.Text = v;
            SqlCommand cmd = new SqlCommand("SELECT * FROM etudiant WHERE nom = @nom_etudiant", con);

            cmd.Parameters.AddWithValue("@nom_etudiant", v);
            DataTable dt = new DataTable();
            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                btn_cne.Text = sdr.GetValue(0).ToString();
                btn_name.Text = sdr.GetValue(1).ToString();
                btn_prenom.Text = sdr.GetValue(2).ToString();
                btn_sex.Text = sdr.GetValue(3).ToString();
                btn_dateSelect.Text= sdr.GetValue(4).ToString(); 
            }
            con.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (btn_cne.Text.Trim().Length != 0 && btn_name.Text.Trim().Length != 0 && btn_prenom.Text.Trim().Length != 0 && btn_dateSelect.Text.Trim().Length != 0)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE etudiant SET nom = @nom_etudiant,prenom = @prenom_etudiant, sexe = @sexe_etudiant, date_naiss = @naissance_etudiant WHERE cne = @cne_etudiant ", con);
                cmd.Parameters.AddWithValue("@cne_etudiant", btn_cne.Text);
                cmd.Parameters.AddWithValue("@nom_etudiant", btn_name.Text);
                cmd.Parameters.AddWithValue("@prenom_etudiant", btn_prenom.Text);
                cmd.Parameters.AddWithValue("@sexe_etudiant", btn_sex.Text);
                cmd.Parameters.AddWithValue("@naissance_etudiant", btn_dateSelect.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Successfully Updated!!");
                
            }else
            {
                MessageBox.Show("S'il vous plait entrez toutes Les informations !");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            btn_cne.Text ="";
            btn_name.Text = "";
            btn_prenom.Text = "";
            btn_sex.Text = "";
            btn_dateSelect.Text = "";
            this.Close();
        }
    }
}
