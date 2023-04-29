using System.ComponentModel.DataAnnotations; //for data anotations
using System.ComponentModel;

namespace Wheat.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z ,.'-]+$",
         ErrorMessage = "There are characters that are not allowed:)\n")]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "The E-mail address is not valid:)\n")]
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
