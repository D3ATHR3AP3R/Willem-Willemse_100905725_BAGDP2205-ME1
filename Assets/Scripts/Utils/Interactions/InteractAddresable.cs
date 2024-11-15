using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InteractAddresable : InteractionBase
{
    private AsyncOperationHandle _ResourceLoad;

    [SerializeField] private Transform _ResourceSpawnPoint;

    [SerializeField] private AssetReferenceGameObject _ResourceToLoad;

    public override void Interact()
    {
        _ResourceLoad = Addressables.InstantiateAsync(_ResourceToLoad);
        _ResourceLoad.Completed += SpawnResource;
    }

    private void OnDisable()
    {
        _ResourceLoad.Completed -= SpawnResource;
    }

    private void SpawnResource(AsyncOperationHandle handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Successfully Loaded the resources");
        }
    }
}
