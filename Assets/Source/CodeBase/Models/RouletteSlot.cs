using System;
using UnityEngine;

namespace Source.CodeBase.Models
{
  [Serializable]
  public class RouletteSlot
  {
    public RouletteSlot(int value, RewardType rewardType, Sprite icon, int slotIndex)
    {
      Value = value;
      RewardType = rewardType;
      Icon = icon;
      SlotIndex = slotIndex;
    }

    public int Value { get; set; }

    public RewardType RewardType { get; set; }

    public Sprite Icon { get; set; }

    public int SlotIndex { get; set; }
  }
}