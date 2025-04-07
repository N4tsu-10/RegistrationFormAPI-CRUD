using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationFormAPI.Models;

namespace RegistrationFormAPI.DataAccessLayer.Interfaces
{
    
    /// Interface for user data access operations
    
    public interface IUserRepository
    {
        
        /// Creates a new user in the database
       
        /// <param name="fullName">Full name of the user</param>
        /// <param name="email">Email address of the user</param>
        /// <param name="phone">Phone number of the user</param>
        /// <param name="hashedPassword">SHA256 hashed password</param>
        /// <returns>A task representing the asynchronous operation with the result containing success status, message, and user ID if successful</returns>
        Task<(bool Success, string Message, int? UserId)> CreateUserAsync(string fullName, string email, string phone, string hashedPassword);

      
        /// Gets all users from the database
        
        /// <returns>A task representing the asynchronous operation with the list of users</returns>
        Task<(bool Success, string Message, IEnumerable<User>? Users)> GetAllUsersAsync();

        
        /// Gets a user by their ID
      
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>A task representing the asynchronous operation with the user if found</returns>
        Task<(bool Success, string Message, User? User)> GetUserByIdAsync(int id);

        
        /// Updates an existing user
        
        /// <param name="id">The ID of the user to update</param>
        /// <param name="fullName">Full name of the user</param>
        /// <param name="email">Email address of the user</param>
        /// <param name="phone">Phone number of the user</param>
        /// <param name="hashedPassword">SHA256 hashed password (optional)</param>
        /// <returns>A task representing the asynchronous operation with success status and message</returns>
        Task<(bool Success, string Message)> UpdateUserAsync(int id, string fullName, string email, string phone, string? hashedPassword = null);

        
        /// Deletes a user by their ID
       
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>A task representing the asynchronous operation with success status and message</returns>
        Task<(bool Success, string Message)> DeleteUserAsync(int id);
    }
}