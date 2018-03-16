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
using System.Windows.Shapes;

namespace Haushaltsbuch
{
    /// <summary>
    /// Interaction logic for VerbindenFenster.xaml
    /// </summary>
    /// 
    public delegate void DelConnect(string connectstring);
    public partial class VerbindenFenster : Window
    {
        public event DelConnect Verbinden;
        public VerbindenFenster()
        {
            InitializeComponent();
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            string conn_string = "server=" + txtServer.Text + ";uid=" + txtUser.Text + ";password=" + txtPassword.Password + ";";
            Verbinden?.Invoke(conn_string);
            Close();
        }
    }
}
