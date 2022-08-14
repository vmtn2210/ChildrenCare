using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Controllers;

public class StaffSpecializationController : Controller
{
    private readonly IStaffSpecializationService _staffSpecializationService;
    private readonly IServiceService _serviceService;
    private readonly ChildrenCareDBContext _db;

    public StaffSpecializationController(IStaffSpecializationService staffSpecializationService,
        IServiceService serviceService, ChildrenCareDBContext db)
    {
        _staffSpecializationService = staffSpecializationService;
        _serviceService = serviceService;
        this._db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.StaffSpecializations = await _staffSpecializationService.GetStaffSpecializationList();
        ViewBag.Services = await _serviceService.GetAll();
        ViewBag.Staffs = await _db.UserRoles.Where(x => "doctor".Equals(x.Role.Name) || "nurse".Equals(x.Role.Name))
            .Include(x => x.User)
            .ToListAsync();
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(StaffSpecialization staffSpecialization)
    {
        await _staffSpecializationService.Delete(staffSpecialization.ServiceId, staffSpecialization.StaffId);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Create(StaffSpecialization staffSpecialization)
    {
        await _staffSpecializationService.Create(staffSpecialization.ServiceId, staffSpecialization.StaffId);
        return RedirectToAction("Index");
    }
}