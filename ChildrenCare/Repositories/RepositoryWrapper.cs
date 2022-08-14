using ChildrenCare.Data;
using ChildrenCare.Repositories.Interfaces;
using HairCutAppAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenCare.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ChildrenCareDBContext _context;
        private IUserRepository _user;
        private IBlogRepository _blog;
        private IServiceRepository _service;
        private IParameterRepository _parameter;
        private IReservationRepository _reservation;
        private IReservationServiceRepository _reservationService;
        private IPrescriptionRepository _prescription;
        private IFeedbackRepository _feedback;
        private ICategoryRepository _category;
        private ISliderRepository _slider;
        private IStaffSpecializationRepository _staffSpecialization;
        private IAppUserRoleRepository _appUserRoleRepository;
        public RepositoryWrapper(ChildrenCareDBContext context)
        {
            _context = context;
        }

        #region Create concrete repositories if there aren't
        
        public IUserRepository User => _user ??= new UserRepository(_context);
        public IBlogRepository Blog => _blog ??= new BlogRepository(_context);
        public IServiceRepository Service => _service ??= new ServiceRepository(_context);
        public IParameterRepository Parameter => _parameter ??= new ParameterRepository(_context);
        public IReservationRepository Reservation => _reservation ??= new ReservationRepository(_context);
        public IReservationServiceRepository ReservationService => _reservationService ??= new ReservationServiceRepository(_context);
        public IPrescriptionRepository Prescription => _prescription ??= new PrescriptionRepository(_context);
        public IFeedbackRepository Feedback => _feedback ??= new FeedbackRepository(_context);
        public ICategoryRepository Category => _category ??= new CategoryRepository(_context);
        public ISliderRepository Slider => _slider ??= new SliderRepository(_context);
        public IAppUserRoleRepository AppUserRole => _appUserRoleRepository ??= new AppUserRoleRepository(_context);
        public IStaffSpecializationRepository StaffSpecialization => _staffSpecialization ??= new StaffSpecializationRepository(_context);

        #endregion Create concrete repositories if there aren't

        //For saving multiple changes, if lower than 0 -> no changes
        public async Task<bool> SaveAllAsync()
        { 
            return await _context.SaveChangesAsync() > 0;
        }
        
        public bool HasChanged()
        {
            return _context.ChangeTracker.HasChanges();
        }

        
        public void DeleteChanges()
        {
            var changedEntriesCopy = _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
        
    }
}