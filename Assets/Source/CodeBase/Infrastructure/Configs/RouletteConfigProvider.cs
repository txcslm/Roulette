using Source.CodeBase.Infrastructure.Configs.Interfaces;

namespace Source.CodeBase.Infrastructure.Configs
{
  public class RouletteConfigProvider : ConfigProvider<RouletteConfig>, IRouletteConfigProvider
  {
    public RouletteConfigProvider(RouletteConfig config) : base(config) { }
  }
}