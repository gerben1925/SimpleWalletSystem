using Castle.Core.Configuration;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleWalletSystem.Controllers;
using SimpleWalletSystem.Model;
using SimpleWalletSystem.Services;
using System;
using System.Data;
using Xunit;

namespace SimpleWalletSystem.Tests
{

    public class AuthControllerTests
    {
        [Fact]
        public void RegisterUser_ConflictOnExistingUsername_ReturnsConflict()
        {
            // Arrange
            var authServiceFake = A.Fake<IAuth>();
            A.CallTo(() => authServiceFake.ValidateUser(A<string>._)).Returns(true);
            var controller = new AuthController(authServiceFake);

            // Act
            var newUser = new RegisterUser { Username = "existing_username" };
            var result = controller.registerUser(newUser);

            // Assert
            result.Should().BeOfType<ConflictObjectResult>()
                  .Which.Value.Should().Be("Username already exist! ");
        }

        [Fact]
        public void RegisterUser_SuccessfullyRegister_ReturnsOk()
        {
            // Arrange
            var authServiceFake = A.Fake<IAuth>();
            A.CallTo(() => authServiceFake.ValidateUser(A<string>._)).Returns(false);
            A.CallTo(() => authServiceFake.NewUser(A<RegisterUser>._)).Returns(new DataSet());
            var controller = new AuthController(authServiceFake);

            // Act
            var newUser = new RegisterUser { Username = "new_username" };
            var result = controller.registerUser(newUser);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().Be("Successfully Register!");
        }

        [Fact]
        public void RegisterUser_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var authServiceFake = A.Fake<IAuth>();
            A.CallTo(() => authServiceFake.ValidateUser(A<string>._)).Throws(new Exception("Some error"));
            var controller = new AuthController(authServiceFake);

            // Act
            var newUser = new RegisterUser { Username = "new_username" };
            var result = controller.registerUser(newUser);

            // Assert
            result.Should().BeOfType<ObjectResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
