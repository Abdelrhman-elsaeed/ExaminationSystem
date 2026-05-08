namespace ExaminationSystem.Enums
{
    public enum Feature
    {
        // Question Controller Features
        AddQuestion,
        DeleteQuestion,
        GetAllQuestions,
        UpdateQuestion,
        UpdateChoice,

        // Exam Controller Features
        AddExam,
        AssignStudentToExam,
        AssignQuestionToExam,
        UpdateExam,
        DeleteExam,
        UpdateQuestionOnExam,
        DeleteQuestionFromExam,
        ViewExam,
        SubmitExam,
        CreateRandomExam,
        ViewStudentsGrades,
        TopGrade,
        AverageGrade
    }
}
