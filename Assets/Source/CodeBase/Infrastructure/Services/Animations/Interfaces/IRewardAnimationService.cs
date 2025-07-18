using Cysharp.Threading.Tasks;
using Source.CodeBase.Models;
using TMPro;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IRewardAnimationService
  {
    UniTask PlayRewardAnimationAsync(RouletteSlot winningSlot, Transform container, TextMeshProUGUI counterText, float rouletteRotationZ);

    void ClearRewardObjects();
  }
}