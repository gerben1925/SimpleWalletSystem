using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWalletSystem.Model
{
    public class TransferHistory
    {
        public string From { get; set; }
        public string To { get; set; }
        public string AmountTransferred { get; set; }
        public DateTime TransferredDatetime { get; set; }
    }

}
