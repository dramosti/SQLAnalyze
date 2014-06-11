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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HLP.SQLAnalyse.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.ViewModel = new AnalyzeViewModel();
            txtPassword.LostFocus += txtPassword_LostFocus;
            lbServidores.KeyDown += this.ViewModel.lbServidores_KeyDown;
        }

        public void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.servers.Count() > 0)
                if (!this.ViewModel.servers.FirstOrDefault().Contains("SEARCH"))
                    this.ViewModel.command.CarregaBases();
        }

        public AnalyzeViewModel ViewModel
        {
            get { return this.DataContext as AnalyzeViewModel; }
            set { this.DataContext = value; }
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
