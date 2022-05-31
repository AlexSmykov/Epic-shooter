using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Способность врага спавнить других врагов по вызову
/// </summary>
[RequireComponent(typeof(Enemy))]
public class SpawnEnemiesAbility : MonoBehaviour
{
    [SerializeField] private GameObject _EnemyToSpawn;
    [SerializeField] private int _EnemyCount;
    private Enemy _Enemy;

    private void Start()
    {
        _Enemy = GetComponent<Enemy>();
    }

    public void SpawnEnemies()
    {
        for(int i = 0; i < _EnemyCount; i++)
        {
            SpawnEnemy();
        }
    }
    public void SpawnEnemies(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            SpawnEnemy();
        }
    }
    public void SpawnEnemies(GameObject Enemy, int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            SpawnEnemy(Enemy);
        }
    }

    private void SpawnEnemy()
    {
        GameObject Enemy = Instantiate(_EnemyToSpawn, new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1), transform.position.z), Quaternion.identity);
        Enemy.GetComponent<Enemy>().Target = GetComponent<Enemy>().Target;
        Enemy.transform.parent = transform.parent;
        _Enemy.Room.GetComponent<Room>().EnemiesLeft.Add(Enemy);
    }
    private void SpawnEnemy(GameObject enemy)
    {
        GameObject Enemy = Instantiate(enemy, new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1), transform.position.z), Quaternion.identity);
        Enemy.GetComponent<Enemy>().Target = GetComponent<Enemy>().Target;
        Enemy.transform.parent = transform.parent;
        _Enemy.Room.GetComponent<Room>().EnemiesLeft.Add(Enemy);
    }
}
