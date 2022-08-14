namespace ChildrenCare.Utilities
{
    public class CustomResponse
    {
        
        public bool IsSuccess { get; set; }
        
        public string Message { get; set; }

        public object? DataObject { get; set; }
        
        public CustomResponse(bool isSuccess = false, string message = "", object? dataObject = null)
        {
            DataObject = dataObject;
            IsSuccess = isSuccess;
            Message = message;
        }

    }
}