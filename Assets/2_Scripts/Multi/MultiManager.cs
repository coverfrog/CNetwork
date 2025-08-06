using System;
using Netcode.Transports.Facepunch;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
[RequireComponent(typeof(FacepunchTransport))]
public class MultiManager : Singleton<MultiManager>
{
    public event Action<Lobby> OnConnectSuccess;
    public event Action<string> OnConnectFail;
    public event Action<int> OnLoadSuccess;

    private NetworkManager _mNetworkManager;
    private FacepunchTransport _mTransport;
    
    private IMultiConnector _mClientConnector, _mServerConnector;
    private IMultiSceneLoader _mSceneLoader;

    protected override void Awake()
    {
        base.Awake();
        
        _mNetworkManager = GetComponent<NetworkManager>();
        _mTransport = GetComponent<FacepunchTransport>();
    }

    private void Start()
    {
        _mClientConnector = new MultiClientConnector(_mTransport,
            lobby =>
            {
                OnConnectSuccess?.Invoke(lobby);
            }, msg =>
            {
                OnConnectFail?.Invoke(msg);
            });
        
        _mServerConnector = new MultiServerConnector(4,
            lobby =>
            {
                OnConnectSuccess?.Invoke(lobby);
            }, msg =>
            {
                OnConnectFail?.Invoke(msg);
            });

        _mSceneLoader = new MultiSceneRpcLoader((id, count) =>
        {
            OnLoadSuccess?.Invoke(count);
            
            Debug.Log($"id : {id} , count : {count}");
        });
    }

    public void ConnectServer() => _mServerConnector?.Connect();
    
    public void ConnectClient() => _mClientConnector?.Connect();
    
    [ContextMenu("> Context : Load Scene Game")]
    public void LoadSceneGame() => _mSceneLoader?.Request("3_Game");
}
