namespace ChildrenCare.Utilities
{
    //Store Variables used throughout the app
    public static class GlobalVariables
    {
        public static IConfiguration Configuration { get; set; }

        //Regex
        public const string DateRegex = @"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$";
        public const string EmailRegex = @"^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$";
        public const string PhoneNumberRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})";
        
        //Date Formats
        public const int TimeZone = 7;
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public const string DayFormat = "dd/MM/yyyy";
        public const string TimeFormat = @"hh\:mm";
        
        //Name of roles
        public const int AdministratorRoleId = 1;
        public const int ManagerRoleId = 2;
        public const int DoctorRoleId = 3;
        public const int NurseRoleId = 4;
        public const int CustomerRoleId = 5;
        public const string AdministratorRole = "admin";
        public const string ManagerRole = "manager";
        public const string DoctorRole = "doctor";
        public const string NurseRole = "nurse";
        public const string CustomerRole = "customer";

        // User Statuses
        public const int UserStatusParameterTypeId = 1;

        public const int ActiveUserStatusId = 1;
        public const int InActiveUserStatusId = 2;
        
        //Service Status
        public const int ServiceStatusParameterTypeId = 2;

        public const int ActiveServiceStatusId = 3;
        public const int InActiveServiceStatusId = 4;
        public const int NewServiceStatusId = 5;


        //Blog Status
        public const int BlogStatusParameterTypeId = 3;

        public const int ActiveBlogStatusId = 6;
        public const int InActiveBlogStatusId = 7;
        public const int NewBlogStatusId = 8;

        //Reservation Status
        public const int ReservationStatusParameterType = 4;

        public const int SubmittedReservationStatus = 9;
        public const int ApprovedReservationStatus = 10;
        public const int SuccessReservationStatus = 11;
        public const int CancelledReservationStatus = 12;
        public const int DraftReservationStatus = 16;

        //Slider Status
        public const int SliderStatusParameterTypeId = 5;

        public const int ActiveSliderStatusId = 13;
        public const int InActiveSliderStatusId = 14;
        public const int NewSliderStatusId = 15;

        public enum GenderType
        {
            Male = 1,
            Female = 2
        }
    }
}