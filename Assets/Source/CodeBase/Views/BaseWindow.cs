using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Source.CodeBase.Infrastructure;
using Source.CodeBase.Views.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Views
{
  public abstract class BaseWindow : MonoBehaviour, IWindowView
  {
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private float _animationDuration = GameConstants.BaseWindowAnimationDuration;

    protected virtual void Awake()
    {
      if (_canvasGroup == null)
        _canvasGroup = GetComponent<CanvasGroup>();

      if (_rectTransform == null)
        _rectTransform = GetComponent<RectTransform>();

      gameObject.SetActive(false);
    }

    protected virtual void OnDestroy()
    {
      DOTween.Kill(_canvasGroup);
      DOTween.Kill(_rectTransform);
    }

    public event Action StartShow;

    public event Action EndShow;

    public event Action StartHide;

    public event Action EndHide;

    public virtual void Show()
    {
      ShowAsync().Forget();
    }

    public virtual void Hide()
    {
      HideAsync().Forget();
    }

    private async UniTask ShowAsync()
    {
      gameObject.SetActive(true);
      StartShow?.Invoke();

      _canvasGroup.alpha = 0f;
      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;

      _rectTransform.localScale = Vector3.zero;

      var fadeTask = _canvasGroup.DOFade(1f, _animationDuration).AsyncWaitForCompletion();
      var scaleTask = _rectTransform.DOScale(Vector3.one, _animationDuration).SetEase(Ease.OutBack).AsyncWaitForCompletion();

      await UniTask.WhenAll(fadeTask.AsUniTask(), scaleTask.AsUniTask());

      _canvasGroup.interactable = true;
      _canvasGroup.blocksRaycasts = true;

      EndShow?.Invoke();
    }

    private async UniTask HideAsync()
    {
      StartHide?.Invoke();

      _canvasGroup.interactable = false;
      _canvasGroup.blocksRaycasts = false;

      var fadeTask = _canvasGroup
        .DOFade(0f, _animationDuration)
        .AsyncWaitForCompletion();
      var scaleTask = _rectTransform
        .DOScale(Vector3.zero, _animationDuration)
        .SetEase(Ease.InBack)
        .AsyncWaitForCompletion();

      await UniTask.WhenAll(fadeTask.AsUniTask(), scaleTask.AsUniTask());

      EndHide?.Invoke();
      gameObject.SetActive(false);
    }
  }
}