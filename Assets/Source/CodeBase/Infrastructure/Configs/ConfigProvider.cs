using Source.CodeBase.Infrastructure.Configs.Interfaces;
using UnityEngine;

namespace Source.CodeBase.Infrastructure.Configs
{
  public class ConfigProvider<T> : IConfigProvider<T> where T : ScriptableObject
  {
    protected ConfigProvider(T config) =>
      Config = config;

    public T Config { get; }
  }
}