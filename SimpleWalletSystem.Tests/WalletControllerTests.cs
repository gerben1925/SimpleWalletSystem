using Castle.Core.Configuration;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWalletSystem.Controllers;
using SimpleWalletSystem.Model;
using SimpleWalletSystem.Services;
using SimpleWalletSystem.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace SimpleWalletSystem.Tests
{
    public class WalletControllerTests
    {
        [Fact]
        public void GetBalance_UserAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail>());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.GetBalance(userId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("User account not found!");
        }



        [Fact]
        public void GetBalance_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var userId = 1;
            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Throws(new Exception("Something went wrong."));

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.GetBalance(userId);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
            var objectResult = (ObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(new { message = "Something went wrong." });
        }

        [Fact]
        public void GetBalance_ValidUserId_ReturnsOkWithData()
        {
            // Arrange
            var userId = 1;
            var fakeAccntDetail = new AccntDetail
            {
                Fullname = "John Doe",
                AccountNumber = "123456789",
                CurrentBalance = 1000.0m,
                TransferHistories = new List<TransferHistory>(),
                transactionHistories = new List<TransactionHistory>()
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail> { fakeAccntDetail });

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.GetBalance(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.Value.Should().BeEquivalentTo(new
            {
                Fullname = "John Doe",
                AccountNumber = "123456789",
                CurrentBalance = "1000.0",
                TransferHistory = new List<TransferHistory>(),
                TransactionHistory = new List<TransactionHistory>()
            });
        }

        [Fact]
        public void TransferMoney_InsufficientBalance_ReturnsBadRequest()
        {
            // Arrange
            var senderUserId = 1;
            var transferRequest = new TransferRequest
            {
                SenderUserid = senderUserId,
                Amount = 100
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(senderUserId)).Returns(new List<AccntDetail>
            {
                new AccntDetail { CurrentBalance = 50 }
            });

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.TransferMoney(transferRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Insufficient balance");
        }

        [Fact]
        public void TransferMoney_SenderAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            var senderUserId = 1;
            var transferRequest = new TransferRequest
            {
                SenderUserid = senderUserId,
                Amount = 100
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(senderUserId)).Returns(new List<AccntDetail>());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.TransferMoney(transferRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("User account not found!");
        }

        [Fact]
        public void TransferMoney_ValidTransfer_ReturnsOk()
        {
            // Arrange
            var senderUserId = 1;
            var transferRequest = new TransferRequest
            {
                SenderUserid = senderUserId,
                Amount = 100
            };

            var fakeSenderAccntDetail = new AccntDetail
            {
                CurrentBalance = 200
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(senderUserId)).Returns(new List<AccntDetail> { fakeSenderAccntDetail });
            A.CallTo(() => fakeWalletService.transferMoney(transferRequest)).Returns(new DataSet());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.TransferMoney(transferRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Money has been successfully transferred!");
        }


        [Fact]
        public void DepositMoney_UserAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = 10;
            var transactionRequest = new TransactionRequest
            {
                Userid = userId,
                Amount = 100
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail>());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.DepositMoney(transactionRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("User account not found!");
        }

        [Fact]
        public void DepositMoney_ValidDeposit_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var transactionRequest = new TransactionRequest
            {
                Userid = userId,
                Amount = 100
            };

            var fakeAccntDetail = new AccntDetail
            {
                WalletId = 123
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail> { fakeAccntDetail });
            A.CallTo(() => fakeWalletService.postTransaction(transactionRequest, 0)).Returns(new DataSet());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.DepositMoney(transactionRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Transaction has been successfully deposited!");
        }


        [Fact]
        public void WithdrawMoney_InsufficientBalance_ReturnsBadRequest()
        {
            // Arrange
            var userId = 10;
            var transactionRequest = new TransactionRequest
            {
                Userid = userId,
                Amount = 150
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail>
            {
                new AccntDetail { CurrentBalance = 100 }
            });

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.WithdrawMoney(transactionRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Insufficient Balance!");
        }

        [Fact]
        public void WithdrawMoney_UserAccountNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = 1;
            var transactionRequest = new TransactionRequest
            {
                Userid = userId,
                Amount = 50
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail>());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.WithdrawMoney(transactionRequest);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("User account not found!");
        }

        [Fact]
        public void WithdrawMoney_ValidWithdraw_ReturnsOk()
        {
            // Arrange
            var userId = 1;
            var transactionRequest = new TransactionRequest
            {
                Userid = userId,
                Amount = 50
            };

            var fakeAccntDetail = new AccntDetail
            {
                CurrentBalance = 100
            };

            var fakeWalletService = A.Fake<IWallet>();
            A.CallTo(() => fakeWalletService.getDetails(userId)).Returns(new List<AccntDetail> { fakeAccntDetail });
            A.CallTo(() => fakeWalletService.postTransaction(transactionRequest, 1)).Returns(new DataSet());

            var controller = new WalletController(fakeWalletService);

            // Act
            var result = controller.WithdrawMoney(transactionRequest);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().Be("Transaction has been successfully withdrawn!");
        }


    }
}
