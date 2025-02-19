using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.ComponentModel.DataAnnotations;
using UserManagement.Api.Controllers;
using UserManagement.Application.Interfaces;
using UserManagement.Core.Entities;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace UserManagement.Api.Tests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UsersController>> _loggerMock;
        private readonly Mock<IValidator<User>> _validatorMock;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UsersController>>();
            _validatorMock = new Mock<IValidator<User>>();
            _controller = new UsersController(_userServiceMock.Object, _loggerMock.Object, _validatorMock.Object);
        }

        [Fact]
        public void GetAllUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    DateOfBirth = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567890"
                }
            };
            _userServiceMock.Setup(service => service.GetAllUsers()).Returns(users);

            // Act
            var result = _controller.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsType<List<object>>(okResult.Value);
            Assert.Single(returnUsers);
        }

        [Fact]
        public void GetUserById_UserExists_ReturnsOkResult_WithUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "1234567890"
            };
            _userServiceMock.Setup(service => service.GetUserById(user.Id)).Returns(user);

            // Act
            var result = _controller.GetUserById(user.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<object>(okResult.Value);
        }

        [Fact]
        public void AddUser_ValidUser_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@example.com",
                DateOfBirth = new DateTime(2000, 1, 1),
                PhoneNumber = "1234567890"
            };
            _validatorMock.Setup(v => v.Validate(user)).Returns(new ValidationResult(new List<ValidationFailure>()));

            // Act
            var result = _controller.AddUser(user);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnUser = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal(user.Id, returnUser.Id);
        }

        [Fact]
        public void UpdateUser_ValidUser_ReturnsNoContentResult()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "janedoe@example.com",
                DateOfBirth = new DateTime(1995, 1, 1),
                PhoneNumber = "0987654321"
            };
            _userServiceMock.Setup(service => service.GetUserById(user.Id)).Returns(user);
            _validatorMock.Setup(v => v.Validate(user)).Returns(new ValidationResult(new List<ValidationFailure>()));

            // Act
            var result = _controller.UpdateUser(user.Id, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteUser_UserExists_ReturnsNoContentResult()
        {
            // Arrange
            var userId = 1;
            _userServiceMock.Setup(service => service.GetUserById(userId)).Returns(new User
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                Email = "janedoe@example.com",
                DateOfBirth = new DateTime(1995, 1, 1),
                PhoneNumber = "0987654321"
            });

            // Act
            var result = _controller.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}