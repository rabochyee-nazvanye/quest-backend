using System;
using System.Collections.Generic;
using System.Text;

namespace Quest.Application
{

    public class BaseResponse<T>
    {
        public BaseResponse(T result, string message)
        {
            Result = result;
            Message = message;
        }

        public T Result { get; set; }
        public string Message { get; set; }
    }

    public static class BaseResponse
    {
        public static BaseResponse<T> Success<T>(T result, string message)
        {
            return new BaseResponse<T>(result, message);
        }

        public static BaseResponse<T> Success<T>(T result)
        {
            return new BaseResponse<T>(result, "");
        }
        public static BaseResponse<T> Failure<T>(string message)
        {
            return new BaseResponse<T>(default, message);
        }
    }
}

