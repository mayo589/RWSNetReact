using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Models;

namespace TranslationManagement.Api.Repositories
{
    public class TranslationJobRepository : RepositoryBase<TranslationJob>
    {
        private readonly AppDbContext repositoryContext;

        public TranslationJobRepository(AppDbContext repositoryContext)
           : base(repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }


        public List<TranslationJob> FindAll()
        {
            var res = repositoryContext
                .TranslationJobs
                .Include(j => j.Translator)
                .ToList();

            return res;
        }
    }
}
