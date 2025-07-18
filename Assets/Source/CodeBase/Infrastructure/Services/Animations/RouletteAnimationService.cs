using Source.CodeBase.Infrastructure.Services.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Services
{
  public class RouletteAnimationService : IRouletteAnimationService
  {
    public float CalculateFinalRotation(int winningSlotIndex, int slotsCount)
    {
      float anglePerSlot = 360f / slotsCount;
      float targetAngle = winningSlotIndex * anglePerSlot;

      int fullRotations = Random.Range(5, 10);
      float totalRotation = fullRotations * 360f;

      return totalRotation + targetAngle;
    }
  }
}