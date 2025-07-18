using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Views;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services
{
  public class GameObjectFactory : IGameObjectFactory
  {
    private readonly IPrefabLoaderService _prefabLoader;
    private readonly string _rewardObjectPrefabKey;
    private readonly string _slotPrefabKey;
    private readonly string _rouletteViewPrefabKey;
    private readonly string _loadingWindowPrefabKey;
    private RewardObjectView _rewardObjectPrefab;
    private SlotView _slotPrefab;
    private RouletteView _rouletteViewPrefab;
    private LoadingWindow _loadingWindowPrefab;

    public GameObjectFactory(IPrefabLoaderService prefabLoader, string slotPrefabKey, string rewardObjectPrefabKey, string rouletteViewPrefabKey, string loadingWindowPrefabKey)    
    {
      _prefabLoader = prefabLoader;
      _slotPrefabKey = slotPrefabKey;
      _rewardObjectPrefabKey = rewardObjectPrefabKey;
      _rouletteViewPrefabKey = rouletteViewPrefabKey;
      _loadingWindowPrefabKey = loadingWindowPrefabKey;
    }

    public async UniTask InitializeAsync()
    {
      _slotPrefab = await _prefabLoader.LoadPrefabAsync<SlotView>(_slotPrefabKey);
      _rewardObjectPrefab = await _prefabLoader.LoadPrefabAsync<RewardObjectView>(_rewardObjectPrefabKey);
      _rouletteViewPrefab = await _prefabLoader.LoadPrefabAsync<RouletteView>(_rouletteViewPrefabKey);
    }

    public SlotView CreateSlot() =>
      Object.Instantiate(_slotPrefab);

    public RewardObjectView CreateRewardObject() =>
      Object.Instantiate(_rewardObjectPrefab);

    public RouletteView CreateRouletteView() =>
      Object.Instantiate(_rouletteViewPrefab);

    public async UniTask<LoadingWindow> CreateLoadingWindowAsync()
    {
      if (_loadingWindowPrefab == null)
      {
        _loadingWindowPrefab = await _prefabLoader.LoadPrefabAsync<LoadingWindow>(_loadingWindowPrefabKey);
      }
      return Object.Instantiate(_loadingWindowPrefab);
    }
  }
}