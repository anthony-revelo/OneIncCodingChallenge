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
        public async void GetAllUsers_ReturnsAllUsers()
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
                    Age = 25,
                    DateOfBirth = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567890"
                }
            };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            Assert.Equal(users, result);
        }

        [Fact]
        public async void GetUserById_UserExists_ReturnsUser()
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
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            var result = await _userService.GetUserByIdAsync(user.Id);

            // Assert
            Assert.Equal(user, result);
        }

        [Fact]
        public async void AddUser_ValidUser_CallsRepository()
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
            await _userService.AddUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.AddUserAsync(user), Times.Once);
        }

        [Fact]
        public async void UpdateUser_ValidUser_CallsRepository()
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

            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(user.Id)).ReturnsAsync(user);

            // Act
            await _userService.UpdateUserAsync(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.UpdateUserAsync(user), Times.Once);
        }

        [Fact]
        public async void DeleteUser_UserExists_CallsRepository()
        {
            // Arrange
            var userId = 1;

            // Act
            await _userService.DeleteUserAsync(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.DeleteUserAsync(userId), Times.Once);
        }
    }
}
