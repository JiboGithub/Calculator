using Calculator.Application.Contract;
using Calculator.Domain.Entities;
using Calculator.Domain.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST api/users 
        [HttpPost("CreateUser")]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser()
        {
            var response = await _userService.CreateUserAsync();
            return StatusCode(response.ResponseCode, response);
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OperationResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(OperationResponse))]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUser(int userId)
        {
            var response = await _userService.GetUserById(userId);
            return StatusCode(response.ResponseCode, response);
        }
    }
}
