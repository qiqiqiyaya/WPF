using System;

namespace Practice.Common
{
    public class Result<T>
    {
        public T Data { get; set; }

        public Exception? Exception { get; protected set; }

        public bool HasException => Exception != null;

        public Result()
        {
            Data = default!;
            Exception = null!;
        }

        public Result(Exception exception)
        {
            Data = default!;
            Exception = exception;
        }

        public Result(T data)
        {
            Data = data;
            Exception = null;
        }

        public void SetException(Exception ex)
        {
            Data = default!;
            Exception = ex;
        }
    }
}
