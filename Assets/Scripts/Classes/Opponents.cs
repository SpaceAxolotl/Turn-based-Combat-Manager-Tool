using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Opponent
{
    public string name;  // Name of the opponent
    
    public List<Stat> stats;  // List of stats for this opponent

    // This constructor is no longer needed as we're handling initialization in StatManager
    // Constructor removed

    // Method to initialize stats based on the StatDefinitions passed in from StatManager
    public void InitializeStats(StatLibrary statLibrary)
    {
         stats = new List<Stat>();

    foreach (var statDef in statLibrary.statDefinitions)
    {
        stats.Add(new Stat(statDef.statName, statDef.defaultValue));
    }
    }

    // Set stat value by name
    public void SetStatValue(string statName, int value)
    {
        Stat stat = stats.Find(s => s.name == statName);
        if (stat != null)
        {
            stat.value = value;
        }
        else
        {
            Debug.LogError($"Stat {statName} not found in {name}'s stats.");
        }
    }

    // Get stat value by name
    public int GetStatValue(string statName)
    {
        Stat stat = stats.Find(s => s.name == statName);
        return stat != null ? stat.value : 0;
    }
}