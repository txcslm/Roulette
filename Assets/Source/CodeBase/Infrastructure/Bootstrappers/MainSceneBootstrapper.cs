using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Presenters.Interfaces;
using Source.CodeBase.Views;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Bootstrappers
{
  public class MainSceneBootstrapper : MonoBehaviour
  {
    [SerializeField] private RectTransform _uiRoot;
    
    private IGameObjectFactory _gameObjectFactory;
    private IRoulettePresenter _roulettePresenter;
    private RouletteView _rouletteView;

    private async void Start()
    {
      await InitializeAsync();
    }

    [Inject]
    public void Construct(IGameObjectFactory gameObjectFactory, IRoulettePresenter roulettePresenter)
    {
      _gameObjectFactory = gameObjectFactory;
      _roulettePresenter = roulettePresenter;
    }

    private async UniTask InitializeAsync()
    {
      await CreateAndSetupRouletteView();
      InitializePresenter();
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    private async UniTask CreateAndSetupRouletteView()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
      _rouletteView = _gameObjectFactory.CreateRouletteView();
      _rouletteView.transform.SetParent(_uiRoot, false);
      
      _roulettePresenter.SetView(_rouletteView);
      
      _rouletteView.Show();
    }

    private void InitializePresenter()
    {
      _roulettePresenter.Initialize();
    }

    private void OnDestroy()
    {
      _roulettePresenter?.Dispose();
    }
  }
}