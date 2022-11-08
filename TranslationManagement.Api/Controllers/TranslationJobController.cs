using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Controlers;
using TranslationManagement.Api.Enums;
using TranslationManagement.Api.Extensions;
using TranslationManagement.Api.Models;
using TranslationManagement.Api.Repositories;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TranslationJobController : ControllerBase
    {
        private readonly ILogger<TranslatorController> logger;
        private readonly TranslationJobRepository translationJobRepository;
        private readonly TranslatorRepository translatorRepository;
        private const double pricePerCharacter = 0.01;

        public TranslationJobController(ILogger<TranslatorController> logger, TranslationJobRepository translationJobRepository, TranslatorRepository translatorRepository)
        {
            this.logger = logger;
            this.translationJobRepository = translationJobRepository;
            this.translatorRepository = translatorRepository;
        }

        // GET: api/TranslationJob
        [HttpGet]
        public IEnumerable<TranslationJob> Get()
        {
            return translationJobRepository.FindAll();
        }

        // GET: api/TranslationJob/id
        [HttpGet("{id:int}")]
        public TranslationJob GetById(int id)
        {
            return translationJobRepository.FindById(id);
        }

        // POST: api/TranslationJob
        [HttpPost]
        public async Task<IActionResult> Add(TranslationJob job)
        {
            job.Status = TranslationJobStatus.New;
            job.Price = GetJobPrice(job);
            translationJobRepository.Create(job);

            await translationJobRepository.SaveChangesAsync();

            var notificationSvc = new UnreliableNotificationService();
            while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
            {
                //TODO
            }

            logger.LogInformation("New job notification sent");

            return Ok();
        }

        // POST: api/TranslationJob/AddWithFile
        [HttpPost("AddWithFile")]
        public async Task<ActionResult> AddWithFile(IFormFile file, string customer)
        {
            var reader = new StreamReader(file.OpenReadStream());
            string content;

            if (file.FileName.EndsWith(".txt"))
            {
                content = reader.ReadToEnd();
            }
            else if (file.FileName.EndsWith(".xml"))
            {
                var xdoc = XDocument.Parse(reader.ReadToEnd());
                content = xdoc.Root.Element("Content").Value;
                customer = xdoc.Root.Element("Customer").Value.Trim();
            }
            else
            {
                string err = $"Unsupported file: {file.FileName}";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            var job = new TranslationJob()
            {
                OriginalContent = content,
                TranslatedContent = "",
                CustomerName = customer,
                Status = TranslationJobStatus.New,
            };
            job.Price = GetJobPrice(job);

            translationJobRepository.Create(job);
            await translationJobRepository.SaveChangesAsync();

            return Ok();
        }

        // POST: api/TranslationJob/AssignTranslator
        [HttpPost("AssignTranslator")]
        public async Task<ActionResult> AssignTranslator(int id, int translatorId)
        {
            logger.LogInformation($"Job assign request received for job id: {id} by translator id: {translatorId}");

            var job = translationJobRepository.FindById(id);
            if (job == null)
            {
                string err = $"Invalid job id {id} in AssignTranslator request";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            var translator = translatorRepository.FindById(id);
            if (translator == null)
            {
                string err = $"Invalid translator id {id} in AssignTranslator request";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            job.Translator = translator;
            translationJobRepository.Update(job);
            await translationJobRepository.SaveChangesAsync();

            return Ok();
        }

        // POST: api/TranslationJob/UpdateStatus
        [HttpPost("UpdateStatus")]
        public async Task<ActionResult> UpdateStatus(int id, string newStatus)
        {
            logger.LogInformation($"Job status update request received: {newStatus} for job id: {id}");

            var newJobStatus = newStatus.ToEnum<TranslationJobStatus>();
            if(newJobStatus == TranslationJobStatus.Invalid)
            {
                string err = $"Job status update received invalid new status: {newStatus} for job id: {id}";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            var job = translationJobRepository.FindById(id);
            if (job == null)
            {
                string err = $"Invalid job id {id} in status update request";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            var originalJobStatus = job.Status;
            bool isValidStatusChange = (originalJobStatus == TranslationJobStatus.New && newJobStatus == TranslationJobStatus.InProgress) ||
                                        (originalJobStatus == TranslationJobStatus.InProgress && newJobStatus == TranslationJobStatus.Completed); 

            if (isValidStatusChange)
            {
                string err = $"Invalid status update for job id: {id}. Original status: {originalJobStatus}, new status: {newJobStatus}";
                logger.LogError(err);
                return StatusCode(StatusCodes.Status500InternalServerError, err);
            }

            job.Status = newJobStatus;
            translationJobRepository.Update(job);
            await translationJobRepository.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/TranslationJob/id
        [HttpPut("{id}")]
        public async Task<ActionResult> PutById(int id, TranslationJob translationJob)
        {
            if (id != translationJob.Id)
                return BadRequest("TranslationJob ID mismatch");

            if (translationJob is null)
                return NotFound();

            var jobToUpdate = translationJobRepository.FindById(id);
            if (jobToUpdate == null)
                return NotFound($"TranslationJob with Id = {id} not found");

            translationJobRepository.Update(translationJob);
            await translationJobRepository.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/TranslationJob/id
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            if (translationJobRepository.FindById(id) is TranslationJob translationJob)
            {
                translationJobRepository.Delete(translationJob);
                await translationJobRepository.SaveChangesAsync();
                return Ok(translationJob);
            }

            return NotFound();
        }

        private double GetJobPrice(TranslationJob job)
        {
            return job.OriginalContent.Length * pricePerCharacter;
        }
    }
}