using CourseLibrary.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription(ErrorMessage = "Title must != Desc")]
    public class CourseForCreationDto// : IValidatableObject
    {
        [Required(ErrorMessage = "Title needed")]
        [MaxLength(100, ErrorMessage = "Max 100 chars")]
        public string Title { get; set; }

        [MaxLength(1500, ErrorMessage = "Max 1500 chars for desc")]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("Error: Title == Description.", new[] { nameof(CourseForCreationDto) });
        //    }
        //}
    }
}
