using Source.CodeBase.Views;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IRewardObjectPoolService
  {
    RewardObjectView GetRewardObject();

    void ReturnRewardObject(RewardObjectView rewardObject);

    void ClearAllRewardObjects();
  }
}