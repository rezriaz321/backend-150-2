using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IssueTracker.Models.Employees
{
    public class PostEmployeeRequest : IValidatableObject
    {
        [Required]
        public string FirstName { get; set; }
        [Required][StringLength(50)]
        public string LastName { get; set; }
        [Range(25000, 500000)]
        public decimal StartingSalary { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           if(FirstName.ToLower() == "darth" && LastName.ToLower() == "vader")
            {
                yield return new ValidationResult("This violates our strict no Sith policy");
            }
        }
    }
}
