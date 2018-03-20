using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Haushaltsbuch
{
    /// <summary>
    /// Interaction logic for Nummernfeld.xaml
    /// </summary>
    public partial class Nummernfeld : UserControl
    {
        Regex number_regex = new Regex("([0-9])");
        Regex komma_regex = new Regex("([0-9,])");
        bool komma = false;
        public Nummernfeld()
        {
            InitializeComponent();
            //txtBox.Width = 100;
        }

        public string Text
        {
            get
            {
                return txtBox.Text = txtBox.Text.Replace(" ", "");
            }
            set
            {
                txtBox.Text = value;
            }
        }

        private void txtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string text)
        {

            if (komma)
            {
                return number_regex.IsMatch(text);
            }
            else
            {
                komma = text == ",";
                return komma_regex.IsMatch(text);
            }
        }

        private void txtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            komma = txtBox.Text.Contains(",");
        }
    }
}
