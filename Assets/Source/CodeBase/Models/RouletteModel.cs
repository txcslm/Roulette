using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Configs;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models.Interfaces;
using UniRx;
using Random = System.Random;

namespace Source.CodeBase.Models
{
  public class RouletteModel : IRouletteModel
  {
    private readonly IRouletteConfigProvider _configProvider;
    private readonly ReactiveProperty<int> _cooldownTime = new ReactiveProperty<int>(0);
    private readonly Random _random = new Random();
    private readonly Dictionary<RewardType, int> _rewardCounts = new Dictionary<RewardType, int>();
    private readonly IRewardDataService _rewardDataService;
    private readonly List<RouletteSlot> _slots = new List<RouletteSlot>();
    private readonly ReactiveProperty<RouletteState> _state = new ReactiveProperty<RouletteState>(RouletteState.Cooldown);

    private RewardType? _lastRewardType;
    private int _winningSlotIndex;

    public RouletteModel(IRewardDataService rewardDataService, IRouletteConfigProvider configProvider)
    {
      _rewardDataService = rewardDataService;
      _configProvider = configProvider;
      InitializeRewardCounts();
    }

    public event Action<int> CooldownTick;

    public IReadOnlyReactiveProperty<RouletteState> State => _state;

    public IReadOnlyReactiveProperty<int> CooldownTime => _cooldownTime;

    public IReadOnlyList<RouletteSlot> Slots => _slots;

    public void AddRewardCount(RewardType rewardType, int amount)
    {
      if (!_rewardCounts.TryAdd(rewardType, amount))
      {
        _rewardCounts[rewardType] += amount;
      }
    }

    public UniTask<int> SpinAsync()
    {
      if (_state.Value != RouletteState.Active)
      {
        return UniTask.FromResult(-1);
      }

      _state.Value = RouletteState.SelectingReward;

      _winningSlotIndex = _random.Next(0, _configProvider.Config.SlotsCount);

      return UniTask.FromResult(_winningSlotIndex);
    }

    public async UniTask StartCooldownAsync()
    {
      _state.Value = RouletteState.Cooldown;
      _cooldownTime.Value = _configProvider.Config.CooldownDuration;

      for (int i = _configProvider.Config.CooldownDuration; i >= 0; i--)
      {
        _cooldownTime.Value = i;

        if (i > 0)
        {
          await GenerateSlots();
        }

        CooldownTick?.Invoke(i);

        if (i == 0)
        {
          _state.Value = RouletteState.Active;
          break;
        }

        await UniTask.Delay(_configProvider.Config.SpinProcessDelay);
      }
    }

    public int GetRewardCount(RewardType rewardType) =>
      _rewardCounts.GetValueOrDefault(rewardType, 0);

    public RewardType GetCurrentRewardType() =>
      _slots.Count > 0 ? _slots[0].RewardType : RewardType.Coins;

    private async UniTask GenerateSlots()
    {
      _slots.Clear();

      var newSlots = await _rewardDataService.GenerateSlotsAsync(_configProvider, _lastRewardType);
      _slots.AddRange(newSlots);

      _lastRewardType = newSlots.Count > 0 ? newSlots[0].RewardType : RewardType.Coins;
    }

    private void InitializeRewardCounts()
    {
      _rewardCounts[RewardType.Coins] = 0;
      _rewardCounts[RewardType.Crystals] = 0;
      _rewardCounts[RewardType.Rubies] = 0;
    }
  }
}