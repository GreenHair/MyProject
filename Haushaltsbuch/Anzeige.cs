﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Haushaltsbuch
{
    class Anzeige
    {
        public ScrollViewer scrlAnzeige = new ScrollViewer();
        protected StackPanel _dashboard = new StackPanel();
        protected StackPanel uebersicht;
        TextBox textBox = new TextBox
        {
            Width = 450,
            Height = 300,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Margin = new Thickness(0, 20, 0, 0)
        };
        protected List<Label> balken = new List<Label>();
        protected List<Label> betrag = new List<Label>();
        protected AnimationClock Clock = null;

        public Anzeige(List<Rechnung> kassenzettel)
        {
            _dashboard.Children.Add(uebersichtErstellen(kassenzettel));
            
            _dashboard.Children.Add(nachKategorien(kassenzettel));
            _dashboard.Children.Add(nachLaeden(kassenzettel));
            _dashboard.Children.Add(datailUebersicht(kassenzettel));
            scrlAnzeige.Content = _dashboard;
        }

        public StackPanel Dashboard
        {
            get
            {
                return _dashboard;
            }

            set
            {
                _dashboard = value;
            }
        }
        
        private StackPanel uebersichtErstellen(List<Rechnung> kassenzettel)
        {
            double alles = (from betrag in kassenzettel where betrag.einmalig == true select (from b in betrag.items select b.betrag).Sum()).Sum();
            double essen = (from betrag in kassenzettel where betrag.einmalig == true select (from b in betrag.items where b.kategorie.essbar == true select b.betrag).Sum()).Sum();
            double sonstige = (from betrag in kassenzettel where betrag.einmalig == true select (from b in betrag.items where b.kategorie.essbar == false select b.betrag).Sum()).Sum();
            uebersicht = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center, MinHeight = 250 };
            StackPanel gesamt = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(10) };
            Label betragAlles = new Label { Content = alles.ToString("C") };
            betrag.Add(betragAlles);
            Label balkenAlles = new Label { Background = Brushes.Green, Height = alles };
            balken.Add(balkenAlles);
            Label nameAlles = new Label { Content = "Gesamt" };
            gesamt.Children.Add(betragAlles); gesamt.Children.Add(balkenAlles); gesamt.Children.Add(nameAlles);
            uebersicht.Children.Add(gesamt);
            StackPanel food = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(10) };
            Label betragEssen = new Label { Content = essen.ToString("C") };
            betrag.Add(betragEssen);
            Label balkenEssen = new Label { Background = Brushes.Green, Height = essen };
            balken.Add(balkenEssen);
            Label nameEssen = new Label { Content = "Lebensmittel" };
            food.Children.Add(betragEssen); food.Children.Add(balkenEssen); food.Children.Add(nameEssen);
            uebersicht.Children.Add(food);
            StackPanel Sonstige = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom, Margin = new Thickness(10) };
            Label betragSonstige = new Label { Content = sonstige.ToString("C") };
            betrag.Add(betragSonstige);
            Label balkenSonstige = new Label { Background = Brushes.Green, Height = sonstige };
            balken.Add(balkenSonstige);
            Label nameSonstige = new Label { Content = "Sonstige" };
            Sonstige.Children.Add(betragSonstige); Sonstige.Children.Add(balkenSonstige); Sonstige.Children.Add(nameSonstige);
            uebersicht.Children.Add(Sonstige);
            return uebersicht;
        }

        private GroupBox nachKategorien(List<Rechnung> kassenzettel)
        {
            List<Posten> ausgaben = new List<Posten>();
            StackPanel diagramm = new StackPanel();
            GroupBox gruppe = erstelleGroubbox("Sortiert nach Kategorien");

            foreach (var item in kassenzettel)
            {
                foreach (Posten p in item.items)
                {
                    ausgaben.Add(p);
                }
            }
            var kategorien = (from kategorie in ausgaben group kategorie by kategorie.kategorie.bezeichnung into cat
                              select new { name = cat.Key, amount = (from c in cat select c.betrag).Sum() }).OrderByDescending(kar => kar.amount);
            
            foreach(var element in kategorien)
            {                
                StackPanel zeile = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(20,5,0,0) };
                zeile.Children.Add(new Label { Content = element.name, Width = 150 });
                zeile.Children.Add(new Label { Background = Brushes.Goldenrod, Width = element.amount, MaxWidth = 400 });
                zeile.Children.Add(new Label { Content = element.amount.ToString("C") });
                diagramm.Children.Add(zeile);
            }
            gruppe.Content = diagramm;
            return gruppe;
        }

        private GroupBox nachLaeden(List<Rechnung> kassenzettel)
        {
            StackPanel diagramm = new StackPanel();
            GroupBox gruppe = erstelleGroubbox("Sortiert nach Läden"); 

            var laeden = (from laden in kassenzettel
                          group laden by laden.laden.name into shops
                          select new { name = shops.Key, amount = (from pos in shops select ((from p in pos.items select p.betrag).Sum())).Sum() }).OrderByDescending(eur => eur.amount);

            foreach(var element in laeden)
            {
                StackPanel zeile = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(20, 5, 0, 0) };
                zeile.Children.Add(new Label { Content = element.name, Width = 150 });
                zeile.Children.Add(new Label { Background = Brushes.Olive, Width = element.amount, MaxWidth = 400 });
                zeile.Children.Add(new Label { Content = element.amount.ToString("C") });
                diagramm.Children.Add(zeile);
            }
            gruppe.Content = diagramm;
            return gruppe;
        }

        private GroupBox datailUebersicht(List<Rechnung> kassenzettel)
        {
            GroupBox gruppe = erstelleGroubbox("Details");
            TreeView tree = new TreeView();
            var sortiert = from ausgabe in kassenzettel group ausgabe by ausgabe.datum into tage orderby tage.Key select tage;

            foreach(var day in sortiert)
            {
                TreeViewItem tItem = new TreeViewItem();
                double sumDay = 0.0;// day.Sum(b => b.items.Sum(a => a.betrag));
                foreach(Rechnung rech in day)
                {
                    double sum = 0.0;
                    TreeViewItem rechnung = new TreeViewItem();
                    foreach(Posten pos in rech.items)
                    {
                        sum += pos.betrag;
                        string wasWieviel = String.Format("{0,-30}\t{1,8:C}", pos.bezeichnung, pos.betrag);
                        TextBlock lbl = new TextBlock { Text = wasWieviel };
                        rechnung.Items.Add(lbl);
                    }
                    rechnung.Header = rech.laden.name + "\t" + sum.ToString("C");
                    sumDay += sum;                    
                    tItem.Items.Add(rechnung);
                }
                string wannWieviel = string.Format("{0,-30}{1,18:C}", day.Key.ToLongDateString(), sumDay);
                tItem.Header = wannWieviel;
                tree.Items.Add(tItem);
            }
            gruppe.Content = tree;
            return gruppe;
        }

        private GroupBox erstelleGroubbox(string inhalt)
        {
            TextBlock kopfzeile = new TextBlock { FontWeight = FontWeights.Bold, FontSize = 20, Text = inhalt };
            GroupBox gruppe = new GroupBox { Header = kopfzeile, Background = Brushes.AntiqueWhite, Margin = new Thickness(0, 20, 0, 0) };
            return gruppe;
        }

        public void diagrammAnimiert()
        {
            DoubleAnimation wachsen = new DoubleAnimation
            {
                From = 0,
                Duration = TimeSpan.Parse("0:0:2"),
                BeginTime = TimeSpan.Parse("0:0:0")
            };

            for(int i = 0; i < balken.Count; i++)
            {
                wachsen.To = balken[i].Height;
                balken[i].Tag = i;
                Clock = wachsen.CreateClock();
                Clock.CurrentTimeInvalidated += Clock_CurrentTimeInvalidated;
                balken[i].ApplyAnimationClock(Label.HeightProperty, Clock);
               // balken[i].BeginAnimation(Label.HeightProperty, wachsen);
            }
        }

        private void Clock_CurrentTimeInvalidated(object sender, EventArgs e)
        {
            double value = Convert.ToDouble( balken[0].GetValue(Label.HeightProperty));
            betrag[0].Content = value.ToString("C");
            double value1 = Convert.ToDouble(balken[1].GetValue(Label.HeightProperty));
            betrag[1].Content = value1.ToString("C");
            double value2 = Convert.ToDouble(balken[2].GetValue(Label.HeightProperty));
            betrag[2].Content = value2.ToString("C");
        }
    }
}
