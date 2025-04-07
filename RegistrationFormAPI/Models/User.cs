using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationFormAPI.Models
{
    
    /// Represents a user in the system

    public class User
    {

        /// Unique identifier for the user
    
        public int Id { get; set; }

      
        /// Full name of the user
        
        [Required]
        [StringLength(100)]
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


        /// Hashed password of the user (using SHA256)
     
        [Required]
        [StringLength(64)]
        public string Password { get; set; } = string.Empty;

      
        /// Date and time when the user was created

        public DateTime CreatedAt { get; set; }
    }
}