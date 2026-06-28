using System.ComponentModel.DataAnnotations;

namespace InterviewExercise.Models
{
    public class UriModel
    {
        [Required(ErrorMessage = "Uri is required")]
        public string Uri { get; set; } = string.Empty;
    }
}
