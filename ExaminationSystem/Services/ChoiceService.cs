using ExaminationSystem.Models;
using ExaminationSystem.Repo;

namespace ExaminationSystem.Services
{
    public class ChoiceService
    {
        public readonly GenericRepository<Choice> _ChoiceRepo;

        public ChoiceService(GenericRepository<Choice> ChoiceRepo)
        {
            _ChoiceRepo = ChoiceRepo;
        }


        public async Task<bool> DeleteByQuestionIdAsync(int id)
        {
            var result = await _ChoiceRepo.DeleteRangeAsync(q=>q.ID==id);

            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
