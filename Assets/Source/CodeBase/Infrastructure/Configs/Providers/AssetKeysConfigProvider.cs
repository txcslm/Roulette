using Source.CodeBase.Infrastructure.Configs.Interfaces;

namespace Source.CodeBase.Infrastructure.Configs
{
  public class AssetKeysConfigProvider : ConfigProvider<AssetKeysConfig>, IAssetKeysConfigProvider
  {
    public AssetKeysConfigProvider(AssetKeysConfig config) : base(config) { }
  }
}