using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkObject))]
public class MultiSceneRpcLoader : MultiSceneLoader
{
    public event Action<List<ulong>> OnLoadSuccess;

    private readonly List<ulong> _mIds = new List<ulong>();

    private bool _mAllow;
    private int _mLoadedCount, _mTargetCount;
    private string _mSceneName;

    public override void Init(Action<List<ulong>> onLoadSuccess)
    {
        OnLoadSuccess += onLoadSuccess;
        
        DontDestroyOnLoad(gameObject);
    }

    public override void SetCount(int target)
    {
        _mTargetCount = target;
    }

    public override void Request(string sceneName)
    {
        Request_Rpc(new FixedString128Bytes(sceneName));
    }

    private async void Load()
    {
        try
        {
            _mAllow = false;
            _mIds.Clear();
            _mLoadedCount = 0;
        
            AsyncOperation op = SceneManager.LoadSceneAsync(_mSceneName, LoadSceneMode.Additive);
        
            if (op == null)
            {
                return;
            }
        
            op.allowSceneActivation = false;

            while (op.progress < 0.9f)
            {
                await Task.Delay(100);
            }
            
            ulong id = NetworkManager.Singleton.LocalClientId;
            
            Load_End_Rpc(id);

            while (!_mAllow)
            {
                await Task.Delay(100);
            }
            
            op.allowSceneActivation = true;
        }
        catch (Exception e)
        {
            // ignore
        }
    }


    [Rpc(SendTo.Everyone)]
    private void Request_Rpc(FixedString128Bytes sceneName)
    {
        _mSceneName = sceneName.Value;

        Load();
    }

    [Rpc(SendTo.Server)]
    private void Load_End_Rpc(ulong id)
    {
        _mIds.Add(id);
        
        ++_mLoadedCount;

        if (_mLoadedCount < _mTargetCount)
        {
            return;
        }

        Load_All_End_Rpc();
    }
    
    [Rpc(SendTo.Everyone)]
    private void Load_All_End_Rpc()
    {
        _mAllow = true;

        if (!IsServer)
        {
            return;
        }
        
        OnLoadSuccess?.Invoke(_mIds);
    }
}
