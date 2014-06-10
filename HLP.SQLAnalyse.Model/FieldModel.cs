using HLP.Base.ClassesBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLP.SQLAnalyse.Model
{
    public partial class FieldModel : modelBase
    {
        public FieldModel()
        {
        }

        private string _xField;
        public string xField
        {
            get { return _xField; }
            set
            {
                _xField = value;
                base.NotifyPropertyChanged(propertyName: "xField");
            }
        }


        private string _xTipo;
        public string xTipo
        {
            get { return _xTipo; }
            set
            {
                _xTipo = value;
                base.NotifyPropertyChanged(propertyName: "xTipo");
            }
        }

        public void SetxTipo()
        {
            switch (this.xTypeName.ToUpper())
            {
                case "INT":
                case "INT IDENTITY":
                    {
                        this.xTipo = "int";
                    } break;
                case "DECIMAL": this.xTipo = string.Format("DECIMAL ({0},{1})", this.xPrecisao, this.xCasasDecimais); break;
                case "VARCHAR": this.xTipo = string.Format("VARCHAR ({0})", this.xTamanho); break;
                case "DATE": this.xTipo = "DATE"; break;
                case "BIT": this.xTipo = "BIT"; break;
                case "TINYINT": this.xTipo = "TINYINT"; break;
                case "TIME": this.xTipo = "TIME"; break;
                case "IMAGE": this.xTipo = "IMAGE"; break;
                case "DATETIME": this.xTipo = "DATETIME"; break;
                default: this.xTipo = ""; break;
            }

        }

        private string _xTypeName;
        public string xTypeName
        {
            get { return _xTypeName; }
            set { _xTypeName = value; }
        }


        private string _xTamanho;
        public string xTamanho
        {
            get { return _xTamanho; }
            set { _xTamanho = value; }
        }

        private string _xCasasDecimais;
        public string xCasasDecimais
        {
            get { return _xCasasDecimais; }
            set { _xCasasDecimais = value; }
        }

        private string _xPrecisao;
        public string xPrecisao
        {
            get { return _xPrecisao; }
            set { _xPrecisao = value; }
        }

        private bool _isNotNul = false;
        public bool isNotNul
        {
            get { return _isNotNul; }
            set
            {
                _isNotNul = value;
                base.NotifyPropertyChanged(propertyName: "isNotNul");
            }
        }

        private int _posicao;
        public int posicao
        {
            get { return _posicao; }
            set
            {
                _posicao = value;
                base.NotifyPropertyChanged(propertyName: "posicao");
            }
        }

        #region Validacao

        private bool _bxField = false;
        public bool bxField
        {
            get { return _bxField; }
            set
            {
                _bxField = value;
                base.NotifyPropertyChanged(propertyName: "bxField");
            }
        }

        private bool _bxTipo = false;
        public bool bxTipo
        {
            get { return _bxTipo; }
            set
            {
                _bxTipo = value;
                base.NotifyPropertyChanged(propertyName: "bxTipo");
            }
        }

        private bool _bisNotNull = false;
        public bool bisNotNull
        {
            get { return _bisNotNull; }
            set
            {
                _bisNotNull = value;
                base.NotifyPropertyChanged(propertyName: "bisNotNull");
            }
        }

        private bool _bPosicao = false;
        public bool bPosicao
        {
            get { return _bPosicao; }
            set
            {
                _bPosicao = value;
                base.NotifyPropertyChanged(propertyName: "bPosicao");
            }
        }

        private bool _wasAnalyze = false;

        public bool wasAnalyze
        {
            get { return _wasAnalyze ; }
            set { _wasAnalyze  = value; }
        }
        

        #endregion

        public void SetTrueValidacao()
        {
            this.bisNotNull = this.bPosicao = this.bxField = this.bxTipo = true;
        }
        public void SetFalseValidacao()
        {
            this.bisNotNull = this.bPosicao = this.bxField = this.bxTipo = false;
        }

        public bool Success { get { return (this.bxField == true && this.bxTipo == true && this.bisNotNull == true && this.bPosicao == true); } }
    }
    public partial class FieldModel
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

