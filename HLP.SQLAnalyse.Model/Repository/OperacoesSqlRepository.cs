using HLP.Comum.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLP.SQLAnalyse.Model.Repository
{
    public class OperacoesSqlRepository
    {
        private SqlConnection conn;
        public OperacoesSqlRepository(string sString)
        {
            conn = new SqlConnection(sString);
        }

        public static DataTable GetServer()
        {
            try
            {
                DataTable dt = new DataTable();
                DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory("System.Data.SqlClient");

                if (dbProviderFactory.CanCreateDataSourceEnumerator)
                {
                    DbDataSourceEnumerator dbDataSourceEnumerator = dbProviderFactory.CreateDataSourceEnumerator();

                    if (dbDataSourceEnumerator != null)
                    {
                        dt = dbDataSourceEnumerator.GetDataSources();
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool TestConnection(string connectionString)
        {
            bool ret = false;
            SqlConnection connection = new SqlConnection();
            try
            {
                connection.ConnectionString = connectionString;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        ret = true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return ret;
        }

        public List<TableModel> GetTabelas()
        {
            List<TableModel> lret = new List<TableModel>();

            if (conn != null)
                try
                {
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    // tras todas as tabelas do banco que esta conectado...
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME", conn);
                    adapter.Fill(dt);

                    lret = (from r in dt.AsEnumerable()
                            select new TableModel
                            {
                                xTable = r["TABLE_NAME"].ToString()
                            }).ToList();

                }
                catch (SqlException se)
                {
                    throw new Exception(se.Message);
                }
                finally
                {
                    conn.Close();
                }
            return lret;

        }

        public List<FieldModel> GetDetalhes(string sNomeTabela)
        {
            List<FieldModel> _tabelasDetalhes = new List<FieldModel>();
            if (conn != null)
                try
                {
                    DataTable dt = new DataTable();
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlCommand command = new SqlCommand("sp_columns", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@table_name", sNomeTabela);

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {

                        _tabelasDetalhes.Add(new FieldModel
                        {
                            xField = row[3].ToString(),
                            xTypeName = row[5].ToString(),
                            xTamanho = row[7].ToString(),
                            xCasasDecimais = row[8].ToString(),
                            xPrecisao = row[6].ToString(),
                            isNotNul = (row[10].ToString() == "0" ? true : false)
                        });
                    }
                    dt.Dispose();

                }
                catch (SqlException se)
                {
                    throw new Exception(se.Message);
                }
                finally
                {
                    conn.Close();
                }
            return _tabelasDetalhes;
        }

        public DataTable GetListaTabFilha(string nmTabPai)
        {
            DataTable dt = new DataTable();
            if (conn != null)
                try
                {

                    if (conn.State == ConnectionState.Closed)
                        conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT    OBJECT_NAME(constid) FK, OBJECT_NAME(FKEYID) Tabela_Filha, OBJECT_NAME(rKEYID) Tabela_Pai, COL_NAME(Rkeyid, Rkey) Colunas_Pai, COL_NAME(fkeyid, fkey) Colunas_Filha" +
                                                                " FROM    SYSFOREIGNKEYS where OBJECT_NAME(rKEYID) = '" + nmTabPai + "'", conn);
                    adapter.Fill(dt);
                }
                catch (SqlException se)
                {
                    throw new Exception(se.Message);
                }
                finally
                {
                    conn.Close();
                }
            return dt;
        }
    }
}
