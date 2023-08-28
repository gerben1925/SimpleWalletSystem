using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimpleWalletSystem.Model;
using SimpleWalletSystem.Services.Interfaces;

namespace SimpleWalletSystem.Services
{
    public class WalletService : IWallet
    {
        public IConfiguration _configuration;
        public string strConnect { get; set; }
        public WalletService(IConfiguration configuration)
        {
            _configuration = configuration;
            strConnect = _configuration.GetConnectionString("DbConn").ToString();
        }

        public List<AccntDetail> getDetails(int userid)
        {
            try
            {
                List<AccntDetail> ret = new List<AccntDetail>();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_CheckAccntDetails", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Userid", userid);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataSet dsReturn = new DataSet();
                        adapter.Fill(dsReturn);
                        sqlconn.Close();
                        DataTable dtRes = dsReturn.Tables[0];
                        foreach (DataRow odr in dtRes.Rows)
                        {
                            AccntDetail list = new AccntDetail();
                            list.Fullname = odr["Fullname"].ToString();
                            list.AccountNumber = odr["AccountNumber"].ToString();
                            list.CurrentBalance = decimal.Parse(odr["Balance"].ToString());
                            list.TransferHistories = GetTransfeHistory(userid);
                            list.transactionHistories = GetTransactionHistory(userid);
                            ret.Add(list);
                        }

                    }
                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the accnt.", ex);
            }
        }


        public DataSet transferMoney(TransferRequest transfer)
        {
            try
            {
                DataSet dsReturn = new DataSet();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_TransferMoney", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@SENDERSWALLETID", transfer.SenderUserid);
                        sqlCommand.Parameters.AddWithValue("@RECEIVERSSWALLETID", transfer.ReceiverUserid);
                        sqlCommand.Parameters.AddWithValue("@AMOUNT", transfer.Amount);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(dsReturn);
                    }
                    sqlconn.Close();
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the user.", ex);
            }
        }


        public List<TransferHistory> GetTransfeHistory(int userId)
        {
            try
            {
                List<TransferHistory> ret = new List<TransferHistory>();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_Transferhistory", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataSet dsReturn = new DataSet();
                        adapter.Fill(dsReturn);
                        sqlconn.Close();
                        DataTable dtRes = dsReturn.Tables[0];
                        foreach (DataRow odr in dtRes.Rows)
                        {
                            TransferHistory a_list = new TransferHistory();
                            a_list.From = odr["SendersName"].ToString();
                            a_list.To = odr["ReceiversName"].ToString();
                            a_list.AmountTransferred = odr["Amount"].ToString();
                            a_list.TransferredDatetime = DateTime.Parse(odr["TransferDateTime"].ToString());
                            ret.Add(a_list);
                        }

                    }
                    return ret;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TransactionHistory> GetTransactionHistory(int userId)
        {
            try
            {
                List<TransactionHistory> ret = new List<TransactionHistory>();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_TransactionHistory", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        DataSet dsReturn = new DataSet();
                        adapter.Fill(dsReturn);
                        sqlconn.Close();
                        DataTable dtRes = dsReturn.Tables[0];
                        foreach (DataRow odr in dtRes.Rows)
                        {
                            TransactionHistory a_list = new TransactionHistory();
                            a_list.Type = odr["Type"].ToString();
                            a_list.Amount = odr["Amount"].ToString();
                            a_list.TransactionDatetime = odr["TransactionDatetime"].ToString();
                            ret.Add(a_list);
                        }

                    }
                    return ret;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataSet postTransaction(TransactionRequest transactionRequest, int type)
        {
            try
            {
                DataSet dsReturn = new DataSet();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_Transaction", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Userid", transactionRequest.Userid);
                        sqlCommand.Parameters.AddWithValue("@Amount", transactionRequest.Amount);
                        sqlCommand.Parameters.AddWithValue("@Type", type);
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(dsReturn);
                    }
                    sqlconn.Close();
                    return dsReturn;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the table.", ex);
            }
        }

    }
}
