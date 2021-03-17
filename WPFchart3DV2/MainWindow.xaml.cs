using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Windows.Data;
using System.Globalization;
using System.Collections.Generic;

namespace WPFchart3DV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-MUMHPSL\\SQLEXPRESS;Initial Catalog=wpf_base;Integrated Security=True");
        string strName, imageName;
        public string myval;

        public string Myval
        {
            get { return myval; }
            set { myval = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            ComboLoid(combo1);
            affiche_filiere();
            grid_filiere.Visibility = Visibility.Hidden;
            GridMain.Visibility = Visibility.Hidden;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var x = combo1.SelectedValue;
            filier_name.Text = (string)x;
            textBox_path.Text = (string)x;
            SqlCommand cmd = new SqlCommand("SELECT Responsable FROM Filiere where Nom_filiere = @resp ", con);
            cmd.Parameters.AddWithValue("@resp", x);
            con.Close();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                btn_resp.Text = sdr.GetValue(0).ToString();
            }
            Affiche_Etudiant();
            con.Close();
        }

        private void Affiche_Etudiant()
        {
            dataGridView1.Columns.Clear();
            con.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM etudiant where nom_fil = @nom_fil ", con);
            cmd.Parameters.AddWithValue("@nom_fil", combo1.SelectedValue);
            DataTable dt = new DataTable();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            
            dataGridView1.ItemsSource = dt.DefaultView;
            //dt.Columns[6].Visibility = Visibility.Collapsed;
            DataGridTemplateColumn column = new DataGridTemplateColumn();
            column.Header = "Image";
            FrameworkElementFactory imageFactory = new FrameworkElementFactory(typeof(Image));
            imageFactory.SetBinding(Image.SourceProperty, new Binding("imgg") { Converter = new ByteArrayToImageSourceConverter() });
            column.CellTemplate = new DataTemplate() { VisualTree = imageFactory };
            dataGridView1.Columns.Add(column);

        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileDialog fldlg = new OpenFileDialog();
                fldlg.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
                fldlg.Filter = "Image File (*.jpg;*.bmp;*.gif)|*.jpg;*.bmp;*.gif";
                fldlg.ShowDialog();
                {
                    strName = fldlg.SafeFileName;
                    imageName = fldlg.FileName;
                    ImageSourceConverter isc = new ImageSourceConverter();
                    image1.SetValue(Image.SourceProperty, isc.ConvertFromString(imageName));
                }
                fldlg = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        FileStream fs;
        void insertImageData()
        {
            try { 
            //Initialize a file stream to read the image file
            fs = new FileStream(imageName, FileMode.Open, FileAccess.Read);
            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];
            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));
            //Close a file stream
            fs.Close();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into etudiant(cne, nom, prenom, sexe, date_naiss,nom_fil, imgg) values( @cne_etudiant ,@nom_etudiant,@prenom_etudiant,@sexe_etudiant,@naissance_etudiant,@nom_fil,@img)", con);
            //Pass byte array into database  

            
            cmd.Parameters.Add(new SqlParameter("img", imgByteArr));
            cmd.Parameters.AddWithValue("@cne_etudiant", btn_cne.Text);
            cmd.Parameters.AddWithValue("@nom_etudiant", btn_name.Text);
            cmd.Parameters.AddWithValue("@prenom_etudiant", btn_prenom.Text);
            cmd.Parameters.AddWithValue("@sexe_etudiant", btn_sex.Text);
            cmd.Parameters.AddWithValue("@naissance_etudiant", btn_dateSelect.Text);
            cmd.Parameters.AddWithValue("@nom_fil", combo2.Text);

            if (cmd.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Student added successfully.");
                Affiche_Etudiant();
            }
            con.Close();
            }catch(ArgumentNullException e)
            {
                MessageBox.Show("Please insert the image !!");
            }
        }

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            if (btn_cne.Text != "" && btn_name.Text != "" && btn_prenom.Text != "" && btn_sex.Text != "" && btn_dateSelect.Text != "" && combo2.Text != "" )
            {
                insertImageData();
                Affiche_Etudiant();
                btn_cne.Text = "";
                btn_name.Text = "";
                btn_prenom.Text = "";
                btn_sex.Text = "";
                btn_dateSelect.Text = "";
                grid_add.Visibility = Visibility.Hidden;
            }
            else MessageBox.Show("Please fill Enter all your informations !!");



        }

        private void dataGridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                Myval = row_selected["nom"].ToString();
            }
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            ModEtudiant bwin = new ModEtudiant(Myval);
            bwin.Show();
        }

        private void textBox_path_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btn_cne.Text = "";
            btn_name.Text = "";
            btn_prenom.Text = "";
            btn_sex.Text = "";
            btn_dateSelect.Text = "";
            grid_add.Visibility = Visibility.Hidden;
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            ComboLoid(combo2);
            grid_add.Visibility = Visibility.Visible;
        }

        void ComboLoid(ComboBox combo)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Filiere", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {
                string name = sdr.GetString(1);
                combo.Items.Add(name);
            };
            combo.SelectedIndex = 0;
            con.Close();
        }

        //**************** Partie Filier *******************************
        List<Filieres> listContact;
        DataSet ds = new DataSet();


        private void affiche_filiere()
        {
            listBoxNames.Items.Clear();
            con.Open();
            SqlCommand filieresCmd = new SqlCommand("SELECT * FROM Filiere", con);
            SqlDataReader filieres = filieresCmd.ExecuteReader();

            while (filieres.Read())
            {
                listBoxNames.Items.Add(new Filieres()
                {
                    Id = Convert.ToString(filieres[0]),
                    Nom = Convert.ToString(filieres[1]),
                    Responsable = Convert.ToString(filieres[2])
                });
            }

            con.Close();
        }

        private void listBoxNames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxNames.Items.Count != 0)
            {
                Filieres selectedItem = (Filieres)listBoxNames.SelectedItem;
                filiere_id.Text = selectedItem.Id;
                filiere_name.Text = selectedItem.Nom;
                responsable_name.Text = selectedItem.Responsable;
            }
        }

        private void ajouter_filiere_Click(object sender, RoutedEventArgs e)
        {
            if (filiere_name.Text.Length == 0)
            {
                MessageBox.Show("S'il vous plais Inserer une filiere!!");
            }
            else
            {
                if (notExist())
                {
                    con.Open();

                    SqlCommand Insertcmd = new SqlCommand("INSERT INTO Filiere(Id_filiere,Nom_filiere,Responsable) VALUES (@id_fil,@nom_filiere,@resp)", con);
                    Insertcmd.Parameters.AddWithValue("@nom_filiere", filiere_name.Text);
                    Insertcmd.Parameters.AddWithValue("@id_fil", filiere_id.Text);
                    Insertcmd.Parameters.AddWithValue("@resp", responsable_name.Text);



                    Insertcmd.ExecuteNonQuery();
                    con.Close();

                    affiche_filiere();
                    MessageBox.Show("Successfully Inserted!!");
                    
                    //ComboLoid(combo1);
                    Affiche_Etudiant();
                }
                else
                {
                    MessageBox.Show("La filiere " + filiere_name.Text + " is already exist !");
                }
                filiere_name.Text = "";
                filiere_id.Text = "";
                responsable_name.Text = "";

            }
        }

        private void Refresh()
        {
            throw new NotImplementedException();
        }

        private void modifier_filiere_Click(object sender, RoutedEventArgs e)
        {
            if (!validFiliere())
            {
                MessageBox.Show("S'il vous plais Inserer une filiere valid!!");
            }
            else
            {
                if (!notExistId())
                {
                    con.Open();

                    SqlCommand Updatecmd = new SqlCommand("UPDATE Filiere SET Nom_filiere=@Nom_filiere , Responsable=@resp WHERE Id_filiere = @Id_filiere", con);
                    Updatecmd.Parameters.AddWithValue("@Nom_filiere", filiere_name.Text);
                    Updatecmd.Parameters.AddWithValue("@resp", responsable_name.Text);
                    Updatecmd.Parameters.AddWithValue("@Id_filiere", Convert.ToInt32(filiere_id.Text));

                    Updatecmd.ExecuteNonQuery();
                    con.Close();
                    filiere_name.Text = "";
                    filiere_id.Text = "";
                    responsable_name.Text = "";

                    affiche_filiere();
                    //ComboLoid(combo1);
                    
                    MessageBox.Show("Successfully Updated!!!");
                    
                }
                else
                {
                    MessageBox.Show("Cette filiere n'existe pas dans notre ecole !");
                }
            }
        }

        private void supprime_filiere_Click(object sender, RoutedEventArgs e)
        {
            if (!validFiliere())
            {
                MessageBox.Show("S'il vous plais Inserer une filiere valid!!");
            }
            else
            {
                if (!notExist())
                {
                    string msg = "Vous voulez supprimer la filiere " + filiere_name.Text + MessageBoxImage.Question;
                    var confirm = MessageBox.Show(msg, "", MessageBoxButton.YesNo);
                    if (confirm == MessageBoxResult.Yes)
                    {
                        con.Open();

                        SqlCommand deleteCmd = new SqlCommand("DELETE FROM Filiere where  Id_Filiere = @Id_Filiere", con);
                        deleteCmd.Parameters.AddWithValue("@Id_Filiere", Convert.ToInt32(filiere_id.Text));
                        deleteCmd.ExecuteNonQuery();
                        con.Close();

                        affiche_filiere();
                        //ComboLoid(combo1);
                        MessageBox.Show("Successfully Deleted!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Cette filiere n'existe pas dans notre ecole !");
                }
            }
        }

        private bool notExist()
        {
            SqlCommand checkcmd = new SqlCommand("SELECT * FROM Filiere where Nom_filiere=@Nom_filiere", con);
            checkcmd.Parameters.AddWithValue("@Nom_filiere", filiere_name.Text);

            SqlDataAdapter da = new SqlDataAdapter(checkcmd);
            da.Fill(ds);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
            {
                ds.Clear();
                return false;
            }
            return true;
        }

        private bool notExistId()
        {
            SqlCommand checkcmd = new SqlCommand("SELECT * FROM Filiere where Id_filiere=@Id_filiere", con);
            checkcmd.Parameters.AddWithValue("@Id_filiere", Convert.ToInt32(filiere_id.Text));

            SqlDataAdapter da = new SqlDataAdapter(checkcmd);
            da.Fill(ds);
            int index = ds.Tables[0].Rows.Count;
            if (index > 0)
            {
                ds.Clear();
                return false;
            }
            return true;
        }

        private bool validFiliere()
        {
            if (filiere_id.Text.Length == 0 || filiere_name.Text.Length == 0 || responsable_name.Text.Length ==0)
            {
                return false;
            }
            return true;
        }

        private void btn_stat_Click(object sender, RoutedEventArgs e)
        {
            grid_filiere.Visibility = Visibility.Visible;
            GridMain.Visibility = Visibility.Hidden;
            grid.ItemsSource = null;
            ViewModel v = new ViewModel();
            grid.ItemsSource = v.Data;

        }

        private void btn_filiere_Click(object sender, RoutedEventArgs e)
        {
            grid_filiere.Visibility = Visibility.Hidden;
            GridMain.Visibility = Visibility.Visible;
        }

        private void btn_etud_Click(object sender, RoutedEventArgs e)
        {
            Affiche_Etudiant();
            grid_filiere.Visibility = Visibility.Hidden;
            GridMain.Visibility = Visibility.Hidden;
        }

        private void btn_sex_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }

        private void displayFiliere(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ComboLoid(combo1);
        }

        public class Filieres
        {
            public string Id { get; set; }
            public string Nom { get; set; }
            public string Responsable { get; set; }

        }
    }

    [ValueConversion(typeof(System.Drawing.Image), typeof(System.Windows.Media.ImageSource))]
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imageData = (byte[])value;

            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
    