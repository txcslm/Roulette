using UnityEngine;

namespace Source.CodeBase.Infrastructure.Configs
{
  [CreateAssetMenu(fileName = "RouletteConfig", menuName = "Game/Roulette Config")]
  public class RouletteConfig : ScriptableObject
  {
    [Header("Roulette Settings")]
    [SerializeField] private int _slotsCount = 12;

    [SerializeField] private float _slotRadius = 400f;

    [Header("Timing Settings")] 
    [SerializeField] private int _cooldownDuration = 10;

    [SerializeField] private int _spinAnimationDuration = 5;
    [SerializeField] private int _rewardAnimationPause = 2;

    [Header("Slot Value Settings")] 
    [SerializeField] private int _minSlotValue = 5;

    [SerializeField] private int _maxSlotValue = 100;
    [SerializeField] private int _slotValueStep = 5;

    [Header("Reward Animation Settings")] 
    [SerializeField] private int _maxRewardObjects = 20;

    [SerializeField] private float _rewardObjectScaleAnimationDuration = 0.3f;
    [SerializeField] private float _rewardObjectMoveToCenterDuration = 0.5f;
    [SerializeField] private float _rewardObjectMinDelay = 1f;
    [SerializeField] private float _rewardObjectMaxDelay = 2.5f;

    [Header("Reward Spawn Settings")] 
    [SerializeField] private float _spawnRadiusMin = 50f;

    [SerializeField] private float _spawnRadiusMax = 150f;

    [Header("Object Pool Settings")]
    [SerializeField] private int _slotPoolMaxSize = 12;

    [SerializeField] private int _rewardObjectPoolMaxSize = 20;

    [Header("Reward Object Settings")] 
    [SerializeField] private Vector2 _rewardObjectSize = new Vector2(70f, 70f);

    [Header("Timing Settings - Delays")]
    [SerializeField] private int _beforeSpinDelay = 500;

    [SerializeField] private int _spinProcessDelay = 1000;

    [Header("UI Settings")] 
    [SerializeField] private string _tryLuckButtonText = "Испытать удачу";

    [SerializeField] private string _cooldownButtonFormat = "{0}";

    [Header("Asset Keys")] 
    [SerializeField] private string _crystalIconKey = "diamond";

    [SerializeField] private string _coinIconKey = "coin";
    [SerializeField] private string _rubyIconKey = "ruby";

    public int SlotsCount => _slotsCount;

    public float SlotRadius => _slotRadius;

    public int CooldownDuration => _cooldownDuration;

    public int SpinAnimationDuration => _spinAnimationDuration;

    public int RewardAnimationPause => _rewardAnimationPause;

    public int MinSlotValue => _minSlotValue;

    public int MaxSlotValue => _maxSlotValue;

    public int SlotValueStep => _slotValueStep;

    public int MaxRewardObjects => _maxRewardObjects;

    public float RewardObjectScaleAnimationDuration => _rewardObjectScaleAnimationDuration;

    public float RewardObjectMoveToCenterDuration => _rewardObjectMoveToCenterDuration;

    public float RewardObjectMinDelay => _rewardObjectMinDelay;

    public float RewardObjectMaxDelay => _rewardObjectMaxDelay;

    public float SpawnRadiusMin => _spawnRadiusMin;

    public float SpawnRadiusMax => _spawnRadiusMax;

    public int SlotPoolMaxSize => _slotPoolMaxSize;

    public int RewardObjectPoolMaxSize => _rewardObjectPoolMaxSize;

    public Vector2 RewardObjectSize => _rewardObjectSize;

    public int BeforeSpinDelay => _beforeSpinDelay;

    public int SpinProcessDelay => _spinProcessDelay;

    public string TryLuckButtonText => _tryLuckButtonText;

    public string CooldownButtonFormat => _cooldownButtonFormat;

    public string CrystalIconKey => _crystalIconKey;

    public string CoinIconKey => _coinIconKey;

    public string RubyIconKey => _rubyIconKey;

    public float SlotAngle => 360f / _slotsCount;
  }
}