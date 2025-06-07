using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    public string enemyDescription;
    public List<EnemyStat> stats = new List<EnemyStat>();
    public List<MoveData> moves = new List<MoveData>();
    public List<TypeDefinition> types = new List<TypeDefinition>();

    public List<EnemyResource> resources = new List<EnemyResource>();
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