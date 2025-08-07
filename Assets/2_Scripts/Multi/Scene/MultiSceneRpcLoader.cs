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
    private List<ulong> _mIdList;

    private bool _mAllow, _mCompleted;
    private int _mLoadedCount, _mTargetCount;
    private string _mSceneName;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }

    public override void Request(string sceneName)
    {
        _mAllow = false;
        _mCompleted = false;
        
        if (!IsServer)
        {
            return;
        }
        
        _mIdList = new List<ulong>();
        _mLoadedCount = 0;
        
        if (MultiManager.Instance.Current != null)
        {
            _mTargetCount = MultiManager.Instance.Current.Value.MemberCount;
        }
        
        Request_Rpc(new FixedString128Bytes(sceneName));
    }

    private async void Load()
    {
        try
        {
            Scene prevScene = SceneManager.GetActiveScene();
            
            AsyncOperation op = SceneManager.LoadSceneAsync(_mSceneName, LoadSceneMode.Additive);
        
            if (op == null)
            {
                return;
            }
        
            op.allowSceneActivation = false;
            op.completed += a =>
            {
                _mCompleted = true;
            };
            
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

            while (!_mCompleted)
            {
                await Task.Delay(100);
            }
            
            await SceneManager.UnloadSceneAsync(prevScene);
            
            FindAnyObjectByType<SceneHandler>()?.OnSceneLoaded(IsServer, _mIdList);
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
        _mIdList.Add(id);
        
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
    }
}
