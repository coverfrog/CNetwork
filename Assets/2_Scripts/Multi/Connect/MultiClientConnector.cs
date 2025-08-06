using System;
using System.Linq;
using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;

public class MultiClientConnector : MultiConnector
{
    private event Action<Lobby> OnConnectSuccess;
    private event Action<string> OnConnectFail;

    private readonly FacepunchTransport _mTransport;
    
    private ulong _mSteamId;
    
    public MultiClientConnector(FacepunchTransport transport, Action<Lobby> onConnectSuccess, Action<string> onConnectFail)
    {
        _mTransport = transport;
        
        OnConnectSuccess += onConnectSuccess;
        OnConnectFail += onConnectFail;
        
        SteamFriends.OnGameLobbyJoinRequested += OnJoinRequested;
        SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;
    }

    public override void Connect()
    {
        _ = SteamMatchmaking.JoinLobbyAsync(_mSteamId);
    }

    public override async void Connect(ulong id)
    {
        try
        {
            Lobby[] lobbies = await SteamMatchmaking.LobbyList
                .WithSlotsAvailable(1)
                .RequestAsync();

            foreach (Lobby lobby in lobbies)
            {
                Debug.Log(lobby.Owner.Id);
                
                if (lobby.Owner.Id != _mSteamId) continue;
                
                _ = await lobby.Join();
            }
        }
        catch (Exception)
        {
            // ignore
        }
    }

    private void OnJoinRequested(Lobby lobby, SteamId steamId)
    {
        _ = lobby.Join();
    }
    
    private void OnLobbyEntered(Lobby lobby)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            return;
        }
        
        _mTransport.targetSteamId = lobby.Owner.Id;

        if (!NetworkManager.Singleton.StartClient())
        {
            OnConnectFail?.Invoke("Connect Lobby Failed");
            return;
        }

        OnConnectSuccess?.Invoke(lobby);
    }
}
