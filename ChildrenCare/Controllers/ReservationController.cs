using System.Security.Claims;
using ChildrenCare.DTOs.ReservationDetailDTOs;
using ChildrenCare.DTOs.ReservationDTOs;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildrenCare.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservation;
        private readonly IReservationServiceService _reservationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStaffSpecializationService _staffSpecializationService;
        private readonly IPrescriptionService _prescriptionService;

        public ReservationController(IReservationService reservation, IReservationServiceService reservationService,
            IHttpContextAccessor httpContextAccessor, IStaffSpecializationService staffSpecializationService,
            IPrescriptionService prescriptionService)
        {
            _reservation = reservation;
            _reservationService = reservationService;
            _httpContextAccessor = httpContextAccessor;
            _staffSpecializationService = staffSpecializationService;
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> ReservationManagement(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            if (string.IsNullOrEmpty(searchString))
                searchString = "";
            ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(searchString);
            return View("ReservationManagement");
        }

        [HttpGet]
        public async Task<IActionResult> ReservationSearch(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;
            if (string.IsNullOrEmpty(searchString))
                searchString = "";
            ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(searchString);
            return View("ReservationManagement");
        }

        [HttpGet]
        public async Task<IActionResult> MyReservation()
        {
            ViewBag.ReservationList = await _reservation.GetMyReservationList();
            return View("MyReservation");
        }

        [HttpGet]
        public async Task<IActionResult> ReservationDetail(int id)
        {
            ViewBag.ReservationId = id;
            var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            var reservation = await _reservation.GetReservationById(id);
            if (reservation == null || role == null)
            {
                return role switch
                {
                    "customer" => RedirectToAction("MyReservation"),
                    "manager" => RedirectToAction("ReservationManagement"),
                    "doctor" => RedirectToAction("MyReservation"),
                    "nurse" => RedirectToAction("MyReservation"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            var status = reservation.Status;
            ViewBag.IsEditable = ("customer".Equals(role) &&
                                  status is GlobalVariables.DraftReservationStatus
                                      or GlobalVariables.SubmittedReservationStatus) ||
                                 ("manager".Equals(role) && status is GlobalVariables.SubmittedReservationStatus);

            var reservationDetails = await _reservationService.GetReservationDetailList(id);
            ViewBag.ReservationDetailList = reservationDetails;
            var staffSpecializations = await
                _staffSpecializationService.GetStaffSpecializationList(reservationDetails.Select(x => x.ServiceId));
            var map = staffSpecializations.GroupBy(x => x.ServiceId).ToDictionary(x => x.Key, x => x.ToList());
            ViewBag.Dictionary = map;
            ViewBag.Role = role;
            return View("ReservationDetail");
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceAmount(ChangeReservationDetailRequestDTO dto)
        {
            await _reservationService.AddServiceAmount(dto.DetailId);
            await _reservationService.RecalculateTotal(dto.ReservationId);
            return RedirectToAction("ReservationDetail", new { id = dto.ReservationId });
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseServiceAmount(ChangeReservationDetailRequestDTO dto)
        {
            await _reservationService.DecreaseServiceAmount(dto.DetailId);
            await _reservationService.RecalculateTotal(dto.ReservationId);
            return RedirectToAction("ReservationDetail", new { id = dto.ReservationId });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStaffService(int detailId, int reservationId, int staffId)
        {
            await _reservationService.AssignStaffToService(detailId, staffId);
            return RedirectToAction("ReservationDetail", new { id = reservationId });
        }

        [HttpPost]
        public async Task<IActionResult> AddServiceNumberOfPeople(ChangeReservationDetailRequestDTO dto)
        {
            await _reservationService.AddServiceNumberOfPeople(dto.DetailId);
            return RedirectToAction("ReservationDetail", new { id = dto.ReservationId });
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseServiceNumberOfPeople(ChangeReservationDetailRequestDTO dto)
        {
            await _reservationService.DecreaseServiceNumberOfPeople(dto.DetailId);
            return RedirectToAction("ReservationDetail", new { id = dto.ReservationId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveServiceFromReservation(ChangeReservationDetailRequestDTO dto)
        {
            await _reservationService.RemoveServiceFromReservation(dto.DetailId);
            await _reservationService.RecalculateTotal(dto.ReservationId);
            return RedirectToAction("ReservationDetail", new { id = dto.ReservationId });
        }

        [HttpGet]
        public async Task<IActionResult> ReservationContact(int id)
        {
            ViewBag.ReservationId = id;
            var reservation = await _reservation.GetReservationById(id);
            var status = reservation.Status;
            var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            ViewBag.IsEditable = ("customer".Equals(role) &&
                                  status is GlobalVariables.DraftReservationStatus
                                      or GlobalVariables.SubmittedReservationStatus) ||
                                 ("manager".Equals(role) && status is GlobalVariables.SubmittedReservationStatus);
            var dto = await _reservation.GetReservationContactInfo(id);
            var genderTypes =
                Enum.GetValues(typeof(GlobalVariables.GenderType)).Cast<GlobalVariables.GenderType>();
            dto.GenderList = from genderType in genderTypes
                select new SelectListItem() { Text = genderType.ToString(), Value = ((int)genderType).ToString() };
            if (reservation == null || role == null)
            {
                return role switch
                {
                    "customer" => RedirectToAction("MyReservation"),
                    "manager" => RedirectToAction("ReservationManagement"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> ReservationContact(UpdateReservationContactDTO dto)
        {
            dto = (UpdateReservationContactDTO)ObjectTrimmer.TrimObject(dto);
            if (!ModelState.IsValid)
            {
                var genderTypes =
                    Enum.GetValues(typeof(GlobalVariables.GenderType)).Cast<GlobalVariables.GenderType>();
                dto.GenderList = from genderType in genderTypes
                    select new SelectListItem() { Text = genderType.ToString(), Value = ((int)genderType).ToString() };
                var reservation = await _reservation.GetReservationById(dto.Id);
                var status = reservation.Status;
                var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
                ViewBag.IsEditable = ("customer".Equals(role) &&
                                  status is GlobalVariables.DraftReservationStatus
                                      or GlobalVariables.SubmittedReservationStatus) ||
                                 ("manager".Equals(role) && status is GlobalVariables.SubmittedReservationStatus);
                return View("ReservationContact", dto);
            }

            var result = await _reservation.SubmitReservation(dto);
            if (!result.IsSuccess)
            {
                var genderTypes =
                    Enum.GetValues(typeof(GlobalVariables.GenderType)).Cast<GlobalVariables.GenderType>();
                dto.GenderList = from genderType in genderTypes
                    select new SelectListItem() { Text = genderType.ToString(), Value = ((int)genderType).ToString() };
                ModelState.AddModelError(string.Empty, result.Message);
                return View(dto);
            }

            ViewBag.ReservationId = dto.Id;
            ViewBag.ReservationDetailList = await _reservationService.GetReservationDetailList(dto.Id);
            return View("ReservationComplete");
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReservation(int id)
        {
            var result = await _reservation.ApproveReservation(id);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            else
            {
                var reservation = await _reservation.GetReservationById(id);
                if (reservation != null)
                {
                    var email = reservation.CustomerEmail;
                    var body = await System.IO.File.ReadAllTextAsync("Resources/MailApproved.html");
                    if (email != null)
                        await MailUtil.SendMail(email, body, "Reservation Notification");
                }
            }

            ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(null);
            return View("ReservationManagement");
        }

        [HttpPost]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            var result = await _reservation.CancelReservation(id);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            else
            {
                var reservation = await _reservation.GetReservationById(id);
                if (reservation != null)
                {
                    var email = reservation.CustomerEmail;
                    var body = await System.IO.File.ReadAllTextAsync("Resources/MailCancelled.html");
                    if (email != null)
                        await MailUtil.SendMail(email, body, "Reservation Notification");
                }
            }

            if (role == "customer")
            {
                ViewBag.ReservationList = await _reservation.GetMyReservationList();
            }
            else
            {
                ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(null);
            }

            return role switch
            {
                "customer" => RedirectToAction("MyReservation"),
                "manager" => RedirectToAction("ReservationManagement"),
                "staff" => RedirectToAction("ReservationManagement"),
                "doctor" => RedirectToAction("ReservationManagement"),
                "nurse" => RedirectToAction("ReservationManagement"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
            var result = await _reservation.DeleteReservation(id);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }

            if (role == "customer")
            {
                ViewBag.ReservationList = await _reservation.GetMyReservationList();
            }
            else
            {
                ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(null);
            }

            return role switch
            {
                "customer" => RedirectToAction("MyReservation"),
                "manager" => RedirectToAction("ReservationManagement"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        [HttpPost]
        public async Task<IActionResult> ReservationSuccess(int id)
        {
            var result = await _reservation.ReservationSuccess(id);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
            }
            else
            {
                var reservation = await _reservation.GetReservationById(id);
                if (reservation != null)
                {
                    var email = reservation.CustomerEmail;
                    var body = await System.IO.File.ReadAllTextAsync("Resources/MailSuccess.html");
                    if (email != null)
                        await MailUtil.SendMail(email, body, "Reservation Notification");
                }
            }

            ViewBag.ReservationList = await _reservation.AdvancedGetReservationList(null);
            return View("ReservationManagement");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription(int? reservationId)
        {
            var prescription = await _prescriptionService.FindByReservationId(reservationId.Value);
            if (null != prescription)
            {
                return RedirectToAction("PrescriptionListEachReservation", "Prescription",
                    new { reservationId = reservationId });
            }

            try
            {
                return RedirectToAction("PrescriptionListEachReservation", "Prescription",
                    new { reservationId = reservationId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View("ReservationManagement");
        }

        [HttpPost]
        public async Task<IActionResult> ViewPrescription(int? reservationId)
        {
            var prescription = await _prescriptionService.FindByReservationId(reservationId.Value);
            if (null != prescription)
            {
                return RedirectToAction("PrescriptionListCustomer", "Prescription",
                    new { reservationId = reservationId });
            }

            try
            {
                return RedirectToAction("PrescriptionListCustomer", "Prescription",
                    new { reservationId = reservationId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View("ReservationManagement");
        }
    }
}