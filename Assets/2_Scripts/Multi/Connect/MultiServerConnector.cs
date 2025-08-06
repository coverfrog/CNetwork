using System;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class MultiServerConnector : MultiConnector
{
    private readonly int _mMaxPlayerCount;
    
    private event Action<Lobby> OnConnectSuccess;
    private event Action<string> OnConnectFail;
    
    public MultiServerConnector(int maxPlayerCount, Action<Lobby> onConnectSuccess, Action<string> onConnectFail)
    {
        _mMaxPlayerCount = maxPlayerCount;
        
        OnConnectSuccess += onConnectSuccess;
        OnConnectFail += onConnectFail;
        
        SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
    }

    public override void Set(ulong id)
    {
        
    }

    public override void Connect()
    {
        _ = SteamMatchmaking.CreateLobbyAsync(_mMaxPlayerCount);
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
