using UnityEngine;

public class explosionLifetime : MonoBehaviour
{
    float lifetime = 2f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        print("Explosion hit: " + other.name);
        other.GetComponent<Health>()?.TakeDamage(10);
    }
};
