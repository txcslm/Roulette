using Reflex.Core;
using Source.CodeBase.Infrastructure.Configs;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using Source.CodeBase.Models.Interfaces;
using Source.CodeBase.Presenters;
using Source.CodeBase.Presenters.Interfaces;
using Source.CodeBase.Views;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Installers
{
  public class MainSceneInstaller : MonoBehaviour, IInstaller
  {
    [SerializeField] private RouletteConfig _config;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
      containerBuilder.AddSingleton<IRouletteConfigProvider>(_ => new RouletteConfigProvider(_config));

      containerBuilder.AddSingleton<IRewardDataService>(container => new RewardDataService(container.Resolve<IAssetService>()));

      containerBuilder.AddSingleton<IRouletteAnimationService>(_ => new RouletteAnimationService());
      containerBuilder.AddSingleton<IUIFormatterService>(_ => new UIFormatterService());

      containerBuilder.AddSingleton<IObjectPool<SlotView>>(container => new ObjectPool<SlotView>(() => container
          .Resolve<IGameObjectFactory>()
          .CreateSlot(),
        maxSize: container.Resolve<IRouletteConfigProvider>().Config.SlotPoolMaxSize));

      containerBuilder
        .AddSingleton<IObjectPool<RewardObjectView>>(container => new ObjectPool<RewardObjectView>(() => container
            .Resolve<IGameObjectFactory>()
            .CreateRewardObject(),
          onReturn: rewardObject =>
          {
            if (rewardObject != null)
            {
              rewardObject.transform.localPosition = Vector3.zero;
              rewardObject.transform.localRotation = Quaternion.identity;
              rewardObject.transform.localScale = Vector3.one;
              rewardObject.transform.SetParent(null);
            }
          },
          maxSize: container.Resolve<IRouletteConfigProvider>().Config.RewardObjectPoolMaxSize));

      containerBuilder
        .AddSingleton<ISlotLayoutService>(container => new SlotLayoutService(container
          .Resolve<IObjectPool<SlotView>>()));

      containerBuilder
        .AddSingleton<IRewardObjectPoolService>(container => new RewardObjectPoolService(container
          .Resolve<IObjectPool<RewardObjectView>>()));

      containerBuilder
        .AddSingleton<IRewardAnimationService>(container => new RewardAnimationService(container.Resolve<IRewardObjectPoolService>(),
          container.Resolve<IRouletteConfigProvider>()));

      containerBuilder
        .AddSingleton<IRouletteModel>(container => new RouletteModel(container.Resolve<IRewardDataService>(),
          container.Resolve<IRouletteConfigProvider>()));

      containerBuilder
        .AddTransient<IRoulettePresenter>(container => new RoulettePresenter(container.Resolve<IRouletteModel>(),
          container.Resolve<IRewardAnimationService>(),
          container.Resolve<IRouletteAnimationService>(),
          container.Resolve<ISlotLayoutService>(),
          container.Resolve<IUIFormatterService>(),
          container.Resolve<IRouletteConfigProvider>()));
    }
  }
}