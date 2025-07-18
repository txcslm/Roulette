using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Configs;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using UnityEngine;
using Random = System.Random;

namespace Source.CodeBase.Infrastructure.Services
{
  public class RewardDataService : IRewardDataService
  {
    private readonly IAssetService _assetService;
    private readonly Random _random = new Random();

    public RewardDataService(IAssetService assetService) =>
      _assetService = assetService;

    public async UniTask<List<RouletteSlot>> GenerateSlotsAsync(IRouletteConfigProvider configProvider, RewardType? lastRewardType)
    {
      var slots = new List<RouletteSlot>();
      var slotValues = GenerateSlotValues(configProvider.Config);
      var currentRewardType = SelectNextRewardType(lastRewardType);
      var rewardIcon = await GetRewardIconAsync(currentRewardType, configProvider.Config);  

      for (int i = 0; i < configProvider.Config.SlotsCount; i++)
      {
        var slot = new RouletteSlot(slotValues[i], currentRewardType, rewardIcon, i);
        slots.Add(slot);
      }

      return slots;
    }

    public RewardType SelectNextRewardType(RewardType? lastType)
    {
      var availableTypes = Enum.GetValues(typeof(RewardType)).Cast<RewardType>().ToList();

      if (lastType.HasValue)
      {
        availableTypes.Remove(lastType.Value);
      }

      return availableTypes[_random.Next(availableTypes.Count)];
    }

    private async UniTask<Sprite> GetRewardIconAsync(RewardType rewardType, RouletteConfig config)
    {
      return rewardType switch
      {
        RewardType.Crystals => await LoadRewardIconAsync(config.CrystalIconKey),
        RewardType.Coins => await LoadRewardIconAsync(config.CoinIconKey),
        RewardType.Rubies => await LoadRewardIconAsync(config.RubyIconKey),
        _ => null
      };
    }

    private async UniTask<Sprite> LoadRewardIconAsync(string iconKey)
    {
      try
      {
        return await _assetService.LoadAssetAsync<Sprite>(iconKey);
      }
      catch (Exception ex)
      {
        Debug.LogWarning($"Failed to load icon {iconKey}: {ex.Message}");
        return null;
      }
    }

    private List<int> GenerateSlotValues(RouletteConfig config)
    {
      var values = new List<int>();
      var availableValues = new List<int>();

      for (int i = config.MinSlotValue; i <= config.MaxSlotValue; i += config.SlotValueStep)
      {
        availableValues.Add(i);
      }

      for (int i = 0; i < config.SlotsCount; i++)
      {
        int randomIndex = _random.Next(availableValues.Count);
        values.Add(availableValues[randomIndex]);
        availableValues.RemoveAt(randomIndex);
      }

      return values;
    }
  }
}