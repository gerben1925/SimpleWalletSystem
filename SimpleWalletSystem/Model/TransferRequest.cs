using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWalletSystem.Model
{
    public class TransferRequest
    {
        [Required]
        public int SenderUserid { get; set; }
        [Required]
        public int ReceiverUserid { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
