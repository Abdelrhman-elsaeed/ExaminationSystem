using ExaminationSystem.Enums;

namespace ExaminationSystem.ViewModels
{
    public record ResponseViewModel<T>(T? Data, bool IsSuccess, string Message, ErrorCode ErrorCode)
    {
        public static ResponseViewModel<T> Success(T data, ErrorCode errorCode = ErrorCode.None, string message = "")
        {
            return new ResponseViewModel<T>(data, true, message, errorCode);
        }

        public static ResponseViewModel<T> Failure(ErrorCode errorCode, string message = "")
        {
            return new ResponseViewModel<T>(default, false, message, errorCode);
        }
    }
}
