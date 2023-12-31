using Grpc.Core;
using Networking;

namespace GRPCServer.Services
{
    public class MainServiceImpl : MainService.MainServiceBase
    {
        #region Init

        private readonly ILogger<MainServiceImpl> _logger;
        public MainServiceImpl(ILogger<MainServiceImpl> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Refs

        public static readonly Dictionary<string, GRPCClient> clients = new();
        public static readonly Dictionary<string, UnrealClient> unrealClients = new Dictionary<string, UnrealClient>();
        public static NetcodeServer? netcodeServer = null;

        public static event Action<UnrealClient>? OnUnrealClientConnected;
        public static event Action<UnrealClient>? OnUnrealClientDisconnected;
        
        private void AddNetcodeServer(string ip)
        {
            if (netcodeServer != null)
            {
                Debug.LogWarning("AddNetcodeServer > Netcode server already connected. IP: " + ip);
                return;
            }
            if (clients.ContainsKey(ip))
            {
                Debug.LogWarning("AddNetcodeServer > Trying to connect a client that is already connected but not as Netcode server. IP: " + ip);
                return;
            }

            netcodeServer = new NetcodeServer(ip);
            clients.Add(ip, netcodeServer);
        }

        private void AddUnrealClient(string ip, string name)
        {
            if (unrealClients.ContainsKey(ip))
            {
                Debug.LogWarning("AddUnrealClient > Unreal client already connected. IP: " + ip);
                return;
            }
            if (clients.ContainsKey(ip))
            {
                Debug.LogWarning("AddUnrealClient > Trying to connect a client that is already connected but not as unreal client. IP: " + ip);
                return;
            }

            UnrealClient unrealClient = new UnrealClient(ip, name);
            unrealClient.id = -(clients.Keys.Count + 1);
            unrealClients.Add(ip, unrealClient);
            clients.Add(ip, unrealClient);

            OnUnrealClientConnected?.Invoke(unrealClient);
        }

        private void DisplayClients()
        {
            Debug.Log("-----------------------------------", Debug.separatorColor);
            Console.WriteLine();

            Debug.Log($"Connected clients : {clients.Keys.Count}");
            Console.WriteLine();

            // Display Unreal clients
            Console.WriteLine($"Unreal clients : {unrealClients.Keys.Count}");
            foreach (KeyValuePair<string, UnrealClient> unrealClient in unrealClients)
            {
                Debug.Log(unrealClient.Value.Adress.ToString());
            }

            Console.WriteLine();
            Console.WriteLine();

            // Display NetcodeServer
            Debug.Log($"NetcodeServer :");
            if (netcodeServer != null)
            {
                Debug.Log(netcodeServer.Adress);
            }
            else
            {
                Debug.Log("Not connected");
            }
            Console.WriteLine();

            Debug.Log("-----------------------------------", Debug.separatorColor);
            Console.WriteLine();
        }

        public void DisconnectClient(string clientAdress)
        {
            if(!clients.TryGetValue(clientAdress, out var client)) 
            {
                //Can't use logger since this is static
                Debug.LogWarning("DisconnectClient > Trying to disconnect a client that is not connected. IP: " + clientAdress);
                return; 
            }
            
            client.Disconnect();
            
            Debug.Log($"DisconnectClient > Disconnect {clientAdress}\n");

            //Strange but cool way to cast then null check
            if(client as UnrealClient is { } unrealClient)
                OnUnrealClientDisconnected?.Invoke(unrealClient);
            
            DisplayClients();
        }

        #endregion

        #region Handshake

        public override Task<GRPC_HandshakeGet> GRPC_Handshake(GRPC_HandshakePost request, ServerCallContext context)
        {          
            //Debug
            Debug.Log("Handshake");
            Debug.Log(context.Host);
            Debug.Log(context.Peer + "\n");

            if (clients.Count > 0)
            {
                // Catch if client is already added to the clients
                string clientAdress = context.Peer;
                AddUnrealClient(clientAdress, request.Name);
                

                DisplayClients();

                if (netcodeServer == null)
                {
                    Debug.LogError("GRPC_Handshake > NetcodeServer is null");
                    return Task.FromResult(new GRPC_HandshakeGet { Result = 1, ClientId = -1 });
                }

                return Task.FromResult(new GRPC_HandshakeGet
                {
                    Result = 0, //0 = good!
                    ClientId = unrealClients[clientAdress].id,
                    
                    NetObjects = { netcodeServer.GetNetworkObjectsAsUpdates() },
                    NetVars = { netcodeServer.GetNetworkVariablesAsUpdates() }
                });
            }
            else
            {
                Debug.LogError("GRPC_Handshake > Getting Handshake, but there is no NetcodeServer. " +
                    "Connect UnrealClients after NetcodeServer!");

                //Result != 0 => error
                return Task.FromResult(new GRPC_HandshakeGet { Result = 1, ClientId = -1 });
            }
        }

        public override Task<GRPC_NHandshakeGet> GRPC_NetcodeHandshake(GRPC_NHandshakePost request, ServerCallContext context)
        {
            //Debug
            Debug.Log("NetcodeHandshake");
            Debug.Log(context.Host);
            Debug.Log(context.Peer + "\n");

            if (netcodeServer == null)
            {
                string adress = context.Peer;
                AddNetcodeServer(adress);

                DisplayClients();

                return Task.FromResult(new GRPC_NHandshakeGet { Result = 0 });
            }
            
            Debug.LogError("GRPC_NetcodeHandshake > Getting NetcodeHandshake, but there is already an active NetcodeServer!");

            return Task.FromResult(new GRPC_NHandshakeGet { Result = 1 });
        }

        #endregion

        #region Ping

        public override async Task GRPC_Ping(IAsyncStreamReader<GRPC_PingPost> requestStream, 
            IServerStreamWriter<GRPC_PingGet> responseStream, ServerCallContext context)
        {
            GRPC_PingGet empty = new();

            try
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(empty);
                }
            }
            catch (IOException)
            {
                Debug.Log("GRPC_Ping > Connection lost with client.");
                DisconnectClient(context.Peer);
            }
        }

        #endregion

        #region Clients Connection

        public override async Task GRPC_SrvClientUpdate(GRPC_EmptyMsg request,
            IServerStreamWriter<GRPC_ClientUpdate> responseStream, ServerCallContext context)
        {
            if (netcodeServer == null)
            {
                Debug.LogError(
                    $"GRPC_SrvClientUpdate > Presumed NetcodeServer {context.Peer} is trying to get ClientUpdate stream but NetcodeServer is not registered.");
                return;
            }
            if (netcodeServer.Adress != context.Peer)
            {
                Debug.LogError(
                    $"GRPC_SrvClientUpdate > Client {context.Peer} is trying to get ClientUpdate stream but is not NetcodeServer {netcodeServer.Adress}.");
                return;
            }

            OnUnrealClientConnected += SendClientConnectedUpdate;
            OnUnrealClientDisconnected += SendClientDisconnectedUpdate;

            netcodeServer.ClientUpdateStream = responseStream;
            
            try
            {
                await Task.Delay(-1, context.CancellationToken);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("GRPC_SrvClientUpdate > Connection lost with NetcodeServer.");
                UnsubscribeClientUpdateEvent();
                DisconnectClient(context.Peer);
            }
        }

        private async void SendClientConnectedUpdate(UnrealClient cli)
        {
            //This should never happen
            if (netcodeServer == null)
            {
                Debug.LogError("SendClientConnectedUpdate > Trying to send client connected update without NetcodeServer connected.");
                UnsubscribeClientUpdateEvent();
                return;
            }
                        
            try
            {
                await netcodeServer.ClientUpdateStream.WriteAsync(ToClientUpdate(cli, GRPC_ClientUpdateType.Connect));
            }
            catch (IOException)
            {
                Debug.Log("SendClientConnectedUpdate > SendClientConnectedUpdate > Connection lost with NetcodeServer.");
                UnsubscribeClientUpdateEvent();
                DisconnectClient(netcodeServer.Adress);
            }
        }
        
        private async void SendClientDisconnectedUpdate(UnrealClient cli)
        {
            //This should never happen
            if (netcodeServer == null)
            {
                Debug.LogError("SendClientDisconnectedUpdate > Trying to send client disconnected update without NetcodeServer connected.");
                UnsubscribeClientUpdateEvent();
                return;
            }
            
            try
            {
                await netcodeServer.ClientUpdateStream.WriteAsync(ToClientUpdate(cli, GRPC_ClientUpdateType.Disconnect));
            }
            catch (IOException)
            {
                Debug.Log("SendClientDisconnectedUpdate > Connection lost with NetcodeServer.");
                UnsubscribeClientUpdateEvent();
                DisconnectClient(netcodeServer.Adress);
            }
        }

        private GRPC_ClientUpdate ToClientUpdate(UnrealClient cli, GRPC_ClientUpdateType type) =>
            new() { ClientIP = cli.Adress, Type = type, ClientId = cli.id, Name = cli.name};

        private void UnsubscribeClientUpdateEvent()
        {
            OnUnrealClientConnected -= SendClientConnectedUpdate;
            OnUnrealClientDisconnected -= SendClientDisconnectedUpdate;
        }
        
        #endregion
        
        #region NetObjects / NetVars Update

        public override async Task<GRPC_EmptyMsg> GRPC_SrvNetObjUpdate(IAsyncStreamReader<GRPC_NetObjUpdate> requestStream, ServerCallContext context)
        {
            if (netcodeServer == null)
            {
                Debug.LogError("GRPC_SrvNetObjUpdate > Trying to open NetObjUpdate stream without " +
                                    "NetcodeServer connected. Client IP: " + context.Peer);
                return new GRPC_EmptyMsg();
            }
            
            Debug.Log("GRPC_SrvNetObjUpdate > NetcodeServer Network Objects update stream opened.");
            
            try
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    Debug.Log("GRPC_SrvNetObjUpdate > Got new NetworkObject update: type " + requestStream.Current.Type + ", netId " +
                                      requestStream.Current.NetId + ", prefabId " + requestStream.Current.PrefabId +
                                      "\n");
                    
                    netcodeServer.HandleNetObjUpdate(requestStream.Current);
                    
                    foreach (var client in unrealClients)
                    {
                        //If client has just connected and doesn't have a stream yet,
                        //queue the update for when it will have a stream
                        var stream = client.Value.NetObjectsStream;
                        
                        if (stream != null!)
                        {
                            await stream.WriteAsync(requestStream.Current);
                        }
                        else
                        {
                            client.Value.QueueNetObjUpdate(requestStream.Current);
                        }
                    }

                    Debug.Log("GRPC_SrvNetObjUpdate > Update sent to all unreal clients.\n");
                }
            }
            catch (IOException)
            {
                Debug.Log("GRPC_SrvNetObjUpdate > Connection lost with client - NetObj Stream closed");
                //DisconnectClient(context.Peer);
            }

            return new GRPC_EmptyMsg();
        }

        public override async Task GRPC_CliNetObjUpdate(GRPC_EmptyMsg request, IServerStreamWriter<GRPC_NetObjUpdate> responseStream, ServerCallContext context)
        {
            if (!unrealClients.ContainsKey(context.Peer))
            {
                Debug.LogError(
                    $"GRPC_CliNetObjUpdate > Client {context.Peer} is trying to get NetworkObjects update stream without being registered.");
                return;
            }

            Debug.Log($"GRPC_CliNetObjUpdate > Client NetObject stream opened");
            unrealClients[context.Peer].NetObjectsStream = responseStream;

            try
            {
                await Task.Delay(-1, context.CancellationToken);
            }
            catch (TaskCanceledException)
            {
                Debug.Log("GRPC_CliNetObjUpdate > Connection lost with client - NetObj Client NetObject stream closed");
                //DisconnectClient(context.Peer);
            }

            Debug.Log($"GRPC_CliNetObjUpdate > NetObj Client NetObject stream closed");
        }
        
        public override async Task<GRPC_EmptyMsg> GRPC_SrvNetVarUpdate(IAsyncStreamReader<GRPC_NetVarUpdate> requestStream, ServerCallContext context)
        {
            Debug.Log($"GRPC_SrvNetVarUpdate > NetVar writing stream opened");
            
            try
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    Debug.Log($"GRPC_SrvNetVarUpdate > NetVar received for HashName : {requestStream.Current.HashName} / Type {requestStream.Current.NewValue.Type} / New Value : {requestStream.Current.NewValue.Value}");

                    if (netcodeServer.NetObjs[requestStream.Current.NetId].NetVars.ContainsKey(requestStream.Current.HashName))
                    {
                        netcodeServer.NetObjs[requestStream.Current.NetId].NetVars[requestStream.Current.HashName] = requestStream.Current.NewValue;
                    }
                    else
                    {
                        netcodeServer.NetObjs[requestStream.Current.NetId].NetVars.Add(requestStream.Current.HashName, requestStream.Current.NewValue);
                    }

                    foreach (KeyValuePair<string, UnrealClient> unrealClient in unrealClients)
                    {
                        //There could be a problem if a client does GRPC_CliNetNetVarUpdate
                        //and at the same time netcode server send a net var update
                        if (unrealClient.Value.netVarStream.ContainsKey(requestStream.Current.NewValue.Type) == false) continue;

                        Debug.Log($"GRPC_SrvNetVarUpdate > Unreal client receiving NetVar : HashName : {requestStream.Current.HashName} / New Value : {requestStream.Current.NewValue.Value}");

                        await unrealClient.Value.netVarStream[requestStream.Current.NewValue.Type].WriteAsync(requestStream.Current);
                    }

                    //Console.WriteLine($"GRPC_SrvNetVarUpdate > VRAIMENT TU AS RECU :  NetVar received for HashName : {requestStream.Current.HashName} / New Value : {requestStream.Current.NewValue.Value}");
                }
            }
            catch (IOException)
            {
                //Debug.Log("GRPC_SrvNetVarUpdate > Connection lost with client.");
                Debug.Log($"GRPC_SrvNetVarUpdate > Connection lost with client - NetVar Writing stream closed");
                if (netcodeServer != null)
                {
                    foreach (var netObjs in netcodeServer.NetObjs.Values)
                    {
                        netObjs.NetVars.Clear();
                    }

                    netcodeServer.NetObjs.Clear();
                }
                
                return new GRPC_EmptyMsg();
            }


            if (netcodeServer != null)
            {
                foreach (var netObjs in netcodeServer.NetObjs.Values)
                {
                    netObjs.NetVars.Clear();
                }

                netcodeServer.NetObjs.Clear();
            }

            Debug.Log($"GRPC_SrvNetVarUpdate > Witring stream closed manually");
            return new GRPC_EmptyMsg();
        }

        public override async Task GRPC_CliNetNetVarUpdate(GRPC_GenericValue request, IServerStreamWriter<GRPC_NetVarUpdate> responseStream, ServerCallContext context)
        {
            Debug.Log($"Response stream opened : {context.Peer} / {request.Type}");

            lock (unrealClients[context.Peer].netVarStream)
            {
                if (unrealClients[context.Peer].netVarStream.TryAdd(request.Type, responseStream) == false)
                {
                    Debug.Log($"GRPC_CliNetNetVarUpdate > Unreal client {context.Peer} already open listening stream for {request.Type}");
                }

                Debug.Log($"GRPC_CliNetNetVarUpdate > Check : {unrealClients[context.Peer].netVarStream[request.Type]}");
            }

            try
            {
                await Task.Delay(-1, context.CancellationToken);
            }
            catch (TaskCanceledException)
            {
                if(unrealClients.TryGetValue(context.Peer, out var client))
                    client.netVarStream.Remove(request.Type);
            }
            Debug.Log($"GRPC_CliNetNetVarUpdate > Response stream closed : {context.Peer} / {request.Type}");
        }

        public override async Task<GRPC_EmptyMsg> GRPC_RequestNetVarUpdateUnrealToGrpc(GRPC_NetVarUpdate request, ServerCallContext context)
        {
            if (netcodeServer == null)
            {
                Debug.LogError("GRPC_RequestNetVarUpdateUnrealToGrpc > NetcodeServer is null");
                return new GRPC_EmptyMsg();
            }

            bool found = false;
            foreach(var requestNetVarUpdate in netcodeServer.requestNetvarUpdateStream)
            {
                if (request.HashName == requestNetVarUpdate.Value.HashName 
                    && request.NetId == requestNetVarUpdate.Value.NetId)
                {
                    await requestNetVarUpdate.Key.WriteAsync(request);
                    found = true;
                }
            }

            if (found == false)
            {
                Debug.Log("GRPC_RequestNetVarUpdateUnrealToGrpc > Unreal requested a sync for the NetVar but Netcode was not waiting for a request");
            }
            return new GRPC_EmptyMsg();
        }

        public override async Task GRPC_RequestNetVarUpdateGrpcToNetcode(GRPC_NetVarUpdate request, IServerStreamWriter<GRPC_NetVarUpdate> responseStream, ServerCallContext context)
        {
            Debug.Log("GRPC_RequestNetVarUpdateGrpcToNetcode > Opening new NetVar sync request for : ");

            if (netcodeServer == null)
            {
                Debug.LogError("GRPC_RequestNetVarUpdateGrpcToNetcode > Netcde server is null");
                return;
            }

            if (netcodeServer.requestNetvarUpdateStream.ContainsKey(responseStream))
            {
                netcodeServer.requestNetvarUpdateStream.Remove(responseStream);

                Debug.Log("GRPC_RequestNetVarUpdateGrpcToNetcode > A request netVar sync for this is already opened");
            }

            netcodeServer.requestNetvarUpdateStream.Add(responseStream, request);

            try
            {
                await Task.Delay(-1, context.CancellationToken);
            }
            catch (TaskCanceledException)
            {
                if (netcodeServer != null && netcodeServer.requestNetvarUpdateStream.ContainsKey(responseStream))
                {
                    netcodeServer?.requestNetvarUpdateStream.Remove(responseStream);
                }
            }
        }
        #endregion

        #region Lobby
        public override async Task GRPC_TeamSelectionUnrealToGrpc(IAsyncStreamReader<GRPC_Team> requestStream, IServerStreamWriter<GRPC_TeamResponse> responseStream, ServerCallContext context)
        {
            if (netcodeServer == null)
            {
                Debug.LogError("GRPC_TeamSelectionUnrealToGrpc > NetcodeServer is null");
                return;
            }

            netcodeServer.teamSelectionResponseStream.Add(responseStream);

            Debug.Log("GRPC_TeamSelectionUnrealToGrpc > Opened stream");
            try
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    GRPC_Team messageReceived = requestStream.Current;
                    await UnrealClient.teamSelectionResponseStream.WriteAsync(messageReceived);

                }
            }
            catch (IOException)
            {
                Debug.Log("GRPC_TeamSelectionUnrealToGrpc > Connection lost with client - TeamSelection stream closed");
                //DisconnectClient(context.Peer);
            }
        }

        public override async Task GRPC_TeamSelectionGrpcToNetcode(IAsyncStreamReader<GRPC_TeamResponse> requestStream, IServerStreamWriter<GRPC_Team> responseStream, ServerCallContext context)
        {
            Debug.Log("GRPC_TeamSelectionGrpcToNetcode > Opened stream");
            UnrealClient.teamSelectionResponseStream = responseStream;

            try
            {
                while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
                {
                    // Callback the response to all the Unreal clients
                    foreach (IServerStreamWriter<GRPC_TeamResponse> item in netcodeServer.teamSelectionResponseStream)
                    {
                        await item.WriteAsync(requestStream.Current);
                    }
                    
                }
            }
            catch (IOException)
            {
                Debug.Log("GRPC_TeamSelectionGrpcToNetcode > Connection lost with client - TeamSelection stream closed");
                //DisconnectClient(context.Peer);
            }
        }
        #endregion
    }
}