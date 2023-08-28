using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SimpleWalletSystem.Model;

namespace SimpleWalletSystem.Services.Interfaces
{
    public interface IWallet
    {
        List<AccntDetail> getDetails(int userid);
        DataSet transferMoney(TransferRequest transfer);
        DataSet postTransaction(TransactionRequest transactionRequest, int type);
    }
}
