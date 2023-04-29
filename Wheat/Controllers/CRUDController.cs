using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using Wheat.Data;
using Wheat.Models;

namespace Wheat.Controllers
{
    public class CRUDController : Controller
    {
        Post objPost = new Post();
        

        private readonly ApplicationDbContext _db;
        public CRUDController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Post> objPost = _db.Posts;
            return View(objPost);
        }
        //get
        [HttpGet]
        public IActionResult CreatePost()
        {
            var itm = _db.Tags.ToList();
            PostTagMod ptm = new PostTagMod();
            ptm.chkedTags = itm.Select(x => new Checkbx()
            {
                Id = x.TagId,
                Name = x.TagName,
                IsChecked = false
            }).ToList();
            return View(ptm);
        }
        //post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreatePost(PostTagMod ptm, Post pst, PostTag pt)
        {
            List<PostTag> lpt = new List<PostTag>();
            pst.PostTitle = ptm.PostTitle;
            pst.PostBody = ptm.PostBody;
            _db.Posts.Add(pst);
            _db.SaveChanges();

            int pstId = pst.PostId; 

            foreach (var obj in ptm.chkedTags)
			{
                if (obj.IsChecked == true)
                    lpt.Add(new PostTag()
                    {
                        PostId = pstId,
                        TagId = obj.Id
                    });
			}

            foreach (var obj in lpt) 
                _db.PostsTags.Add(obj);
            _db.SaveChanges();
            TempData["Success"] = "Post created succesfully!";
            return RedirectToAction("Index");
        }

        //get
        public IActionResult CreateTag()
        {
            return View();
        }
        //post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CreateTag(Tag obj)
        {
            if (obj.TagName == obj.TagDescr.ToString())
            {
                ModelState.AddModelError("name", "The tag name cannot exactly match the tag description");
            }
            if (true) //ModelState.IsValid not working but otherwise, yes????
            {
                _db.Tags.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Tag created succesfully!";
                return RedirectToAction("Index"); //RedirectToAction("Index", "Home")
            }
            return View(obj);
        }


        public IActionResult RevTags()
        {
            IEnumerable<Tag> objTag = _db.Tags;
            return View(objTag);
        }

        public IActionResult Read(int? id)
        {
            if (id == null || id == 0)
                return NotFound(); //not valid Id
            var postFromDb = _db.Posts.Find(id);
            if (postFromDb == null)
                return NotFound();



            dynamic mymodel = new ExpandoObject();
            mymodel.Posts = postFromDb;

            IEnumerable<PostTag> PTofPost = _db.PostsTags.Where(x => x.PostId == id); //collection of objs where neccesary TagIds are
            List<Tag> TagsforPost = new List<Tag>();
            foreach (PostTag t in PTofPost)
            {
                try
                {
                    TagsforPost.Add(_db.Tags.Find(t.TagId));
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            mymodel.Tags = TagsforPost;

            return View(mymodel);
        }
   //     [HttpGet]
        public IActionResult Update(int? id)
        {
            PostTagMod ptm = new PostTagMod();
            var pst = _db.Posts
                .Include(x => x.PostsTags)
                .ThenInclude(y => y.Tag)
                .AsNoTracking()
                .SingleOrDefault(z => z.PostId == id);

            var alltags = _db.Tags.Select(x => new Checkbx()
            {
                Id = x.TagId,
                Name = x.TagName,
                IsChecked = x.PostsTags.Any(y => y.PostId == pst.PostId) ? true : false
            }).ToList();

            ptm.PostId = pst.PostId;
            ptm.PostTitle = pst.PostTitle;
            ptm.PostBody = pst.PostBody;
            ptm.chkedTags = alltags;
            return View(ptm);
        }

        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public IActionResult Update(PostTagMod ptm, Post pst, PostTag pt)
        {
               List<PostTag> lpt = new List<PostTag>();
                pst.PostTitle = ptm.PostTitle;
                pst.PostBody = ptm.PostBody;
                pst.Created = ptm.Created;

               _db.Posts.Update(pst);
               _db.SaveChanges();

                int pstId = pst.PostId;

                foreach (var obj in ptm.chkedTags)
                {
                    if (obj.IsChecked == true)
                        lpt.Add(new PostTag()
                        {
                            PostId = pstId,
                            TagId = obj.Id
                        });
                }

                var dbTable = _db.PostsTags.Where(x => x.PostId == pstId).ToList();
                var rlist = dbTable.Except(lpt).ToList();
                foreach (var obj in rlist)
                {
                    _db.PostsTags.Remove(obj);
                    _db.SaveChanges();
                }

                var tagid = _db.PostsTags.Where(x => x.PostId == pstId).ToList();
                foreach (var obj in lpt)
                {
                    if (!tagid.Contains(obj))
                    {
                        _db.PostsTags.Add(obj);
                        _db.SaveChanges();
                    }
                }
            //////////////////////////////////////////////////////
            //////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////

            TempData["Success"] = "Post Edited succesfully!";
            return RedirectToAction("Index");
        }
        //[HttpPost, ActionName("DeleteTag")]
        // [AutoValidateAntiforgeryToken]
        public IActionResult DeleteTag(int? id)
        {
            //delete from middle table
            var tagidsdlt = _db.PostsTags.Where(x => x.TagId == id); 
            if (tagidsdlt == null)
                return NotFound();
            _db.PostsTags.RemoveRange(tagidsdlt);
            _db.SaveChanges();

            //delete from tag table
            var obj = _db.Tags.Find(id);
            if (obj == null)
                return NotFound();
            _db.Tags.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Tag deleted succesfully!";
            return RedirectToAction("RevTags");
        }

        public IActionResult DeletePost(int? id)
        {
            //delete from middle table
            var postidsdlt = _db.PostsTags.Where(x => x.PostId == id);
            if (postidsdlt == null)
                return NotFound();
            _db.PostsTags.RemoveRange(postidsdlt);
            _db.SaveChanges();

            //delete from post table
            var obj = _db.Posts.Find(id);
            if (obj == null)
                return NotFound();
            _db.Posts.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Post deleted succesfully!";
            return RedirectToAction("Index");
        }

    }
}


/*
 @model Wheat.Models.PostTagMod

@{
    ViewData["Title"] = "Update Post";
}

<form asp-action="Update">
	<div class="border p-3 mt-4">
		<div class="row pb-2">
			<h2 class="text-primary">Update Post</h2> <hr/>
		</div>
		<div class="mb-3">
			<label asp-for="PostTitle">Post Title</label>
			<input asp-for="PostTitle" class="form-control" />
			<span asp-validation-for="PostTitle" class="text-danger"></span><br />
		</div>
		<div class="mb-3">
			<label asp-for="PostBody">Post Body</label>
			<input asp-for="PostBody" class="form-control" />
			<span asp-validation-for="PostBody" class="text-danger"></span>
		</div>

		<div class="mb-3">
			<table>
				@for (int j = 0; j < Model.chkedTags.Count; ++j) {
					<tr>
						<td>
							@Html.HiddenFor(x => Model.chkedTags[j].Id)
							@Html.DisplayFor(x => Model.chkedTags[j].Name)
						</td>
						<td>
							@Html.CheckBoxFor(x => Model.chkedTags[j].IsChecked)
						</td>
					</tr>
				}
			</table>
		</div>

		<button type="submit" class="btn btn-primary" value="Save" style="width:150px">Edit</button>
		<a asp-controller="CRUD" asp-action="Index" class="btn btn-secondary" style="width:150px">
			Back to List
		</a>
	</div>
</form>

@section Scripts {
	@{<partial name="_ValidationScriptsPartial" />}
}



 */