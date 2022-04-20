using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDeafultMessageByStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
        
        private string GetDeafultMessageByStatusCode(int statusCode)
        {
            return statusCode switch{
                400 => "حصل خطأ بالمخدم",
                401 => "حصل خطأ بالمخدم",
                404 => "حصل خطأ بالمخدم",
                500 => "حصل خطأ بالمخدم",
                _ => "I don't know what is the error but also حصل خطأ بالمخدم"
            };
        }
    }
}