using System;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class MultiServerConnector : MultiConnector
{
    private event Action<Lobby> OnConnectSuccess;
    private event Action<string> OnConnectFail;
    
    private readonly int _mMaxPlayerCount;
    
    
    public MultiServerConnector(int maxPlayerCount, Action<Lobby> onConnectSuccess, Action<string> onConnectFail)
    {
        _mMaxPlayerCount = maxPlayerCount;
        
        OnConnectSuccess += onConnectSuccess;
        OnConnectFail += onConnectFail;
        
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
    }

    public override void Connect()
    {
        _ = SteamMatchmaking.CreateLobbyAsync(_mMaxPlayerCount);
    }

    public override void Connect(ulong id)
    {
        
    }

    private void OnLobbyCreated(Result result, Lobby lobby)
    {
        if (result != Result.OK)
        {
            OnConnectFail?.Invoke("Create Lobby Fail");
            return;
        }

        lobby.SetJoinable(true);
        lobby.SetPublic();

        if (!NetworkManager.Singleton.StartServer())
        {
            OnConnectFail?.Invoke("Start Server failed");
            return;
        }
        
        OnConnectSuccess?.Invoke(lobby);
    }
}
