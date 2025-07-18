using UnityEngine;

namespace Source.CodeBase.Models
{
  public class RewardObjectData
  {
    public Vector3 StartPosition { get; set; }

    public Vector3 TargetPosition { get; set; }

    public Sprite Icon { get; set; }

    public string ValueText { get; set; }

    public bool IsActive { get; set; }
  }
}