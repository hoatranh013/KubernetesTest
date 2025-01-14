// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/requesthandlerrequest.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GrpcServiceInteractingBetweenUsers {
  /// <summary>
  /// The greeting service definition.
  /// </summary>
  public static partial class RequestHandler
  {
    static readonly string __ServiceName = "requesthandlerrequest.RequestHandler";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest> __Marshaller_requesthandlerrequest_HandlerFriendRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse> __Marshaller_requesthandlerrequest_HandlerFriendResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest> __Marshaller_requesthandlerrequest_GetAllRequestsRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse> __Marshaller_requesthandlerrequest_GetAllRequestsResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse> __Method_HandlerFriend = new grpc::Method<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "HandlerFriend",
        __Marshaller_requesthandlerrequest_HandlerFriendRequest,
        __Marshaller_requesthandlerrequest_HandlerFriendResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse> __Method_HandlerFriends = new grpc::Method<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "HandlerFriends",
        __Marshaller_requesthandlerrequest_HandlerFriendRequest,
        __Marshaller_requesthandlerrequest_HandlerFriendResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest, global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse> __Method_GetAllRequest = new grpc::Method<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest, global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GetAllRequest",
        __Marshaller_requesthandlerrequest_GetAllRequestsRequest,
        __Marshaller_requesthandlerrequest_GetAllRequestsResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GrpcServiceInteractingBetweenUsers.RequesthandlerrequestReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of RequestHandler</summary>
    [grpc::BindServiceMethod(typeof(RequestHandler), "BindService")]
    public abstract partial class RequestHandlerBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse> HandlerFriend(global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse> HandlerFriends(grpc::IAsyncStreamReader<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task GetAllRequest(global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest request, grpc::IServerStreamWriter<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(RequestHandlerBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_HandlerFriend, serviceImpl.HandlerFriend)
          .AddMethod(__Method_HandlerFriends, serviceImpl.HandlerFriends)
          .AddMethod(__Method_GetAllRequest, serviceImpl.GetAllRequest).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, RequestHandlerBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_HandlerFriend, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse>(serviceImpl.HandlerFriend));
      serviceBinder.AddMethod(__Method_HandlerFriends, serviceImpl == null ? null : new grpc::ClientStreamingServerMethod<global::GrpcServiceInteractingBetweenUsers.HandlerFriendRequest, global::GrpcServiceInteractingBetweenUsers.HandlerFriendResponse>(serviceImpl.HandlerFriends));
      serviceBinder.AddMethod(__Method_GetAllRequest, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::GrpcServiceInteractingBetweenUsers.GetAllRequestsRequest, global::GrpcServiceInteractingBetweenUsers.GetAllRequestsResponse>(serviceImpl.GetAllRequest));
    }

  }
}
#endregion
