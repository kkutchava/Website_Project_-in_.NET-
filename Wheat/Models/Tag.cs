using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Wheat.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        public string TagName { get; set; }
        public string TagDescr { get; set; }
        public IList<PostTag> PostsTags { get; set; }
    }
}
