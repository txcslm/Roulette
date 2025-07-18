using System;
using Cysharp.Threading.Tasks;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using Source.CodeBase.Models.Interfaces;
using Source.CodeBase.Presenters.Interfaces;
using Source.CodeBase.Views;
using Source.CodeBase.Views.Interfaces;
using UniRx;
using UnityEngine;

namespace Source.CodeBase.Presenters
{
  public class RoulettePresenter : IRoulettePresenter
  {
    private readonly IRouletteConfigProvider _configProvider;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly IRouletteModel _model;
    private readonly IRewardAnimationService _rewardAnimationService;
    private readonly IRouletteAnimationService _rouletteAnimationService;
    private readonly ISlotLayoutService _slotLayoutService;
    private readonly IUIFormatterService _uiFormatterService;
    private bool _isInitialized;

    public RoulettePresenter(IRouletteModel model,
      IRewardAnimationService rewardAnimationService,
      IRouletteAnimationService rouletteAnimationService,
      ISlotLayoutService slotLayoutService,
      IUIFormatterService uiFormatterService,
      IRouletteConfigProvider configProvider)
    {
      _model = model;
      _rewardAnimationService = rewardAnimationService;
      _rouletteAnimationService = rouletteAnimationService;
      _slotLayoutService = slotLayoutService;
      _uiFormatterService = uiFormatterService;
      _configProvider = configProvider;
    }

    public IRouletteView View { get; private set; }

    public void SetView(IRouletteView view)
    {
      View = view;

      if (View is not RouletteView rouletteView)
        return;

      _slotLayoutService.Initialize(View.SlotContainer, _configProvider);
      rouletteView.Initialize(_slotLayoutService);
    }

    public void Initialize()
    {
      if (_isInitialized)
        return;

      SubscribeToModel();
      SubscribeToView();

      View.EndShow += OnWindowShown;
      View.StartHide += OnWindowHiding;

      _isInitialized = true;
    }

    public void Dispose()
    {
      _disposables?.Dispose();

      if (View != null)
      {
        View.EndShow -= OnWindowShown;
        View.StartHide -= OnWindowHiding;
        View.SpinButtonClicked -= OnSpinButtonClicked;
      }

      if (_model == null)
        return;

      _model.CooldownTick -= OnCooldownTick;
    }

    private void SubscribeToModel()
    {
      _model.State
        .Subscribe(OnStateChanged)
        .AddTo(_disposables);

      _model.CooldownTime
        .Subscribe(OnCooldownTimeChanged)
        .AddTo(_disposables);

      _model.CooldownTick += OnCooldownTick;
    }

    private void SubscribeToView()
    {
      View.SpinButtonClicked += OnSpinButtonClicked;
    }

    private void OnWindowShown()
    {
      _model.StartCooldownAsync().Forget();
    }

    private static void OnWindowHiding()
    {
    }

    private void OnStateChanged(RouletteState state)
    {
      switch (state)
      {
        case RouletteState.Active:
          View.SetButtonInteractable(true);
          View.SetButtonText(_configProvider.Config.TryLuckButtonText);
          View.SetupSlots(_model.Slots);
          View.UpdateRewardTypeIcon(null);
          break;

        case RouletteState.SelectingReward:
          View.SetButtonInteractable(false);
          View.SetButtonText(_configProvider.Config.TryLuckButtonText);
          View.UpdateRewardTypeIcon(null);
          break;

        case RouletteState.Cooldown:
          View.SetButtonInteractable(false);
          break;
      }
    }

    private void OnCooldownTimeChanged(int seconds)
    {
      if (seconds > 0)
      {
        string formattedText = _uiFormatterService.FormatCooldownText(seconds, _configProvider.Config.CooldownButtonFormat);
        View.UpdateCooldownTimer(formattedText);
      }
    }

    private void OnCooldownTick(int seconds)
    {
      string formattedText = _uiFormatterService.FormatCooldownText(seconds, _configProvider.Config.CooldownButtonFormat);
      View.UpdateCooldownTimer(formattedText);

      View.SetupSlots(_model.Slots);
      UpdateCenterIcon();
    }

    private void UpdateCenterIcon()
    {
      if (_model.Slots.Count > 0)
      {
        var rewardIcon = _model.Slots[0].Icon;
        View.UpdateRewardTypeIcon(rewardIcon);
      }
    }

    private async void OnSpinButtonClicked()
    {
      if (_model.State.Value != RouletteState.Active) return;

      try
      {
        _rewardAnimationService.ClearRewardObjects();

        int winningSlotIndex = await _model.SpinAsync();

        if (winningSlotIndex < 0)
          return;

        var winningSlot = _model.Slots[winningSlotIndex];
        Debug.Log($"Winning slot selected: {winningSlot.RewardType} - {winningSlot.Value}");

        await UniTask.Delay(_configProvider.Config.BeforeSpinDelay);

        float finalRotationZ = _rouletteAnimationService.CalculateFinalRotation(winningSlotIndex, _configProvider.Config.SlotsCount);

        await View.PlaySpinAnimationAsync(finalRotationZ, _configProvider.Config.SpinAnimationDuration);

        await _rewardAnimationService.PlayRewardAnimationAsync(winningSlot, View.RewardAnimationContainer, View.RewardCounterText);

        _model.AddRewardCount(winningSlot.RewardType, winningSlot.Value);

        await UniTask.Delay(_configProvider.Config.RewardAnimationPause * 1000);

        await _model.StartCooldownAsync();
      }
      catch (Exception ex)
      {
        Debug.LogError($"Error during spin: {ex.Message}");
      }
    }
  }
}