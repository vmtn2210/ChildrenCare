using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenCare.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly ChildrenCareDBContext dBContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionController(ChildrenCareDBContext dBContext, UserManager<AppUser> userManager,
            IRepositoryWrapper repositoryWrapper, IPrescriptionService prescriptionService)
        {
            this.dBContext = dBContext;
            this._userManager = userManager;
            _repositoryWrapper = repositoryWrapper;
            _prescriptionService = prescriptionService;
        }


        public async Task<IActionResult> PrescriptionListEachReservation(int? reservationId)
        {
            ViewBag.ReservationId = reservationId;
            IEnumerable<Prescription> listPrescription = new List<Prescription>();
            if (reservationId.HasValue)
            {
                listPrescription = await _prescriptionService.FindByReservationId(reservationId.Value);
            }
            else
            {
                listPrescription = await _prescriptionService.FindAll();
            }
            foreach (var prescription in listPrescription)
            {
                prescription.AuthorAccount = await _userManager.FindByIdAsync(prescription.AuthorAccountId.ToString());
                prescription.CustomerAccount = await _userManager.FindByIdAsync(prescription.CustomerAccountId.ToString());
            }
            return View(listPrescription);
        }
        public async Task<IActionResult> PrescriptionListCustomer(int? reservationId)
        {
            ViewBag.ReservationId = reservationId;
            IEnumerable<Prescription> listPrescription = new List<Prescription>();
            if (reservationId.HasValue)
            {
                listPrescription = await _prescriptionService.FindByReservationId(reservationId.Value);
            }
            else
            {
                listPrescription = await _prescriptionService.FindAll();
            }
            foreach (var prescription in listPrescription)
            {
                prescription.AuthorAccount = await _userManager.FindByIdAsync(prescription.AuthorAccountId.ToString());
                prescription.CustomerAccount = await _userManager.FindByIdAsync(prescription.CustomerAccountId.ToString());
            }
            return View(listPrescription);
        }

        public async Task<IActionResult> PrescriptionsList(int? reservationId)
        {

            IEnumerable<Prescription> listPrescription = new List<Prescription>();
            if (reservationId.HasValue)
            {
                listPrescription = await _prescriptionService.FindByReservationId(reservationId.Value);
            }
            else
            {
                listPrescription = await _prescriptionService.FindAll();
            }
            foreach (var prescription in listPrescription)
            {
                prescription.AuthorAccount = await _userManager.FindByIdAsync(prescription.AuthorAccountId.ToString());
                prescription.CustomerAccount = await _userManager.FindByIdAsync(prescription.CustomerAccountId.ToString());
            }
            return View(listPrescription);
        }

        // GET: SliderController/Details/5
        public async Task<IActionResult> DetailsPescriptions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = dBContext.Prescriptions.FirstOrDefault(s => s.Id == id);
            detail.AuthorAccount = await _userManager.FindByIdAsync(detail.AuthorAccountId.ToString());
            detail.CustomerAccount = await _userManager.FindByIdAsync(detail.CustomerAccountId.ToString());

            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        public async Task<IActionResult> YourPrescriptionDetail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var detail = dBContext.Prescriptions.FirstOrDefault(s => s.Id == id);
            detail.AuthorAccount = await _userManager.FindByIdAsync(detail.AuthorAccountId.ToString());
            detail.CustomerAccount = await _userManager.FindByIdAsync(detail.CustomerAccountId.ToString());

            if (detail == null)
            {
                return NotFound();
            }
            return View(detail);
        }

        // POST: SliderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePrescription(Prescription prescriptions)
        {
            if (ModelState.IsValid)
            {
                dBContext.Add(prescriptions);
                dBContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("ReservationManagement", "Reservation");
        }

        [HttpGet]
        public ActionResult CreatePrescription(int? reservationId)
        {
            var prescription = new Prescription()
            {
                ReservationId = reservationId.Value
            };
            return View("CreatePrescriptions", prescription);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCreatePrescription(Prescription prescription)
        {
            var id = _userManager.GetUserId(User);
            prescription.AuthorAccountId = Int32.Parse(id);
            prescription.CustomerAccountId = dBContext.Reservations
                .Find(prescription.ReservationId)!.CustomerAccountId;
            var result = await dBContext.Prescriptions.AddAsync(prescription);
            await dBContext.SaveChangesAsync();
            if (null != result.Entity.Id)
            {
                return RedirectToAction("ReservationManagement", "Reservation");

            }
            return RedirectToAction("ReservationManagement", "Reservation");
        }

        // GET: SliderController/Edit/5
        public async Task<IActionResult> UpdatePrescription(int? reservationId)
        {
            var prescription = await _prescriptionService.FindByReservationId(reservationId.Value);


            if (null != prescription)
            {

                return View("UpdatePrescriptions", prescription.FirstOrDefault());
            }
            return RedirectToAction("ReservationManagement", "Reservation");

        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePrescriptions(Prescription prescriptions)
        {
            var id = _userManager.GetUserId(User);
            prescriptions.AuthorAccountId = Int32.Parse(id);
            prescriptions.CreatedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                dBContext.Update(prescriptions);
                dBContext.SaveChanges();
                TempData["Success"] = "Update Succesfully";
                return RedirectToAction("ReservationManagement", "Reservation");
            }
            return View(prescriptions);
        }

        // GET: SliderController/Delete/5
        public ActionResult DeletePrescriptions(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = dBContext.Prescriptions.Find(id);
            if (entity != null)
            {
                dBContext.Prescriptions.Remove(entity);
            }
            else
            {
                return NotFound();
            }
            return View("PrescriptionsList");
            //var slider = dBContext.Sliders.Find(id);
            //dBContext.Sliders.Remove(slider);
            //dBContext.SaveChanges();
            //return RedirectToAction(nameof(CreateSlider));
        }

        // POST: SliderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePrescriptions(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var prescriptions = dBContext.Prescriptions.Find(id);
            if (ModelState.IsValid)
            {
                dBContext.Prescriptions.Remove(prescriptions);
                dBContext.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(prescriptions);
        }

    }
}
