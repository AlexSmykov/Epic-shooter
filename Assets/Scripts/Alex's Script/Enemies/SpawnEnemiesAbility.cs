using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesAbility : MonoBehaviour
{
    public GameObject EnemyToSpawn;

    public void SpawnEnemies(int Count)
    {
        for(int i = 0; i < Count; i++)
        {
            GameObject Enemy = Instantiate(EnemyToSpawn, new Vector3(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1), transform.position.z), Quaternion.identity);
            Enemy.GetComponent<Enemy>().Target = GetComponent<Enemy>().Target;
            Enemy.transform.parent = transform.parent;
            GetComponent<Enemy>().Room.GetComponent<Room>().EnemiesLeft.Add(Enemy);
        }
    }
}
