using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Source.CodeBase.Models.Interfaces
{
  public interface IRouletteModel
  {
    IReadOnlyReactiveProperty<RouletteState> State { get; }

    IReadOnlyReactiveProperty<int> CooldownTime { get; }

    IReadOnlyList<RouletteSlot> Slots { get; }

    event Action<int> CooldownTick;

    UniTask StartCooldownAsync();

    UniTask<int> SpinAsync();

    void AddRewardCount(RewardType rewardType, int amount);
  }
}