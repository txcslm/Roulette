using UnityEngine;

namespace Source.CodeBase.Infrastructure.Configs.Interfaces
{
  public interface IConfigProvider<out T> where T : ScriptableObject
  {
    T Config { get; }
  }
}