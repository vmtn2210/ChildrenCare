using System.Security.Claims;
using ChildrenCare.DTOs.ServiceDTOs;
using ChildrenCare.Entities;
using ChildrenCare.Repositories.Interfaces;
using ChildrenCare.Services.Interface;
using ChildrenCare.Utilities;
using ChildrenCare.Utilities.ImageUpload;
using ChildrenCare.Utilities.Pagination;

namespace ChildrenCare.Services;

public class ServiceService : IServiceService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPhotoService _photoService;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IReservationServiceService _reservationServiceService;
    private readonly IStaffSpecializationService _staffSpecializationService;

    public ServiceService(IPhotoService photoService, IRepositoryWrapper repositoryWrapper,
        IHttpContextAccessor httpContextAccessor, IReservationServiceService reservationServiceService,
        IStaffSpecializationService staffSpecializationService)
    {
        _photoService = photoService;
        _repositoryWrapper = repositoryWrapper;
        _httpContextAccessor = httpContextAccessor;
        _reservationServiceService = reservationServiceService;
        _staffSpecializationService = staffSpecializationService;
    }

    public async Task<IEnumerable<Service>> GetAll()
    {
        return await _repositoryWrapper.Service.FindAllAsync();
    }

    public async Task<PagedList<GetServiceListResponseDTO>> AdvancedGetServiceList(AdvancedGetServiceRequestDTO dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.SortBy) &&
            !AdvancedGetServiceRequestDTO.OrderingParams.Contains(dto.SortBy.ToLower()))
        {
            //Throw Exception
        }

        var result = await _repositoryWrapper.Service.AdvancedGetServices(dto);
        return result;
    }

    public async Task<CustomResponse> AddService(int serviceId)
    {
        //Check nếu service tồn tại
        if (!await _repositoryWrapper.Service.ServiceExists(serviceId))
            return new CustomResponse(false, "Service Doesn't Exist");

        //Lấy Id của user hiện tại
        var currentUserId = GetCurrentUserId();
        if (currentUserId < 0) return new CustomResponse(false, "Current User is not logged in");

        if (await _repositoryWrapper.Reservation.HasOngoingReservation(currentUserId))
            return new CustomResponse(false, "Customer already has ongoing reservation");

        var staffSpecialization = await _staffSpecializationService.GetFirstStaffByServiceId(serviceId);
        var staffId = staffSpecialization?.StaffId;

        var draftReservationId = await _repositoryWrapper.Reservation.GetCurrentReservationId(currentUserId);
        //If user doesn't draft reservation, create new
        if (draftReservationId is 0 or null)
        {
            //Lấy thông tin để tạo reservation mới
            var info = await _repositoryWrapper.Reservation.GetInfoForCreateNewReservation(currentUserId, serviceId);

            //Chuẩn bị reservation
            var newReservation = new Reservation
            {
                CustomerAccountId = currentUserId,
                CustomerEmail = info.Customer.CustomerEmail,
                CustomerName = info.Customer.CustomerName,
                CustomerGender = info.Customer.CustomerGender,
                CreatedDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                PhoneNumber = info.Customer.CustomerPhoneNumber,
                Status = GlobalVariables.DraftReservationStatus,
                TotalBasePrice = info.Service.ServicePrice
            };

            newReservation.ActualTotalPrice = info.Service?.ServiceSalePrice ?? newReservation.TotalBasePrice;

            //Chuẩn bị reservation detail
            var newReservationDetail = new ReservationDetail
            {
                ServiceId = serviceId,
                CreatedDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                BasePrice = info.Service.ServicePrice,
                Amount = 1,
                NumberOfPeople = 1,
                StaffId = staffId
            };

            newReservationDetail.SalePrice = info.Service.ServiceSalePrice ?? newReservationDetail.BasePrice;

            //Gắn new reservation vào
            newReservation.ReservationDetails = new List<ReservationDetail>();
            newReservation.ReservationDetails.Add(newReservationDetail);

            //Pend create change
            await _repositoryWrapper.Reservation.CreateWithoutSaveAsync(newReservation);

            //Save appointment to database
            try
            {
                //Save all changes above to database 
                await _repositoryWrapper.SaveAllAsync();
            }
            catch (Exception e)
            {
                //clear pending changes if fail
                _repositoryWrapper.DeleteChanges();
                return new CustomResponse(false,
                    "Internal Error happened when trying to add service to new reservation", e.Message);
            }
        }
        else
        {
            //Nếu user đang có draft reservation, thêm service
            //Check nếu service đã nằm trong reservation
            var existedReservationDetail =
                await _repositoryWrapper.ReservationService.FindSingleByConditionWithIncludeAsync(
                    detail => detail.ServiceId == serviceId &&
                              detail.ReservationId == draftReservationId,
                    detail => detail.Reservation);

            //Nếu ko có từ trước, tạo detail mới
            if (existedReservationDetail == null)
            {
                var info = await _repositoryWrapper.Service.GetInfoForCreateNewReservationDetail(serviceId);

                //Chuẩn bị reservation detail
                var newReservationDetail = new ReservationDetail
                {
                    ServiceId = serviceId,
                    CreatedDate = DateTime.Now,
                    LastUpdate = DateTime.Now,
                    BasePrice = info.ServicePrice,
                    Amount = 1,
                    NumberOfPeople = 1,
                    ReservationId = draftReservationId.Value,
                    StaffId = staffId
                };

                newReservationDetail.SalePrice = info.ServiceSalePrice ?? newReservationDetail.BasePrice;

                try
                {
                    await _repositoryWrapper.ReservationService.CreateAsync(newReservationDetail);
                }
                catch (Exception e)
                {
                    //clear pending changes if fail
                    _repositoryWrapper.DeleteChanges();
                    //TODO: Log Exception
                    throw new Exception(e.StackTrace);
                    return new CustomResponse(false,
                        "Internal Error happened when trying to create new reservation detail for added service",
                        e.Message);
                }

                return new CustomResponse(true, "Blog Created");
            }

            existedReservationDetail.Amount++;
            existedReservationDetail.Reservation.TotalBasePrice += existedReservationDetail.BasePrice;
            existedReservationDetail.StaffId ??= staffId;
            if (existedReservationDetail.SalePrice != null)
            {
                existedReservationDetail.Reservation.ActualTotalPrice += existedReservationDetail.SalePrice.Value;
            }
            else
            {
                existedReservationDetail.Reservation.ActualTotalPrice += existedReservationDetail.BasePrice;
            }

            try
            {
                await _repositoryWrapper.ReservationService.UpdateAsync(existedReservationDetail,
                    existedReservationDetail.Id);
            }
            catch (Exception e)
            {
                //clear pending changes if fail
                _repositoryWrapper.DeleteChanges();
                //TODO: Log Exception

                return new CustomResponse(false,
                    "Internal Error happened when trying to update amount of reservation detail for added service",
                    e.Message);
            }

            await _reservationServiceService.RecalculateTotal((int)draftReservationId);
        }

        return new CustomResponse(true, "Service added to Cart");
    }

    public async Task<CustomResponse> CreateService(CreateServiceRequestDTO dto)
    {
        if (!CheckNewServiceInfo(dto).IsSuccess)
        {
            //TODO: Handle Validation
        }

        var thumbnailUrld = "";

        if (dto.ThumbnailFile != null)
        {
            var imageUploadResult = await _photoService.AppPhotoAsync(dto.ThumbnailFile);
            //If there's error
            if (imageUploadResult.Error != null)
            {
                //TODO: Throw Exception
            }

            thumbnailUrld = imageUploadResult.SecureUrl.AbsoluteUri;
        }

        var newService = dto.ToNewService(thumbnailUrld);

        try
        {
            await _repositoryWrapper.Service.CreateAsync(newService);
        }
        catch (Exception e)
        {
            //clear pending changes if fail
            _repositoryWrapper.DeleteChanges();
            //TODO: Log Exception

            return new CustomResponse(false, e.Message);
        }

        return new CustomResponse(true, "Service Created");
    }

    public async Task<GetFeaturedServiceListResponseDTO> GetFeaturedServiceList()
    {
        return await _repositoryWrapper.Service.GetFeatureServiceList();
    }

    private CustomResponse CheckNewServiceInfo(CreateServiceRequestDTO dto)
    {
        return new CustomResponse(true);
    }

    private int GetCurrentUserId()
    {
        var currentUserId = -1;
        if (_httpContextAccessor.HttpContext == null)
        {
            //TODO: Throw Exception
        }

        try
        {
            //Get Current user Id
            currentUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        catch (ArgumentNullException e)
        {
            currentUserId = -1;
            //TODO:Throw Exception
        }

        return currentUserId;
    }

    //public Task<IEnumerable<Service>> SearchService(string title, string briedInfo)
    //{
    //    throw new NotImplementedException();
    //}
    public async Task<IEnumerable<Service>> SearchService(string status,string title, string briefInfo)
    {
        return await _repositoryWrapper.Service.FindByConditionAsync(r => r.Status.ToString().Contains(status) && r.Name.Contains(title) && r.BriefInfo.Contains(briefInfo));
    }
}