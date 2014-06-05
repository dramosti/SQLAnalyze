using HLP.Base.ClassesBases;
using HLP.SQLAnalyse.Model.Repository;
using HLP.SQLAnalyse.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLP.SQLAnalyse.ViewModel.Commands
{
    public class AnalyzeCommand
    {
        public AnalyzeViewModel ViewModel { get; set; }
        public BackgroundWorker bWorkerTables;
        public BackgroundWorker bWorkerAnalyseTable;
        OperacoesSqlRepository operacao;

        public AnalyzeCommand(AnalyzeViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
            bWorkerTables = new BackgroundWorker();

            this.ViewModel.AddCommand = new RelayCommand(execute: i => this.AddConexao(),
                   canExecute: i => CanTesteAndADD());
            this.ViewModel.TestarCommand = new RelayCommand(execute: i => this.TestConnection(),
                   canExecute: i => CanTesteAndADD());
            this.ViewModel.NextCommand = new RelayCommand(execute: i => this.Next(tpAnalyse: i),
                   canExecute: i => CanTesteAndADD());

            // Pesquisa servidores SQL
            this.ViewModel.bWorkerPesquisa.DoWork += bWorkerPesquisa_DoWork;
            this.ViewModel.bWorkerPesquisa.RunWorkerAsync();
            this.bWorkerTables.DoWork += bWorkerTables_DoWork;
        }


        private void ExecAnalyze()
        {
            this.ViewModel.currentModel.ExecuteAnalyse();
        }

        private bool CanExecAnalyze()
        {
            return this.ViewModel.currentModel.lTablePrincipal.Where(c => c.isSelect).Count() > 0;
        }


        private void Next(object tpAnalyse)
        {
            switch (tpAnalyse.ToString())
            {
                case "Tabelas": this.bWorkerTables.RunWorkerAsync();
                    break;
            }

        }
        private bool CanNext()
        {
            return this.ViewModel.currentModel.conexoes.Count == 2;
        }


        void bWorkerTables_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                operacao = new OperacoesSqlRepository(this.ViewModel.currentModel.conexoes.FirstOrDefault().ConnectionStringCompleted);
                this.ViewModel.currentModel.lTablePrincipal = new System.Collections.ObjectModel.ObservableCollection<Model.TableModel>(operacao.GetTabelas());
                foreach (var table in this.ViewModel.currentModel.lTablePrincipal)
                {
                    table.lField = operacao.GetDetalhes(table.xTable);
                }
                operacao = new OperacoesSqlRepository(this.ViewModel.currentModel.conexoes.LastOrDefault().ConnectionStringCompleted);
                this.ViewModel.currentModel.lTableSecudary = operacao.GetTabelas();
                foreach (var table in this.ViewModel.currentModel.lTablePrincipal)
                {
                    table.lField = operacao.GetDetalhes(table.xTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void bWorkerPesquisa_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                DataTable dt = OperacoesSqlRepository.GetServer();
                if (dt != null)
                {
                    this.ViewModel.servers = new System.Collections.ObjectModel.ObservableCollection<string>(dt.AsEnumerable().Select(c => c["ServerName"].ToString()).ToList());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void TestConnection()
        {
            try
            {
                if (String.IsNullOrEmpty(this.ViewModel.currentConexao.xBaseDados))
                    MessageHlp.Show(StMessage.stAlert, "Banco de Dados não foi selecionado");
                if (OperacoesSqlRepository.TestConnection(this.ViewModel.currentConexao.ConnectionStringCompleted))
                    MessageHlp.Show(StMessage.stAlert, "Teste de conexão realizado com sucesso!");
                else
                    MessageHlp.Show(StMessage.stAlert, "Teste de Conexão não obteve êxito!");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void AddConexao()
        {
            this.ViewModel.currentModel.conexoes.Add(this.ViewModel.currentConexao);
            this.ViewModel.currentConexao = new Comum.Model.Models.ConnectionConfigModel();
        }
        bool CanTesteAndADD()
        {
            if (this.ViewModel.currentConexao.ConnectionStringCompleted != ""
                && !(string.IsNullOrEmpty(this.ViewModel.currentConexao.xBaseDados)))
                return true;
            else
                return false;
        }
    }
}
