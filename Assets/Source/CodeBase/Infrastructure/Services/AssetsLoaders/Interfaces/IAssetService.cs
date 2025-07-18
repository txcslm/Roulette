using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IAssetService
  {
    UniTask<T> LoadAssetAsync<T>(string key) where T : Object;

    UniTask<IList<T>> LoadAssetsAsync<T>(string key) where T : Object;

    UniTask<GameObject> InstantiateAsync(string key, Transform parent = null);

    void ReleaseAsset(string key);

    void ReleaseInstance(GameObject instance);
  }
}