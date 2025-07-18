using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using Source.CodeBase.Views.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.CodeBase.Views
{
  public class RouletteView : BaseWindow, IRouletteView
  {
    [SerializeField] private RectTransform _wheelTransform;
    [SerializeField] private RectTransform _slotsContainer;
    [SerializeField] private Button _spinButton;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TextMeshProUGUI _rewardCounterText;
    [SerializeField] private RectTransform _rewardAnimationContainer;
    [SerializeField] private Image _rewardTypeIcon;

    private ISlotLayoutService _slotLayoutService;

    public event Action SpinButtonClicked;

    public Transform RewardAnimationContainer => _rewardAnimationContainer;

    public TextMeshProUGUI RewardCounterText => _rewardCounterText;

    public RectTransform SlotContainer => _slotsContainer;

    protected override void Awake()
    {
      base.Awake();

      if (_spinButton != null)
      {
        _spinButton.onClick.AddListener(() => SpinButtonClicked?.Invoke());
      }

      if (_rewardCounterText != null)
      {
        _rewardCounterText.text = "0";
        _rewardCounterText.gameObject.SetActive(false);
      }
    }

    protected override void OnDestroy()
    {
      base.OnDestroy();

      if (_spinButton != null)
      {
        _spinButton.onClick.RemoveAllListeners();
      }

      DOTween.Kill(_wheelTransform);
    }

    public async UniTask PlaySpinAnimationAsync(float finalRotationZ, int duration)
    {
      if (!_wheelTransform)
        return;

      _wheelTransform.rotation = Quaternion.identity;

      await _wheelTransform
        .DORotate(new Vector3(0, 0, finalRotationZ), duration, RotateMode.FastBeyond360)
        .SetEase(Ease.OutCubic)
        .AsyncWaitForCompletion()
        .AsUniTask();
    }

    public void SetupSlots(IReadOnlyList<RouletteSlot> slots)
    {
      _slotLayoutService?.UpdateSlots(slots);
    }

    public void SetButtonInteractable(bool interactable)
    {
      if (_spinButton == null)
        return;

      _spinButton.interactable = interactable;

      var colors = _spinButton.colors;
      colors.normalColor = interactable ? Color.white : Color.gray;
      _spinButton.colors = colors;
    }

    public void SetButtonText(string text)
    {
      if (_buttonText != null)
      {
        _buttonText.text = text;
      }
    }

    public void UpdateCooldownTimer(string formattedText)
    {
      SetButtonText(formattedText);
    }

    public void UpdateRewardTypeIcon(Sprite icon)
    {
      if (!_rewardTypeIcon)
        return;

      _rewardTypeIcon.sprite = icon;
      _rewardTypeIcon.gameObject.SetActive(icon != null);
    }

    public void Initialize(ISlotLayoutService slotLayoutService)
    {
      _slotLayoutService = slotLayoutService;
    }
  }
}