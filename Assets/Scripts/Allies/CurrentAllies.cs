using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CurrentAllies : MonoBehaviour
{
    private static CurrentAllies instance;
    public static CurrentAllies Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<CurrentAllies>();
                if (instance == null)
                {
                    GameObject go = new GameObject("CurrentAllies");
                    instance = go.AddComponent<CurrentAllies>();
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<GameObject> activeAllyGameObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> backupAllyGameObjects = new List<GameObject>();
    private List<AllyData> activeAllyData = new List<AllyData>();
    private List<AllyData> backupAllyData = new List<AllyData>();

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
            ClearAllAllies();
        }
    }

    public void AddAllyGameObject(GameObject ally)
    {
        if (ally == null) return;

        // Check if ally is already in either list
        if (activeAllyGameObjects.Contains(ally) || backupAllyGameObjects.Contains(ally))
        {
            Debug.LogWarning($"Ally {ally.name} is already registered!");
            return;
        }

        var selector = ally.GetComponent<AllySelector>();
        if (selector == null || selector.SelectedAlly == null)
        {
            Debug.LogWarning($"Ally {ally.name} has no valid AllySelector or SelectedAlly!");
            return;
        }

        // Get the sibling index of the ally in the scene hierarchy
        int siblingIndex = ally.transform.GetSiblingIndex();
        
        // If this is the first ally or it has a lower sibling index than the current active ally
        if (activeAllyGameObjects.Count == 0 || 
            (activeAllyGameObjects.Count > 0 && siblingIndex < activeAllyGameObjects[0].transform.GetSiblingIndex()))
        {
            // If we already have an active ally, move it to backup
            if (activeAllyGameObjects.Count > 0)
            {
                var currentActive = activeAllyGameObjects[0];
                backupAllyGameObjects.Add(currentActive);
                var currentSelector = currentActive.GetComponent<AllySelector>();
                if (currentSelector != null && currentSelector.SelectedAlly != null)
                {
                    backupAllyData.Add(currentSelector.SelectedAlly);
                }
                activeAllyGameObjects.Clear();
                activeAllyData.Clear();
            }

            // Add the new ally as active
            activeAllyGameObjects.Add(ally);
            activeAllyData.Add(selector.SelectedAlly);
        }
        else
        {
            // Add to backup list
            backupAllyGameObjects.Add(ally);
            backupAllyData.Add(selector.SelectedAlly);
        }
    }

    public void RemoveAllyGameObject(GameObject ally)
    {
        if (ally == null) return;

        // Check if it's the active ally
        if (activeAllyGameObjects.Contains(ally))
        {
            activeAllyGameObjects.Remove(ally);
            var selector = ally.GetComponent<AllySelector>();
            if (selector != null && selector.SelectedAlly != null)
            {
                activeAllyData.Remove(selector.SelectedAlly);
            }

            // If we have backup allies, promote the first one
            if (backupAllyGameObjects.Count > 0)
            {
                var newActive = backupAllyGameObjects[0];
                backupAllyGameObjects.RemoveAt(0);
                activeAllyGameObjects.Add(newActive);

                var newActiveSelector = newActive.GetComponent<AllySelector>();
                if (newActiveSelector != null && newActiveSelector.SelectedAlly != null)
                {
                    activeAllyData.Add(newActiveSelector.SelectedAlly);
                    backupAllyData.Remove(newActiveSelector.SelectedAlly);
                }
            }
        }
        else if (backupAllyGameObjects.Contains(ally))
        {
            backupAllyGameObjects.Remove(ally);
            var selector = ally.GetComponent<AllySelector>();
            if (selector != null && selector.SelectedAlly != null)
            {
                backupAllyData.Remove(selector.SelectedAlly);
            }
        }
    }

    public List<GameObject> GetAllyGameObjects()
    {
        return activeAllyGameObjects.Concat(backupAllyGameObjects).ToList();
    }

    public List<AllySelector> GetAllySelectors()
    {
        return GetAllyGameObjects()
            .Select(go => go.GetComponent<AllySelector>())
            .Where(selector => selector != null)
            .ToList();
    }

    public List<AllyData> GetAllyData()
    {
        return activeAllyData.Concat(backupAllyData).ToList();
    }

    public List<AllySelector> GetAllySelectorsByType(string typeName)
    {
        return GetAllySelectors()
            .Where(selector => selector.SelectedAlly != null && 
                   selector.SelectedAlly.types.Exists(t => t.typeName == typeName))
            .ToList();
    }

    public List<AllyData> GetAllyDataByType(string typeName)
    {
        return GetAllyData()
            .Where(data => data.types.Exists(t => t.typeName == typeName))
            .ToList();
    }

    public bool IsAllyActive(GameObject ally)
    {
        return activeAllyGameObjects.Contains(ally);
    }

    public void ClearAllAllies()
    {
        activeAllyGameObjects.Clear();
        backupAllyGameObjects.Clear();
        activeAllyData.Clear();
        backupAllyData.Clear();
    }

    // Properties to access active ally
    public GameObject ActiveAllyGameObject => activeAllyGameObjects.Count > 0 ? activeAllyGameObjects[0] : null;
    public AllySelector ActiveAllySelector => ActiveAllyGameObject?.GetComponent<AllySelector>();
    public AllyData ActiveAllyData => ActiveAllySelector?.SelectedAlly;

    // Properties to access backup allies
    public List<GameObject> BackupAllyGameObjects => backupAllyGameObjects;
    public List<AllyData> BackupAllyData => backupAllyData;

    // Helper methods for active ally stats
    public float GetActiveAllyStat(string statName)
    {
        if (ActiveAllyData == null) return 0f;
        var stat = ActiveAllyData.stats.Find(s => s.statDefinition.statName == statName);
        return stat?.value ?? 0f;
    }

    public float GetActiveAllySpeed()
    {
        return GetActiveAllyStat("Speed");
    }
} 

