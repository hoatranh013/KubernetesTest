using Dapper;
using Grpc.Core;
using GrpcServiceInteractingBetweenUsers;
using GrpcServiceInteractingBetweenUsers.Extensions;
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
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var insertIntoRequestTable = "INSERT INTO \"RequestsTable\"(\"Id\", \"SenderId\", \"ReceiverId\", \"Status\", \"Message\") VALUES (@Id, @SenderId, @ReceiverId, @Status, @Message)";
                    var insertedRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending", Message = request.Message };
                    connection.Execute(insertIntoRequestTable, insertedRecord);

                    var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'SendFriendRequest'";
                    var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                    foreach (var notificationModel in notificationTableModels)
                    {
                        InsertRecordToDatabase.InsertToNotificationTable(connection, insertedRecord, notificationModel);
                    }

                    transaction.Commit();
                }
            }
            return new SendFriendRequestServiceResponse
            {
                Message = "Success"
            };
        }

        public override async Task<SendFriendRequestServiceResponse> AddFriends(IAsyncStreamReader<SendFriendRequestServiceRequest> requestStreams, ServerCallContext context)
        {
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var request in requestStreams.ReadAllAsync())
                    {
                        var insertIntoRequestTable = "INSERT INTO \"RequestsTable\"(\"Id\", \"SenderId\", \"ReceiverId\", \"Status\", \"Message\") VALUES (@Id, @SenderId, @ReceiverId, @Status, @Message)";
                        var insertedRecord = new RequestModel() { Id = Guid.NewGuid(), SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid), Status = "Pending", Message = request.Message };
                        connection.Execute(insertIntoRequestTable, insertedRecord);

                        var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'SendFriendRequest'";
                        var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                        foreach (var notificationModel in notificationTableModels)
                        {
                            InsertRecordToDatabase.InsertToNotificationTable(connection, insertedRecord, notificationModel);
                        }
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
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    var deleteIntoRequestTable = "DELETE FROM \"FriendsTable\" WHERE \"SenderId\" = @SenderId OR \"ReceiverId\" = @ReceiverId";
                    var deletedRecord = new FriendModel() { SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid) };
                    connection.Execute(deleteIntoRequestTable, deletedRecord);

                    var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'DeleteFriendRequest'";
                    var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                    foreach (var notificationModel in notificationTableModels)
                    {
                        InsertRecordToDatabase.InsertToNotificationTable(connection, deletedRecord, notificationModel);
                    }

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
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var request in requestStreams.ReadAllAsync())
                    {
                        var deleteIntoRequestTable = "DELETE FROM \"FriendsTable\" WHERE \"SenderId\" = @SenderId OR \"ReceiverId\" = @ReceiverId";
                        var deletedRecord = new FriendModel() { SenderId = new Guid(request.Senderid), ReceiverId = new Guid(request.Receiverid) };
                        connection.Execute(deleteIntoRequestTable, deletedRecord);

                        var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'DeleteFriendRequest'";
                        var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                        foreach (var notificationModel in notificationTableModels)
                        {
                            InsertRecordToDatabase.InsertToNotificationTable(connection, deletedRecord, notificationModel);
                        }
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
