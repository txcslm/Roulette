namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IRouletteAnimationService
  {
    float CalculateFinalRotation(int winningSlotIndex, int slotsCount);
  }
}