using Calculator.Application.Dtos.Request;
using Calculator.Application.Dtos.Response;
using Calculator.Application.Service;
using Calculator.Domain.Entities;
using Calculator.Domain.Enums;
using Calculator.Domain.Models.Response;
using Calculator.Infrastructure.Repository.Contracts;
using FluentAssertions;
using Moq;
using System.Data;

namespace Calculator.Test.Service
{
    public class CalculatorServiceTests
    {
        private readonly Mock<ISqlQuery> _mockQuery;
        private readonly CalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _mockQuery = new Mock<ISqlQuery>();
            _calculatorService = new CalculatorService(_mockQuery.Object);
        }

        [Fact]
        public async Task PerformCalculationAsync_ShouldReturnCorrectResult_ForAddition()
        {
            // Arrange
            var request = new CalculationRequest
            {
                FirstValue = 2,
                SecondValue = 3,
                OperationType = OperationTypeEnum.ADD
            };

            // Act
            var response = await _calculatorService.PerformCalculationAsync(request);

            // Assert
            response.Status.Should().BeTrue();
            response.ResponseData.Should().BeEquivalentTo(new CalculationResponse { Result = 5 });
        }

        [Fact]
        public async Task PerformCalculationAsync_ShouldThrowDivideByZeroException_WhenDividingByZero()
        {
            // Arrange
            var request = new CalculationRequest
            {
                FirstValue = 2,
                SecondValue = 0,
                OperationType = OperationTypeEnum.DIVIDE
            };

            // Act & Assert
            await Assert.ThrowsAsync<DivideByZeroException>(() => _calculatorService.PerformCalculationAsync(request));
        }

        [Fact]
        public async Task GetCalculationHistoryByUserIdAsync_ShouldReturnCalculationHistory()
        {
            // Arrange
            int userId = 1;
            _mockQuery.Setup(q => q.GetAllAsync<CalculationHistoryResponse>(It.IsAny<string>(), It.IsAny<object>(), CommandType.StoredProcedure))
                      .ReturnsAsync(new List<CalculationHistoryResponse>
                      {
                          new CalculationHistoryResponse { Result = 5 }
                      });

            // Act
            var result = await _calculatorService.GetCalculationHistoryByUserIdAsync(userId);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().BeTrue();
        }

        [Fact]
        public async Task SaveCalculationAsync_ShouldSaveCalculationHistory_Successfully()
        {
            // Arrange
            var calculationHistory = new CalculationHistory
            {
                UserId = 1,
                FirstValue = 2,
                SecondValue = 3,
                OperationType = (int)OperationTypeEnum.ADD,
                Result = 5
            };

            _mockQuery.Setup(q => q.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
                      .ReturnsAsync(1);

            // Act
            await _calculatorService.SaveCalculationAsync(calculationHistory);

            // Assert
            _mockQuery.Verify(q => q.CreateOrUpdateAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, CommandType.StoredProcedure), Times.Once);
        }
    }


}
