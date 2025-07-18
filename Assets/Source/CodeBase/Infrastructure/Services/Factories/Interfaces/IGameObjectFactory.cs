using Cysharp.Threading.Tasks;
using Source.CodeBase.Views;

namespace Source.CodeBase.Infrastructure.Services.Interfaces
{
  public interface IGameObjectFactory
  {
    UniTask InitializeAsync();

    SlotView CreateSlot();

    RewardObjectView CreateRewardObject();
    
    RouletteView CreateRouletteView();
    
    UniTask<LoadingWindow> CreateLoadingWindowAsync();
  }
}