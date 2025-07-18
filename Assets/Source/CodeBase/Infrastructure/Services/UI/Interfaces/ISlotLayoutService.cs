using System.Collections.Generic;
using Source.CodeBase.Infrastructure.Configs.Interfaces;
using Source.CodeBase.Models;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface ISlotLayoutService
  {
    void Initialize(Transform container, IRouletteConfigProvider configProvider);

    void UpdateSlots(IReadOnlyList<RouletteSlot> slots);

    void ClearSlots();
  }
}