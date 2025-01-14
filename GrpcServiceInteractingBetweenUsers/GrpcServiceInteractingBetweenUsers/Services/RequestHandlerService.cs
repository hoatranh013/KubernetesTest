using Dapper;
using Grpc.Core;
using GrpcServiceInteractingBetweenUsers;
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
                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
                {
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        var requestQuery = "SELECT * FROM RequestsTable WHERE id = @Id";
                        var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = request.Requestid });

                        var insertFriendTableSql = "INSERT INTO FriendsTable (Id, SenderId, ReceiverId) VALUES (@Id, @SenderId, @ReceiverId)";
                        var insertedRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId };
                        connection.Execute(insertFriendTableSql, insertedRecord);

                        var insertFriendNotificationTableSql = "INSERT INTO FriendNotificationsTable (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                        var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId, Status = "Accepted" };
                        connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                        var deleteFriendTableSql = "DELETE FROM RequestsTable WHERE Id = @Id";
                        var deletedRecord = new FriendModel() { Id = new Guid(request.Requestid) };
                        connection.Execute(deleteFriendTableSql, deletedRecord);
                        transaction.Commit();
                        handlerFriendResponse.Messageresponse = "Success";
                    }
                }
            }
            else if (request.Message == "Refuse")
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
                {
                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                    {
                        var requestQuery = "SELECT * FROM RequestsTable WHERE id = @Id";
                        var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = request.Requestid });

                        var insertFriendNotificationTableSql = "INSERT INTO FriendNotificationsTable (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                        var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId, Status = "Refused" };
                        connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                        var deleteFriendTableSql = "DELETE FROM RequestsTable WHERE Id = @Id";
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
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
            {
                using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    await foreach (var requestHandler in requestStream.ReadAllAsync())
                    {
                        if (requestHandler.Message == "Accept")
                        {
                            var requestQuery = "SELECT * FROM RequestsTable WHERE id = @Id";
                            var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = requestHandler.Requestid });

                            var insertFriendTableSql = "INSERT INTO FriendsTable (Id, SenderId, ReceiverId) VALUES (@Id, @SenderId, @ReceiverId)";
                            var insertedRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId };
                            connection.Execute(insertFriendTableSql, insertedRecord);

                            var insertFriendNotificationTableSql = "INSERT INTO FriendNotificationsTable (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                            var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId, Status = "Accepted" };
                            connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                            var deleteFriendTableSql = "DELETE FROM RequestsTable WHERE Id = @Id";
                            var deletedRecord = new FriendModel() { Id = new Guid(requestHandler.Requestid) };
                            connection.Execute(deleteFriendTableSql, deletedRecord);
                        }
                        else if (requestHandler.Message == "Refuse")
                        {
                            var requestQuery = "SELECT * FROM RequestsTable WHERE id = @Id";
                            var requestModel = connection.QuerySingle<RequestModel>(requestQuery, new { Id = requestHandler.Requestid });

                            var insertFriendNotificationTableSql = "INSERT INTO FriendNotificationsTable (Id, SenderId, ReceiverId, Status) VALUES (@Id, @SenderId, @ReceiverId, @Status)";
                            var insertedNotificationRecord = new FriendModel() { Id = Guid.NewGuid(), SenderId = requestModel.SenderId, ReceiverId = requestModel.ReceiverId, Status = "Refused" };
                            connection.Execute(insertFriendNotificationTableSql, insertedNotificationRecord);

                            var deleteFriendTableSql = "DELETE FROM RequestsTable WHERE Id = @Id";
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
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=rsoOPR45;Database=SocialDatabase"))
            {
                var requestQuery = "SELECT * FROM RequestsTable WHERE UserId = @UserId";
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
