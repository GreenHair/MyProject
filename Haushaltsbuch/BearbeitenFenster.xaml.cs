using System;
using System.Collections.Generic;
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

namespace Haushaltsbuch
{
    /// <summary>
    /// Interaktionslogik für BearbeitenFenster.xaml
    /// </summary>

    public delegate void DelUpdater(MySql.Data.MySqlClient.MySqlCommand command);

    public partial class BearbeitenFenster : Window
    {
        public event DelUpdater Update;
        Suchergebnis _suche;

        public BearbeitenFenster(Suchergebnis suche)
        {
            InitializeComponent();
            _suche = suche;            
           // numBetrag.Text = suche.Artikel.betrag.ToString();
            DataContext = suche;
            
        }

        private void chkRechnung_Checked(object sender, RoutedEventArgs e)
        {
            btnSpeichern.IsEnabled = true;
            Enable_Fields(stckRechnung);
        }

        private void Enable_Fields(StackPanel stack)
        {
            foreach(UIElement element in stack.Children)
            {
                element.IsEnabled = true;
            }
        }

        private void chkRechnung_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkArtikel.IsChecked == false || chkArtikel.IsChecked == null) btnSpeichern.IsEnabled = false;
            Disable_Fields(stckRechnung);
        }

        private void Disable_Fields(StackPanel stack)
        {
            foreach (UIElement element in stack.Children)
            {
                element.IsEnabled = false;
            }
        }

        private void btnAbbrechen_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSpeichern_Click(object sender, RoutedEventArgs e)
        {
            MySql.Data.MySqlClient.MySqlCommand comm = new MySql.Data.MySqlClient.MySqlCommand();
            string sql = "";
            if(chkRechnung.IsChecked == true)
            {
                sql += "update rechnung set datum = '" + string.Format("{0:yyyy-MM-dd}", dpDatum.SelectedDate) + "', laden = " + ((Shop)cmbLaden.SelectedItem).id +
                    ((Person)cmbPerson.SelectedItem).id + ", einmalig = " + Convert.ToInt32((bool)chkEinmal.IsChecked) + " where rechnung.id = " + _suche.Kassenzettel.id + ";";
            }

            if(chkArtikel.IsChecked == true)
            {
                sql += "update ausgaben set bezeichnung = @bez, betrag = @betrag, prod_gr = " + ((Produktgruppe)cmbKategorie.SelectedItem).id +
                    " where ausgaben.id = " + _suche.Artikel.id;
                MySql.Data.MySqlClient.MySqlParameter par_bez = new MySql.Data.MySqlClient.MySqlParameter("@bez", MySql.Data.MySqlClient.MySqlDbType.VarChar);
                par_bez.Value = txtBezeichnung.Text;
                comm.Parameters.Add(par_bez);
                MySql.Data.MySqlClient.MySqlParameter par_betr = new MySql.Data.MySqlClient.MySqlParameter("@betr", MySql.Data.MySqlClient.MySqlDbType.Double);
                par_betr.Value = Convert.ToDouble(numBetrag.Text);
                comm.Parameters.Add(par_betr);
            }

            comm.CommandText = sql;
            //MessageBox.Show(sql);
            Update?.Invoke(comm);
        }

        private void chkArtikel_Checked(object sender, RoutedEventArgs e)
        {
            btnSpeichern.IsEnabled = true;
            Enable_Fields(stckArtikel);
        }

        private void chkArtikel_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!(chkRechnung.IsChecked == true)) btnSpeichern.IsEnabled = false;
            Disable_Fields(stckArtikel);
        }
    }
}
