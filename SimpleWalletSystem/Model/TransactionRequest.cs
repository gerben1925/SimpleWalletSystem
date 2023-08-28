using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWalletSystem.Model
{
    public class TransactionRequest
    {
        [Required]
        public int Userid { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
