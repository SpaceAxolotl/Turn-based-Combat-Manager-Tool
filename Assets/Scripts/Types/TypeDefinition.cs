using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewType", menuName = "Types/Type Definition")]
public class TypeDefinition : ScriptableObject
{
    public string typeName;
    [TextArea]
    public string description;

    public List<TypeDefinition> offensiveStrengths;
    public List<TypeDefinition> offensiveWeaknesses;

    public List<TypeDefinition> defensiveStrengths;
    public List<TypeDefinition> defensiveWeaknesses;
}