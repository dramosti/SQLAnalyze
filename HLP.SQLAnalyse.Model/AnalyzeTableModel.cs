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

        private ObservableCollection<TableModel> _lTablePrincipal;
        public ObservableCollection<TableModel> lTablePrincipal
        {
            get { return _lTablePrincipal; }
            set
            {
                _lTablePrincipal = value;
                base.NotifyPropertyChanged(propertyName: "lTablePrincipal");
            }
        }

        private List<TableModel> _lTableSecudary;
        public List<TableModel> lTableSecudary
        {
            get { return _lTableSecudary; }
            set
            {
                _lTableSecudary = value;
                base.NotifyPropertyChanged(propertyName: "lTableSecudary");
            }
        }

        public void ExecuteAnalyse()
        {
            try
            {
                TableModel tableSecundary = null;
                FieldModel fieldSecundary = null;

                foreach (var tablePrincipal in lTablePrincipal)
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
