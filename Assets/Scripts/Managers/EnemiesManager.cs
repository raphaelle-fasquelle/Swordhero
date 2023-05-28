using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private List<EnemyController> _enemies;

    [SerializeField] private float _enemiesSpawnDelay;

    public void Init()
    {
        _enemies = transform.GetComponentsInChildren<EnemyController>().ToList();
        foreach (EnemyController enemyController in _enemies)
        {
            enemyController.Init();
        }
    }

    public EnemyController GetClosestAliveEnemyInConeRange(Transform referenceTransform, float coneAngle, float distance)
    {
        EnemyController[] matchingEnemies = _enemies.Where(enemy =>
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
}
