namespace ExaminationSystem.Enums
{
    public enum ErrorCode
    {
        None = 0,

        QustionNotFound = 100,
        QuestionUpdateFail = 101,
        QuestionAddFail = 102,
        QuestionDeleteFail = 103,
        QuestoinChoicesTransactionFail = 104,

        ChoiceUpdateFail = 200,
        ChoiceDeleteFail = 201,
        ChoiceNotFound = 202,

        InvalidExamInput = 300,
        CourseNotFound = 301,
        InstructorNotFound = 302,
        ExamAddFail = 303,
        ExamNotFound = 304,
        ExamDeleteFail = 305,
        ExamUpdateFail = 306,
        QuestionAlreadyAssignedToExam = 307,
        AssignQuestionToExamFail = 308,
        ExamQuestionRecordNotFound = 309,
        DeleteQuestionFromExamFail = 310,
        StudentAlreadyAssignedToExam = 311,
        AssignStudentToExamFail = 312,
        StudentNotAssignedToExam = 313,
        NoQuestionsAssignedToExam = 314,
        SubmitExamFail = 315,


        InvalidCredentials=400,
        UserNotFound=401,
        AssignFeatureToRoleFail=402,
        InvalidRole=403,
        InvalidFeature=404,
        FeatureAlreadyAssignedToRole=405,



        AddUserFail=500,
        UpdateUserFail=501,
        DeleteUserFail=502
    }
}
