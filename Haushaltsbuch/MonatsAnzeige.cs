using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Haushaltsbuch
{
    class MonatsAnzeige : Anzeige
    {
        double _einnahmen;

        public MonatsAnzeige(List<Rechnung> kassenzettel, double einnahmen) : base(kassenzettel)
        {
            _einnahmen = einnahmen;
            uebersicht.Children.Add(bilanz(kassenzettel));
            foreach (Label lab in balken)
            {
                lab.Height = lab.Height / 10;
            }
        }


        private StackPanel bilanz(List<Rechnung> kassenzettel)
        {
            double ausgaben = (from posten in kassenzettel select (from pos in posten.items select pos.betrag).Sum()).Sum();
            StackPanel stkBilanz = new StackPanel { Margin = new System.Windows.Thickness(5) };
            Grid grdBilanz = new Grid();
            grdBilanz.ColumnDefinitions.Add(new ColumnDefinition());
            grdBilanz.ColumnDefinitions.Add(new ColumnDefinition());
            grdBilanz.RowDefinitions.Add(new RowDefinition());
            grdBilanz.RowDefinitions.Add(new RowDefinition());
            grdBilanz.RowDefinitions.Add(new RowDefinition());
            Label lblSumAus = new Label { Content = "Summe alle Ausgaben:" };
            Grid.SetColumn(lblSumAus, 0);
            Grid.SetRow(lblSumAus, 0);
            grdBilanz.Children.Add(lblSumAus);

            Label lblSumEin = new Label { Content = "Summe alle Einnahmen:" };
            Grid.SetColumn(lblSumEin, 0);
            Grid.SetRow(lblSumEin, 1);
            grdBilanz.Children.Add(lblSumEin);

            Label lblBilanz = new Label { Content = "Bilanz:" };
            Grid.SetColumn(lblBilanz, 0);
            Grid.SetRow(lblBilanz, 2);
            grdBilanz.Children.Add(lblBilanz);

            Label lblSumAus1 = new Label { Content = ausgaben.ToString("C") };
            Grid.SetColumn(lblSumAus1, 1);
            Grid.SetRow(lblSumAus1, 0);
            grdBilanz.Children.Add(lblSumAus1);

            Label lblSumEin1 = new Label { Content = _einnahmen.ToString("C") };
            Grid.SetColumn(lblSumEin1, 1);
            Grid.SetRow(lblSumEin1, 1);
            grdBilanz.Children.Add(lblSumEin1);

            Label lblBilanz1 = new Label { Content = (_einnahmen-ausgaben).ToString("C") };
            Grid.SetColumn(lblBilanz1, 1);
            Grid.SetRow(lblBilanz1, 2);
            grdBilanz.Children.Add(lblBilanz1);
            stkBilanz.Children.Add(grdBilanz);

            return stkBilanz;
        }

    }
}
