using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationFormAPI.BusinessLayer.Interfaces;
using RegistrationFormAPI.DTOs;
using RegistrationFormAPI.Models;

namespace RegistrationFormAPI.Controllers
{
   
    /// Controller for managing user operations
 
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

      
        /// Initializes a new instance of the UsersController
        
        /// <param name="userService">The user service</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

       
        /// Creates a new user
      
        /// <param name="createUserDto">User information</param>
        /// <returns>API response with created user ID</returns>
        /// <response code="201">Returns the newly created user ID</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<int>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var response = await _userService.CreateUserAsync(createUserDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = response.Data }, response);
        }

      
        /// Gets all users
       
        /// <returns>API response with list of all users</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="500">If there was an internal error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponseDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _userService.GetAllUsersAsync();

            if (!response.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

   
        /// Gets a user by ID
        
        /// <param name="id">User ID</param>
        /// <returns>API response with the user</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there was an internal error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _userService.GetUserByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

    
        /// Updates a user

        /// <param name="id">User ID</param>
        /// <param name="updateUserDto">Updated user information</param>
        /// <returns>API response indicating success or failure</returns>
        /// <response code="200">If the user was updated successfully</response>
        /// <response code="400">If the input is invalid</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there was an internal error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUserAsync(id, updateUserDto);

            if (!response.Success)
            {
                if (response.Message.Contains("not found"))
                {
                    return NotFound(response);
                }
                return BadRequest(response);
            }

            return Ok(response);
        }

       
        /// Deletes a user
       
        /// <param name="id">User ID</param>
        /// <returns>API response indicating success or failure</returns>
        /// <response code="200">If the user was deleted successfully</response>
        /// <response code="404">If the user is not found</response>
        /// <response code="500">If there was an internal error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}