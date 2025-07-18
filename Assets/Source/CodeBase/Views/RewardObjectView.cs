using UnityEngine;
using UnityEngine.UI;

namespace Source.CodeBase.Views
{
  public class RewardObjectView : BaseWindow
  {
    [SerializeField] private Image _icon;

    public RectTransform RectTransform => (RectTransform)transform;

    public void Setup(Sprite icon, string valueText)
    {
      if (_icon != null)
        _icon.sprite = icon;
    }

    public void SetPosition(Vector3 position)
    {
      transform.position = position;
    }

    public void SetActive(bool active)
    {
      gameObject.SetActive(active);
    }
  }
}