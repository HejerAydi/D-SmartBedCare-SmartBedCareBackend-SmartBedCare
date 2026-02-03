using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ApiResponse<T>
    {
        public string message { get; set; }
        public T data { get; set; }
        public ApiResponse(string _message, T _data)
        {
            message = _message;
            data = _data;
        }
    }
}
