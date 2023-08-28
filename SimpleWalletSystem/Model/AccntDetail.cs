using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWalletSystem.Model
{
    public class AccntDetail
    {
        public string Fullname { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public List<TransactionHistory> transactionHistories { get; set; }
        public List<TransferHistory> TransferHistories { get; set; }
    }
}
