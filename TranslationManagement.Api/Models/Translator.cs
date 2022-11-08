using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TranslationManagement.Api.Enums;

namespace TranslationManagement.Api.Models
{
    public class Translator
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public double HourlyRate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public TranslatorStatus Status { get; set; }
        
        public string CreditCardNumber { get; set; }

        public IEnumerable<TranslationJob> TranslationJobs { get; set; }
    }
}
