using System;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Views;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Source.CodeBase.Infrastructure.Bootstrappers
{
  public class InitialSceneBootstrapper : MonoBehaviour
  {
    [SerializeField] private RectTransform _container;
    private LoadingWindow _loadingWindow;
    private IGameObjectFactory _gameObjectFactory;

    private ISceneService _sceneService;

    private void Awake()
    {
      StartInitialization().Forget();
    }

    private async UniTask StartInitialization()
    {
      await InitializeAsync();
    }

    [Inject]
    public void Construct(ISceneService sceneService, IGameObjectFactory gameObjectFactory)
    {
      _sceneService = sceneService;
      _gameObjectFactory = gameObjectFactory;
    }

    private async UniTask InitializeAsync()
    {
     _loadingWindow =  await _gameObjectFactory.CreateLoadingWindowAsync();
      _loadingWindow.transform.SetParent(_container, false);
      _loadingWindow.Show();
      _loadingWindow.EndShow += OnLoadingWindowShown;
    }

    private async void OnLoadingWindowShown()
    {
      _loadingWindow.EndShow -= OnLoadingWindowShown;

      try
      {
        await InitializeAddressables();
        await LoadRequiredAssets();
        await TransitionToMainScene();
      }
      catch (Exception ex)
      {
        Debug.LogError($"Initialization failed: {ex.Message}");
      }
    }

    private async UniTask InitializeAddressables()
    {
      _loadingWindow.SetText(GameConstants.InitializingAddressablesText);
      _loadingWindow.SetProgress(0.1f);

      await Addressables.InitializeAsync().ToUniTask();

      _loadingWindow.SetProgress(0.3f);
    }

    private async UniTask LoadRequiredAssets()
    {
      _loadingWindow.SetText(GameConstants.LoadingAssetsText);
      _loadingWindow.SetProgress(0.5f);

      await _gameObjectFactory.InitializeAsync();

      _loadingWindow.SetProgress(0.8f);
    }

    private async UniTask TransitionToMainScene()
    {
      _loadingWindow.SetText(GameConstants.LoadingMainSceneText);
      _loadingWindow.SetProgress(0.9f);

      await UniTask.Delay(500);

      _loadingWindow.SetProgress(1f);

      _loadingWindow.EndHide += OnLoadingWindowHidden;
      _loadingWindow.Hide();
    }

    private async void OnLoadingWindowHidden()
    {
      _loadingWindow.EndHide -= OnLoadingWindowHidden;

      var progress = new Progress<float>();
      await _sceneService.LoadSceneAsync(GameConstants.MainSceneName, progress);
    }
  }
}