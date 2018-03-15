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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Haushaltsbuch
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Hauptbuch myHaushaltsbuch;// = new Hauptbuch();
        WarteBalken progressbar;
        Anzeige this_week;
        Anzeige last_week;
        MonatsAnzeige this_month;
        MonatsAnzeige last_month;
        Eintrag eintrag;

        public MainWindow()
        {
            InitializeComponent();
            progressbar = new WarteBalken();
            progressbar.Show();

            //myHaushaltsbuch = new Hauptbuch();
            //this_week = new Anzeige(myHaushaltsbuch.GetRechnung_W());
            //last_week = new Anzeige(myHaushaltsbuch.GetRechnung_W(1));
            //double ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month select pos.Betrag).Sum();
            //this_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(), ein);
            //ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month - 1 select pos.Betrag).Sum();
            //last_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(1), ein);
            //thisWeek.Content = this_week.scrlAnzeige;
            //lastWeek.Content = last_week.scrlAnzeige;
            //thisMonth.Content = this_month.scrlAnzeige;
            //lastMonth.Content = last_month.scrlAnzeige;

        }

        private void Uebersicht_Click(object sender, RoutedEventArgs e)
        {
            uebersicht.Visibility = Visibility.Visible;//scrbar
            grdEinkommen.Visibility = Visibility.Collapsed;
            tbcEintrag.Visibility = Visibility.Collapsed;
            tabItem_GotFocus(uebersicht.SelectedItem, e);
        }

        private void Eintrag_Click(object sender, RoutedEventArgs e)
        {
            tbcEintrag.Visibility = Visibility.Visible;
            uebersicht.Visibility = Visibility.Collapsed;//scrbar
            grdEinkommen.Visibility = Visibility.Collapsed;            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            myHaushaltsbuch = new Hauptbuch();
            this_week = new Anzeige(myHaushaltsbuch.GetRechnung_W());
            last_week = new Anzeige(myHaushaltsbuch.GetRechnung_W(1));
            double ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month select pos.Betrag).Sum();
            this_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(), ein);
            ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month - 1 select pos.Betrag).Sum();
            last_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(1), ein);
            thisWeek.Content = this_week.scrlAnzeige;
            lastWeek.Content = last_week.scrlAnzeige;
            thisMonth.Content = this_month.scrlAnzeige;
            lastMonth.Content = last_month.scrlAnzeige;

            thisWeek.Loaded += ThisWeek_Loaded;

            var AktMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month select einnahmen;
            UebersichtEinkommen("Einkommen aktueller Monat", stckEinkommen, AktMonEin);

            var PrevMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month -1 select einnahmen;
            UebersichtEinkommen("Einkommen vorherigen Monat", stckEinkommenPrev, PrevMonEin);
            // Eintrag eintrag = new Eintrag(myHaushaltsbuch.AlleLaeden, myHaushaltsbuch.Kategorien);
            eintrag = new Eintrag(myHaushaltsbuch);
            tbiRechnung.Content = eintrag.NeuerRechnung(myHaushaltsbuch.familienmitglied);
            tbiShop.Content = eintrag.NeuerLaden();
            tbiProdgr.Content = eintrag.NeuerKategorie();
            eintrag.Insert += Eintrag_Insert;
            eintrag.InsertLaden += Eintrag_InsertLaden;
            eintrag.InsertKategorie += Eintrag_InsertKategorie;

            progressbar.Close();

            //this_week.diagrammAnimiert();
            
        }

        private void ThisWeek_Loaded(object sender, RoutedEventArgs e)
        {
            this_week.diagrammAnimiert();
        }

        private int Eintrag_InsertKategorie(MySqlCommand mysqlcommand)
        {
            int result = myHaushaltsbuch.Eintragen(mysqlcommand);
            if (result > 0)
            {
                myHaushaltsbuch.Kategorien = myHaushaltsbuch.GetKategorien();
                tbiRechnung.Content = eintrag.NeuerRechnung(myHaushaltsbuch.familienmitglied);
            }
            return result;
        }

        private int Eintrag_InsertLaden(MySqlCommand mysqlcommand)
        {
            int result = myHaushaltsbuch.Eintragen(mysqlcommand);
            if (result > 0)
            {
                myHaushaltsbuch.AlleLaeden = myHaushaltsbuch.GetLaeden();
                tbiRechnung.Content = eintrag.NeuerRechnung(myHaushaltsbuch.familienmitglied);
            }
            return result;
        }

        private int Eintrag_Insert(MySqlCommand mysqlcommand)
        {
            progressbar.ShowDialog();
            int result = myHaushaltsbuch.Eintragen(mysqlcommand);
            if (result > 0)
            {
                // TODO check refresh content,eventuell in function auslagern
                tbiRechnung.Content = eintrag.NeuerRechnung(myHaushaltsbuch.familienmitglied);
                this_week = new Anzeige(myHaushaltsbuch.GetRechnung_W());
                last_week = new Anzeige(myHaushaltsbuch.GetRechnung_W(1));
                double ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month select pos.Betrag).Sum();
                this_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(), ein);
                ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month - 1 select pos.Betrag).Sum();
                last_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(1), ein);
                thisWeek.Content = this_week.scrlAnzeige;
                lastWeek.Content = last_week.scrlAnzeige;
                thisMonth.Content = this_month.scrlAnzeige;
                lastMonth.Content = last_month.scrlAnzeige;
            }
            progressbar.Close();
            return result;
        }

        private void tabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            switch (((TabItem)sender).Name)
            {
                case "thisWeek": this_week.scrlAnzeige.ScrollToTop(); this_week.diagrammAnimiert(); break;
                case "lastWeek": last_week.scrlAnzeige.ScrollToTop(); last_week.diagrammAnimiert(); break;
                case "lastMonth": last_month.scrlAnzeige.ScrollToTop(); last_month.diagrammAnimiert(); break;
                case "thisMonth": this_month.scrlAnzeige.ScrollToTop(); this_month.diagrammAnimiert(); break;
            }
            
        }

        private void Einkommen_Click(object sender, RoutedEventArgs e)
        {
            grdEinkommen.Visibility = Visibility.Visible;
            uebersicht.Visibility = Visibility.Collapsed; //scrbar
            tbcEintrag.Visibility = Visibility.Collapsed;
        }

        private void Suchen_CLick(object sender, RoutedEventArgs e)
        {

        }

        private static void UebersichtEinkommen(string wann, StackPanel stckEinkommen, IEnumerable<Einkommen> einkommen)
        {
            stckEinkommen.Children.Add(new Label
            {
                Content = wann,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 16,
                FontWeight = FontWeights.Black
            });
            Grid Tabelle = new Grid { HorizontalAlignment = HorizontalAlignment.Center };
            Tabelle.ColumnDefinitions.Add(new ColumnDefinition());
            Tabelle.ColumnDefinitions.Add(new ColumnDefinition());

            int i = 0;
            foreach(Einkommen ein in einkommen)
            {
                Tabelle.RowDefinitions.Add(new RowDefinition());
                TextBox was = new TextBox { Text = ein.Bezeichnung, IsReadOnly = true, Width = 150 };
                Grid.SetColumn(was, 0);
                Grid.SetRow(was, i);
                Tabelle.Children.Add(was);
                TextBox wieviel = new TextBox
                {
                    Text = ein.Betrag.ToString("C"),
                    IsReadOnly = true,
                    Width = 150,
                    HorizontalContentAlignment = HorizontalAlignment.Right
                }; 
                Grid.SetColumn(wieviel, 1);
                Grid.SetRow(wieviel, i);
                Tabelle.Children.Add(wieviel);
                i++;
            }
            Tabelle.RowDefinitions.Add(new RowDefinition());
            TextBox gesamt = new TextBox
            {
                Text = "Gesamt",
                Width = 150,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 2, 0, 0)
            };
            Grid.SetRow(gesamt, i);
            Tabelle.Children.Add(gesamt);
            TextBox gesamtSumme = new TextBox
            {
                Text = einkommen.Sum(e => e.Betrag).ToString("C"),
                HorizontalContentAlignment = HorizontalAlignment.Right,
                Width = 150,
                FontWeight = FontWeights.Bold,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 2, 0, 0)
            };
            Grid.SetRow(gesamtSumme, i);
            Grid.SetColumn(gesamtSumme, 1);
            Tabelle.Children.Add(gesamtSumme);
            stckEinkommen.Children.Add(Tabelle);
        }

        //private void uebersicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // MessageBox.Show("TabItem gewwechselt zu " + uebersicht.SelectedIndex);
            
        //    switch (((TabItem)(uebersicht.SelectedItem)).Name)
        //    {
        //        case "lastWeek": scrlbar.ScrollToTop(); last_week.diagrammAnimiert(); break;
        //        case "thisWeek": scrlbar.ScrollToTop(); this_week.diagrammAnimiert();  break;
        //        case "lastMonth": last_month.diagrammAnimiert(); break;
        //        case "thisMonth": this_month.diagrammAnimiert(); break;
        //    }
        //}
    }
}
