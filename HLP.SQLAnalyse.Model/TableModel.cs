using HLP.Base.ClassesBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HLP.SQLAnalyse.Model
{
    public partial class TableModel : modelBase
    {
        public TableModel() { }

        private string _xTable;
        public string xTable
        {
            get { return _xTable; }
            set
            {
                _xTable = value;
                base.NotifyPropertyChanged(propertyName: "xTable");
            }
        }

        private bool _isSelect = false;
        public bool isSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                base.NotifyPropertyChanged(propertyName: "isSelect");
            }
        }

        private Visibility _visibility = Visibility.Visible;
        public Visibility visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                base.NotifyPropertyChanged(propertyName: "visibility");
            }
        }
        

        
        private List<FieldModel> _lField = new List<FieldModel>();
        public List<FieldModel> lField
        {
            get { return _lField;  }
            set
            {
                _lField = value;
                base.NotifyPropertyChanged(propertyName: "lField");
            }
        }




    }
    public partial class TableModel
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
