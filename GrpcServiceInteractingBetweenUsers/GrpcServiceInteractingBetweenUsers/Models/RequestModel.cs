namespace GrpcServiceInteractingBetweenUsers.Models
{
    public class RequestModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
