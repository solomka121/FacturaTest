using UnityEngine;

public class Health : MonoBehaviour
{
    [field:SerializeField] public float maxHealth { get; private set; }
    [field: SerializeField] public float currentHealth { get; private set; }

    public event System.Action<float> OnHealthChange;
    public event System.Action OnDeath;

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;
        
        OnHealthChange?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        OnDeath?.Invoke();
    }

    public void Reset()
    {
        currentHealth = maxHealth;
    }
}
