using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    private List<EnemyController> _enemies;

    public void Init()
    {
        _enemies = transform.GetComponentsInChildren<EnemyController>().ToList();
        foreach (EnemyController enemyController in _enemies)
        {
            enemyController.Init();
        }
    }

    public EnemyController GetClosestAliveEnemy(Vector3 position)
    {
        EnemyController closestEnemy = _enemies.FirstOrDefault(enemy => enemy.IsAlive);
        if (closestEnemy == null)
        {
            Debug.Log("No more enemies alive");
        }
        foreach (EnemyController enemyController in _enemies)
        {
            if (enemyController.IsAlive)
            {
                if (enemyController.SqrDistanceToPosition(position) < closestEnemy.SqrDistanceToPosition(position))
                {
                    closestEnemy = enemyController;
                }
            }
        }

        return closestEnemy;
    }
}
