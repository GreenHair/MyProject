using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Haushaltsbuch
{
    class Eintrag
    {
        private StackPanel _zeileRechnung;
        private StackPanel _rechnungsPosten;
        public StackPanel stckRechnung { get; private set; }

        public StackPanel NeuerRechnung(List<Shop> Laeden, List<Produktgruppe> Prodgr, List<Person> Familie)
        {
            _rechnungsPosten = new StackPanel();
            stckRechnung = new StackPanel();
            stckRechnung.Children.Add(new Label
            {
                Content = "Neuer Rechnung",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            _zeileRechnung = new StackPanel { Orientation = Orientation.Horizontal };
            _zeileRechnung.Children.Add(new DatePicker { SelectedDate = DateTime.Now.Date });
            _zeileRechnung.Children.Add(new ComboBox { ItemsSource = Laeden });
            _zeileRechnung.Children.Add(new ComboBox { ItemsSource = Familie });
            _zeileRechnung.Children.Add(new CheckBox { Content = "Einmalig", IsChecked = true });
            stckRechnung.Children.Add(_zeileRechnung);
            stckRechnung.Children.Add(new Label
            {
                Content = "Rechnungsposten",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            for (int i = 0; i < 20; i++)
            {
                StackPanel ZeilePosten = new StackPanel { Orientation = Orientation.Horizontal };
                ZeilePosten.Children.Add(new TextBox { Width = 200 });
                ZeilePosten.Children.Add(new TextBox { Width = 100 });
                ZeilePosten.Children.Add(new ComboBox { ItemsSource = Prodgr });
                _rechnungsPosten.Children.Add(ZeilePosten);
            }
            stckRechnung.Children.Add(_rechnungsPosten);
            Button btnRechnung = new Button { HorizontalAlignment = HorizontalAlignment.Right, Content = "OK" };
            btnRechnung.Click += BtnRechnung_Click;
            stckRechnung.Children.Add(btnRechnung);
            return stckRechnung;
        }

        private void BtnRechnung_Click(object sender, RoutedEventArgs e)
        {
            DatePicker datum = _zeileRechnung.Children[0] as DatePicker;
            ComboBox laden = _zeileRechnung.Children[1] as ComboBox;
            ComboBox person = _zeileRechnung.Children[2] as ComboBox;
            CheckBox einmalig = _zeileRechnung.Children[3] as CheckBox;
            MessageBox.Show(datum.SelectedDate.ToString() + " | " + laden + " | " + person + " | " + einmalig);
        }
    }
}
