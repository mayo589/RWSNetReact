using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Repositories
{
    public class TranslatorRepository : RepositoryBase<Translator>
    {
        public TranslatorRepository(AppDbContext repositoryContext)
           : base(repositoryContext)
        {
        }
    }
}
