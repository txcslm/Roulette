using Reflex.Core;
using Source.CodeBase.Infrastructure.Configs;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Installers
{
  public class ProjectInstaller : MonoBehaviour, IInstaller
  {
    [SerializeField] private AssetKeysConfig _assetKeysConfig;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
      containerBuilder.AddSingleton<IAssetService>(_ => new AddressableAssetService());

      containerBuilder.AddSingleton<IPrefabLoaderService>(container => new PrefabLoaderService(container.Resolve<IAssetService>()));
      containerBuilder.AddSingleton<IAssetKeysConfigProvider>(_ => new AssetKeysConfigProvider(_assetKeysConfig));

      containerBuilder.AddSingleton<ISceneService>(_ => new SceneService());
      containerBuilder.AddSingleton<IGameObjectFactory>(container => new GameObjectFactory(container.Resolve<IPrefabLoaderService>(),
        container.Resolve<IAssetKeysConfigProvider>()));
    }
  }
}