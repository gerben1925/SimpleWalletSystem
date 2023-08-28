using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWalletSystem.Model;
using SimpleWalletSystem.Services.Interfaces;

namespace SimpleWalletSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWallet _walletService;

        public WalletController(IWallet wallet)
        {
            _walletService = wallet;
        }

        [HttpGet("TransactionHistory")]
        public IActionResult GetBalance(int userid)
        {
            try
            {
                List<AccntDetail> accntDetails = _walletService.getDetails(userid);
                if (accntDetails.Count <= 0)
                {
                    return BadRequest("User account not found!");
                }

                var data = new
                {
                    Fullname = accntDetails[0].Fullname.ToString(),
                    AccountNumber = accntDetails[0].AccountNumber.ToString(),
                    CurrentBalance = accntDetails[0].CurrentBalance.ToString(),
                    TransferHistory = accntDetails[0].TransferHistories,
                    TransactionHistory = accntDetails[0].transactionHistories
                };
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost("TransferMoney")]
        public IActionResult TransferMoney([FromBody] TransferRequest transferRequest)
        {
            try
            {
                List<AccntDetail> senderAccntDetails = _walletService.getDetails(transferRequest.SenderUserid);

                if (senderAccntDetails.Count == 0)
                {
                    return BadRequest("User account not found!");
                }

                decimal availableBalance = Decimal.Parse(senderAccntDetails[0].CurrentBalance.ToString());

                if (availableBalance < 0 || transferRequest.Amount > availableBalance)
                {
                    return BadRequest("Insufficient balance");
                }


                DataSet ds = _walletService.transferMoney(transferRequest);
                return Ok("Money has been successfully transferred!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("DepositMoney")]
        public IActionResult DepositMoney([FromBody] TransactionRequest transactionRequest)
        {
            try
            {
                List<AccntDetail> AccntDetails = _walletService.getDetails(transactionRequest.Userid);

                if (AccntDetails.Count == 0)
                {
                    return BadRequest("User account not found!");
                }

                DataSet ds = _walletService.postTransaction(transactionRequest, 0);
                return Ok("Transaction has been successfully deposited!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("WithdrawMoney")]
        public IActionResult WithdrawMoney([FromBody] TransactionRequest transactionRequest)
        {

            try
            {
                List<AccntDetail> AccntDetails = _walletService.getDetails(transactionRequest.Userid);
                if (AccntDetails.Count == 0)
                {
                    return BadRequest("User account not found!");
                }
                decimal currentBalance = decimal.Parse(AccntDetails[0].CurrentBalance.ToString());

                if (currentBalance <= 0 || transactionRequest.Amount > currentBalance)
                {
                    return BadRequest("Insufficient Balance!");
                }


                DataSet ds = _walletService.postTransaction(transactionRequest, 1);
                return Ok("Transaction has been successfully withdrawn!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}
