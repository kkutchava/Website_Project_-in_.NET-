using System.ComponentModel.DataAnnotations; //for data anotations
using System.ComponentModel;

namespace Wheat.Models
{
    public class Invitations
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "The E-mail address is not valid:)\n")]
        public string Email { get; set; }
    }
}
