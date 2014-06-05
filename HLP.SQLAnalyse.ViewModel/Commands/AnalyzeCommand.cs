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
        OperacoesSqlRepository operacao;

        public AnalyzeCommand(AnalyzeViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
            bWorkerTables = new BackgroundWorker();

            this.ViewModel.AddCommand = new RelayCommand(execute: i => this.AddConexao(),
             canExecute: i => CanTesteAndADD());
            this.ViewModel.TestarCommand = new RelayCommand(execute: i => this.TestConnection(),
            canExecute: i => CanTesteAndADD());
            //this.ViewModel.ProsseguirCommand

            // Pesquisa servidores SQL
            this.ViewModel.bWorkerPesquisa.DoWork += bWorkerPesquisa_DoWork;
            this.ViewModel.bWorkerPesquisa.RunWorkerAsync();

            this.bWorkerTables.DoWork += bWorkerTables_DoWork;

        }

        void bWorkerTables_DoWork(object sender, DoWorkEventArgs e)
        {
            operacao = new OperacoesSqlRepository(this.ViewModel.currentModel.conexoes.FirstOrDefault().ConnectionStringCompleted);
            this.ViewModel.currentModel.lTablePrincipal = new System.Collections.ObjectModel.ObservableCollection<Model.TableModel>(operacao.GetTabelas());
            operacao = new OperacoesSqlRepository(this.ViewModel.currentModel.conexoes.LastOrDefault().ConnectionStringCompleted);
            this.ViewModel.currentModel.lTableSecudary = operacao.GetTabelas();
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
