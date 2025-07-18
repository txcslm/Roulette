using Reflex.Core;
using Source.CodeBase.Infrastructure.Services;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Installers
{
  public class ProjectInstaller : MonoBehaviour, IInstaller
  {
    [SerializeField] private string _slotPrefabKey = "Prefabs/SlotPrefab.prefab";
    [SerializeField] private string _rewardObjectPrefabKey = "Prefabs/RewardObjectPrefab.prefab";
    [SerializeField] private string _rouletteViewPrefabKey = "Prefabs/RouletteView.prefab";
    [SerializeField] private string _loadingWindowPrefabKey = "Prefabs/LoadingWindow.prefab";

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
      containerBuilder.AddSingleton<IAssetService>(_ => new AddressableAssetService());

      containerBuilder.AddSingleton<IPrefabLoaderService>(container => new PrefabLoaderService(container.Resolve<IAssetService>()));

      containerBuilder.AddSingleton<ISceneService>(_ => new SceneService());
      containerBuilder.AddSingleton<IGameObjectFactory>(container => new GameObjectFactory(container.Resolve<IPrefabLoaderService>(),
        _slotPrefabKey,
        _rewardObjectPrefabKey,
        _rouletteViewPrefabKey,
        _loadingWindowPrefabKey));
    }
  }
}