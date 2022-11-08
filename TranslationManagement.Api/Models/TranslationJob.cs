using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TranslationManagement.Api.Enums;

namespace TranslationManagement.Api.Models
{
    public class TranslationJob
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        
        [Required]
        public string CustomerName { get; set; }
        
        public TranslationJobStatus Status { get; set; }

        [Required]
        public string OriginalContent { get; set; }
        
        public string TranslatedContent { get; set; }
        
        public double Price { get; set; }
        
        public Translator Translator { get; set; }
 
        public int? TranslatorId { get; set; }
    }
}
