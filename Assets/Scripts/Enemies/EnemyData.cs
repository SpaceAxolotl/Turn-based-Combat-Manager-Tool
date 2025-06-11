using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int currentHealth;

    public string enemyDescription;
    public List<EnemyStat> stats = new List<EnemyStat>();
    public List<MoveData> moves = new List<MoveData>();
    public List<TypeDefinition> types = new List<TypeDefinition>();

    public List<EnemyResource> resources = new List<EnemyResource>();
    public Sprite sprite;

    public int GetStatValue(string statName)
    {
        var stat = stats.Find(s => s.statDefinition.statName == statName);
        return stat?.value ?? 0;
    }
    public void InitializeCurrentHealthFromMax()
{
    currentHealth = stats.Find(s => s.statDefinition.statName == "maxHealth")?.value ?? 0;
}
}

[System.Serializable]
public class EnemyStat
{
    public StatDefinition statDefinition;
    public int value;
}

[System.Serializable]
public class EnemyResource
{
    public ResourceDefinition resourceDefinition;
    public int value;

    public EnemyResource(ResourceDefinition definition, int initialValue)
    {
        resourceDefinition = definition;
        value = initialValue;
    }
}