using Microsoft.AspNetCore.Mvc;
using Wheat.Data;
using Wheat.Models;

namespace Wheat.Controllers
{
    public class InfoController : Controller
    {

        private readonly ApplicationDbContext _db;
        public InfoController(ApplicationDbContext db)
        {
            _db = db;
        }

        //get
        public IActionResult Join()
        {
            return View();
        }

        //post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Join(Invitations obj)
        {
            if (ModelState.IsValid)
            {
                _db.invitations.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Received! Your Id is " + obj.Id + "! Check your place in Waiting List!";
                //return RedirectToAction("Index"); //RedirectToAction("Index", "Home")
            }
            //i do redirect because like that it will be reopened without
            //previous information in the input boxes
            return RedirectToAction("Join");
        }

        public IActionResult WaitingList ()
        {
            IEnumerable<Invitations> obj = _db.invitations;
            return View(obj);
        }

        //get
        public IActionResult Contact()
        {
            return View();
        }

        //post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Contact(Contact obj)
        {
            if (ModelState.IsValid)
            {
                _db.contacts.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Yout message was succesfully sent!";
                //return RedirectToAction("Index"); //RedirectToAction("Index", "Home")
            }
            //i do redirect because like that it will be reopened without
            //previous information in the input boxes
            return RedirectToAction("Contact");
        }
    }
}
