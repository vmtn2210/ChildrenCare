using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Services;
using ChildrenCare.Services.Interface;

namespace ChildrenCare.Controllers
{
    public class SliderController : Controller
    {
        private readonly ChildrenCareDBContext dBContext;
        private readonly ISliderService _sliderService;

        public SliderController(ChildrenCareDBContext dBContext, ISliderService sliderService)
        {
            this.dBContext = dBContext;
            _sliderService = sliderService;
        }

        // GET: SliderController
        public ActionResult SliderList()
        {   
            var list = dBContext.Sliders.ToList();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SearchSliderList(string status, string title, string backlink)
        {
            ViewData["CurrentStatus"] = status;
            ViewData["CurrentTitle"] = title;
            ViewData["CurrentBacklink"] = backlink;
            if (string.IsNullOrEmpty(status))
                status = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            if (string.IsNullOrEmpty(backlink))
                backlink = "";
            var list = await _sliderService.SearchSlider(status, title, backlink);
            return View("SliderList", list);
        }

        // GET: SliderController/Details/5
        public ActionResult SliderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = dBContext.Sliders.FirstOrDefault(s => s.Id == id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        // GET: SliderController/Create
        public ActionResult CreateSlider()
        {
            return View();
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSlider(Slider slider)
        {
            if (ModelState.IsValid)
            {
                dBContext.Add(slider);
                dBContext.SaveChanges();
                return RedirectToAction(nameof(SliderList));
            }
            return View(slider);
        }

        // GET: SliderController/Edit/5
        public ActionResult UpdateSlider(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = dBContext.Sliders.Find(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail); 
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSlider(int id, Slider slider)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                dBContext.Update(slider);
                dBContext.SaveChanges();
                TempData["Success"] = "Update Succesfully";
                return RedirectToAction(nameof(UpdateSlider));
            }
            return View(slider);
        }

        // GET: SliderController/Delete/5
        public ActionResult DeleteSlider(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var slider = dBContext.Sliders.Find(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
            //var slider = dBContext.Sliders.Find(id);
            //dBContext.Sliders.Remove(slider);
            //dBContext.SaveChanges();
            //return RedirectToAction(nameof(CreateSlider));
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSlider(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var slider = dBContext.Sliders.Find(id);
            if (ModelState.IsValid)
            {               
                dBContext.Sliders.Remove(slider);
                dBContext.SaveChanges();
                return RedirectToAction(nameof(SliderList));
            }
            return View(slider);
        }

    }
}
