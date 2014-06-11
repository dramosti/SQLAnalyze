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
        public DataGrid dgvFieldSecundary { get; set; }
        public DataGrid dgvFieldPrimary { get; set; }

        public ICommand FecharCommand { get; set; }
        public ICommand MinimizeCommand { get; set; }

        public ICommand FieldNotFoundCommand { get; set; }
        public ICommand TestarCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand TpAnalyseCommand { get; set; }
        public ICommand ExecuteAnalyzeCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }
        public ICommand FindTableCommand { get; set; }
        public string tpAnalyze { get; set; }

        private string _xBasePrincipal;
        public string xBasePrincipal
        {
            get { return _xBasePrincipal; }
            set
            {
                _xBasePrincipal = value;
                base.NotifyPropertyChanged(propertyName: "xBasePrincipal");
            }
        }

        private string _xBaseSecundary;
        public string xBaseSecundary
        {
            get { return _xBaseSecundary; }
            set
            {
                _xBaseSecundary = value;
                base.NotifyPropertyChanged(propertyName: "xBaseSecundary");
            }
        }

        public AnalyzeViewModel()
        {
            this.currentModel = new AnalyzeTableModel();
            command = new AnalyzeCommand(ViewModel: this);
            this.currentConexao.xLogin = "SA";
            this.currentConexao.xPassword = "H029060tSql";

        }

        public AnalyzeCommand command { get; set; }
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



        private int _indexPrincipal;
        public int indexPrincipal
        {
            get { return _indexPrincipal; }
            set
            {
                _indexPrincipal = value;
                base.NotifyPropertyChanged(propertyName: "indexPrincipal");
            }
        }



        private FieldModel _currentFieldPrincipal;
        public FieldModel currentFieldPrincipal
        {
            get { return _currentFieldPrincipal; }
            set
            {
                _currentFieldPrincipal = value;
                base.NotifyPropertyChanged(propertyName: "currentFieldPrincipal");
                if (value != null)
                {
                    FieldModel f = this.currentModel.currentTableSecundary.lField.FirstOrDefault(C => C.xField == value.xField);
                    if (f != null)
                        if (this.currentFieldSecundary != f)
                        {
                            this.dgvFieldSecundary.ScrollIntoView(f);
                            this.currentFieldSecundary = f;
                        }
                }
            }
        }


        private FieldModel _currentFieldSecundary;
        public FieldModel currentFieldSecundary
        {
            get { return _currentFieldSecundary; }
            set
            {

                _currentFieldSecundary = value;
                base.NotifyPropertyChanged(propertyName: "currentFieldSecundary");
                if (value != null)
                {
                    FieldModel f = this.currentModel.currentTablePrincipal.lField.FirstOrDefault(C => C.xField == value.xField);
                    if (f != null)
                        if (this.currentFieldPrincipal != f)
                        {
                            this.dgvFieldPrimary.ScrollIntoView(f);
                            this.currentFieldPrincipal = f;
                        }
                }
            }
        }


        private ConnectionConfigModel _currentConexao = new ConnectionConfigModel();
        public ConnectionConfigModel currentConexao
        {
            get { return _currentConexao; }
            set { _currentConexao = value; base.NotifyPropertyChanged("currentConexao"); }
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

        private ObservableCollection<string> _bases = new ObservableCollection<string>();
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
                if (this.currentModel.lTablePrincipal.Where(c => c.isSelect).Count() > 0)
                {
                    ((TableModel)(sender as ListBox).SelectedItem).isSelect = false;
                }
            }
        }
        public void lbServidores_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (this.bases.Count() > 0)
                {
                    string s = (sender as ListBox).SelectedItem.ToString();
                    ConnectionConfigModel item = ((sender as ListBox).SelectedItem) as ConnectionConfigModel;
                    this.currentModel.conexoes.Remove(item);
                }
            }
        }

        public void lbSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetVisibleFields((sender as ListBox).SelectedItem as TableModel);
        }

        public void SetVisibleFields(TableModel objTableModel)
        {
            if (objTableModel != null)
            {
                if (objTableModel.xTable != null)
                {
                    objTableModel = this.currentModel.lTablePrincipalResult.FirstOrDefault(c => c.xTable == objTableModel.xTable);
                    this.currentModel.currentTablePrincipal = new TableModel();
                    this.currentModel.currentTableSecundary = new TableModel();

                    if (this.currentModel.bFieldNotFound)
                    {
                        this.currentModel.currentTablePrincipal.xTable = objTableModel.xTable;
                        foreach (var item in objTableModel.lField.Where(c => c.bxField == false))
                        {
                            this.currentModel.currentTablePrincipal.lField.Add(item);
                        }

                        TableModel objTableSecundaryModel = this.currentModel.lTableSecundaryResult.FirstOrDefault(c => c.xTable == this.currentModel.currentTablePrincipal.xTable);

                        if (objTableSecundaryModel != null)
                        {
                            foreach (var item in objTableSecundaryModel.lField.Where(c => c.bxField == false))
                            {
                                this.currentModel.currentTableSecundary.lField.Add(item);
                            }
                        }
                    }
                    else
                    {
                        this.currentModel.currentTablePrincipal = objTableModel;

                        if (this.currentModel.currentTablePrincipal != null)
                            this.currentModel.currentTableSecundary = this.currentModel.lTableSecundaryResult.FirstOrDefault(c => c.xTable == this.currentModel.currentTablePrincipal.xTable);
                    }
                }
            }
        }



    }
}
