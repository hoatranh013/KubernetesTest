using Dapper;
using Grpc.Core;
using GrpcServiceInteractingBetweenUsers;
using GrpcServiceInteractingBetweenUsers.Models;
using Npgsql;

namespace GrpcServiceInteractingBetweenUsers.Services
{
    public class SendFriendRequestHandlerService : SendFriendRequestHandler.SendFriendRequestHandlerBase
    {
        private readonly ILogger<SendFriendRequestHandlerService> _logger;
        public SendFriendRequestHandlerService(ILogger<SendFriendRequestHandlerService> logger)
        {
            _logger = logger;
        }

        public override async Task<SendFriendRequestServiceResponse> AddFriend(SendFriendRequestServiceRequest request, ServerCallContext context)
        {
            try
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            var insertIntoRequestTable = "INSERT INTO \"RequestsTable\"(\"Id\", \"SenderId\", \"ReceiverId\", \"Status\") VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                            var insertedRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                            connection.Execute(insertIntoRequestTable, insertedRecord);

                            var insertFriendNotificationTableSql = "INSERT INTO \"RequestsTableNotification\" (\"Id\", \"SenderId\", \"ReceiverId\", \"Status\") VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                            var insertedNotificationRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                            connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return new SendFriendRequestServiceResponse
            {
                Message = "Success"
            };
        }

        public override async Task<SendFriendRequestServiceResponse> AddFriends(IAsyncStreamReader<SendFriendRequestServiceRequest> requestStreams, ServerCallContext context)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var request in requestStreams.ReadAllAsync())
                    {
                        var insertIntoRequestTable = "INSERT INTO RequestsTable(Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                        var insertedRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                        connection.Execute(insertIntoRequestTable, insertedRecord);

                        var insertFriendNotificationTableSql = "INSERT INTO RequestsTableNotification (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                        var insertedNotificationRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                        connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);
                    }
                    transaction.Commit();
                }
            }
            return new SendFriendRequestServiceResponse
            {
                Message = "Success"
            };
        }

        public override async Task<SendFriendRequestServiceResponse> DeleteFriend(SendFriendRequestServiceRequest request, ServerCallContext context)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var deleteIntoRequestTable = "DELETE FROM FriendsTable WHERE SenderId = @SenderId OR ReceiverId = @ReceiverId";
                    var deletedRecord = new FriendModel() { SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid) };
                    connection.Execute(deleteIntoRequestTable, deletedRecord);

                    var insertFriendNotificationTableSql = "INSERT INTO DeleteFriendsNotification (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                    var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                    connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                    transaction.Commit();
                }
            }
            return new SendFriendRequestServiceResponse
            {
                Message = "Success"
            };
        }

        public override async Task<SendFriendRequestServiceResponse> DeleteFriends(IAsyncStreamReader<SendFriendRequestServiceRequest> requestStreams, ServerCallContext context)
        {
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var request in requestStreams.ReadAllAsync())
                    {
                        var deleteIntoRequestTable = "DELETE FROM FriendsTable WHERE SenderId = @SenderId OR ReceiverId = @ReceiverId";
                        var deletedRecord = new FriendModel() { SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid) };
                        connection.Execute(deleteIntoRequestTable, deletedRecord);

                        var insertFriendNotificationTableSql = "INSERT INTO DeleteFriendsNotification (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                        var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending" };
                        connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);
                    }
                    transaction.Commit();
                }
            }
            return new SendFriendRequestServiceResponse
            {
                Message = "Success"
            };
        }
    }
}
