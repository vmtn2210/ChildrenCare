using ChildrenCare.Data;
using ChildrenCare.DTOs.BlogDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.ImageUpload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenCare.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ChildrenCareDBContext _db;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRepositoryWrapper _wrapper;
        private readonly IPhotoService _photoService;
        public BlogController(IPhotoService photoService,ChildrenCareDBContext db, RoleManager<AppRole> roleManager, IBlogService blogService, ICategoryService categoryService, UserManager<AppUser> userManager, IRepositoryWrapper wrapper)
        {
            this._db = db;
            _blogService = blogService;
            _categoryService = categoryService;
            _userManager = userManager;
            _roleManager = roleManager;
            _wrapper = wrapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> BlogManagement(string category, string status, string title)
        {
            ViewData["CurrentCategory"] = category;
            //ViewData["CurrentAuthor"] = author;
            ViewData["CurrentStatus"] = status;
            ViewData["CurrentTitle"] = title;
            if (string.IsNullOrEmpty(category))
                category = "";
            //if (string.IsNullOrEmpty(author))
            //    author = "";
            if (string.IsNullOrEmpty(status))
                status = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            var list = _db.Users.ToList();
            foreach (var user in list)
            {
                var listRole = await _userManager.GetRolesAsync(user);
                var userRole = await _roleManager.FindByNameAsync(listRole.First());
                var userRoles = new List<AppUserRole>();
                var appUserRole = new AppUserRole()
                {
                    User = user,
                    Role = userRole
                };
                userRoles.Add(appUserRole);
                user.UserRoles = userRoles;
            }
            var managerList = list.FindAll(x => x.UserRoles.First().Role.Id == 2);

            ViewBag.Blogs = await _blogService.GetBlogManagementList(category, status, title);
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.ManagerList = managerList;

            //foreach (GetBlogManagementListResponseDTO blog in ViewBag.Blogs.Items)
            //{
            //    var user = await _userManager.FindByIdAsync(blog.AuthorUserId.ToString());
            //    blog.AuthorName = user.FullName;
            //}
            //ViewBag.Author = await _blogService.GetBlogManagementList(new GetBlogManagementListRequestDTO().AuthorUserIds);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchBlogManagement(string category, string status, string title)
        {
            ViewData["CurrentCategory"] = category;
            //ViewData["CurrentAuthor"] = author;
            ViewData["CurrentStatus"] = status;
            ViewData["CurrentTitle"] = title;
            if (string.IsNullOrEmpty(category))
                category = "";
            //if (string.IsNullOrEmpty(author))
            //    author = "";
            if (string.IsNullOrEmpty(status))
                status = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            var list = _db.Users.ToList();
            foreach (var user in list)
            {
                var listRole = await _userManager.GetRolesAsync(user);
                var userRole = await _roleManager.FindByNameAsync(listRole.First());
                var userRoles = new List<AppUserRole>();
                var appUserRole = new AppUserRole()
                {
                    User = user,
                    Role = userRole
                };
                userRoles.Add(appUserRole);
                user.UserRoles = userRoles;
            }
            var managerList = list.FindAll(x => x.UserRoles.First().Role.Id == 2);


            //ViewBag.Blogs = await _blogService.GetBlogManagementList(new GetBlogManagementListRequestDTO());
            ViewBag.Blogs = await _blogService.GetBlogManagementList(category, status, title);
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.ManagerList = managerList;

           
            return View("BlogManagement");
        }

        public async Task<IActionResult> BlogList(string category, string title)
        {
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentTitle"] = title;
            if (string.IsNullOrEmpty(category))
                category = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            ViewBag.Blogs = await _blogService.GetBlogList(category, title);
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View("BlogList");
        }

        public async Task<IActionResult> BlogSearch(string category, string title)
        {
            ViewData["CurrentCategory"] = category;
            ViewData["CurrentTitle"] = title;
            if (string.IsNullOrEmpty(category))
                category = "";
            if (string.IsNullOrEmpty(title))
                title = "";
            ViewBag.Blogs = await _blogService.GetBlogList(category, title);
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View("BlogList");
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlog()
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return View("CreateBlog");
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(CreateBlogRequestDTO dto)
        {
            dto = (CreateBlogRequestDTO)ObjectTrimmer.TrimObject(dto);

            //Check input server side
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllCategories();
                ViewBag.Blog = dto;
                return View();
            }

            await _blogService.CreateBlog(dto);
            return RedirectToAction("BlogManagement", "Blog");
        }

        [HttpGet]
        public async Task<IActionResult> BlogDetail(int id)
        {
            var blogDetail = await _blogService.GetBlogDetail(id);

            if (blogDetail == null)
            {
                //TODO: Catch not found
            }

            ViewBag.BlogDetail = blogDetail;

            return View("BlogDetail");
        }
        public ActionResult BlogDetailManager(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
            //var slider = _db.Sliders.Find(id);
            //_db.Sliders.Remove(slider);
            //_db.SaveChanges();
            //return RedirectToAction(nameof(CreateSlider));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBlog(int id)
        {
            ViewBag.Categories = await _categoryService.GetAllCategories();
            ViewBag.Blog = await _blogService.GetBlog(id);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(UpdateBlogRequestDTO dto)
        {
            //Trim tất cả string trong object
            dto = (UpdateBlogRequestDTO)ObjectTrimmer.TrimObject(dto);

            //Check input server side
            if (!ModelState.IsValid)
            {
                return View();
            }

            var blog = await _wrapper.Blog.FindSingleByConditionAsync(x => x.Id == dto.Id);
            blog.CategoryId = dto.CategoryId;
            blog.Tittle = dto.Tittle;
            blog.Status = dto.Status;
            blog.BriefInfo = dto.BriefInfo;
            blog.BlogBody = dto.BlogBody;
            var upload = await _photoService.AppPhotoAsync(dto.ThumbnailFile);
            blog.ThumbnailUrl = upload.SecureUrl.AbsoluteUri;
            await _wrapper.Blog.UpdateAsync(blog, blog.Id);
            await _wrapper.SaveAllAsync();
            ViewBag.Categories = await _categoryService.GetAllCategories();
            return RedirectToAction("BlogManagement", "Blog");
        }
        public ActionResult DeleteBlog(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = _db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
            //var slider = _db.Sliders.Find(id);
            //_db.Sliders.Remove(slider);
            //_db.SaveChanges();
            //return RedirectToAction(nameof(CreateSlider));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBlog(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = _db.Blogs.Find(id);
            if (ModelState.IsValid)
            {
                _db.Blogs.Remove(blog);
                _db.SaveChanges();
                return RedirectToAction(nameof(BlogManagement));
            }
            return View(blog);
        }
    }
}
