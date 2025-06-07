using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAlly", menuName = "Allies/Ally")]
public class AllyData : ScriptableObject
{
    public string allyName;

    public string allyDescription;
    public List<AllyStat> stats = new List<AllyStat>();
    public List<MoveData> moves = new List<MoveData>();
    public List<TypeDefinition> types = new List<TypeDefinition>();

    public List<AllyResource> resources = new List<AllyResource>();
}

[System.Serializable]
public class AllyStat
{
    public StatDefinition statDefinition;
    public int value;
}

[System.Serializable]
public class AllyResource
{
    public ResourceDefinition resourceDefinition;
    public int value;

    public AllyResource(ResourceDefinition definition, int initialValue)
    {
        resourceDefinition = definition;
        value = initialValue;
    }
}
