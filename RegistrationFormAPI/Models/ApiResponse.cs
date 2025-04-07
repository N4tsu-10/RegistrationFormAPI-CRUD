namespace RegistrationFormAPI.Models
{

    /// Standard response format for all API endpoints
 
    /// <typeparam name="T">Type of data returned</typeparam>
    public class ApiResponse<T>
    {
        
        /// Indicates if the operation was successful
     
        public bool Success { get; set; }

        
        /// Message describing the result of the operation
     
        public string Message { get; set; } = string.Empty;


        /// Data returned by the operation (if applicable)
     
        public T? Data { get; set; }

       
        /// Creates a successful response with data
        
        public static ApiResponse<T> SuccessResponse(string message, T data)
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = data };
        }

       
        /// Creates a successful response without data
       
        public static ApiResponse<T> SuccessResponse(string message)
        {
            return new ApiResponse<T> { Success = true, Message = message, Data = default };
        }

        
        /// Creates an error response
      
        public static ApiResponse<T> ErrorResponse(string message)
        {
            return new ApiResponse<T> { Success = false, Message = message, Data = default };
        }
    }
}