using System.Collections.Generic;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Models;
using Source.CodeBase.Views;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services
{
  public class SlotLayoutService : ISlotLayoutService
  {
    private readonly IObjectPool<SlotView> _slotPool;
    private readonly List<SlotView> _slotViews = new List<SlotView>();
    private IRouletteConfigProvider _configProvider;
    private Transform _container;

    public SlotLayoutService(IObjectPool<SlotView> slotPool) =>
      _slotPool = slotPool;

    public void Initialize(Transform container, IRouletteConfigProvider configProvider)
    {
      _container = container;
      _configProvider = configProvider;

      CreateSlots();
    }

    public void UpdateSlots(IReadOnlyList<RouletteSlot> slots)
    {
      for (int i = 0; i < slots.Count && i < _slotViews.Count; i++)
      {
        _slotViews[i].Setup(slots[i].Value.ToString());
      }
    }

    public void ClearSlots()
    {
      foreach (var slotView in _slotViews)
      {
        if (slotView != null)
        {
          _slotPool.Return(slotView);
        }
      }
      _slotViews.Clear();
    }

    private void CreateSlots()
    {
      for (int i = 0; i < _configProvider.Config.SlotsCount; i++)
      {
        var slotView = _slotPool.Get();

        if (slotView == null)
          continue;

        slotView.transform.SetParent(_container, false);

        float angle = i * _configProvider.Config.SlotAngle;
        float radians = angle * Mathf.Deg2Rad;
        float radius = _configProvider.Config.SlotRadius;
        var position = new Vector3(Mathf.Sin(radians) * radius,
          Mathf.Cos(radians) * radius,
          0);

        slotView.transform.localPosition = position;
        slotView.transform.localRotation = Quaternion.Euler(0, 0, -angle);
        _slotViews.Add(slotView);
      }
    }
  }
}