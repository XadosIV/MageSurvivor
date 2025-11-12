using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Prefabs à spawner")]
    public GameObject[] mobPrefabs;

    [Header("Règles de spawn")]
    public bool autoStart = true;
    public float spawnInterval = 2f;
    public int maxAlive = 20;

    [Header("Zone de spawn (locale au spawner)")]
    public Vector2 areaSize = new Vector2(20f, 20f); 
    public bool snapToGround = true;
    public float raycastHeight = 10f;
    public LayerMask groundMask = ~0; 

    Transform spawnRoot;
    Coroutine loop;

    void Awake()
    {
        if (mobPrefabs == null || mobPrefabs.Length == 0)
            Debug.LogWarning("RandomSpawner: Aucun prefab configuré.");
        spawnRoot = new GameObject("Spawned").transform;
        spawnRoot.SetParent(transform, false);
    }

    void OnEnable()
    {
        if (autoStart) StartSpawning();
    }

    void OnDisable()
    {
        StopSpawning();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (loop == null) StartSpawning();
            else StopSpawning();
        }
    }

    public void StartSpawning()
    {
        if (loop == null) loop = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (loop != null)
        {
            StopCoroutine(loop);
            loop = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawnRoot.childCount < maxAlive)
                SpawnOne();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public GameObject SpawnOne()
    {
        if (mobPrefabs == null || mobPrefabs.Length == 0) return null;

        Vector3 pos = GetRandomPositionInArea();

        if (snapToGround)
        {
            Vector3 rayOrigin = new Vector3(pos.x, transform.position.y + raycastHeight, pos.z);
            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundMask))
                pos = hit.point;
        }

        GameObject prefab = mobPrefabs[Random.Range(0, mobPrefabs.Length)];
        return Instantiate(prefab, pos, Quaternion.identity, spawnRoot);
    }

    Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
        float z = Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);
        return transform.TransformPoint(new Vector3(x, 0f, z));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Matrix4x4 m = transform.localToWorldMatrix;
        Gizmos.matrix = m;
        Vector3 size = new Vector3(areaSize.x, 0.01f, areaSize.y);
        Gizmos.DrawCube(Vector3.zero, size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, 0f, areaSize.y));
    }
}