using System;
using Cysharp.Threading.Tasks;

namespace Source.CodeBase.Views.Interfaces
{
  public interface IWindowView
  {
    void Show();

    void Hide();
    

    event Action StartShow;

    event Action EndShow;

    event Action StartHide;

    event Action EndHide;
  }
}