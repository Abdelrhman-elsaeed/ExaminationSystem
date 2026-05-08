namespace ExaminationSystem.BLL.DTOs.Choice
{
    public class UpdateChoiceDTO
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public bool IsCorrectChoice { get; set; }
    }
}
