using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationFormAPI.DTOs
{
 
    /// DTO for creating a new user
   
    public class CreateUserDto
    {
     
        /// Full name of the user
     
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

        
        /// Email address of the user (must be unique)
   
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

  
        /// Phone number of the user
   
        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

  
        /// Password for the user account
    
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }


    /// DTO for updating an existing user

    public class UpdateUserDto
    {

        /// Full name of the user
   
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; } = string.Empty;

     
        /// Email address of the user (must be unique)
 
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

       
        /// Phone number of the user
 
        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

      
        /// New password (optional when updating)
     
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }
    }


    /// DTO for user information returned to clients
  
    public class UserResponseDto
    {
    
        /// Unique identifier for the user
      
        public int Id { get; set; }

  
        /// Full name of the user
    
        public string FullName { get; set; } = string.Empty;


        /// Email address of the user
   
        public string Email { get; set; } = string.Empty;

    
        /// Phone number of the user
    
        public string Phone { get; set; } = string.Empty;

   
        /// Date and time when the user was created
     
        public DateTime CreatedAt { get; set; }
    }
}