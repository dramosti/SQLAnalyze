using HLP.Base.ClassesBases;
using HLP.SQLAnalyse.Model;
using HLP.SQLAnalyse.Model.Repository;
using HLP.SQLAnalyse.ViewModel.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
            this.ViewModel.NextCommand = new RelayCommand(execute: i => this.Next(i),
                   canExecute: i => CanNext());
            this.ViewModel.TpAnalyseCommand = new RelayCommand(execute: i => this.SetTpAnalyse(i),
                   canExecute: i => true);
            this.ViewModel.SelectAllCommand = new RelayCommand(execute: i => this.SelectAllCheckBox(),
                 canExecute: i => true);
            this.ViewModel.FindTableCommand = new RelayCommand(execute: i => this.FindTable(),
               canExecute: i => CanFindTable());

            // Pesquisa servidores SQL
            this.ViewModel.bWorkerPesquisa.DoWork += bWorkerPesquisa_DoWork;
            this.ViewModel.bWorkerPesquisa.RunWorkerAsync();
            this.bWorkerTables.DoWork += bWorkerTables_DoWork;
        }


        private void FindTable()
        {
            try
            {
                if (this.ViewModel.xValueFind != "")
                {
                    foreach (var table in this.ViewModel.currentModel.lTablePrincipal)
                    {
                        table.visibility = Visibility.Collapsed;
                    }
                    foreach (var table in this.ViewModel.currentModel.lTablePrincipal.Where(c => c.xTable.Contains(this.ViewModel.xValueFind.ToUpper())))
                    {
                        table.visibility = Visibility.Visible;
                    }
                }
                else
                {
                    foreach (var table in this.ViewModel.currentModel.lTablePrincipal)
                    {
                        table.visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool CanFindTable() 
        {
            if (this.ViewModel.currentModel.lTablePrincipal.Count() > 0)
                return true;
            else
                return false;
        }

        private void SetTpAnalyse(object tpAnalyze)
        {
            this.ViewModel.tpAnalyze = tpAnalyze.ToString();
        }

        private void ExecAnalyze()
        {
            this.ViewModel.currentModel.ExecuteAnalyse();
        }
        private bool CanExecAnalyze()
        {
            return this.ViewModel.currentModel.lTablePrincipal.Where(c => c.isSelect).Count() > 0;
        }

        private void Next(object win)
        {
            switch (this.ViewModel.tpAnalyze.ToString())
            {
                case "Table":
                    {

                        this.bWorkerTables.RunWorkerAsync(win);
                    }
                    break;
            }

        }
        private bool CanNext()
        {
            return this.ViewModel.currentModel.conexoes.Count == 2 && this.ViewModel.tpAnalyze != null;
        }

        /// <summary>
        /// Metodo para marcar e desmarcar tabelas para análise.
        /// </summary>
        public void SelectAllCheckBox()
        {
            if (this.ViewModel.currentModel.lTablePrincipal.Where(c => c.isSelect).Count() != this.ViewModel.currentModel.lTablePrincipal.Count())
            {
                foreach (var item in this.ViewModel.currentModel.lTablePrincipal.Where(c => !c.isSelect))
                {
                    item.isSelect = true;
                }
            }
            else
            {
                foreach (var item in this.ViewModel.currentModel.lTablePrincipal.Where(c => !c.isSelect))
                {
                    item.isSelect = false;
                }
            }
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
                Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                    new Action(() => ((Window)e.Argument).Visibility = Visibility.Hidden));

                Static.ExecuteMethodByReflection
                    (xNamespace: "HLP.SQLAnalyse.View.exe",
                    xType: "WinSQLAnalyse",
                    xMethod: "ShowAnalyse",
                    parameters: new object[] { ((object)e.Argument), this.ViewModel });
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
                    this.ViewModel.servers = new System.Collections.ObjectModel.ObservableCollection<string>(dt.AsEnumerable().Select
                        (
                            c => c["ServerName"].ToString() + (c["instanceName"].ToString() != "" ? (@"\" + c["instanceName"].ToString()) : "")
                        )
                        .ToList());
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
                    MessageBox.Show("Banco de Dados não foi selecionado!", "AVISO");
                if (OperacoesSqlRepository.TestConnection(this.ViewModel.currentConexao.ConnectionStringCompleted))
                    MessageBox.Show("Teste de conexão realizado com sucesso!", "AVISO");
                else
                    MessageBox.Show("Teste de Conexão não obteve êxito!", "AVISO");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        public void AddConexao()
        {
            if (this.ViewModel.currentModel.conexoes.Count <= 1)
            {
                if (this.ViewModel.currentModel.conexoes.Where(c => c.xBaseDados == this.ViewModel.currentConexao.xBaseDados).Count() == 0)
                {
                    this.ViewModel.currentModel.conexoes.Add(
                        new ConnectionConfigModel
                        {
                            Autentication = this.ViewModel.currentConexao.Autentication,
                            xBaseDados = this.ViewModel.currentConexao.xBaseDados,
                            xLogin = this.ViewModel.currentConexao.xLogin,
                            xPassword = this.ViewModel.currentConexao.xPassword,
                            xServerName = this.ViewModel.currentConexao.xServerName
                        });
                    this.ViewModel.currentConexao.xBaseDados = "";
                }
                else
                {
                    MessageBox.Show("Base deve ser diferente!", "AVISO");
                }
            }
            else
                MessageBox.Show("Base deve ser diferente!", "Somente é possível analisar duas bases de uma vez.");

        }
        bool CanTesteAndADD()
        {
            if (this.ViewModel.currentConexao != null)
                if (this.ViewModel.currentConexao.ConnectionStringCompleted != ""
                    && !(string.IsNullOrEmpty(this.ViewModel.currentConexao.xBaseDados)))
                    return true;
                else
                    return false;
            else
                return false;
        }


        public void CarregaBases()
        {
            try
            {
                if (this.ViewModel.currentConexao.ConnectionString != "")
                {
                    DataSet ds = OperacoesSqlRepository.GetDatabases(this.ViewModel.currentConexao.ConnectionString);

                    if (ds.Tables.Count > 0)
                    {
                        this.ViewModel.bases = new System.Collections.ObjectModel.ObservableCollection<string>(ds.Tables[0].AsEnumerable().Select(c => c["name"].ToString()).ToList());
                    }
                    else
                    {
                        this.ViewModel.bases = new System.Collections.ObjectModel.ObservableCollection<string>();
                    }
                }
                else
                {
                    this.ViewModel.bases = new System.Collections.ObjectModel.ObservableCollection<string>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



    }
}
