using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CurrentEnemies : MonoBehaviour
{
    private static CurrentEnemies instance;
    public static CurrentEnemies Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<CurrentEnemies>();
                if (instance == null)
                {
                    GameObject go = new GameObject("CurrentEnemies");
                    instance = go.AddComponent<CurrentEnemies>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<GameObject> activeEnemyGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> backupEnemyGameObjects = new List<GameObject>();
    private List<EnemyData> activeEnemyData = new List<EnemyData>();
    private List<EnemyData> backupEnemyData = new List<EnemyData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void OnApplicationQuit()
    {
        if (instance == this)
        {
            instance = null;
            ClearAllEnemies();
        }
    }

    public void AddEnemyGameObject(GameObject enemy)
    {
        if (enemy == null) return;

        // Check if enemy is already in either list
        if (activeEnemyGameObjects.Contains(enemy) || backupEnemyGameObjects.Contains(enemy))
        {
            Debug.LogWarning($"Enemy {enemy.name} is already registered!");
            return;
        }

        var selector = enemy.GetComponent<EnemySelector>();
        if (selector == null || selector.SelectedEnemy == null)
        {
            Debug.LogWarning($"Enemy {enemy.name} has no valid EnemySelector or SelectedEnemy!");
            return;
        }

        // If this is the first enemy, add to active list
        if (activeEnemyGameObjects.Count == 0)
        {
            activeEnemyGameObjects.Add(enemy);
            activeEnemyData.Add(selector.SelectedEnemy);
        }
        else
        {
            // Otherwise add to backup list
            backupEnemyGameObjects.Add(enemy);
            backupEnemyData.Add(selector.SelectedEnemy);
        }
    }

    public void RemoveEnemyGameObject(GameObject enemy)
    {
        if (enemy == null) return;

        // Check if it's the active enemy
        if (activeEnemyGameObjects.Contains(enemy))
        {
            activeEnemyGameObjects.Remove(enemy);
            var selector = enemy.GetComponent<EnemySelector>();
            if (selector != null && selector.SelectedEnemy != null)
            {
                activeEnemyData.Remove(selector.SelectedEnemy);
            }

            // If we have backup enemies, promote the first one
            if (backupEnemyGameObjects.Count > 0)
            {
                var newActive = backupEnemyGameObjects[0];
                backupEnemyGameObjects.RemoveAt(0);
                activeEnemyGameObjects.Add(newActive);

                var newActiveSelector = newActive.GetComponent<EnemySelector>();
                if (newActiveSelector != null && newActiveSelector.SelectedEnemy != null)
                {
                    activeEnemyData.Add(newActiveSelector.SelectedEnemy);
                    backupEnemyData.Remove(newActiveSelector.SelectedEnemy);
                }
            }
        }
        else if (backupEnemyGameObjects.Contains(enemy))
        {
            backupEnemyGameObjects.Remove(enemy);
            var selector = enemy.GetComponent<EnemySelector>();
            if (selector != null && selector.SelectedEnemy != null)
            {
                backupEnemyData.Remove(selector.SelectedEnemy);
            }
        }
    }

    public List<GameObject> GetEnemyGameObjects()
    {
        return activeEnemyGameObjects.Concat(backupEnemyGameObjects).ToList();
    }

    public List<EnemySelector> GetEnemySelectors()
    {
        return GetEnemyGameObjects()
            .Select(go => go.GetComponent<EnemySelector>())
            .Where(selector => selector != null)
            .ToList();
    }

    public List<EnemyData> GetEnemyData()
    {
        return activeEnemyData.Concat(backupEnemyData).ToList();
    }

    public List<EnemySelector> GetEnemySelectorsByType(string typeName)
    {
        return GetEnemySelectors()
            .Where(selector => selector.SelectedEnemy != null && 
                   selector.SelectedEnemy.types.Exists(t => t.typeName == typeName))
            .ToList();
    }

    public List<EnemyData> GetEnemyDataByType(string typeName)
    {
        return GetEnemyData()
            .Where(data => data.types.Exists(t => t.typeName == typeName))
            .ToList();
    }

    public bool IsEnemyActive(GameObject enemy)
    {
        return activeEnemyGameObjects.Contains(enemy);
    }

    public void ClearAllEnemies()
    {
        activeEnemyGameObjects.Clear();
        backupEnemyGameObjects.Clear();
        activeEnemyData.Clear();
        backupEnemyData.Clear();
    }

    // Properties to access active enemy
    public GameObject ActiveEnemyGameObject => activeEnemyGameObjects.Count > 0 ? activeEnemyGameObjects[0] : null;
    public EnemySelector ActiveEnemySelector => ActiveEnemyGameObject?.GetComponent<EnemySelector>();
    public EnemyData ActiveEnemyData => ActiveEnemySelector?.SelectedEnemy;

    // Properties to access backup enemies
    public List<GameObject> BackupEnemyGameObjects => backupEnemyGameObjects;
    public List<EnemyData> BackupEnemyData => backupEnemyData;

    // Helper methods for active enemy stats
    public float GetActiveEnemyStat(string statName)
    {
        if (ActiveEnemyData == null) return 0f;
        var stat = ActiveEnemyData.stats.Find(s => s.statDefinition.statName == statName);
        return stat?.value ?? 0f;
    }

    public float GetActiveEnemySpeed()
    {
        return GetActiveEnemyStat("Speed");
    }
} 