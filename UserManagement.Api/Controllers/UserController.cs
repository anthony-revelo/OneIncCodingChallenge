using Microsoft.AspNetCore.Mvc;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using FluentValidation;

namespace UserManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IValidator<User> _validator;

        public UsersController(IUserService userService, ILogger<UsersController> logger, IValidator<User> validator)
        {
            _userService = userService;
            _logger = logger;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var users = _userService.GetAllUsers().Select(user => new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.DateOfBirth,
                    Age = CalculateAge(user.DateOfBirth),
                    user.PhoneNumber
                });
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                _logger.LogInformation($"Getting user with ID {id}");
                var user = _userService.GetUserById(id);
                if (user == null) return NotFound();
                return Ok(new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.DateOfBirth,
                    Age = CalculateAge(user.DateOfBirth),
                    user.PhoneNumber
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            try
            {
                _logger.LogInformation("Adding new user");

                var validationResult = _validator.Validate(user);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                _userService.AddUser(user);
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            try
            {
                _logger.LogInformation($"Updating user with ID {id}");

                if (_userService.GetUserById(id) == null) return NotFound();

                user.Id = id;
                var validationResult = _validator.Validate(user);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                _userService.UpdateUser(user);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting user with ID {id}");
                if (_userService.GetUserById(id) == null) return NotFound();
                _userService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
}
