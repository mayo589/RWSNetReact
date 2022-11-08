using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TranslationManagement.Api.Enums
{
    public enum TranslatorStatus
    {
        Invalid = 0,
        Applicant, 
        Certified, 
        Deleted
    }
}
