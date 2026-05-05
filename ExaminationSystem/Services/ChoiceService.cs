using ExaminationSystem.Helper.AutoMapper;
using ExaminationSystem.ModelDTO.Choice;
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

        //you dont have to make this RespnseViewModel Because on our business we will not use this action on Controller
        public async Task<bool> UpdateChoiceAsync(UpdateChoiceDTO model)
        {
            var updateChoice = model.Map<Choice>();

            return await _ChoiceRepo.UpdateInclude(
                updateChoice,
                nameof(Choice.Text),
                nameof(Choice.IsCorrectChoice));
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _ChoiceRepo.AnyAsync(x => x.ID == id && !x.Deleted);
        }
    }
}
