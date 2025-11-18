using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] mobPrefabs;

    public Transform player;       
    public bool autoFindPlayer = true;
    public string playerTag = "Player";

    public bool autoStart = true;
    public float spawnInterval = 2f;
    public int maxAlive = 20;

    public Vector2 areaSize = new Vector2(20f, 20f);
    public float raycastHeight = 10f;
    public LayerMask groundMask = ~0;

    Transform spawnRoot;
    Coroutine loop;

    void Awake()
    {
        spawnRoot = new GameObject("Spawned").transform;
        spawnRoot.SetParent(transform, false);

        ResolvePlayerTarget();
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

    void ResolvePlayerTarget()
    {
        if (player || !autoFindPlayer) return;
        GameObject go = GameObject.FindWithTag(playerTag);
        if (go) player = go.transform;
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


        GameObject prefab = mobPrefabs[Random.Range(0, mobPrefabs.Length)];
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity, spawnRoot);

        var enemy = spawned.GetComponent<Enemy>();
        if (enemy)
        {
            if (!player) ResolvePlayerTarget(); 
            enemy.Init(player);
        }

        return spawned;
    }

    Vector3 GetRandomPositionInArea()
    {
        float x = Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f);
        float z = Random.Range(-areaSize.y * 0.5f, areaSize.y * 0.5f);
        return transform.TransformPoint(new Vector3(x, 0f, z));
    }


    //zone
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.15f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Vector3 size = new Vector3(areaSize.x, 0.01f, areaSize.y);
        Gizmos.DrawCube(Vector3.zero, size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, 0f, areaSize.y));
    }

}