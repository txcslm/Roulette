using Source.CodeBase.Views.Interfaces;

namespace Source.CodeBase.Presenters.Interfaces
{
  public interface IRoulettePresenter : IWindowPresenter<IRouletteView>
  {
    void SetView(IRouletteView view);
  }
}