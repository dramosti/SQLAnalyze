using HLP.Base.ClassesBases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        
        private TableModel _currentTableSecundary = new TableModel();;

        public TableModel currentTableSecundary
        {
            get { return _currentTableSecundary; }
            set
            {
                _currentTableSecundary = value;
                base.NotifyPropertyChanged(propertyName: "currentTableSecundary");
            }
        }
        
        


        private ObservableCollection<TableModel> _lTableToSelect = new ObservableCollection<TableModel>();
        /// <summary>
        /// Lista de tabelas para ser manipulada e ficar a disposição de selecionar.
        /// </summary>
        public ObservableCollection<TableModel> lTableToSelect
        {
            get { return _lTableToSelect; }
            set
            {
                _lTableToSelect = value;
                base.NotifyPropertyChanged(propertyName: "lTableToSelect");
            }
        }

        public void ExecuteAnalyse()
        {
            try
            {
                TableModel tableSecundary = null;
                FieldModel fieldSecundary = null;

                foreach (var tablePrincipal in lTablePrincipal.Where(c => c.isSelect))
                {
                    //busca a tabela a ser analisada
                    tableSecundary = lTableSecudary.FirstOrDefault(c => c.xTable == tablePrincipal.xTable);

                    if (tableSecundary != null)
                    {
                        //percorre os campos da tabela principal da 1º conexão
                        foreach (FieldModel fieldPrincipal in tablePrincipal.lField)
                        {
                            fieldSecundary = tableSecundary.lField.FirstOrDefault(c => c.xField == fieldPrincipal.xField);

                            // se existir o campo eu analiso todos os campos, caso nao tenha, eu deixo tudo como inválido.
                            if (fieldSecundary != null)
                            {
                                fieldPrincipal.SetTrueValidacao();
                                fieldSecundary.bxTipo = fieldPrincipal.xTipo == fieldSecundary.xTipo;
                                fieldSecundary.bisNotNull = fieldPrincipal.isNotNul == fieldSecundary.isNotNul;
                                fieldSecundary.bPosicao = fieldPrincipal.posicao == fieldSecundary.posicao;
                            }
                            else
                                fieldPrincipal.SetFalseValidacao();
                        }
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
