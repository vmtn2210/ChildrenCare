using ChildrenCare.DTOs.AppUserDTOs;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildrenCare.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IAppUserService _appUserService;

        // public IActionResult Index()
        // {
        //     return View();
        // }

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public IActionResult Register()
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
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
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
                return View("Register", dto);
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _appUserService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO dto)
        {
            dto = (LoginRequestDTO)ObjectTrimmer.TrimObject(dto);

            if (!ModelState.IsValid)
            {
                return View("Login");
            }

            var result = await _appUserService.Login(dto);

            if (!result.IsSuccess)
            {
                ViewBag.ErrorMessage = result.Message;
                return View("Login");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
