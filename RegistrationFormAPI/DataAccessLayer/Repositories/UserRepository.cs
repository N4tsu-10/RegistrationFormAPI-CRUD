using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using RegistrationFormAPI.DataAccessLayer.Interfaces;
using RegistrationFormAPI.Models;

namespace RegistrationFormAPI.DataAccessLayer.Repositories
{
    
    /// Repository for user-related database operations using PostgreSQL stored procedures
    
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

       
        /// Initializes a new instance of the UserRepository
       
        /// <param name="configuration">Application configuration</param>
        /// <param name="logger">Logger instance</param>
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("PostgreSQL")
                ?? throw new ArgumentNullException(nameof(configuration), "PostgreSQL connection string is not configured");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        
        /// Creates a new user in the database
  
        public async Task<(bool Success, string Message, int? UserId)> CreateUserAsync(string fullName, string email, string phone, string hashedPassword)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT create_user(@fullName, @email, @phone, @password)", connection);
                command.Parameters.AddWithValue("@fullName", fullName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@password", hashedPassword);

                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return (false, "Failed to create user: No result returned from database", null);
                }

                var jsonResult = JsonDocument.Parse(result.ToString()!);
                var success = jsonResult.RootElement.GetProperty("success").GetBoolean();
                var message = jsonResult.RootElement.GetProperty("message").GetString() ?? "Unknown error";

                int? userId = null;
                if (success && jsonResult.RootElement.TryGetProperty("data", out var dataElement) &&
                    !dataElement.ValueKind.Equals(JsonValueKind.Null))
                {
                    userId = dataElement.GetProperty("id").GetInt32();
                }

                return (success, message, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return (false, $"An error occurred while creating the user: {ex.Message}", null);
            }
        }

        
        /// Gets all users from the database
       
        public async Task<(bool Success, string Message, IEnumerable<User>? Users)> GetAllUsersAsync()
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT get_all_users()", connection);
                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return (false, "Failed to get users: No result returned from database", null);
                }

                var jsonResult = JsonDocument.Parse(result.ToString()!);
                var success = jsonResult.RootElement.GetProperty("success").GetBoolean();
                var message = jsonResult.RootElement.GetProperty("message").GetString() ?? "Unknown error";

                if (!success)
                {
                    return (success, message, null);
                }

                var users = new List<User>();
                var dataElement = jsonResult.RootElement.GetProperty("data");

                if (dataElement.ValueKind != JsonValueKind.Null)
                {
                    foreach (var userElement in dataElement.EnumerateArray())
                    {
                        var user = new User
                        {
                            Id = userElement.GetProperty("id").GetInt32(),
                            FullName = userElement.GetProperty("fullName").GetString() ?? string.Empty,
                            Email = userElement.GetProperty("email").GetString() ?? string.Empty,
                            Phone = userElement.GetProperty("phone").GetString() ?? string.Empty,
                            CreatedAt = userElement.GetProperty("createdAt").GetDateTime()
                        };

                        users.Add(user);
                    }
                }

                return (success, message, users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return (false, $"An error occurred while retrieving users: {ex.Message}", null);
            }
        }

        
        /// Gets a user by their ID
       
        public async Task<(bool Success, string Message, User? User)> GetUserByIdAsync(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT get_user_by_id(@id)", connection);
                command.Parameters.AddWithValue("@id", id);

                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return (false, "Failed to get user: No result returned from database", null);
                }

                var jsonResult = JsonDocument.Parse(result.ToString()!);
                var success = jsonResult.RootElement.GetProperty("success").GetBoolean();
                var message = jsonResult.RootElement.GetProperty("message").GetString() ?? "Unknown error";

                if (!success)
                {
                    return (success, message, null);
                }

                var dataElement = jsonResult.RootElement.GetProperty("data");
                if (dataElement.ValueKind == JsonValueKind.Null)
                {
                    return (false, "User not found", null);
                }

                var user = new User
                {
                    Id = dataElement.GetProperty("id").GetInt32(),
                    FullName = dataElement.GetProperty("fullName").GetString() ?? string.Empty,
                    Email = dataElement.GetProperty("email").GetString() ?? string.Empty,
                    Phone = dataElement.GetProperty("phone").GetString() ?? string.Empty,
                    CreatedAt = dataElement.GetProperty("createdAt").GetDateTime()
                };

                return (success, message, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", id);
                return (false, $"An error occurred while retrieving the user: {ex.Message}", null);
            }
        }

        
        /// Updates an existing user
       
        public async Task<(bool Success, string Message)> UpdateUserAsync(
            int id, string fullName, string email, string phone, string? hashedPassword = null)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT update_user(@id, @fullName, @email, @phone, @password)", connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@fullName", fullName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phone", phone);
                command.Parameters.AddWithValue("@password", hashedPassword == null ? DBNull.Value : hashedPassword);

                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return (false, "Failed to update user: No result returned from database");
                }

                var jsonResult = JsonDocument.Parse(result.ToString()!);
                var success = jsonResult.RootElement.GetProperty("success").GetBoolean();
                var message = jsonResult.RootElement.GetProperty("message").GetString() ?? "Unknown error";

                return (success, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", id);
                return (false, $"An error occurred while updating the user: {ex.Message}");
            }
        }

        
        /// Deletes a user by their ID
       
        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new NpgsqlCommand("SELECT delete_user(@id)", connection);
                command.Parameters.AddWithValue("@id", id);

                var result = await command.ExecuteScalarAsync();
                if (result == null)
                {
                    return (false, "Failed to delete user: No result returned from database");
                }

                var jsonResult = JsonDocument.Parse(result.ToString()!);
                var success = jsonResult.RootElement.GetProperty("success").GetBoolean();
                var message = jsonResult.RootElement.GetProperty("message").GetString() ?? "Unknown error";

                return (success, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                return (false, $"An error occurred while deleting the user: {ex.Message}");
            }
        }
    }
}