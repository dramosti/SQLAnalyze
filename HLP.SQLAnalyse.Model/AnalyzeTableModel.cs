using HLP.Base.ClassesBases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HLP.SQLAnalyse.Model
{
    public partial class AnalyzeTableModel : modelBase
    {
        public AnalyzeTableModel() { }

        private ObservableCollection<ConnectionConfigModel> _conexoes = new ObservableCollection<ConnectionConfigModel>();
        public ObservableCollection<ConnectionConfigModel> conexoes
        {
            get { return _conexoes; }
            set
            {
                _conexoes = value;
                base.NotifyPropertyChanged(propertyName: "conexoes");
            }
        }

        private ObservableCollection<TableModel> _lTablePrincipal = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> lTablePrincipal
        {
            get { return _lTablePrincipal; }
            set
            {
                _lTablePrincipal = value;
                base.NotifyPropertyChanged(propertyName: "lTablePrincipal");
            }
        }

        private ObservableCollection<TableModel> _lTableSecudary = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> lTableSecudary
        {
            get { return _lTableSecudary; }
            set
            {
                _lTableSecudary = value;
                base.NotifyPropertyChanged(propertyName: "lTableSecudary");
            }
        }


        private ObservableCollection<TableModel> _lTablePrincipalResult = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> lTablePrincipalResult
        {
            get { return _lTablePrincipalResult; }
            set
            {
                _lTablePrincipalResult = value;
                base.NotifyPropertyChanged(propertyName: "lTablePrincipalResult");
            }
        }

        private ObservableCollection<TableModel> _lTableSecundaryResult = new ObservableCollection<TableModel>();
        public ObservableCollection<TableModel> lTableSecundaryResult
        {
            get { return _lTableSecundaryResult; }
            set
            {
                _lTableSecundaryResult = value;
                base.NotifyPropertyChanged(propertyName: "lTableSecundaryResult");
            }
        }
                




        private TableModel _currentTablePrincipal = new TableModel();
        public TableModel currentTablePrincipal
        {
            get { return _currentTablePrincipal; }
            set
            {
                _currentTablePrincipal = value;
                base.NotifyPropertyChanged(propertyName: "currentTablePrincipal");
            }
        }

        private TableModel _currentTableSecundary = new TableModel();
        public TableModel currentTableSecundary
        {
            get { return _currentTableSecundary; }
            set
            {
                _currentTableSecundary = value;
                base.NotifyPropertyChanged(propertyName: "currentTableSecundary");
            }
        }




        private ObservableCollection<TableModel> _lTableSelected = new ObservableCollection<TableModel>();
        /// <summary>
        /// Lista de tabelas para ser manipulada e ficar a disposição de selecionar.
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

        public void ExecuteAnalyse()
        {
            try
            {
                TableModel tableSecundary = null;
                FieldModel fieldSecundary = null;
                this.lTablePrincipalResult = new ObservableCollection<TableModel>();
                this.lTableSecundaryResult = new ObservableCollection<TableModel>();
                TableModel tablePrimaryResult = null;
                TableModel tableSecundaryResult = null;

                foreach (var tablePrincipal in lTablePrincipal.Where(c => c.isSelect))
                {
                    try
                    {
                        //busca a tabela a ser analisada
                        tableSecundary = lTableSecudary.FirstOrDefault(c => c.xTable == tablePrincipal.xTable);

                        if (tableSecundary != null)
                        {
                            tablePrimaryResult = new TableModel();
                            tableSecundaryResult = new TableModel();
                            tablePrimaryResult.xTable = tableSecundaryResult.xTable = tablePrincipal.xTable;


                            //percorre os campos da tabela principal da 1º conexão
                            foreach (FieldModel fieldPrincipal in tablePrincipal.lField)
                            {
                                fieldSecundary = tableSecundary.lField.FirstOrDefault(c => c.xField == fieldPrincipal.xField);

                                // se existir o campo eu analiso todos os campos, caso nao tenha, eu deixo tudo como inválido.
                                if (fieldSecundary != null)
                                {
                                    fieldPrincipal.SetTrueValidacao();
                                    fieldPrincipal.wasAnalyze = fieldSecundary.wasAnalyze = fieldSecundary.bxField = true;
                                    fieldSecundary.bxTipo = fieldPrincipal.xTipo == fieldSecundary.xTipo;
                                    fieldSecundary.bisNotNull = fieldPrincipal.isNotNul == fieldSecundary.isNotNul;
                                    fieldSecundary.bPosicao = fieldPrincipal.posicao == fieldSecundary.posicao;
                                    if (!fieldSecundary.Success)
                                    {
                                        tablePrimaryResult.lField.Add(fieldPrincipal);
                                        tableSecundaryResult.lField.Add(fieldSecundary);
                                    }
                                }
                                else
                                {
                                    fieldPrincipal.SetFalseValidacao();
                                }

                            }
                            if (tablePrimaryResult.lField.Count() > 0)
                            {
                                //Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                                //{
                                foreach (var item in tableSecundary.lField.Where(c => c.wasAnalyze == false))
                                {
                                    tableSecundaryResult.lField.Add(item);
                                }
                                foreach (var item in tablePrincipal.lField.Where(c => c.wasAnalyze == false))
                                {
                                    tablePrimaryResult.lField.Add(item);
                                }

                                this.lTablePrincipalResult.Add(tablePrimaryResult);
                                this.lTableSecundaryResult.Add(tableSecundaryResult);
                                //}));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public partial class AnalyzeTableModel
    {
        public override string this[string columnName]
        {
            get
            {
                return base[columnName];
            }
        }
    }
}
