using System.Security.Claims;
using ChildrenCare.Data;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc;
using ChildrenCare.Entities;

namespace ChildrenCare.Controllers
{
    public class ServiceController : Controller
    {
        private readonly IServiceService _service;
        private readonly ChildrenCareDBContext db;

        // public IActionResult Index()
        // {
        //     return View();
        // }

        public ServiceController(IServiceService service, ChildrenCareDBContext dBContext)
        {
            _service = service;
            db = dBContext;
        }

        [HttpGet]
        public async Task<IActionResult> ServiceList()
        {            
            var requestDto = new AdvancedGetServiceRequestDTO()
            {
                SortBy = "status_asc",
                PageSize = 100,
                PageNumber = 1,
            };
            ViewBag.ServiceList = await _service.AdvancedGetServiceList(requestDto);
            return View("ServiceList");
        }

        [HttpPost]
        public async Task<IActionResult> ServiceList(AdvancedGetServiceRequestDTO dto)
        {
            //Trim tất cả các chuỗi trong object
            dto = (AdvancedGetServiceRequestDTO) ObjectTrimmer.TrimObject(dto);
            return View("ServiceList");
        }

        [HttpGet]
        public async Task<IActionResult> SearchService(string title)
        {
            ViewData["CurrentTitle"] = title;
            if (string.IsNullOrEmpty(title))
                title = "";
            var requestDto = new AdvancedGetServiceRequestDTO()
            {
                SortBy = "createddate_desc",
                PageSize = 100,
                PageNumber = 1,
                Title = title
            };
            ViewBag.ServiceList = await _service.AdvancedGetServiceList(requestDto);
            return View("ServiceList");
        }

        [HttpGet]
        public async Task<IActionResult> AddService(int id)
        {
            var result = await _service.AddService(id);
            if (result.IsSuccess) return RedirectToAction("ServiceList");
            ModelState.AddModelError("",result.Message);
            var requestDto = new AdvancedGetServiceRequestDTO()
            {
                SortBy = "createddate_desc",
                PageSize = 5,
                PageNumber = 1,
            };
            ViewBag.ServiceList = await _service.AdvancedGetServiceList(requestDto);

            return View("ServiceList");
        }

        public ActionResult ServiceManagement()
        {
            var list = db.Services.ToList();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> SearchServiceList(string status, string title, string briefInfo)
        {
            ViewData["CurrentStatus"] = status;
            ViewData["CurrentTitle"] = title;
            ViewData["CurrentBriefinfo"] = briefInfo;
            if (string.IsNullOrEmpty(status))
                status = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            if (string.IsNullOrEmpty(briefInfo))
                briefInfo = "";
            var list = await _service.SearchService(status, title, briefInfo);
            return View("ServiceManagement", list);
        }

        // GET: ServiceController/Details/5
        public ActionResult ServiceDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = db.Services.FirstOrDefault(s => s.Id == id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        public ActionResult ServiceDetailCus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = db.Services.FirstOrDefault(s => s.Id == id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        // GET: ServiceController/Create
        public ActionResult CreateService()
        {
            return View();
        }

        // POST: ServiceController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateService(Service service)
        {
            if (ModelState.IsValid)
            {
                db.Add(service);
                db.SaveChanges();
                return RedirectToAction(nameof(ServiceManagement));
            }
            return View(service);
        }

        // GET: ServiceController/Edit/5
        public ActionResult UpdateService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = db.Services.Find(id);
            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateService(int id, Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                db.Update(service);
                db.SaveChanges();
                TempData["Success"] = "Update Succesfully";
                return RedirectToAction(nameof(UpdateService));
            }
            return View(service);
        }

        // GET: ServiceController/Delete/5
        public ActionResult DeleteService(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = db.Services.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        // POST: ServiceController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteService(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var service = db.Services.Find(id);
            if (ModelState.IsValid)
            {
                db.Services.Remove(service);
                db.SaveChanges();
                return RedirectToAction(nameof(ServiceManagement));
            }
            return View(service);
        }
    }
}
