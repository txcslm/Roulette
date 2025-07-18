using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IPrefabLoaderService
  {
    UniTask<T> LoadPrefabAsync<T>(string key) where T : MonoBehaviour;

    void ReleasePrefab(string key);
  }
}