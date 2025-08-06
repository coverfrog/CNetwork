using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneRpcLoader : MultiSceneLoader
{
    public delegate void OnLoadSuccessDelegate(ulong id, int count);
    
    public event OnLoadSuccessDelegate OnLoadSuccess;

    private int _mLoadedCount = 0;
    private string _mSceneName;

    public MultiSceneRpcLoader(OnLoadSuccessDelegate onLoadSuccess)
    {
        OnLoadSuccess += onLoadSuccess;
    }
    
    public override void Request(string sceneName)
    {
        Load_Send_Rpc(new FixedString128Bytes(sceneName));
    }

    private void Load()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(_mSceneName);

        if (op == null)
        {
            return;
        }
        
        op.completed += a =>
        {
            ulong id = NetworkManager.Singleton.LocalClientId;
            
            Load_Complete_Rpc(id);
        };
    }

    [Rpc(SendTo.Everyone)]
    private void Load_Send_Rpc(FixedString128Bytes sceneName)
    {
        _mSceneName = sceneName.Value;

        Load();
    }

    [Rpc(SendTo.Server)]
    private void Load_Complete_Rpc(ulong id)
    {
        ++_mLoadedCount;
        
        OnLoadSuccess?.Invoke(id, _mLoadedCount);
    }
}
