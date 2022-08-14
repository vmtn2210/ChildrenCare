namespace ChildrenCare.DTOs.ReservationDTOs;

public class GetInfoForCreateNewReservationCustomerDTO
{
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public bool CustomerGender { get; set; }
    public string? CustomerPhoneNumber { get; set; }
}

public class GetInfoForCreateNewReservationServiceDTO
{
    public long ServicePrice { get; set; }
    public long? ServiceSalePrice { get; set; }
}

public class GetInfoForCreateNewReservationDTO
{
    public GetInfoForCreateNewReservationCustomerDTO? Customer { get; set; }
    public GetInfoForCreateNewReservationServiceDTO? Service { get; set; }
}