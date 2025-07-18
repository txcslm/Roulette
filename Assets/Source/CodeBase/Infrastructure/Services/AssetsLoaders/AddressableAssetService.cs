using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Source.CodeBase.Infrastructure.Services
{
  public class AddressableAssetService : IAssetService
  {
    private readonly Dictionary<string, AsyncOperationHandle> _handles = new Dictionary<string, AsyncOperationHandle>();
    private readonly Dictionary<GameObject, AsyncOperationHandle<GameObject>> _instances = new Dictionary<GameObject, AsyncOperationHandle<GameObject>>();

    public async UniTask<T> LoadAssetAsync<T>(string key) where T : Object
    {
      if (_handles.TryGetValue(key, out var existingHandle))
      {
        await existingHandle.ToUniTask();
        return existingHandle.Result as T;
      }

      var handle = Addressables.LoadAssetAsync<T>(key);
      _handles[key] = handle;

      return await handle.ToUniTask();
    }

    public async UniTask<IList<T>> LoadAssetsAsync<T>(string key) where T : Object
    {
      var handle = Addressables.LoadAssetsAsync<T>(key, null);
      return await handle.ToUniTask();
    }

    public async UniTask<GameObject> InstantiateAsync(string key, Transform parent = null)
    {
      var handle = Addressables.InstantiateAsync(key, parent);
      var instance = await handle.ToUniTask();

      _instances[instance] = handle;
      return instance;
    }

    public void ReleaseAsset(string key)
    {
      if (!_handles.TryGetValue(key, out var handle))
        return;

      Addressables.Release(handle);
      _handles.Remove(key);
    }

    public void ReleaseInstance(GameObject instance)
    {
      if (!_instances.TryGetValue(instance, out var handle))
        return;

      Addressables.ReleaseInstance(handle);
      _instances.Remove(instance);
    }
  }
}