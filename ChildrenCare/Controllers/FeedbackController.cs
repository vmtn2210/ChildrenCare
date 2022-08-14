using ChildrenCare.Data;
using ChildrenCare.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenCare.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ChildrenCareDBContext db;
        public FeedbackController(ChildrenCareDBContext db)
        {
            this.db = db;
        }

        // GET: FeedbackController
        public ActionResult Index()
        {
            var list = db.FeedBacks.ToList();
            return View(list);
        }

        // GET: FeedbackController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var feedback = db.FeedBacks.FirstOrDefault(x => x.Id == id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // GET: FeedbackController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FeedbackController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeedBacks feedback)
        {
            if (ModelState.IsValid)
            {
                db.Add(feedback);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // GET: FeedbackController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var feedback = db.FeedBacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }
            return View(feedback);
        }

        // POST: FeedbackController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FeedBacks feedback)
        {
            if(ModelState.IsValid)
            {
                db.Update(feedback);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(feedback);
        }

        // GET: FeedbackController/Delete/5
        public ActionResult Delete(int? id)
        {
            var feedback = db.FeedBacks.Find(id);
            db.FeedBacks.Remove(feedback);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
