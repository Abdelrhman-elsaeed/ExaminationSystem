namespace ExaminationSystem.Models
{
    public class Course
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Hours { get; set; }

        public bool Deleted { get; set; } = false;
    }
}
