using Calculator.Api.Controllers;
using Calculator.Application.Contract;
using Calculator.Domain.Constants;
using Calculator.Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Calculator.Test.Api
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldReturn201Created_WhenSuccess()
        {
            // Arrange
            _mockUserService.Setup(service => service.CreateUserAsync())
                .ReturnsAsync(new OperationResponse { Status = true, ResponseCode = ApiStatusConstants.Created });

            // Act
            var result = await _userController.CreateUser();

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.Created);
        }

        [Fact]
        public async Task GetUser_ShouldReturn200OK_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(new OperationResponse { Status = true, ResponseCode = ApiStatusConstants.OK });

            // Act
            var result = await _userController.GetUser(userId);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.OK);
        }

        [Fact]
        public async Task GetUser_ShouldReturn404NotFound_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>()))
                .ReturnsAsync(new OperationResponse { Status = false, ResponseCode = ApiStatusConstants.NotFound });

            // Act
            var result = await _userController.GetUser(userId);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.NotFound);
        }
    }

}
