using System;
using System.Collections.Generic;
using Netcode.Transports.Facepunch;
using Steamworks.Data;
using Unity.Netcode;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(NetworkManager))]
[RequireComponent(typeof(FacepunchTransport))]
public class MultiManager : Singleton<MultiManager>
{
    #region > Event

    public event Action<Lobby> OnConnectSuccess;
    
    public event Action<string> OnConnectFail;

    #endregion

    [SerializeField] private MultiSceneLoader mSceneLoader;
    
    private FacepunchTransport _mTransport;
    
    private IMultiConnector _mClientConnector, _mServerConnector;
    
    public Lobby? Current { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        _mTransport = GetComponent<FacepunchTransport>();
    }

    private void Start()
    {
        _mClientConnector = new MultiClientConnector(_mTransport,
            lobby =>
            {
                Current = lobby;
                OnConnectSuccess?.Invoke(lobby);
            }, msg =>
            {
                OnConnectFail?.Invoke(msg);
            });
        
        _mServerConnector = new MultiServerConnector(4,
            lobby =>
            {
                Current = lobby;
                OnConnectSuccess?.Invoke(lobby);
            }, msg =>
            {
                OnConnectFail?.Invoke(msg);
            });
    }

    public void ConnectServer() => _mServerConnector?.Connect();

    public void ConnectClient(ulong lobbyId) =>  _mClientConnector?.Connect(lobbyId);
    
    public void LoadSceneGame() => mSceneLoader?.Request("3_Game");
}
