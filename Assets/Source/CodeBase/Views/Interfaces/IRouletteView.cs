using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Models;
using TMPro;
using UnityEngine;

namespace Source.CodeBase.Views.Interfaces
{
  public interface IRouletteView : IWindowView
  {
    Transform RewardAnimationContainer { get; }

    TextMeshProUGUI RewardCounterText { get; }

    RectTransform SlotContainer { get; }

    void SetupSlots(IReadOnlyList<RouletteSlot> slots);

    void SetButtonInteractable(bool interactable);

    void SetButtonText(string text);

    void UpdateCooldownTimer(string formattedText);

    void UpdateRewardTypeIcon(Sprite icon);

    UniTask PlaySpinAnimationAsync(float finalRotationZ, int duration);

    event Action SpinButtonClicked;
  }
}