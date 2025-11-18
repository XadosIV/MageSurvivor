using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float sightRange = 100f;
    public float attackRange = 1.8f;
    public int damage = 10;
    public float attackCooldown = 1f;

    NavMeshAgent agent;
    Transform player;
    Health playerHealth;
    float nextHit;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Init(Transform target)
    {
        player = target;
        playerHealth = null;
        if (player) player.TryGetComponent(out playerHealth);
    }

    void Update()
    {
        if (!player) return;

        if (!IsPlayerInSight()) return;

        if (agent && agent.enabled)
            agent.SetDestination(player.position);

        float d = Vector3.Distance(transform.position, player.position);
        if (d <= attackRange && Time.time >= nextHit)
        {
            nextHit = Time.time + attackCooldown;
            if (playerHealth) playerHealth.TakeDamage(damage);
        }
    }

    bool IsPlayerInSight()
    {
        if (!player) return false;
        float sqr = (player.position - transform.position).sqrMagnitude;
        return sqr <= sightRange * sightRange;
    }
}