// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: main.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GRPCClient {
  public static partial class MainService
  {
    static readonly string __ServiceName = "main.MainService";

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
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_HandshakePost> __Marshaller_main_GRPC_HandshakePost = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_HandshakePost.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_HandshakeGet> __Marshaller_main_GRPC_HandshakeGet = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_HandshakeGet.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_NHandshakePost> __Marshaller_main_GRPC_NHandshakePost = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_NHandshakePost.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_NHandshakeGet> __Marshaller_main_GRPC_NHandshakeGet = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_NHandshakeGet.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_PingPost> __Marshaller_main_GRPC_PingPost = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_PingPost.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_PingGet> __Marshaller_main_GRPC_PingGet = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_PingGet.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_NetObjUpdate> __Marshaller_main_GRPC_NetObjUpdate = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_NetObjUpdate.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_EmptyMsg> __Marshaller_main_GRPC_EmptyMsg = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_EmptyMsg.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_NetVarUpdate> __Marshaller_main_GRPC_NetVarUpdate = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_NetVarUpdate.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GRPCClient.GRPC_GenericValue> __Marshaller_main_GRPC_GenericValue = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GRPCClient.GRPC_GenericValue.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_HandshakePost, global::GRPCClient.GRPC_HandshakeGet> __Method_GRPC_Handshake = new grpc::Method<global::GRPCClient.GRPC_HandshakePost, global::GRPCClient.GRPC_HandshakeGet>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GRPC_Handshake",
        __Marshaller_main_GRPC_HandshakePost,
        __Marshaller_main_GRPC_HandshakeGet);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_NHandshakePost, global::GRPCClient.GRPC_NHandshakeGet> __Method_GRPC_NetcodeHandshake = new grpc::Method<global::GRPCClient.GRPC_NHandshakePost, global::GRPCClient.GRPC_NHandshakeGet>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GRPC_NetcodeHandshake",
        __Marshaller_main_GRPC_NHandshakePost,
        __Marshaller_main_GRPC_NHandshakeGet);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_PingPost, global::GRPCClient.GRPC_PingGet> __Method_GRPC_Ping = new grpc::Method<global::GRPCClient.GRPC_PingPost, global::GRPCClient.GRPC_PingGet>(
        grpc::MethodType.DuplexStreaming,
        __ServiceName,
        "GRPC_Ping",
        __Marshaller_main_GRPC_PingPost,
        __Marshaller_main_GRPC_PingGet);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_NetObjUpdate, global::GRPCClient.GRPC_EmptyMsg> __Method_GRPC_SrvNetObjUpdate = new grpc::Method<global::GRPCClient.GRPC_NetObjUpdate, global::GRPCClient.GRPC_EmptyMsg>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "GRPC_SrvNetObjUpdate",
        __Marshaller_main_GRPC_NetObjUpdate,
        __Marshaller_main_GRPC_EmptyMsg);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_EmptyMsg, global::GRPCClient.GRPC_NetObjUpdate> __Method_GRPC_CliNetObjUpdate = new grpc::Method<global::GRPCClient.GRPC_EmptyMsg, global::GRPCClient.GRPC_NetObjUpdate>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GRPC_CliNetObjUpdate",
        __Marshaller_main_GRPC_EmptyMsg,
        __Marshaller_main_GRPC_NetObjUpdate);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_NetVarUpdate, global::GRPCClient.GRPC_EmptyMsg> __Method_GRPC_SrvNetVarUpdate = new grpc::Method<global::GRPCClient.GRPC_NetVarUpdate, global::GRPCClient.GRPC_EmptyMsg>(
        grpc::MethodType.ClientStreaming,
        __ServiceName,
        "GRPC_SrvNetVarUpdate",
        __Marshaller_main_GRPC_NetVarUpdate,
        __Marshaller_main_GRPC_EmptyMsg);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GRPCClient.GRPC_GenericValue, global::GRPCClient.GRPC_NetVarUpdate> __Method_GRPC_CliNetNetVarUpdate = new grpc::Method<global::GRPCClient.GRPC_GenericValue, global::GRPCClient.GRPC_NetVarUpdate>(
        grpc::MethodType.ServerStreaming,
        __ServiceName,
        "GRPC_CliNetNetVarUpdate",
        __Marshaller_main_GRPC_GenericValue,
        __Marshaller_main_GRPC_NetVarUpdate);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GRPCClient.MainReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of MainService</summary>
    [grpc::BindServiceMethod(typeof(MainService), "BindService")]
    public abstract partial class MainServiceBase
    {
      /// <summary>
      ///Establish first connection with client
      ///Sync all NetworkBehaviour + SyncVars
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GRPCClient.GRPC_HandshakeGet> GRPC_Handshake(global::GRPCClient.GRPC_HandshakePost request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GRPCClient.GRPC_NHandshakeGet> GRPC_NetcodeHandshake(global::GRPCClient.GRPC_NHandshakePost request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      ///Ping stream checks regularly for client connections 
      ///(Used to disconnect safely from game if no response)
      /// </summary>
      /// <param name="requestStream">Used for reading requests from the client.</param>
      /// <param name="responseStream">Used for sending responses back to the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>A task indicating completion of the handler.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task GRPC_Ping(grpc::IAsyncStreamReader<global::GRPCClient.GRPC_PingPost> requestStream, grpc::IServerStreamWriter<global::GRPCClient.GRPC_PingGet> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetObjUpdate(grpc::IAsyncStreamReader<global::GRPCClient.GRPC_NetObjUpdate> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task GRPC_CliNetObjUpdate(global::GRPCClient.GRPC_EmptyMsg request, grpc::IServerStreamWriter<global::GRPCClient.GRPC_NetObjUpdate> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetVarUpdate(grpc::IAsyncStreamReader<global::GRPCClient.GRPC_NetVarUpdate> requestStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task GRPC_CliNetNetVarUpdate(global::GRPCClient.GRPC_GenericValue request, grpc::IServerStreamWriter<global::GRPCClient.GRPC_NetVarUpdate> responseStream, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for MainService</summary>
    public partial class MainServiceClient : grpc::ClientBase<MainServiceClient>
    {
      /// <summary>Creates a new client for MainService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public MainServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for MainService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public MainServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected MainServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected MainServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      ///Establish first connection with client
      ///Sync all NetworkBehaviour + SyncVars
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GRPCClient.GRPC_HandshakeGet GRPC_Handshake(global::GRPCClient.GRPC_HandshakePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_Handshake(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Establish first connection with client
      ///Sync all NetworkBehaviour + SyncVars
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GRPCClient.GRPC_HandshakeGet GRPC_Handshake(global::GRPCClient.GRPC_HandshakePost request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GRPC_Handshake, null, options, request);
      }
      /// <summary>
      ///Establish first connection with client
      ///Sync all NetworkBehaviour + SyncVars
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GRPCClient.GRPC_HandshakeGet> GRPC_HandshakeAsync(global::GRPCClient.GRPC_HandshakePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_HandshakeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Establish first connection with client
      ///Sync all NetworkBehaviour + SyncVars
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GRPCClient.GRPC_HandshakeGet> GRPC_HandshakeAsync(global::GRPCClient.GRPC_HandshakePost request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GRPC_Handshake, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GRPCClient.GRPC_NHandshakeGet GRPC_NetcodeHandshake(global::GRPCClient.GRPC_NHandshakePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_NetcodeHandshake(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GRPCClient.GRPC_NHandshakeGet GRPC_NetcodeHandshake(global::GRPCClient.GRPC_NHandshakePost request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GRPC_NetcodeHandshake, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GRPCClient.GRPC_NHandshakeGet> GRPC_NetcodeHandshakeAsync(global::GRPCClient.GRPC_NHandshakePost request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_NetcodeHandshakeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GRPCClient.GRPC_NHandshakeGet> GRPC_NetcodeHandshakeAsync(global::GRPCClient.GRPC_NHandshakePost request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GRPC_NetcodeHandshake, null, options, request);
      }
      /// <summary>
      ///Ping stream checks regularly for client connections 
      ///(Used to disconnect safely from game if no response)
      /// </summary>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::GRPCClient.GRPC_PingPost, global::GRPCClient.GRPC_PingGet> GRPC_Ping(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_Ping(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      ///Ping stream checks regularly for client connections 
      ///(Used to disconnect safely from game if no response)
      /// </summary>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncDuplexStreamingCall<global::GRPCClient.GRPC_PingPost, global::GRPCClient.GRPC_PingGet> GRPC_Ping(grpc::CallOptions options)
      {
        return CallInvoker.AsyncDuplexStreamingCall(__Method_GRPC_Ping, null, options);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::GRPCClient.GRPC_NetObjUpdate, global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetObjUpdate(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_SrvNetObjUpdate(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::GRPCClient.GRPC_NetObjUpdate, global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetObjUpdate(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_GRPC_SrvNetObjUpdate, null, options);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::GRPCClient.GRPC_NetObjUpdate> GRPC_CliNetObjUpdate(global::GRPCClient.GRPC_EmptyMsg request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_CliNetObjUpdate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::GRPCClient.GRPC_NetObjUpdate> GRPC_CliNetObjUpdate(global::GRPCClient.GRPC_EmptyMsg request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GRPC_CliNetObjUpdate, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::GRPCClient.GRPC_NetVarUpdate, global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetVarUpdate(grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_SrvNetVarUpdate(new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncClientStreamingCall<global::GRPCClient.GRPC_NetVarUpdate, global::GRPCClient.GRPC_EmptyMsg> GRPC_SrvNetVarUpdate(grpc::CallOptions options)
      {
        return CallInvoker.AsyncClientStreamingCall(__Method_GRPC_SrvNetVarUpdate, null, options);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::GRPCClient.GRPC_NetVarUpdate> GRPC_CliNetNetVarUpdate(global::GRPCClient.GRPC_GenericValue request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GRPC_CliNetNetVarUpdate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncServerStreamingCall<global::GRPCClient.GRPC_NetVarUpdate> GRPC_CliNetNetVarUpdate(global::GRPCClient.GRPC_GenericValue request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncServerStreamingCall(__Method_GRPC_CliNetNetVarUpdate, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override MainServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new MainServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(MainServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GRPC_Handshake, serviceImpl.GRPC_Handshake)
          .AddMethod(__Method_GRPC_NetcodeHandshake, serviceImpl.GRPC_NetcodeHandshake)
          .AddMethod(__Method_GRPC_Ping, serviceImpl.GRPC_Ping)
          .AddMethod(__Method_GRPC_SrvNetObjUpdate, serviceImpl.GRPC_SrvNetObjUpdate)
          .AddMethod(__Method_GRPC_CliNetObjUpdate, serviceImpl.GRPC_CliNetObjUpdate)
          .AddMethod(__Method_GRPC_SrvNetVarUpdate, serviceImpl.GRPC_SrvNetVarUpdate)
          .AddMethod(__Method_GRPC_CliNetNetVarUpdate, serviceImpl.GRPC_CliNetNetVarUpdate).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, MainServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GRPC_Handshake, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCClient.GRPC_HandshakePost, global::GRPCClient.GRPC_HandshakeGet>(serviceImpl.GRPC_Handshake));
      serviceBinder.AddMethod(__Method_GRPC_NetcodeHandshake, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GRPCClient.GRPC_NHandshakePost, global::GRPCClient.GRPC_NHandshakeGet>(serviceImpl.GRPC_NetcodeHandshake));
      serviceBinder.AddMethod(__Method_GRPC_Ping, serviceImpl == null ? null : new grpc::DuplexStreamingServerMethod<global::GRPCClient.GRPC_PingPost, global::GRPCClient.GRPC_PingGet>(serviceImpl.GRPC_Ping));
      serviceBinder.AddMethod(__Method_GRPC_SrvNetObjUpdate, serviceImpl == null ? null : new grpc::ClientStreamingServerMethod<global::GRPCClient.GRPC_NetObjUpdate, global::GRPCClient.GRPC_EmptyMsg>(serviceImpl.GRPC_SrvNetObjUpdate));
      serviceBinder.AddMethod(__Method_GRPC_CliNetObjUpdate, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::GRPCClient.GRPC_EmptyMsg, global::GRPCClient.GRPC_NetObjUpdate>(serviceImpl.GRPC_CliNetObjUpdate));
      serviceBinder.AddMethod(__Method_GRPC_SrvNetVarUpdate, serviceImpl == null ? null : new grpc::ClientStreamingServerMethod<global::GRPCClient.GRPC_NetVarUpdate, global::GRPCClient.GRPC_EmptyMsg>(serviceImpl.GRPC_SrvNetVarUpdate));
      serviceBinder.AddMethod(__Method_GRPC_CliNetNetVarUpdate, serviceImpl == null ? null : new grpc::ServerStreamingServerMethod<global::GRPCClient.GRPC_GenericValue, global::GRPCClient.GRPC_NetVarUpdate>(serviceImpl.GRPC_CliNetNetVarUpdate));
    }

  }
}
#endregion
