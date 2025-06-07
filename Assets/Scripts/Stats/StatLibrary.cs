using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatLibrary", menuName = "Stats/Stat Library")]
public class StatLibrary : ScriptableObject
{
    public List<StatDefinition> statDefinitions;
}