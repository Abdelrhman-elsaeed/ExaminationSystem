using ExaminationSystem.BLL.DTOs.Choice;
using ExaminationSystem.BLL.Services.Interfaces;
using ExaminationSystem.BLL.AutoMapper;
using ExaminationSystem.DAL.Models;
using System.Threading;
using System.Threading.Tasks;

namespace ExaminationSystem.BLL.Services.Implementaiton
{
    public class ChoiceService : IChoiceService
    {
        public readonly IRepository<Choice> _ChoiceRepo;

        public ChoiceService(IRepository<Choice> ChoiceRepo)
        {
            _ChoiceRepo = ChoiceRepo;
        }

        public async Task<bool> DeleteByQuestionIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var choices = await _ChoiceRepo.GetAllByConditionAsync(q => q.QuestionId == id, cancellationToken);
            if (choices == null || choices.Count == 0)
                return false;

            _ChoiceRepo.DeleteRange(choices);
            var result = await _ChoiceRepo.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task<bool> UpdateChoiceAsync(UpdateChoiceDTO model, CancellationToken cancellationToken = default)
        {
            var updateChoice = model.Map<Choice>();

            _ChoiceRepo.UpdateInclude(
                updateChoice,
                nameof(Choice.Text),
                nameof(Choice.IsCorrectChoice));
            
            return await _ChoiceRepo.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _ChoiceRepo.CheckExistsByConditionAsync(x => x.ID == id && !x.Deleted, cancellationToken);
        }
    }
}
