using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using Source.CodeBase.Presenters;
using TMPro;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services
{
  public class RewardAnimationService : IRewardAnimationService
  {
    private readonly List<RewardObjectPresenter> _activeRewardObjects = new List<RewardObjectPresenter>();
    private readonly IRouletteConfigProvider _configProvider;
    private readonly IRewardObjectPoolService _rewardObjectPool;

    public RewardAnimationService(IRewardObjectPoolService rewardObjectPool, IRouletteConfigProvider configProvider)
    {
      _rewardObjectPool = rewardObjectPool;
      _configProvider = configProvider;
    }

    public async UniTask PlayRewardAnimationAsync(RouletteSlot winningSlot, Transform container, TextMeshProUGUI counterText)
    {
      if (counterText == null || container == null) return;

      counterText.gameObject.SetActive(true);
      counterText.text = "0";

      var rewardObjects = CalculateRewardObjects(winningSlot.Value);

      var slotWorldPosition = GetSlotWorldPosition(winningSlot.SlotIndex);

      var spawnTasks = new List<UniTask>();

      for (int i = 0; i < rewardObjects.Count; i++)
      {
        int objectValue = rewardObjects[i];
        spawnTasks.Add(SpawnRewardObjectAsync(container, winningSlot, objectValue, counterText, slotWorldPosition));
      }

      await UniTask.WhenAll(spawnTasks);

      await UniTask.Delay(_configProvider.Config.RewardAnimationPause * 1000);
      counterText.gameObject.SetActive(false);
    }

    public void ClearRewardObjects()
    {
      foreach (var presenter in _activeRewardObjects)
      {
        if (presenter?.View != null)
        {
          _rewardObjectPool.ReturnRewardObject(presenter.View);
          presenter.Dispose();
        }
      }
      _activeRewardObjects.Clear();
    }

    private List<int> CalculateRewardObjects(int totalReward)
    {
      var result = new List<int>();

      if (totalReward <= _configProvider.Config.MaxRewardObjects)
      {
        for (int i = 0; i < totalReward; i++)
        {
          result.Add(1);
        }
      }
      else
      {
        int baseValue = totalReward / _configProvider.Config.MaxRewardObjects;
        int remainder = totalReward % _configProvider.Config.MaxRewardObjects;

        if (remainder == 0)
        {
          for (int i = 0; i < _configProvider.Config.MaxRewardObjects; i++)
          {
            result.Add(baseValue);
          }
        }
        else
        {
          for (int i = 0; i < remainder; i++)
          {
            result.Add(baseValue + 1);
          }
          for (int i = 0; i < _configProvider.Config.MaxRewardObjects - remainder; i++)
          {
            result.Add(baseValue);
          }
        }
      }

      return result;
    }

    private Vector3 GetSlotWorldPosition(int slotIndex) =>
      new Vector3(0, _configProvider.Config.SlotRadius, 0);

    private async UniTask SpawnRewardObjectAsync(Transform container, RouletteSlot winningSlot, int objectValue, TextMeshProUGUI counterText, Vector3 slotPosition)
    {
      var rewardObjectView = _rewardObjectPool.GetRewardObject();

      float randomRadius = Random.Range(_configProvider.Config.SpawnRadiusMin, _configProvider.Config.SpawnRadiusMax);
      float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

      var offsetFromSlot = new Vector3(Mathf.Cos(randomAngle) * randomRadius,
        Mathf.Sin(randomAngle) * randomRadius,
        0);

      var spawnPosition = slotPosition + offsetFromSlot;

      var rewardObjectData = new RewardObjectData
      {
        StartPosition = spawnPosition,
        TargetPosition = Vector3.zero,
        Icon = winningSlot.Icon,
        ValueText = objectValue.ToString(),
        IsActive = true
      };

      var presenter = new RewardObjectPresenter(rewardObjectView, rewardObjectData);
      presenter.Initialize();

      rewardObjectView.transform.SetParent(container, false);
      _activeRewardObjects.Add(presenter);

      var rectTransform = rewardObjectView.RectTransform;
      rectTransform.localPosition = spawnPosition;
      rectTransform.localScale = Vector3.zero;
      rectTransform.sizeDelta = _configProvider.Config.RewardObjectSize;

      await rectTransform.DOScale(Vector3.one, _configProvider.Config.RewardObjectScaleAnimationDuration).AsyncWaitForCompletion().AsUniTask();

      float waitTime = Random.Range(_configProvider.Config.RewardObjectMinDelay, _configProvider.Config.RewardObjectMaxDelay);
      await UniTask.Delay((int)(waitTime * 1000));

      var moveToCenter = rectTransform.DOLocalMove(Vector3.zero, _configProvider.Config.RewardObjectMoveToCenterDuration);
      await moveToCenter.AsyncWaitForCompletion().AsUniTask();

      int currentCount = int.Parse(counterText.text);
      counterText.text = (currentCount + objectValue).ToString();

      _rewardObjectPool.ReturnRewardObject(rewardObjectView);
      _activeRewardObjects.Remove(presenter);
    }
  }
}