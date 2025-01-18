using Dapper;
using Grpc.Core;
using GrpcServiceInteractingBetweenUsers;
using GrpcServiceInteractingBetweenUsers.Extensions;
using GrpcServiceInteractingBetweenUsers.Models;
using Npgsql;

namespace GrpcServiceInteractingBetweenUsers.Services
{
    public class RequestHandlerService : RequestHandler.RequestHandlerBase
    {
        private readonly ILogger<RequestHandlerService> _logger;
        public RequestHandlerService(ILogger<RequestHandlerService> logger)
        {
            _logger = logger;
        }

        public override async Task<HandlerFriendResponse> HandlerFriend(HandlerFriendRequest request, ServerCallContext context)
        {
            var handlerFriendResponse = new HandlerFriendResponse();
            if (request.Message == "Accept")
            {
                using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        var requestQuery = "SELECT * FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                        var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = request.Requestid });

                        var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'HandlerFriendRequest'";
                        var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                        var insertFriendTableSql = "INSERT INTO \"FriendsTable\" (\"Id\", \"SenderId\", \"ReceiverId\") VALUES (@Id, @SenderId, @ReceiverId)";
                        var insertedRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId };
                        connection.Execute(insertFriendTableSql, insertedRecord);

                        foreach (var notificationModel in notificationTableModels)
                        {
                            InsertRecordToDatabase.InsertToNotificationTable(connection, requestModel, notificationModel);
                        }

                        var deleteFriendTableSql = "DELETE FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                        var deletedRecord = new FriendModel() { Id = new Guid(request.Requestid) };
                        connection.Execute(deleteFriendTableSql, deletedRecord);
                        transaction.Commit();
                        handlerFriendResponse.Messageresponse = "Success";
                    }
                }
            }
            else if (request.Message == "Refuse")
            {
                using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        var requestQuery = "SELECT * FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                        var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = request.Requestid });

                        var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CalLEvent\" = 'HandlerFriendRequest'";
                        var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                        foreach (var notificationModel in notificationTableModels)
                        {
                            InsertRecordToDatabase.InsertToNotificationTable(connection, requestModel, notificationModel);
                        }

                        var deleteFriendTableSql = "DELETE FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                        var deletedRecord = new FriendModel() { Id = new Guid(request.Requestid) };
                        connection.Execute(deleteFriendTableSql, deletedRecord);
                        transaction.Commit();
                        handlerFriendResponse.Messageresponse = "Rejected";
                    }
                }
            }
            return handlerFriendResponse;
        }

        public override async Task<HandlerFriendResponse> HandlerFriends(IAsyncStreamReader<HandlerFriendRequest> requestStream, ServerCallContext context)
        {
            var handlerFriendResponse = new HandlerFriendResponse();
            handlerFriendResponse.Messageresponse = "Fail";
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var requestHandler in requestStream.ReadAllAsync())
                    {
                        if (requestHandler.Message == "Accept")
                        {
                            var requestQuery = "SELECT * FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                            var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = requestHandler.Requestid });

                            var insertFriendTableSql = "INSERT INTO \"FriendsTable\" (\"Id\", \"SenderId\", \"ReceiverId\") VALUES (@Id, @SenderId, @ReceiverId)";
                            var insertedRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId };
                            connection.Execute(insertFriendTableSql, insertedRecord);

                            var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'HandlerFriendRequest'";
                            var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                            foreach (var notificationModel in notificationTableModels)
                            {
                                InsertRecordToDatabase.InsertToNotificationTable(connection, requestModel, notificationModel);
                            }

                            var deleteFriendTableSql = "DELETE FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                            var deletedRecord = new FriendModel() { Id = new Guid(requestHandler.Requestid) };
                            connection.Execute(deleteFriendTableSql, deletedRecord);
                        }
                        else if (requestHandler.Message == "Refuse")
                        {
                            var requestQuery = "SELECT * FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                            var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = requestHandler.Requestid });

                            var getNotificationTableQuery = "SELECT \"NotificationTable\" FROM \"NotificationEventsTable\" WHERE \"CallEvent\" = 'HandlerFriendRequest'";
                            var notificationTableModels = connection.Query<string>(getNotificationTableQuery);

                            foreach (var notificationModel in notificationTableModels)
                            {
                                InsertRecordToDatabase.InsertToNotificationTable(connection, requestModel, notificationModel);
                            }

                            var deleteFriendTableSql = "DELETE FROM \"RequestsTable\" WHERE \"Id\" = CAST(@Id AS uuid)";
                            var deletedRecord = new FriendModel() { Id = new Guid(requestHandler.Requestid) };
                            connection.Execute(deleteFriendTableSql, deletedRecord);
                        }
                    }
                    handlerFriendResponse.Messageresponse = "Success";
                    transaction.Commit();
                }
            }
            return handlerFriendResponse;
        }

        public override async Task GetAllRequest(GetAllRequestsRequest request, IServerStreamWriter<GetAllRequestsResponse> response, ServerCallContext context)
        {
            using (var connection = new NpgsqlConnection("Host=10.99.62.254;Username=postgres;Password=Bu6!ERGA@2024;Database=SocialDatabase"))
            {
                var requestQuery = "SELECT * FROM \"RequestsTable\" WHERE \"SenderId\" = CAST(@UserId AS uuid)";
                var requestModels = connection.Query<RequestModel>(requestQuery, new { UserId = request.Userid });
                foreach (var requestModel in requestModels)
                {
                    var getRequestResponse = new GetAllRequestsResponse
                    {
                        Message = "Friend Request",
                        Messagetype = "Add Friend",
                        Requestid = requestModel.Id.ToString(),
                        Senderid = requestModel.SenderId.ToString()
                    };
                    await response.WriteAsync(getRequestResponse);
                }
            }
        }

    }
}
