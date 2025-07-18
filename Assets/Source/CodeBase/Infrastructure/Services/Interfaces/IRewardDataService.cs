using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Models;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IRewardDataService
  {
    UniTask<List<RouletteSlot>> GenerateSlotsAsync(IRouletteConfigProvider configProvider, RewardType? lastRewardType);

    RewardType SelectNextRewardType(RewardType? lastType);
  }
}