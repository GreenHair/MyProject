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

namespace NummerfeldTest
{
    /// <summary>
    /// Interaktionslogik für NumberInput.xaml
    /// </summary>
    public partial class NumberInput : UserControl
    {
        bool komma = false;
        public NumberInput(int breite = 100)
        {
            InitializeComponent();
            txtBox.Width = breite;
        }

        public string Text
        {
            get
            {
                return txtBox.Text;
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
                Regex regex = new Regex("([0-9])");
                return regex.IsMatch(text);
            }
            else
            {
                komma = new Regex(",").IsMatch(text);
                Regex regex = new Regex("([0-9,])");
                return regex.IsMatch(text);
            }
        }
    }
}
