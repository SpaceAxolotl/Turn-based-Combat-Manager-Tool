using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypeLibrary", menuName = "Types/Type Library")]
public class TypeLibrary : ScriptableObject
{
    public List<TypeDefinition> allTypes;
}