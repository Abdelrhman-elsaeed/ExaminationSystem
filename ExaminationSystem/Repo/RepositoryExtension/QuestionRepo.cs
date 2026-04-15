using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Repo.RepositoryExtension
{
    public class QuestionRepo : GenericRepository<Question>
    {
        public async Task<bool> DeleteQuestionAndChoicesAsync(int id)
        {
            //start Transaction
            await using var DeleteTransaction = await _Context.Database.BeginTransactionAsync();

            try
            {
                // Delete Question
                var QuestionResult = await base.DeleteAsync(id);
                if (!QuestionResult)
                    return false;

                //Delete Question Choices
                var ChoicesResult = await _Context.Set<Choice>()
                    .Where(c => c.QuestionId == id && !c.Deleted)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.Deleted, true));

                //commit Transaction
                await DeleteTransaction.CommitAsync();
                return true;

            }
            catch
            {
                await DeleteTransaction.RollbackAsync();
                throw;

            }
        }


    }
}
