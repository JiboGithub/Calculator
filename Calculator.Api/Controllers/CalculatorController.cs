using Calculator.Application.Contract;
using Calculator.Application.Dtos.Request;
using Calculator.Application.Filter;
using Calculator.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Api.Controllers
{
    [ApiController]
    [Route("api/calculator")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        // POST api/calculator/perform-calculation
        [HttpPost("perform-calculation")]
        [ServiceFilter(typeof(ValidationFilter))]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PerformCalculation(CalculationRequest request)
        {
            var response = await _calculatorService.PerformCalculationAsync(request);
            return StatusCode(response.ResponseCode, response);
        }

        // GET api/calculator/history/{userId}
        [HttpGet("history/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResponse))]
        [ProducesResponseType(typeof(CalculationHistoryResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCalculationHistory(int userId)
        {
            var response = await _calculatorService.GetCalculationHistoryByUserIdAsync(userId);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
