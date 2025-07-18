using Source.CodeBase.Models;
using Source.CodeBase.Presenters.Interfaces;
using Source.CodeBase.Views;
using UnityEngine;

namespace Source.CodeBase.Presenters
{
  public class RewardObjectPresenter : IRewardObjectPresenter
  {
    private readonly RewardObjectData _data;

    public RewardObjectPresenter(RewardObjectView view, RewardObjectData data)
    {
      View = view;
      _data = data;
    }

    public RewardObjectView View { get; }

    public void Initialize()
    {
      View.Setup(_data.Icon, _data.ValueText);
      View.SetPosition(_data.StartPosition);
      View.SetActive(_data.IsActive);
    }

    public void Dispose()
    {
    }

    public void UpdatePosition(Vector3 position)
    {
      _data.StartPosition = position;
      View.SetPosition(position);
    }

    public void SetActive(bool active)
    {
      _data.IsActive = active;
      View.SetActive(active);
    }
  }
}