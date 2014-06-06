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
            txtPassword.LostFocus+=txtPassword_LostFocus;
        }

        public void txtPassword_LostFocus(object sender, RoutedEventArgs e)
        {
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized; 
        }
    }
}
