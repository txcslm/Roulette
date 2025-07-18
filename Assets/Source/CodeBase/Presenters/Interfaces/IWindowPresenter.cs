using System;
using Source.CodeBase.Views.Interfaces;

namespace Source.CodeBase.Presenters.Interfaces
{
  public interface IWindowPresenter<TView> : IDisposable where TView : IWindowView
  {
    TView View { get; }

    void Initialize();

#pragma warning disable CS0108, CS0114
    void Dispose();
#pragma warning restore CS0108, CS0114
  }
}