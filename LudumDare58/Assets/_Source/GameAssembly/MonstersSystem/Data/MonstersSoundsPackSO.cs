using UnityEngine;

namespace MonstersSystem.Data
{
    [CreateAssetMenu(fileName = "New MonstersSoundsPack", menuName = "SO/MonstersSoundsPack")]
    public class MonstersSoundsPackSO : ScriptableObject
    {
        [field: SerializeField] public AudioClip[] DeathSounds { get; private set; }
        [field: SerializeField] public AudioClip[] DamageSounds { get; private set; }
    }
}