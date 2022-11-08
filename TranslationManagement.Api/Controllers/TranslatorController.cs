using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Enums;
using TranslationManagement.Api.Extensions;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories;

namespace TranslationManagement.Api.Controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslatorController : ControllerBase
    {
        private readonly ILogger<TranslatorController> logger;
        private readonly TranslatorRepository translatorRepository;

        public TranslatorController(ILogger<TranslatorController> logger, TranslatorRepository translatorRepository)
        {
            this.logger = logger;
            this.translatorRepository = translatorRepository;
        }

        // GET: api/Translator
        [HttpGet]
        public IEnumerable<Translator> Get()
        {
            return translatorRepository.FindAll();
        }

        // GET: api/Translator/name
        [HttpGet("{name}")]
        public IEnumerable<Translator> GetByName(string name)
        {
            return translatorRepository.FindByCondition(t => t.Name == name);
        }

        // GET: api/Translator/id
        [HttpGet("{id:int}")]
        public Translator GetById(int id)
        {
            return translatorRepository.FindById(id);
        }

        // POST: api/Translator
        [HttpPost]
        public async Task<ActionResult> Add(Translator translator)
        {
            translator.Status = TranslatorStatus.Applicant;
            translator.TranslationJobs = new List<TranslationJob>();
            translatorRepository.Create(translator);
            await translatorRepository.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Translator/UpdateStatus
        [HttpPost("UpdateStatus")]
        public async Task<ActionResult> UpdateStatus(int id, string newStatus = "")
        {
            logger.LogInformation($"Translator status update request: {newStatus} for translator id {id}");

            var newTranslatorStatus = newStatus.ToEnum<TranslatorStatus>();
            if (newTranslatorStatus == TranslatorStatus.Invalid)
            {
                string err = $"Invalid status update request: {newStatus} for translator id {id}";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            var translator = translatorRepository.FindById(id);
            if(translator == null)
            {
                string err = $"Invalid translator id {id} in status update request";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            translator.Status = newTranslatorStatus;
            translatorRepository.Update(translator);
            await translatorRepository.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/Translator/id
        [HttpPut("{id}")]
        public async Task<ActionResult> PutById(int id, Translator translator)
        {
            if (id != translator.Id)
                return BadRequest("Translator ID mismatch");

            if (translator is null) 
                return NotFound();

            var translatorToUpdate = translatorRepository.FindById(id);
            if (translatorToUpdate == null)
                return NotFound($"Translator with Id = {id} not found");

            translatorRepository.Update(translator);
            await translatorRepository.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Translator/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            if (translatorRepository.FindById(id) is Translator translator)
            {
                translatorRepository.Delete(translator);
                await translatorRepository.SaveChangesAsync();
                return Ok(translator);
            }

            return NotFound();
        }
    }
}