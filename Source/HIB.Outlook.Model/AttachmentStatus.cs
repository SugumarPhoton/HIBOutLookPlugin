
namespace HIB.Outlook.Model
{
    public class AttachmentStatus
    {
        public string ErrorMessage { get; set; }
        public Status Status { get; set; } = Status.InProgress;
    }

    public enum Status
    {
        Success = 1,
        InProgress = 2,
        Failed = 3
    }
}
