namespace ChildrenCare.Repositories.Interfaces
{
    public interface IRepositoryWrapper
    {
        //For saving multiple changes
        Task<bool> SaveAllAsync();

        void DeleteChanges();

        bool HasChanged();

        public IUserRepository User { get; }
        public IServiceRepository Service { get; }
        public IBlogRepository Blog { get; }
        public ICategoryRepository Category { get; }
        public IFeedbackRepository Feedback { get; }
        public IReservationRepository Reservation { get; }
        public IReservationServiceRepository ReservationService { get; }
        public IPrescriptionRepository Prescription { get; }
        public ISliderRepository Slider { get; }
        public IParameterRepository Parameter { get; }
        public IStaffSpecializationRepository StaffSpecialization { get; }
        public IAppUserRoleRepository AppUserRole { get; }
    }
}
