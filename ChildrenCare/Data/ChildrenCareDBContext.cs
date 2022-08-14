using ChildrenCare.Entities;
using ChildrenCare.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Data;

public class ChildrenCareDBContext : IdentityDbContext<AppUser, AppRole, int, // TKey
    IdentityUserClaim<int>, // TUserClaim
    AppUserRole, // TUserRole,
    IdentityUserLogin<int>, // TUserLogin
    IdentityRoleClaim<int>, // TRoleClaim
    IdentityUserToken<int>> // TUserToken>
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<AppParameter> AppParameters { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationDetail> ReservationDetails { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<FeedBacks> FeedBacks { get; set; }
    public DbSet<StaffSpecialization> StaffSpecializations { get; set; }
    public DbSet<AppUserRole> UserRoles { get; set; }

    public ChildrenCareDBContext(DbContextOptions<ChildrenCareDBContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Prescription>(entity =>
        {
            entity.HasIndex(e => e.AuthorAccountId, "IX_Prescriptions_AuthorAccountId");

            entity.HasIndex(e => e.CustomerAccountId, "IX_Prescriptions_CustomerAccountId");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AuthorAccount)
                .WithMany(p => p.PrescriptionAuthorAccounts)
                .HasForeignKey(d => d.AuthorAccountId);

            entity.HasOne(d => d.CustomerAccount)
                .WithMany(p => p.PrescriptionCustomerAccounts)
                .HasForeignKey(d => d.CustomerAccountId);

            entity.HasOne(d => d.Reservation)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("FK_Prescriptions_Reservations");
        });

        // builder.Entity<AppUserRole>().HasKey(sc => new { sc.RoleId, sc.UserId });

        builder.Entity<AppUserRole>()
            .HasOne<AppUser>(r => r.User)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(x => x.UserId);

        builder.Entity<AppUserRole>()
            .HasOne<AppRole>(r => r.Role)
            .WithMany(user => user.UserRoles)
            .HasForeignKey(x => x.RoleId);

        #region Seed Database

        builder.Entity<AppRole>().HasData(
            new AppRole() { Id = 1, Name = "admin", NormalizedName = "ADMIN" },
            new AppRole() { Id = 2, Name = "manager", NormalizedName = "MANAGER" },
            new AppRole() { Id = 3, Name = "doctor", NormalizedName = "DOCTOR" },
            new AppRole() { Id = 4, Name = "nurse", NormalizedName = "NURSE" },
            new AppRole() { Id = 5, Name = "customer", NormalizedName = "CUSTOMER" });

        builder.Entity<Category>().HasData(
            new Category() { Id = 1, Name = "Cardiology" },
            new Category() { Id = 2, Name = "Neurology" },
            new Category() { Id = 3, Name = "News" },
            new Category() { Id = 4, Name = "Opinion" },
            new Category() { Id = 5, Name = "Scientific Paper" },
            new Category() { Id = 6, Name = "Advertisement" });

        builder.Entity<Service>().HasData(
            new Service()
            {
                Id = 1,
                Name = "Cardiology",
                BriefInfo = "Treatment of disorders of the heart and the blood vessels",
                Description =
                    @"Cardiology is a branch of internal medicine. A cardiologist is not the same as a cardiac surgeon. 
                A cardiac surgeon opens the chest and performs heart surgery.
                A cardiologist specializes in diagnosing and treating diseases of the cardiovascular system.The cardiologist will carry out tests,
                and they may perform some procedures,
                such as heart catheterizations,
                angioplasty,
                or inserting a pacemaker.
                Heart disease relates specifically to the heart, while cardiovascular disease affects the heart, the blood vessels, or both.
                To become a cardiologist in the United States, it is necessary to complete 4 years of medical school, 3 years of training in internal medicine, and at least 3 years specializing in cardiology.",
                Price = 250000,
                SalePrice = 200000,
                Status = 1,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            },
            new Service()
            {
                Id = 2,
                Name = "Surgery",
                BriefInfo = "BriefInfo of Surgery",
                Description = @"Long Description of Surgery",
                Price = 300000,
                SalePrice = 250000,
                Status = 1,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            },
            new Service()
            {
                Id = 3,
                Name = "General Examination",
                BriefInfo = "BriefInfo of General Examination",
                Description = @"Long Description of General Examination",
                Price = 500000,
                SalePrice = 400000,
                Status = 1,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            },
            new Service()
            {
                Id = 4,
                Name = "Diagnosis",
                BriefInfo = "BriefInfo of Diagnosis",
                Description = @"Long Description of Diagnosis",
                Price = 100000,
                SalePrice = 80000,
                Status = 1,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            },
            new Service()
            {
                Id = 5,
                Name = "Digital Testing",
                BriefInfo = "BriefInfo of Digital Testing",
                Description = @"Long Description of Digital Testing",
                Price = 100000,
                SalePrice = 80000,
                Status = 1,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            },
            new Service()
            {
                Id = 6,
                Name = "Fake Service",
                BriefInfo = "BriefInfo of Fake Service",
                Description = @"Long Description of Fake Service",
                Price = 999999999,
                SalePrice = 999999,
                Status = 2,
                ThumbnailUrl = "https://placeimg.com/500/500/tech",
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now
            });

        builder.Entity<AppParameter>().HasData(
            new AppParameter()
            {
                Id = 1, Name = "Active User Status Id", Type = GlobalVariables.UserStatusParameterTypeId, Value = "",
                Description = "Status that a user account has when it's active", DisplayName = "Active"
            },
            new AppParameter()
            {
                Id = 2, Name = "Inactive User Status Id", Type = GlobalVariables.UserStatusParameterTypeId, Value = "",
                Description = "Status that a user account has when it's not active", DisplayName = "Inactive"
            },
            new AppParameter()
            {
                Id = 3, Name = "Active Service Status Id", Type = GlobalVariables.ServiceStatusParameterTypeId,
                Value = "", Description = "Status that a service has when it's active", DisplayName = "Active"
            },
            new AppParameter()
            {
                Id = 4, Name = "Inactive Service Status Id", Type = GlobalVariables.ServiceStatusParameterTypeId,
                Value = "", Description = "Status that a service has when it's not active", DisplayName = "Inactive"
            },
            new AppParameter()
            {
                Id = 5, Name = "New Service Status Id", Type = GlobalVariables.ServiceStatusParameterTypeId,
                Value = "3", Description = "Status that a service will have when it's created",
                DisplayName = "New Service Status Id"
            },
            new AppParameter()
            {
                Id = 6, Name = "Active Blog Status Id", Type = GlobalVariables.BlogStatusParameterTypeId, Value = "",
                Description = "Status that a blog has when it's active", DisplayName = "Active"
            },
            new AppParameter()
            {
                Id = 7, Name = "Inactive Blog Status Id", Type = GlobalVariables.BlogStatusParameterTypeId, Value = "",
                Description = "Status that a blog has when it's not active", DisplayName = "Inactive"
            },
            new AppParameter()
            {
                Id = 8, Name = "New Blog Status Id", Type = GlobalVariables.BlogStatusParameterTypeId, Value = "6",
                Description = "Status that a blog will have when it's created", DisplayName = "New Blog Status Id"
            },
            new AppParameter()
            {
                Id = 9, Name = "Submitted Reservation Status Id", Type = GlobalVariables.ReservationStatusParameterType,
                Value = "", Description = "Status that a reservation will have when it's created by customer",
                DisplayName = "Submitted"
            },
            new AppParameter()
            {
                Id = 10, Name = "Approved Reservation Status Id", Type = GlobalVariables.ReservationStatusParameterType,
                Value = "", Description = "Status that a reservation will have when it's approved by manager",
                DisplayName = "Approved"
            },
            new AppParameter()
            {
                Id = 11, Name = "Success Reservation Status Id", Type = GlobalVariables.ReservationStatusParameterType,
                Value = "", Description = "Status that a reservation will have when it's marked success by manager",
                DisplayName = "Success"
            },
            new AppParameter()
            {
                Id = 12, Name = "Cancelled Reservation Status Id",
                Type = GlobalVariables.ReservationStatusParameterType, Value = "",
                Description = "Status that a reservation will have when it's cancelled by customer or manager",
                DisplayName = "Cancelled"
            },
            new AppParameter()
            {
                Id = 13, Name = "Active Slider Status Id", Type = GlobalVariables.SliderStatusParameterTypeId,
                Value = "", Description = "Status that a Slider has when it's active", DisplayName = "Active"
            },
            new AppParameter()
            {
                Id = 14, Name = "Inactive Slider Status Id", Type = GlobalVariables.SliderStatusParameterTypeId,
                Value = "", Description = "Status that a Slider has when it's not active", DisplayName = "Inactive"
            },
            new AppParameter()
            {
                Id = 15, Name = "New Slider Status Id", Type = GlobalVariables.SliderStatusParameterTypeId,
                Value = "13", Description = "Status that a Slider will have when it's created",
                DisplayName = "New Slider Status Id"
            },
            new AppParameter()
            {
                Id = 16, Name = "Draft Reservation Status Id", Type = GlobalVariables.ReservationStatusParameterType,
                Value = "",
                Description = "Status that a Reservation will have when it's created but not submitted by customer",
                DisplayName = "Draft"
            });

        builder.Entity<AppUser>().HasData(
            new AppUser()
            {
                Id = 1,
                FullName = "Doctor 1",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "doctor1@gmail.com",
                NormalizedUserName = "DOCTOR1@GMAIL.COM",
                Email = "doctor1@gmail.com",
                NormalizedEmail = "DOCTOR1@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 2,
                FullName = "Doctor 2",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "doctor2@gmail.com",
                NormalizedUserName = "DOCTOR2@GMAIL.COM",
                Email = "doctor2@gmail.com",
                NormalizedEmail = "DOCTOR2@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 3,
                FullName = "Doctor 3",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "doctor3@gmail.com",
                NormalizedUserName = "DOCTOR3@GMAIL.COM",
                Email = "doctor3@gmail.com",
                NormalizedEmail = "DOCTOR3@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 4,
                FullName = "Nurse 1",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "nurse1@gmail.com",
                NormalizedUserName = "NURSE1@GMAIL.COM",
                Email = "nurse1@gmail.com",
                NormalizedEmail = "NURSE1@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 5,
                FullName = "Nurse 2",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "nurse2@gmail.com",
                NormalizedUserName = "NURSE2@GMAIL.COM",
                Email = "nurse2@gmail.com",
                NormalizedEmail = "NURSE2@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 6,
                FullName = "Nurse 3",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "nurse3@gmail.com",
                NormalizedUserName = "NURSE3@GMAIL.COM",
                Email = "nurse3@gmail.com",
                NormalizedEmail = "NURSE3@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 7,
                FullName = "Admin 1",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "admin1@gmail.com",
                NormalizedUserName = "ADMIN1@GMAIL.COM",
                Email = "admin1@gmail.com",
                NormalizedEmail = "ADMIN1@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            },
            new AppUser()
            {
                Id = 8,
                FullName = "Manager 1",
                Gender = true,
                IsPotentialCustomer = false,
                UserName = "manager1@gmail.com",
                NormalizedUserName = "MANAGER1@GMAIL.COM",
                Email = "manager1@gmail.com",
                NormalizedEmail = "MANAGER1@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                PhoneNumberConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEGd0+eWoiCIzUIjCGM6x0nuxNzgcfiRNj6zfyYnSfim2h3tuzvYAjo9gymyJD7uCgg=="
            });

        #endregion Seed Database
    }
}