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

namespace HLP.SQLAnalyse.View
{
    /// <summary>
    /// Interaction logic for WinSQLAnalyse.xaml
    /// </summary>
    public partial class WinSQLAnalyse : Window
    {
        Window WinPrincipal;
        public WinSQLAnalyse(object WinPrincipal)
        {
            InitializeComponent();
            this.WinPrincipal = WinPrincipal as Window;
        }

        public static void ShowAnalyse(object WinPrincipal)
        {
            WinSQLAnalyse win = new WinSQLAnalyse(WinPrincipal);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Show();
        }
    }
}
