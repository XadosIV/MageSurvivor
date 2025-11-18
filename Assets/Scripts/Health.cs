using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    void Awake() { currentHealth = maxHealth; }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
