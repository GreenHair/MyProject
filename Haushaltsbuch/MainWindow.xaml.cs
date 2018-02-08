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
        Anzeige this_week;
        Anzeige last_week;
        MonatsAnzeige this_month;
        MonatsAnzeige last_month;

        public MainWindow()
        {
            InitializeComponent();
            myHaushaltsbuch = new Hauptbuch();
            this_week = new Anzeige(myHaushaltsbuch.dieseWoche);
            last_week = new Anzeige(myHaushaltsbuch.LetzteWoche);
            double ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month select pos.Betrag).Sum();
            this_month = new MonatsAnzeige(myHaushaltsbuch.monatsliste(0), ein);
            ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month-1 select pos.Betrag).Sum();
            last_month = new MonatsAnzeige(myHaushaltsbuch.monatsliste(1), ein);
            thisWeek.Content = this_week.Dashboard;
            lastWeek.Content = last_week.Dashboard;
            thisMonth.Content = this_month.Dashboard;
            lastMonth.Content = last_month.Dashboard;

            
        }

        private void Uebersicht_Click(object sender, RoutedEventArgs e)
        {
            scrlbar.Visibility = Visibility.Visible;
            grdEinkommen.Visibility = Visibility.Collapsed;
            tbcEintrag.Visibility = Visibility.Collapsed;            
        }

        private void Eintrag_Click(object sender, RoutedEventArgs e)
        {
            tbcEintrag.Visibility = Visibility.Visible;
            scrlbar.Visibility = Visibility.Collapsed;
            grdEinkommen.Visibility = Visibility.Collapsed;            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var AktMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month select einnahmen;
            UebersichtEinkommen("Einkommen aktueller Monat", stckEinkommen, AktMonEin);

            var PrevMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month -1 select einnahmen;
            UebersichtEinkommen("Einkommen vorherigen Monat", stckEinkommenPrev, PrevMonEin);

            tbiRechnung.Content = Eintrag.NeuerRechnung(myHaushaltsbuch.)

            this_week.diagrammAnimiert();
            
        }

        private void tabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            //if(thisWeek.IsFocused == false)
                scrlbar.ScrollToTop();
            switch (((TabItem)sender).Name)
            {
                case "thisWeek": this_week.diagrammAnimiert(); break;
                case "lastWeek": last_week.diagrammAnimiert(); break;
                case "lastMonth": last_month.diagrammAnimiert(); break;
                case "thisMonth": this_month.diagrammAnimiert(); break;
            }
            
        }

        private void Einkommen_Click(object sender, RoutedEventArgs e)
        {
            grdEinkommen.Visibility = Visibility.Visible;
            scrlbar.Visibility = Visibility.Collapsed;
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
