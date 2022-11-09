using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace TranslationManagement.Tests
{
    [TestClass]
    public class TranslatorTests : BaseTest
    {
        [TestMethod]
        public async Task TestCreate()
        {
            TranslatorRepository.Create(new Api.Models.Translator()
            {
                Name = "TEST-CREATE",
                CreditCardNumber = "123",
                HourlyRate = 500
            });

            await TranslatorRepository.SaveChangesAsync();

            var foundTranslators = TranslatorRepository.FindByCondition(tr=> tr.Name== "TEST-CREATE" && tr.CreditCardNumber == "123" & tr.HourlyRate == 500);

            Assert.IsNotNull(foundTranslators.Any());
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            TranslatorRepository.Create(new Api.Models.Translator()
            {
                Name = "TEST-UPDATE",
                CreditCardNumber = "123",
                HourlyRate = 500
            });

            await TranslatorRepository.SaveChangesAsync();

            var foundTranslators = TranslatorRepository.FindByCondition(tr => tr.Name == "TEST-UPDATE" && tr.CreditCardNumber == "123" & tr.HourlyRate == 500);

            Assert.IsNotNull(foundTranslators.Any());

            var translator = TranslatorRepository.FindById(foundTranslators.First().Id);
            translator.Name = "TEST-UPDATED";
            TranslatorRepository.Update(translator);
            await TranslatorRepository.SaveChangesAsync();

            foundTranslators = TranslatorRepository.FindByCondition(tr => tr.Name == "TEST-UPDATED" && tr.CreditCardNumber == "123" & tr.HourlyRate == 500);

            Assert.IsNotNull(foundTranslators.Any());
        }

        [TestMethod]
        public async Task TestDelete()
        {
            TranslatorRepository.Create(new Api.Models.Translator()
            {
                Name = "TEST-DELETE",
                CreditCardNumber = "123",
                HourlyRate = 500
            });

            await TranslatorRepository.SaveChangesAsync();

            var foundTranslators = TranslatorRepository.FindByCondition(tr => tr.Name == "TEST-DELETE" && tr.CreditCardNumber == "123" & tr.HourlyRate == 500);

            Assert.IsNotNull(foundTranslators.Any());

            var translator = TranslatorRepository.FindById(foundTranslators.First().Id);
            TranslatorRepository.Delete(translator);

            foundTranslators = TranslatorRepository.FindByCondition(tr => tr.Name == "TEST-DELETE" && tr.CreditCardNumber == "123" & tr.HourlyRate == 500);

            Assert.IsNotNull(foundTranslators.Any());
        }
    }
}
