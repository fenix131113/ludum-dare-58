using System;

namespace HealthSystem
{
    public interface IHealth
    {
        event Action<int, int> OnHealthChanged;
        event Action OnDeath;
        
        HealthType GetHealthType();
        int GetHealth();
        int GetMaxHealth();
        void ChangeHealth(int health);
    }
}