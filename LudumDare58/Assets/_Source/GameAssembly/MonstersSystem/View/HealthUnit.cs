using UnityEngine;

namespace MonstersSystem.View
{
    public class HealthUnit : MonoBehaviour
    {
        [SerializeField] private GameObject filler;

        private int _maxUnitHp;
        private int _unitHp;

        public void Initialize(int maxUnitHp) => _maxUnitHp = maxUnitHp;

        public void SetDamage(int damage)
        {
            _unitHp = Mathf.Clamp(damage, 0, _maxUnitHp);
            
            filler.SetActive(_unitHp == _maxUnitHp);
        }
    }
}