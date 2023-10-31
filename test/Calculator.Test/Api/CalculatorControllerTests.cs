using Calculator.Api.Controllers;
using Calculator.Application.Contract;
using Calculator.Application.Dtos.Request;
using Calculator.Domain.Constants;
using Calculator.Domain.Enums;
using Calculator.Domain.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Calculator.Test.Api
{
    public class CalculatorControllerTests
    {
        private readonly Mock<ICalculatorService> _mockCalculatorService;
        private readonly CalculatorController _calculatorController;

        public CalculatorControllerTests()
        {
            _mockCalculatorService = new Mock<ICalculatorService>();
            _calculatorController = new CalculatorController(_mockCalculatorService.Object);
        }

        [Fact]
        public async Task PerformCalculation_ShouldReturn200OK_WhenCalculationSuccess()
        {
            // Arrange
            var calculationRequest = new CalculationRequest { FirstValue = 1, SecondValue = 2, OperationType = OperationTypeEnum.ADD };
            _mockCalculatorService.Setup(service => service.PerformCalculationAsync(It.IsAny<CalculationRequest>()))
                .ReturnsAsync(new OperationResponse { Status = true, ResponseCode = ApiStatusConstants.OK });

            // Act
            var result = await _calculatorController.PerformCalculation(calculationRequest);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.OK);
        }

        [Fact]
        public async Task GetCalculationHistory_ShouldReturn200OK_WhenHistoryExists()
        {
            // Arrange
            int userId = 1;
            _mockCalculatorService.Setup(service => service.GetCalculationHistoryByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OperationResponse { Status = true, ResponseCode = ApiStatusConstants.OK });

            // Act
            var result = await _calculatorController.GetCalculationHistory(userId);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.OK);
        }

        [Fact]
        public async Task GetCalculationHistory_ShouldReturn404NotFound_WhenHistoryDoesNotExist()
        {
            // Arrange
            int userId = 1;
            _mockCalculatorService.Setup(service => service.GetCalculationHistoryByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new OperationResponse { Status = false, ResponseCode = ApiStatusConstants.NotFound });

            // Act
            var result = await _calculatorController.GetCalculationHistory(userId);

            // Assert
            var objectResult = result as ObjectResult;
            objectResult.Should().NotBeNull();
            objectResult.StatusCode.Should().Be(ApiStatusConstants.NotFound);
        }
    }

}
