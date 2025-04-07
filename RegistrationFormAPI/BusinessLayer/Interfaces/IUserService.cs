using System.Collections.Generic;
using System.Threading.Tasks;
using RegistrationFormAPI.DTOs;
using RegistrationFormAPI.Models;

namespace RegistrationFormAPI.BusinessLayer.Interfaces
{
   
    /// Interface for user-related business operations
    
    public interface IUserService
    {
        
        /// Creates a new user
        
        /// <param name="createUserDto">The user data transfer object containing the information for the new user</param>
        /// <returns>API response with the created user's ID</returns>
        Task<ApiResponse<int>> CreateUserAsync(CreateUserDto createUserDto);

        
        /// Gets all users
        
        /// <returns>API response with a collection of user response DTOs</returns>
        Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync();

        
        /// Gets a user by their ID
    
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>API response with the user response DTO if found</returns>
        Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id);

       
        /// Updates an existing user
      
        /// <param name="id">The ID of the user to update</param>
        /// <param name="updateUserDto">The user data transfer object containing the updated information</param>
        /// <returns>API response indicating success or failure</returns>
        Task<ApiResponse<object>> UpdateUserAsync(int id, UpdateUserDto updateUserDto);

    
        /// Deletes a user by their ID
   
        /// <param name="id">The ID of the user to delete</param>
        /// <returns>API response indicating success or failure</returns>
        Task<ApiResponse<object>> DeleteUserAsync(int id);
    }
}