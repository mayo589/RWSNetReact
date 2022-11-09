using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Api;
using TranslationManagement.Api.Repositories;

namespace TranslationManagement.Tests
{
    public class BaseTest
    {
        public readonly TranslatorRepository TranslatorRepository;
        public readonly TranslationJobRepository TranslationJobRepository;

        public BaseTest()
        {
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                     .UseInMemoryDatabase(databaseName: "MyBlogDb")
                     .Options;

            AppDbContext dbContext = new AppDbContext(dbContextOptions);
            TranslatorRepository = new TranslatorRepository(dbContext);
            TranslationJobRepository = new TranslationJobRepository(dbContext);

        }
    }
}
