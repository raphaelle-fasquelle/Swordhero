using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemiesManager : MonoBehaviour
{
    [Header("Spawn parameters")]
    [SerializeField] private int _enemiesCountOnStart;
    [SerializeField] private float _enemiesSpawnDelay;
    [SerializeField] private Vector2 _minSpawnBounds;
    [SerializeField] private Vector2 _maxSpawnBounds;
    [Space(10)]
    [Header("External Links")]
    [SerializeField] private EnemyController _enemyPrefab;
    [Space(10)]
    [Header("Internal Links")]
    [SerializeField] private Transform _enemiesInPoolParent;
    [SerializeField] private Transform _enemiesSpawnedParent;
    
    private readonly List<EnemyController> _enemiesSpawned = new List<EnemyController>();
    private readonly Queue<EnemyController> _enemiesInPool = new Queue<EnemyController>();

    private float _lastSpawnTime;

    public void Init()
    {
        for (int i = 0; i < _enemiesCountOnStart; i++)
        {
            SpawnEnemy();
        }

        _lastSpawnTime = Time.time;
    }

    public EnemyController GetClosestAliveEnemyInConeRange(Transform referenceTransform, float coneAngle, float distance)
    {
        EnemyController[] matchingEnemies = _enemiesSpawned.Where(enemy =>
            enemy.IsAlive 
            && enemy.IsInCone(referenceTransform.position, referenceTransform.forward, coneAngle)
            && Vector3.Distance(referenceTransform.position, enemy.transform.position) < distance).ToArray();
        if (matchingEnemies.Length == 0) return null;
        EnemyController closestEnemy = matchingEnemies[0];
        foreach (EnemyController enemyController in matchingEnemies)
        {
            if (enemyController.SqrDistanceToPosition(referenceTransform.position) <
                closestEnemy.SqrDistanceToPosition(referenceTransform.position))
            {
                closestEnemy = enemyController;
            }
        }
        return closestEnemy;
    }

    private void Update()
    {
        if (Time.time - _lastSpawnTime >= _enemiesSpawnDelay)
        {
            SpawnEnemy();
            _lastSpawnTime = Time.time;
        }
    }

    private void SpawnEnemy()
    {
        EnemyController enemyController = PickEnemy();
        enemyController.Spawn(new Vector3(Random.Range(_minSpawnBounds.x, _maxSpawnBounds.x), 0,
            Random.Range(_minSpawnBounds.y, _maxSpawnBounds.y)));
        _enemiesSpawned.Add(enemyController);
        enemyController.transform.parent = _enemiesSpawnedParent;
    }

    private EnemyController PickEnemy()
    {
        if (_enemiesInPool.Count > 0) return _enemiesInPool.Dequeue();
        return CreateNewEnemy();
    }

    private EnemyController CreateNewEnemy()
    {
        EnemyController enemyController = Instantiate(_enemyPrefab, transform);
        enemyController.Init(this);
        return enemyController;
    }

    public void ReturnToPool(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
        _enemiesInPool.Enqueue(enemy);
        _enemiesSpawned.Remove(enemy);
        enemy.transform.parent = _enemiesInPoolParent;
    }
}
