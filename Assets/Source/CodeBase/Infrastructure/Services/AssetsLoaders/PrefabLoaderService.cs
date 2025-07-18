using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services
{
  public class PrefabLoaderService : IPrefabLoaderService
  {
    private readonly IAssetService _assetService;
    private readonly Dictionary<string, GameObject> _loadedPrefabs = new Dictionary<string, GameObject>();

    public PrefabLoaderService(IAssetService assetService) =>
      _assetService = assetService;

    public async UniTask<T> LoadPrefabAsync<T>(string key) where T : MonoBehaviour
    {
      if (!_loadedPrefabs.ContainsKey(key))
      {
        var prefab = await _assetService.LoadAssetAsync<GameObject>(key);
        _loadedPrefabs[key] = prefab;
      }

      var loadedPrefab = _loadedPrefabs[key];
      return loadedPrefab.GetComponent<T>();
    }

    public void ReleasePrefab(string key)
    {
      if (_loadedPrefabs.ContainsKey(key))
      {
        _loadedPrefabs.Remove(key);
      }
    }
  }
}