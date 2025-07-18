using TMPro;
using UnityEngine;

namespace Source.CodeBase.Views
{
  public class SlotView : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _valueText;

    public void Setup(string slotValue)
    {
      if (_valueText != null)
        _valueText.text = slotValue;
    }
  }
}