using Moq;
using UserManagement.Application.Services;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;

namespace UserManagement.Application.Tests
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Fact]
        public void GetAllUsers_ReturnsAllUsers()
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
            _userRepositoryMock.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public void GetUserById_UserExists_ReturnsUser()
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
            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).Returns(user);

            // Act
            var result = _userService.GetUserById(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public void AddUser_ValidUser_CallsRepository()
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

            // Act
            _userService.AddUser(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddUser(user), Times.Once);
        }

        [Fact]
        public void UpdateUser_ValidUser_CallsRepository()
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

            _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).Returns(user);

            // Act
            _userService.UpdateUser(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateUser(user), Times.Once);
        }

        [Fact]
        public void DeleteUser_UserExists_CallsRepository()
        {
            // Arrange
            var userId = 1;

            // Act
            _userService.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteUser(userId), Times.Once);
        }
    }
}
