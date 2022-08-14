using ChildrenCare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ChildrenCare.Services.Interface;
using ChildrenCare.Data;
using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using Microsoft.AspNetCore.Identity;
using ChildrenCare.Repositories.Interfaces;

namespace ChildrenCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServiceService _service;
        private readonly IBlogService _blogService;
        private readonly ILogger<HomeController> _logger;

        private readonly ChildrenCareDBContext _dBContext;
        private readonly IAppUserService _appUserService;
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepositoryWrapper _repositoryWrapper;

        public HomeController(ILogger<HomeController> logger, IServiceService service, 
            ChildrenCareDBContext dBContext, IAppUserService appUserService,
            UserManager<AppUser> userManager, IBlogService blogService, RoleManager<AppRole> roleManager, IRepositoryWrapper repositoryWrapper)
        {
            _logger = logger;
            _service = service;
            _dBContext = dBContext;
            _appUserService = appUserService;
            _userManager = userManager;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var slider = _dBContext.Sliders.ToList();
            ViewBag.Services = await _service.GetFeaturedServiceList();
            ViewBag.Blogs = await _blogService.GetBlogList(new GetBlogListRequestDTO());
            return View(slider);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: AdminController/Home/Profile/5
        public async Task<IActionResult> Profile()
        {
            var id = _userManager.GetUserId(this.User);
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AdminController/Home/Profile/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(int? id, AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _dBContext.Update(appUser);
                _dBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appUser);
        }
        
        // POST: AdminController/Home/Profile/5/ChangePassword
        public async Task<IActionResult> ChangePassword()
        {
            return View("ChangePassword");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitChangePassword(string newPassword, string confirmPassword)
        {
            if (!newPassword.Equals(confirmPassword))
            {
                return View("ChangePassword");
            }
            var user  = this.User;
            var id = _userManager.GetUserId(User);
            var appUser = await _userManager.FindByIdAsync(id);

            var result = await _appUserService.UpdatePassword(appUser, newPassword);
            if (result.IsSuccess)
            {
                return View("Profile", appUser);
            }
            return View("ChangePassword");
        }


        /////////////////////////////////////////
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
            var baseUser = await _userManager.FindByIdAsync(id);
            if (baseUser != null)
            {
                baseUser.Email = appUser.Email;
                baseUser.NormalizedEmail = appUser.Email.ToUpper();
                baseUser.Gender = appUser.Gender;
                baseUser.FullName = appUser.FullName;
                baseUser.PhoneNumber = appUser.PhoneNumber;
                await _userManager.UpdateAsync(baseUser);
            }
            return RedirectToAction("Profile", "Home");
        }

    }
}