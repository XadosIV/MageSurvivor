using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] PlayerHealth playerHealth;

    public float sightRange = 100f;
    public float attackRange = 1.8f;
    public int damage = 10;
    public float attackCooldown = 1f;

    NavMeshAgent agent;
    float nextHit;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player && !playerHealth) player.TryGetComponent(out playerHealth);
    }

    void Update()
    {
        if (!player) return;
        agent.SetDestination(player.position);

        float d = Vector3.Distance(transform.position, player.position);
        if (d <= attackRange && Time.time >= nextHit)
        {
            nextHit = Time.time + attackCooldown;
            if (playerHealth) playerHealth.TakeDamage(damage);
        }
    }
}
