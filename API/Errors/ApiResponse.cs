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
                400 => "You made a Bad Request !",
                401 => "You are unauthorized to go there !!!",
                404 => "Not Found",
                500 => "INTERANL SERVER ERROR: something has gone wrong on the web site's server but the server could not be more specific on what the exact problem is.",
                _ => "I don't know what is the error but also حصل خطأ بالمخدم"
            };
        }
    }
}