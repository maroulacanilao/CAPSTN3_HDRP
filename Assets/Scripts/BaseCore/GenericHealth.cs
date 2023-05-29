using CustomEvent;

namespace BaseCore
{
    [System.Serializable]
    public class GenericHealth
    {
        private int currentHealth;
        private int maxHealth;
        
        public int CurrentHealth => currentHealth;

        public int MaxHealth => maxHealth;
        
        public readonly Evt<GenericHealth> onHealthChanged = new Evt<GenericHealth>();

        public GenericHealth(int maxHealth_)
        {
            maxHealth = maxHealth_;
            currentHealth = maxHealth;
        }
        
        public void AddHealth(int amount_)
        {
            currentHealth += amount_;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            if(currentHealth < 0) currentHealth = 0;
            onHealthChanged?.Invoke(this);
        }
        
        public void SetHealth(int amount_)
        {
            currentHealth = amount_;
            if (currentHealth > maxHealth) currentHealth = maxHealth;
            if(currentHealth < 0) currentHealth = 0;
            onHealthChanged?.Invoke(this);
        }
    }
}
