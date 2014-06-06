using HLP.Base.ClassesBases;
using HLP.SQLAnalyse.Model;
using HLP.SQLAnalyse.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace HLP.SQLAnalyse.ViewModel.ViewModels
{
    public class AnalyzeViewModel : ViewModelBase<AnalyzeTableModel>
    {
        public ICommand TestarCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand TpAnalyseCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }
        public ICommand FindTableCommand { get; set; }
        public ICommand CheckBoxSelectCommand { get; set; }


        public string tpAnalyze { get; set; }

        public AnalyzeViewModel()
        {
            this.currentModel = new AnalyzeTableModel();
            command = new AnalyzeCommand(ViewModel: this);
        }

        public AnalyzeCommand command { get; set; }

        private ObservableCollection<TableModel> _lTableSelected = new ObservableCollection<TableModel>();
        /// <summary>
        /// Lista de tabelas selecionadas para pesquisa
        /// </summary>
        public ObservableCollection<TableModel> lTableSelected
        {
            get { return _lTableSelected; }
            set
            {
                _lTableSelected = value;
                base.NotifyPropertyChanged(propertyName: "lTableSelected");
            }
        }



        private bool _bisChecked;

        public bool bisChecked
        {
            get { return _bisChecked; }
            set
            {
                _bisChecked = value;
                base.NotifyPropertyChanged(propertyName: "bisChecked");
            }
        }




        private ConnectionConfigModel _currentConexao = new ConnectionConfigModel();
        public ConnectionConfigModel currentConexao
        {
            get { return _currentConexao; }
            set { _currentConexao = value; base.NotifyPropertyChanged("currentConexao"); }
        }

        private ObservableCollection<string> _servers;
        public ObservableCollection<string> servers
        {
            get { return _servers; }
            set
            {
                _servers = value;
                base.NotifyPropertyChanged(propertyName: "servers");
            }
        }

        private ObservableCollection<string> _bases;
        public ObservableCollection<string> bases
        {
            get { return _bases; }
            set
            {
                _bases = value;
                base.NotifyPropertyChanged(propertyName: "bases");
            }
        }

        private string _xValueFind = string.Empty;
        public string xValueFind
        {
            get { return _xValueFind; }
            set { _xValueFind = value.ToUpper(); base.NotifyPropertyChanged("xValueFind"); }
        }

        public void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (this.lTableSelected.Count() > 0)
                    this.lTableSelected.Remove((TableModel)(sender as ListBox).SelectedItem);
            }
        }

    }
}
