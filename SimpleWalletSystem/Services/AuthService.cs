using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SimpleWalletSystem.Model;

namespace SimpleWalletSystem.Services
{
    public class AuthService : IAuth
    {
        public IConfiguration _configuration;
        public string strConnect { get; set; }
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            strConnect = _configuration.GetConnectionString("DbConn").ToString();
        }

        public bool ValidateUser(string Username)
        {
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_CheckExistingUser", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Username", Username);
                        SqlDataReader reader = sqlCommand.ExecuteReader();
                        bool ret = reader.HasRows;
                        reader.Close();
                        sqlconn.Close();
                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the user.", ex);
            }
        }


        public DataSet NewUser(RegisterUser user)
        {
            try
            {
                DataSet dsReturn = new DataSet();
                using (SqlConnection sqlconn = new SqlConnection(strConnect))
                {
                    sqlconn.Open();
                    using (SqlCommand sqlCommand = new SqlCommand("SPR_RegisterNewUser", sqlconn))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@Fullname", user.Fullname);
                        sqlCommand.Parameters.AddWithValue("@Username", user.Username);
                        sqlCommand.Parameters.AddWithValue("@Password", user.Password);
                        sqlCommand.Parameters.AddWithValue("@AccountNumber", GenerateAccountNumber(12));
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

        private string GenerateAccountNumber(int length)
        {
            Random random = new Random();
            string number = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int digit = random.Next(0, 10);
                number += digit.ToString();
            }
            return number;
        }



    }
}
