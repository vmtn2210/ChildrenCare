using ChildrenCare.Data;
using ChildrenCare.Entities;
using ChildrenCare.Repositories;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.ImageUpload;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ChildrenCareDBContextConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'ChildrenCareDBContextConnection' not found.");

builder.Services.AddDbContext<ChildrenCareDBContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddMemoryCache();

builder.Services.AddIdentity<AppUser, AppRole>(options =>
    {
        //TODO: implement email confirmation
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 3;
        options.Lockout.MaxFailedAccessAttempts = 999;
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ChildrenCareDBContext>();

builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Inject to access current user's information
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Add Cloudinary for image upload
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

//
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
GlobalVariables.Configuration = configuration;

// Inject Services 
builder.Services.AddScoped<IAppUserService, AppUserService>();
builder.Services.AddScoped<IAppRoleService, AppRoleService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationServiceService, ReservationServiceService>();
builder.Services.AddScoped<IFeedBackService, FeedBackService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IAppParameterService, AppParameterService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IStaffSpecializationService, StaffSpecializationService>();

//Add Photo Service
builder.Services.AddScoped<IPhotoService, PhotoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();