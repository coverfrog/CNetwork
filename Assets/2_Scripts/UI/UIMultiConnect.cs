using System;
using Steamworks.Data;
using UnityEngine;
using UnityEngine.UI;

public class UIMultiConnect : MonoBehaviour
{
    [SerializeField] private Button mServerButton;

    private void Start()
    {
        mServerButton.interactable = true;
        mServerButton.onClick.AddListener(ConnectServer);
  
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
        MultiManager.Instance.LoadScene("3_Game");
    }
    
    private void OnConnectFail(string msg)
    {
        mServerButton.interactable = true;
    }
}
