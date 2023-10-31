using Calculator.Application.Service;
using Calculator.Domain.Entities;
using Calculator.Infrastructure.Repository.Contracts;
using FluentAssertions;
using Moq;
using System.Data;

namespace Calculator.Test.Service
{

    public class UserServiceTests
    {
        private readonly Mock<ISqlQuery> _mockQuery;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockQuery = new Mock<ISqlQuery>();
            _userService = new UserService(_mockQuery.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnSuccessfulResponse_WhenUserIsCreated()
        {
            // Arrange
            _mockQuery.Setup(q => q.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, CommandType.StoredProcedure)).ReturnsAsync(1);

            // Act
            var result = await _userService.CreateUserAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
        }

        [Fact]
        public async Task CreateUserAsync_ShouldReturnFailureResponse_WhenUserCreationFails()
        {
            // Arrange
            _mockQuery.Setup(q => q.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).ReturnsAsync(0);

            // Act
            var result = await _userService.CreateUserAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnUserIfExists()
        {
            // Arrange
            int userId = 1;
            var user = new User { Id = userId, Username = "testuser" };

            _mockQuery.Setup(q => q.GetAsync<User>(It.IsAny<string>(), It.IsAny<object>(), null))
                      .ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
        }

        [Fact]
        public async Task GetUserById_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 1;

            _mockQuery.Setup(q => q.GetAsync<User>(It.IsAny<string>(), It.IsAny<object>(), null))
                      .ReturnsAsync((User)null);

            // Act
            var result = await _userService.GetUserById(userId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeFalse();
        }
    }

}
