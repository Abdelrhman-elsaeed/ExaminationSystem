namespace ExaminationSystem.DAL.Enums
{
    public enum ErrorCode
    {
        None = 0,
        UnexpectedError=1,

        QustionNotFound = 100,
        QuestionUpdateFail = 101,
        QuestionAddFail = 102,
        QuestionDeleteFail = 103,
        QuestoinChoicesTransactionFail = 104,
        SaveQuestionFail = 105,

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
        SaveExamFail = 316,


        InvalidCredentials = 400,
        UserNotFound = 401,
        AssignFeatureToRoleFail = 402,
        InvalidRole = 403,
        InvalidFeature = 404,
        FeatureAlreadyAssignedToRole = 405,
        AccessDenied = 406,
        EmailAlreadyRegistered=407,
        UsernameAlreadyRegistered=408,
        RegisterationFail=409,
        RoleNotFound=410,
        RoleAssignedBefore=411,
        AddRoleFail=412,



        AddUserFail = 500,
        UpdateUserFail = 501,
        DeleteUserFail = 502,

        AddCourseFail=600,
        UpdateCourseFail=601,
        DeleteCourseFail=602,
        SaveCourseFail=603,


        StudentNotExist=700,


        AssignStudentToCoursefail=800,
        StudentAssignedBefore=801,
        StudentNotAssignedToCourse=802,
        DeleteStudentFromCourseFail=803,
    }
}
