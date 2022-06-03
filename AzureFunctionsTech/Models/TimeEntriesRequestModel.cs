using System;
using System.ComponentModel.DataAnnotations;

namespace AzureFunctionsTech.Api.Models
{
    public class TimeEntriesRequestModel
    {
        [Required]
        public DateTime StartOn { get; set; }
        [Required]
        public DateTime EndOn { get; set; }
    }
}
