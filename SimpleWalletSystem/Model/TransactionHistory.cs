using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWalletSystem.Model
{
    public class TransactionHistory
    {
        public string Type { get; set; }
        public string Amount { get; set; }
        public string TransactionDatetime { get; set; }
    }
}
