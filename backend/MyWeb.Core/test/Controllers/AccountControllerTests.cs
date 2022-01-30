using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyWeb.Core.Controllers;
using MyWeb.Core.Models.Dtos;
using MyWeb.Core.Models.Entities;
using Xunit;

namespace MyWeb.Core.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task DoRegister_NewUser_Return200()
        {
            // Arrange
            Mock<UserManager<LoginUser>> mockUserManager = new(Mock.Of<IUserStore<LoginUser>>(),
                    null, null, null, null, null, null, null, null);
            mockUserManager.Setup(e => e.CreateAsync(It.IsAny<LoginUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            AccountController controller = new(null, null, null, mockUserManager.Object, null, null);
            RegisterDto registerDto = new("test", "test", "some@email.com");

            // Act
            var result = await controller.DoRegister(registerDto);

            // Assert
            var okResult = result as OkResult;
            okResult.Should().NotBeNull();
            if (okResult != null)
                okResult.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task DoRegister_DuplicateUser_Return422()
        {
            // Arrange
            Mock<UserManager<LoginUser>> mockUserManager = new(Mock.Of<IUserStore<LoginUser>>(),
                    null, null, null, null, null, null, null, null);
            mockUserManager.Setup(e => e.CreateAsync(It.IsAny<LoginUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(
                    new IdentityError() { Code = "Duplicate", Description = "Duplicate user." }));
            AccountController controller = new(null, null, null, mockUserManager.Object, null, null);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.TraceIdentifier = "12345";
            RegisterDto registerDto = new("test", "test", "some@email.com");

            // Act
            var result = await controller.DoRegister(registerDto);

            // Assert
            var unprocessableResult = result as UnprocessableEntityObjectResult;
            unprocessableResult.Should().NotBeNull();
            if (unprocessableResult != null)
                unprocessableResult.StatusCode.Should().Be(422);
        }
    }
}
