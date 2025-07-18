using Source.CodeBase.Infrastructure.Services.Interfaces;
using Source.CodeBase.Views;

namespace Source.CodeBase.Infrastructure.Services
{
  public class RewardObjectPoolService : IRewardObjectPoolService
  {
    private readonly IObjectPool<RewardObjectView> _pool;

    public RewardObjectPoolService(IObjectPool<RewardObjectView> pool) =>
      _pool = pool;

    public RewardObjectView GetRewardObject() =>
      _pool.Get();

    public void ReturnRewardObject(RewardObjectView rewardObject)
    {
      _pool.Return(rewardObject);
    }

    public void ClearAllRewardObjects()
    {
      _pool.Clear();
    }
  }
}