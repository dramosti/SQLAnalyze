using HLP.SQLAnalyse.ViewModel.ViewModels;
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
        public WinSQLAnalyse(object WinPrincipal, AnalyzeViewModel ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.WinPrincipal = WinPrincipal as Window;
            lbSelected.KeyDown += this.ViewModel.ListBox_KeyDown;
            lbSelected.SelectionChanged += this.ViewModel.lbSelected_SelectionChanged;
        }
        public AnalyzeViewModel ViewModel
        {
            get { return this.DataContext as AnalyzeViewModel; }
            set { this.DataContext = value; }
        }
        public static void ShowAnalyse(object WinPrincipal, object objViewModel)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
           {
               WinSQLAnalyse win = new WinSQLAnalyse(WinPrincipal, objViewModel as AnalyzeViewModel);
               win.WindowState = WindowState.Maximized;
               win.ShowDialog();
           }));

        }
        public void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        public void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WinPrincipal.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
