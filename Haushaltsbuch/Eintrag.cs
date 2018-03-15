using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NummerfeldTest;
using MySql.Data.MySqlClient;

namespace Haushaltsbuch
{
    public delegate int DelInsert(MySqlCommand mysqlcommand);
    class Eintrag
    {
        public event DelInsert Insert;
        private StackPanel _zeileRechnung;
        private StackPanel _rechnungsPosten;
        public StackPanel stckRechnung { get; private set; }
        private StackPanel stckZeileLaden;
        private StackPanel stckZeileKategorie;
        private List<Shop> Laeden;
        private List<Produktgruppe> Prodgr;
        private Hauptbuch _haushaltsbuch;

       // public Eintrag(List<Shop> _laeden, List<Produktgruppe> _prodgr)
        public Eintrag(Hauptbuch haushalt)
        {
            //Laeden = _laeden;
            //Prodgr = _prodgr;
            Laeden = haushalt.AlleLaeden;
            Prodgr = haushalt.Kategorien;
            _haushaltsbuch = haushalt;
        }

        public ScrollViewer NeuerRechnung(List<Person> Familie)
        {
            ScrollViewer viewer = new ScrollViewer();
            _rechnungsPosten = new StackPanel();
            stckRechnung = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            Grid Zeile1 = new Grid();
            Zeile1.ColumnDefinitions.Add(new ColumnDefinition());
            Zeile1.ColumnDefinitions.Add(new ColumnDefinition());
            Label Titel = new Label
            {
                Content = "Neuer Rechnung",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold
            };
            Zeile1.Children.Add(Titel);
            Button btnRechnung = new Button { HorizontalAlignment = HorizontalAlignment.Right, Content = "OK", Width = 100, Height = 25 };
            btnRechnung.Click += BtnRechnung_Click;
            btnRechnung.IsEnabled = (Laeden.Count > 0) ? true : false;
            Grid.SetColumn(btnRechnung, 1);
            Zeile1.Children.Add(btnRechnung);
            stckRechnung.Children.Add(Zeile1);
            _zeileRechnung = new StackPanel { Orientation = Orientation.Horizontal };
            _zeileRechnung.Children.Add(new DatePicker { SelectedDate = DateTime.Now.Date });
            _zeileRechnung.Children.Add(new ComboBox { ItemsSource = Laeden,Width = 100, Text = "Laden" });
            _zeileRechnung.Children.Add(new ComboBox { ItemsSource = Familie,Width = 100, Text = "Person" });
            _zeileRechnung.Children.Add(new CheckBox { Content = "Einmalig", IsChecked = true, VerticalAlignment = VerticalAlignment.Center });
            stckRechnung.Children.Add(_zeileRechnung);
            stckRechnung.Children.Add(new Label
            {
                Content = "Rechnungsposten",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold
            });
            for (int i = 0; i < 30; i++)
            {                
                StackPanel ZeilePosten = new StackPanel { Orientation = Orientation.Horizontal };
                ZeilePosten.Children.Add(new TextBox { Text = "", Width = 200 });
                ZeilePosten.Children.Add(new NumberInput { Width = 100, HorizontalContentAlignment = HorizontalAlignment.Right });
               // ZeilePosten.Children.Add(new TextBox { Text = "", Width = 100, HorizontalContentAlignment = HorizontalAlignment.Right });
                ZeilePosten.Children.Add(new Label { Content = "€" });
                ZeilePosten.Children.Add(new ComboBox { ItemsSource = Prodgr,Width = 100, Text = "Produktgruppe" });
                _rechnungsPosten.Children.Add(ZeilePosten);
            }
            stckRechnung.Children.Add(_rechnungsPosten);
            viewer.Content = stckRechnung;            
            return viewer;
        }

        private void BtnRechnung_Click(object sender, RoutedEventArgs e)
        {
            DatePicker datum = _zeileRechnung.Children[0] as DatePicker;
            ComboBox laden = _zeileRechnung.Children[1] as ComboBox;
            ComboBox person = _zeileRechnung.Children[2] as ComboBox;
            CheckBox einmalig = _zeileRechnung.Children[3] as CheckBox;
            DateTime date = Convert.ToDateTime(datum.SelectedDate);

            CultureInfo ci = new CultureInfo("DE-de");
            NumberFormatInfo ni = ci.NumberFormat;
            ni.NumberDecimalSeparator = ".";

            string result = "INSERT INTO `rechnung`(datum,laden,person,einmalig) VALUES ('";
            result += date.ToString("yyyy-MM-dd") + "'," + ((Shop)(laden.SelectedItem)).id + "," 
                + ((Person)(person.SelectedItem)).id + "," + Convert.ToInt32(einmalig.IsChecked) + ");";
            result += "INSERT INTO `ausgaben`(bezeichnung,betrag,prod_gr,rechnungsnr) VALUES ";

            List<TextBox> tb_list = new List<TextBox>();
            MySqlCommand comm = new MySqlCommand();
            int i = 1;

            foreach (var zeile in _rechnungsPosten.Children)
            {
                if(zeile is StackPanel)
                {
                    StackPanel row = zeile as StackPanel;
                    TextBox bez = row.Children[0] as TextBox;
                    NumberInput bet = row.Children[1] as NumberInput;
                    ComboBox kat = row.Children[3] as ComboBox;
                    double betrag;
                    if (bez.Text != "" && bez.Text != null)
                    {
                        betrag = Convert.ToDouble(bet.Text);
                        string b = string.Format(ni, "{0}", betrag);
                        // result += "('" +bez.Text + "'," + b + "," + ((Produktgruppe)kat.SelectedItem).id + ",LAST_INSERT_ID()),";
                        result += "(@bezeichnung"+i+",@betrag"+i+"," + ((Produktgruppe)kat.SelectedItem).id + ",LAST_INSERT_ID()),";
                        MySqlParameter par_bez = new MySqlParameter("@bezeichnung"+i, MySqlDbType.VarChar);
                        par_bez.Value = bez.Text;
                        comm.Parameters.Add(par_bez);
                        MySqlParameter par_bet = new MySqlParameter("@betrag"+i, MySqlDbType.Double, 0);
                        par_bet.Value = betrag;
                        comm.Parameters.Add(par_bet);
                        i++;
                       // tb_list.Add(bez);
                       // tb_list.Add(bet.txtBox);
                    }
                }
            }

            result = result.Remove(result.Length - 1);
            //MessageBox.Show(result);
            comm.CommandText = result;
            
            int? rowsaffected = Insert?.Invoke(comm);
           // if(rowsaffected > 0) Clear(tb_list.ToArray());
        }

        public StackPanel NeuerLaden()
        {
            StackPanel stckLaden = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            stckLaden.Children.Add(new Label
            {
                Content = "Neuer Laden",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold
            });
            stckZeileLaden = new StackPanel { Orientation = Orientation.Horizontal };            
            stckZeileLaden.Children.Add(new TextBox { Width = 200 });
            stckZeileLaden.Children.Add(new CheckBox { Content = "Online",Width = 100 });
            Button btnLaden = new Button { Content = "OK", Width = 100 };
            btnLaden.Click += BtnLaden_Click;
            stckZeileLaden.Children.Add(btnLaden);
            stckLaden.Children.Add(stckZeileLaden);
            return stckLaden;
        }

        private void BtnLaden_Click(object sender, RoutedEventArgs e)
        {
            TextBox laden = stckZeileLaden.Children[0] as TextBox;
            CheckBox online = stckZeileLaden.Children[1] as CheckBox;
            MySqlCommand comm = new MySqlCommand("insert into laden(name,online) values (@name," + Convert.ToInt32(online) + ")");
            MySqlParameter param = new MySqlParameter("@name", MySqlDbType.VarChar,20);
            param.Value = laden.Text;
            comm.Parameters.Add(param);

            MessageBox.Show(Laeden.Count.ToString() + "," + laden.Text + "," + Convert.ToInt32(online));
            Clear(laden);
        }

        public StackPanel NeuerKategorie()
        {
            StackPanel stckKategorie = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            stckKategorie.Children.Add(new Label
            {
                Content = "Neuer Kategorie",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold
            });
            stckZeileKategorie = new StackPanel { Orientation = Orientation.Horizontal };
            stckZeileKategorie.Children.Add(new TextBox { Width = 200 });
            stckZeileKategorie.Children.Add(new CheckBox { Content = "Lebensmittel", Width = 100 });
            Button btnKategorie = new Button { Content = "OK", Width = 100 };
            btnKategorie.Click += BtnKategorie_Click;
            stckZeileKategorie.Children.Add(btnKategorie);
            stckKategorie.Children.Add(stckZeileKategorie);
            return stckKategorie;
        }

        private void BtnKategorie_Click(object sender, RoutedEventArgs e)
        {
            TextBox kategorie = stckZeileLaden.Children[0] as TextBox;
            CheckBox essen = stckZeileLaden.Children[1] as CheckBox;
            // bezeichnung varchar 20
            MessageBox.Show(Prodgr.Count.ToString() + "," + kategorie.Text + "," + Convert.ToInt32(essen));
            Clear(kategorie);
        }

        private void Clear(params TextBox[] text)
        {
            foreach(TextBox t in text)
            {
                t.Text = "";
            }
        }
    }
}
