using System;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIMultiConnect : MonoBehaviour
{
    [SerializeField] private Button mServerButton;
    [SerializeField] private Button mClientButton;

    private void Start()
    {
        mServerButton.interactable = true;
        mServerButton.onClick.AddListener(ConnectServer);
  
        mClientButton.interactable = true;
        mClientButton.onClick.AddListener(() =>
        {
            MultiManager.Instance.ConnectClient(76561199223957438);
        });
        
        MultiManager.Instance.OnConnectSuccess += OnConnectSuccess;
        MultiManager.Instance.OnConnectFail += OnConnectFail;
    }

    private void ConnectServer()
    {
        mServerButton.interactable = false;
        
        MultiManager.Instance.ConnectServer();
    }
    
    private void OnConnectSuccess(Lobby lobby)
    {
        
    }
    
    private void OnConnectFail(string msg)
    {
        mServerButton.interactable = true;
    }
}
