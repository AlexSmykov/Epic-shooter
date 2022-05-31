using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Способность спавна врагов после смерти
/// </summary>
[RequireComponent(typeof(SpawnEnemiesAbility))]
public class SpawnEnemiesAfterDieAbility : MonoBehaviour
{
    [SerializeField] private GameObject _EnemyToSpawn;
    [SerializeField] private int _EnemyCount;
    private SpawnEnemiesAbility _SpawnEnemiesAbility;

    void Start()
    {
        _SpawnEnemiesAbility = GetComponent<SpawnEnemiesAbility>();
    }

    public void OnDeath()
    {
        Debug.Log(_EnemyToSpawn);
        _SpawnEnemiesAbility.SpawnEnemies(_EnemyToSpawn, _EnemyCount);
    }
}
