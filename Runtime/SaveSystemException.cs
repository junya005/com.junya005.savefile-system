using System;

namespace junya005.SaveFileSystem
{
    public class SaveSystemException : Exception
    {
        public SaveSystemErrorCode ErrorCode { get; }

        public SaveSystemException(SaveSystemErrorCode errorCode, string message, Exception innerException = null)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
