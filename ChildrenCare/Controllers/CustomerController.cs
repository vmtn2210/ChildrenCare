using ChildrenCare.Entities;
using ChildrenCare.Data;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ChildrenCare.DTOs.AppUserDTOs;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildrenCare.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ChildrenCareDBContext _db;

        private readonly IAppUserService _appUserService;

        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;

        private readonly IRepositoryWrapper _repositoryWrapper;
        public CustomerController(ChildrenCareDBContext db, IAppUserService appUserService,
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IRepositoryWrapper repositoryWrapper)
        {
            this._db = db;
            _appUserService = appUserService;
            _userManager = userManager;
            _roleManager = roleManager;
            _repositoryWrapper = repositoryWrapper;
        }
        // GET: AdminController
        public ActionResult CustomerList()
        {
            var list = _db.Users.ToList();
            return View(list);
        }

        // GET: AdminController/Details/5
        public ActionResult DetailsCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.Users.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // GET: AdminController/Create
        //public ActionResult AddCustomer()
        //{
        //    return View();
        //}

        //// POST: AdminController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddCustomer(AppUser appUser)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _db.Add(appUser);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(appUser);
        //}

        // GET: AdminController/Edit/5
        public async Task<IActionResult> EditCustomer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(id.ToString());
            var listRole = await _userManager.GetRolesAsync(user);
            var userRole = await _roleManager.FindByNameAsync(listRole.First());
            ICollection<AppUserRole> userRoles = new List<AppUserRole>();
            AppUserRole appUserRole = new AppUserRole()
            {
                User = user,
                Role = userRole
            };
            userRoles.Add(appUserRole);
            user.UserRoles = userRoles;
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomerEdit(AppUser appUser, string? inputRole)
        {
            var id = appUser.Id.ToString();
            var baseUser =  await _userManager.FindByIdAsync(id);
            if (baseUser != null) {
                var newRole = await _roleManager.FindByNameAsync(inputRole);
                var listOldRole = await _userManager.GetRolesAsync(baseUser);
                var userOldRole = await _roleManager.FindByNameAsync(listOldRole.First());
                if (!newRole.Equals(userOldRole))
                {
                    ICollection<AppUserRole> userRoles = new List<AppUserRole>();
                    AppUserRole appUserRole = new AppUserRole()
                    {
                        User = baseUser,
                        Role = userOldRole,
                    };
                    userRoles.Add(appUserRole);
                    baseUser.UserRoles = userRoles;
                
                    //Xóa role cũ
                    await _repositoryWrapper.AppUserRole.DeleteAsync(baseUser.UserRoles.First());
                    baseUser.UserRoles.Remove(baseUser.UserRoles.First());
                    //thêm role mới
                    await _userManager.AddToRoleAsync(baseUser, newRole.Name);
                }
                baseUser.Email = appUser.Email;
                baseUser.NormalizedEmail = appUser.Email.ToUpper();
                baseUser.Gender = appUser.Gender;
                baseUser.IsPotentialCustomer = appUser.IsPotentialCustomer;
                baseUser.FullName = appUser.FullName;
                baseUser.PhoneNumber = appUser.PhoneNumber;
                await _userManager.UpdateAsync(baseUser);
            }
            return RedirectToAction("CustomerList", "Customer");
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            //Lấy list các gender
            RegisterRequestDTO dto = new RegisterRequestDTO();
            IEnumerable<GlobalVariables.GenderType> genderTypes =
                Enum.GetValues(typeof(GlobalVariables.GenderType)).Cast<GlobalVariables.GenderType>();
            dto.GenderList = from genderType in genderTypes
                             select new SelectListItem() { Text = genderType.ToString(), Value = ((int)genderType).ToString() };
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(RegisterRequestDTO dto)
        {
            //Trim tất cả các string trong object
            dto = (RegisterRequestDTO)ObjectTrimmer.TrimObject(dto);

            var result = await _appUserService.Register(dto);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                IEnumerable<GlobalVariables.GenderType> genderTypes =
                    Enum.GetValues(typeof(GlobalVariables.GenderType)).Cast<GlobalVariables.GenderType>();
                dto.GenderList = from genderType in genderTypes
                                 select new SelectListItem() { Text = genderType.ToString(), Value = ((int)genderType).ToString() };
                return View("AddCustomer", dto);
            }

            return RedirectToAction("CustomerList");
        }
    }
}