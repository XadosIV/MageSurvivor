using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    void Awake() { currentHealth = maxHealth; }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            Transform h = transform.Find("Hands");
            if (h) Destroy(h.gameObject);
            return;
        }

        Destroy(gameObject);
    }
}