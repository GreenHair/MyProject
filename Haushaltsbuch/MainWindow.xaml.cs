using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            myHaushaltsbuch = new Hauptbuch();
            
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
            grdSuchen.Visibility = Visibility.Collapsed;
            tabItem_GotFocus(uebersicht.SelectedItem, e);
        }

        private void Eintrag_Click(object sender, RoutedEventArgs e)
        {
            tbcEintrag.Visibility = Visibility.Visible;
            uebersicht.Visibility = Visibility.Collapsed;//scrbar
            grdEinkommen.Visibility = Visibility.Collapsed;
            grdSuchen.Visibility = Visibility.Collapsed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //progressbar.Show();
            //myHaushaltsbuch = new Hauptbuch();
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
            
            var AktMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month select einnahmen;
            UebersichtEinkommen("Einkommen aktueller Monat", stckEinkommen, AktMonEin);

            var PrevMonEin = from einnahmen in myHaushaltsbuch.einnahmen where einnahmen.Datum.Month == DateTime.Now.Month -1 select einnahmen;
           // UebersichtEinkommen("Einkommen vorherigen Monat", stckEinkommenPrev, PrevMonEin);
            // Eintrag eintrag = new Eintrag(myHaushaltsbuch.AlleLaeden, myHaushaltsbuch.Kategorien);
            eintrag = new Eintrag(myHaushaltsbuch);
            tbiRechnung.Content = eintrag.NeuerRechnung(myHaushaltsbuch.familienmitglied);
            tbiShop.Content = eintrag.NeuerLaden();
            tbiProdgr.Content = eintrag.NeuerKategorie();
            tbiEinkommen.Content = eintrag.NeuesEinkommen();
            eintrag.Insert += Eintrag_Insert;
            eintrag.InsertLaden += Eintrag_InsertLaden;
            eintrag.InsertKategorie += Eintrag_InsertKategorie;
            eintrag.InsertEinkommen += Eintrag_InsertEinkommen;

            cmbKategorie.ItemsSource = myHaushaltsbuch.Kategorien;
            cmbLaden.ItemsSource = myHaushaltsbuch.AlleLaeden;


            progressbar.Close();

            this_week.diagrammAnimiert();
            
        }

        private int Eintrag_InsertEinkommen(MySqlCommand mysqlcommand)
        {
            int result = myHaushaltsbuch.Eintragen(mysqlcommand);
            if(result > 0)
            {

            }
            return result;
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
            progressbar = new WarteBalken();
            progressbar.Show();
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
            grdSuchen.Visibility = Visibility.Collapsed;
        }

        private void Suchen_CLick(object sender, RoutedEventArgs e)
        {
            grdSuchen.Visibility = Visibility.Visible;
            tbcEintrag.Visibility = Visibility.Collapsed;
            uebersicht.Visibility = Visibility.Collapsed;//scrbar
            grdEinkommen.Visibility = Visibility.Collapsed;

            Suchergebnis s = new Suchergebnis();
            s.Kassenzettel = new Rechnung { datum = DateTime.Now, einmalig = true };
            s.Artikel = new Posten { bezeichnung = "Milch", betrag = 0.68 };
            lstSuchResultat.Items.Add(s);
        }

        private void UebersichtEinkommen(string wann, StackPanel stckEinkommen, IEnumerable<Einkommen> einkommen)
        {            
            var sortiert = from eingang in myHaushaltsbuch.einnahmen group eingang by eingang.Datum.Month into promonat orderby promonat.First().Datum descending select promonat;

            foreach(var monat in sortiert)
            {
                stckEinkommen.Children.Add(new Label { Content = string.Format("{0:MMMM}", monat.First().Datum), FontWeight = FontWeights.Bold });
                ListView tabelle = new ListView { BorderThickness = new Thickness(0, 0, 0, 2), BorderBrush = Brushes.Black };
                GridView grid = new GridView();
                grid.Columns.Add(new GridViewColumn { Header = "Bezeichnung", Width = 150, DisplayMemberBinding = new Binding { Path = new PropertyPath("Bezeichnung") } });
                grid.Columns.Add(new GridViewColumn { Header = "Betrag", Width = 100, DisplayMemberBinding = new Binding { Path = new PropertyPath("Betrag"), StringFormat = "{0:C}", ConverterCulture = new System.Globalization.CultureInfo("de-DE") } });
                tabelle.View = grid;
                tabelle.ItemsSource = monat;
                stckEinkommen.Children.Add(tabelle);
                StackPanel zeile = new StackPanel { Orientation = Orientation.Horizontal };
                zeile.Children.Add(new Label { Content = "Summe",Width = 150 });
                zeile.Children.Add(new Label { Content = string.Format("{0:C}", monat.Sum(b => b.Betrag)), Width = 100 });
                stckEinkommen.Children.Add(zeile);
            }

            //stckEinkommen.Children.Add(new Label
            //{
            //    Content = wann,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    FontSize = 16,
            //    FontWeight = FontWeights.Black
            //});
            //Grid Tabelle = new Grid { HorizontalAlignment = HorizontalAlignment.Center };
            //Tabelle.ColumnDefinitions.Add(new ColumnDefinition());
            //Tabelle.ColumnDefinitions.Add(new ColumnDefinition());

            //int i = 0;
            //foreach(Einkommen ein in einkommen)
            //{
            //    Tabelle.RowDefinitions.Add(new RowDefinition());
            //    TextBox was = new TextBox { Text = ein.Bezeichnung, IsReadOnly = true, Width = 150 };
            //    Grid.SetColumn(was, 0);
            //    Grid.SetRow(was, i);
            //    Tabelle.Children.Add(was);
            //    TextBox wieviel = new TextBox
            //    {
            //        Text = ein.Betrag.ToString("C"),
            //        IsReadOnly = true,
            //        Width = 150,
            //        HorizontalContentAlignment = HorizontalAlignment.Right
            //    }; 
            //    Grid.SetColumn(wieviel, 1);
            //    Grid.SetRow(wieviel, i);
            //    Tabelle.Children.Add(wieviel);
            //    i++;
            //}
            //Tabelle.RowDefinitions.Add(new RowDefinition());
            //TextBox gesamt = new TextBox
            //{
            //    Text = "Gesamt",
            //    Width = 150,
            //    FontWeight = FontWeights.Bold,
            //    BorderBrush = Brushes.Black,
            //    BorderThickness = new Thickness(0, 2, 0, 0)
            //};
            //Grid.SetRow(gesamt, i);
            //Tabelle.Children.Add(gesamt);
            //TextBox gesamtSumme = new TextBox
            //{
            //    Text = einkommen.Sum(e => e.Betrag).ToString("C"),
            //    HorizontalContentAlignment = HorizontalAlignment.Right,
            //    Width = 150,
            //    FontWeight = FontWeights.Bold,
            //    BorderBrush = Brushes.Black,
            //    BorderThickness = new Thickness(0, 2, 0, 0)
            //};
            //Grid.SetRow(gesamtSumme, i);
            //Grid.SetColumn(gesamtSumme, 1);
            //Tabelle.Children.Add(gesamtSumme);
            //stckEinkommen.Children.Add(Tabelle);
        }
        
        private void Verbinden_Click(object sender, RoutedEventArgs e)
        {
            VerbindenFenster window = new VerbindenFenster();
            window.Verbinden += Window_Verbinden;
            window.ShowDialog();
        }

        private void Window_Verbinden(string connectstring)
        {
            string result = myHaushaltsbuch.Verbinden(connectstring);
            if(result == "Verbunden") { Window_Loaded(new object(),new RoutedEventArgs()); }
            else { MessageBox.Show(result); }
        }

        private void btnSuchenStarten_Click(object sender, RoutedEventArgs e)
        {
            string search = " select rechnung.id as r_id, laden,datum,einmalig,person,ausgaben.ID as a_id,bezeichnung,betrag,prod_gr from ausgaben join rechnung on rechnung.id = ausgaben.rechnungsnr";
            bool hatVorgaenger = false;
            MySqlCommand command = new MySqlCommand();

            if (txtBezeichnung.Text.Length != 0 || numPreis.Betrag != 0 || cmbKategorie.SelectedItem != null || cmbLaden.SelectedItem != null || dpDatum.SelectedDate != null)
            {
                search += " where ";
                if(txtBezeichnung.Text.Length != 0)
                {
                    search += "bezeichnung = @bez";
                    MySqlParameter bez_par = new MySqlParameter("@bez", MySqlDbType.VarChar);
                    bez_par.Value = txtBezeichnung.Text;
                    command.Parameters.Add(bez_par);
                    hatVorgaenger = true;
                }
                if(numPreis.Betrag != 0)
                {
                    if(hatVorgaenger)
                    {
                        search += " and ";
                    }
                    search += "betrag ";
                    if (rHigher.IsChecked == true) search += ">= ";
                    if (rLower.IsChecked == true) search += "<= ";
                    if (rEquals.IsChecked == true) search += "= ";
                    search += "@betr";
                    MySqlParameter par_betr = new MySqlParameter("@betr", MySqlDbType.Double);
                    par_betr.Value = Convert.ToDouble(numPreis.Text);
                    command.Parameters.Add(par_betr);
                    hatVorgaenger = true;
                }
                if(cmbKategorie.SelectedItem != null)
                {
                    if(hatVorgaenger)
                    { 
                        search += " and ";
                    }
                    search += "prod_gr = " + ((Produktgruppe)cmbKategorie.SelectedItem).id;
                    hatVorgaenger = true;
                }
                if (cmbLaden.SelectedItem != null)
                {
                    if (hatVorgaenger)
                    {
                        search += " and ";
                    }
                    search += "laden = " + ((Shop)cmbLaden.SelectedItem).id;
                    hatVorgaenger = true;
                }
                if(dpDatum.SelectedDate != null)
                {
                    if (hatVorgaenger)
                    {
                        search += " and ";
                    }
                    if(rVor.IsChecked == true) search += "datum < '" + string.Format("{0:yyyy-MM-dd}", dpDatum.SelectedDate) + "'";
                    if(rAm.IsChecked == true)  search += "datum = '" + string.Format("{0:yyyy-MM-dd}", dpDatum.SelectedDate) + "'";
                    if(rNach.IsChecked == true) search += "datum > '" + string.Format("{0:yyyy-MM-dd}", dpDatum.SelectedDate) + "'";
                    if(rZwischen.IsChecked == true) search += "datum between '" + string.Format("{0:yyyy-MM-dd}", dpDatum.SelectedDate) + "' and '" + string.Format("{0:yyyy-MM-dd}",dpDatum2.SelectedDate) + "'";
                    hatVorgaenger = true;
                }
                if(rBeide.IsChecked == false)
                {
                    if (hatVorgaenger)
                    {
                        search += " and ";
                    }
                    if (rEinmal.IsChecked == true) search += "einmalig = 1";
                    if (rFest.IsChecked == true) search += "einmalig = 0";
                }

            }
           // MessageBox.Show(search);
            command.CommandText = search;
            List<Suchergebnis> Gefunden = myHaushaltsbuch.Suchen(command);
            if (Gefunden.Count == 0)
            {
                MessageBox.Show("Leider nichts gefunden");
            }
            else
            {
                lstSuchResultat.ItemsSource = Gefunden;

                txtAnz.Text = "Anzahl gefundene Ausgaben: " + lstSuchResultat.Items.Count;
                lblSumme.Content = "Summe: " + string.Format("{0:C}", (from ausgabe in Gefunden select ausgabe.Artikel.betrag).Sum());
                lblAvg.Content = "Durchschnittspreis: " + string.Format("{0:C}", (from ausgabe in Gefunden select ausgabe.Artikel.betrag).Average());
            }
        }

        private void bearbeiten_Click(object sender, RoutedEventArgs e)
        {
            BearbeitenFenster Window = new BearbeitenFenster((Suchergebnis)lstSuchResultat.SelectedItem);
            Window.Update += Window_Update;
            Window.Show();
        }

        private void Window_Update(MySqlCommand command)
        {
            int result = myHaushaltsbuch.Eintragen(command);
            if (result > 0) MessageBox.Show("Daten erfolgreich geändert");
        }

        //private void RefreshContent()
        //{
        //    //myHaushaltsbuch.AlleLaeden = myHaushaltsbuch.GetLaeden();
        //    //myHaushaltsbuch.Kategorien = myHaushaltsbuch.GetKategorien();
        //    //myHaushaltsbuch.familienmitglied = myHaushaltsbuch.GetFamilie();
        //    //myHaushaltsbuch.einnahmen = myHaushaltsbuch.GetEinkommen();

        //    this_week = new Anzeige(myHaushaltsbuch.GetRechnung_W());
        //    last_week = new Anzeige(myHaushaltsbuch.GetRechnung_W(1));
        //    double ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month select pos.Betrag).Sum();
        //    this_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(), ein);
        //    ein = (from pos in myHaushaltsbuch.einnahmen where pos.Datum.Month == DateTime.Now.Month - 1 select pos.Betrag).Sum();
        //    last_month = new MonatsAnzeige(myHaushaltsbuch.GetRechnung_M(1), ein);
        //    thisWeek.Content = this_week.scrlAnzeige;
        //    lastWeek.Content = last_week.scrlAnzeige;
        //    thisMonth.Content = this_month.scrlAnzeige;
        //    lastMonth.Content = last_month.scrlAnzeige;
        //}

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
