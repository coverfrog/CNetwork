using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiSceneRpcLoader : MultiSceneLoader
{
    public event Action<int> OnLoadSuccess;

    private int _mLoadedCount = 0;
    private string _mSceneName;

    public MultiSceneRpcLoader(Action<int> onLoadSuccess)
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
            Load_Complete_Rpc();
        };
    }

    [Rpc(SendTo.Everyone)]
    private void Load_Send_Rpc(FixedString128Bytes sceneName)
    {
        _mSceneName = sceneName.Value;

        Load();
    }

    [Rpc(SendTo.Server)]
    private void Load_Complete_Rpc()
    {
        ++_mLoadedCount;
        
        OnLoadSuccess?.Invoke(_mLoadedCount);
    }
}
