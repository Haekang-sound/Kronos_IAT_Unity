using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnEnemyWave : MonoBehaviour
{
    public GameObject Target;
    public EnemyController enemyPrefab;

    public float spawnCycleTime;
    private float spawnTimer;

    private IObjectPool<EnemyController> pool;
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int maxSize;
    [SerializeField] private int currentCapacity;

    private List<EnemyController> pooledObjects = new List<EnemyController>();

    private void Awake()
    {
        spawnTimer = spawnCycleTime;

        enemyPrefab.gameObject.SetActive(false);

        pool = new ObjectPool<EnemyController>(CreateEnemy,
            OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject,
            collectionCheck, maxSize, maxSize);
    }

    void Start()
    {
        if (Target == null)
        {
            Target = Player.Instance.gameObject;
        }
    }

    void Update()
    {
        if (currentCapacity < maxSize)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnCycleTime)
            {
                pool.Get();
				// 여기서 추가하자 
                currentCapacity++;
                spawnTimer = 0f;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var enemy in pooledObjects)
        {
            if (enemy != null && enemy.gameObject.activeInHierarchy)
            {
                enemy.GetComponent<ReplaceWithRagdoll>().Replace();
                enemy.Release();
            }
        }
    }

    private void SubtractCount()
    {
        currentCapacity--;
    }

    private EnemyController CreateEnemy()
    {
        EnemyController enemyInstance = Instantiate(enemyPrefab);
        enemyInstance.target = Target;
        enemyInstance.ObjectPool = pool;
        enemyInstance.transform.position = transform.position;

        pooledObjects.Add(enemyInstance);

        return enemyInstance;
        
    }

    private void OnGetFromPool(EnemyController pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(EnemyController pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.position = transform.position;
        pooledObject.transform.rotation = enemyPrefab.transform.rotation;
        SubtractCount();
    }

    private void OnDestroyPooledObject(EnemyController pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

	public void DestroyMe()
	{
		pool.Clear();
		Destroy(gameObject);
	}

}
