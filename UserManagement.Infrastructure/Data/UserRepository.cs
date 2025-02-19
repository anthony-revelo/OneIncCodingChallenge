using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using UserManagement.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace UserManagement.Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseClient _databaseClient;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IDatabaseClient databaseClient, ILogger<UserRepository> logger)
        {
            _databaseClient = databaseClient;
            _logger = logger;
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                var users = new List<User>();

                using (var connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Users", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Email = reader.GetString(3),
                                DateOfBirth = reader.GetDateTime(4),
                                PhoneNumber = reader.GetString(5)
                            });
                        }
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                throw;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                User user = null;

                using (var connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Email = reader.GetString(3),
                                DateOfBirth = reader.GetDateTime(4),
                                PhoneNumber = reader.GetString(5)
                            };
                        }
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user with ID {id}");
                throw;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                using (var connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "INSERT INTO Users (FirstName, LastName, Email, DateOfBirth, PhoneNumber) " +
                        "VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @PhoneNumber)", connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new user");
                throw;
            }
        }

        public void UpdateUser(User user)
        {
            try
            {
                using (var connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    var command = new SqlCommand(
                        "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
                        "DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID {user.Id}");
                throw;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                using (var connection = _databaseClient.CreateConnection())
                {
                    connection.Open();
                    var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}");
                throw;
            }
        }
    }
}
