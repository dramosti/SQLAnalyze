using HLP.Base.ClassesBases;
using HLP.Comum.Model.Models;
using HLP.SQLAnalyse.Model;
using HLP.SQLAnalyse.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HLP.SQLAnalyse.ViewModel.ViewModels
{
    public class AnalyzeViewModel : ViewModelBase<AnalyzeTableModel>
    {
        public ICommand TestarCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand ProsseguirCommand { get; set; }


        public AnalyzeCommand command { get; set; }

        private ConnectionConfigModel _currentConexao;
        public ConnectionConfigModel currentConexao
        {
            get { return _currentConexao; }
            set { _currentConexao = value; }
        }

        private ObservableCollection<string> _servers = new ObservableCollection<string>();
        public ObservableCollection<string> servers
        {
            get { return _servers; }
            set
            {
                _servers = value;
                base.NotifyPropertyChanged(propertyName: "servers");
            }
        }


        public AnalyzeViewModel()
        {
            this.currentModel = new AnalyzeTableModel();
            command = new AnalyzeCommand(ViewModel: this);

        }

    }
}
