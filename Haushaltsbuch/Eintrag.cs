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
        public static StackPanel NeuerRechnung(List<Shop> Laeden, List<Produktgruppe> Prodgr, List<Person> Familie)
        {
            StackPanel stackpanel = new StackPanel();
            stackpanel.Children.Add(new Label
            {
                Content = "Neuer Rechnung",
                FontSize = 18,
                FontWeight = FontWeights.DemiBold,
                HorizontalAlignment = HorizontalAlignment.Center
            });
            StackPanel ZeileRechnung = new StackPanel { Orientation = Orientation.Horizontal };
            ZeileRechnung.Children.Add(new DatePicker { SelectedDate = DateTime.Now.Date });
            ZeileRechnung.Children.Add(new ComboBox { ItemsSource = Laeden });
            ZeileRechnung.Children.Add(new ComboBox { ItemsSource = Familie });
            ZeileRechnung.Children.Add(new CheckBox { Content = "Einmalig", IsChecked = true });
            stackpanel.Children.Add(ZeileRechnung);
            stackpanel.Children.Add(new Label
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
                stackpanel.Children.Add(ZeilePosten);
            }
            return stackpanel;
        }
    }
}
