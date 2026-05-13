using ExaminationSystem.DAL.Enums;

namespace ExaminationSystem.BLL.Helper.BusinessExceptions
{
    public class BusinessException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public BusinessException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
