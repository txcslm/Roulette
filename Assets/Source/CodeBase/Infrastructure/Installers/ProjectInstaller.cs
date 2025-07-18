using System.Collections.Generic;
using Reflex.Core;
using Source.CodeBase.Infrastructure.Services;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Installers
{
  public class ProjectInstaller : MonoBehaviour, IInstaller
  {
    [SerializeField] private string _slotPrefabKey = "SlotPrefab";
    [SerializeField] private string _rewardObjectPrefabKey = "RewardObjectPrefab";
    [SerializeField] private string _rouletteViewPrefabKey = "RouletteViewPrefab";

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
      containerBuilder.AddSingleton<IAssetService>(_ => new AddressableAssetService());

      containerBuilder.AddSingleton<IPrefabLoaderService>(container => new PrefabLoaderService(container.Resolve<IAssetService>()));

      containerBuilder.AddSingleton<ISceneService>(_ => new SceneService());
      containerBuilder.AddSingleton<IGameObjectFactory>(container => new GameObjectFactory(container.Resolve<IPrefabLoaderService>(),
        _slotPrefabKey,
        _rewardObjectPrefabKey,
        _rouletteViewPrefabKey));
    }
  }
}