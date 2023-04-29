using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Wheat.Models
{
	public class PostTagMod
	{
		[Key]
		public int PostId { get; set; }
		public string PostTitle { get; set; }
		public string PostBody { get; set; } = string.Empty;
		public DateTime Created { get; set; } = DateTime.Now;
		public List<Checkbx> chkedTags { get; set; }
	}
}
