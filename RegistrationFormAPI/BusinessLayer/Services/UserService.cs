using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RegistrationFormAPI.BusinessLayer.Interfaces;
using RegistrationFormAPI.DataAccessLayer.Interfaces;
using RegistrationFormAPI.DTOs;
using RegistrationFormAPI.Models;
using RegistrationFormAPI.Utils;

namespace RegistrationFormAPI.BusinessLayer.Services
{
    
    /// Service for user-related business operations

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

    
        /// Initializes a new instance of the UserService
       
        /// <param name="userRepository">The user repository</param>
        /// <param name="logger">Logger instance</param>
        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        /// Creates a new user
    
        public async Task<ApiResponse<int>> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(createUserDto.FullName) ||
                    string.IsNullOrWhiteSpace(createUserDto.Email) ||
                    string.IsNullOrWhiteSpace(createUserDto.Phone) ||
                    string.IsNullOrWhiteSpace(createUserDto.Password))
                {
                    return ApiResponse<int>.ErrorResponse("All fields are required");
                }

                // Hash the password
                string hashedPassword = PasswordHasher.HashPassword(createUserDto.Password);

                // Call repository to create user
                var (success, message, userId) = await _userRepository.CreateUserAsync(
                    createUserDto.FullName,
                    createUserDto.Email,
                    createUserDto.Phone,
                    hashedPassword);

                if (!success || userId == null)
                {
                    return ApiResponse<int>.ErrorResponse(message);
                }

                return ApiResponse<int>.SuccessResponse(message, userId.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return ApiResponse<int>.ErrorResponse($"An error occurred: {ex.Message}");
            }
        }

        
        /// Gets all users
        
        public async Task<ApiResponse<IEnumerable<UserResponseDto>>> GetAllUsersAsync()
        {
            try
            {
                var (success, message, users) = await _userRepository.GetAllUsersAsync();

                if (!success || users == null)
                {
                    return ApiResponse<IEnumerable<UserResponseDto>>.ErrorResponse(message);
                }

                var userDtos = users.Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    CreatedAt = u.CreatedAt
                });

                return ApiResponse<IEnumerable<UserResponseDto>>.SuccessResponse(message, userDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return ApiResponse<IEnumerable<UserResponseDto>>.ErrorResponse($"An error occurred: {ex.Message}");
            }
        }

        
        /// Gets a user by their ID
        
        public async Task<ApiResponse<UserResponseDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var (success, message, user) = await _userRepository.GetUserByIdAsync(id);

                if (!success || user == null)
                {
                    return ApiResponse<UserResponseDto>.ErrorResponse(message);
                }

                var userDto = new UserResponseDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    CreatedAt = user.CreatedAt
                };

                return ApiResponse<UserResponseDto>.SuccessResponse(message, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", id);
                return ApiResponse<UserResponseDto>.ErrorResponse($"An error occurred: {ex.Message}");
            }
        }

       
        /// Updates an existing user
      
        public async Task<ApiResponse<object>> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(updateUserDto.FullName) ||
                    string.IsNullOrWhiteSpace(updateUserDto.Email) ||
                    string.IsNullOrWhiteSpace(updateUserDto.Phone))
                {
                    return ApiResponse<object>.ErrorResponse("FullName, Email, and Phone are required");
                }

                // Check if user exists
                var (userSuccess, _, _) = await _userRepository.GetUserByIdAsync(id);
                if (!userSuccess)
                {
                    return ApiResponse<object>.ErrorResponse($"User with ID {id} not found");
                }

                // Hash password if provided
                string? hashedPassword = null;
                if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
                {
                    hashedPassword = PasswordHasher.HashPassword(updateUserDto.Password);
                }

                // Call repository to update user
                var (success, message) = await _userRepository.UpdateUserAsync(
                    id,
                    updateUserDto.FullName,
                    updateUserDto.Email,
                    updateUserDto.Phone,
                    hashedPassword);

                if (!success)
                {
                    return ApiResponse<object>.ErrorResponse(message);
                }

                return ApiResponse<object>.SuccessResponse(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user: {UserId}", id);
                return ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}");
            }
        }

   
        /// Deletes a user by their ID
       
        public async Task<ApiResponse<object>> DeleteUserAsync(int id)
        {
            try
            {
                // Check if user exists
                var (userSuccess, _, _) = await _userRepository.GetUserByIdAsync(id);
                if (!userSuccess)
                {
                    return ApiResponse<object>.ErrorResponse($"User with ID {id} not found");
                }

                // Call repository to delete user
                var (success, message) = await _userRepository.DeleteUserAsync(id);

                if (!success)
                {
                    return ApiResponse<object>.ErrorResponse(message);
                }

                return ApiResponse<object>.SuccessResponse(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", id);
                return ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}");
            }
        }
    }
}