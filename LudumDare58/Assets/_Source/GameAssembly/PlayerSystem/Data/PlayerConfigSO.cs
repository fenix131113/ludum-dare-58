using UnityEngine;

namespace PlayerSystem.Data
{
    [CreateAssetMenu(fileName = "new PlayerConfigSO", menuName = "SO/PlayerConfigSO")]
    public class PlayerConfigSO : ScriptableObject
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}