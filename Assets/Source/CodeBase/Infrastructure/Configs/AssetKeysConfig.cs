using UnityEngine;

namespace Source.CodeBase.Infrastructure.Configs
{
  [CreateAssetMenu(fileName = "AssetKeysConfig", menuName = "Game/Asset Keys Config")]
  public class AssetKeysConfig : ScriptableObject
  {
    [Header("Prefab Asset Keys")] 
    [SerializeField] private string _slotPrefabKey = "Prefabs/SlotPrefab.prefab";
    [SerializeField] private string _rewardObjectPrefabKey = "Prefabs/RewardObjectPrefab.prefab";
    [SerializeField] private string _rouletteViewPrefabKey = "Prefabs/RouletteView.prefab";
    [SerializeField] private string _loadingWindowPrefabKey = "Prefabs/LoadingWindow.prefab";

    public string RewardObjectPrefabKey => _rewardObjectPrefabKey;
    public string RouletteViewPrefabKey => _rouletteViewPrefabKey;
    public string SlotPrefabKey => _slotPrefabKey;
    public string LoadingWindowPrefabKey => _loadingWindowPrefabKey;
  }
}