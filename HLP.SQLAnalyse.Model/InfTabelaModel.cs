using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLP.SQLAnalyse.Model
{
    public class InfTabelaModel
    {
        private string _nomeTabela;
        public string NomeTabela
        {
            get { return _nomeTabela; }
            set { _nomeTabela = value; }
        }

        private string _tabelaOwner;
        public string TabelaOwner
        {
            get { return _tabelaOwner; }
            set { _tabelaOwner = value; }
        }

        private string _nomeColuna;
        public string NomeColuna
        {
            get { return _nomeColuna; }
            set { _nomeColuna = value; }
        }

        private string _tipoColuna;
        public string TipoColuna
        {
            get { return _tipoColuna; }
            set { _tipoColuna = value; }
        }

        private string _tamanho;
        public string Tamanho
        {
            get { return _tamanho; }
            set { _tamanho = value; }
        }

        private string _casasDecimais;
        public string CasasDecimais
        {
            get { return _casasDecimais; }
            set { _casasDecimais = value; }
        }

        private string _precisao;
        public string Precisao
        {
            get { return _precisao; }
            set { _precisao = value; }
        }

        private string _isNullable;
        public string IsNullable
        {
            get { return _isNullable; }
            set { _isNullable = value; }
        }

    }
}
