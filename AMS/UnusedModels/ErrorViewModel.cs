
/// <summary>
/// This is used to track errors when sending emails. Don't think it was ever implemented. 
/// </summary>
namespace AMS.UnusedModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}