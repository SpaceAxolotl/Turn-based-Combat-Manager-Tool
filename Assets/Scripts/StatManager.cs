using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class StatManager : MonoBehaviour
{
    // List of predefined StatDefinition ScriptableObjects (assigned in the Inspector)
    [SerializeField]
private StatLibrary statLibrary;

    // List of predefined Opponents (assigned in the Inspector)
    [SerializeField]
    private List<Opponent> opponents = new List<Opponent>();

    private void OnValidate()
{
    if (statLibrary == null || statLibrary.statDefinitions == null)
        return;

    foreach (var opponent in opponents)
    {
        if (opponent.stats == null)
            opponent.stats = new List<Stat>();

        foreach (var def in statLibrary.statDefinitions)
        {
            // Only add stat if it's not already present
            if (!opponent.stats.Exists(s => s.name == def.statName))
            {
                opponent.stats.Add(new Stat(def.statName, def.defaultValue));
            }
        }
    }
}



    // Optionally, you can modify or add more logic to update stats
    public void SetOpponentStat(string opponentName, string statName, int value)
    {
        Opponent opponent = opponents.Find(o => o.name == opponentName);
        if (opponent != null)
        {
            opponent.SetStatValue(statName, value);
            Debug.Log($"{opponentName}'s {statName} set to {value}");
        }
        else
        {
            Debug.LogError($"Opponent {opponentName} not found.");
        }
    }

    // Optionally, get the stat value of an opponent
    public int GetOpponentStat(string opponentName, string statName)
    {
        Opponent opponent = opponents.Find(o => o.name == opponentName);
        if (opponent != null)
        {
            return opponent.GetStatValue(statName);
        }
        else
        {
            Debug.LogError($"Opponent {opponentName} not found.");
            return 0;
        }
    }
}