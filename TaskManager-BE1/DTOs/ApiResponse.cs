namespace TaskManager_BE1.DTOs
{

    public class ApiResponse<T>
    {
        public bool Status { get; set; }  // "success" or "error"
        public string Message { get; set; }
        public T Data { get; set; }
        public List<ApiError> Errors { get; set; } // Optional, for validation or business errors
        public Meta Meta { get; set; } // Optional, for additional information like pagination, timestamps
    }

    public class ApiError
    {
        public string Field { get; set; } // Field name that caused the error
        public string Message { get; set; } // Error message for that field
    }
    public class Meta
    {
        public string Timestamp { get; set; } // Timestamp when the response was generated
        public int Page { get; set; } // Pagination data (optional)
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }



    public class ApiResponseHelper
    {
        public static ApiResponse<T> Success<T>(T data, string message = "Request was successful")
        {
            return new ApiResponse<T>
            {
                Status = true,
                Message = message,
                Data = data,
                Errors = null,
                Meta = new Meta { Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };
        }


        public static ApiResponse<T> Error<T>(List<ApiError> errors, string message = "Validation failed")
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Data = default,
                Errors = errors,
                Meta = new Meta { Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };
        }

        public static ApiResponse<T> Failure<T>(string message)
        {
            return new ApiResponse<T>
            {
                Status = false,
                Message = message,
                Data = default,
                Errors = null,
                Meta = new Meta { Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };
        }
    }
}