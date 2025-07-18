using Source.CodeBase.Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.CodeBase.Views
{
  public class LoadingWindow : BaseWindow
  {
    private const float SpinSpeed = GameConstants.LoadingSpinnerSpeed;
    
    [SerializeField] private TextMeshProUGUI _loadingText;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private Image _loadingSpinner;

    protected override void Awake()
    {
      base.Awake();

      if (_loadingText != null)
        _loadingText.text = GameConstants.LoadingDefaultText;

      if (_loadingBar != null)
        _loadingBar.fillAmount = 0f;
    }

    private void Update()
    {
      if (_loadingSpinner != null && gameObject.activeInHierarchy)
      {
        _loadingSpinner.transform.Rotate(0, 0, -SpinSpeed * Time.deltaTime);
      }
    }

    public void SetProgress(float progress)
    {
      if (_loadingBar != null)
        _loadingBar.fillAmount = Mathf.Clamp01(progress);
    }

    public void SetText(string text)
    {
      if (_loadingText != null)
        _loadingText.text = text;
    }
  }
}